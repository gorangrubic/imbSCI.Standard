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
using imbSCI.Core.math.classificationMetrics;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.Extractors.Core;

namespace imbSCI.DataExtraction.Analyzers.Templates
{
    public class RecordTemplateSet
    {

        public List<RecordTemplate> items { get; set; } = new List<RecordTemplate>();

        public SourceTable MakeSourceTable(HtmlNode tableNode, HtmlExtractorBase extractor)
        {
            RecordTemplateTableModel tableModel = new RecordTemplateTableModel();

            foreach (RecordTemplate template in items)
            {
                String xpath = template.BuildXPathQuery();

                HtmlNodeCollection records = tableNode.SelectNodes(xpath);

                if (records == null)
                {
                    continue;
                } else
                {

                }


                Int32 rc = 0;
                foreach (HtmlNode recordNode in records) {

                    if (template.RecordSelectionLimit >= 0)
                    {
                        if (rc > template.RecordSelectionLimit)
                        {
                            break;
                        }
                    }
                    var takes = template.SelectTakes(recordNode);

                    tableModel.StoreRecord(takes);
                    rc++;
                }
            }

            tableModel.Analyze();

            var sourceTable =  tableModel.GetSourceTable(extractor);

            if (sourceTable != null)
            {
                sourceTable.ExpandedData.AddObjectEntry(nameof(RecordTemplateTableModel), tableModel, "Record table model");
            }
            return sourceTable;
        }

    }
}