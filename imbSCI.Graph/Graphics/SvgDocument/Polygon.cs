// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Polygon.cs" company="imbVeles" >
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

using System;
using System.Drawing;
using System.Xml;

namespace imbSCI.Graph.Graphics.SvgDocument
{
    public class Polygon : ISVGTranform
    {
        #region Attributes

        private SVGPoint[] points;
        private Color color;
        private bool border;

        #endregion Attributes

        #region Properties

        public SVGPoint[] Points
        {
            get
            {
                return this.points;
            }
        }

        public Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
            }
        }

        public bool Border
        {
            get
            {
                return this.border;
            }
            set
            {
                this.border = value;
            }
        }

        #endregion Properties

        #region Constructor

        public Polygon()
        {
            this.points = new SVGPoint[0];
            this.color = Color.White;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Add a point to the array
        /// </summary>
        /// <param name="_xvalue">double</param>
        /// <param name="_yvalue">double</param>
        public void AddPoint(double _xvalue, double _yvalue)
        {
            Array.Resize(ref this.points, this.points.Length + 1);
            SVGPoint point = new SVGPoint(Math.Round(_xvalue, 2), Math.Round(_yvalue, 2));
            this.points[this.points.Length - 1] = point;
        }

        /// <summary>
        /// Add a point to the array
        /// </summary>
        /// <param name="_point">SVGPoint</param>
        public void AddPoint(SVGPoint _point)
        {
            this.AddPoint(_point.X, _point.Y);
        }

        /// <summary>
        /// Add points to the current array
        /// </summary>
        /// <param name="_points">Point[]</param>
        public void AddPoint(SVGPoint[] _points)
        {
            int pointLength = this.points.Length;
            Array.Resize(ref this.points, this.points.Length + _points.Length);
            for (int i = 0; i < _points.Length; i++)
            {
                this.points[pointLength] = _points[i];
                pointLength++;
            }
        }

        /// <summary>
        /// Get a XmlNode with all details of this object
        /// </summary>
        /// <returns>XmlNode</returns>
        public XmlNode ToXml()
        {
            XmlDocument xmlPolygon = new XmlDocument();
            xmlPolygon.LoadXml("<polygon></polygon>");
            XmlAttribute xmlColor = xmlPolygon.CreateAttribute("color");
            XmlAttribute xmlPoints = xmlPolygon.CreateAttribute("points");
            XmlAttribute xmlBorder = xmlPolygon.CreateAttribute("border");
            xmlColor.Value = ColorTranslator.ToHtml(this.color);
            xmlBorder.Value = this.Border.ToString().ToLower();

            if (this.points.Length > 0)
            {
                xmlPoints.Value = string.Empty;
                System.Globalization.CultureInfo ci = SVGObject.CI;

                if (this.points.Length > 0)
                {
                    int i = 0;
                    while (i < this.points.Length - 1)
                    {
                        xmlPoints.Value += string.Format(ci.NumberFormat, "{0},{1} ", this.points[i].X, this.points[i].Y);
                        i++;
                    }
                    xmlPoints.Value += string.Format(ci.NumberFormat, "{0},{1}", this.points[i].X, this.points[i].Y);
                }
            }

            xmlPolygon.FirstChild.Attributes.Append(xmlColor);
            xmlPolygon.FirstChild.Attributes.Append(xmlPoints);
            xmlPolygon.FirstChild.Attributes.Append(xmlBorder);
            return xmlPolygon.FirstChild;
        }

        #endregion Methods
    }
}