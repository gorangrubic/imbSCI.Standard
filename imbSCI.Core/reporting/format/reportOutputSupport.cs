// --------------------------------------------------------------------------------------------------------------------
// <copyright file="reportOutputSupport.cs" company="imbVeles" >
//
// Copyright (C) 2018 imbVeles
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// <summary>
// Project: imbSCI.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Core.reporting.format
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Collection of supported reportOutputFormat as <c>Key</c> with default filename with extension as <c>Value</c>
    /// </summary>
    /// <seealso cref="System.Data.PropertyCollection" />
    public class reportOutputSupport : PropertyCollection
    {
        /// <summary>
        /// Gets fileinfo
        /// </summary>
        /// <param name="filenameBase">Filename base - automatically trims any existing extension</param>
        /// <param name="dir">directorium to put file into</param>
        /// <param name="format">Targeted output format</param>
        /// <returns></returns>
        public FileInfo getFileInfo(String filenameBase, DirectoryInfo dir, reportOutputFormatName format = reportOutputFormatName.none, getWritableFileMode mode = getWritableFileMode.autoRenameExistingOnOtherDate)
        {
            //String filename = getFilename(filenameBase, format);
            String filepath = getFilename(filenameBase, dir, format);

            return filepath.getWritableFile(mode); //new FileInfo(filepath);
        }

        /// <summary>
        /// Makes filename from <c>filenameBase</c> according to specified <c>format</c>.
        /// </summary>
        /// <param name="filenameBase">Filename base - automatically trims any existing extension</param>
        /// <param name="dir">directorium to put file into</param>
        /// <param name="format">Targeted output format</param>
        /// <returns>
        /// filename with proper extension
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">format</exception>
        /// <remarks>
        /// If <c>format</c> is not supported throws <see cref="ArgumentOutOfRangeException" />. For <c>none</c> returns default
        /// </remarks>
        public String getFilename(String filenameBase, DirectoryInfo dir, reportOutputFormatName format = reportOutputFormatName.none)
        {
            String filename = getFilename(filenameBase, format);
            String filepath = Path.Combine(dir.FullName, filename);

            return filepath;
        }

        /// <summary>
        /// Makes filename from <c>filenameBase</c> according to specified <c>format</c>.
        /// </summary>
        /// <remarks>If <c>format</c> is not supported throws <see cref="ArgumentOutOfRangeException"/>. For <c>none</c> returns default</remarks>
        /// <param name="filenameBase">Filename base - automatically trims any existing extension</param>
        /// <param name="format">Targeted output format</param>
        /// <returns>filename with proper extension</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">format</exception>
        public String getFilename(String filenameBase, reportOutputFormatName format = reportOutputFormatName.none)
        {
            String dir = Path.GetDirectoryName(filenameBase);

            if (format == reportOutputFormatName.none) format = defaultFormat;
            if (format == reportOutputFormatName.unknown)
            {
                format = defaultFormat;
            }

            if (!supported.Contains(format))
            {
                String exmsg = String.Format("Supplied output format is not supported: format {0}, supported formats {1}", format.ToString(), supported.toCsvInLine(";"));
                throw new ArgumentOutOfRangeException("format", this, exmsg);
            }

            filenameBase = Path.GetFileNameWithoutExtension(filenameBase);

            String output = filenameBase;
            throw new NotImplementedException("Format resolve not implemented yet");
            // output = imbSciStringExtensions.add(output, format.getDefaultExtension(), ".");
            if (!imbSciStringExtensions.isNullOrEmptyString(dir))
            {
                output = imbSciStringExtensions.add(dir, output, "\\");
            }

            return output;
        }

        //public builderSelector prepareSelector(builderSelector target)
        //{
        //}

        private reportOutputFormatName _defaultFormat = reportOutputFormatName.none;

        /// <summary>
        /// Gets the default format.
        /// </summary>
        /// <value>
        /// The default format.
        /// </value>
        internal reportOutputFormatName defaultFormat
        {
            get
            {
                if (_defaultFormat == reportOutputFormatName.none)
                {
                    if (!supported.Any())
                    {
                        _defaultFormat = reportOutputFormatName.textFile;
                    }
                    else
                    {
                        _defaultFormat = supported.First();
                    }
                }

                return _defaultFormat;
            }
            set
            {
                if (supported.Contains(value))
                {
                    _defaultFormat = value;
                }
            }
        }

        /// <summary>
        /// Provides default format
        /// </summary>
        /// <returns></returns>
        public reportOutputFormatName getDefaultFormat()
        {
            return defaultFormat;
        }

        //public static List<reportOutputFormat> FORMATS_EEPLUS = new List<reportOutputFormat>()

        /// <summary>
        /// Gets the reportOutputSupport for an API
        /// </summary>
        /// <param name="api">The API.</param>
        /// <param name="filename_base">The filename base.</param>
        /// <returns></returns>
        public static reportOutputSupport getFormatSupportFor(reportAPI __api, String filename_base)
        {
            var tmp = new reportOutputSupport(filename_base, getFormatsFor(__api).ToArray());
            tmp.api = __api;
            return tmp;
        }

        private reportAPI _api;

        /// <summary>
        ///
        /// </summary>
        public reportAPI api
        {
            get { return _api; }
            set { _api = value; }
        }

        //public static ITextRender getRenderFor(reportAPI api)
        //{
        //}

        public static ITextRender getRenderFor(reportAPI api)
        {
            ITextRender output = null;
            switch (api)
            {
                case reportAPI.imbXmlHtml:
                    output = new builderForHtml();
                    break;
                //case reportAPI.EEPlus:
                //    output = new builderForTableDocument();
                //    break;
                case reportAPI.textBuilder:
                    output = new builderForText();
                    break;

                default:
                    output = new builderForMarkdown();
                    break;
            }
            output.settings.api = api;

            return output;
        }

        /// <summary>
        /// Gets the formats for API
        /// </summary>
        /// <param name="api">The API.</param>
        /// <returns>List of supported formats</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static List<reportOutputFormatName> getFormatsFor(reportAPI api)
        {
            List<reportOutputFormatName> output = new List<reportOutputFormatName>();
            switch (api)
            {
                case reportAPI.imbReporting:
                    output.AddMultiple(
                        reportOutputFormatName.docXAML, reportOutputFormatName.docHTML,
                        reportOutputFormatName.htmlReport, reportOutputFormatName.htmlViaMD,
                        reportOutputFormatName.xml, reportOutputFormatName.rdf,
                        reportOutputFormatName.json, reportOutputFormatName.owl);
                    break;

                case reportAPI.imbFlowDocument:
                    output.AddMultiple(reportOutputFormatName.docXAML, reportOutputFormatName.docHTML, reportOutputFormatName.xml, reportOutputFormatName.Word, reportOutputFormatName.textFile, reportOutputFormatName.markdown);
                    break;

                case reportAPI.imbSerialization:
                    output.AddMultiple(reportOutputFormatName.xml, reportOutputFormatName.owl, reportOutputFormatName.rdf, reportOutputFormatName.json, reportOutputFormatName.textFile);
                    break;

                case reportAPI.imbMarkdown:
                    output.AddMultiple(reportOutputFormatName.textMdFile, reportOutputFormatName.markdown, reportOutputFormatName.htmlViaMD, reportOutputFormatName.textLog, reportOutputFormatName.textFile, reportOutputFormatName.emailHTML);
                    break;

                case reportAPI.imbXmlHtml:
                    output.AddMultiple(reportOutputFormatName.xml, reportOutputFormatName.textXml, reportOutputFormatName.textHtml, reportOutputFormatName.emailHTML);
                    break;

                case reportAPI.imbDiagnostics:
                    output.AddMultiple(reportOutputFormatName.textMdFile, reportOutputFormatName.textFile, reportOutputFormatName.textLog);
                    break;

                case reportAPI.textBuilder:
                    output.AddMultiple(reportOutputFormatName.textMdFile, reportOutputFormatName.textLog, reportOutputFormatName.textFile, reportOutputFormatName.textHtml, reportOutputFormatName.csv, reportOutputFormatName.textCss, reportOutputFormatName.rdf,
                        reportOutputFormatName.json, reportOutputFormatName.owl);
                    break;

                case reportAPI.EEPlus:
                    output.AddMultiple(reportOutputFormatName.sheetCsv, reportOutputFormatName.sheetExcel, reportOutputFormatName.sheetHtml, reportOutputFormatName.sheetPDF, reportOutputFormatName.sheetXML, reportOutputFormatName.serXml);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return output;
        }

        #region -----------  supported  -------  [Report output formats that are supported by the API]

        private List<reportOutputFormatName> _supported = new List<reportOutputFormatName>();

        /// <summary>
        /// Report output formats that are supported by the API
        /// </summary>
        // [XmlIgnore]
        [Category("reportOutputSupport")]
        [DisplayName("supported")]
        [Description("Report output formats that are supported by the API")]
        public List<reportOutputFormatName> supported
        {
            get
            {
                return _supported;
            }
            set
            {
                // Boolean chg = (_supported != value);
                _supported = value;
                // if (chg) {}
            }
        }

        #endregion -----------  supported  -------  [Report output formats that are supported by the API]

        /// <summary>
        /// Initializes a new instance of the <see cref="reportOutputSupport"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="outputs">The outputs.</param>
        public reportOutputSupport(String filename, params reportOutputFormatName[] outputs)
        {
            setSupport(filename, outputs);
        }

        /// <summary>
        /// Checks collection of outputs for compatibility with API.
        /// </summary>
        /// <param name="exceptionOnNotSupported">if set to <c>true</c> [exception on not supported].</param>
        /// <param name="outputs">The outputs.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Format [" + format.ToString() + "] not supported by reporting API.</exception>
        public List<reportOutputFormatName> checkSupport(Boolean exceptionOnNotSupported, params reportOutputFormatName[] outputs)
        {
            List<reportOutputFormatName> output = new List<reportOutputFormatName>();
            var checkList = outputs.getFlatList<reportOutputFormatName>();
            foreach (reportOutputFormatName format in checkList)
            {
                if (supported.Contains(format))
                {
                    output.Add(format);
                }
                else
                {
                    if (exceptionOnNotSupported)
                    {
                        throw new NotSupportedException("Format [" + format.ToString() + "] not supported by reporting API.");
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="reportOutputSupport"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="outputs">The outputs.</param>
        public void setSupport(String filename, params reportOutputFormatName[] outputs)
        {
            /// this.Clear();
            if (supported.Any())
            {
            }
            supported = outputs.getFlatList<reportOutputFormatName>();
            String name = Path.GetFileNameWithoutExtension(filename);
            foreach (reportOutputFormatName format in supported)
            {
                String val = imbSciStringExtensions.add(filename, format.ToString(), "_");
                // this[format] = imbSciStringExtensions.add(val, format.getFilenameExtension()); // TODO: Format for reports not implemented yet
            }
        }
    }
}