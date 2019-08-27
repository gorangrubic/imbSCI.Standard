using HtmlAgilityPack;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Analyzers.Templates;
using imbSCI.DataExtraction.Extractors.Core;
using imbSCI.DataExtraction.Extractors.Task;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.SourceData.Analysis;
using imbSCI.DataExtraction.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.Extractors
{

    public class DLTagExtractor : HtmlExtractorBase
    {
        public String html_terms_xpath { get; set; } = HtmlExtractionTools.XPATH_SELECT_LIST_DT;
        public String html_definitions_xpath { get; set; } = HtmlExtractionTools.XPATH_SELECT_LIST_DD;

        public DLTagExtractor()
        {

        }

        public String GetNodeValue(HtmlNode cellNode)
        {
            if (cellNode == null) return "";
              String v = cellNode.InnerText;

            //   v = HtmlDocument.HtmlEncode(cellNode.InnerText);
            //htmlDocument.Encoding
            String nv = valueExtraction.ProcessInput(v,  cellNode.OwnerDocument.StreamEncoding, cellNode.OwnerDocument.Encoding);

            return nv;
        }

        public override MetaTableDescription GetTableDescription()
        {
            MetaTableDescription output = new MetaTableDescription();
            output.index_propertyID = 0;
            output.index_entryID = 0;
            output.format = MetaTableFormatType.vertical;
            output.Interpretation = MetaTableInterpretation.singleEntity;
            output.sourceDescription.valueZone = new imbSCI.Data.primitives.coordinateXY();
            output.sourceDescription.valueZone.x = 0;
            output.sourceDescription.valueZone.y = 0;
            return output;
        }

        //public override MetaTableSchema GetTableSchema()
        //{
        //    return null;
        //    //throw new NotImplementedException();
        //}

        /// <summary>
        /// Constructs the specified source table.
        /// </summary>
        /// <param name="sourceTable">The source table.</param>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        public override MetaTable Construct(SourceTable sourceTable, TableExtractionTask task)
        {

           

            if (UseUniversalConstructors)
            {
                return base.Construct(sourceTable, task);
            }

            MetaTable table = new MetaTable(GetTableDescription());

            var rows = sourceTable.GetContentCells();
            //var data = sourceTable.GetContentCells();

            Boolean IsMultiEntryList = false;

            if (sourceTable.Width > 2)
            {
                IsMultiEntryList = true;
            }

            if (IsMultiEntryList)
            {
                table.description.format = MetaTableFormatType.vertical;

                var entryIDProperty = table.properties.Add("ID");
                entryIDProperty.index = EntryID;

                var EntryPropertyTerm = table.properties.Add("Term");
                EntryPropertyTerm.index = PropertyX;

                var EntryPropertyValue = table.properties.Add("Value");
                EntryPropertyValue.index = ValueX;

                foreach (var row in rows)
                {
                    table.entries.CreateEntry(row, true);
                }

            } else
            {
                table.description.format = MetaTableFormatType.horizontal;

                Dictionary<String, MetaTableProperty> propDict = new Dictionary<string, MetaTableProperty>();

                List<String> propertyValues = new List<string>();

                foreach (var row in rows)
                {
                    String propertyName = row[PropertyX].Value;
                    String propertyValue = row[ValueX].Value;
                    propertyValues.Add(propertyValue);

                    var vInfo = sourceContentAnalysis.DetermineContentType(propertyValue, true);
                    var metaProperty = table.properties.Add(propertyName);
                    metaProperty.ContentType = vInfo.type;
                    propDict.Add(propertyName, metaProperty);

                    RefinedPropertyStats pStats = new RefinedPropertyStats();
                    pStats.Assign(vInfo);
                    pStats.Compute();
                    pStats.Deploy(metaProperty);
                }

                MetaTableEntry entry = table.entries.CreateEntry(propertyValues, true);

            }

            return table;
        }

        public const Int32 EntryID = 2;

        public const Int32 PropertyX = 0;
        public const Int32 ValueX = 1;

        public override SourceTable MakeSourceTable(HtmlNode dlNode)
        {
            HtmlNodeCollection html_terms = dlNode.SelectNodes(html_terms_xpath);
            HtmlNodeCollection html_definitions = dlNode.SelectNodes(html_definitions_xpath);

            if (html_terms == null || html_definitions == null) return null;

            Int32 rows = Math.Min(html_terms.Count, html_definitions.Count);
           

            
            
            XPathValueSet  definitionsByTerms = new XPathValueSet();
            if (UseUniversalConstructors)
            {
                definitionsByTerms.Add(new XPathValueSet()
                {
                    XPath = "Property",
                    Value = "Value"
                });
            }
           

            Boolean IsMultiEntryList = false;

            List<String> t_known = new List<string>();

            for (int i = 0; i < rows; i++)
            {
                String t = GetNodeValue(html_terms[i]);
                String d = GetNodeValue(html_definitions[i]);
                
                if (t.isNullOrEmpty() || d.isNullOrEmpty())
                {

                } else
                {
                    if (t_known.Contains(t))
                    {
                        IsMultiEntryList = true;
                    }
                    t_known.Add(t);
                    definitionsByTerms.Add(new XPathValueSet()
                    {
                        XPath = t,
                        Value = d
                    });

                }
            }

            Int32 columns = 2;
            if (IsMultiEntryList)
            {
                columns = 3;
            }

            SourceTable sourceTable = new SourceTable(columns, definitionsByTerms.Count);
            
            for (int i = 0; i < rows; i++)
            {
                SetSourceTableCell(sourceTable[PropertyX, i], html_terms[i], html_terms[i].OwnerDocument);
                SetSourceTableCell(sourceTable[ValueX, i], html_definitions[i], html_terms[i].OwnerDocument);

             //   sourceTable[PropertyX, i].Value = definitionsByTerms[i].XPath;
             //   sourceTable[ValueX, i].Value = definitionsByTerms[i].Value;
                if (IsMultiEntryList)
                {
                    sourceTable[EntryID, i].Value = i.ToString();
                }
            }

            return sourceTable;
        }
    }
}