using imbSCI.Core.extensions.text;
using imbSCI.Core.style.preset;
using imbSCI.Data.enums.fields;
using imbSCI.DataComplex.data;
using imbSCI.DataComplex.extensions.data.formats;
using imbSCI.DataComplex.tables;
using System;
using System.Data;
using System.Drawing;
using System.IO;

namespace imbSCI.BibTex
{
    /// <summary>
    /// Class with example - test methods
    /// </summary>
    /// <see cref="Example1_Basic"/>
    /// <seealso cref="imbSCI.DataComplex.data.TestMicroEnvironmentBase" />
    public class BibTexExamples : TestMicroEnvironmentBase
    {
        /// <summary>Method demonstrating basic operation of loading BibTex file</summary>
        /** <example><para>Method demonstrating basic operation of loading BibTex file</para>
         *  <code> // Example 1: Loading BibTex file
            BibTexDataFile bib_1 = new BibTexDataFile("Resources\\test\\S0306457309000259.bib");
            // Converting BibTex data into object model dictionary
            System.Collections.Generic.Dictionary{string, BibTexEntryModel} model = bib_1.ConvertToModel(log);
            // Printing [Author : Title] to a ILogBuilder log builder
            foreach (var pair in model)
            {
                log.log(pair.Value.author.or("Unknown") + ": " + pair.Value.title);
            }
            </code></example>
        */
        public void Example1_Basic()
        {
            // Example 1: Loading BibTex file
            BibTexDataFile bib_1 = new BibTexDataFile("Resources\\test\\S0306457309000259.bib");

            // Converting BibTex data into object model dictionary
            BibTexCollection<BibTexEntryModel> model = bib_1.ConvertToModel<BibTexEntryModel>(log);

            // Printing [Author : Title] to a ILogBuilder log builder
            foreach (var pair in model)
            {
                log.log(pair.author.or("Unknown") + ": " + pair.title);
            }
        }

        /// <summary>Load BibTex file, convert data into DataTable, create Excel file without Legend information and generate full Excel report</summary>
        /** <example><para>Load BibTex file, convert data into DataTable, create Excel file without Legend information and generate full Excel report</para>
         *  <code>
             // Example 2: Loading BibTex file
            String path = folderResources.findFile("S0306457309000259.bib", SearchOption.AllDirectories);

            // initializes bibtex data file object
            BibTexDataFile bib = new BibTexDataFile();

            // loads .bib or .bibtex file from path specified
            bib.Load(path, log);

            // converts loaded BibTex entries into DataTable, with all columns discovered in the entries
            DataTable dt = bib.ConvertToDataTable();

            // saves DataTable to Excel file, without adding Legend spreadsheet
            var finalPath = dt.serializeDataTable(Data.enums.reporting.dataTableExportEnum.excel, bib.name, folderResults, notation);

            // creates extended version of Excel file, with additional spreadsheet for Legend and other meta information
            var reportDataTable_ref = dt.GetReportAndSave(folderResults, notation);</code></example>
        */

        public void Example2_LoadAndExport()
        {
            // Example 2: Loading BibTex file
            String path = folderResources.findFile("S0306457309000259.bib", SearchOption.AllDirectories);

            // initializes bibtex data file object
            BibTexDataFile bib = new BibTexDataFile();

            // loads .bib or .bibtex file from path specified
            bib.Load(path, log);

            // converts loaded BibTex entries into DataTable, with all columns discovered in the entries
            DataTable dt = bib.ConvertToDataTable();

            // saves DataTable to Excel file, without adding Legend spreadsheet
            var finalPath = dt.serializeDataTable(Data.enums.reporting.dataTableExportEnum.excel, bib.name, folderResults, notation);

            // creates extended version of Excel file, with additional spreadsheet for Legend and other meta information
            var reportDataTable_ref = dt.GetReportAndSave(folderResults, notation);
        }

        /// <summary>
        /// Short way for exporting BibTex files ...
        /// </summary>
        /** <example><para>Short way for exporting BibTex files ...</para>
         *  <code>
            // Example 3: Short way - using imbSCI find file
            String path = folderResources.findFile("S0306457309000259.bib", SearchOption.AllDirectories);

            // High-level method, creates extended version of Excel file, with additional spreadsheet for Legend and other meta information
            var reportDataTable = BibTexTools.ExportToExcel(path, notation, log);

            // Now, let's load all *.bib files
            System.Collections.Generic.List<string> paths = folderResources.findFiles("*.bib", SearchOption.AllDirectories);

            // Exports all *.bib files
            System.Collections.Generic.List<DataTableForStatistics> exports = BibTexTools.ExportToExcel(paths, notation, log, null, folderResults);

            // Printing filename and destination path, to a ILogBuilder log builder
            foreach (var export in exports)
            {
                log.log(export.TableName + " exported to: " + export.lastFilePath);
            }</code></example>
        */

    public void Example3_LoadAndExportToExcel_ShortWay()
    {
        // Example 3: Short way - using imbSCI find file
        String path = folderResources.findFile("S0306457309000259.bib", SearchOption.AllDirectories);

        // High-level method, creates extended version of Excel file, with additional spreadsheet for Legend and other meta information
        var reportDataTable = BibTexTools.ExportToExcel(path, notation, log);

        // Now, let's load all *.bib files
        System.Collections.Generic.List<string> paths = folderResources.findFiles("*.bib", SearchOption.AllDirectories);

        // Exports all *.bib files
        System.Collections.Generic.List<DataTableForStatistics> exports = BibTexTools.ExportToExcel(paths, notation, log, null, folderResults);

        // Printing filename and destination path, to a ILogBuilder log builder
        foreach (var export in exports)
        {
            log.log(export.TableName + " exported to: " + export.lastFilePath);
        }
    }

        /// <summary>Creation of BibTex entry from code, and generation of Excel table</summary>
        /** <example><para>Creation of BibTex entry from code, and generation of Excel table</para>
         *  <code>
            // --- We create the entry and write its source code to the disk

            // Creation of BibTex entry from code
            BibTexEntryModel entry = new BibTexEntryModel()
            {
                EntryKey = "SOKOLOVA2009427",
                EntryType = "article",
                journal = "Information Processing & Management",
                title = "A systematic analysis of performance measures for classification tasks",
                keywords = "Performance evaluation, Machine Learning, Text classification",
                year = 2005,
                number = 2,
                issn = "0000-0000",
                @abstract = "Abs",
                doi = "https://doi.org/10.1016/j.ipm.2009.03.002",
                url = "http://www.sciencedirect.com/science/article/pii/S0306457309000259",
                author = "Marina Sokolova and Guy Lapalme"
            };

            // New instance of TextProcessor object, this one you would share with other parts of your code.
            BibTexSourceProcessor processor = new BibTexSourceProcessor();

            // Generating BibTex code
            String code = entry.GetSource(processor.latex, log);

            // Making path
            String path = folderResults.pathFor(nameof(Example4_UsingObjectModel) + ".txt");

            File.WriteAllText(path, code);

            // --- Now we export it to the Excel file

            // Creation of data table collection
            DataTableTypeExtended<BibTexEntryModel> bibTable = new DataTableTypeExtended<BibTexEntryModel>("RuntimeCreatedBibTex", "BibTex table, created in Run Time");
            bibTable.AddRow(entry);

            // creates extended version of Excel file, with additional spreadsheet for Legend and other meta information
            var codeDataTable_ref = bibTable.GetReportAndSave(folderResults, notation, nameof(Example4_UsingObjectModel));
        */
        public void Example4_UsingObjectModel()
        {
            // --- We create the entry and write its source code to the disk

            // Creation of BibTex entry from code
            BibTexEntryModel entry = new BibTexEntryModel()
            {
                EntryKey = "SOKOLOVA2009427",
                EntryType = "article",
                journal = "Information Processing & Management",
                title = "A systematic analysis of performance measures for classification tasks",
                keywords = "Performance evaluation, Machine Learning, Text classification",
                year = 2005,
                number = 2,
                issn = "0000-0000",
                @abstract = "Abs",
                doi = "https://doi.org/10.1016/j.ipm.2009.03.002",
                url = "http://www.sciencedirect.com/science/article/pii/S0306457309000259",
                author = "Marina Sokolova and Guy Lapalme"
            };

            // New instance of TextProcessor object, this one you would share with other parts of your code.
            BibTexSourceProcessor processor = new BibTexSourceProcessor();

            // Generating BibTex code
            String code = entry.GetSource(processor.latex, log);

            // Making path
            String path = folderResults.pathFor(nameof(Example4_UsingObjectModel) + ".txt");

            File.WriteAllText(path, code);

            // --- Now we export it to the Excel file

            // Creation of data table collection
            DataTableTypeExtended<BibTexEntryModel> bibTable = new DataTableTypeExtended<BibTexEntryModel>("RuntimeCreatedBibTex", "BibTex table, created in Run Time");
            bibTable.AddRow(entry);

            // creates extended version of Excel file, with additional spreadsheet for Legend and other meta information
            var codeDataTable_ref = bibTable.GetReportAndSave(folderResults, notation, nameof(Example4_UsingObjectModel));
        }


        /// <summary>
        /// Example5s this instance.
        /// </summary>
        /** <example><para></para>
         * <code> var files = folderResources.findFiles("*.bib*", SearchOption.AllDirectories);
           var targetFolder = folderResults.Add("WithoutTemplate", "Without template", "Exporting Excel files without column data annotation template");
           BibTexTools.ExportToExcel(files, notation, log, null, targetFolder);
           targetFolder = folderResults.Add("WithTemplate", "With template", "Exporting Excel files with column data annotation template");
           // creating template from Type
           propertyAnnotationPreset template = new propertyAnnotationPreset(typeof(BibTexEntryModel));
           template.defaultItem.definitions.Add(templateFieldDataTable.columnWidth, 10);
           template.defaultItem.definitions.Add(templateFieldDataTable.col_color, "#FF6600");
           BibTexTools.ExportToExcel(files, notation, log, template, targetFolder); </code>
           </example>
        */
        public void Example5_SpecifyFormattingManually()
        {
            var files = folderResources.findFiles("*.bib*", SearchOption.AllDirectories);

            var targetFolder = folderResults.Add("WithoutTemplate", "Without template", "Exporting Excel files without column data annotation template");

            BibTexTools.ExportToExcel(files, notation, log, null, targetFolder);

            targetFolder = folderResults.Add("WithTemplate", "With template", "Exporting Excel files with column data annotation template");

            // creating template from Type
            propertyAnnotationPreset template = new propertyAnnotationPreset(typeof(BibTexEntryModel));
            template.defaultItem.definitions.Add(templateFieldDataTable.columnWidth, 10);
            template.defaultItem.definitions.Add(templateFieldDataTable.col_color, "#FF6600");

            template.GetAnnotationPresetItem(nameof(BibTexEntryModel.journal)).definitions.Add(templateFieldDataTable.col_color, Color.Red);

            BibTexTools.ExportToExcel(files, notation, log, template, targetFolder);
        }
    }
}