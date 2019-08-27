using imbSCI.Core.data;
using imbSCI.Core.extensions.text;
using imbSCI.Core.files.folders;
using imbSCI.DataComplex.tables;
using imbSCI.DataExtraction.Extractors.Task;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.DataExtraction.Analyzers.Templates;
using HtmlAgilityPack;

namespace imbSCI.DataExtraction.MetaTables
{

    public static class SourceTableExtensions
    {
        public static List<HtmlNode> GetLinkedNodes(this IEnumerable<SourceTable> source, Func<HtmlNode, Boolean> nodeEvaluation, Boolean allChildren = true)
        {
            return source.SelectMany(x => x.GetContentCells().GetLinkedNodes(nodeEvaluation, allChildren)).ToList();
        }

        public static List<HtmlNode> GetLinkedNodes(this SourceTable source, Func<HtmlNode, Boolean> nodeEvaluation, Boolean allChildren = true)
        {
            return source.GetContentCells().GetLinkedNodes(nodeEvaluation, allChildren);
        }

        public static List<HtmlNode> GetLinkedNodes(this IEnumerable<IEnumerable<SourceTableCell>> cells, Func<HtmlNode, Boolean> nodeEvaluation, Boolean allChildren = true)
        {
            List<HtmlNode> output = new List<HtmlNode>();

            foreach (var row in cells)
            {
                output.AddRange(row.GetLinkedNodes(nodeEvaluation, allChildren));
            }

            return output;
        }

        public static List<HtmlNode> GetLinkedNodes(this IEnumerable<SourceTableCell> cells, Func<HtmlNode, Boolean> nodeEvaluation, Boolean allChildren=true)
        {
            List<HtmlNode> output = new List<HtmlNode>();

            foreach(SourceTableCell cell in cells)
            {
                if (cell != null)
                {
                    if (cell.SourceNode != null)
                    {
                        List<HtmlNode> nodes = new List<HtmlNode>();
                        if (allChildren)
                        {
                            nodes.AddRange(cell.SourceNode.DescendantNodesAndSelf());
                        } else
                        {
                            nodes.Add(cell.SourceNode);
                        }

                        output.AddRange(nodes.Where(x => nodeEvaluation(x)));

                    }
                }
            }

            return output;
        }

        public static void Publish(this SourceTable source, folderNode outputFolder, ExtractionResultPublishFlags flags, String filename_prefix = "", aceAuthorNotation notation = null)
        {
            if (source == null) return;

            var sourcep = outputFolder.pathFor(filename_prefix + "_source.xml", imbSCI.Data.enums.getWritableFileMode.autoRenameThis, "Exported source table");

            if (flags.HasFlag(ExtractionResultPublishFlags.sourceTableSerialization))
            {
                source.Save(sourcep);
            }

            String fl = Path.GetFileNameWithoutExtension(sourcep);

            if (flags.HasFlag(ExtractionResultPublishFlags.sourceTableExcel))
            {
                source.GetDataTable().GetReportAndSave(outputFolder, notation, fl);
            }

            source.ExpandedData.SaveObjectPairs(filename_prefix + "_data", outputFolder);
        }

        public static List<List<SourceTableCell>> TakeDistinct(this List<List<SourceTableCell>> all)
        {
            Dictionary<String, List<SourceTableCell>> registry = new Dictionary<string, List<SourceTableCell>>();

            foreach (List<SourceTableCell> e in all)
            {

                String sg = e.GetSignature();
                if (!registry.ContainsKey(sg))
                {
                    registry.Add(sg, e);
                }
                else
                {
                    // dismiss
                }
            }

            return registry.Values.ToList();
        }

        public static String GetSignature(this List<SourceTableCell> e)
        {
            StringBuilder sb = new StringBuilder();
            foreach ( var ec in e)
            {
                sb.Append(ec.Value.toStringSafe()); 
                
            }
            return sb.ToString();
        }

        public static SourceTable GetDistinctRows(this SourceTable source)
        {
            var all = source.GetContentCells(false);
            all = TakeDistinct(all);

            if (!all.Any()) return null;
            Int32 Width = all.Max(x => x.Count);

            Int32 Height = all.Count;

            var output = new SourceTable(Width, Height);

            

            for (int i = 0; i < output.Height; i++)
            {
                var row = all[i];

                for (int j = 0; j < output.Width; j++)
                {
                    if (j < row.Count())
                    {
                        output.SetCell(j, i, row[j]);
                        //   output[j, i].Value = row[j];
                    }

                }
            }

            return output;

        }

        public static List<List<String>> TakeDistinct(this List<List<String>> all)
        {
            Dictionary<String, List<String>> registry = new Dictionary<string, List<string>>();

            foreach (List<String> e in all)
            {
                String sg = String.Concat(e);
                if (!registry.ContainsKey(sg))
                {
                    registry.Add(sg, e);
                }
                else
                {
                    // dismiss
                }
            }

            return registry.Values.ToList();
        }

        //public static SourceTable Merge(this IEnumerable<SourceTable> SourceTables, Boolean asColumns, Boolean OnlyDistinct)
        //{
        //    List<List<List<SourceTableCell>>> sourceSets = SourceTables.Select(x => x.GetContentCells(asColumns)).ToList();
        //    return Merge(sourceSets, asColumns, OnlyDistinct);
        //}

        public static SourceTable Merge(this IEnumerable<SourceTable> SourceTables, Boolean asColumns, Boolean OnlyDistinct)
        {
            List<List<List<SourceTableCell>>> sourceSets = SourceTables.Select(x => x.GetContentCells(false)).ToList();
            return Merge(sourceSets, asColumns, OnlyDistinct);
        }

        public static SourceTable Merge(this IEnumerable<List<List<SourceTableCell>>> contentSets, Boolean asColumns, Boolean OnlyDistinct)
        {

            var all = new List<List<SourceTableCell>>();
            foreach (var cset in contentSets)
            {
                all.AddRange(cset);
            }

            if (OnlyDistinct) all = TakeDistinct(all);
            if (!all.Any()) return null;

            Int32 Width = all.Max(x => x.Count);


            SourceTable output = null;

            if (asColumns)
            {
                output = new SourceTable(all.Count, Width);

                for (int i = 0; i < output.Width; i++)
                {
                    var column = all[i];

                    for (int j = 0; j < output.Height; j++)
                    {
                        if (j < column.Count)
                        {
                            output.SetCell(i, j, column[j]);
                            //output[i, j].Value = column[j];
                        }

                    }

                }

            }
            else
            {
                output = new SourceTable(Width, all.Count);

                for (int i = 0; i < output.Height; i++)
                {
                    var row = all[i];

                    for (int j = 0; j < output.Width; j++)
                    {
                        if (j < row.Count())
                        {
                            output.SetCell(i, j, row[j]);
                         //   output[j, i].Value = row[j];
                        }

                    }
                }
            }

            return output;

        }

        public static SourceTable Merge(this IEnumerable<List<List<String>>> contentSets, Boolean asColumns, Boolean OnlyDistinct)
        {

            var all = new List<List<String>>();
            foreach (var cset in contentSets)
            {
                all.AddRange(cset);
            }

            if (OnlyDistinct) all = TakeDistinct(all);

            Int32 Width = all.Max(x => x.Count);


            SourceTable output = null;

            if (asColumns)
            {
                output = new SourceTable(all.Count, Width);

                for (int i = 0; i < output.Width; i++)
                {
                    var column = all[i];

                    for (int j = 0; j < output.Height; j++)
                    {
                        if (j < column.Count)
                        {
                            output[i, j].Value = column[j];
                        }

                    }

                }

            }
            else
            {
                output = new SourceTable(Width, all.Count);

                for (int i = 0; i < output.Height; i++)
                {
                    var row = all[i];

                    for (int j = 0; j < output.Width; j++)
                    {
                        if (j < row.Count())
                        {
                            output[j, i].Value = row[j];
                        }

                    }
                }
            }

            return output;

        }
    }
}