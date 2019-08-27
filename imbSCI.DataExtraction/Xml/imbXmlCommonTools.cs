#region USING

//using HtmlAgilityPack;

#endregion USING

namespace imbSCI.DataExtraction.Xml
{
    #region imbVeles using

    using imbSCI.Core.extensions.text;
    using imbSCI.DataExtraction.Xml.enums;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.XPath;

    #endregion imbVeles using

    /// <summary>
    /// Klasa za osnovne operacije nad xmlNode objektima
    /// </summary>
    public static class imbXmlCommonTools
    {
        public const String defaultNamespace = "h";

        /// <summary>
        /// Konvertuje XmlNodeList u List XmlNode
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<XmlNode> imbToList(this XmlNodeList source)
        {
            List<XmlNode> output = new List<XmlNode>();
            foreach (XmlNode it in source)
            {
                output.Add(it);
            }
            return output;
        }

        /// <summary>
        /// Vraca listu MPS redova
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pathOfParent">Path prefix</param>
        /// <param name="ignoreAttributes"></param>
        /// <param name="ignoreChild"></param>
        /// <returns></returns>
        public static String getMPSRows(this List<XmlNode> sourceList, String pathOfParent = "",
                                        Boolean ignoreAttributes = false, Boolean ignoreChild = false)
        {
            String output = "";

            foreach (XmlNode node in sourceList)
            {
                output += getMPSRow(node, pathOfParent, ignoreAttributes, ignoreChild);
            }

            return output;
        }

        /// <summary>
        /// Vraca kompletan MPS row na osnovu prosledjenog XmlNodea
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pathOfParent">Path prefix</param>
        /// <param name="ignoreAttributes"></param>
        /// <param name="ignoreChild"></param>
        /// <returns></returns>
        public static String getMPSRow(this XmlNode source, String pathOfParent = "", Boolean ignoreAttributes = false,
                                       Boolean ignoreChild = false)
        {
            List<XmlNode> sourceList = new List<XmlNode>();
            sourceList.Add(source);

            return getMPSRow(sourceList, pathOfParent, ignoreAttributes, ignoreChild);
        }

        /// <summary>
        /// Vraca kompletan MPS row na osnovu prosledjenog XmlNodea
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pathOfParent">Path prefix</param>
        /// <param name="ignoreAttributes"></param>
        /// <param name="ignoreChild"></param>
        /// <returns></returns>
        public static String getMPSRow(this List<XmlNode> sourceList, String pathOfParent = "",
                                       Boolean ignoreAttributes = false, Boolean ignoreChild = false)
        {
            String output = "{{{" + Environment.NewLine;
            String path = pathOfParent;

            foreach (XmlNode item in sourceList)
            {
                output += getMPSLine(item, pathOfParent, ignoreAttributes, ignoreChild);
            }
            output += "}}}" + Environment.NewLine;
            return output;
        }

        /// <summary>
        /// Vraca MPS line za jedan node - bez row wrappera
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pathOfParent">Path prefix</param>
        /// <param name="ignoreAttributes"></param>
        /// <param name="ignoreChild"></param>
        /// <returns></returns>
        public static String getMPSLine(this XmlNode source, String pathOfParent = "", Boolean ignoreAttributes = false,
                                        Boolean ignoreChild = false)
        {
            String output = "";
            String path = pathOfParent + "/" + source.Name;

            output += imbStringOperations.getMPSLine(path, getNodeDataSourceValue(source, "innerText"));

            if (!ignoreAttributes)
            {
                foreach (XmlAttribute ita in source.Attributes)
                {
                    output += imbStringOperations.getMPSLine(path + "/" + ita.Name, ita.Value.ToString());
                }
            }

            if (!ignoreChild)
            {
                foreach (XmlNode ch in source.ChildNodes)
                {
                    output += getMPSLine(ch, path, ignoreAttributes, ignoreChild);
                }
            }
            return output;
        }

        /// <summary>
        /// Metod vraca vrednost za node - prema @dataSource name iz #nodeValueSource# ili prema istoimenom atributu ako nema @
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static String getNodeValueUniversal(this IXPathNavigable _source, String dataSource,
                                                   Boolean leaveDataSourceMark = true)
        {
            String output = "";
            XmlNode source = _source as XmlNode;
            if (String.IsNullOrEmpty(dataSource)) return output;

            if (dataSource[0] == '@')
            {
                dataSource = dataSource.Replace("@", "");
                output = getNodeDataSourceValue(source, dataSource, leaveDataSourceMark);
            }
            else
            {
                // onda je atribut
                output = getAttributeValue(source, dataSource);
            }

            return output;
        }

        /// <summary>
        /// Vraća string sa NodeValue vrednostima za svaki XmlNode iz liste
        /// </summary>
        /// <param name="source">Izvor</param>
        /// <param name="dataSource">Koji podatak se koristi</param>
        /// <param name="nodeSufix">Opcioni dodatak na kraju svakog nodea</param>
        /// <returns>String sa svim vrednostima</returns>
        public static String getNodeValues(this IEnumerable<XmlNode> source, imbNodeValueSource dataSource,
                                           String nodeSufix = "")
        {
            String output = "";
            foreach (XmlNode ss in source)
            {
                output += getNodeDataSourceValue(ss, dataSource) + nodeSufix + Environment.NewLine;
            }
            return output;
        }

        /// <summary>
        /// 2013a: U dataSourceTemplate popunjava podatke na osnovu zadatih dataSource-a
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dataSourceTemplate">Ako ne nadje podatak ostavice datasource poziv</param>
        /// <param name="leaveDataSourceMark">Da li da ostavi data source mark u slucaju da ne postoji atribut</param>
        /// <returns></returns>
        public static String getNodeDataSourceMulti(this IXPathNavigable _source, String dataSourceTemplate,
                                                    Boolean leaveDataSourceMark)
        {
            XmlNode source = _source as XmlNode;
            String output = "";
            String[] elements = dataSourceTemplate.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (String el in elements)
            {
                output += getNodeValueUniversal(source, el) + " ";
            }
            return output;
        }

        /// <summary>
        /// 2013a: vraća reprezentativnu vrednost grane u skladu sa njenom definicijom !
        /// Ovo je low level i treba koristiti textRetriveEngine
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static String getNodeDataSourceAutomatic(this IXPathNavigable _source, Boolean newLineAtEnd = true)
        {
            XmlNode source = _source as XmlNode;
            String tag = source.Name.ToLower();
            String output = "";
            String dsTemplate = "";
            switch (tag)
            {
                case "meta":
                    output += Environment.NewLine;
                    output += getNodeDataSourceMulti(source, "@property =\"", true);
                    output += getNodeDataSourceMulti(source, "@content @http-equiv", false);
                    output += getNodeDataSourceMulti(source, "\" ", true) + Environment.NewLine;
                    output += Environment.NewLine;
                    break;

                default:
                case "p":
                case "h1":
                case "h2":
                case "h3":
                case "h4":
                case "h5":
                case "h6":
                case "title":
                case "div":
                case "span":
                    output += getNodeDataSourceMulti(source, "allinnertext", false) + Environment.NewLine;
                    break;

                case "a":
                    output += getNodeDataSourceMulti(source, "allinnertext", false) + Environment.NewLine;
                    output += getNodeDataSourceMulti(source, "@href", false) + Environment.NewLine;
                    break;

                case "script":
                    output += Environment.NewLine;
                    output += getNodeDataSourceMulti(source, "SCRIPT:@language @type @src", true) + Environment.NewLine;
                    output += getNodeDataSourceMulti(source, "allinnertext", false) + Environment.NewLine;
                    output += Environment.NewLine;
                    break;

                case "#comment":
                    break;
            }
            if (newLineAtEnd) output += Environment.NewLine + Environment.NewLine;
            return output;
        }

        /// <summary>
        /// Izvlaci String value iz xmlNode-a prema zadatom dataSource-u. Ako nije pronadjen dataSource, onda pretpostavlja da je rec o atributu
        /// </summary>
        /// <param name="source">Node iz kojeg vadi podatak</param>
        /// <param name="dataSource">Koji podatak vadi</param>
        /// <param name="leaveDataSourceMark">Da li ispisuje odakle je uzeta vrednost</param>
        /// <returns></returns>
        public static String getNodeDataSourceValue(this IXPathNavigable source, imbNodeValueSource dataSource,
                                                    Boolean leaveDataSourceMark = true)
        {
            return getNodeDataSourceValue(source, dataSource.ToString(), leaveDataSourceMark);
        }

        /// <summary>
        /// Izvlaci String value iz xmlNode-a prema zadatom dataSource-u. Ako nije pronadjen dataSource, onda pretpostavlja da je rec o atributu
        /// </summary>
        /// <param name="source">Node iz kojeg vadi podatak</param>
        /// <param name="dataSource">Koji podatak vadi</param>
        /// <param name="leaveDataSourceMark">Da li ispisuje odakle je uzeta vrednost</param>
        /// <returns></returns>
        public static String getNodeDataSourceValue(this IXPathNavigable _source, String dataSource,
                                                    Boolean leaveDataSourceMark = true)
        {
            String output = "";
            XmlNode source = _source as XmlNode;

            if (String.IsNullOrEmpty(dataSource)) return output;

            try
            {
                switch (dataSource.ToLower())
                {
                    case "haschildren":

                        output = source.HasChildNodes.ToString();
                        break;

                    case "hasrelatives":
                        if (source.ParentNode != null)
                        {
                            if (source.ParentNode.ChildNodes.Count > 1)
                            {
                                output = true.ToString();
                            }
                            else
                            {
                                output = false.ToString();
                            }
                        }
                        else
                        {
                            output = false.ToString();
                        }
                        break;

                    case "tagname":
                    case "name":
                        output = source.Name;
                        break;

                    case "innerxml":
                        output = source.InnerXml;
                        break;

                    case "innertext":
                    case "outerxml":
                        output = source.InnerText;
                        break;

                    case "nspace":
                    case "namespace":
                        output = source.GetNamespaceOfPrefix(source.NamespaceURI).ToString();
                        break;

                    case "index":
                        output = FindElementIndex((XmlElement)source).ToString();
                        break;

                    case "nodecount":
                    case "count":
                        output = source.ChildNodes.Count.ToString();
                        break;

                    case "allinnertext":
                        output = getSubnodeString(source);
                        break;

                    case "atttocsv":
                        foreach (XmlAttribute ita in source.Attributes)
                        {
                            output = output + ita.Value + ",";
                        }
                        output = imbStringOperations.trimToLimit(output, output.Length - 1, false, "");

                        break;

                    case "atttodlist":
                        foreach (XmlAttribute ita in source.Attributes)
                        {
                            output = output + ita.Value + "|" + ita.Name + ";";
                        }
                        break;

                    case "attline":
                        output = getAttributeLine(source);
                        break;

                    case "xpath":
                        output = FindXPath(source, defaultNamespace);
                        break;

                    case "json":
                        throw new NotImplementedException("JSON conversion of xml node");
                        // output = JsonConvert.SerializeXmlNode(source);
                        break;

                    case "mps":
                        output = getMPSRow(source, "", false, false);
                        break;

                    default:
                        // ako nije podrzan onda pretpostavlja da je u pitanju attribut
                        if (leaveDataSourceMark)
                        {
                            output = getAttributeValue(source, dataSource);
                        }
                        else
                        {
                            output = getAttributeValue(source, "");
                        }
                        // ako nema tog atributa vratice dataSource

                        break;
                }
            }
            catch //(Exception ex)
            {
            }

            return output;
        }

        /// <summary>
        /// Vraca vrednost attributa
        /// </summary>
        /// <param name="source"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static String getAttributeValue(this XmlNode source, String attributeName)
        {
            try
            {
                return source.CreateNavigator().getAttributeValue(attributeName);
            }
            catch //Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// Vraca vrednost attributa
        /// </summary>
        /// <param name="source"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static String getAttributeValue(this XPathNavigator source, String attributeName)
        {
            try
            {
                if (source.HasAttributes)
                {
                    var val = source.GetAttribute(attributeName, source.NamespaceURI);
                    if (val == null)
                    {
                        return "";
                    }
                    else
                    {
                        return val;
                    }
                }
                //XmlAttribute tmpAtt = source.Attributes[attributeName];
                //if (tmpAtt == null) return "";
                //return tmpAtt.Value;
            }
            catch //Exception ex)
            {
                return "";
            }
            return "";
        }

        /// <summary>
        /// Vraca liniju atributa
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string getAttributeLine(this XmlNode node)
        {
            String output = "";
            if (node != null)
            {
                if (node.Attributes != null)
                {
                    foreach (XmlAttribute att in node.Attributes)
                    {
                        output += att.Name + "=\"" + att.Value + "\" ";
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Prikuplja sav vidljiv tekst iz nodea -
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String getSubnodeString(this XmlNode input)
        {
            String output = "";
            if (input != null)
            {
                output += input.InnerText + Environment.NewLine;

                if (input.HasChildNodes)
                {
                    output = "";
                    foreach (XmlNode it in input.ChildNodes)
                    {
                        output += getSubnodeString(it) + Environment.NewLine;
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Konvertuje XmlDocument u XPathDocument
        /// </summary>
        /// <param name="input">XmlDocument izvor</param>
        /// <returns>XPathDocument</returns>
        public static XPathDocument convertXml(this XmlDocument input)
        {
            MemoryStream memStream = new MemoryStream();
            input.Save(memStream);
            memStream.Position = 0;
            return new XPathDocument(memStream);
        }

        /// <summary>
        /// Testira da li se u prosledjenom nodeu poklapa bar jedna vrednost attributa
        /// </summary>
        /// <param name="source">Node koji se testira</param>
        /// <param name="attributeNames">DList spisak atributa ili @dataSource</param>
        /// <param name="attributeValues">DList spisak vrednosti za svaki od atributa - redosled je bitan</param>
        /// <returns></returns>
        public static Boolean compareAttributeValue(this XmlNode source, String attributeNames, String attributeValues,
                                                    stringMatchPolicy matchPolicy =
                                                        stringMatchPolicy.trimSpaceContainCaseFree,
                                                    collectionTestMode testMode = collectionTestMode.any)
        {
            List<String> attNames = imbStringCommonTools.multiOpsInputProcessing(attributeNames);
            List<String> attValues = imbStringCommonTools.multiOpsInputProcessing(attributeValues);
            return compareAttributeValue(source, attNames, attValues, matchPolicy, testMode);
        }

        /// <summary>
        /// Testira da li se u prosledjenom nodeu poklapa bar jedna vrednost attributa
        /// </summary>
        /// <param name="source">Node koji se testira</param>
        /// <param name="attNames">Lista imena atributa / odnosno specijalnih oznaka za izvor podatka</param>
        /// <param name="attValues">Lista vrednosti za svaki od imena atributa - prema istom redosledu</param>
        /// <returns></returns>
        public static Boolean compareAttributeValue(this XmlNode source, List<String> attNames, List<String> attValues,
                                                    stringMatchPolicy matchPolicy =
                                                        stringMatchPolicy.trimSpaceContainCaseFree,
                                                    collectionTestMode testMode = collectionTestMode.any)
        {
            Int32 limit = attNames.Count;
            XPathNavigator nav;
            if (limit == 0) return true;

            if (attValues.Count == 0) return true;

            if (attValues.Count > limit) limit = attValues.Count;

            Int32 a = 0;
            Boolean marked = false;

            switch (testMode)
            {
                case collectionTestMode.any:
                    marked = false;
                    break;

                case collectionTestMode.all:
                    marked = true;
                    break;
            }

            nav = source.CreateNavigator();
            if (nav != null)
            {
                //if (nav.HasAttributes)
                //{
                for (a = 0; a < limit; a++)
                {
                    String attName = "";
                    if (attNames.Count > a)
                    {
                        attName = attNames[a];
                    }
                    else
                    {
                        attName = attNames.Last();
                    }

                    String valName = "";
                    if (attValues.Count > a)
                    {
                        valName = attValues[a];
                    }
                    else
                    {
                        valName = attValues.Last();
                    }

                    String tmpVal = getNodeValueUniversal(source, attName); //nav.GetAttribute(attNames[a], "");

                    if (!String.IsNullOrEmpty(tmpVal))
                    {
                        Boolean test = imbStringOperations.compareStrings(tmpVal, valName, matchPolicy);

                        switch (testMode)
                        {
                            case collectionTestMode.any:
                                if (test)
                                {
                                    marked = true;
                                    a = limit;
                                }
                                break;

                            case collectionTestMode.all:
                                if (!test)
                                {
                                    marked = false;
                                    a = limit;
                                }
                                break;
                        }
                    }
                }
                //}
            }
            else
            {
                marked = false;
            }
            return marked;
        }

        /// <summary>
        /// Filtriranje liste XML nodova na osnovu zadate liste ključnih reči i vrednosti
        /// </summary>
        /// <param name="output">Lista XMLNodes koju će filtrirati komanda </param>
        /// <param name="attributeNames">MultiOps spisak izvora vrednosti - imena atributa odnosno specijalne oznake</param>
        /// <param name="attributeValues">MultiOps spisak vrednsoti za svaki od imena atributa / izvora. Redosled je važan.</param>
        /// <param name="matchPolicy">Vrsta testa vrednosti</param>
        /// <param name="testMode">Kriterijum za ceo skup</param>
        /// <param name="isInverted">Da li da koristi inverzni kriterijum</param>
        /// <returns>Filtirana lista</returns>
        public static List<XmlNode> filterNodeList(this List<XmlNode> output, String attributeNames,
                                                   String attributeValues,
                                                   stringMatchPolicy matchPolicy =
                                                       stringMatchPolicy.trimSpaceContainCaseFree,
                                                   collectionTestMode testMode = collectionTestMode.any,
                                                   Boolean isInverted = false)
        {
            List<XmlNode> filtered = new List<XmlNode>();
            Boolean test = false;
            foreach (XmlNode source in output)
            {
                test = compareAttributeValue(source, attributeNames, attributeValues, matchPolicy, testMode);
                if (isInverted)
                {
                    if (!test) filtered.Add(source);
                }
                else
                {
                    if (test) filtered.Add(source);
                }
            }
            return filtered;
        }

        /// <summary>
        /// Filtriranje liste XML nodova na osnovu zadate liste ključnih reči i vrednosti
        /// </summary>
        /// <param name="output">Lista XMLNodes koju će filtrirati komanda </param>
        /// <param name="attributeNames">Lista imena atributa odnosno specijalne oznake</param>
        /// <param name="attributeValues">Lista vrednosti svaki od imena atributa / izvora. Redosled je važan.</param>
        /// <param name="matchPolicy">Vrsta testa vrednosti</param>
        /// <param name="testMode">Kriterijum za ceo skup</param>
        /// <param name="isInverted">Da li da koristi inverzni kriterijum</param>
        /// <returns>Filtirana lista</returns>
        public static List<XmlNode> filterNodeList(this List<XmlNode> output, List<String> attributeNames,
                                                   List<String> attributeValues,
                                                   stringMatchPolicy matchPolicy =
                                                       stringMatchPolicy.trimSpaceContainCaseFree,
                                                   collectionTestMode testMode = collectionTestMode.any,
                                                   Boolean isInverted = false)
        {
            List<XmlNode> filtered = new List<XmlNode>();
            Boolean test = false;
            foreach (XmlNode source in output)
            {
                test = compareAttributeValue(source, attributeNames, attributeValues, matchPolicy, testMode);
                if (isInverted)
                {
                    if (!test) filtered.Add(source);
                }
                else
                {
                    if (test) filtered.Add(source);
                }
            }
            return filtered;
        }

        /// <summary>
        /// Vraca listu XmlNode-a koja odgovara definiciji indeksa
        /// </summary>
        /// <param name="indexes">Definicija indexa u string formatu</param>
        /// <param name="input">Ulazna lista</param>
        /// <returns>Filtriranu listu</returns>
        public static List<XmlNode> filterNodeList(this String indexes, List<XmlNode> input)
        {
            var ind = indexes.rangeStringToIndexList(input.Count());

            return input.imbIndexOps<XmlNode>(ind, indexOps.pass);

            //   return filterNodeList(rangeStringToIndexList(indexes, input.Count), input);
        }

        /// <summary>
        /// Vraca listu XmlNode-a prema datom spisku indeksa
        /// </summary>
        /// <param name="indexes">Lista indeksa</param>
        /// <param name="input">Ulazna lista</param>
        /// <returns>Filtrirana lista</returns>
        public static List<XmlNode> filterNodeList(this List<Int32> indexes, List<XmlNode> input)
        {
            List<XmlNode> output = new List<XmlNode>();

            foreach (Int32 ind in indexes)
            {
                if (ind < input.Count)
                {
                    output.Add(input[ind]);
                }
            }

            return output;
        }

        /// <summary>
        /// Pronalazi namespace prefix
        /// </summary>
        /// <param name="nsManager"></param>
        /// <returns></returns>
        public static string FindNamespacePrefix(XmlNamespaceManager nsManager)
        {
            if (nsManager == null) return "";
            String nameSpaceUrl = nsManager.LookupNamespace(imbXmlXPathTools.defaultNamespacePrefix);
            String nameSpacePrefix = "";
            if (!String.IsNullOrEmpty(nameSpaceUrl))
            {
                nameSpacePrefix = imbXmlXPathTools.defaultNamespacePrefix;
            }
            return nameSpacePrefix;
        }

        /// <summary>
        /// Pronalazi lokalni XPath
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parentNode"></param>
        /// <param name="nsManager"></param>
        /// <returns></returns>
        public static string FindLocalXPath(this XmlNode node, XmlNode parentNode, XmlNamespaceManager nsManager)
        {
            return FindLocalXPath(node, parentNode, FindNamespacePrefix(nsManager));
        }

        public static string FindLocalXPath(XmlNode childNode, XmlNode parentNode, String namespacePrefix = "")
        {
            String output = FindXPath(childNode, namespacePrefix);

            String sourcePath = FindXPath(parentNode, namespacePrefix) + "/";
            if (!String.IsNullOrEmpty(sourcePath))
            {
                if (output.IndexOf(sourcePath) == 0)
                {
                    output = output.Replace(sourcePath, "");
                }
            }
            return output;
        }

        /// <summary>
        /// Vraca xPath
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string FindXPath(this XmlNode node, String namespacePrefix = "")
        {
            if (node == null) return "";

            if (!String.IsNullOrEmpty(namespacePrefix))
            {
                namespacePrefix = namespacePrefix + ":";
            }

            StringBuilder builder = new StringBuilder();
            while (node != null)
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Attribute:
                        builder.Insert(0, "/@" + node.Name);
                        node = ((XmlAttribute)node).OwnerElement;
                        break;

                    case XmlNodeType.Comment:
                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Element:
                        int index = FindElementIndex(node);
                        builder.Insert(0, "/" + namespacePrefix + node.Name + "[" + index + "]");
                        node = node.ParentNode;
                        break;

                    case XmlNodeType.Document:
                        return builder.ToString();

                    default:
                        return "";
                        //throw new ArgumentException("Only elements and attributes are supported");
                }
            }
            return "";

            //throw new ArgumentException("Node was not in a document");
        }

        /// <summary>
        /// Vraća samo sopstveni XPath element, odnosno ime XML nodea
        /// </summary>
        /// <param name="node"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        public static String getXPathName(this XmlNode node, XmlNamespaceManager ns)
        {
            return getXPathName(node, FindNamespacePrefix(ns));
        }

        public static String getXPathName(this XmlNode node, String namespacePrefix = "")
        {
            StringBuilder builder = new StringBuilder();

            switch (node.NodeType)
            {
                case XmlNodeType.Attribute:
                    builder.Insert(0, "@" + node.Name);
                    node = ((XmlAttribute)node).OwnerElement;
                    break;

                case XmlNodeType.Comment:
                case XmlNodeType.Text:
                case XmlNodeType.CDATA:
                case XmlNodeType.Element:
                    int index = FindElementIndex(node);
                    builder.Insert(0, namespacePrefix + node.Name + "[" + index + "]");
                    node = node.ParentNode;
                    break;

                case XmlNodeType.Document:
                    return builder.ToString();

                default:
                    return "";
                    //throw new ArgumentException("Only elements and attributes are supported");
            }

            return builder.ToString();
        }

        public static string FindXPath(this XmlNode node, imbNamespaceSetup nsSetup)
        {
            return FindXPath(node, nsSetup.nsPrefix);
        }

        /// <summary>
        /// Vraca XPath putanju prosledjenog nodea
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nsManager"></param>
        /// <returns></returns>
        public static string FindXPath(this XmlNode node, XmlNamespaceManager nsManager)
        {
            return FindXPath(node, FindNamespacePrefix(nsManager));
        }

        public static int FindElementIndex(XmlElement element)
        {
            return FindElementIndex(element as XmlNode);
        }

        public static int FindElementIndex(XmlNode element)
        {
            XmlNode parentNode = element.ParentNode;
            if (parentNode is XmlDocument)
            {
                return 1;
            }
            XmlNode parent = (XmlNode)parentNode;
            int index = 1;
            foreach (XmlNode candidate in parent.ChildNodes)
            {
                if (candidate is XmlNode && candidate.Name == element.Name)
                {
                    if (candidate == element)
                    {
                        return index;
                    }
                    index++;
                }
            }
            return 0;
            //throw new ArgumentException("Couldn't find element within parent");
        }

        /// <summary>
        /// Vraca spisak svih xPathova - PREVAZIDJENO JER POSTOJI EFIKASNIJI METOD PREKO END NODE-a i XPatha
        /// </summary>
        /// <param name="node">Parent xmlNode</param>
        /// <param name="nsManager">Menadžer za namespace</param>
        /// <param name="prefixToTrim">Od svake putanje može odseći određeni prefiks</param>
        /// <param name="toDefaultList">Rezultat da bude u DefaultList / Preset list formatu</param>
        /// <returns>Spisak svih xPath putanja</returns>
        public static List<String> FindAllXPaths(XmlNode node, imbNamespaceSetup nsSetup = null,
                                                 String prefixToTrim = "", Boolean toDefaultList = false)
        {
            List<String> output = new List<string>();

            if (nsSetup == null)
            {
                if (node.OwnerDocument != null)
                {
                    nsSetup = new imbNamespaceSetup(node.OwnerDocument);
                    //nsManager = new XmlNamespaceManager(node.OwnerDocument.NameTable);
                }
            }

            String path = FindXPath(node, nsSetup.nsPrefix);

            // path = imbStringPathTools.removeCommonRoot(path, prefixToTrim);

            if (toDefaultList)
            {
                path = node.Name + "|" + path + ";";
            }

            output.Add(path);

            foreach (XmlNode nd in node.ChildNodes)
            {
                output.AddRange(FindAllXPaths(nd, nsSetup, prefixToTrim, toDefaultList));
            }

            return output;
        }
    }
}