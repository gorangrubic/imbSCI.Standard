using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
// imbSCI.DataExtraction.Analyzers.Similarity.similarity;
using imbSCI.DataExtraction.Analyzers.Similarity.similarity;
using imbSCI.Core.math.range;
using imbSCI.Core.math.range.finder;
using HtmlAgilityPack;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.Data;
using imbSCI.Core.math.range.matrix;
using imbSCI.DataExtraction.Tools;
using imbSCI.Core.files.folders;
using System.Data;
using imbSCI.Core.extensions.table;
using imbSCI.DataComplex.tables;
using imbSCI.Core.reporting.render.builders;
using System.IO;
using imbSCI.Core.reporting.render;
using imbSCI.Graph.Graphics.HeatMap;

namespace imbSCI.DataExtraction.Analyzers.Similarity
{
    public class DocumentSimilarityResult
    {
        public DocumentSimilarityResult()
        {

        }

        public DataTable PublishDataTable(Dictionary<HtmlNode, HtmlSourceAndUrl> documentNodeDictionary, DocumentSimilarityScoreEnum scoreSource, List<HtmlNode> reportOn=null)
        {
            if (reportOn == null) reportOn = GetDocuments();


            String tablename = scoreSource.ToString();


            Func<DocumentSimilarityResultPair, Double> scoreFunction = scoreSource.GetSelector();

            

            DataTable table = new DataTable(tablename);

            var DocumentByLabel = GetLabeledDocuments();
            var LabelByDocument = GetLabelsByDocument();

            List<DocumentSimilarityResultPair> selected = new List<DocumentSimilarityResultPair>();

            Dictionary<HtmlNode, Dictionary<HtmlNode, DocumentSimilarityResultPair>> matrix = new Dictionary<HtmlNode, Dictionary<HtmlNode, DocumentSimilarityResultPair>>();

            foreach (HtmlNode node in reportOn)
            {
                var results = GetResultsFor(node);
                matrix.Add(node, results);
            }

            Dictionary<String, DataColumn> ColumnsByLabel = new Dictionary<string, DataColumn>();

            DataColumn labelColumn = table.Columns.Add("Label");
            
            foreach (var pair in matrix)
            {
                var cn = table.Columns.Add(LabelByDocument[pair.Key], typeof(Double));

                var source = documentNodeDictionary[pair.Key];

                cn.SetFormat("F3");
                cn.SetDesc("Similarity score with " + source.filepath);
                ColumnsByLabel.Add(LabelByDocument[pair.Key], cn);
            }

            DataColumn UrlColumn = table.Columns.Add("Url");
            DataColumn sourceColumn = table.Columns.Add("Filepath");
            

            foreach (var pair in matrix)
            {
                DataRow dr = table.NewRow();

                var source = documentNodeDictionary[pair.Key];

                dr[labelColumn] = LabelByDocument[pair.Key];
                dr[sourceColumn] = source.filepath;
                dr[UrlColumn] = source.url;

                foreach (var subPair in pair.Value)
                {
                    if (reportOn.Contains(subPair.Key))
                    {
                        var dc = ColumnsByLabel[LabelByDocument[subPair.Key]];

                        dr[dc] = scoreFunction(subPair.Value);
                    }
                }

                table.Rows.Add(dr);
            }

            return table;

        }

        /// <summary>
        /// Publishes the data set.
        /// </summary>
        /// <param name="documentNodeDictionary">The document node dictionary.</param>
        /// <param name="reportOn">The report on.</param>
        /// <returns></returns>
        public DataSet PublishDataSet(Dictionary<HtmlNode, HtmlSourceAndUrl> documentNodeDictionary, List<HtmlNode> reportOn=null)
        {
            if (reportOn == null) reportOn = GetDocuments();

            DataSet output = new DataSet("SimilarityResults");

            var table = PublishDataTable(documentNodeDictionary, DocumentSimilarityScoreEnum.content, reportOn); // nameof(DocumentSimilarityResultPair.ContentSimilarity), x => x.ContentSimilarity, reportOn);


            output.Tables.Add(table);
            
            table = PublishDataTable(documentNodeDictionary, DocumentSimilarityScoreEnum.combined, reportOn); // PublishDataTable(documentNodeDictionary, nameof(DocumentSimilarityResultPair.OverallSimilarity), x => x.OverallSimilarity, reportOn);
            output.Tables.Add(table);

            table = PublishDataTable(documentNodeDictionary, DocumentSimilarityScoreEnum.structure, reportOn); // nameof(DocumentSimilarityResultPair.StructureSimilarity), x => x.StructureSimilarity, reportOn);
            output.Tables.Add(table);

            table = PublishDataTable(documentNodeDictionary, DocumentSimilarityScoreEnum.higherComponent, reportOn); // nameof(DocumentSimilarityResultPair.StructureSimilarity), x => x.StructureSimilarity, reportOn);
            output.Tables.Add(table);

            return output;
        }

        public void Report(rangeFinder ranger, ITextRender output, String rangeName, String prefix)
        {
            output.AppendLine("Range [" + rangeName + "]");
            output.nextTabLevel();
            foreach (var pair in ranger.GetDictionary(prefix))
            {
                output.AppendPair(pair.Key, pair.Value.ToString("F3"));
            }
            output.prevTabLevel();
        }


        public void Publish(Dictionary<HtmlNode, HtmlSourceAndUrl> documentNodeDictionary, folderNode folderWithResults, List<HtmlNode> reportOn=null)
        {
            if (reportOn == null) reportOn = GetDocuments();

            folderWithResults.generateReadmeFiles(null);

            builderForText reporter = new builderForText();
            foreach (var item in items)
            {
                if (item.IsRelatedTo(reportOn))
                {
                    reporter.AppendLine("Pair [" + items.IndexOf(item) + "]");
                    item.Publish(documentNodeDictionary, folderWithResults, reporter);
                }
            }

            Report(StructureSimilarityRange, reporter, "Structure similarity", "SS");
            Report(ContentSimilarityRange,reporter, "Content similarity", "CS");

            String reporterPath = folderWithResults.pathFor("report.txt");
            File.WriteAllText(reporterPath, reporter.GetContent());

            

            DataSet output = PublishDataSet(documentNodeDictionary, reportOn);

            HeatMapRender hmRender = new HeatMapRender();

            foreach (DataTable table in output.Tables)
            {
                HeatMapModel heatMap = new HeatMapModel(table);


                var heatMapSVG = hmRender.Render(heatMap, folderWithResults.pathFor(table.TableName + ".svg", imbSCI.Data.enums.getWritableFileMode.overwrite, "Headmap render for " + table.TableName));
                heatMapSVG.SaveJPEG(folderWithResults.pathFor(table.TableName + ".jpg", imbSCI.Data.enums.getWritableFileMode.overwrite, "Headmap render for " + table.TableName));
            }

            output.GetReportAndSave(folderWithResults, null, "");
            
        }

        public Dictionary<String, HeatMapModel> GetHeatMaps()
        {
            Dictionary<String, HeatMapModel> output = new Dictionary<string, HeatMapModel>();
            Dictionary<String, HtmlNode> labeledDocuments = GetLabeledDocuments();
            List<String> labels = labeledDocuments.Keys.ToList();

            output.Add(nameof(DocumentSimilarityResultPair.ContentSimilarity), new HeatMapModel(labels));
            output.Add(nameof(DocumentSimilarityResultPair.StructureSimilarity), new HeatMapModel(labels));
            output.Add(nameof(DocumentSimilarityResultPair.OverallSimilarity), new HeatMapModel(labels));

            var matrix = GetSquareMatrix();

            output[nameof(DocumentSimilarityResultPair.ContentSimilarity)].SetFrom(GetSquareScoreMatrix(x => x.ContentSimilarity, matrix));
            output[nameof(DocumentSimilarityResultPair.StructureSimilarity)].SetFrom(GetSquareScoreMatrix(x => x.StructureSimilarity, matrix));
            output[nameof(DocumentSimilarityResultPair.OverallSimilarity)].SetFrom(GetSquareScoreMatrix(x => x.OverallSimilarity, matrix));

            return output;

        }

        public List<List<Double>> GetSquareScoreMatrix(Func<DocumentSimilarityResultPair, Double> scoreFunction, Dictionary<HtmlNode, Dictionary<HtmlNode, DocumentSimilarityResultPair>> matrix = null)
        {
            var documents = GetDocuments();
            var labelsByDocuments = GetLabelsByDocument();
            
            List<List<Double>> output = new List<List<double>>();
            var keys = matrix.Keys.ToList();

            for (int i = 0; i < documents.Count; i++)
            {
                var newLine = new List<Double>();

                var results = GetResultsFor(documents[i]);

                for (int y = 0; y < documents.Count; y++)
                {
                    var documentB = documents[y];

                    Double score = 0;
                    if (y == i)
                    {
                        score = 0;
                    } else
                    {
                        score = scoreFunction(results[documentB]);
                    }

                    newLine.Add(score);
                    
                }
                output.Add(newLine);
            }

            //foreach (var column in matrix)
            //{
            //    Int32 i = keys.IndexOf(column.Key);
            //    foreach (var row in matrix)
            //    {
            //        Int32 y = keys.IndexOf(row.Key);
            //        var pair = matrix[column.Key][row.Key];
            //        output[i][y] = scoreFunction(pair);
            //    }
            //}


            return output;
        }

        public Dictionary<HtmlNode, Dictionary<HtmlNode, DocumentSimilarityResultPair>> GetSquareMatrix()
        {
            Dictionary<HtmlNode, Dictionary<HtmlNode, DocumentSimilarityResultPair>> output = new Dictionary<HtmlNode, Dictionary<HtmlNode, DocumentSimilarityResultPair>>();

            foreach (var pair in items)
            {
                if (!output.ContainsKey(pair.itemA)) output.Add(pair.itemA, new Dictionary<HtmlNode, DocumentSimilarityResultPair>());
                if (!output.ContainsKey(pair.itemB)) output.Add(pair.itemB, new Dictionary<HtmlNode, DocumentSimilarityResultPair>());

                if (!output[pair.itemA].ContainsKey(pair.itemB)) output[pair.itemA].Add(pair.itemB, pair);
                if (!output[pair.itemB].ContainsKey(pair.itemA)) output[pair.itemB].Add(pair.itemA, pair);

            }
            return output;
        }

        public Dictionary<HtmlNode, DocumentSimilarityResultPair> GetResultsFor(HtmlNode document)
        {
            Dictionary<HtmlNode, DocumentSimilarityResultPair> output = new Dictionary<HtmlNode, DocumentSimilarityResultPair>();
            foreach (var pair in items)
            {
                if (pair.itemA == document)
                {
                    output.Add(pair.itemB, pair);
                } else if (pair.itemB == document)
                {
                    output.Add(pair.itemA, pair);
                }
            }
            return output;
        }

         public Dictionary<HtmlNode, String> GetLabelsByDocument(String labelPrefix = "D", String countFormat = "D3")
        {
            Dictionary<HtmlNode, String> output = new Dictionary<HtmlNode, String>();
            Int32 c = 1;
            foreach (HtmlNode node in LeafDictionaryByDocuments.Keys)
            {
                output.Add(node,labelPrefix + c.ToString(countFormat));
                c++;
            }
            return output;
        }


        public Dictionary<String, HtmlNode> GetLabeledDocuments(String labelPrefix = "D", String countFormat = "D3")
        {
            Dictionary<String, HtmlNode> output = new Dictionary<string, HtmlNode>();
            Int32 c = 1;
            foreach (HtmlNode node in LeafDictionaryByDocuments.Keys)
            {
                output.Add(labelPrefix + c.ToString(countFormat), node);
                c++;
            }
            return output;
        }


        public Dictionary<HtmlNode, Exception> DocumentsWithExceptions { get; protected set; } = new Dictionary<HtmlNode, Exception>();

        public Dictionary<HtmlNode, LeafNodeDictionary> LeafDictionaryByDocuments { get; protected set; } = new Dictionary<HtmlNode, LeafNodeDictionary>();
        public Dictionary<LeafNodeDictionary, HtmlNode> DocumentsByLeafDictionary { get; protected set; } = new Dictionary<LeafNodeDictionary, HtmlNode>();

        public Dictionary<HtmlNode, List<LeafNodeDictionaryEntryNGram>> NGramsByDocuments { get; protected set; } = new Dictionary<HtmlNode, List<LeafNodeDictionaryEntryNGram>>();
        public Dictionary<List<LeafNodeDictionaryEntryNGram>, HtmlNode> DocumentsByNGrams { get; protected set; } = new Dictionary<List<LeafNodeDictionaryEntryNGram>, HtmlNode>();

        public void AddResult(DocumentSimilarityResultPair pair)
        {
            ContentSimilarityRange.Learn(pair.ContentSimilarity);
            StructureSimilarityRange.Learn(pair.StructureSimilarity);
            items.Add(pair);
        }

        public rangeFinder ContentSimilarityRange { get; protected set; } = new rangeFinder();
        public rangeFinder StructureSimilarityRange { get; protected set; } = new rangeFinder();

        public List<HtmlNode> GetDocuments()
        {
            return LeafDictionaryByDocuments.Keys.ToList();
        }

        public List<DocumentSimilarityResultPair> GetAllResults()
        {
            return items.ToList();
        }

        protected List<DocumentSimilarityResultPair> items { get; set; } = new List<DocumentSimilarityResultPair>();
    }
}