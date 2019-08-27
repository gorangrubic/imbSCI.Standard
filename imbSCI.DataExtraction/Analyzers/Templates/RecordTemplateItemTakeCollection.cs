using imbSCI.Core.math.range.frequency;
using System;
using System.Collections.Generic;
using System.Text;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Extractors;
using imbSCI.DataExtraction.Analyzers;
using HtmlAgilityPack;
using System.Linq;
using imbSCI.Core.extensions.data;
using System.Xml.Serialization;
using imbSCI.Core.collection;
using imbSCI.DataExtraction.Extractors.Detectors;
using imbSCI.Core.math.range.finder;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.Extractors.Core;

namespace imbSCI.DataExtraction.Analyzers.Templates
{
    public class RecordTemplateCells
    {
        [XmlIgnore]
        public Int32 Count { get => items.Count; }

        public RecordTemplateCells() { }

        public List<RecordTemplateItemTake> items { get; set; } = new List<RecordTemplateItemTake>();

        public void AddRange(IEnumerable<RecordTemplateItemTake> takes)
        {
            foreach (var take in takes)
            {
                AddCell(take);
            }
        }

        public void AddCell(RecordTemplateItemTake take, Boolean overwrite=true)
        {
            var cell = items.FirstOrDefault(x => x.SubXPath == take.SubXPath);
            if (cell != null)
            {
                if (overwrite)
                {
                    items.Remove(cell);
                    items.Add(take);
                }
            } else
            {
                items.Add(take);
            }
            
        }

    }

    public class RecordTemplateItemTakeCollection:RecordTemplateCells
    {
        public RecordTemplateItemTakeCollection() { }

        [XmlIgnore]
        public RecordTemplate Template { get; set; }

        [XmlIgnore]
        public HtmlNode RecordNode { get; set; }
    }
}