// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistogramChart.cs" company="imbVeles" >
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
    /// <summary>
    /// Bar chart (not histogram really)
    /// </summary>
    /// <seealso cref="imbSCI.Graph.Graphics.SvgChart.AxisChart" />
    public class HistogramChart : AxisChart
    {
        #region Constructor

        public HistogramChart(int _width, int _height, int _legendItems) : base(_width, _height, _legendItems)
        {
        }

        #endregion Constructor

        #region Methods

        #region Public Methods

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

            Rectangle rect;
            int indexRow = 0;
            int indexColumn = 0;
            double xColPos = this.X1Axis.X;

            for (int i = 0; i < _data.Rows.Count; i++)
            {
                double val = Convert.ToDouble(_data.Rows[i][_colValues]);
                rect = this.GenerateBar(val, xColPos, this.X1Axis.Y, indexRow);
                if (rect != null)
                    this.Doc.SvgObjects.Add(rect);
                base.GenerateLegendItem(_data.Rows[i][_colNames].ToString(), ref indexColumn, ref indexRow, i);
                xColPos += colWidth;
                indexRow++;
            }

            return this.Transform();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Creates the value's Rectangle
        /// </summary>
        /// <param name="_value">double</param>
        /// <param name="_x">double</param>
        /// <param name="_y">double</param>
        /// <param name="_indexRow">int</param>
        /// <returns>Rectangle</returns>
        protected Rectangle GenerateBar(double _value, double _x, double _y, int _indexRow)
        {
            Rectangle rec = null;
            if (_value != 0)
            {
                double pixelValue = (_value / this.maxFillFactor) * 100 * this.tpcmaxFillFactor;
                rec = new Rectangle(this.colWidth, 0 - pixelValue);
                rec.X = _x;
                rec.Y = _y + pixelValue;
                rec.Color = this.Colors[_indexRow];
                rec.Border = true;
            }
            return rec;
        }

        #endregion Private Methods

        #endregion Methods
    }
}