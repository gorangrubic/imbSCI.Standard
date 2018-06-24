//namespace imbReportingCore.meta
//{
//    using System;
//    using System.ComponentModel;
//    using aceCommonTypes.primitives;
//    using aceCommonTypes.diagnostic;
//    using aceCommonTypes.extensions;
//    using aceCommonTypes.reporting;
//    using imbReportingCore.meta.basic;
//    using imbReportingCore.meta.core;
//    using imbReportingCore.reporting.core;
//    using imbReportingCore.reporting.documents;

//    /// <summary>
//    /// (prevazidjeno) Complete META container structure -- describing document: header, footer, keywords and extra notation
//    /// </summary>
//    public class metaBundle:metaContentBase
//    {
//        public String getCreationSignature(String prefix = "Created:")
//        {
//            String output = prefix.add(timeStamp.GenerateTimeStamp(imbTimeStampFormat.imbDatabase), " ");
//            return output;
//        }

//        //public metaBundle(metaBundle source, metaFooter footerOverride = null, metaHeader headerOverride = null, metaKeywords keywordsOverride = null)
//        //{
//        //}

//        public metaBundle()
//        {
//        }

//        /// <summary>
//        /// Constructor with main data initialization: name, description and keywords. Other is defined by default values
//        /// </summary>
//        /// <param name="title"></param>
//        /// <param name="description"></param>
//        /// <param name="words"></param>
//        public metaBundle(String title, String description, params String[] words)
//        {
//            header.name = title;
//            header.description = description;
//            keywords.content.AddRange(words);
//        }

//        /// <summary>
//        /// Create clone with main sub objects as references to original. Override properties will replace inherited references if not Null.
//        /// </summary>
//        /// <param name="meta"></param>
//        /// <param name="footerOverride"></param>
//        /// <param name="headerOverride"></param>
//        /// <param name="keywordsOverride"></param>
//        /// <returns></returns>
//        public metaBundle makeReferenceCopy(metaFooter footerOverride = null, metaHeader headerOverride = null, metaKeywords keywordsOverride = null)
//        {
//            metaBundle meta = this;
//            metaBundle output = new metaBundle();
//            output.doInsertCaptions = meta.doInsertCaptions;
//            output.keywords = meta.keywords;
//            output.header = meta.header;
//            output.footer = meta.footer;
//            output.notation = meta.notation;

//            if (footerOverride != null) output.footer = footerOverride;
//            if (headerOverride != null) output.header = headerOverride;
//            if (keywordsOverride != null) output.keywords = keywordsOverride;

//            return output;
//        }

//        #region ----------- Boolean [ doInsertCaptions ] -------  [If captions of DataColumns/Properties/item display names should be exported]
//        private Boolean _doInsertCaptions = true;
//        /// <summary>
//        /// If captions of DataColumns/Properties/item display names should be exported
//        /// </summary>
//        [Category("Switches")]
//        [DisplayName("doInsertCaptions")]
//        [Description("If captions of DataColumns/Properties/item display names should be exported")]
//        public Boolean doInsertCaptions
//        {
//            get { return _doInsertCaptions; }
//            set { _doInsertCaptions = value; OnPropertyChanged("doInsertCaptions"); }
//        }
//        #endregion

//        #region -----------  keywords  -------  [Keywords for the document]
//        private metaKeywords _keywords = new metaKeywords(); // = new metaKeywords();
//                                    /// <summary>
//                                    /// Keywords for the document
//                                    /// </summary>
//        // [XmlIgnore]
//        [Category("metaBundle")]
//        [DisplayName("keywords")]
//        [Description("Keywords for the document")]
//        public metaKeywords keywords
//        {
//            get
//            {
//                return _keywords;
//            }
//            set
//            {
//                // Boolean chg = (_keywords != value);
//                _keywords = value;
//                OnPropertyChanged("keywords");
//                // if (chg) {}
//            }
//        }
//        #endregion

//        #region -----------  notation  -------  [Extra notation data]
//        private metaNotation _notation = new metaNotation(); // = new metaNotation();
//                                    /// <summary>
//                                    /// Extra notation data
//                                    /// </summary>
//        // [XmlIgnore]
//        [Category("metaBundle")]
//        [DisplayName("notation")]
//        [Description("Extra notation data")]
//        public metaNotation notation
//        {
//            get
//            {
//                return _notation;
//            }
//            set
//            {
//                // Boolean chg = (_notation != value);
//                _notation = value;
//                OnPropertyChanged("notation");
//                // if (chg) {}
//            }
//        }
//        #endregion

//        #region -----------  footer  -------  [Footer meta container]
//        private metaFooter _footer = new metaFooter(); // = new metaFooter();
//        /// <summary>
//        /// Footer meta container
//        /// </summary>
//        // [XmlIgnore]
//        [Category("metaBundle")]
//        [DisplayName("footer")]
//        [Description("Footer meta container")]
//        public metaFooter footer
//        {
//            get
//            {
//                return _footer;
//            }
//            set
//            {
//                // Boolean chg = (_footer != value);
//                _footer = value;
//                OnPropertyChanged("footer");
//                // if (chg) {}
//            }
//        }
//        #endregion

//        #region -----------  header  -------  [Header meta container]
//        private metaHeader _header = new metaHeader(); // = new metaHeader();
//        /// <summary>
//        /// Header meta container
//        /// </summary>
//        // [XmlIgnore]
//        [Category("metaBundle")]
//        [DisplayName("header")]
//        [Description("Header meta container")]
//        public metaHeader header
//        {
//            get
//            {
//                return _header;
//            }
//            set
//            {
//                // Boolean chg = (_header != value);
//                _header = value;
//                OnPropertyChanged("header");
//                // if (chg) {}
//            }
//        }

//        public reportHtmlDocument makeHtml(reportHtmlDocument htmlReport = null)
//        {
//            return null;
//        }

//        public string makeTextTemplate(imbStringBuilder sb = null)
//        {
//            if (sb == null)
//            {
//                sb = new imbStringBuilder();
//            }
//            header.makeTextTemplate(sb);

//            sb.nextTabLevel();

//            sb.AppendLine("---");
//            content.ForEach(x => sb.AppendLine(x));
//            sb.AppendLine("---");

//            sb.AppendLine();
//            sb.prevTabLevel();

//            notation.makeTextTemplate(sb);
//            footer.makeTextTemplate(sb);
//            keywords.makeTextTemplate(sb);

//            return sb.ToString();
//        }

//        public string makeText(imbStringBuilder sb = null)
//        {
//            if (sb == null)
//            {
//                sb = new imbStringBuilder();
//            }

//            header.makeText(sb);

//            sb.nextTabLevel();

//            sb.AppendLine("- Content --");
//            content.ForEach(x => sb.AppendLine(x));
//            sb.AppendLine("- Content --");

//            sb.AppendLine();
//            sb.prevTabLevel();

//            notation.makeText(sb);
//            footer.makeText(sb);
//            keywords.makeText(sb);

//            return sb.ToString();
//        }

//        public reportXmlDocument makeXml(reportXmlDocument xmlReport = null)
//        {
//            return null;
//        }

//        #endregion

//    }
//}