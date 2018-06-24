// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pie3DChart.cs" company="imbVeles" >
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
using System.Collections;
using System.Drawing;

namespace imbSCI.Graph.Graphics.SvgChart
{
    public class Pie3DChart : PieChart
    {
        #region Attributes and Constants

        protected double b;
        protected double a;
        private const double THICKNESS = 20;

        #endregion Attributes and Constants

        #region Constructor

        public Pie3DChart(int _width, int _height, int _legendItems) : base(_width, _height, _legendItems)
        {
            this.b = _height / 4;
            this.a = _width / 3;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Returns a point in the area depending of the angle's degree.
        /// </summary>
        /// <param name="_degree">double</param>
        /// <returns>SVGPoint</returns>
        protected override SVGPoint GetPoint(double _degree)
        {
            return this.GetPoint(_degree, this.a, this.b);
        }

        /// <summary>
        /// Returns a point in the area depending of the angle's degree and the width and height of the pie.
        /// </summary>
        /// <param name="_degree">double</param>
        /// <param name="_width">double</param>
        /// <param name="_height">double</param>
        /// <returns>SVGPoint</returns>
        protected SVGPoint GetPoint(double _degree, double _width, double _height)
        {
            double rad = this.GetRadians(_degree);
            SVGPoint p = new SVGPoint();
            p.X = Math.Sin(rad) * _width + this.CenterPos.X;
            p.Y = this.CenterPos.Y - Math.Cos(rad) * _height;
            return p;
        }

        /// <summary>
        /// Creates the polygon's value
        /// </summary>
        /// <param name="_value">double</param>
        /// <param name="_sumDegree">double</param>
        /// <param name="_color">Color</param>
        /// <param name="_degree">double</param>
        protected override void CreateCheese(double _value, ref double _sumDegree, Color _color, out double _degree)
        {
            double k;
            ArrayList points = null;
            SVGPoint p1, p2;
            Polygon pol1;
            Polygon pol2 = null;
            _degree = this.Relation * _value;

            pol1 = new Polygon();
            pol1.Color = _color;

            //1st Segment
            p1 = this.GetPoint(_sumDegree);
            pol1.AddPoint(this.CenterPos);
            pol1.AddPoint(p1);

            //Curve
            k = _sumDegree;
            if ((_sumDegree >= 90 && _sumDegree <= 270) || (_degree + _sumDegree >= 90 && _degree + _sumDegree <= 270))
            {
                pol2 = new Polygon();
                pol2.Color = _color;
                points = new ArrayList();
                p1 = this.GetPoint(k);
            }
            while (k < _degree + _sumDegree)
            {
                p1 = this.GetPoint(k);
                pol1.AddPoint(p1);
                k += 0.2;

                if (k > 90 && k < 270)
                {
                    pol2.AddPoint(p1);
                    p2 = p1;
                    p2.Y += THICKNESS;
                    points.Insert(0, p2);
                }
            }
            if (pol2 != null)
            {
                foreach (SVGPoint point in points)
                {
                    pol2.AddPoint(point);
                }
                this.Doc.SvgObjects.Add(pol2);
            }

            //2th Segment
            _sumDegree += _degree;
            pol1.AddPoint(this.CenterPos);

            this.Doc.SvgObjects.Add(pol1);
        }

        #endregion Methods
    }
}