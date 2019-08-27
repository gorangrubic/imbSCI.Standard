using HtmlAgilityPack;
using imbSCI.Core.extensions;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.files;
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
    [Serializable]
    public class DataPointMapBlock
    {
        

        public String GetCode()
        {
            StringBuilder sb = new StringBuilder();

            String bp = "block_" + name;
            bp = bp.getCleanPropertyName();

            sb.AppendLine("\t " + nameof(DataPointMapBlock) + bp + " = new " + nameof(DataPointMapBlock) + "() {");

            sb.AppendLine($"\t name=\"{name}\",");
            sb.AppendLine($"\t BlockXPathRoot=\"{BlockXPathRoot}\"");
            sb.AppendLine("};");


            sb.AppendLine("\t DataPointMapEntry dpm = null;");
            foreach (DataPointMapEntry p in DataPoints)
            {
                p.GetCode("dpm", sb);

                //sb.AppendLine("\t dpm = new DataPointMapEntry() {");
                //sb.AppendLine($"\t name=\"{p.name}\",");
                //sb.AppendLine($"\t DataPointXPathRoot=\"{p.DataPointXPathRoot}\",");
                //sb.AppendLine($"\t LabelXPathRelative=\"{p.LabelXPathRelative}\",");
                //sb.AppendLine($"\t DataXPathRelative=\"{p.DataXPathRelative}\",");
                //sb.AppendLine($"\t DisplayName=\"{p.DisplayName}\",");
                //sb.AppendLine($"\t value=\"{p.value}\"");
                //sb.AppendLine("};");

                sb.AppendLine("\t " + bp + ".DataPoints.Add(dpm); ");
            }

            return sb.ToString();

        }


        public void Save(String filepath)
        {
            this.saveObjectToXML(filepath);

        }



        /// <summary>
        /// Sets the labels.
        /// </summary>
        /// <param name="node">The node.</param>
        public XPathValueSetForRecordCollection SetLabels(List<HtmlNode> nodes)
        {
            XPathValueSetForRecordCollection output = new XPathValueSetForRecordCollection()
            {
                Block = this
            };

            foreach (HtmlNode node in nodes)
            {
                XPathValueSetForRecord output_for_record = new XPathValueSetForRecord();

                var nd = node.SelectSingleNode(BlockXPathRoot.GetRelativeXPath(node.XPath));

                if (nd != null)
                {
                    HtmlNode hNode = nd.SelectHeading();
                    if (hNode != null)
                    {
                        name = hNode.GetInnerText().getCleanPropertyName().Replace(" ", "");
                    }

                    ID = nd.GetAttributeValue("id", "");
                    Class = nd.GetAttributeValue("class", "");


                    foreach (var dp in DataPoints)
                    {
                        output_for_record.Add(dp.SetLabel(nd, this));
                    }

                    output.Records.Add(output_for_record);
                }
            }
            return output;
        }

        /// <summary>
        /// Sets the relative x paths.
        /// </summary>
        public void SetRelativeXPaths()
        {
            foreach (DataPointMapEntry dp in DataPoints)
            {
                dp.DataXPathRelative = dp.DataXPathRelative.removeStartsWith(dp.DataPointXPathRoot);
                dp.LabelXPathRelative = dp.LabelXPathRelative.removeStartsWith(dp.DataPointXPathRoot);
                dp.DataPointXPathRoot = dp.DataPointXPathRoot.removeStartsWith(BlockXPathRoot);
                
            }

        }

        /// <summary>
        /// Sets the absolute x paths.
        /// </summary>
        public void SetAbsoluteXPaths()
        {
            foreach (DataPointMapEntry dp in DataPoints)
            {
                dp.DataPointXPathRoot = dp.DataPointXPathRoot.ensureStartsWith(BlockXPathRoot);
                dp.DataXPathRelative = dp.DataXPathRelative.ensureStartsWith(dp.DataPointXPathRoot);
                dp.LabelXPathRelative = dp.LabelXPathRelative.ensureStartsWith(dp.DataPointXPathRoot);
                
            }
        }

        public static DataPointMapBlock Load(String filepath)
        {
            return objectSerialization.loadObjectFromXML<DataPointMapBlock>(filepath);
        }


        public String name { get; set; }


        public String Class { get; set; } = "";

        public String ID { get; set; } = "";

        public String BlockXPathRoot { get; set; }


        public List<DataPointMapEntry> DataPoints { get; set; } = new List<DataPointMapEntry>();

        public DataPointMapBlock() { }

        public DataPointMapBlock(String root) {

            BlockXPathRoot = root;
        }

        public DataPointMapBlock(IEnumerable<DataPointMapEntry> items, String root)
        {

            BlockXPathRoot = root;

            foreach (var i in items)
            {
                i.DataPointXPathRoot = i.DataPointXPathRoot.removeStartsWith(BlockXPathRoot);
                DataPoints.Add(i);
            }


        }
    }
}