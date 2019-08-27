using HtmlAgilityPack;
using imbSCI.Core.extensions.data;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Analyzers.Structure;
using imbSCI.DataExtraction.Analyzers.Templates;
using imbSCI.DataExtraction.Extractors.Core;
using imbSCI.DataExtraction.Extractors.Task;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.SourceData.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.Extractors
{
    public class RecordTemplateExtractor : HtmlExtractorBase, IHtmlExtractor
    {
        private RecordTemplateSet _templateSet;

        //private RecordTemplate _template;

        //public RecordTemplate Template
        //{
        //    get { return _template; }
        //    set { _template = value; }
        //}

        public RecordTemplateSet TemplateSet
        {
            get { return _templateSet; }
            set {
                _templateSet = value;
            }
        }


        public override MetaTableDescription GetTableDescription()
        {
            return null;
        }

        //public override MetaTableSchema GetTableSchema()
        //{
        //    return null;
        //}

        


        public override MetaTable Construct(SourceTable sourceTable, TableExtractionTask task)
        {

            return base.Construct(sourceTable, task);

            //if (UseUniversalConstructors)
            //{
            //    return base.Construct(sourceTable, task);
            //}

            //Dictionary<String, Int32> ColumnIndexByPropertyName = new Dictionary<string, int>();
            //Dictionary<Int32, String> PropertyNameByColumnIndex = new Dictionary<Int32, String>();

            //for (int i = 0; i < Template.items.Count; i++)
            //{
            //    var tItem = Template.items[i];

            //    tItem.Category.HasFlag(NodeInTemplateRole.Static);

            //    List<string> uniContent = sourceTable.GetColumn(i).GetUnique();

            //    if (uniContent.Count == 1)
            //    {
            //      // ColumnIndexByPropertyName.Add(uniContent.First(), i);
            //       PropertyNameByColumnIndex.Add(i, uniContent.First());
            //    }
            //}

            //if (!PropertyNameByColumnIndex.Any()) {

            //    return base.Construct(sourceTable, task);
            //} else
            //{
            //    MetaTable output = new MetaTable();

            //    List<List<String>> ColumnData = new List<List<string>>();

            //    for (int i = 0; i < sourceTable.Width; i++)
            //    {
            //        if (PropertyNameByColumnIndex.ContainsKey(i))
            //        {
            //            output.properties.Add(PropertyNameByColumnIndex[i], ColumnData.Count);
            //        } else
            //        {
            //            ColumnData.Add(sourceTable.GetColumn(i));
            //        }
            //    }

            //    for (int i = 0; i < ColumnData.Count; i++)
            //    {
            //        var p = output.properties.FirstOrDefault(x => x.index == i);
            //        if (p == null)
            //        {
            //             output.properties.Add("P" + i.ToString(), i);
            //        }
            //    }

            //    for (int i = 0; i < sourceTable.Height; i++)
            //    {
            //        MetaTableEntry entry = null;
            //        for (int j = 0; j < ColumnData.Count; j++)
            //        {
                       
            //            if (j == 0)
            //            {
            //                entry = new MetaTableEntry()
            //                {
            //                    ID = ColumnData[j][i]
            //                };
            //            }

            //            var p = output.properties.FirstOrDefault(x => x.index == j);
            //            entry.properties[p.PropertyName] = ColumnData[j][i];
            //        }
            //        output.entries.Add(entry);
            //    }

            //    output.RefineSchema(sourceContentAnalysis);
            //    return output;
            //}
    
        }

        public override void SetSourceTableCell(SourceTableCell cell, HtmlNode cellNode, HtmlDocument htmlDocument)
        {
            base.SetSourceTableCell(cell, cellNode, htmlDocument);
        }



        public override SourceTable MakeSourceTable(HtmlNode tableNode)
        {
            if (TemplateSet == null) return null;

            return TemplateSet.MakeSourceTable(tableNode, this);

            /*
            String xpath = Template.BuildXPathQuery();

            HtmlNodeCollection records = tableNode.SelectNodes(xpath);
                        
            if (records == null) return null;
            if (records.Count == 0)
            {

            }


            SourceTable sourceTable = new SourceTable(Template.items.Count,  records.Count);

            for (int i = 0; i < records.Count; i++)
            {
                var node = records[i];
                var cells = Template.SelectCells(node);
                var cellNodes = cells.Values.ToList();

                for (int j = 0; j < cells.Count; j++)
                {
                    var cNode = cellNodes[j];
                    if (cNode==null)
                    {
                        sourceTable[j, i].Value = "";
                    } else
                    {
                        SetSourceTableCell(sourceTable[j, i], cNode, tableNode.OwnerDocument);
                    }
                }
            }

            return sourceTable;
            */
        }
    }
}