using imbSCI.Core.data;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.table;
using imbSCI.Core.files;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.reporting;
using imbSCI.Data;
using imbSCI.DataComplex.converters;
using imbSCI.DataComplex.extensions.data.operations;
using imbSCI.DataComplex.tables;
using System;
using System.Collections.Generic;
using System.Data;

namespace imbSCI.DataComplex.data
{
    public static class ClassificationReportDataComplexExtensions
    {

        



        public static void MakeOverviewReports(this classificationReportCollection reportCollection, classificationReportCollectionSettings settings, aceAuthorNotation appInfo, ILogBuilder log, String name = "", String description = "")
        {
            classificationReportDataComplexContext context = new classificationReportDataComplexContext();

            DataTableConverterASCII dataTableConverterASCII = new DataTableConverterASCII();

            classificationReportStyleDefinition style = classificationReportStyleDefinition.GetDefault(settings.style.groups);
            if (reportCollection.name.isNullOrEmpty())
            {
                reportCollection.name = name;
            }
            if (description.isNullOrEmpty()) reportCollection.description += description;

            MakeReport(reportCollection, settings, appInfo, log, style, context);  //reportSpace.ConstructTable("cumulative_" + reportCollection.name, reportCollection.description, style);

            foreach (var pair in reportCollection.Children)
            {
                MakeReport(pair.Value, settings, appInfo, log, style, context);

            }
            foreach (var pair in context.comparative_tables)
            {
                var comparative_table = pair.Value.GetRowCollectionTable("complete_summary_" + pair.Key); //context.comparative_tables.GetRowCollectionTable("complete_comparative");
                comparative_table.SetTitle("complete_summary_" + pair.Key);


                comparative_table.GetReportAndSave(reportCollection.rootFolder, appInfo);



            }

            foreach (var pair in context.comparative_narrow_tables)
            {
                var comparative_table = pair.Value.GetRowCollectionTable("complete_summary_ascii" + pair.Key); //context.comparative_tables.GetRowCollectionTable("complete_comparative");
                comparative_table.SetTitle("complete_summary_ascii" + pair.Key);

                dataTableConverterASCII.ConvertToFile(comparative_table, reportCollection.rootFolder, comparative_table.TableName + "_ascii.txt");

                //builderForText bt = new builderForText();
                //bt.AppendTable(comparative_table);

                //bt.GetContent().saveStringToFile(reportCollection.rootFolder.pathFor(comparative_table.TableName + "_ascii.txt", Data.enums.getWritableFileMode.overwrite, "ASCII export of comparative table"));


                comparative_table.GetReportAndSave(reportCollection.rootFolder, appInfo);



            }




            var overview_table = context.overview_tables.GetRowCollectionTable("complete_overview");
            overview_table.SetTitle("complete_overview");
            overview_table.GetReportAndSave(reportCollection.rootFolder, appInfo);


            //.GetAggregatedTable("summary", Core.math.aggregation.dataPointAggregationAspect.subSetOfRows, log);
        }

        public static void MakeReport(classificationReportCollection reportCollection, classificationReportCollectionSettings settings, aceAuthorNotation appInfo, ILogBuilder log, classificationReportStyleDefinition style, classificationReportDataComplexContext context)
        {

            DataTableTypeExtended<classificationReportExpanded> table = reportCollection.MakeOverviewTable(context, reportCollection.name, reportCollection.description);
            table.SetTitle(reportCollection.name);

            var statDataTable = table.GetReportAndSave(reportCollection.rootFolder, appInfo);
            log.log("Report [" + table.TableName + "] created at " + statDataTable.lastFilePath);

            //  context.cumulative_tables.Add(table);

            var layers = reportCollection.GetSpaceLayers(style);

            foreach (var pair in layers)
            {
                var reportSpace = classificationReportSpace.BuildReportSpace(pair.Value, reportCollection.datasetName, settings.SELECT_REPORT_NAME_PARTS, style, pair.Key);

                if (!context.report_spaces.ContainsKey(reportSpace.name))
                {
                    context.report_spaces.Add(reportSpace.name, new List<classificationReportSpace>());
                    context.comparative_tables.Add(reportSpace.name, new List<DataTable>());
                    context.comparative_narrow_tables.Add(reportSpace.name, new List<DataTable>());
                }
                context.report_spaces[reportSpace.name].Add(reportSpace);


                System.Data.DataTable comparative_table = reportSpace.ConstructTable("comparative_" + reportCollection.name + "_" + reportSpace.name, reportCollection.description);

                context.comparative_tables[reportSpace.name].Add(comparative_table);

                comparative_table.AddExtra("Group path: " + reportCollection.rootFolder.path);

                comparative_table.GetReportAndSave(reportCollection.rootFolder, appInfo);


                System.Data.DataTable comparative_table_small = reportSpace.ConstructTable("comparative_" + reportCollection.name + "_" + reportSpace.name + "_small", reportCollection.description, classificationReportTableMode.onlyBasic);

                context.comparative_narrow_tables[reportSpace.name].Add(comparative_table_small);


                var styleFS = style.CloneViaXML();
                styleFS.valueToUse = classificationReportStyleDefinition.GetFS(); //new reportExpandedDataPair(classificationReportStyleDefinition.VALUE_FS, "Selected Features", "Number of features actually selected");

                reportSpace = classificationReportSpace.BuildReportSpace(pair.Value, reportCollection.datasetName, settings.SELECT_REPORT_NAME_PARTS, styleFS, pair.Key);
                reportSpace.ConstructTable("featureSelected_" + reportCollection.name + "_" + reportSpace.name, reportCollection.description).GetReportAndSave(reportCollection.rootFolder, appInfo);

            }




            //    return comparative_table;




        }






        public static DataTableTypeExtended<classificationReportExpanded> MakeOverviewTable(this classificationReportCollection reportCollection, classificationReportDataComplexContext context, String name = "", String description = "")
        {

            DataTableTypeExtended<classificationReportExpanded> table = new DataTableTypeExtended<classificationReportExpanded>("overview_" + reportCollection.name, "Aggregate report for experiments ran [" + DateTime.Now.ToShortDateString() + "]");
            if (name != "") table.SetAdditionalInfoEntry("Group name", name);
            if (description != "") table.SetAdditionalInfoEntry("Group desc", description);



            table.SetDescription(reportCollection.description);
            table.SetAdditionalInfoEntry("Path", reportCollection.rootPath, "Path of the collection");

            table.SetAdditionalInfoEntry("Subcollections", reportCollection.Children.Count, "Number of sub collections");
            foreach (var rep in reportCollection)
            {
                table.AddRow(rep);
            }

            context.overview_tables.Add(table);

            return table;
            //table.GetReportAndSave(folder, imbACE.Core.appManager.AppInfo);



        }

    }
}