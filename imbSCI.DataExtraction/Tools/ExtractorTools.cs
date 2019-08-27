using HtmlAgilityPack;
using imbSCI.Core.data;
using imbSCI.Core.data.providers;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.files;
using imbSCI.Core.math;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Extractors;
using imbSCI.DataExtraction.Extractors.Core;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.DataExtraction.Tools
{
  /// <summary>
    /// 
    /// </summary>
    public static class ExtractorTools
    {

        public static List<String> GetInnerTexts(this HtmlNode node)
        {
            var textLeafs = node.SelectTextLeafNodes();

            List<String> texts = new List<string>();
            foreach (var tn in textLeafs)
            {
                texts.Add(tn.GetInnerText());
            }
            
            return texts;
        }



        public static String GetInnerText(this HtmlNode node)
        {

            String output = node.InnerText;
            output = output.Replace(Environment.NewLine, "");
            output = output.Trim();
            return output;
        }


        private static Object _regexWords_lock = new Object();
        private static Regex _regexWords;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static Regex regexWords
        {
            get
            {
                if (_regexWords == null)
                {
                    lock (_regexWords_lock)
                    {

                        if (_regexWords == null)
                        {
                            _regexWords = new Regex(@"([a-zA-Z]+)");
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _regexWords;
            }
        }


        public static String GetInnerLetterText(this HtmlNode node, String wordSeparator=" ")
        {
            String output = node.GetInnerText();
            MatchCollection mch = regexWords.Matches(output);
            StringBuilder sb = new StringBuilder();
            Int32 c = 0;
            foreach (Match mc in mch)
            {
                sb.Append(mc.Value);
                if (c <  mch.Count-1)
                {
                    sb.Append(wordSeparator);
                }
                c++;
                
            }
            return sb.ToString();
        }


        private static Object _descriptionTagPriorityList_lock = new Object();
        private static List<String> _descriptionTagPriorityList;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static List<String> descriptionTagPriorityList
        {
            get
            {
                if (_descriptionTagPriorityList == null)
                {
                    lock (_descriptionTagPriorityList_lock)
                    {

                        if (_descriptionTagPriorityList == null)
                        {
                            _descriptionTagPriorityList = new List<String>() { "b", "i", "strong", "p","div","span" };
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _descriptionTagPriorityList;
            }
        }
        /**/

        private static Object _headingTagPriorityList_lock = new Object();
        private static List<String> _headingTagPriorityList;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static List<String> headingTagPriorityList
        {
            get
            {
                if (_headingTagPriorityList == null)
                {
                    lock (_headingTagPriorityList_lock)
                    {

                        if (_headingTagPriorityList == null)
                        {
                            _headingTagPriorityList = new List<String>() { "h1", "h2", "h3", "h4", "h5", "h6", "header", "title"};
                            
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _headingTagPriorityList;
            }
        }

        public static List<ScoredText> GetTopScoreCandidateTexts(this List<ScoredHtmlNode> candidates)
        {
            List<ScoredText> output = new List<ScoredText>();
            Double maxScore = candidates.Max(x => x.score);
            var nodeScores = candidates.Where(x => x.score == maxScore);
            foreach (var n in nodeScores)
            {
                String innerText = n.item.GetInnerText();
                output.Add(new ScoredText(innerText, n.score));
            }
            return output;
        }

        public static List<T> ScoreNodeCandidates<T>(this NodeDictionary nodes, List<String> tagNamePriorityList = null) where T : IScoredContent, new()
        {
           
            if (tagNamePriorityList == null) tagNamePriorityList = headingTagPriorityList;

            List<T> output = new List<T>();
            HtmlDocument document = null;
            foreach (var entry in nodes.items)
            {
                //if (document == null)
                //{
                //    if (entry.node != null)
                //    {
                //        document = entry.node.OwnerDocument;
                //    }
                //}
            
                Double score = 0;

                var xpath_parts = entry.XPath.SplitSmart("/");
                xpath_parts.Reverse();
                for (int i2 = 0; i2 < xpath_parts.Count; i2++)
                {
                    for (int i = 0; i < tagNamePriorityList.Count; i++)
                    {
                        var heading_tag = tagNamePriorityList[i];


                        if (xpath_parts[i2].StartsWith(heading_tag, StringComparison.InvariantCultureIgnoreCase))
                        {

                            Double tagPriorityFactor = 1.GetRatio(i + 1);
                            Double xpathLevelFactor = 1.GetRatio(i2 + 1);

                            score += tagPriorityFactor * xpathLevelFactor;
                            break;
                        }

                        
                    }

                    
                }

                if (score > 0)
                {
                    T scoreEntry = new T();
                    scoreEntry.item = entry.node;
                    scoreEntry.score = score;

                    output.Add(scoreEntry);
                }

            }
            if (output.Any())
            {
                return output.OrderByDescending(x => x.score).ToList();
            }
            return output;

        }

        public static List<T> SelectNodeCandidates<T>(this HtmlNode node, List<String> tagNamePriorityList=null, List<String> xpathToIgnore=null) where T:IScoredContent, new()
        {
            String q = "//*[not(*)]";
            LeafNodeDictionary headingLeafs = new LeafNodeDictionary(node, "//*", null, xpathToIgnore);
            
            if (tagNamePriorityList == null) tagNamePriorityList = headingTagPriorityList;

            var removed = headingLeafs.GetAndRemoveByTagNames(tagNamePriorityList, true, true);

            return headingLeafs.ScoreNodeCandidates<T>(tagNamePriorityList);
           

        }

        /// <summary>
        /// Selects a heading node
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static HtmlNode SelectHeading(this HtmlNode node, Int32 backwardLimit=3, Int32 backwardIndex = 0)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node), "Source node can't be null");
            }
            HtmlNode output = node.SelectSingleNode("//h1");
            if (output == null) output = node.SelectSingleNode("//h2");
            if (output == null) output = node.SelectSingleNode("//h3");

            if (output == null) output = node.SelectSingleNode("//*[contains(@class, 'caption')]");
            if (output == null) output = node.SelectSingleNode("//*[contains(@class, 'label')]");
            if (output == null) output = node.SelectSingleNode("//*[contains(@class, 'title')]");
            if (output == null) output = node.SelectSingleNode("//*[contains(@class, 'uppercase')]");
            if (output == null) output = node.SelectSingleNode("//h4");
            if (output == null) output = node.SelectSingleNode("//h5");
            if (output == null) output = node.SelectSingleNode("//b");

            if (output == null) output = node.SelectSingleNode("//strong");
            if (output == null) output = node.SelectSingleNode("//label");

            if (output == null)
            {
                if (backwardIndex < backwardLimit)
                {
                    output = SelectHeading(node.ParentNode, backwardLimit, backwardIndex + 1);
                }
            }

            return output;
        }

        public static Int32 GetXPathLevel(this String path, String delimiter = "/")
        {
            var parts = path.SplitSmart(delimiter);
            return parts.Count;
        }

        /// <summary>
        /// Returns common path, shared between all <c>paths</c>
        /// </summary>
        /// <param name="paths">The paths.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static String GetCommonPathRoot(this IEnumerable<String> paths, String delimiter = "/")
        {
            List<String> p = paths.ToList();

            List<List<String>> pts = new List<List<string>>();

            foreach (var pi in p)
            {
                pts.Add(pi.SplitSmart(delimiter));
            }

            Int32 p_count = pts.Count;
            Int32 p_len = pts.Min(x => x.Count);

            List<String> common = new List<String>();

            for (int i = 0; i < p_len; i++)
            {
                String ch = default(String);

                Int32 m = 1;
                for (int y = 0; y < p_count; y++)
                {
                    if (y == 0)
                    {
                        ch = pts[y][i];
                    }
                    else
                    {
                        if (ch == pts[y][i])
                        {

                            m++;
                        }
                        else
                        {
                            break;
                        }
                    }

                }
                if (m == p_count)
                {
                    common.Add(ch);
                }
                else
                {
                    break;
                }
            }

            String output = delimiter;
            foreach (var s in common)
            {
                output = output.add(s, delimiter);
            }

            return output;

        }


        /// <summary>
        /// Gets the common string root.
        /// </summary>
        /// <param name="paths">The paths.</param>
        /// <returns></returns>
        public static String GetCommonStringRoot(this IEnumerable<String> paths)
        {
            List<String> pts = paths.ToList();

            Int32 p_count = pts.Count;
            Int32 p_len = pts.Min(x => x.Length);

            List<Char> common = new List<char>();

            for (int i = 0; i < p_len; i++)
            {
                Char ch = default(Char);

                Int32 m = 1;
                for (int y = 0; y < p_count; y++)
                {
                    if (y == 0)
                    {
                        ch = pts[y][i];
                    }
                    else
                    {
                        if (ch == pts[y][i])
                        {

                            m++;
                        }
                        else
                        {
                            break;
                        }
                    }

                }
                if (m == p_count)
                {
                    common.Add(ch);
                }
                else
                {
                    break;
                }
            }


            return String.Concat(common);


        }



        /// <summary>
        /// Sets the settings from data.
        /// </summary>
        /// <param name="Extractor">The extractor.</param>
        /// <param name="settings">The settings.</param>
        public static void SetSettingsFromData(this IHtmlExtractor Extractor, reportExpandedData settings)
        {
            settings.SetSettingsFromData(Extractor);
        }

        /// <summary>
        /// Gets the settings from extractor.
        /// </summary>
        /// <param name="Extractor">The extractor.</param>
        /// <returns></returns>
        public static reportExpandedData GetSettingsFromExtractor(this IHtmlExtractor Extractor)
        {
            reportExpandedData settings = new reportExpandedData();

            settings.GetSettingsFromObjectToSet(Extractor);


            return settings;
        }



        /// <summary>
        /// Regex select ASCIILetters : ([a-zA-Z]+)
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_isASCIILetters = new Regex(@"([a-zA-Z0123456789]+)", RegexOptions.Compiled);

        /// <summary>
        /// Test if input matches ([a-zA-Z]+)
        /// </summary>
        /// <param name="input">String to test</param>
        /// <returns>IsMatch against _select_isASCIILetters</returns>
        public static Boolean isASCIILetters(this String input)
        {
            if (String.IsNullOrEmpty(input)) return false;
            return _select_isASCIILetters.IsMatch(input);
        }

        public static String GetPropertyName(String input)
        {
            input = ValueCleanUp(input, false);
            StringBuilder sb = new StringBuilder();

            var mch = _select_isASCIILetters.Matches(input);
            Boolean isFirst = true;
            foreach (Match m in mch)
            {
                if (!isFirst)
                {
                    sb.Append("_");
                }
                sb.Append(m.Value);
                isFirst = false;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Cleans string value extracted from HTML/XML.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="preserveNewLines">if set to <c>true</c> [preserve new lines].</param>
        /// <returns></returns>
        public static String ValueCleanUp(String input, Boolean preserveNewLines = true)
        {
            if (input == null)
            {
                input = "";
                return input;
            }

            input = input.Replace("&nbsp;", " ");
            input = input.Trim(new char[] { '\t', '\n', '\r' });
            input = input.Trim();

            if (preserveNewLines)
            {
                var lines = input.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                StringBuilder sb = new StringBuilder();
                foreach (String ln in lines)
                {
                    String cln = ValueCleanUp(ln, false);
                    if (cln != "")
                    {
                        //        cleanLines.Add(cln);
                        sb.AppendLine(cln);
                    }
                }

                input = sb.ToString();
                input = input.Trim(Environment.NewLine.ToCharArray());
            }
            else
            {
                input = input.Replace(Environment.NewLine, " ");
                input = input.Replace("\n", " ");
                input = input.Replace("\r", " ");
                input = input.Trim();
            }

            return input;
        }

        private static Object _HtmlExtractorProvider_lock = new Object();
        private static UniversalTypeProvider<IHtmlExtractor> _HtmlExtractorProvider;

        /// <summary>
        /// Type provider for Document ScoreModel factors
        /// </summary>
        public static UniversalTypeProvider<IHtmlExtractor> HtmlExtractorProvider
        {
            get
            {
                if (_HtmlExtractorProvider == null)
                {
                    lock (_HtmlExtractorProvider_lock)
                    {
                        if (_HtmlExtractorProvider == null)
                        {
                            _HtmlExtractorProvider = new UniversalTypeProvider<IHtmlExtractor>("imbSCI.DataExtraction.Extractors");
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _HtmlExtractorProvider;
            }
        }

        /*
        private static Object _htmlExtractors_lock = new Object();
        private static Dictionary<String, IHtmlExtractor> _htmlExtractors;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static Dictionary<String, IHtmlExtractor> htmlExtractors
        {
            get
            {
                if (_htmlExtractors == null)
                {
                    lock (_htmlExtractors_lock)
                    {
                        if (_htmlExtractors == null)
                        {
                            _htmlExtractors = new Dictionary<String, IHtmlExtractor>();
                            _htmlExtractors.Add(nameof(DLTagExtractor), new DLTagExtractor());
                            _htmlExtractors.Add(nameof(TableTagExtractor), new TableTagExtractor());
                            _htmlExtractors.Add(nameof(TableDivExtractor), new TableDivExtractor());
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _htmlExtractors;
            }
        }
        */
    }
}