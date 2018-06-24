// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Line.cs" company="imbVeles" >
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
    public class Line : SVGObject, ISVGTranform
    {
        #region Attributes

        private double x1;
        private double x2;
        private double y1;
        private double y2;
        private decimal width;
        private Color color;
        private bool dashed;

        #endregion Attributes

        #region Properties

        public double X1
        {
            get
            {
                return Math.Round(this.x1, 2);
            }
        }

        public double X2
        {
            get
            {
                return Math.Round(this.x2, 2);
            }
        }

        public double Y1
        {
            get
            {
                return Math.Round(this.y1, 2);
            }
        }

        public double Y2
        {
            get
            {
                return Math.Round(this.y2, 2);
            }
        }

        public decimal Width
        {
            get
            {
                if (this.width == 0)
                    this.width = (decimal)0.5;
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }

        public Color Color
        {
            get
            {
                if (this.color.IsEmpty)
                    this.color = Color.Black;
                return this.color;
            }
            set
            {
                this.color = value;
            }
        }

        public bool Dashed
        {
            get
            {
                return this.dashed;
            }
            set
            {
                this.dashed = value;
            }
        }

        #endregion Properties

        #region Constructor

        public Line(SVGPoint _source, SVGPoint _destination)
            : this(_source.X, _source.Y, _destination.X, _destination.Y)
        {
        }

        public Line(double _x1, double _y1, double _x2, double _y2)
        {
            this.x1 = _x1;
            this.x2 = _x2;
            this.y1 = _y1;
            this.y2 = _y2;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get a XmlNode with all details of this object.
        /// </summary>
        /// <returns>XmlNode</returns>
        public XmlNode ToXml()
        {
            System.Globalization.CultureInfo ci = SVGObject.CI;
            XmlDocument xmlLine = new XmlDocument();
            xmlLine.LoadXml("<line></line>");
            XmlAttribute xmlX1 = xmlLine.CreateAttribute("x1");
            XmlAttribute xmlX2 = xmlLine.CreateAttribute("x2");
            XmlAttribute xmlY1 = xmlLine.CreateAttribute("y1");
            XmlAttribute xmlY2 = xmlLine.CreateAttribute("y2");
            XmlAttribute xmlColor = xmlLine.CreateAttribute("color");
            XmlAttribute xmlWidth = xmlLine.CreateAttribute("width");
            XmlAttribute xmlDash = xmlLine.CreateAttribute("dashed");
            xmlX1.Value = this.X1.ToString(ci.NumberFormat);
            xmlX2.Value = this.X2.ToString(ci.NumberFormat);
            xmlY1.Value = this.Y1.ToString(ci.NumberFormat);
            xmlY2.Value = this.Y2.ToString(ci.NumberFormat);
            xmlColor.Value = ColorTranslator.ToHtml(this.Color);
            xmlWidth.Value = this.Width.ToString(ci.NumberFormat);
            xmlDash.Value = this.dashed.ToString().ToLower();
            xmlLine.FirstChild.Attributes.Append(xmlX1);
            xmlLine.FirstChild.Attributes.Append(xmlX2);
            xmlLine.FirstChild.Attributes.Append(xmlY1);
            xmlLine.FirstChild.Attributes.Append(xmlY2);
            xmlLine.FirstChild.Attributes.Append(xmlColor);
            xmlLine.FirstChild.Attributes.Append(xmlWidth);
            xmlLine.FirstChild.Attributes.Append(xmlDash);
            return xmlLine.FirstChild;
        }

        #endregion Methods
    }
}