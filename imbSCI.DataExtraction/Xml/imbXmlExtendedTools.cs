namespace imbSCI.DataExtraction.Xml
{
    #region imbVeles using

    using imbSCI.Core.extensions.text;
    using imbSCI.DataExtraction.Xml.enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    #endregion imbVeles using

    public static class imbXmlExtendedTools
    {
        /*
        static private XmlNode setNodeByxPath(XmlDocument doc, string xPath, XmlNode source)
        {
            return setNodeByxPath(doc, doc as XmlNode, xPath, source); //makeXPath(doc, doc as XmlNode, xpath);
        }*/

        /// <summary>
        /// Da li da koristi XPath tamo gde je moguce
        /// </summary>
        public static Boolean useXPathOptimization = true;

        /// <summary>
        /// Osigurava da prosledjeni XmlDocument ima element za dati xPath i vraća referencu prema poslednjem XmlNode-u koji je kreiran
        /// </summary>
        public static XmlNode makeNodeByxPath(XmlDocument doc, XmlNode parent, String xPath, XmlNode source,
                                              imbNamespaceSetup nsSetup = null)
        {
            XmlNode output; // = new XmlNode();

            if (String.IsNullOrEmpty(xPath)) return parent;

            string[] partsOfXPath = xPath.Trim('/').Split('/');
            string nextNodeInXPath = partsOfXPath.First();
            if (string.IsNullOrEmpty(nextNodeInXPath)) return parent;

            if (nsSetup == null) nsSetup = new imbNamespaceSetup(doc);

            // get or create the node from the name
            //XmlNode node = parent.SelectSingleNode(nextNodeInXPath);
            String pt = "/" + nextNodeInXPath;

            XmlNode node = imbAdvancedXPath.xPathExecution(pt, parent, nsSetup, queryEngine.imbXPathQuery, true, 0);

            if (node == null)
            {
                String cleanName = imbStringPathTools.getTitleFromPath(nextNodeInXPath);
                node = doc.CreateNode(XmlNodeType.Element, nsSetup.nsPrefix, cleanName, nsSetup.nsSourceUrl);
                try
                {
                    node = parent.AppendChild(node);
                }
                catch (Exception ex)
                {
                    throw;

                    //logSystem.log("Node on> " + node.FindXPath(nsSetup) + " failed: " + ex.Message,
                    //              logType.Notification);
                }
            }

            // rejoin the remainder of the array as an xpath expression and recurse
            string rest = String.Join("/", partsOfXPath.Skip(1).ToArray());

            if (partsOfXPath.Length == 1)
            {
                foreach (XmlAttribute att in source.Attributes)
                {
                    throw new NotImplementedException(nameof(makeNodeByxPath));
                    //  node.SetAttribute(att.Name, att.Value);
                }
            }

            return makeNodeByxPath(doc, node, rest, source, nsSetup);
        }

        /// <summary>
        /// Vraća OuterXml String od liste
        /// </summary>
        /// <param name="source">Node lista koju konvertuje</param>
        /// <returns>String cele liste</returns>
        public static String getOuterXml(this List<XmlNode> source)
        {
            String output = "";
            foreach (XmlNode nd in source)
            {
                output += nd.OuterXml + Environment.NewLine;
            }
            return output;
        }

        /// <summary>
        /// Vraća listu XmlNode-a u skladu sa kriterijumima
        /// </summary>
        /// <param name="source">Node lista od koje kreće</param>
        /// <param name="collectMode">Način na koji prikuplja</param>
        /// <param name="level">Koliko nivoa u dubinu (opciono)</param>
        /// <returns>Lista XmlNode-a koji su u određenom odnosu sa Source</returns>
        public static List<XmlNode> collectChildren(this List<XmlNode> source, String collectMode, Int32 level)
        {
            List<XmlNode> output = new List<XmlNode>();
            foreach (XmlNode item in source)
            {
                output.AddRange(item.collectChildren(collectMode, level));
            }
            return output;
        }

        /// <summary>
        /// Vraća listu XmlNode-a u skladu sa kriterijumima
        /// </summary>
        /// <param name="source">Node lista od koje kreće</param>
        /// <param name="collectMode">Način na koji prikuplja</param>
        /// <param name="level">Koliko nivoa u dubinu (opciono)</param>
        /// <returns>Lista XmlNode-a koji su u određenom odnosu sa Source</returns>
        public static List<XmlNode> collectChildren(this List<XmlNode> source, collectRelatives collectMode, Int32 level)
        {
            List<XmlNode> output = new List<XmlNode>();
            foreach (XmlNode item in source)
            {
                output.AddRange(item.collectChildren(collectMode, level));
            }
            return output;
        }

        /// <summary>
        /// Vraća listu XmlNode-a u skladu sa kriterijumima
        /// </summary>
        /// <param name="source">Node od kojeg kreće</param>
        /// <param name="collectMode">Način na koji prikuplja</param>
        /// <param name="level">Koliko nivoa u dubinu (opciono)</param>
        /// <returns>Lista XmlNode-a koji su u određenom odnosu sa Source</returns>
        public static List<XmlNode> collectChildren(this XmlNode source, collectRelatives collectMode, Int32 level)
        {
            return source.collectChildren(collectMode.ToString().ToLower(), level);
        }

        /// <summary>
        /// Vraća listu XmlNode-a u skladu sa kriterijumima
        /// </summary>
        /// <param name="source">Node od kojeg kreće</param>
        /// <param name="collectMode">Način na koji prikuplja</param>
        /// <param name="level">Koliko nivoa u dubinu (opciono)</param>
        /// <returns>Lista XmlNode-a koji su u određenom odnosu sa Source</returns>
        public static List<XmlNode> collectChildren(this XmlNode source, String collectMode, Int32 level)
        {
            List<XmlNode> tmpList = null;

            List<XmlNode> output = new List<XmlNode>();
            switch (collectMode)
            {
                case "endnodes":
                    if (useXPathOptimization)
                    {
                        output.AddRange(imbXmlXPathTools.queryXPath(@"//*[not(node())]", null, source, false));
                    }
                    else
                    {
                        output.AddRange(source.getChildren(0, 500, false));
                    }
                    break;

                case "uptolevel":
                    output.AddRange(source.getChildren(0, level, true));
                    break;

                case "downtolevel":
                    tmpList = source.collectChildren(collectRelatives.endNodes, 0);
                    output = tmpList.collectChildren(collectRelatives.parentNodes, level);
                    break;

                default:
                case "onlevelnodes":
                case "onlevel":
                    output.AddRange(source.getChildren(0, level, false));
                    break;

                case "relatives":
                    tmpList = source.collectChildren(collectRelatives.parentNodes, level);
                    output.AddRange(tmpList.collectChildren(collectRelatives.brothers, 0));
                    break;

                case "childnodes":
                case "childs":
                    output.AddRange(source.ChildNodes.imbToList());
                    break;

                case "parentnodes":
                case "parents":
                    XmlNode selNode = source;
                    Int32 a = 0;
                    for (a = 0; a < level; a++)
                    {
                        if (selNode.ParentNode != null)
                        {
                            if (!output.Contains(selNode))
                            {
                                output.Add(selNode.ParentNode);
                            }
                            selNode = selNode.ParentNode;
                        }
                    }
                    break;

                case "brothers":
                    if (source.ParentNode != null)
                    {
                        output.AddRange(source.ParentNode.ChildNodes.imbToList());
                    }
                    break;
            }
            return output;
        }

        /// <summary>
        /// Vraća decu nodea u skladu sa parametrima
        /// </summary>
        /// <param name="nd">Source xmlNode lista</param>
        /// <param name="l">Trenutna dubina - zbog rekurzivnog pozivanja</param>
        /// <param name="depth">Nivo do kojeg se ide - relativno je na startLevel</param>
        /// <param name="isAdditive">Da li dodaje ili samo prosleđuje</param>
        /// <param name="startLevel">Od kog nivoa počinje da skuplja - koristi se za selektovanje "od kraja"</param>
        /// <return>Lista child nodea</return>
        public static List<XmlNode> getChildren(this List<XmlNode> nd, Int32 l = 0, Int32 depth = 1,
                                                Boolean isAdditive = false, Int32 startLevel = 0)
        {
            List<XmlNode> output = new List<XmlNode>();
            foreach (XmlNode sub in nd)
            {
                output.AddRange(getChildren(sub, l, depth, isAdditive, startLevel));
            }
            return output;
        }

        /// <summary>
        /// Vraća decu nodea u skladu sa parametrima
        /// </summary>
        /// <param name="nd">Source xmlNode</param>
        /// <param name="l">Trenutna dubina - zbog rekurzivnog pozivanja</param>
        /// <param name="depth">Nivo do kojeg se ide - relativno je na startLevel</param>
        /// <param name="isAdditive">Da li dodaje ili samo prosleđuje</param>
        /// <param name="startLevel">Od kog nivoa počinje da skuplja - koristi se za selektovanje "od kraja"</param>
        /// <return>Lista child nodea</return>
        public static List<XmlNode> getChildren(this XmlNode nd, Int32 l = 0, Int32 depth = 1,
                                                Boolean isAdditive = false, Int32 startLevel = 0)
        {
            List<XmlNode> input = new List<XmlNode>();

            foreach (XmlNode sub in nd.ChildNodes)
            {
                if (isAdditive)
                {
                    input.Add(sub);
                }
                else if (l == (depth + startLevel))
                {
                    input.Add(sub);
                }
                else if (!sub.HasChildNodes)
                {
                    input.Add(sub);
                }

                if (sub.HasChildNodes)
                {
                    if (l < (depth + startLevel))
                    {
                        if (l >= startLevel)
                        {
                            input.AddRange(sub.getChildren(l + 1, depth, isAdditive));
                        }
                    }
                }
            }
            return input;
        }
    }
}