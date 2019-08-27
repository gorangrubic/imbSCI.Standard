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
    /// Set of <see cref="SpanTransformationRule"/>s
    /// </summary>
    public class SpanTransformationRuleSet
    {


        /// <summary>
        /// Creates new <see cref="SpanTransformationRule"/>
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public SpanTransformationRule Add(String name)
        {
            var mergeAsPropertyHeader = new SpanTransformationRule()
            {
                name = name
            };

            return mergeAsPropertyHeader;
        }

        public SpanTransformationRuleSet()
        {

        }

        /// <summary>
        /// Rules confained in this set
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<SpanTransformationRule> items { get; set; } = new List<SpanTransformationRule>();

    }
}