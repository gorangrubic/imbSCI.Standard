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
[Flags]
    public enum SourceTableCase
    {
        unknown=0,
        stable=1,
        variable=2,
        height=16,
        width=32,
        vertically=64,
        horizontally=128,
        orientation=256,
        staticContent=1024,
        dynamicContent=2048,
        variableHeight = variable | height,
        variableWidth = variable | width,
        stableHeight = stable | height,
        stableWidth = stable | width,
        /// <summary>
        /// The vertical orientation: variableHeight,stableWidth
        /// </summary>
        verticalOrientation = vertically | orientation,
        /// <summary>
        /// The horizontal orientation: stableHeight, variableWidth
        /// </summary>
        horizontalOrientation = horizontally | orientation,
    }
}