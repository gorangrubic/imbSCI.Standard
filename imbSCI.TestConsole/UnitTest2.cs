using imbSCI.DataComplex.data;
using imbSCI.DataComplex.extensions.data.formats;
using imbSCI.DataComplex.tables;
using imbSCI.BibTex;

using System;
using System.Data;
using System.IO;

namespace imbSCI.TestConsole
{

    public class UnitTest2 : TestMicroEnvironmentBase
    {


        public void UnitTestMethod()
        {


            // Example 1: Loading BibTex file
            BibTexDataFile bib_1 = new BibTexDataFile("Resources\\test\\S0306457309000259.bib");



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


            // Example 3: Short way

            // High-level method, creates extended version of Excel file, with additional spreadsheet for Legend and other meta information
            var reportDataTable = BibTexTools.ExportToExcel(path, notation, log);


            // Example 4: Working with BibTexEntryModel

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
                url = "http://www.sciencedirect.com/science/article/pii/S0306457309000259"
            };

            // Creation of data table collection
            DataTableTypeExtended<BibTexEntryModel> bibTable = new DataTableTypeExtended<BibTexEntryModel>("RuntimeCreatedBibTex", "BibTex table, created in Run Time");

            // creates extended version of Excel file, with additional spreadsheet for Legend and other meta information
            var codeDataTable_ref = bibTable.GetReportAndSave(folderResults, notation);



        }
    }
}
