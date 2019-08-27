using HtmlAgilityPack;
using imbSCI.Core.extensions;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Core.math.functions;
using imbSCI.Core.math.range.frequency;
using imbSCI.Core.style.color;
using imbSCI.Data;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.extensions;
using imbSCI.Data.extensions.data;
using imbSCI.Data.extensions.data;
using imbSCI.Graph.Converters;
using imbSCI.Graph.Converters.tools;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;
using imbSCI.Graph.DGML.enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Analyzers.Templates
{
[Flags]
    public enum DataPointMapBlockDetectionFlags
    {
        none=0,
        /// <summary>
        /// Greedy approach
        /// </summary>
        maximizeBlockSize=1,
        /// <summary>
        /// Results in more blacks with less data points, that are more related 
        /// </summary>
        maximizeDataRelatness=2,
        /// <summary>
        /// It will split blocks to separate single-column and multi-column data points
        /// </summary>
        BreakByDimensions=4,
        /// <summary>
        /// The allow asimetric multi column data points: may contain inuequal number of static (labels) and dynamic (variables) properties
        /// </summary>
        AllowAsimetricMultiColumnDataPoints = 8,
        /// <summary>
        /// Allows multi column data points, having two or more entries wrappet in parent entry
        /// </summary>
        AllowMultiColumnDataPoints = 16,

        All=maximizeBlockSize|BreakByDimensions|AllowAsimetricMultiColumnDataPoints

    }
}