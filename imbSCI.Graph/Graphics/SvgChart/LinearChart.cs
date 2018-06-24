// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearChart.cs" company="imbVeles" >
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
using System.Xml;

namespace imbSCI.Graph.Graphics.SvgChart
{
    public class LinearChart : AxisChart
    {
        #region Attributes and Constants

        private SVGPoint[] points;
        private const double FONT_SIZE = 7;
        private const ushort ROTATION_ANGLE = 30;

        #endregion Attributes and Constants

        #region Properties

        public SVGPoint[] Points
        {
            get
            {
                return this.points;
            }
        }

        #endregion Properties

        #region Constructor

        public LinearChart(int _width, int _height, int _legendItems) : base(_width, _height, _legendItems)
        {
            this.points = new SVGPoint[0];
        }

        #endregion Constructor

        #region Methods

        #region Public Methods

        /// <summary>
        /// Add the value's point
        /// </summary>
        /// <param name="_point">SVGObject.point</param>
        public void AddPoint(SVGPoint _point)
        {
            Array.Resize(ref this.points, this.points.Length + 1);
            this.points[this.points.Length - 1] = _point;
        }

        /// <summary>
        /// Get a XmlDocument with the svg's transformed document.
        /// </summary>
        /// <param name="_data">DataTable</param>
        /// <param name="_colNames">string</param>
        /// <param name="_colValues">string</param>
        /// <returns>XmlDocument</returns>
        public override XmlDocument GenerateChart(DataTable _data, string _colNames, string _colValues)
        {
            base.InitializeValues(_data, _colValues);
            double xColPos = this.X1Axis.X;
            foreach (DataRow row in _data.Rows)
            {
                this.AddPoint(this.GeneratePoint(Convert.ToDouble(row[_colValues]), xColPos, (double)this.X1Axis.Y));
                xColPos += colWidth;
            }
            this.UnionPoints();
            this.GenerateLegendItem(_data, _colNames);

            return this.Transform();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Creates the line
        /// </summary>
        private void UnionPoints()
        {
            for (int i = 0; i < this.points.Length - 1; i++)
            {
                this.CreateLine(this.points[i], this.points[i + 1]);
            }
        }

        /// <summary>
        /// Creates a line with two points of the collection
        /// </summary>
        /// <param name="_source">SVGPoint</param>
        /// <param name="_target">SVGPoint</param>
        protected void CreateLine(SVGPoint _source, SVGPoint _target)
        {
            Line line = new Line(_source, _target);
            line.Width = (decimal)0.5;
            line.Color = System.Drawing.Color.Black;
            line.Dashed = false;
            this.Doc.SvgObjects.Add(line);
        }

        /// <summary>
        /// Get a value's point
        /// </summary>
        /// <param name="_value">double</param>
        /// <param name="_xStartPosition">double</param>
        /// <param name="_yStartPosition">double</param>
        /// <returns>SVGPoint</returns>
        private SVGPoint GeneratePoint(double _value, double _xStartPosition, double _yStartPosition)
        {
            double pixelValue = _value - this.minFillFactor;
            pixelValue = pixelValue / (this.maxFillFactor - this.minFillFactor);
            pixelValue = pixelValue * 100 * this.tpcmaxFillFactor;

            SVGPoint p1 = new SVGPoint();
            p1.X = _xStartPosition + (this.colWidth / 2);
            p1.Y = _yStartPosition + pixelValue;
            return p1;
        }

        /// <summary>
        /// Creates a value's legend item
        /// </summary>
        /// <param name="_data">DataTable</param>
        /// <param name="_colNames">string</param>
        private void GenerateLegendItem(DataTable _data, string _colNames)
        {
            int numberRowDivision = _data.Rows.Count / this.legendItems;
            Text txt;
            for (int i = 0; i < _data.Rows.Count; i += numberRowDivision)
            {
                double x = (this.X1Axis.X + (this.colWidth * i));
                double y = this.X1Axis.Y + 10;
                for (int k = 0; k < _data.Rows[i][_colNames].ToString().Length; k++)
                {
                    txt = new Text(_data.Rows[i][_colNames].ToString()[k].ToString());
                    txt.Y = y;
                    txt.X = x;
                    txt.Font_Size = FONT_SIZE;
                    txt.Rotate = ROTATION_ANGLE;
                    this.Doc.SvgObjects.Add(txt);
                    x += 3;
                    y += 2;
                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}