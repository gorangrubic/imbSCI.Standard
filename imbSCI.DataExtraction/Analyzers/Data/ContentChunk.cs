using HtmlAgilityPack;
using imbSCI.Core.data;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Core.math;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.math.range.frequency;
using imbSCI.Core.style.color;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.extensions;
using imbSCI.Data.extensions.data;
using imbSCI.DataExtraction.Analyzers.Structure;
using imbSCI.DataExtraction.Analyzers.Templates;
using imbSCI.DataExtraction.Extractors;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.NodeQuery;
using imbSCI.DataExtraction.Tools;
using imbSCI.DataExtraction.Tools.XPathSelectors;
using imbSCI.Graph.Data;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Analyzers.Data
{
    public enum TaskMultiNodePolicy
    {
        undefined,
        AsSeparatedTables,
        AsSingleTableRows,
        AsSignleTableColumns,

    }

    public enum ContentChunkType
    {
        Undefined,
        DynamicDataExtraction,
        TemplateExtraction,
        Diagnostic,
        Failed
    }

    [Serializable]
    public class ContentChunk
    {

        public ContentChunk()
        {

        }

        public void DeployRootNode(NodeGraph rootGraphNode)
        {

            var nodesWithItemSet = rootGraphNode.GetChildrenWithItemSet();

            XPathRoot = rootGraphNode.path;
            NodeName = rootGraphNode.path.GetLastNodeNameFromXPath();

            HtmlNode htmlNode = rootGraphNode.FindHtmlNodeInstance(rootGraphNode.path);

            if (htmlNode != null)
            {
                RootNode = htmlNode;
                ID = RootNode.Id;
                Class = RootNode.GetAttributeValue("class", "");
            }

        }

        public List<List<Node>> PublishAnnotation(DirectedGraph graph, String nodeColor = "#f7941d", NodeDictionaryGraphStyleSettings style=null)
        {
            if (style == null) style = new NodeDictionaryGraphStyleSettings();

            Node node = graph.Nodes[XPathRoot];

            node.Background = nodeColor;

            var chunkInfoNode = new Node()
            {
                Id = name,
                Label = name,
                Background = Color.LightGreen.ColorToHex(),
                Foreground = Color.DarkGray.ColorToHex(),
                Stroke = nodeColor,
                StrokeThinkness = 0
            };

            graph.Nodes.Add(chunkInfoNode);
            graph.Links.Add(new Link(node, chunkInfoNode) { StrokeDashArray = "2,2,2,2,2", Stroke = nodeColor });

            var allLinked = graph.GetAllLinkedInIterations(node);
            foreach (var iteration in allLinked)
            {
                foreach (var ln in iteration)
                {
                    if (ln != chunkInfoNode)
                    {
                        if (ln.Stroke == style.DynamicStrokeColor)
                        {
                            ln.Background = style.DynamicStrokeColor;
                        }
                        else if (ln.Stroke == style.StaticStrokeColor)
                        {
                            ln.Background = style.StaticStrokeColor;
                        }
                        else if (ln.StrokeDashArray.isNullOrEmpty())
                        {
                            ln.Background = nodeColor;

                        }
                    }
                    else
                    {
                        

                    }

                }
            }
            return allLinked;
        }

        public void Publish(folderNode folder)
        {
            String chunkStrictName = name.getCleanPropertyName();

            // var subfolder = folder.Add(chunkStrictName, name, "Cluster group diagnostics");
            if (SubGraph != null)
            {
                var dgml = SubGraph.BuildDirectedGraph();

                dgml.Save(folder.pathFor("chunk_" + chunkStrictName + "_subgraph.dgml", imbSCI.Data.enums.getWritableFileMode.overwrite, "DGML of subgraph selected by chunk"));

            }

            ExtractorCustomizationSettings.ReportSave(folder, name);

            if (taken_graph != null) taken_graph.Report(folder, null);

            var cp = folder.pathFor("chunk_" + chunkStrictName + ".xml", imbSCI.Data.enums.getWritableFileMode.overwrite, "");
            objectSerialization.saveObjectToXML(this, cp);


            var recordTemplate = ExtractorCustomizationSettings.GetObjectEntry<RecordTemplateSet>(nameof(RecordTemplateExtractor.TemplateSet)); // / .AddObjectEntry(, pair.Template);
            if (recordTemplate != null)
            {
                var tp = folder.pathFor("chunk_" + chunkStrictName + "_template.xml", imbSCI.Data.enums.getWritableFileMode.overwrite, "");
                objectSerialization.saveObjectToXML(this, tp);
            }
        }

        public StructureGraphInformation taken_graph { get; set; }

        public ContentChunkType type { get; set; } = ContentChunkType.Undefined;

        public String title { get; set; } = "";
        public String description { get; set; } = "";

        //[XmlIgnore]
        //[NonSerialized]
        //public NodeGraph ChunkGraphNode = null;

        public String name { get; set; } = "";

        public String NodeName { get; set; } = "";

        public String Class { get; set; } = "";

        public String ID { get; set; } = "";

        public String XPathRoot { get; set; } = "";

        public String ExtractorName { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public HtmlNode RootNode;

       // public MetaTableFormatType format { get; set; } = MetaTableFormatType.unknown;

        public TaskMultiNodePolicy multiNodePolicy { get; set; } = TaskMultiNodePolicy.undefined;

        public reportExpandedData ExtractorCustomizationSettings { get; set; } = new reportExpandedData();



    //    public DataPointMapperResult DPMapperResult { get; set; }

        public NodeQueryDefinition GetQueryDefinition()
        {

            NodeQueryDefinition output = new NodeQueryDefinition();

            //if (RootNode != null)
            //{
            //    var xpathSelector = RootNode.FindSelector();


            //    var xq = new NodeQueryDefinition()
            //    {
            //        query = xpathSelector.ToString(),
            //        queryType = NodeQuery.Enums.NodeQueryType.xpath,
            //        operand = NodeQuery.Enums.NodeQueryPredicateOperand.AND
            //    };
            //    return xq;

            //}

           

             NodeQueryDefinition q = new NodeQueryDefinition()
              {
                    query = XPathRoot,
                    queryType = NodeQuery.Enums.NodeQueryType.xpath,
                    operand = NodeQuery.Enums.NodeQueryPredicateOperand.AND
                };
                output.groupQueries.Add(q);

            /*
            if (!Class.isNullOrEmpty())
            {
                NodeQueryDefinition classQ = new NodeQueryDefinition()
                {
                    query = "//" + NodeName + "[@class='" + Class + "']",
                    queryType = NodeQuery.Enums.NodeQueryType.xpath,
                    operand = NodeQuery.Enums.NodeQueryPredicateOperand.AND,
                    resultOperand = NodeQuery.Enums.NodeQueryResultOperand.OVERLAP
                };
                output.groupQueries.Add(classQ);
            }*/

            if (!ID.isNullOrEmpty())
            {
                NodeQueryDefinition qID = new NodeQueryDefinition()
                {
                    query = "//" + NodeName + "[@id='" + ID + "']",
                    queryType = NodeQuery.Enums.NodeQueryType.xpath,
                    operand = NodeQuery.Enums.NodeQueryPredicateOperand.AND,
                    resultOperand = NodeQuery.Enums.NodeQueryResultOperand.OVERLAP
                };
                output.groupQueries.Add(qID);
            }

            //if (output.groupQueries.Count == 0)
            //{

            //}

            return output;
        }

        //public Int32 StaticAndDynamicMinCount
        //{
        //    get
        //    {

        //        return Math.Min(ContentAnalysis.DynamicContent.items.Count, ContentAnalysis.StaticContent.items.Count);
        //    }
        //}

        [XmlIgnore]
        public NodeGraph SubGraph { get; set; } = null;

        //[XmlIgnore]
        //public LeafNodeDictionaryAnalysis ContentAnalysis { get; set; } = new LeafNodeDictionaryAnalysis();

    }
}