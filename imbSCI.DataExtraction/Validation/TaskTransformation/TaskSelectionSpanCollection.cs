using imbSCI.Core.data;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.files.folders;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.math.range.finder;
using imbSCI.Core.reporting.render;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Core.reporting.zone;
using imbSCI.Data;
using imbSCI.Data.primitives;
using imbSCI.DataExtraction.Extractors.Core;
using imbSCI.DataExtraction.Extractors.Task;
using imbSCI.DataExtraction.MetaTables;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData.Analysis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.SourceData
{
    /// <summary>
    /// Collection of <see cref="TaskSelectionSpan"/>s
    /// </summary>
    public class TaskSelectionSpanCollection
    {

        public void Report(folderNode folder, ITextRender output)
        {
            if (output == null) return;

            this.ReportBase(output, false, "TaskSelectionSpanCollection");

            foreach (var item in items)
            {
                output.AppendLine(item.GetSignature());

                item.Report(folder, output);
            }

        }


        public TaskSelectionSpanCollection()
        {

        }

        public void Close()
        {
            if (currentSpan != null)
            {
                if (currentSpan.IsValid())
                {
                    items.Add(currentSpan);
                }
            }

            currentSpan = new TaskSelectionSpan();
        }
        public TaskSelectionSpan currentSpan { get; set; } = new TaskSelectionSpan();

        public List<TaskSelectionSpan> items { get; set; } = new List<TaskSelectionSpan>();

        public SpanTransformationRule Add(SourceTableAggregation input, SpanTransformationRuleSet ruleSet)
        {
            SpanTransformationRule matchedRule = null;

            foreach (SpanTransformationRule rule in ruleSet.items) {

                if (rule.IsMatch(currentSpan.items, input))
                {
                    matchedRule = rule;

                }
            }

            if (matchedRule == null)
            {
                Close();
            } else
            {
                currentSpan.items.Add(input);
                currentSpan.MatchedRule = matchedRule;
            }

            return matchedRule;
        }
    }
}