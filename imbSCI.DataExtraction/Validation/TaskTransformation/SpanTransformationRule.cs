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
using imbSCI.Data.interfaces;
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
    /// <see cref="TaskSelectionSpan"/> creation rule
    /// </summary>
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithName" />
    public class SpanTransformationRule:IObjectWithName
    {
        public String name { get; set; } = "";

        public SpanTransformationRule()
        {

        }
 
        public List<Func<SourceTableAggregation, SourceTableAggregation, Boolean>> ExtraCriteria { get; protected set; } = new List<Func<SourceTableAggregation, SourceTableAggregation, bool>>();

        public List<SourceTableCase> Required { get; protected set; } = new List<SourceTableCase>();

        public Boolean IsMatch(IEnumerable<SourceTableAggregation> items, SourceTableAggregation input)
        {
            List<SourceTableAggregation> allItems = items.ToList();
            allItems.Add(input);

            var existing = allItems.Select(x => x.Features).ToList();
            //existing.Add(input.Features);

            var common = existing.GetCrossSection<SourceTableCase>();

            if (!common.ContainsAll(Required)) return false;

            SourceTableAggregation last = null;
            foreach (SourceTableAggregation item in allItems)
            {
                if (last == null)
                {
                    last = item;
                } else
                {
                    foreach (Func<SourceTableAggregation, SourceTableAggregation, Boolean> criterion in ExtraCriteria)
                    {
                        if (!criterion(last, item)) return false;
                    }
                }
            }
            return true;
        }
    }
}