using imbSCI.Core.data;
using imbSCI.Core.files.folders;
using imbSCI.DataComplex.tables;
using imbSCI.DataExtraction.Extractors.Task;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.MetaTables
{
    public static class MetaTableExtensions
    {
        public static SourceTableDescriptionAggregation CompileSourceDescription(this IEnumerable<SourceTableDescription> sourceDescriptions)
        {
            SourceTableDescriptionAggregation aggregatedDescriptions = new SourceTableDescriptionAggregation();  //task.score.LastEntry().aggregatedDescriptions;

            // List<SourceTableDescription> sourceDescriptions = task.score.TaskRuns.Where(x => x.executionMode == ExtractionTaskEngineMode.Training).Where(x => x.IsSuccess).Select(x => x.metaTableDescription.sourceDescription).ToList();
            aggregatedDescriptions = new SourceTableDescriptionAggregation(sourceDescriptions);
            return aggregatedDescriptions;
        }

        public static MetaTableDescription CompileDescription(this TableExtractionTask task, List<SourceTableDescription> sourceDescriptions)
        {
            MetaTableDescription metaDescription = null;


            SourceTableDescriptionAggregation aggregatedDescriptions = sourceDescriptions.CompileSourceDescription(); // new SourceTableDescriptionAggregation();  //task.score.LastEntry().aggregatedDescriptions;

            // List<SourceTableDescription> sourceDescriptions = task.score.TaskRuns.Where(x => x.executionMode == ExtractionTaskEngineMode.Training).Where(x => x.IsSuccess).Select(x => x.metaTableDescription.sourceDescription).ToList();
            

            //aggregatedDescriptions.Report(report_folder, reporter);

            metaDescription = new imbSCI.DataExtraction.MetaTables.Descriptors.MetaTableDescription(new SourceTableDescription(), imbSCI.DataExtraction.MetaTables.Descriptors.MetaTableFormatType.unknown);
            metaDescription.Comment = "Created by for " + task.name;

            SourceTableSliceTestAggregation SelectedAsPropertyUID = null;
            SourceTableSliceTestAggregation SelectedAsEntryUID = null;

            if (aggregatedDescriptions.rowTestAggregation.IsPreferedAsPropertyUID)
            {
                SelectedAsPropertyUID = aggregatedDescriptions.rowTestAggregation;
            }
            else if (aggregatedDescriptions.columnTestAggregation.IsPreferedAsPropertyUID)
            {
                SelectedAsPropertyUID = aggregatedDescriptions.columnTestAggregation;
            }
            else if (aggregatedDescriptions.rowTestAggregation.IsSuitableAsUID)
            {
                SelectedAsPropertyUID = aggregatedDescriptions.rowTestAggregation;
            }
            else if (aggregatedDescriptions.columnTestAggregation.IsSuitableAsUID)
            {
                SelectedAsPropertyUID = aggregatedDescriptions.columnTestAggregation;
            }
            else if (aggregatedDescriptions.rowTestAggregation.IsAcceptableAsPropertyUID)
            {
                SelectedAsPropertyUID = aggregatedDescriptions.rowTestAggregation;
            }
            else if (aggregatedDescriptions.columnTestAggregation.IsAcceptableAsPropertyUID)
            {
                SelectedAsPropertyUID = aggregatedDescriptions.columnTestAggregation;
            }

            if (SelectedAsPropertyUID == null)
            {
                metaDescription.index_propertyID = -1;
            }
            else
            {
                if (aggregatedDescriptions.rowTestAggregation == SelectedAsPropertyUID)
                {
                    SelectedAsEntryUID = aggregatedDescriptions.columnTestAggregation;
                }
                else if (aggregatedDescriptions.columnTestAggregation == SelectedAsPropertyUID)
                {
                    SelectedAsEntryUID = aggregatedDescriptions.rowTestAggregation;
                }
            }

            if (SelectedAsEntryUID != null)
            {

                if (SelectedAsEntryUID.IsDistinctValue)
                {

                }
                else
                {
                    SelectedAsEntryUID = null;
                    metaDescription.index_entryID = -1;
                }
            }


            if (SelectedAsEntryUID != null)
            {
                metaDescription.entrySource = SelectedAsEntryUID.format;
                metaDescription.index_entryID = 0;
            }

            if (SelectedAsPropertyUID != null)
            {
                metaDescription.propertySource = SelectedAsPropertyUID.format;
                metaDescription.index_propertyID = 0;
            }


            if (metaDescription.propertySource == SourceTableSliceType.undefined)
            {
                if (aggregatedDescriptions.sourceWidth.Range <= aggregatedDescriptions.sourceHeight.Range)
                {
                    metaDescription.propertySource = SourceTableSliceType.column;
                }
                else if (aggregatedDescriptions.sourceWidth.Range > aggregatedDescriptions.sourceHeight.Range)
                {
                    metaDescription.propertySource = SourceTableSliceType.row;
                }
            }

            if (metaDescription.entrySource == SourceTableSliceType.undefined)
            {
                switch (metaDescription.propertySource)
                {
                    default:

                        break;
                    case SourceTableSliceType.column:
                        metaDescription.entrySource = SourceTableSliceType.row;
                        break;
                    case SourceTableSliceType.row:
                        metaDescription.entrySource = SourceTableSliceType.column;
                        break;
                }
            }

            switch (metaDescription.propertySource)
            {
                default:

                    break;
                case SourceTableSliceType.column:
                    metaDescription.sourceDescription.valueZone.y = 0;// metaDescription.index_propertyID + 1;
                    metaDescription.sourceDescription.valueZone.x = 0;//metaDescription.index_entryID + 1;
                                                                      // metaDescription.IsTransposed = true;
                                                                      //   metaDescription.entrySource = SourceTableSliceType.row;
                    metaDescription.format = MetaTableFormatType.vertical;
                    break;
                case SourceTableSliceType.row:
                    //  metaDescription.entrySource = SourceTableSliceType.column;
                    metaDescription.format = MetaTableFormatType.horizontal;
                    metaDescription.sourceDescription.valueZone.x = 0; // metaDescription.index_propertyID + 1;
                    metaDescription.sourceDescription.valueZone.y = 0; // metaDescription.index_entryID + 1;
                    break;
            }




            // metaDescription.Report(report_folder, reporter);

            return metaDescription;

        }

        public static void Publish(this MetaTable source, folderNode outputFolder, ExtractionResultPublishFlags flags, String filename_prefix="", aceAuthorNotation notation = null)
        {
            if (source == null) return;

            var sourcep = outputFolder.pathFor(filename_prefix + "meta.xml", imbSCI.Data.enums.getWritableFileMode.autoRenameThis, "Exported source table");

            if (flags.HasFlag(ExtractionResultPublishFlags.metaTableSerialization))
            {
                source.Save(sourcep);
            }

            String fl = Path.GetFileNameWithoutExtension(sourcep);

            if (flags.HasFlag(ExtractionResultPublishFlags.metaTableExcel))
            {
                source.GetDataTable("meta").GetReportAndSave(outputFolder, notation, fl);
            }
        }


        public static MetaTable MergeTablesByParams(MetaTablePropertyAliasList alias, params MetaTable[] tables)
        {
            return MergeTables(alias, tables);
        }

        public static MetaTable MergeTables(MetaTablePropertyAliasList alias, IEnumerable<MetaTable> tables)
        {
            var d = tables.First().description;

            MetaTable output = new MetaTable(d);
            
            foreach (MetaTable table in tables)
            {
                Dictionary<String, MetaTableProperty> matchedPropertyBySourceName = new Dictionary<string, MetaTableProperty>();

                foreach (var property in table.properties.items)
                {
                    MetaTableProperty matched_property = output.properties.Get(property.PropertyName, alias);

                    if (matched_property == null)
                    {
                        output.properties.Import(property);
                        matched_property = property;
                    }

                    matchedPropertyBySourceName.Add(property.PropertyName, matched_property);
                }

                output.entries.ExpandEntries(output.properties);

                var targetEntries = output.entries.GetEntryDictionary(d.entryIDPropertyName);
                var sourceEntries = table.entries.GetEntryDictionary(d.entryIDPropertyName);

                foreach (var pair in sourceEntries)
                {
                   // targetEntries.entries.CreateEntry()

                    MetaTableEntry entry = null;
                    if (!targetEntries.ContainsKey(pair.Key))
                    {
                        throw new NotImplementedException();
                        //entry = output.CreateEntry()
                        //output.entries.Add(entry);
                    }
                    else
                    {
                        entry = targetEntries[pair.Key];
                    }

                    foreach (var matchedPair in matchedPropertyBySourceName)
                    {
                        var sourceVal = pair.Value.properties[matchedPair.Key];

                        entry.properties[matchedPair.Value.PropertyName] = sourceVal;
                    }
                }
            }

            return output;
        }
    }
}