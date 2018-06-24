// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PieChart.cs" company="imbVeles" >
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
using System.Xml;

namespace imbSCI.Graph.Graphics.SvgChart
{
    public class PieChart : Chart
    {
        #region Attributes

        private double radio;
        protected double relation;
        protected double amount;
        protected SVGPoint centerPos;
        protected System.Globalization.CultureInfo ci;

        #endregion Attributes

        #region Properties

        public double Radio
        {
            get
            {
                if (this.radio == 0)
                {
                    this.radio = this.ChartArea.Height / 2;
                }
                return radio;
            }
        }

        public double Relation
        {
            get
            {
                return relation;
            }
            set
            {
                relation = value;
            }
        }

        public double Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
            }
        }

        public SVGPoint CenterPos
        {
            get
            {
                if (centerPos == null)
                {
                    this.centerPos = new SVGPoint(this.ChartArea.X + (this.ChartArea.Width / 2), this.ChartArea.Y + (this.ChartArea.Height / 2));
                }
                return this.centerPos;
            }
        }

        #endregion Properties

        #region Constructor

        public PieChart(int _width, int _height, int _legendItems) : base(_width, _height, _legendItems)
        {
            this.ci = new System.Globalization.CultureInfo("en-US");
            this.ci.NumberFormat.NumberDecimalSeparator = ".";
            this.ci.NumberFormat.NumberDecimalDigits = 2;
            this.ci.NumberFormat.PercentDecimalDigits = 2;
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
            double degree = 0;
            double sumDegree = 0;
            int indexRow = 0;
            int indexColumn = 0;

            //Initialize
            for (int i = 0; i < _data.Rows.Count; i++)
                this.amount += Convert.ToDouble(_data.Rows[i][_colValues]);
            if (this.amount != 0)
                this.Relation = 360.0 / (double)this.amount;

            for (int i = 0; i < _data.Rows.Count; i++)
            {
                this.CreateCheese(Convert.ToDouble(_data.Rows[i][_colValues]), ref sumDegree, this.Colors[i], out degree);
                this.GenerateLegendItem(_data.Rows[i][_colNames].ToString(), ref indexColumn, ref indexRow, i, degree);
                indexRow++;
            }

            return this.Transform();
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Get a point from one degree
        /// </summary>
        /// <param name="_degree">double</param>
        /// <returns>SVGPoint</returns>
        protected virtual SVGPoint GetPoint(double _degree)
        {
            return this.GetPoint(_degree, this.Radio);
        }

        /// <summary>
        /// Returns a point in the area depending of the angle's degree and the height.
        /// </summary>
        /// <param name="_degree">double</param>
        /// <param name="_height">double</param>
        /// <returns>SVGPoint</returns>
        protected virtual SVGPoint GetPoint(double _degree, double _height)
        {
            SVGPoint p = new SVGPoint();
            double rad = this.GetRadians(_degree);
            p.X = Math.Sin(rad) * _height + this.CenterPos.X;
            p.Y = this.CenterPos.Y - Math.Cos(rad) * _height;
            return p;
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="_degree">double</param>
        /// <returns>double</returns>
        protected double GetRadians(double _degree)
        {
            return Math.PI * _degree / 180;
        }

        /// <summary>
        /// Creates the polygon's value
        /// </summary>
        /// <param name="_value">double</param>
        /// <param name="_sumDegree">double</param>
        /// <param name="_color">Color</param>
        /// <param name="_degree">double</param>
        protected virtual void CreateCheese(double _value, ref double _sumDegree, Color _color, out double _degree)
        {
            SVGPoint point;
            Polygon pol = new Polygon();
            pol.Color = _color;
            _degree = this.Relation * _value;

            //1st Segment
            point = this.GetPoint(_sumDegree);
            pol.AddPoint(this.CenterPos);
            pol.AddPoint(point);

            //Curve
            double k = _sumDegree;
            while (k < _degree + _sumDegree)
            {
                pol.AddPoint(this.GetPoint(k));
                k += 0.2;
            }

            //2th Segment
            _sumDegree += _degree;
            point = this.GetPoint(_sumDegree);
            pol.AddPoint(this.CenterPos);
            pol.AddPoint(point);

            this.Doc.SvgObjects.Add(pol);
        }

        /// <summary>
        /// Creates the legend item value
        /// </summary>
        /// <param name="text">string</param>
        /// <param name="indexColumn">int</param>
        /// <param name="indexRow">int</param>
        /// <param name="indexColor">int</param>
        /// <param name="currentDegree">double</param>
        protected void GenerateLegendItem(string text, ref int indexColumn, ref int indexRow, int indexColor, double currentDegree)
        {
            double currentTpc = currentDegree / 360 * 100;
            currentTpc = Math.Round(currentTpc, 2, MidpointRounding.ToEven);
            base.GenerateLegendItem(currentTpc.ToString() + " % " + text, ref indexColumn, ref indexRow, indexColor);
        }

        #endregion Protected Methods

        #endregion Methods
    }
}