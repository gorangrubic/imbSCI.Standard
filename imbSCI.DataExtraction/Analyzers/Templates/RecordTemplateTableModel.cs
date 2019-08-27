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
using imbSCI.DataExtraction.MetaTables;
using imbSCI.DataExtraction.Tools;

namespace imbSCI.DataExtraction.Analyzers.Templates
{
    public class RecordTemplateTableModel
    {

        public RecordTemplateTableModel()
        {

        }

        public RecordTemplateTableRow ColumnContextTakes { get; set; } = new RecordTemplateTableRow();

        public List<RecordTemplateTableRow> Rows { get; set; } = new List<RecordTemplateTableRow>();

        

        public void StoreRecord(RecordTemplateItemTakeCollection takes)
        {
            switch (takes.Template.Role)
            {
                case RecordTemplateRole.GeneralContextProvider:
                    ColumnContextTakes.RecordXPath = takes.RecordNode.XPath;
                    foreach (var take in takes.items)
                    {
                      
                        ColumnContextTakes.Cells.AddCell(take, false);
                    }
                    break;
                case RecordTemplateRole.RowDataAndContextProvider:
                case RecordTemplateRole.RowDataProvider:
                    RecordTemplateTableRow row = null;
                    if (!Rows.Any(x=>x.RecordXPath == takes.RecordNode.XPath))
                    {
                        RecordTemplateTableRow newRow = new RecordTemplateTableRow()
                        {
                            RecordXPath = takes.RecordNode.XPath
                        };
                        Rows.Add(newRow);
                        row = newRow;
                    } else
                    {
                        row = Rows.First(x => x.RecordXPath == takes.RecordNode.XPath);
                    }

                    if (row.OriginalTake == null)
                    {
                        row.Populated = 1;
                        row.OriginalTake = takes;
                    }
                    else
                    {
                        row.Populated++;
                        row.OriginalTake.items.AddRange(takes.items);
                    }


                    break;
            }
        }

        public RecordTemplateTakeDescriptorDictionary ItemDescriptors { get; set; } 

        public void Analyze()
        {
            ItemDescriptors = new RecordTemplateTakeDescriptorDictionary();

            foreach (var rowPair in Rows)
            {
                for (int i = 0; i < rowPair.OriginalTake.Count; i++)
                {
                    RecordTemplateItemTake take = rowPair.OriginalTake.items[i];
                    
                    String innerText = take.SelectedNode.GetInnerText();

                    ItemDescriptors.TakeCountersBySubXPath[take.SubXPath].AddUnique(innerText);

                }
            }

            ItemDescriptors.Deploy();


            List<RecordTemplateTableRow> acceptedRows = new List<RecordTemplateTableRow>();

            foreach (var rowPair in Rows)
            {
                var row = rowPair;

                RecordTemplateCells ContextTakes = new RecordTemplateCells();
                // List<RecordTemplateItemTake> ValueTakes = new List<RecordTemplateItemTake>();

                RecordTemplateCells ValueTakes = new RecordTemplateCells();

                for (int i = 0; i < row.OriginalTake.Count; i++)
                {
                    RecordTemplateItemTake take = row.OriginalTake.items[i];
                    var descriptor = ItemDescriptors.BySubXPath.FirstOrDefault(x => x.SubXPath == take.SubXPath);
                    if (descriptor == null)
                    {
                        ValueTakes.AddCell(take);
                        row.Cells.AddCell(take);
                    }
                    else
                    {
                        if (descriptor.Category.HasFlag(NodeInTemplateRole.Static))
                        {
                            ContextTakes.AddCell(take);
                        }
                        else
                        {
                            ValueTakes.AddCell(take);
                            row.Cells.AddCell(take);
                        }
                    }
                }

                if (ContextTakes.Count > 0)
                {
                    if (ContextTakes.Count == ValueTakes.Count)
                    {
                        foreach (var take in ContextTakes.items)
                        {
                            ColumnContextTakes.Cells.AddCell(take, false);
                        }
                    }
                    else if (ContextTakes.Count == 1)
                    {
                        row.RowContextTake = ContextTakes.items.FirstOrDefault();
                    } else if (ContextTakes.Count < ValueTakes.Count)
                    {

                    } else
                    {

                    }
                }
                
                if (row.Cells.Count > 0)
                {
                    acceptedRows.Add(row);
                }
            }

            Rows = acceptedRows;
        }

        public SourceTable GetSourceTable(HtmlExtractorBase htmlExtractor)
        {

            Boolean AddHeaderRow = false;
            Boolean AddContextColumn = false;

            Int32 Height = Rows.Count;
            if (ColumnContextTakes.Cells.items.Any())
            {
                Height++;
                AddHeaderRow = true;
            }

            Int32 Width = 0;
            if (Rows.Any()) Width = Rows.Max(x => x.Cells.Count);

            if (Rows.All(x=>x.RowContextTake != null))
            {
                Width++;
                AddContextColumn = true;
            }

            SourceTable output = new SourceTable(Width, Height);
            
            Int32 ri = 0;
            if (AddHeaderRow)
            {
                for (int i = 0; i < ColumnContextTakes.Cells.Count; i++)
                {
                    //Int32 rx = i;
                    
                    
                    RecordTemplateItemTake take = (RecordTemplateItemTake)ColumnContextTakes.Cells.items[i];
                    
                    //String takeSubPath = ItemDescriptors.ValueCellSubXPath[i];
                    //Int32 rx = ItemDescriptors.CellIndex(takeSubPath, AddContextColumn);
                    Int32 rx = ItemDescriptors.CellIndex(take, AddContextColumn);
                    if (rx > -1) htmlExtractor.SetSourceTableCell(output[rx, ri], take.SelectedNode, null);
                    //if (AddContextColumn)
                    //{
                    //    rx++;
                    //}


                }
                ri++;
            }

            foreach (var row in Rows)
            {
                if (AddContextColumn)
                {
                    if (row.RowContextTake != null)
                    {
                        htmlExtractor.SetSourceTableCell(output[0, ri], row.RowContextTake.SelectedNode, null);
                    }
                }
                foreach (var take in row.Cells.items)
                {
                    Int32 rx = ItemDescriptors.CellIndex(take, AddContextColumn);
                    if (rx < output.Width)
                    {
                        htmlExtractor.SetSourceTableCell(output[rx, ri], take.SelectedNode, null);
                    }
                }

                ri++;
            }

            output = output.GetDistinctRows();

            return output;


        }

        /*
        public void Deploy(RecordTemplateItemTakeCollection takes)
        {
            
            if (!Rows.ContainsKey(takes.RecordNode.XPath))
            {
                Rows.Add(takes.RecordNode.XPath, new RecordTemplateTableRow());
            }

            RecordTemplateTableRow row = Rows[takes.RecordNode.XPath];

            if (takes.Template.Role == RecordTemplateRole.GeneralContextProvider)
            {
                GeneralContextTakes.AddRange(takes);
            } else 
            {
                RecordTemplateItemTake lastContextTake = takes.FirstOrDefault(x => x.TemplateItem.Category.HasFlag(NodeInTemplateRole.Static));

                for (int i = 0; i < takes.Count; i++)
                {
                    RecordTemplateItemTake take = takes[i];

                    if (take.TemplateItem.Category.HasFlag(NodeInTemplateRole.Dynamic))
                    {
                        lastContextTake = take;
                    }
                    if (lastContextTake != null)
                    {
                        row.ContextTakes[i] = lastContextTake;
                    }

                    row.ValueTakes[i] = take;
                }
            } 
        }*/


    }
}