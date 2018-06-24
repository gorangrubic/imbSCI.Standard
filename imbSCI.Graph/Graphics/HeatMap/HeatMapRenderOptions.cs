using System;

//using Svg.Document_Structure;

namespace imbSCI.Graph.Graphics.HeatMap
{
    /// <summary>
    /// Remdering options
    /// </summary>
    [Flags]
    public enum HeatMapRenderOptions
    {
        none = 0,
        addHorizontalValueScale = 1,
        addVerticalValueScale = 2,
        showValueInField = 4,
        showPercentOfRange = 8,
        showSignedPercentOfRange = 16,
        resizeFields = 32,
        addHorizontalLabels = 64,
        addVerticalLabels = 128,
    }
}