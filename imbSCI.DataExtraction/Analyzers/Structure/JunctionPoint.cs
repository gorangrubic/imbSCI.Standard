using imbSCI.Core.math.range.frequency;
using System;
using System.Collections.Generic;
using System.Text;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.Analyzers.Data;
using HtmlAgilityPack;
using imbSCI.Core.extensions.text;
using System.Linq;
using imbSCI.Core.extensions.data;
using System.Xml.Serialization;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting.render.builders;
using System.IO;
using imbSCI.Core.collection;
using imbSCI.DataExtraction.Extractors;
using imbSCI.DataExtraction.Analyzers.Templates;
using imbSCI.DataExtraction.Tools;

namespace imbSCI.DataExtraction.Analyzers.Structure
{

    [Serializable]
    public class JunctionPoint<T> where T : graphWrapNode<LeafNodeDictionaryEntry>, IGraphNode
    {


        public List<JunctionPoint<T>> ExplodeByParentJunctions()
        {
            List<JunctionPoint<T>> output = new List<JunctionPoint<T>>();

            ListDictionary<T, T> splits = items.SplitByParentJunction(2);
            if (splits.Any())
            {
                foreach (var pair in splits)
                {
                    JunctionPoint<T> newJunction = new JunctionPoint<T>();
                    newJunction.Learn(this);
                    newJunction.items = pair.Value;
                    newJunction.rootItem = pair.Key;
                    //newJunction.ProcessItems(pair.Key);
                    output.Add(newJunction);
                }
            } else
            {
                output.Add(this);
                if (this.rootItem == null)
                {
                    var item = items.FirstOrDefault();
                    if (item != null)
                    {
                        item = item.parent as T;
                    }
                    rootItem = item;
                }
                //this.ProcessItems(item);
            }

            return output;
        }

        public void ProcessItems(T _rootItem, Boolean TrimTemplateItems)
        {
            XPathRoot = items.Select(x => x.path).GetCommonPathRoot();
            Level = XPathRoot.GetXPathLevel();

            var firstItem = items.FirstOrDefault();
            if (_rootItem != null)
            {
                rootItem = _rootItem;
                XPathRoot = rootItem.path;
            }

            if (rootItem == null) rootItem = firstItem.parent as T;

            if (firstItem != null)
            {
                Template = new RecordTemplate();

                Template.SubXPath = rootItem.path.GetLastNodeNameFromXPath();

                type = JunctionPointType.BranchToLeafs;

                foreach (graphWrapNode<LeafNodeDictionaryEntry> child in firstItem.GetChildren())
                {

                    RecordTemplateItem r_item = new RecordTemplateItem();

                    if (child.IsBranchToLeaf())
                    {

                    }
                    else
                    {
                        type = JunctionPointType.DeepJunctionPoint;
                        break;
                    }

                    if (child.item != null)
                    {
                        r_item.Category = child.item.Category;
                    }

                    if (TrimTemplateItems)
                    {
                        r_item.SubXPath = child.name;
                    }
                    else
                    {
                        r_item.SubXPath = child.path.GetRelativeXPath(XPathRoot);
                    }

                    Template.items.Add(r_item);
                }
            }

            JunctionSize = items.Count;
        }

        public void Learn(JunctionPoint<T> source)
        {
            XPathRoot = source.XPathRoot;
            Signature = source.Signature;
            JunctionSize = source.JunctionSize;
            JunctionSizeFrequency = source.JunctionSizeFrequency;
            type = source.type;
        }

        public String XPathRoot { get; set; } = "";
        public Int32 Level { get; set; } = 0;

        public RecordTemplate Template { get; set; } = new RecordTemplate();

        public String Signature { get; set; } = "";

        [XmlIgnore]
        public List<T> items { get; set; } = new List<T>();

        [XmlIgnore]
        public T rootItem { get; set; }

        public Int32 JunctionSize { get; set; } = 0;
        public Int32 JunctionSizeFrequency { get; set; } = 0;

   

        public JunctionPointType type { get; set; } = JunctionPointType.Undefined;

        public void Report(folderNode folder)
        {
            builderForText output = new builderForText();
            output.AppendLine($"Signature: \t\t\t {Signature}");
            output.AppendLine($"XPathRoot: \t\t\t {XPathRoot}");
            output.AppendLine($"JunctionSize: \t\t {JunctionSize}");
            output.AppendLine($"JunctionSizeFrequency: \t {JunctionSizeFrequency}");
            output.AppendLine($"Level: \t {Level}");
            output.AppendLine($"Junction type: \t {type}");

            output.AppendLine($"Template:"); // \t {JunctionSizeFrequency}");
            output.AppendLine($"XSubPath: \t\t {Template.SubXPath}");
            output.AppendLine($"Signature: \t\t {Template.Signature}");
            output.AppendLine($"Query: \t\t {Template.BuildXPathQuery()}");


            output.AppendLine($"Items: \t\t ");
            foreach (T item in items)
            {
                 output.AppendLine(item.path);
            }


            String op = folder.pathFor("JunctionPoint_" + Signature + ".txt", imbSCI.Data.enums.getWritableFileMode.overwrite);
            File.WriteAllText(op, output.GetContent());
        }

        public JunctionPoint()
        {

        }
    }
}