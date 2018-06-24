// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxisChart.cs" company="imbVeles" >
//
// Copyright (C) 2018 imbVeles
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// <summary>
// Project: imbSCI.Graph
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------

//AUTHOR	: GERARD CASTELLÓ
//DATE		: 17/JUN/2010

using imbSCI.Graph.Graphics.SvgDocument;
using System;
using System.Data;
using System.Drawing;

namespace imbSCI.Graph.Graphics.SvgChart
{
    public abstract class AxisChart : Chart
    {
        #region Attributes

        protected bool withGuideLines;
        protected double maxFillFactor;
        protected double minFillFactor;
        protected double tpcmaxFillFactor;
        private SVGPoint x1;
        private SVGPoint x2;
        private SVGPoint y1;
        private Line lineY;
        private Line lineX;
        protected double colWidth;
        protected Color backgroundColor;
        private double maxValue;
        private double minValue;

        #endregion Attributes

        #region Properties

        #region Axis Points

        public SVGPoint X1Axis
        {
            get
            {
                if (this.x1 == null)
                {
                    this.x1 = new SVGPoint();
                    this.x1.X = this.ChartArea.X;
                    this.x1.Y = this.ChartArea.Y + this.ChartArea.Height;

                    if (this.GetType() == typeof(LinearChart))
                        this.x1.Y -= 10;
                }
                return this.x1;
            }
        }

        public SVGPoint X2Axis
        {
            get
            {
                if (this.x2 == null)
                {
                    this.x2 = new SVGPoint();
                    this.x2.X = this.ChartArea.X + this.ChartArea.Width - 20 - (this.MaxValue.ToString().Length * (weightChar / 2));
                    this.x2.Y = this.X1Axis.Y;
                }
                return this.x2;
            }
        }

        public SVGPoint Y1Axis
        {
            get
            {
                if (this.y1 == null)
                {
                    this.y1 = new SVGPoint();
                    this.y1.X = this.X2Axis.X;
                    this.y1.Y = this.ChartArea.Y;
                }
                return this.y1;
            }
        }

        public SVGPoint Y2Axis
        {
            get
            {
                return this.X2Axis;
            }
        }

        #endregion Axis Points

        public double MaxValue
        {
            get
            {
                return this.maxValue;
            }
            set
            {
                this.maxValue = value;
            }
        }

        public double MinValue
        {
            get
            {
                return this.minValue;
            }
            set
            {
                this.minValue = value;
            }
        }

        public bool WithGuideLines
        {
            get
            {
                return this.withGuideLines;
            }
            set
            {
                this.withGuideLines = value;
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
            set
            {
                this.backgroundColor = value;
            }
        }

        #endregion Properties

        #region Constructor

        public AxisChart(int _width, int _height, int _legendItems) : base(_width, _height, _legendItems)
        {
        }

        #endregion Constructor

        #region Methods

        #region Private Methods

        /// <summary>
        /// Get the collection's max value
        /// </summary>
        /// <param name="_data">DataTable</param>
        /// <param name="_colValues">string</param>
        /// <returns>double</returns>
        private double GetMaxValue(DataTable _data, string _colValues)
        {
            double maxValue = 0;
            foreach (DataRow row in _data.Rows)
            {
                if (maxValue < Convert.ToDouble(row[_colValues]))
                    maxValue = Convert.ToDouble(row[_colValues]);
            }
            return maxValue;
        }

        /// <summary>
        /// Get the collection's min value
        /// </summary>
        /// <param name="_data">DataTable</param>
        /// <param name="_colValues">string</param>
        /// <returns>double</returns>
        private double GetMinValue(DataTable _data, string _colValues)
        {
            double minValue = double.MaxValue;
            foreach (DataRow row in _data.Rows)
            {
                if (minValue > Convert.ToDouble(row[_colValues]))
                    minValue = Convert.ToDouble(row[_colValues]);
            }
            return minValue;
        }

        /// <summary>
        /// Get the max factor
        /// </summary>
        /// <param name="maxValue">double</param>
        /// <param name="minValue">double</param>
        /// <returns>double</returns>
        private double GetMaxFillFactor(double maxValue, double minValue)
        {
            double tmpMax = 0;
            if (Convert.ToInt32(maxValue - minValue) == 0)
                tmpMax = Math.Round(maxValue, 2);
            else
                tmpMax = Math.Round(maxValue, MidpointRounding.ToEven);

            while (tmpMax < maxValue)
            {
                double dif = maxValue - tmpMax;
                double absolute = Math.Abs(dif);
                if (Convert.ToInt32(absolute) > 1)
                    dif += Math.Pow((double)10, (double)(Convert.ToInt32(absolute).ToString().Length - 1));
                else if (Convert.ToInt32(absolute) < -1)
                    dif += Math.Pow((double)0.1, (double)(Convert.ToInt32(absolute).ToString().Length - 1));
                else
                    dif += Math.Pow((double)0.01, (double)(Convert.ToInt32(absolute).ToString().Length));

                tmpMax += dif;
                tmpMax = Math.Round(tmpMax, 2);
            }

            return tmpMax;
        }

        /// <summary>
        /// Get the min factor
        /// </summary>
        /// <param name="minValue">double</param>
        /// <param name="maxValue">double</param>
        /// <returns>double</returns>
        private double GetMinFillFactor(double minValue, double maxValue)
        {
            double tmpMin = 0;
            if (Convert.ToInt32(maxValue - minValue) == 0)
                tmpMin = Math.Round(minValue, 2);
            else
                tmpMin = Math.Round(minValue, MidpointRounding.ToEven);

            while (tmpMin > minValue)
            {
                double dif = tmpMin - minValue;
                double absolute = Math.Abs(dif);
                if (Convert.ToInt32(absolute) > 1)
                    dif += Math.Pow((double)10, (double)(Convert.ToInt32(absolute).ToString().Length - 1));
                else if (Convert.ToInt32(absolute) < -1)
                    dif += Math.Pow((double)0.1, (double)(Convert.ToInt32(absolute).ToString().Length - 1));
                else
                    dif += Math.Pow((double)0.01, (double)(Convert.ToInt32(absolute).ToString().Length));

                tmpMin -= dif;
                tmpMin = Math.Round(tmpMin, 2);
            }

            return tmpMin;
        }

        /// <summary>
        /// Add the text legend text bellow the x axis.
        /// </summary>
        /// <param name="_x">double</param>
        /// <param name="_y">double</param>
        /// <param name="_fontSize">double</param>
        /// <param name="_legendValue">double</param>
        private void AddTextLegend(double _x, double _y, double _fontSize, double _legendValue)
        {
            Text txt;
            _legendValue = Math.Round(_legendValue, 2);
            for (int i = 0; i < _legendValue.ToString().Length; i++)
            {
                txt = new Text(_legendValue.ToString()[i].ToString());
                txt.Font_Size = _fontSize;
                txt.X = _x;
                txt.Y = _y;

                this.Doc.SvgObjects.Add(txt);
                _x += 4;
            }
        }

        #endregion Private Methods

        #region Protected Methods

        /// <summary>
        /// Initialize main values for generate the layout
        /// </summary>
        /// <param name="_data">DataTable</param>
        /// <param name="_colValues">string</param>
        protected void InitializeValues(DataTable _data, string _colValues)
        {
            this.colWidth = (this.X2Axis.X - this.X1Axis.X) / (double)_data.Rows.Count;
            this.maxValue = this.GetMaxValue(_data, _colValues);
            this.minValue = this.GetMinValue(_data, _colValues);
            this.maxFillFactor = this.GetMaxFillFactor(this.MaxValue, this.MinValue);
            this.minFillFactor = this.GetMinFillFactor(this.MinValue, this.MaxValue);
            this.GenerateAxis();
        }

        /// <summary>
        /// Creates x and y Axis
        /// </summary>
        protected void GenerateAxis()
        {
            #region Axis

            this.lineX = new Line(this.X1Axis.X, this.X1Axis.Y, this.X2Axis.X, this.X2Axis.Y);
            this.lineX.Width = (decimal)0.4;
            this.lineX.Color = Color.Gray;

            this.lineY = new Line(this.Y2Axis.X, this.Y2Axis.Y, this.Y1Axis.X, this.Y1Axis.Y);
            this.lineY.Width = (decimal)0.4;
            this.lineY.Color = Color.Gray;

            #endregion Axis

            #region Y Legend

            double value = 0;
            double legendValue = 0;
            short fontSize = 7;
            double xStartPos = this.X2Axis.X + 5;

            this.tpcmaxFillFactor = Convert.ToDouble((this.Y1Axis.Y - this.Y2Axis.Y) / 100.0);
            double scallability = (this.maxFillFactor - this.minFillFactor) / 4;

            value = 0 * tpcmaxFillFactor;
            double y1 = this.X1Axis.Y - value;
            AddTextLegend(xStartPos, y1, fontSize, this.minFillFactor);

            if (!(this.MaxValue == 0 && this.MinValue == 0))
            {
                value = 25 * tpcmaxFillFactor;
                legendValue = (scallability + this.minFillFactor);
                double y4 = this.X1Axis.Y + value;
                AddTextLegend(xStartPos, y4, fontSize, legendValue);

                value = 50 * tpcmaxFillFactor;
                legendValue = ((scallability * 2) + this.minFillFactor);
                double y2 = this.X1Axis.Y + value;
                AddTextLegend(xStartPos, y2, fontSize, legendValue);

                value = 75 * tpcmaxFillFactor;
                legendValue = ((scallability * 3) + this.minFillFactor);
                double y5 = this.X1Axis.Y + value;
                AddTextLegend(xStartPos, y5, fontSize, legendValue);

                value = 100 * tpcmaxFillFactor;
                double y3 = this.X1Axis.Y + value;
                AddTextLegend(xStartPos, y3, fontSize, this.maxFillFactor);

                if (!this.backgroundColor.IsEmpty)
                {
                    SvgDocument.Rectangle rect = new SvgDocument.Rectangle(this.X2Axis.X - this.X1Axis.X, this.Y2Axis.Y - this.Y1Axis.Y);
                    rect.X = this.x1.X;
                    rect.Y = this.Y1Axis.Y;
                    rect.Color = this.backgroundColor;
                    this.Doc.SvgObjects.Add(rect);
                }

                if (this.withGuideLines)
                {
                    Line lin1 = new Line(xStartPos, y2, this.lineX.X1, y2);
                    lin1.Color = Color.Gray;
                    lin1.Width = (decimal)0.2;
                    lin1.Dashed = true;

                    Line lin2 = new Line(xStartPos, y3, this.lineX.X1, y3);
                    lin2.Color = Color.Gray;
                    lin2.Width = (decimal)0.2;
                    lin2.Dashed = true;

                    Line lin3 = new Line(xStartPos, y4, this.lineX.X1, y4);
                    lin3.Color = Color.Gray;
                    lin3.Width = (decimal)0.2;
                    lin3.Dashed = true;

                    Line lin4 = new Line(xStartPos, y5, this.lineX.X1, y5);
                    lin4.Color = Color.Gray;
                    lin4.Width = (decimal)0.2;
                    lin4.Dashed = true;

                    this.Doc.SvgObjects.Add(lin1);
                    this.Doc.SvgObjects.Add(lin2);
                    this.Doc.SvgObjects.Add(lin3);
                    this.Doc.SvgObjects.Add(lin4);
                }
            }

            #endregion Y Legend

            this.Doc.SvgObjects.Add(lineX);
            this.Doc.SvgObjects.Add(lineY);
        }

        #endregion Protected Methods

        #endregion Methods
    }
}