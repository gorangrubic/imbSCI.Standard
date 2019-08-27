namespace imbSCI.DataExtraction.Xml
{
    #region imbVeles using

    using imbSCI.Core.extensions.text;
    using imbSCI.DataExtraction.Xml.enums;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Xml;

    #endregion imbVeles using

    /// <summary>
    /// Operacije sa multi mapama
    /// </summary>
    public static class imbXmlMultiMapping
    {
        /// <summary>
        /// Izvršava MultiMap modul
        /// </summary>
        /// <param name="rootCell">Polje u koje je root za upisivanje vrednosti</param>
        /// <param name="contextForModules"></param>
        /// <param name="xPathPrefix"></param>
        /// <param name="source"></param>
        public static Dictionary<String, String> executeMultiMap(String rootPath, XmlNamespaceManager nsm,
                                                                 String xPathPrefix, XmlNode source,
                                                                 ObservableCollection<imbxPathMultiMapPair> map,
                                                                 imbNodeValueSource vsource)
        {
            Dictionary<String, String> output = new Dictionary<string, string>();
            String path = "";
            String xPath = "";
            foreach (imbxPathMultiMapPair pair in map)
            {
                path = rootPath;
                xPath = xPathPrefix;

                if (!String.IsNullOrEmpty(pair.cellPath)) path = (path + "/" + pair.cellPath).Replace("//", "/");
                if (!String.IsNullOrEmpty(pair.xPath))
                {
                    xPath = pair.xPath.Replace(xPathPrefix, "");
                }

                xPath = xPath.getPathVersion(1); // imbStringPathTools.getRelativePathVersion(xPath);
                if (xPath.Contains("#text"))
                {
                    xPath = imbStringPathTools.getPathVersion(xPath, 1);
                }

                List<XmlNode> response = imbXmlXPathTools.queryXPathObj(xPath, nsm, source, false) as List<XmlNode>;
                String value = response.getNodeValues(vsource, "");

                output.Add(path, value);
                //  output.Add(path, imbXmlXPathTools.queryXPathStr(xPath, nsm, source, true,vsource ));
            }
            return output;
        }
    }
}