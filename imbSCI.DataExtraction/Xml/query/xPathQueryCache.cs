namespace imbSCI.DataExtraction.Xml.query
{
    //using imbACE.Core.core.exceptions;
    using imbSCI.Data.data;

    #region imbVeles using

    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.XPath;

    #endregion imbVeles using

    /// <summary>
    /// 2014c>Kesirani podaci xPath Queryija
    /// </summary>
    public class xPathQueryCache : imbBindable, IxPathQueryCache
    {
        #region KONSTRUKTORI

        /// <summary>
        /// Pravi xPath za selektovanje svih kljuceva
        /// </summary>
        /// <param name="__source"></param>
        /// <param name="__keysForXpath"></param>
        /// <param name="count"></param>
        /// <param name="extendVariations"></param>
        public xPathQueryCache(IXPathNavigable __source, ICollection<string> __keysForXpath, Boolean __count = false,
                               Boolean extendVariations = false)
        {
            count = __count;

            //            basic("", __source);
            //throw new NotImplementedException("xPathQueryCache");

            keys = __keysForXpath.extendKeys(extendVariations, extendVariations, true);
            this.namespaceSetup(__source);
            //var _navigator = __source.CreateNavigator();
            //nsPrefix = _navigator.Prefix;
            //xNm = new XmlNamespaceManager(_navigator.NameTable);

            xPath = keys.makeXPathForAllNodes(count, nsPrefix, false, false);
            basic(xPath, __source);
            // recompile(__source);
        }

        /// <summary>
        /// Pravi proizvoljan query
        /// </summary>
        /// <param name="__xPath"></param>
        /// <param name="__source"></param>
        public xPathQueryCache(String __xPath, IXPathNavigable __source)
        {
            basic(__xPath, __source);
        }

        /// <summary>
        /// samo snima upit
        /// </summary>
        /// <param name="__xPath"></param>
        public xPathQueryCache(String __xPath)
        {
            if (!String.IsNullOrEmpty(__xPath)) xPath = __xPath;
        }

        public xPathQueryCache()
        {
        }

        #endregion KONSTRUKTORI

        private String _nsPrefix = "h";

        // private XPathNavigator _navigator;
        private XPathExpression _xExp;

        public Boolean count;
        public Boolean isSimpleValue;

        public ICollection<String> keys;
        public Type returnType;
        private XmlNamespaceManager xNM;

        public String XPath
        {
            get { return _xPath; }
            set
            {
                _xPath = value;
                OnPropertyChanged("XPath");
            }
        }

        #region IxPathQueryCache Members

        public XmlNamespaceManager xNm
        {
            get { return xNM; }
            set { xNM = value; }
        }

        public XPathExpression xExp
        {
            get { return _xExp; }
            set { _xExp = value; }
        }

        public string nsPrefix
        {
            get { return _nsPrefix; }
            set { _nsPrefix = value; }
        }

        public void purgeCachedQueries()
        {
            throw new NotImplementedException();
        }

        public void purgeReportValues()
        {
            throw new NotImplementedException();
        }

        #endregion IxPathQueryCache Members

        #region --- xPath ------- komanda XPath upita

        private String _xPath = "";

        /// <summary>
        /// komanda XPath upita
        /// </summary>
        public String xPath
        {
            get { return _xPath; }
            set
            {
                _xPath = value;
                OnPropertyChanged("XPath");
            }
        }

        #endregion --- xPath ------- komanda XPath upita

        public Int32 queryXPathCount(IXPathNavigable navSource)
        {
            checkExp(navSource);
            XPathNavigator xNav = navSource.CreateNavigator();
            XPathNodeIterator xIterator = xNav.Select(_xExp);
            Int32 primCount = xIterator.Count;

            /*
            var alt = keys.makeXPathForAllNodes(false, "", false, false);

            XPathNodeIterator xIterator2 = xNav.Select(alt);
            Int32 Count2 = xIterator2.Count;

            XPathNodeIterator xIterator3 = xNav.Select(xPath, xNm);
            Int32 Count3 = xIterator3.Count;
            */
            //XPathNodeIterator xIterator4 = xNav.Select()

            return xIterator.Count;
        }

        public List<IXPathNavigable> queryXPathToList(IXPathNavigable navSource)
        {
            checkExp(navSource);
            XPathNavigator xNav = navSource.CreateNavigator();
            XPathNodeIterator xIterator = xNav.Select(_xExp);
            List<IXPathNavigable> listOutput = new List<IXPathNavigable>();
            while (xIterator.MoveNext())
            {
                IXPathNavigable tmp = xIterator.Current.Clone() as IXPathNavigable;
                listOutput.Add(tmp);
            }
            return listOutput;
        }

        public Object queryXPathSimple(IXPathNavigable navSource)
        {
            checkExp(navSource);
            XPathNavigator _navigator = navSource.CreateNavigator();
            return _navigator.Evaluate(_xExp);
        }

        private void checkExp(IXPathNavigable __source)
        {
            if (_xExp == null)
            {
                if (String.IsNullOrEmpty(xPath) || xNm == null)
                {
                    recompile(__source);
                }
                else
                {
                    _xExp = XPathExpression.Compile(xPath, xNm);
                }
            }
        }

        /// <summary>
        /// Osnovno izvrsavanje kverija
        /// </summary>
        /// <param name="__xPath"></param>
        /// <param name="__source"></param>
        private void basic(String __xPath, IXPathNavigable __source)
        {
            var _navigator = __source.CreateNavigator();

            //  this.namespaceSetup(__source);

            if (String.IsNullOrEmpty(_nsPrefix) || xNm == null)
            {
                _nsPrefix = _navigator.Prefix;
                xNm = new XmlNamespaceManager(_navigator.NameTable);
            }

            if (!String.IsNullOrEmpty(__xPath)) xPath = __xPath;

            if (!String.IsNullOrEmpty(xPath))
            {
                _xExp = XPathExpression.Compile(xPath, xNM);
                switch (_xExp.ReturnType)
                {
                    case XPathResultType.Error:
                        throw new ArgumentException("Error in XPath query", nameof(__xPath));

                        break;

                    case XPathResultType.NodeSet:
                        isSimpleValue = false;
                        break;

                    case XPathResultType.String:
                    case XPathResultType.Boolean:
                    case XPathResultType.Number:
                        isSimpleValue = true;
                        break;

                    case XPathResultType.Any:
                    default:
                        isSimpleValue = true;
                        break;
                }
            }
        }

        protected void recompile(IXPathNavigable __source)
        {
            if (__source != null)
            {
                this.namespaceSetup(__source);

                basic(xPath, __source);
            }
            xPath = keys.makeXPathForAllNodes(count, _nsPrefix, false, false);
        }
    }
}