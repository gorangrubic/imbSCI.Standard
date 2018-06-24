using imbSCI.Core.data;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting;
using imbSCI.Core.style.preset;
using imbSCI.DataComplex.tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace imbSCI.BibTex
{
    /// <summary>
    /// Tools for BibTex data conversion
    /// </summary>
    public static class BibTexTools
    {
        public const Int32 FileExtensionAbortCount = 5;

        public const Boolean ExportInParallel = false;

        /// <summary>
        /// Exports all acceptable file formats, to excel tables with the same name
        /// </summary>
        /// <param name="filePaths">The file paths.</param>
        /// <param name="author">The author.</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        public static List<DataTableForStatistics> ExportToExcel(IEnumerable<String> filePaths, aceAuthorNotation author, ILogBuilder log = null, propertyAnnotationPreset customTemplate = null, folderNode outputFolder = null)
        {
            List<DataTableForStatistics> output = new List<DataTableForStatistics>();
            Int32 c = 0;

            if (ExportInParallel)
            {
                Stack<DataTableForStatistics> outputStack = new Stack<DataTableForStatistics>();

                Parallel.ForEach(filePaths, x =>
                {
                    if (CheckExtension(x))
                    {
                        var excel = ExportToExcel(x, author, log, customTemplate, outputFolder);
                        outputStack.Push(excel);

                        c = 0;
                    }
                    else
                    {
                        c++;
                    }
                });

                output.AddRange(outputStack.ToArray());
            }
            else
            {
                foreach (String f in filePaths)
                {
                    //if (c > FileExtensionAbortCount)
                    //{
                    //    if (log != null) log.log("File list aborted after [" + c.ToString() + "] incompatibile files");

                    //    break;
                    //}

                    if (CheckExtension(f))
                    {
                        output.Add(ExportToExcel(f, author, log, customTemplate, outputFolder));
                        c = 0;
                    }
                    else
                    {
                        c++;
                    }
                }
            }

            return output;
        }

        private static Object _template_lock = new object();
        private static propertyAnnotationPreset _template;

        /// <summary>
        /// Default data annotation template, based on <see cref="BibTexEntryModel"/>
        /// </summary>
        public static propertyAnnotationPreset template
        {
            get
            {
                if (_template == null)
                {
                    lock (_template_lock)
                    {
                        if (_template == null)
                        {
                            DataTableTypeExtended<BibTexEntryModel> dataTable = new DataTableTypeExtended<BibTexEntryModel>();
                            _template = new propertyAnnotationPreset(dataTable);
                            //_template = new propertyAnnotationPreset(typeof(BibTexEntryModel));
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _template;
            }
        }

        /// <summary>
        /// Loads the BibTex file and converts it to Excel
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="author">The author.</param>
        /// <param name="log">The log.</param>
        /// <param name="customTemplate">Custom data annotation template, to be used for Excel file generation. If not specified, default <see cref="template"/> is used</param>
        /// <returns>Extended DataTable that is written in Excel file</returns>
        public static DataTableForStatistics ExportToExcel(String filePath, aceAuthorNotation author, ILogBuilder log = null, propertyAnnotationPreset customTemplate = null, folderNode outputFolder = null)
        {
            BibTexDataFile bibFile = new BibTexDataFile(filePath);
            FileInfo fi = new FileInfo(filePath);

            if (outputFolder == null) outputFolder = fi.Directory;

            if (customTemplate == null) customTemplate = template;

            DataTable reportTable = bibFile.ConvertToDataTable(null, customTemplate, log); //.GetReportAndSave(fi.Directory, author, bibFile.name);

            DataTableForStatistics report = reportTable.GetReportAndSave(outputFolder, author);

            if (log != null)
            {
                log.log("BibTex [" + filePath + "] -> Excel [" + report.lastFilePath + "]");
            }

            return report;
        }

        /// <summary>
        /// Gets or sets the file extensions.
        /// </summary>
        /// <value>
        /// The file extensions.
        /// </value>
        public static List<String> fileExtensions { get; set; } = new List<string>() { ".bib", ".bibtex" };

        /// <summary>
        /// Checks if the extension is right
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static Boolean CheckExtension(String filePath)
        {
            foreach (String fe in fileExtensions)
            {
                if (filePath.EndsWith(fe))
                {
                    return true;
                }
            }

            filePath = filePath.ToLower();

            foreach (String fe in fileExtensions)
            {
                if (filePath.EndsWith(fe))
                {
                    return true;
                }
            }

            return false;
        }
    }
}