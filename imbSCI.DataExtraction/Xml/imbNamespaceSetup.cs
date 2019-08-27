namespace imbSCI.DataExtraction.Xml
{
    using imbSCI.Data.data;

    #region imbVeles using

    using System;
    using System.ComponentModel;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Xml.XPath;

    #endregion imbVeles using

    public class imbNamespaceSetup : imbBindable
    {
        public imbNamespaceSetup()
        {
        }

        /*
        public imbNamespaceSetup(String filePath)
        {
            XmlTextReader reader = new XmlTextReader(filePath)
            XmlNamespaceManager nsmanager = new XmlNamespaceManager(reader.NameTable);
            nsmanager.AddNamespace("msbooks", "www.microsoft.com/books");
            nsmanager.PushScope();
            nsmanager.AddNamespace("msstore", "www.microsoft.com/store");

            while (reader.Read())
            {
                Console.WriteLine("Reader Prefix:{0}", reader.Prefix);
                Console.WriteLine("XmlNamespaceManager Prefix:{0}",
                nsmanager.LookupPrefix(nsmanager.NameTable.Get(reader.NamespaceURI)));
            }
        }
       */

        public imbNamespaceSetup(IXPathNavigable loadedXml, String defNamespaceName = "h")
        {
            XmlNode source = loadedXml as XmlNode;
            XmlDocument xdoc = null;
            if (source is XmlDocument)
            {
                xdoc = source as XmlDocument;
            }
            else
            {
                while (source.ParentNode != null && !(source.ParentNode is XmlDocument))
                {
                    source = source.ParentNode;
                    if (source == source.ParentNode)
                    {
                        break;
                    }
                }
                xdoc = source.OwnerDocument;
            }
            if (xdoc != null)
            {
                deploy(xdoc, defNamespaceName);
            }
        }

        public imbNamespaceSetup(XmlDocument loadedXml, String defNamespaceName = "h")
        {
            deploy(loadedXml, defNamespaceName);
        }

        #region -----------  namespaceManager  -------  [XmlNamespace]

        private XmlNamespaceManager _namespaceManager; // = new XmlNamespaceManager();

        /// <summary>
        /// XmlNamespace
        /// </summary>
        [XmlIgnore]
        // [JsonIgnore]
        [Category("imbNamespaceSetup")]
        [DisplayName("namespaceManager")]
        [Description("XmlNamespace")]
        public XmlNamespaceManager namespaceManager
        {
            get { return _namespaceManager; }
            set
            {
                _namespaceManager = value;
                OnPropertyChanged("namespaceManager");
            }
        }

        #endregion -----------  namespaceManager  -------  [XmlNamespace]

        #region -----------  nsPrefix  -------  [Prefix za namespace]

        private String _nsPrefix = "h"; // = new String();

        /// <summary>
        /// Prefix za namespace
        /// </summary>
        // [XmlIgnore]
        [Category("imbNamespaceSetup")]
        [DisplayName("nsPrefix")]
        [Description("Prefix za namespace")]
        public String nsPrefix
        {
            get { return _nsPrefix; }
            set
            {
                _nsPrefix = value;
                OnPropertyChanged("nsPrefix");
            }
        }

        #endregion -----------  nsPrefix  -------  [Prefix za namespace]

        #region -----------  nsSourceUrl  -------  [Putanja za dati namespace]

        private String _nsSourceUrl = ""; // = new String();

        /// <summary>
        /// Putanja za dati namespace
        /// </summary>
        // [XmlIgnore]
        [Category("imbNamespaceSetup")]
        [DisplayName("nsSourceUrl")]
        [Description("Putanja za dati namespace")]
        public String nsSourceUrl
        {
            get { return _nsSourceUrl; }
            set
            {
                _nsSourceUrl = value;
                OnPropertyChanged("nsSourceUrl");
            }
        }

        #endregion -----------  nsSourceUrl  -------  [Putanja za dati namespace]

        private void deploy(XmlDocument loadedXml, String defNamespaceName = "h")
        {
            if (loadedXml == null) return;
            String nsp = loadedXml.GetNamespaceOfPrefix("");
            // String pns = loadedXml.DocumentElement.GetAttribute("xmlns");

            String nms = loadedXml.DocumentElement.NamespaceURI;

            //XmlNamespaceManager mng = new XmlNamespaceManager(loadedXml.NameTable);
            nsPrefix = nsp;

            if (String.IsNullOrEmpty(nsPrefix)) nsPrefix = defNamespaceName;
            nsSourceUrl = nms;
            namespaceManager = new XmlNamespaceManager(loadedXml.NameTable);
            namespaceManager.AddNamespace(nsPrefix, nsSourceUrl);
        }
    }
}