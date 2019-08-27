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
public static class ScoredContentTools
    {
        public static List<ScoredContent<TItem>> GetTopNScores<TItem>(this IEnumerable<ScoredContent<TItem>> scores, Int32 limit = 1) where TItem : class
        {
            List<ScoredContent<TItem>> output = new List<ScoredContent<TItem>>();
            var ranked = scores.OrderByDescending(x => x.score);
            foreach(var r in ranked)
            {
                output.Add(r);
                if (output.Count > limit)
                {
                    break;
                }
            }
            

            return output;
        }

        public static List<ScoredContent<TItem>> GetMaxScored<TItem>(this IEnumerable<ScoredContent<TItem>> scores, Int32 limit=0) where TItem:class
        {
            List<ScoredContent<TItem>> output = new List<ScoredContent<TItem>>();
            if (scores != null)
            {
                if (scores.Any())
                {
                    Double maxScore = scores.Max(x => x.score);
                    output.AddRange(scores.Where(x => x.score == maxScore));
                }
            }

            if (limit > 0)
            {
                output = output.Take(Math.Min(output.Count, limit)).ToList();
            }


            return output;
        }
    }
}