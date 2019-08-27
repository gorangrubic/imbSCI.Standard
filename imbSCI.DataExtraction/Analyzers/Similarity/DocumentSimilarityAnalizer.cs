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
using imbSCI.Core.extensions.text;
using imbSCI.Core.reporting.render;
using System.Threading.Tasks;
using imbSCI.Core.extensions.data;
using imbSCI.Core.math.range.frequency;

namespace imbSCI.DataExtraction.Analyzers.Similarity
{
    /// <summary>
    /// Performs similarity computation for collection of HTML documents (or nodes)
    /// </summary>
    public class DocumentSimilarityAnalizer
    {
        protected DocumentSimilaritySettings settings { get; set; }

        protected setAnalysisTools<LeafNodeDictionaryEntryNGram> ContentSimilarity { get; set; } = new setAnalysisTools<LeafNodeDictionaryEntryNGram>();

        protected setAnalysisTools<LeafNodeDictionaryEntryNGram> StructureSimilarity { get; set; } = new setAnalysisTools<LeafNodeDictionaryEntryNGram>();

        /// <summary>
        /// Prepares the analyser
        /// </summary>
        /// <param name="_settings">The settings.</param>
        public DocumentSimilarityAnalizer(DocumentSimilaritySettings _settings)
        {
            settings = _settings;

            Func<LeafNodeDictionaryEntryNGram, LeafNodeDictionaryEntryNGram, bool> contentSimilarityFunction = (x, y) => {
                if (x.Count != y.Count) return false;
                for (int i = 0; i < Math.Min(x.Count,y.Count); i++)
                {
                    if (!x[i].ContentHash.Equals(y[i].ContentHash))
                    {
                        return false;
                    }
                }
                return true;
            };

            Func<LeafNodeDictionaryEntryNGram, LeafNodeDictionaryEntryNGram, bool> structureSimilarityFunction = (x, y) => {
                if (x.Count != y.Count) return false;
                for (int i = 0; i < Math.Min(x.Count,y.Count); i++)
                {
                    if (!x[i].XPath.Equals(y[i].XPath))
                    {
                        return false;
                    }
                }
                return true;
            };

            ContentSimilarity = new setAnalysisTools<LeafNodeDictionaryEntryNGram>()
            {
                CustomIsEqual = contentSimilarityFunction
            };

            StructureSimilarity = new setAnalysisTools<LeafNodeDictionaryEntryNGram>()
            {
                CustomIsEqual = structureSimilarityFunction
            };
        }

        /// <summary>
        /// Builds <see cref="LeafNodeDictionary"/> and <see cref="LeafNodeDictionaryEntryNGram"/>s for each document, to allow performance optimization 
        /// </summary>
        /// <param name="documents">The documents.</param>
        /// <param name="leafSelectXPath">The leaf select x path, leave blank to use from settings, <see cref="DocumentSimilaritySettings.XPathToSelectLeafs"/></param>
        /// <param name="tagsToIgnore">The tags to ignore, leave unspecified to use from settings, <see cref="DocumentSimilaritySettings.TagsToIgnore"/>.</param>
        /// <returns></returns>
        public DocumentSimilarityResult Prepare(IEnumerable<HtmlNode> documents, String leafSelectXPath="", List<String> tagsToIgnore=null)
        {
            leafSelectXPath = leafSelectXPath.or(settings.XPathToSelectLeafs, LeafNodeDictionary.DefaultNodeSelectionXPath);
            tagsToIgnore = tagsToIgnore.or(settings.TagsToIgnore, LeafNodeDictionary.DefaultTagsToIgnore);

            DocumentSimilarityResult result = new DocumentSimilarityResult();

            frequencyCounter<String> xpathCounter = new frequencyCounter<string>();

            Dictionary<HtmlNode, LeafNodeDictionary> leafDictionary = new Dictionary<HtmlNode, LeafNodeDictionary>();


            foreach (HtmlNode documentA in documents)
            {
                LeafNodeDictionary leafNodeDictionaryA = new LeafNodeDictionary(documentA, leafSelectXPath, tagsToIgnore);
                if (leafNodeDictionaryA.items.Count < 5)
                {

                }
                foreach (var entry in leafNodeDictionaryA.items)
                {
                    xpathCounter.Count(entry.XPath);
                }
                leafDictionary.Add(documentA, leafNodeDictionaryA);
            }

            var commonXPaths = xpathCounter.GetItemsWithTopFrequency();

            foreach (var pair in leafDictionary)
            {
                pair.Value.RemoveEntriesByXPath(commonXPaths);
            }

            foreach (HtmlNode documentA in documents)
            {
                try
                {
                    LeafNodeDictionary leafNodeDictionaryA = leafDictionary[documentA];

                    List<LeafNodeDictionaryEntryNGram> nGrams_A = setAnalysisTools<LeafNodeDictionaryEntry>.getNGramSet<LeafNodeDictionaryEntryNGram>(leafNodeDictionaryA.items, settings.nGramWidth, settings.nGramMode);

                    result.DocumentsByLeafDictionary.Add(leafNodeDictionaryA, documentA);
                    result.DocumentsByNGrams.Add(nGrams_A, documentA);
                    result.LeafDictionaryByDocuments.Add(documentA, leafNodeDictionaryA);
                    result.NGramsByDocuments.Add(documentA, nGrams_A);
                } catch (Exception ex)
                {
                    result.DocumentsWithExceptions.Add(documentA, ex);
                }
            }
            return result;
        }

        /// <summary>
        /// Computes the similarity between two items. If you have to compare more than two documents, use <see cref="Prepare(IEnumerable{HtmlNode}, string, List{string})"/> and <see cref="ComputeSimilarity(HtmlNode, HtmlNode, DocumentSimilarityResult)"/> methods for greater performances
        /// </summary>
        /// <param name="documentA">The document a.</param>
        /// <param name="documentB">The document b.</param>
        /// <returns></returns>
        public DocumentSimilarityResultPair ComputeSimilarity(HtmlNode documentA, HtmlNode documentB)
        {
            LeafNodeDictionary leafNodeDictionaryA = new LeafNodeDictionary(documentA);
            LeafNodeDictionary leafNodeDictionaryB = new LeafNodeDictionary(documentB);

            List<LeafNodeDictionaryEntryNGram> nGrams_A = setAnalysisTools<LeafNodeDictionaryEntry>.getNGramSet<LeafNodeDictionaryEntryNGram>(leafNodeDictionaryA.items, settings.nGramWidth, settings.nGramMode);
            List<LeafNodeDictionaryEntryNGram> nGrams_B = setAnalysisTools<LeafNodeDictionaryEntry>.getNGramSet<LeafNodeDictionaryEntryNGram>(leafNodeDictionaryB.items, settings.nGramWidth, settings.nGramMode);

            DocumentSimilarityResultPair output = new DocumentSimilarityResultPair
            {
                itemA = documentA,
                itemB = documentB
            };

            output.ContentSimilarity = ContentSimilarity.GetSimilarity(nGrams_A, nGrams_B, settings.computationMethod);
            output.StructureSimilarity = ContentSimilarity.GetSimilarity(nGrams_A, nGrams_B, settings.computationMethod);

            return output;
        }

        /// <summary>
        /// Computes similarity for two documents, that are part of result's inner collections
        /// </summary>
        /// <param name="documentA">The document a.</param>
        /// <param name="documentB">The document b.</param>
        /// <param name="result">The result object, previously created with <see cref="Prepare(IEnumerable{HtmlNode}, string, List{string})"/></param>
        /// <returns>Result for these two documents</returns>
        public DocumentSimilarityResultPair ComputeSimilarity(HtmlNode documentA, HtmlNode documentB, DocumentSimilarityResult result)
        {
            List<LeafNodeDictionaryEntryNGram> nGrams_A = result.NGramsByDocuments[documentA]; //setAnalysisTools<LeafNodeDictionaryEntry>.getNGramSet<LeafNodeDictionaryEntryNGram>(leafNodeDictionaryA.items, settings.nGramWidth, settings.nGramMode);
            List<LeafNodeDictionaryEntryNGram> nGrams_B = result.NGramsByDocuments[documentB]; //setAnalysisTools<LeafNodeDictionaryEntry>.getNGramSet<LeafNodeDictionaryEntryNGram>(leafNodeDictionaryB.items, settings.nGramWidth, settings.nGramMode);

            var score_StructureSimilarity = StructureSimilarity.GetSimilarity(nGrams_A, nGrams_B, settings.computationMethod);
            var score_ContentSimilarity = ContentSimilarity.GetSimilarity(nGrams_A, nGrams_B, settings.computationMethod);

            DocumentSimilarityResultPair output = new DocumentSimilarityResultPair
            {
                itemA = documentA,
                itemB = documentB,
                StructureSimilarity = score_StructureSimilarity,
                ContentSimilarity = score_ContentSimilarity
                
            };



            return output;
        }

        protected  void ComputeSimilarity(ComputeSimilarityTask task)
        {
             var score_StructureSimilarity = StructureSimilarity.GetSimilarity(task.nGrams_A, task.nGrams_B, settings.computationMethod);
            var score_ContentSimilarity = ContentSimilarity.GetSimilarity(task.nGrams_A, task.nGrams_B, settings.computationMethod);

            DocumentSimilarityResultPair output = new DocumentSimilarityResultPair
            {
                itemA = task.documentA,
                itemB = task.documentB,
                StructureSimilarity = score_StructureSimilarity,
                ContentSimilarity = score_ContentSimilarity
                
            };


            task.output = output;

        }

        /// <summary>
        /// Computes the similarity for the result object
        /// </summary>
        /// <param name="result">The result object, previously created with <see cref="Prepare(IEnumerable{HtmlNode}, string, List{string})" /></param>
        /// <param name="output">The output.</param>
        /// <param name="documents">Optional: select subset of documents to be analysed. These must be within <see cref="result" /> inner collections</param>
        /// <returns>
        /// The same result object specified in parameters
        /// </returns>
        public DocumentSimilarityResult ComputeSimilarity(DocumentSimilarityResult result, ITextRender output, List<HtmlNode> documents=null)
        {
            if (documents.isNullOrEmpty())
            {
                documents = result.LeafDictionaryByDocuments.Keys.ToList();
            }

            List<ComputeSimilarityTask> tasks = new List<ComputeSimilarityTask>();

            for (int i = 0; i < documents.Count-1; i++)
            {
                for (int y = i+1; y < documents.Count; y++)
                {
                    ComputeSimilarityTask task = new ComputeSimilarityTask()
                    {
                        documentA = documents[i],
                        documentB = documents[y],
                        nGrams_A = result.NGramsByDocuments[documents[i]],
                        nGrams_B = result.NGramsByDocuments[documents[y]]
                    };
                    tasks.Add(task);

                    //var documentA = ;
                    //var documentB = documents[y];

                    //var ABResult = ComputeSimilarity(documentA, documentB, result);
                    //result.AddResult(ABResult);
                }
            }

            var task_chunks = tasks.SplitBySize((tasks.Count / 5));
            foreach (var task_chunk in task_chunks)
            {
                output.AppendLine("Executing similarity computation task chunk [size:" + task_chunk.Count + "] " + task_chunks.IndexOf(task_chunk) + " of " + task_chunks.Count);

                Parallel.ForEach<ComputeSimilarityTask>(task_chunk, x =>
                {
                    ComputeSimilarity(x);
                }
                );

                foreach (var task in task_chunk)
                {
                    if (task.output != null)
                    {
                        result.AddResult(task.output);
                    }
                }
            }

           

            

            return result;
        }

    }
}