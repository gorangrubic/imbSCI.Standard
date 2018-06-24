// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Rectangle.cs" company="imbVeles" >
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

using System.Drawing;
using System.Xml;

namespace imbSCI.Graph.Graphics.SvgDocument
{
    public class Rectangle : SVGObject, ISVGTranform
    {
        #region Attributes

        private double width;
        private double height;
        private Color color;
        private bool border;

        #endregion Attributes

        #region Properties

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

        public double Height
        {
            get
            {
                return this.height;
            }
        }

        public double Width
        {
            get
            {
                return this.width;
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

        public Rectangle(double _width, double _height)
        {
            this.width = _width;
            this.height = _height;
            this.color = Color.White;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get a XmlNode with all details of this object
        /// </summary>
        /// <returns>XmlNode</returns>
        public XmlNode ToXml()
        {
            XmlDocument xmlRectangle = new XmlDocument();
            xmlRectangle.LoadXml("<rect></rect>");
            xmlRectangle = base.GetNode(xmlRectangle);
            XmlAttribute xmlColor = xmlRectangle.CreateAttribute("color");
            XmlAttribute xmlWidth = xmlRectangle.CreateAttribute("width");
            XmlAttribute xmlHeight = xmlRectangle.CreateAttribute("height");
            XmlAttribute xmlBorder = xmlRectangle.CreateAttribute("border");
            xmlColor.Value = ColorTranslator.ToHtml(this.color);
            System.Globalization.CultureInfo ci = SVGObject.CI;
            xmlWidth.Value = this.width.ToString(ci.NumberFormat);
            xmlHeight.Value = this.height.ToString(ci.NumberFormat);
            xmlBorder.Value = this.border.ToString().ToLower();
            xmlRectangle.FirstChild.Attributes.Append(xmlColor);
            xmlRectangle.FirstChild.Attributes.Append(xmlWidth);
            xmlRectangle.FirstChild.Attributes.Append(xmlHeight);
            xmlRectangle.FirstChild.Attributes.Append(xmlBorder);
            return xmlRectangle.FirstChild;
        }

        #endregion Methods
    }
}