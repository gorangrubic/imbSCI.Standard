namespace imbSCI.DataExtraction.Xml
{
    #region imbVeles using

    using imbSCI.DataExtraction.Xml.enums;
    using imbSCI.DataExtraction.Xml.query;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.XPath;

    #endregion imbVeles using

    /// <summary>
    /// imbVeles napredni xPath mehanizam (2013a)
    /// </summary>
    /// <remarks>
    /// Nema upotrebu u novijem queryXPath mehanizmu (2014)
    /// </remarks>
    public static class imbAdvancedXPath
    {
        /// <summary>
        /// 2014c: glavni poziv za izvrsavanje xPath upita nad izvorom - ne vrsi lokalizaciju putanje
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="source"></param>
        /// <param name="ns"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static imbXPathQueryCollection xPathExecute(this String xPath, IXPathNavigable _source,
                                                           imbNamespaceSetup ns = null,
                                                           queryEngine engine = queryEngine.xmlNodeSelect)
        {
            XmlNode source = _source as XmlNode;
            if (ns == null) ns = new imbNamespaceSetup(source.OwnerDocument);
            var lst = imbAdvancedXPath.xPathExecution(xPath, source, ns, engine, false);
            imbXPathQueryCollection output = new imbXPathQueryCollection(lst);
            return output;
        }

        /// <summary>
        /// 2013a: Vrši xPath upit i vraća onaj element koji je definisan u Index parametru.
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="source"></param>
        /// <param name="nsSetup"></param>
        /// <param name="engine"></param>
        /// <param name="trimPathWithSource"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static XmlNode xPathExecution(String xPath, XmlNode source, imbNamespaceSetup nsSetup, queryEngine engine,
                                             Boolean trimPathWithSource, Int32 index)
        {
            List<imbXPathQuery> output = xPathExecution(xPath, source, nsSetup, engine, trimPathWithSource);

            imbXPathQuery first = output.FirstOrDefault(); //.imbGetFirstValue<imbXPathQuery>(null, false, index);

            XmlNode nd = null;
            if (first != null)
            {
                nd = first.xmlNode;
            }

            return nd;
        }

        /// <summary>
        /// GLAVNI MEHANIZAM - low-level
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="source"></param>
        /// <param name="nsSetup"></param>
        /// <param name="engine"></param>
        /// <param name="report"></param>
        /// <returns></returns>
        public static List<imbXPathQuery> xPathExecution(String xPath, XmlNode source, imbNamespaceSetup nsSetup,
                                                         queryEngine engine, Boolean trimPathWithSource)
        {
            List<imbXPathQuery> output = new List<imbXPathQuery>();
            imbXPathQuery tmp = null;
            XmlNode nd = null;
            if (source == null)
            {
                throw new ArgumentNullException(nameof(xPath));

                //logSystem.log("Source XML is empty", logType.Notification);
                return output;
            }
            if (nsSetup == null) nsSetup = new imbNamespaceSetup(source.OwnerDocument);

            try
            {
                switch (engine)
                {
                    case queryEngine.imbXPathQuery:
                        List<XmlNode> inp = imbXmlXPathTools.queryXPath(xPath, nsSetup.namespaceManager, source,
                                                                        trimPathWithSource);
                        foreach (XmlNode ndd in inp)
                        {
                            tmp = new imbXPathQuery(xPath, ndd);
                            output.Add(tmp);
                        }
                        break;

                    case queryEngine.xmlNodeSelect:
                        XmlNodeList rez = source.SelectNodes(xPath, nsSetup.namespaceManager);
                        foreach (XmlNode ndd in rez)
                        {
                            tmp = new imbXPathQuery(xPath, ndd);
                            output.Add(tmp);
                        }
                        break;

                    case queryEngine.xmlNodeSelectSingle:
                        nd = source.SelectSingleNode(xPath, nsSetup.namespaceManager);
                        tmp = new imbXPathQuery(xPath, nd);
                        output.Add(tmp);
                        break;
                }
            }
            catch (Exception ex)
            {
                tmp = new imbXPathQuery(xPath, null);
                tmp.report += Environment.NewLine + "Error: " + ex.Message;
                output.Add(tmp);
            }

            return output;
        }
    }
}