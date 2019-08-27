namespace imbSCI.DataExtraction.Xml
{
    #region imbVeles using

    using imbSCI.DataExtraction.Xml.enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;

    #endregion imbVeles using

    /// <summary>
    /// Posebne alatke za debug i izvršavanje XPath tool-a
    /// </summary>
    public static class imbXmlXPathTools
    {
        private static XmlNode _defaultSource;

        public static String defaultNamespacePrefix = "h";

        #region imbObject Property <XmlNode> defaultSource

        /// <summary>
        /// Podrazumevani source za xPath query - kada je prosledjeni source null
        /// </summary>
        public static XmlNode defaultSource
        {
            get { return _defaultSource; }
            set { _defaultSource = value; }
        }

        #endregion imbObject Property <XmlNode> defaultSource

        public static void namespaceSetup(this IxPathQueryCache query, IXPathNavigable navSource)
        {
            if (query.xNm != null)
            {
                if (query.xNm.HasNamespace("h")) return;
            }

            XPathNavigator _navigator = navSource.CreateNavigator();
            query.xNm = new XmlNamespaceManager(_navigator.NameTable);

            query.nsPrefix = "h";
            String nsUrl = "";
            var rs = _navigator.SelectDescendants(XPathNodeType.Namespace, true);
            while (rs.MoveNext())
            {
                nsUrl = rs.Current.Value;
            }
            query.xNm.AddNamespace(query.nsPrefix, nsUrl);
        }

        /// <summary>
        /// Pravi XPath koji ce selektovati sve nodove sa spiska
        /// </summary>
        /// <param name="keys">Nazivi nodova (tagova) koje treba selektovati</param>
        /// <param name="count">Samo da prebroji nodove</param>
        /// <returns></returns>
        public static String makeXPathForAllNodes(this ICollection<string> keys, Boolean count = false,
                                                  String nsPrefix = null, Boolean lowLetters = false,
                                                  Boolean capLetters = false, String axes = "")
        {
            String output = "";

            keys = keys.extendKeys(lowLetters, capLetters, true);

            if (keys.Count > 0)
            {
                output = "";
                foreach (String key in keys)
                {
                    String insert = key;
                    if (!String.IsNullOrEmpty(axes))
                    {
                        insert = axes + "::" + key;
                    }

                    if (String.IsNullOrEmpty(nsPrefix))
                    {
                        output += insert + " ";
                    }
                    else
                    {
                        output += "//" + nsPrefix + ":" + insert;
                        //output += "//" + nsPrefix + ":" + key + "[@*]";
                    }

                    if (key != keys.Last())
                    {
                        output += " | ";
                    }
                }
            }
            //if (count) output = "fn:count(" + output + ")";
            return output;
        }

        public static XElement GetXElement(this XmlNode node)
        {
            XDocument xDoc = new XDocument();
            using (XmlWriter xmlWriter = xDoc.CreateWriter())
                node.WriteTo(xmlWriter);
            return xDoc.Root;
        }

        public static XmlNode GetXmlNode(this XElement element)
        {
            using (XmlReader xmlReader = element.CreateReader())
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                return xmlDoc;
            }
        }

        /// <summary>
        /// Pravi DEBUG poruku u skladu sa podešavanjima
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="sourceXPath"></param>
        /// <param name="finalPath"></param>
        /// <param name="result"></param>
        /// <param name="resCount"></param>
        public static void makeDebugMessage(String xPath, String sourceXPath, String finalPath, String result,
                                            Int32 resCount)
        {
            //switch (imbSettingsManager.current.xpathDebugMode)
            //{
            //    case xpathDebugMode.disabled:
            //        return;
            //    case xpathDebugMode.onNullResult:
            //        if (resCount > 0) return;
            //        break;
            //    case xpathDebugMode.fullDebug:
            //        break;
            //}

            String msg = "xPath query: " + xPath + Environment.NewLine;
            msg += "Source xPath: " + sourceXPath + Environment.NewLine;
            msg += "Executed xPath: " + finalPath + Environment.NewLine;
            msg += "Source + Executed: " + sourceXPath + finalPath + Environment.NewLine;
            msg += "Result: " + result + Environment.NewLine;
            msg += "Nodex returned: " + resCount + Environment.NewLine;

            //if (imbSettingsManager.current.xpathOpenMsgBox)
            //{
            //    MessageBox.Show(msg, "XPath Execution debug message", MessageBoxButton.OK, MessageBoxImage.Information);
            //}
            throw new ArgumentException(msg, nameof(xPath));
            //logSystem.log(msg, logType.ExecutionError);
        }

        //public static XmlNode queryXPathQuick();

        /// <summary>
        /// Izvršava xPath query i konvertuje rezultat u listu - OVO JE ISPRAVNI MEHANIZAM
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="nsManager"></param>
        /// <param name="source"></param>
        /// <param name="trimPathWithSource">SA TRUE RADI DOBRO</param>
        /// <returns></returns>
        public static List<XmlNode> queryXPath(String xPath, XmlNamespaceManager nsManager, XmlNode source,
                                               Boolean trimPathWithSource = true) //List<XmlNode>
        {
            List<XmlNode> output = new List<XmlNode>();

            Object response = queryXPathObj(xPath, nsManager, source, trimPathWithSource);
            if (response == null)
            {
                return output;
            }
            if (response.GetType() == typeof(string))
            {
                return output;
            }
            else
            {
                return response as List<XmlNode>;
            }
        }

        /// <summary>
        /// 2014> izvrsava upit nad bilo kojim IXPathNavigable objektom
        /// </summary>
        /// <param name="source"></param>
        /// <param name="xPath"></param>
        /// <param name="nsManager"></param>
        /// <returns></returns>
        public static List<XmlNode> queryXPath(this IXPathNavigable source, String xPath,
                                               XmlNamespaceManager nsManager = null)
        {
            XPathNavigator xNav = source.CreateNavigator();
            if (nsManager == null) nsManager = new XmlNamespaceManager(xNav.NameTable);
            List<XmlNode> output = new List<XmlNode>();
            XPathExpression xExp = XPathExpression.Compile(xPath, nsManager);
            XPathNodeIterator xIterator = xNav.Select(xExp);
            while (xIterator.MoveNext())
            {
                XmlNode tmp = xIterator.Current.UnderlyingObject as XmlNode;

                output.Add(tmp);
            }
            return output;
        }

        public static Object queryXPathObj(this IXPathNavigable source, String xPath,
                                           XmlNamespaceManager nsManager = null)
        {
            XPathNavigator xNav = source.CreateNavigator();
            if (nsManager == null) nsManager = new XmlNamespaceManager(xNav.NameTable);
            List<XmlNode> output = new List<XmlNode>();
            XPathExpression xExp = XPathExpression.Compile(xPath, nsManager);
            XPathNodeIterator xIterator = xNav.Select(xExp);
            return xIterator.Current.Value as Object;
        }

        //public static Object queryXPathObj(this webDocument source, String xPath) //List<XmlNode>
        //{
        //    XPathExpression xExp = XPathExpression.Compile(xPath, source.nsManager);
        //    XPathNavigator xNav = source.getDocumentNavigator();

        //    return xNav.Select(xExp);
        //}

        /// <summary>
        /// Vraca podatak a ne node
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="nsManager"></param>
        /// <param name="source"></param>
        /// <param name="trimPathWithSource"></param>
        /// <param name="valueSource"></param>
        /// <returns></returns>
        public static String queryXPathStr(String xPath, XmlNamespaceManager nsManager, XmlNode source,
                                           Boolean trimPathWithSource = true,
                                           imbNodeValueSource valueSource = imbNodeValueSource.allInnerText)
        //List<XmlNode>
        {
            List<XmlNode> output = new List<XmlNode>();

            Object response = queryXPathObj(xPath, nsManager, source, trimPathWithSource);

            if (response.GetType() == typeof(string))
            {
                return response.ToString();
            }
            else
            {
                String str = "";
                output = response as List<XmlNode>;
                foreach (XmlNode nd in output)
                {
                    str += imbXmlCommonTools.getNodeDataSourceValue(nd, valueSource);
                }
                return str;
            }
        }

        /// <summary>
        /// Glavna XPath Funkcija
        /// </summary>
        /// <param name="xPath">XPathInstrukcija</param>
        /// <param name="nsManager">NameSpace manager, ako ostane Null koristiće default</param>
        /// <param name="source">XmlNode od kojeg kreće izvršavanje</param>
        /// <param name="trimPathWithSource">Da li da od xPath instrukcije oduzme source putanju? Ostaviti Ne ako je već lokalizovano</param>
        /// <returns>XmlNode ili neki drugi objekat - u zavisnosti od upita</returns>
        public static Object queryXPathObj(String xPath, XmlNamespaceManager nsManager, XmlNode source,
                                           Boolean trimPathWithSource = true) //List<XmlNode>
        {
            if (source == null)
            {
                makeDebugMessage(xPath, "[SOURCE IS NULL]", "", "", 0);
                return source;
            }

            String inputXPath = xPath;

            List<XmlNode> output = new List<XmlNode>();
            String sourcePath = "";

            if (trimPathWithSource)
            {
                sourcePath = imbXmlCommonTools.FindXPath(source, nsManager) + "/";
                if (nsManager != null)
                {
                    if (!String.IsNullOrEmpty(sourcePath))
                    {
                        if (xPath.IndexOf(sourcePath) == 0)
                        {
                            xPath = xPath.Replace(sourcePath, "");
                        }
                    }
                }
            }

            XPathExpression xExp = XPathExpression.Compile(xPath, nsManager);
            String debugRes = "";
            switch (xExp.ReturnType)
            {
                case XPathResultType.NodeSet:
                    //case XPathResultType.Navigator:
                    XmlNodeList result = source.SelectNodes(xPath, nsManager);
                    Int32 c = 0;
                    foreach (XmlNode it in result)
                    {
                        if (c < 20) debugRes += it.NodeType.ToString() + ", ";
                        else if (c == 20) debugRes += "... ";
                        c++;
                        output.Add(it);
                    }

                    makeDebugMessage(inputXPath, sourcePath, xPath, debugRes, c);

                    return output;

                case XPathResultType.Boolean:
                case XPathResultType.Number:
                case XPathResultType.String:
                case XPathResultType.Any:
                    XElement xSource = source.GetXElement(); //.ToXElement();

                    return xSource.XPathEvaluate(xPath, nsManager).ToString();
            }

            return output;
        }

        #region diagnostic

        public static String xPathDiag(String xPath, Boolean useXNode = false, XmlNamespaceManager mng = null,
                                       XmlNode source = null)
        {
            String output = Environment.NewLine + "path[ " + xPath + " ]" + Environment.NewLine;

            List<XmlNode> tmpList = new List<XmlNode>();
            //if (useXNode)
            //    {
            //        tmpList.AddRange(queryXPathViaXNode(xPath));
            //    }
            //    else
            //    {
            tmpList.AddRange(queryXPath(xPath, mng, source));
            //}

            output += tmpList.Count.ToString() + Environment.NewLine;
            foreach (XmlNode t in tmpList)
            {
                //String nm = t.GetPrefixOfNamespace(t.NamespaceURI);
                //if (!String.IsNullOrEmpty(nm)) {
                //    output += nm + ":";
                //}
                output += imbXmlCommonTools.getNodeDataSourceValue(t, "tagname") + Environment.NewLine;
            }
            //logSystem.log(output, logType.Notification);
            return output + Environment.NewLine + Environment.NewLine;
        }

        public static String xPathDiag(List<string> xPaths, Boolean useXNode, XmlNamespaceManager mng = null,
                                       XmlNode source = null)
        {
            String output = "";
            foreach (String xp in xPaths)
            {
                output += xPathDiag(xp, useXNode, mng, source);
            }
            return output;
        }

        /// <summary>
        /// Izvrsava xPath uz dijagnostiku
        /// </summary>
        /// <param name="xPaths"></param>
        /// <param name="mng"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static String xPathDiagSelect(List<string> xPaths, XmlNamespaceManager mng, XmlNode source = null)
        {
            String output = "";
            foreach (String xp in xPaths)
            {
                try
                {
                    XmlNode nd = source.SelectSingleNode(xp, mng);
                    output += "xPath : " + xp + Environment.NewLine;
                    if (nd != null)
                    {
                        output += "Node: " + nd.Name + " // " + nd.getXPathName() + " // " + nd.ChildNodes.Count +
                                  Environment.NewLine + Environment.NewLine;
                    }
                    else
                    {
                        output += "Failed: No data returned " + Environment.NewLine + Environment.NewLine;
                    }
                }
                catch (Exception ex)
                {
                    output += "Failed: " + ex.Message + Environment.NewLine + Environment.NewLine;
                }
                //output += xPathDiag(xp, useXNode, mng, source);
            }
            return output;
        }

        #endregion diagnostic
    }
}