// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Doughnut3DChart.cs" company="imbVeles" >
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
using System.Data;
using System.Drawing;
using System.Xml;

namespace imbSCI.Graph.Graphics.SvgChart
{
    public class Doughnut3DChart : Pie3DChart
    {
        #region Attributes and Constants

        private double a2;
        private double b2;
        private const double THICKNESS = 20;

        #endregion Attributes and Constants

        #region Constructor

        public Doughnut3DChart(int _width, int _height, int _legendItems) : base(_width, _height, _legendItems)
        {
            this.a2 = this.a / 2;
            this.b2 = this.b / 2;
        }

        #endregion Constructor

        #region Methods

        #region Protected and Public Methods

        /// <summary>
        /// Get a XmlDocument with the svg's transformed document.
        /// </summary>
        /// <param name="_data">DataTable</param>
        /// <param name="_colNames">string</param>
        /// <param name="_colValues">string</param>
        /// <returns>XmlDocument</returns>
        public override XmlDocument GenerateChart(DataTable _data, string _colNames, string _colValues)
        {
            double degree = 0;
            double sumDegree = 0;
            int indexRow = 0;
            int indexColumn = 0;
            int i = 0;

            //Initialize
            for (i = 0; i < _data.Rows.Count; i++)
                this.amount += Convert.ToDouble(_data.Rows[i][_colValues]);
            if (this.amount != 0)
                this.Relation = 360 / this.amount;

            for (i = 0; i < _data.Rows.Count; i++)
                this.CreateInnerSection(Convert.ToDouble(_data.Rows[i][_colValues]), ref sumDegree, this.Colors[i], out degree);

            degree = 0;
            sumDegree = 0;

            for (i = 0; i < _data.Rows.Count; i++)
            {
                this.CreateCheese(Convert.ToDouble(_data.Rows[i][_colValues]), ref sumDegree, this.Colors[i], out degree);
                this.GenerateLegendItem(_data.Rows[i][_colNames].ToString(), ref indexColumn, ref indexRow, i, degree);
                indexRow++;
            }

            return this.Transform();
        }

        /// <summary>
        /// Creates the polygon's value.
        /// </summary>
        /// <param name="_value">double</param>
        /// <param name="_sumDegree">double</param>
        /// <param name="_color">Color</param>
        /// <param name="_degree">double</param>
        protected override void CreateCheese(double _value, ref double _sumDegree, Color _color, out double _degree)
        {
            double k;
            ArrayList points = null;
            SVGPoint p1, p2, p1_2;
            Polygon pol1, pol2 = null;
            _degree = this.Relation * _value;

            pol1 = new Polygon();
            pol1.Color = _color;

            //1st Segment
            p1 = this.GetPoint(_sumDegree);
            p1_2 = this.GetPoint(_sumDegree, this.a2, this.b2);
            pol1.AddPoint(p1_2);
            pol1.AddPoint(p1);

            //Curve
            k = _sumDegree;
            if ((_sumDegree >= 90 && _sumDegree <= 270) || (_degree + _sumDegree >= 90 && _degree + _sumDegree <= 270))
            {
                pol2 = new Polygon();
                pol2.Color = _color;
                points = new ArrayList();
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
                    p2.Y += 20;
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
            p1_2 = this.GetPoint(_sumDegree, this.a2, this.b2);
            pol1.AddPoint(p1_2);

            //Curve
            k = _sumDegree;
            while (k > _sumDegree - _degree)
            {
                p1 = this.GetPoint(k, this.a2, this.b2);
                pol1.AddPoint(p1);
                k -= 0.2;
            }

            this.Doc.SvgObjects.Add(pol1);
        }

        #endregion Protected and Public Methods

        #region Private Methods

        /// <summary>
        /// Create the inner polygon of the doughnut.
        /// </summary>
        /// <param name="_value">double</param>
        /// <param name="_sumDegree">double</param>
        /// <param name="_color">Color</param>
        /// <param name="_degree">double</param>
        private void CreateInnerSection(double _value, ref double _sumDegree, Color _color, out double _degree)
        {
            SVGPoint p1 = null, p2 = null;
            Polygon pol = null;
            ArrayList points = null;
            _degree = this.Relation * _value;

            if (_sumDegree >= 270 || _sumDegree <= 90 || _degree + _sumDegree >= 270 || _degree + _sumDegree <= 90)
            {
                pol = new Polygon();
                pol.Color = _color;
                points = new ArrayList();

                double k = _sumDegree;
                while (k < _degree + _sumDegree)
                {
                    p1 = this.GetPoint(k, this.a2, this.b2);
                    pol.AddPoint(p1);
                    p2 = p1;
                    p2.Y += THICKNESS;
                    points.Insert(0, p2);
                    k += 0.2;
                }

                foreach (SVGPoint point in points)
                {
                    pol.AddPoint(point);
                }
                this.Doc.SvgObjects.Add(pol);
            }

            _sumDegree += _degree;
        }

        #endregion Private Methods

        #endregion Methods
    }
}