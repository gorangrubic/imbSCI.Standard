using imbSCI.Core.files;
using imbSCI.Core.reporting.style.core;
using imbSCI.Core.reporting.style.enums;
using imbSCI.Core.reporting.style.shot;
using imbSCI.Core.reporting.zone;
using System;
using System.Drawing;
using System.Xml.Serialization;

namespace imbSCI.Graph.Graphics.HeatMap
{
    /// <summary>
    /// Heat map rendering style
    /// </summary>
    public class HeatMapRenderStyle
    {
        public HeatMapRenderOptions options { get; set; } = HeatMapRenderOptions.addVerticalValueScale | HeatMapRenderOptions.addHorizontalLabels | HeatMapRenderOptions.addVerticalLabels;

        //public ColorGradient fieldGradient { get; set; } = ColorGradient.RedBlueAtoBPreset;

        public Color BaseColor { get; set; } = Color.Black;

        public Color LowColor { get; set; } = Color.Black;

        public Color HighColor { get; set; } = Color.LightGray;

        public Double MinOpacity { get; set; } = 0.2;

        public Double MaxOpacity { get; set; } = 1;

        [XmlIgnore]
        public styleTextFontSingle axisText { get; set; } = new styleTextFontSingle();

        [XmlIgnore]
        public styleContainerShot fieldContainer { get; set; } = new styleContainerShot();

        public Int32 fieldWidth { get; set; } = 50;

        public Int32 fieldHeight { get; set; } = 50;

        public Double minimalOpacity { get; set; } = 50;

        public String valueFormat { get; set; } = "F1";

        /// <summary>
        /// Number of letters in key accronims
        /// </summary>
        /// <value>
        /// The length of the accronim.
        /// </value>
        public Int32 accronimLength { get; set; } = 3;

        public HeatMapRenderStyle Clone()
        {
            String xml = objectSerialization.ObjectToXML(this);
            var output = objectSerialization.ObjectFromXML<HeatMapRenderStyle>(xml);
            output.axisText = axisText;
            output.fieldContainer = fieldContainer;
            return output;
        }

        public HeatMapRenderStyle()
        {
            axisText.Color = Color.DarkGray;

            fieldContainer.aligment = textCursorZoneCorner.center;
            fieldContainer.minSize = new styleSize(fieldWidth, fieldHeight);
            fieldContainer.numberFormat = valueFormat;
            fieldContainer.sizeAndBorder.setup(1, 1, Color.Gray, 3, styleBorderType.Thick, styleSideDirection.bottom, styleSideDirection.top, styleSideDirection.left, styleSideDirection.right);
        }
    }
}