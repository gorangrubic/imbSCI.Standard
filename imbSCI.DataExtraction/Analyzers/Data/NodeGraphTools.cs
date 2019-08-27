using HtmlAgilityPack;
using imbSCI.Core.math;
using imbSCI.Data.extensions.data;
using imbSCI.Data.extensions;
using imbSCI.Data;
using imbSCI.Graph.Converters;
using imbSCI.Core.extensions.data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using imbSCI.Core.files.folders;
using imbSCI.Data.collection.graph;
using imbSCI.Core.math.range.frequency;
using System.IO;
using imbSCI.Data.interfaces;
using imbSCI.Core.math.range.finder;
using imbSCI.Core.reporting.render;
using imbSCI.Core.data;
using System.Data;
using imbSCI.DataComplex.tables;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Core.extensions.table;
using imbSCI.Graph.Data;

namespace imbSCI.DataExtraction.Analyzers.Data
{
public static class NodeGraphTools
    {

        public static HtmlNode FindHtmlNodeInstance(this NodeGraph graph, String XPath)
        {
            var allNodesWithItems = graph.GetChildrenWithItemSet(true);
            foreach (var entry in allNodesWithItems.Select(x=>x.item))
            {
                if (entry.node != null)
                {
                    var result = entry.node.OwnerDocument.DocumentNode.SelectSingleNode(XPath);
                    if (result != null)
                    {
                        return result;
                    }
                        
                }
            }

            return null;
        }

       

        public static List<NodeGraph> GetChildrenWithItemSet(this NodeGraph graph, Boolean includeSelf=false)
        {
            var allNodes = graph.getAllChildren(null, false, false, 1, 500, includeSelf); //.Where(x => x.item != null);

            List<NodeGraph> output = new List<NodeGraph>();

            foreach (NodeGraph node in allNodes)
            {
                if (node.item != null)
                {
                    output.Add(node);
                }
            }
            return output;
        }

public static void Publish(this StructureGraphInformationSet info, folderNode folder, String name, aceAuthorNotation notation=null)
        {
            DataTable dt = info.items.ReportToDataTable<StructureGraphInformation>(true);
            dt.SetTitle(name + " records");
            dt.GetReportAndSave(folder, notation);

            dt = info.changes.ReportToDataTable<StructureGraphInformation>(true);
            dt.SetTitle(name + " changes");
            dt.GetReportAndSave(folder, notation);

            builderForText output = new builderForText();
            foreach (StructureGraphInformation item in info.items)
            {
                item.Report(null, output);
            }
            output.ReportSave(folder, name + "_records", "Structure graph entries");

            output = new builderForText();
            foreach (var item in info.changes)
            {
                item.Report(null, output);
            }
            output.ReportSave(folder, name + "_changes", "Structure graph changes log");
        }
    }
}