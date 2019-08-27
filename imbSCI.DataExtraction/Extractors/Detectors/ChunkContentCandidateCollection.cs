using HtmlAgilityPack;
using imbSCI.Core.data.providers;
using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.math.range.frequency;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Analyzers.Structure;
using imbSCI.Graph.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Extractors.Detectors
{
    [Serializable]
    public class ChunkContentCandidateCollection:List<ChunkContentCandidate>
    {
        public void Report(builderForText reporter, String title)
        {
            if (!this.Any()) return;

            reporter.AppendLine(title);
            reporter.nextTabLevel();

            for (int i = 0; i < this.Count; i++)
            {
                this[i].Report(reporter, i);
            }
            reporter.prevTabLevel();
        }
        /// <summary>
        /// Computes scores and sorts the collection using <see cref="ChunkDetectorTools.Compare(ChunkContentCandidate, ChunkContentCandidate)"/> method
        /// </summary>
        public void ScoreAndSort()
        {
            foreach (ChunkContentCandidate candidate in this)
            {
                NodeGraphScoreModel scoreModel = new NodeGraphScoreModel(candidate.Node);
                candidate.ScoreModel = scoreModel;

                candidate.Score = scoreModel.GetScore();

            }

            Sort(ChunkDetectorTools.Compare);
            Reverse();
        }

    }
}