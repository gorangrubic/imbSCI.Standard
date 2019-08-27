using HtmlAgilityPack;
using imbSCI.Core.collection;
using imbSCI.Core.data;
using imbSCI.Core.data.providers;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.files;
using imbSCI.Core.math;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using imbSCI.DataExtraction.Analyzers.Data;
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

namespace imbSCI.DataExtraction.Extractors
{
    public class ScoredContentDictionary<TItem, TRelated> 
        where TRelated:class
        where TItem:class
    {

        public Func<TItem, String> UIDFunction { get; set; } = null;

        public ScoredContentDictionary(Func<TItem, String> _UIDFunction=null)
        {
            if (_UIDFunction == null)
            {
                UIDFunction = x => x.ToString();
            } else
            {
                UIDFunction = _UIDFunction;
            }
            
        }

        public String GetUIDBlend(IEnumerable<ScoredContent<TItem>> scores, Int32 UIDBlendWidthLimit = 0, String separator = "_")
        {
            String output = "";

            foreach (var tl in scores)
            {
                output = output.add(UIDFunction(tl.item), separator);
                if (UIDBlendWidthLimit > 0)
                {
                    if (output.Length > UIDBlendWidthLimit)
                    {
                        output = output.Substring(0, UIDBlendWidthLimit);
                        break;
                    }
                }
            }

            return output;
        }

        public String GetUIDBlendForTopLocalScores(TRelated related, Int32 limit = 0, Int32 UIDBlendWidthLimit=0, String separator="_")
        {
            var topLocal = GetLocalScores(related).GetMaxScored(limit);
            return GetUIDBlend(topLocal, UIDBlendWidthLimit, separator);
        }

            public List<ScoredContent<TItem>> GetLocalScores(TRelated related, StringBuilder sb=null)
        {
            Dictionary<String, ScoredContent<TItem>> ScoreSumByUID = new Dictionary<string, ScoredContent<TItem>>();
            Dictionary<String, Int32> ScoreCountByUID = new Dictionary<string, int>();

            List<ScoredContent<TItem>> output = new List<ScoredContent<TItem>>();

            foreach (ScoredContent<TItem> scoreEntry in ScoreEntryByRelatedSource[related])
            {
                
                String uid = UIDFunction(scoreEntry.item);

                if (!ScoreSumByUID.ContainsKey(uid))
                {
                    ScoreSumByUID.Add(uid, new ScoredContent<TItem>() { score = 0, item=scoreEntry.item });
                    ScoreCountByUID.Add(uid, 0);
                }
                ScoreCountByUID[uid]++;
                ScoreSumByUID[uid].score += scoreEntry.score;
            }

            Int32 local_candidates = ScoreEntryByRelatedSource[related].Count;
            Int32 documents = RelatedSourceByUID.CountKeys(true);

            foreach (var pair in ScoreSumByUID)
            {
                var uid = pair.Key;

                var local_score = pair.Value.score.GetRatio(ScoreCountByUID[uid]);

                var global_score = ScoreEntryByUID[uid].Average(x => x.score);
                var global_documents = RelatedSourceByUID[uid].Count();

                var score = local_score.GetRatio(global_score);

                var IDF = 1.GetRatio(global_documents); //.GetRatio(documents));

                pair.Value.score = score * IDF;

                if (sb != null)
                {
                    sb.AppendLine($"{uid} : score ={pair.Value.score} (ls={local_score} , gs={global_score} , IDF={IDF}");
                }
                output.Add(new ScoredContent<TItem>() { score= pair.Value.score, item = pair.Value.item });

            }

            output = output.OrderByDescending(x => x.score).ToList();

            return output;
        }

        public void Merge(IEnumerable<ScoredContent<TItem>> scoreEntries, TRelated related=null)
        {
            foreach (var score in scoreEntries)
            {
                String uid = UIDFunction(score.item);

                if (related != null)
                {
                    if (score.score > 0)
                    {
                        if (!uid.isNullOrEmpty())
                        {
                            ScoreEntryByRelatedSource[related].Add(score);
                            RelatedSourceByUID[uid].Add(related);
                        }
                    }
                }

                ScoreEntryByUID[uid].Add(score);
            }
        }

        public ListDictionary<TRelated, ScoredContent<TItem>> ScoreEntryByRelatedSource { get; set; } = new ListDictionary<TRelated, ScoredContent<TItem>>();

        public ListDictionary<String, ScoredContent<TItem>> ScoreEntryByUID { get; set; } = new ListDictionary<String, ScoredContent<TItem>>();

        public ListDictionary<String, TRelated> RelatedSourceByUID { get; set; } = new ListDictionary<string, TRelated>();
    }
}