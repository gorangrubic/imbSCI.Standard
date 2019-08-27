using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.text;
using imbSCI.Core.math.range.finder;
using imbSCI.Core.math.range.matrix;
using imbSCI.Core.reporting.zone;
using imbSCI.Core.style.color;
using imbSCI.Graph.Graphics.SvgDocument;


using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace imbSCI.Graph.Graphics.HeatMap
{
    public class spatialZone : cursorZoneSpatialSettings
    {
    }

    /// <summary>
    /// Utility class used for rendering of <see cref="HeatMapModel"/>
    /// </summary>
    public class HeatMapRender
    {
        private List<String> xLabels { get; set; } = new List<string>();
        private List<String> yLabels { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        /// <value>
        /// The style.
        /// </value>
        public HeatMapRenderStyle style { get; set; } = new HeatMapRenderStyle();

        public HeatMapRender()
        {
        }

        /// <summary>
        /// Renders the heat map and saves the output to <c>filePath</c>. This is void alias to <see cref="Render"/>
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="filePath">The file path.</param>
        public Svg.SvgDocument RenderAndSave(HeatMapModel model, String filePath)
        {
            Svg.SvgDocument output = Render(model, filePath);

            return output;
        }

        protected void prepareLabels(HeatMapModel model)
        {
            foreach (String key in model.xKeys)
            {
                String k = key;
                if (style.accronimLength > 0) k = key.imbGetWordAbbrevation(style.accronimLength, true);
                xLabels.Add(k);
            }

            foreach (String key in model.yKeys)
            {
                String k = key;
                if (style.accronimLength > 0) k = key.imbGetWordAbbrevation(style.accronimLength, true);
                yLabels.Add(k);
            }
        }

        /// <summary>
        /// Renders the specified <see cref="HeatMapModel"/>, optionally saves the output SVG
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public Svg.SvgDocument Render(HeatMapModel model, String filePath = "")
        {
            rangeFinder valueRange = model.DetectMinMax();

            var lColor = style.LowColor.GetColorVersionWithAlpha(style.MinOpacity); //.ColorToHex();
            var hColor = style.HighColor.GetColorVersionWithAlpha(style.MaxOpacity); //.ColorToHex();



            ColorGradient colorGradient = new ColorGradient(lColor, hColor, ColorGradientFunction.AllAToB);
            

            cursorZoneSpatialSettings format = style.fieldContainer.GetFormatSetup();
            format.spatialUnit = 8;
            format.spatialUnitHeight = 10;

            Int32 width = (model.weight * format.width) + format.margin.right;
            Int32 height = (model.height * format.height) + format.margin.bottom;

            Svg.SvgDocument output = new Svg.SvgDocument
            {
                Width = width,
                Height = height,
                Ppi = 100
                
            };

            var mainContainer = new SvgGroup();

            output.Children.Add(mainContainer);

            //(new SvgLength(width), new SvgLength(height));

            // output.ViewBox = new SvgViewBox(-100, -100, width+100, height+100);

            var group = new SvgGroup();
            mainContainer.Children.Add(group);

            var layerTwo = new SvgGroup();
            mainContainer.Children.Add(layerTwo);

            prepareLabels(model);

            var hor = new SvgGroup();
            if (style.options.HasFlag(HeatMapRenderOptions.addHorizontalLabels)) layerTwo.Children.Add(hor);

            for (int x = 0; x < model.weight; x++)
            {
                Int32 xStart = x * format.width;

                Svg.SvgText label = xLabels[x].GetSvgText(format, x, -1);

                //Svg.SvgText label = new SvgText(xLabels[x])
                //{
                //    X = (xStart + (format.width / 2) - format.margin.right).Get_px(),
                //    Y = (-format.height / 2).Get_px(),
                //    Color = new SvgColourServer(Color.Black),
                //    Font = "Gulliver"

                //};

                hor.Children.Add(label);

                var vert = new SvgGroup();
                layerTwo.Children.Add(vert);

                var vertLabels = new SvgGroup();
                var vertValues = new SvgGroup();
                var vertScale = new SvgGroup();

                if (style.options.HasFlag(HeatMapRenderOptions.addVerticalLabels)) vert.Children.Add(vertLabels);
                if (style.options.HasFlag(HeatMapRenderOptions.addVerticalValueScale)) vert.Children.Add(vertScale);
                if (style.options.HasFlag(HeatMapRenderOptions.addVerticalValueScale)) vert.Children.Add(vertValues);

                for (int y = 0; y < model.height; y++)
                {
                    Int32 yStart = y * format.height;

                    if (x == 0)
                    {
                        Double ratio = valueRange.GetPositionInRange(y);  //model.GetRatioForScale(y, style.minimalOpacity, model.height); //(1+ style.minimalOpacity).GetRatio(y+1);
                        Double scaleFactor2 = ratio;
                        if (!style.options.HasFlag(HeatMapRenderOptions.resizeFields))
                        {
                            scaleFactor2 = 1;
                        }

                        if (ratio > 1) ratio = 1;
                        var lbl2 = format.GetRectangle((-format.width * 2), yStart, colorGradient.GetColor(ratio), Convert.ToSingle(ratio), scaleFactor2);
                        vertScale.Children.Add(lbl2);

                        Svg.SvgText label2 = yLabels[y].GetSvgText(format, -1, y);
                        vertLabels.Children.Add(label2);

                        //Svg.SvgText label = xLabels[x].GetSvgText(format, x, -1);

                        //Svg.SvgText label2 = new SvgText(yLabels[y])
                        //{
                        //    X = (format.margin.left - format.width).Get_px(),
                        //    Y = (yStart+(format.height / 2)).Get_px(),
                        //    Color = new SvgColourServer(Color.Black),

                        //    Font = "Gulliver"
                        //};

                        Int32 xp = Convert.ToInt32((-Convert.ToDouble(format.width) * 2.5) + format.margin.left);

                        //Double vl = (1.GetRatio(y + 1)) * model.ranger.Maximum;

                        Double vl = model.GetValueForScaleY(y);

                        Svg.SvgText value = vl.ToString(style.valueFormat).GetSvgText(format, -3, y);

                        //Svg.SvgText value = new SvgText()
                        //{
                        //    X = (xp- format.margin.right).Get_px(),
                        //    Y = (yStart + (format.height / 2) ).Get_px(),
                        //    Fill = new SvgColourServer(Color.Black),
                        //   // Color = new SvgColourServer(Color.White),
                        //    Font = "Gulliver"
                        //};

                        vertValues.Children.Add(value);
                    }

                    Double val = valueRange.GetPositionInRange(model[x, y]); // model.GetRatioValue(x, y, style.minimalOpacity);
                    Color valC = colorGradient.GetColor(val);


                    Double scaleFactor = val;
                    
                    if (!style.options.HasFlag(HeatMapRenderOptions.resizeFields))
                    {
                        scaleFactor = 1;
                    }
                    var rct = format.GetRectangle(xStart, yStart, valC, Convert.ToSingle(val), scaleFactor);

                    group.Children.Add(rct);
                }
            }

            if (!filePath.isNullOrEmpty())
            {
                if (!filePath.EndsWith(".svg", true, CultureInfo.CurrentCulture))
                {
                    filePath += ".svg";
                }


                output.Save(filePath);

                //  throw new NotImplementedException();

                /* var code = output.GetXML();  //Encoding.UTF8.GetString(stream.GetBuffer());

                */
            }

            return output;
        }
    }
}