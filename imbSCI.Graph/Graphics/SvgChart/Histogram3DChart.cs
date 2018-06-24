// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Histogram3DChart.cs" company="imbVeles" >
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
    public class Histogram3DChart : HistogramChart
    {
        #region Attributes and Constants

        private readonly short depth;
        private const short ANGLE = 70;

        #endregion Attributes and Constants

        #region Properties

        public short Depth
        {
            get
            {
                return this.depth;
            }
        }

        #endregion Properties

        #region Constructors

        public Histogram3DChart(int _width, int _height, int _legendItems, short _depth)
            : base(_width, _height, _legendItems)
        {
            this.depth = _depth;
        }

        #endregion Constructors

        #region Methods

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
            Polygon pol;

            int indexRow = 0;
            int indexColumn = 0;
            double xColPos = this.X1Axis.X - (Math.Cos(ANGLE) * this.depth);
            double yBase = this.X1Axis.Y + (Math.Sign(ANGLE) * this.depth);

            for (int i = 0; i < _data.Rows.Count; i++)
            {
                double val = Convert.ToDouble(_data.Rows[i][_colValues]);
                rect = this.GenerateBar(val, xColPos, yBase, indexRow);
                if (rect != null)
                    this.Doc.SvgObjects.Add(rect);
                pol = this.GenerateTopPol(rect, indexRow);
                if (pol != null)
                {
                    pol.Border = true;
                    this.Doc.SvgObjects.Add(pol);
                }
                pol = this.GenerateSidePol(rect, indexRow);
                if (pol != null)
                {
                    pol.Border = true;
                    this.Doc.SvgObjects.Add(pol);
                }

                base.GenerateLegendItem(_data.Rows[i][_colNames].ToString(), ref indexColumn, ref indexRow, i);
                xColPos += colWidth;
                indexRow++;
            }

            return this.Transform();
        }

        /// <summary>
        /// Get the Polygon for the top of the bar.
        /// </summary>
        /// <param name="_bar">Rectangle</param>
        /// <param name="_indexRow">int</param>
        /// <returns>Polygon</returns>
        private Polygon GenerateTopPol(Rectangle _bar, int _indexRow)
        {
            double yBottom = _bar.Y - this.depth * Math.Sign(ANGLE);
            double xBottom = _bar.X + this.depth * Math.Cos(ANGLE);

            Polygon pol = new Polygon();
            pol.Color = this.Colors[_indexRow];
            pol.AddPoint(_bar.X, _bar.Y);
            pol.AddPoint(xBottom, yBottom);
            pol.AddPoint(xBottom + _bar.Width, yBottom);
            pol.AddPoint(_bar.X + _bar.Width, _bar.Y);

            return pol;
        }

        /// <summary>
        /// Get the Polygon for the side of the bar
        /// </summary>
        /// <param name="_bar">Rectangle</param>
        /// <param name="_indexRow">int</param>
        /// <returns>Polygon</returns>
        private Polygon GenerateSidePol(Rectangle _bar, int _indexRow)
        {
            double yBottom = _bar.Y - this.depth * Math.Sign(ANGLE);
            double xBottom = _bar.X + this.depth * Math.Cos(ANGLE);

            Polygon pol = new Polygon();
            pol.Color = this.Colors[_indexRow];
            pol.AddPoint(_bar.X + _bar.Width, _bar.Y);
            pol.AddPoint(xBottom + _bar.Width, yBottom);
            pol.AddPoint(xBottom + _bar.Width, yBottom + _bar.Height);
            pol.AddPoint(_bar.X + _bar.Width, _bar.Y + _bar.Height);

            return pol;
        }

        #endregion Methods
    }
}