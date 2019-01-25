using imbSCI.Core.reporting.zone;
using Svg;
using System;
using System.Drawing;

namespace imbSCI.Graph.Graphics.HeatMap
{
    /// <summary>
    /// Helper tools for SVG
    /// </summary>
    public static class SVGTools
    {
        /// <summary>
        /// Gets the resized.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns></returns>
        public static cursorZoneSpatialSettings GetResized(this cursorZoneSpatialSettings format, Double scaleFactor)
        {
            Int32 newWidth = Convert.ToInt32(Convert.ToDouble(format.width) * scaleFactor);
            Int32 newHeight = Convert.ToInt32(Convert.ToDouble(format.height) * scaleFactor);

            var output = new cursorZoneSpatialSettings(newWidth, newHeight, format.margin.left, format.margin.top, format.padding.left, format.padding.top);

            return output;
        }

        /// <summary>
        /// Gets the SVG text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="format">The format.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static SvgText GetSvgText(this String text, cursorZoneSpatialSettings format, Int32 x, Int32 y)
        {
            Int32 xStart = x * format.width;
            Int32 yStart = y * format.height;

            Svg.SvgText label = new SvgText(text);

            //throw new NotImplementedException();

            //label.X = new SvgUnit (xStart + (format.width / 2) - ((text.Length * format.spatialUnit) / 2)));
            //label.Y = (yStart + (format.height / 2) + (format.spatialUnitHeight)).Get_px();

            //    label.Fill = new SvgColourServer(Color.Black),
            //    label.Font = "Gulliver"

            return label;
        }

        /// <summary>
        /// Gets the rectangle.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="xStart">The x start.</param>
        /// <param name="yStart">The y start.</param>
        /// <param name="color">The color.</param>
        /// <param name="Opacity">The opacity.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns></returns>
        public static SvgRectangle GetRectangle(this cursorZoneSpatialSettings format, Int32 xStart, Int32 yStart, Color color, float Opacity = 0.5F, Double scaleFactor = 1)
        {
            var rct = new SvgRectangle();

            var tmp = format.GetResized(scaleFactor);
            // tmp.x += xStart;
            // tmp.y += yStart;

            Int32 newX = Convert.ToInt32((Convert.ToDouble(format.width) / 2) - (Convert.ToDouble(format.width) * (scaleFactor / 2)));
            Int32 newY = Convert.ToInt32((Convert.ToDouble(format.height) / 2) - (Convert.ToDouble(format.height) * (scaleFactor / 2)));

            rct.Fill = new SvgColourServer(color);
            rct.Stroke = new SvgColourServer(Color.White);
            rct.StrokeWidth = 0;
            rct.FillOpacity = 1;
            rct.FillRule = SvgFillRule.Inherit;
            // rct.Font = "Gulliver";
            // rct.FontSize = 10;

            rct.Opacity = Opacity;

            rct.X = (xStart + newX + tmp.innerLeftPosition).Get_upx();
            rct.Y = (yStart + newY + tmp.innerTopPosition).Get_upx();
            rct.Width = tmp.innerBoxedWidth;
            rct.Height = tmp.innerBoxedHeight;
            return rct;
        }

        /// <summary>
        /// Gets 100th part of inch * <c>val</c>
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public static SvgUnitCollection Get_ci(this Int32 val)
        {
            SvgUnit svgUnit = new SvgUnit(SvgUnitType.Inch, val / 100);
            SvgUnitCollection svgUnitCollection = new SvgUnitCollection();
            svgUnitCollection.Add(svgUnit);
            return svgUnitCollection;
        }

        public static SvgUnitCollection Get_pt(this Int32 val)
        {
            SvgUnit svgUnit = new SvgUnit(SvgUnitType.Point, val);
            SvgUnitCollection svgUnitCollection = new SvgUnitCollection();
            svgUnitCollection.Add(svgUnit);
            return svgUnitCollection;
        }

        public static SvgUnit Get_upx(this Int32 val)
        {
            SvgUnit svgUnit = new SvgUnit(SvgUnitType.Pixel, val);
            return svgUnit;
        }

        public static SvgUnitCollection Get_px(this Int32 val)
        {
            SvgUnit svgUnit = new SvgUnit(SvgUnitType.Pixel, val);
            SvgUnitCollection svgUnitCollection = new SvgUnitCollection();
            svgUnitCollection.Add(svgUnit);
            return svgUnitCollection;
        }

        public static SvgUnitCollection Get_mm(this Int32 val)
        {
            SvgUnit svgUnit = new SvgUnit(SvgUnitType.Millimeter, val);
            SvgUnitCollection svgUnitCollection = new SvgUnitCollection();
            svgUnitCollection.Add(svgUnit);
            return svgUnitCollection;
        }
    }
}