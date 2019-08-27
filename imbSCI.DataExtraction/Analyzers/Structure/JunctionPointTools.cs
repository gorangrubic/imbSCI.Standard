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

namespace imbSCI.DataExtraction.Analyzers.Structure
{
public static class JunctionPointTools
    {

        public static ListDictionary<T, T> SplitByParentJunction<T>(this List<T> items, Int32 MinJunctions) where T : graphWrapNode<LeafNodeDictionaryEntry>, IGraphNode
        {
            ListDictionary<T, T> output = new ListDictionary<T, T>();
            List<String> paths = new List<string>();

            foreach (T item in items)
            {
                T junctionParent = item.GetFirstParent(x => x.Count() > MinJunctions, true, false);
                if (junctionParent != null)
                {
                    paths.Add(junctionParent.path);
                    output[junctionParent].Add(item);
                } else
                {
                    
                }
                
            }

            if (output.Count > 0)
            {

            }

            return output;
        }

    }
}