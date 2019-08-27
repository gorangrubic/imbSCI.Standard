namespace imbSCI.DataExtraction.Xml.query
{
    using imbSCI.Core.attributes;
    using imbSCI.Data.data;

    #region imbVeles using

    using System;
    using System.ComponentModel;
    using System.Xml;

    #endregion imbVeles using

    /// <summary>
    /// 2014c> Collection item: imbXPathQuery, part of imbXPathQueryCollection
    /// </summary>
    [imb(imbAttributeName.collectionPrimaryKey, "xPath")]
    /// <summary>
    /// 2013a> Jedan upit u AdvancedXPath sistemu ---- sadrzi pitanje i odgovor, nije to aktivni upit vec samo beleska
    /// </summary>
    public class imbXPathQuery : imbBindable
    {
        /// <summary>
        /// Instanciranje novog querija -- za dat xpath
        /// </summary>
        /// <param name="__xPath"></param>
        /// <param name="__input"></param>
        public imbXPathQuery(String __xPath = "", XmlNode __input = null)
        {
            _xPath = __xPath;

            xmlNode = __input;

            if (xmlNode == null)
            {
                report = "Failed : No data...";
            }
            else
            {
                report = "Data loaded : child " + xmlNode.ChildNodes.Count;
            }
        }

        #region -----------  xPath  -------  [komanda koja je pozivana]

        private String _xPath; // = new String();

        /// <summary>
        /// komanda koja je pozivana
        /// </summary>
        // [XmlIgnore]
        [Category("imbXPathQuery")]
        [DisplayName("xPath")]
        [Description("komanda koja je pozivana")]
        public String xPath
        {
            get { return _xPath; }
            set
            {
                _xPath = value;
                OnPropertyChanged("xPath");
            }
        }

        #endregion -----------  xPath  -------  [komanda koja je pozivana]

        #region -----------  report  -------  [Report line]

        private String _report; // = new String();

        /// <summary>
        /// Report line
        /// </summary>
        // [XmlIgnore]
        [Category("imbXPathQuery")]
        [DisplayName("report")]
        [Description("Report line")]
        public String report
        {
            get { return _report; }
            set
            {
                _report = value;
                OnPropertyChanged("report");
            }
        }

        #endregion -----------  report  -------  [Report line]

        #region -----------  xmlNode[]  -------  [XmlNode koji je dosao kao odgovor]

        private XmlNode _xmlNode; // = new XmlNode();

        /// <summary>
        /// XmlNode koji je dosao kao odgovor
        /// </summary>
        // [XmlIgnore]
        [Category("imbXPathQuery")]
        [DisplayName("xmlNode")]
        [Description("XmlNode koji je dosao kao odgovor")]
        public XmlNode xmlNode
        {
            get { return _xmlNode; }
            set
            {
                _xmlNode = value;
                OnPropertyChanged("xmlNode");
            }
        }

        #endregion -----------  xmlNode[]  -------  [XmlNode koji je dosao kao odgovor]
    }
}