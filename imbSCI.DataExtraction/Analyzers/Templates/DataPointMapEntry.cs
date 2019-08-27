using HtmlAgilityPack;
using imbSCI.Core.extensions;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Data;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.extensions;
using imbSCI.Data.extensions.data;
using imbSCI.Data.extensions.data;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Extractors;
using imbSCI.DataExtraction.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Analyzers.Templates
{
    /// <summary>
    /// Defines a data point
    /// </summary>
    [Serializable]
    public class DataPointMapEntry
    {

        public String GetCode(String memberName="dpm", StringBuilder sb = null)
        {
            if (sb == null) sb = new StringBuilder();

            if (memberName == "") {
                sb.AppendLine("new DataPointMapEntry() {");
            } else
            {
                sb.AppendLine("\t " + memberName + " = new DataPointMapEntry() {");
            }
            if (Properties.Any())
            {
                foreach (DataPointMapEntry p in Properties)
                {
                    p.GetCode("", sb);
                }
            } else
            {
                sb.AppendLine($"\t name=\"{name}\",");
                sb.AppendLine($"\t DataPointXPathRoot=\"{DataPointXPathRoot}\",");
                sb.AppendLine($"\t LabelXPathRelative=\"{LabelXPathRelative}\",");
                sb.AppendLine($"\t DataXPathRelative=\"{DataXPathRelative}\",");
                sb.AppendLine($"\t DisplayName=\"{DisplayName}\",");
                sb.AppendLine($"\t value=\"{value}\"");
            }

            if (memberName == "")
            {
                sb.AppendLine("},");
            }
            else
            {
                sb.AppendLine("};");
            }
            

            return sb.ToString().Trim(',');
        }

        /// <summary>
        /// Gets the number of dimensions.
        /// </summary>
        /// <param name="withoutLabels">if set to <c>true</c> [without labels].</param>
        /// <returns></returns>
        public Int32 GetNumberOfDimensions(Boolean withoutLabels=true)
        {
            Int32 c = 0;
            if (Properties.Any())
            {
                foreach (var p in Properties)
                {
                    c += p.GetNumberOfDimensions(withoutLabels);
                }

            } else
            {
                if (!withoutLabels) if (!LabelXPathRelative.isNullOrEmpty()) c++;
                if (!DataXPathRelative.isNullOrEmpty()) c++;
            }

            return c;
        }

        /// <summary>
        /// Subproperties
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        public List<DataPointMapEntry> Properties { get; set; } = new List<DataPointMapEntry>();

        /// <summary>
        /// Deploys the DataPoint by graph leaf
        /// </summary>
        /// <param name="leaf">The leaf.</param>
        /// <param name="isDynamic">if set to <c>true</c> [is dynamic].</param>
        /// <returns></returns>
        public Boolean SetLeaf(LeafNodeDictionaryEntry leaf, Boolean isDynamic)
        {
            if (isDynamic)
            {
                if (DataXPathRelative.isNullOrEmpty())
                {
                    DataXPathRelative = leaf.XPath;

                    if (!LabelXPathRelative.isNullOrEmpty()) DataPointXPathRoot = new List<String>() { DataXPathRelative, LabelXPathRelative }.GetCommonPathRoot();
                    
                    return true;
                } else
                {
                    return false;
                    
                }
            } else
            {
                if (LabelXPathRelative.isNullOrEmpty())
                {
                    LabelXPathRelative = leaf.XPath;

                    if (!DataXPathRelative.isNullOrEmpty()) DataPointXPathRoot = new List<String>() { DataXPathRelative, LabelXPathRelative }.GetCommonPathRoot();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        protected XPathValueSet SetLabelByRootNode(HtmlNode node)
        {
            var labelNode = node.selectSingleNode(LabelXPathRelative.GetRelativeXPath(node.XPath));
            var valueNode = node.selectSingleNode(DataXPathRelative.GetRelativeXPath(node.XPath));
            if (labelNode != null)
            {


                DisplayName = labelNode.GetInnerText();

                name = DisplayName.Trim().Replace(" ", "_").getCleanPropertyName().getCleanFilePath().Replace("/", "");
            }
            if (valueNode != null)
            {
                value = valueNode.GetInnerText();
                var xSet = new XPathValueSet()
                {
                    XPath = node.XPath,
                    Value = value
                };
                return xSet;
            }
            return null;
        }

        /// <summary>
        /// Sets the label.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="block">The block.</param>
        public XPathValueSet SetLabel(HtmlNode node, DataPointMapBlock block)
        {
            String path = DataPointXPathRoot.GetAbsoluteXPath(block.BlockXPathRoot).GetRelativeXPath(node.XPath);

            var xSet = new XPathValueSet()
            {
                XPath = path,
                Value = ""
            };

            var rootNode = node.selectSingleNode(path);
             

            if (Properties.Any())
            {
                foreach (var p in Properties)
                {
                    xSet.Add(p.SetLabelByRootNode(rootNode));
                }
                return xSet;
            } else
            {
                return SetLabelByRootNode(rootNode);
            }
           
        }

        public String name { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public String DisplayName { get; set; }


        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public String value { get; set; }

        [XmlIgnore]
        public Boolean IsSet
        {
            get
            {
                return !DataPointXPathRoot.isNullOrEmpty() && !LabelXPathRelative.isNullOrEmpty() && !DataXPathRelative.isNullOrEmpty();
            }
        }

        public DataPointMapEntry()
        {

        }

        public DataPointMapEntry(String labelPath, String dataPath)
        {
            DataPointXPathRoot = ExtractorTools.GetCommonPathRoot(new string[] { labelPath, dataPath }); // labelPath.getCommonRoot(dataPath, true, "/");

            LabelXPathRelative = labelPath.removeStartsWith(DataPointXPathRoot);
            DataXPathRelative = dataPath.removeStartsWith(DataPointXPathRoot);

        }

        /// <summary>
        /// Common root of <see cref="LabelXPathRelative"/> and <see cref="DataXPathRelative"/>
        /// </summary>
        /// <value>
        /// The data point x path root.
        /// </value>
        public String DataPointXPathRoot { get; set; } = "";
        /// <summary>
        /// LabelX path, relative to <see cref="DataPointXPathRoot"/>
        /// </summary>
        /// <value>
        /// The label x path relative.
        /// </value>
        public String LabelXPathRelative { get; set; } = "";

        /// <summary>
        /// Gets or sets the data x path relative.
        /// </summary>
        /// <value>
        /// The data x path relative.
        /// </value>
        public String DataXPathRelative { get; set; } = "";

    }
}