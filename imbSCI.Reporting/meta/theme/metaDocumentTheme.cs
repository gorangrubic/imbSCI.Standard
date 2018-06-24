// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaDocumentTheme.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.meta.theme
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Core.reporting.template;
    using imbSCI.Data;
    using imbSCI.Data.data;
    using imbSCI.Reporting.resources;
    using System.ComponentModel;
    using System.Data;

    /// <summary>
    /// Theme contain all aluxuary settings for HTML and other complex content creation
    /// </summary>
    public class metaDocumentTheme : imbBindable
    {
        /// <summary>
        /// Creates new theme and does template loading
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stil"></param>
        public metaDocumentTheme()
        {
            //name = name;
            //deployStil(stil);
            //loadTemplates();
        }

        /// <summary>
        /// automatic execution during construction of object
        /// </summary>
        /// <param name="stil"></param>
        internal void deployStil(styleTheme stil)
        {
            basicStyle = stil;

            //palleteA = aceColorPaletteManager.getPalette("A", stil.colors[0]);
            //palleteB = aceColorPaletteManager.getPalette("B", stil.colors[1]);
            //palleteC = aceColorPaletteManager.getPalette("C", stil.colors[2]);

            //var headingFontName = stil.fontForHeadings;
            //var pageFontName = stil.fontForText;

            //if (pageFontName == aceFont.none) pageFontName = aceFont.Tahoma;

            //font = new styleTextFont(pageFontName);
            //if (headingFontName == aceFont.none)
            //{
            //    headingFontName = pageFontName;
            //}
            //fontHeading = new styleTextFont(headingFontName);
        }

        /// <summary>
        ///
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// automatic execution during construction of object
        /// </summary>
        internal void loadTemplates()
        {
            metaDocumentTheme theme = this;
            string tFolder = name;

            var tmpSource = reportingCoreManager.loadReportResourceFile(reportResourceFolders.reportTheme, theme.name.add("html", "."), tFolder);
            theme.htmlTemplate = new stringTemplate(tmpSource);

            theme.cssTemplate = new stringTemplate(reportResourceFolders.reportTheme.loadReportResourceFile(theme.name.add("css", "."), tFolder));
            theme.cssXmlTemplate = new stringTemplate(reportResourceFolders.reportTheme.loadReportResourceFile(theme.name.add("xml.css", "_"), tFolder));
            theme.cssColorTemplate = new stringTemplate(reportResourceFolders.reportTheme.loadReportResourceFile(theme.name.add("color.css", "_"), tFolder));

            theme.jsTemplate = new stringTemplate(reportResourceFolders.reportTheme.loadReportResourceFile(theme.name.add("js", "."), tFolder));
            theme.xmlTemplate = new stringTemplate(reportResourceFolders.reportTheme.loadReportResourceFile(theme.name.add("xml", "."), tFolder));
        }

        internal void compileStyle()
        {
            //data[templateFieldStyleInserts.style_colora_css] = cssColorTemplate.applyToContent(palleteA.AppendDataFields(null));
            //data[templateFieldStyleInserts.style_colorb_css] = cssColorTemplate.applyToContent(palleteB.AppendDataFields(null));
            //data[templateFieldStyleInserts.style_colorc_css] = cssColorTemplate.applyToContent(palleteC.AppendDataFields(null));

            basicStyle.AppendDataFields(data);

            cssCompiledCode = cssTemplate.applyToContent(data);

            //data[templateFieldStyleInserts.style_body] =
        }

        #region -----------  cssCompiledCode  -------  [Resulting CSS file that was built using cssColorTemplate and cssTempalte]

        private string _cssCompiledCode = ""; // = new String();

        /// <summary>
        /// Resulting CSS file that was built using cssColorTemplate and cssTempalte
        /// </summary>
        // [XmlIgnore]
        [Category("metaDocumentTheme")]
        [DisplayName("cssCompiledCode")]
        [Description("Resulting CSS file that was built using cssColorTemplate and cssTempalte")]
        public string cssCompiledCode
        {
            get
            {
                return _cssCompiledCode;
            }
            set
            {
                // Boolean chg = (_cssCompiledCode != value);
                _cssCompiledCode = value;
                OnPropertyChanged("cssCompiledCode");
                // if (chg) {}
            }
        }

        #endregion -----------  cssCompiledCode  -------  [Resulting CSS file that was built using cssColorTemplate and cssTempalte]

        #region -----------  data  -------  [Data collected from theme]

        private PropertyCollection _data = new PropertyCollection();

        /// <summary>
        /// Data collected from theme
        /// </summary>
        // [XmlIgnore]
        [Category("metaDocumentTheme")]
        [DisplayName("data")]
        [Description("Data collected from theme")]
        internal PropertyCollection data
        {
            get
            {
                return _data;
            }
            set
            {
                // Boolean chg = (_data != value);
                _data = value;
                OnPropertyChanged("data");
                // if (chg) {}
            }
        }

        #endregion -----------  data  -------  [Data collected from theme]

        #region -----------  basicStyle  -------  [style object instance]

        private styleTheme _basicStyle = new styleTheme(); // = new Style();

        /// <summary>
        /// style object instance
        /// </summary>
        // [XmlIgnore]
        [Category("metaDocumentTheme")]
        [DisplayName("basicStyle")]
        [Description("style object instance")]
        public styleTheme basicStyle
        {
            get
            {
                return _basicStyle;
            }
            set
            {
                // Boolean chg = (_basicStyle != value);
                _basicStyle = value;
                OnPropertyChanged("basicStyle");
                // if (chg) {}
            }
        }

        #endregion -----------  basicStyle  -------  [style object instance]

        #region --- templates

        #region -----------  xmlTemplate  -------  [Template for XML document]

        private stringTemplate _xmlTemplate; // = new stringTemplate();

        /// <summary>
        /// Template for XML document
        /// </summary>
        // [XmlIgnore]
        [Category("metaDocumentTheme")]
        [DisplayName("xmlTemplate")]
        [Description("Template for XML document")]
        public stringTemplate xmlTemplate
        {
            get
            {
                return _xmlTemplate;
            }
            set
            {
                // Boolean chg = (_xmlTemplate != value);
                _xmlTemplate = value;
                OnPropertyChanged("xmlTemplate");
                // if (chg) {}
            }
        }

        #endregion -----------  xmlTemplate  -------  [Template for XML document]

        #region -----------  cssColorTemplate  -------  [CSS template for one color pallete]

        private stringTemplate _cssColorTemplate; // = new stringTemplate();

        /// <summary>
        /// CSS template for one color pallete
        /// </summary>
        // [XmlIgnore]
        [Category("metaDocumentTheme")]
        [DisplayName("cssColorTemplate")]
        [Description("CSS template for one color pallete")]
        public stringTemplate cssColorTemplate
        {
            get
            {
                return _cssColorTemplate;
            }
            set
            {
                // Boolean chg = (_cssColorTemplate != value);
                _cssColorTemplate = value;
                OnPropertyChanged("cssColorTemplate");
                // if (chg) {}
            }
        }

        #endregion -----------  cssColorTemplate  -------  [CSS template for one color pallete]

        #region -----------  cssXmlTemplate  -------  [template for XML css]

        private stringTemplate _cssXmlTemplate; // = new stringTemplate();

        /// <summary>
        /// template for XML css
        /// </summary>
        // [XmlIgnore]
        [Category("metaDocumentTheme")]
        [DisplayName("cssXmlTemplate")]
        [Description("template for XML css")]
        public stringTemplate cssXmlTemplate
        {
            get
            {
                return _cssXmlTemplate;
            }
            set
            {
                // Boolean chg = (_cssXmlTemplate != value);
                _cssXmlTemplate = value;
                OnPropertyChanged("cssXmlTemplate");
                // if (chg) {}
            }
        }

        #endregion -----------  cssXmlTemplate  -------  [template for XML css]

        #region -----------  cssTemplate  -------  [String template of CSS file]

        private stringTemplate _cssTemplate;

        /// <summary>
        /// String template of CSS file
        /// </summary>
        // [XmlIgnore]
        [Category("metaDocumentTheme")]
        [DisplayName("cssTemplate")]
        [Description("String template of CSS file")]
        public stringTemplate cssTemplate
        {
            get
            {
                return _cssTemplate;
            }
            set
            {
                // Boolean chg = (_cssTemplate != value);
                _cssTemplate = value;
                OnPropertyChanged("cssTemplate");
                // if (chg) {}
            }
        }

        #endregion -----------  cssTemplate  -------  [String template of CSS file]

        #region -----------  jsTemplate  -------  [Java Script template]

        private stringTemplate _jsTemplate;

        /// <summary>
        /// Java Script template
        /// </summary>
        // [XmlIgnore]
        [Category("metaDocumentTheme")]
        [DisplayName("jsTemplate")]
        [Description("Java Script template")]
        public stringTemplate jsTemplate
        {
            get
            {
                return _jsTemplate;
            }
            set
            {
                // Boolean chg = (_jsTemplate != value);
                _jsTemplate = value;
                OnPropertyChanged("jsTemplate");
                // if (chg) {}
            }
        }

        #endregion -----------  jsTemplate  -------  [Java Script template]

        #region -----------  htmlTemplate  -------  [html template]

        private stringTemplate _htmlTemplate; // = new stringTemplate();

        /// <summary>
        /// html template
        /// </summary>
        // [XmlIgnore]
        [Category("metaDocumentTheme")]
        [DisplayName("htmlTemplate")]
        [Description("html template")]
        public stringTemplate htmlTemplate
        {
            get
            {
                return _htmlTemplate;
            }
            set
            {
                // Boolean chg = (_htmlTemplate != value);
                _htmlTemplate = value;
                OnPropertyChanged("htmlTemplate");
                // if (chg) {}
            }
        }

        #endregion -----------  htmlTemplate  -------  [html template]

        #endregion --- templates

        #region -----------  plugins  -------  [what plugins to include]

        private metaThemeJSPluginEnum _plugins; // = new metaThemeJSPluginEnum();

        /// <summary>
        /// what plugins to include
        /// </summary>
        // [XmlIgnore]
        [Category("metaDocumentTheme")]
        [DisplayName("plugins")]
        [Description("what plugins to include")]
        public metaThemeJSPluginEnum plugins
        {
            get
            {
                return _plugins;
            }
            set
            {
                // Boolean chg = (_plugins != value);
                _plugins = value;
                OnPropertyChanged("plugins");
                // if (chg) {}
            }
        }

        #endregion -----------  plugins  -------  [what plugins to include]

        #region -----------  options  -------  [metaDocumentThemeOptions]

        private metaDocumentThemeOptions _options; // = new metaDocumentThemeOptions();

        /// <summary>
        /// metaDocumentThemeOptions
        /// </summary>
        // [XmlIgnore]
        [Category("metaDocumentTheme")]
        [DisplayName("options")]
        [Description("metaDocumentThemeOptions")]
        public metaDocumentThemeOptions options
        {
            get
            {
                return _options;
            }
            set
            {
                // Boolean chg = (_options != value);
                _options = value;
                OnPropertyChanged("options");
                // if (chg) {}
            }
        }

        #endregion -----------  options  -------  [metaDocumentThemeOptions]

        #region -----------  include  -------  [what packages to include into output folder]

        private metaDocumentIncludeEnum _include; // = new metaDocumentIncludeEnum();

        /// <summary>
        /// what packages to include into output folder
        /// </summary>
        // [XmlIgnore]
        [Category("metaDocumentTheme")]
        [DisplayName("include")]
        [Description("what packages to include into output folder")]
        public metaDocumentIncludeEnum include
        {
            get
            {
                return _include;
            }
            set
            {
                // Boolean chg = (_include != value);
                _include = value;
                OnPropertyChanged("include");
                // if (chg) {}
            }
        }

        #endregion -----------  include  -------  [what packages to include into output folder]
    }
}