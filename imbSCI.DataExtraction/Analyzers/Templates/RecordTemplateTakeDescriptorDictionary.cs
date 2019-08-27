using imbSCI.Core.math.range.frequency;
using System;
using System.Collections.Generic;
using System.Text;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.Analyzers.Data;
using HtmlAgilityPack;
using System.Linq;
using imbSCI.Core.extensions.data;
using System.Xml.Serialization;
using imbSCI.Core.collection;

namespace imbSCI.DataExtraction.Analyzers.Templates
{
public class RecordTemplateTakeDescriptorDictionary
    {

        public RecordTemplateTakeDescriptorDictionary()
        {

        }

        public List<RecordTemplateTakeDescriptor> BySubXPath { get; set; } = new List<RecordTemplateTakeDescriptor>();

        public List<String> ValueCellSubXPath { get; set; } = new List<string>();
        
        public Int32 CellIndex(String subXpath, Boolean AddContextColumn)
        {
            Int32 index = ValueCellSubXPath.IndexOf(subXpath);
            if (AddContextColumn)
            {
                index++;
            }
            return index;
        }

        public Int32 CellIndex(RecordTemplateItemTake take, Boolean AddContextColumn)
        {
            Int32 index = ValueCellSubXPath.IndexOf(take.SubXPath);
            if (AddContextColumn)
            {
                index++;
            }
            return index;
        }
    
        public ListDictionary<String, String> TakeCountersBySubXPath = new ListDictionary<String, String>();

        public void Deploy()
        {
            if (TakeCountersBySubXPath.Count == 0)
            {
                return;
            }
            var maxCount = TakeCountersBySubXPath.Max(x => x.Value.Count);

            
            foreach (var pair in TakeCountersBySubXPath)
            {
                RecordTemplateTakeDescriptor descriptor = new RecordTemplateTakeDescriptor();
                descriptor.SubXPath = pair.Key;
                if (maxCount == 1)
                {
                    descriptor.Category = NodeInTemplateRole.Dynamic;
                    ValueCellSubXPath.Add(descriptor.SubXPath);
                }
                else
                {
                    if (pair.Value.Count > 1)
                    {
                        descriptor.Category = NodeInTemplateRole.Dynamic;
                        ValueCellSubXPath.Add(descriptor.SubXPath);
                    }
                    else
                    {
                        descriptor.Category = NodeInTemplateRole.Static;
                    }
                }

                BySubXPath.Add(descriptor);
            }
        }

    }
}