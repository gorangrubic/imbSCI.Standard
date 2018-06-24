using imbSCI.Data;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;
using System;
using System.Collections.Generic;

namespace imbSCI.Graph.Converters.enumToGraph
{
    public class enumTypeStructure
    {
        public List<List<String>> items { get; set; } = new List<List<string>>();

        public enumTypeStructure()
        {
        }
    }

    /// <summary>
    /// Converts enum types or values into directed graphs showing flag relationships
    /// </summary>
    public class enumToDMGLConverter
    {
        /// <summary>
        /// Converts the type of an Enum into directed graph that shows relationship between values (by interpreting the flags)
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns>Directed graph showing relationship between the values</returns>
        public DirectedGraph ConvertEnumType(Type enumType, Boolean showTypeRootNode = true)
        {
            DirectedGraph dg = new DirectedGraph();

            dg.GraphDirection = DGML.enums.GraphDirectionEnum.LeftToRight;

            Node typeNode = null;

            if (showTypeRootNode) typeNode = dg.Nodes.AddNode(enumType.FullName, enumType.Name);

            //List<List<Object>> lists = new List<List<object>>();

            Dictionary<Node, List<Object>> dict = new Dictionary<Node, List<object>>();

            foreach (Object item in Enum.GetValues(enumType))
            {
                Enum enItem = (Enum)item;
                System.Collections.IList subItems = enItem.getEnumListFromFlags();

                if (subItems.Count > 1)
                {
                    List<Object> list = new List<object>();

                    foreach (Object sItem in subItems)
                    {
                        list.Add(sItem);
                    }

                    var node = dg.Nodes.AddNode(item.ToString());
                    if (showTypeRootNode) dg.Links.AddLink(typeNode, node, "");
                    dict.Add(node, list);
                }
                else
                {
                    if (subItems.Count == 0) subItems.Add(item);

                    foreach (Object sItem in subItems)
                    {
                        var node = dg.Nodes.AddNode(sItem.ToString());
                        if (showTypeRootNode) dg.Links.AddLink(typeNode, node, "");
                    }
                }
            }

            foreach (KeyValuePair<Node, List<object>> pair in dict)
            {
                foreach (Object item in pair.Value)
                {
                    String label = item.ToString();

                    if (pair.Key.Label != label)
                    {
                        var node = dg.Nodes.AddNode(label);
                        dg.Links.AddLink(pair.Key, node, "");
                    }
                }
            }

            return dg;
        }
    }
}