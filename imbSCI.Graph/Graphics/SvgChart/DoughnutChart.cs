// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoughnutChart.cs" company="imbVeles" >
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
using System.Drawing;

namespace imbSCI.Graph.Graphics.SvgChart
{
    public class DoughnutChart : PieChart
    {
        #region Attributes

        private readonly double secRadio;

        #endregion Attributes

        #region Constructor

        public DoughnutChart(int _width, int _height, int _legendItems) : base(_width, _height, _legendItems)
        {
            this.secRadio = this.Radio / 2;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Creates the polygon's value.
        /// </summary>
        /// <param name="_value">double</param>
        /// <param name="_sumDegree">double</param>
        /// <param name="_color">Color</param>
        /// <param name="_degree">double</param>
        protected override void CreateCheese(double _value, ref double _sumDegree, Color _color, out double _degree)
        {
            Polygon pol = new Polygon();
            pol.Color = _color;
            _degree = this.Relation * _value;

            //1st Segment
            SVGPoint p1 = this.GetPoint(_sumDegree);
            SVGPoint p1_2 = this.GetPoint(_sumDegree, this.secRadio);
            pol.AddPoint(p1_2);
            pol.AddPoint(p1);

            //Curve
            double k = _sumDegree;
            while (k < _degree + _sumDegree)
            {
                pol.AddPoint(this.GetPoint(k));
                k += 0.2;
            }

            //2th Segment
            _sumDegree += _degree;
            SVGPoint p2 = this.GetPoint(_sumDegree);
            SVGPoint p2_2 = this.GetPoint(_sumDegree, this.secRadio);
            pol.AddPoint(p2_2);
            pol.AddPoint(p2);

            //Curve
            k = _sumDegree;
            while (k > _sumDegree - _degree)
            {
                pol.AddPoint(this.GetPoint(k, this.secRadio));
                k -= 0.2;
            }

            this.Doc.SvgObjects.Add(pol);
        }

        #endregion Methods
    }
}