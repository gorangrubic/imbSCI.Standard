// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SVGObject.cs" company="imbVeles" >
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

////// based on code from
//AUTHOR	: GERARD CASTELLÓ
//DATE		: 17/JUN/2010

using System.Xml;

namespace imbSCI.Graph.Graphics.SvgDocument
{
    /// <summary>
    /// Base class for SVG objectg
    /// </summary>
    public class SVGObject
    {
        #region Attributes

        protected SVGPoint point = new SVGPoint();

        #endregion Attributes

        #region Properties

        public static System.Globalization.CultureInfo CI
        {
            get
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
                System.Globalization.NumberFormatInfo ni = new System.Globalization.NumberFormatInfo();
                ni.NumberDecimalDigits = 2;
                ci.NumberFormat = ni;
                return ci;
            }
        }

        public double X
        {
            get
            {
                return this.point.X;
            }
            set
            {
                this.point.X = value;
            }
        }

        public double Y
        {
            get
            {
                return this.point.Y;
            }
            set
            {
                this.point.Y = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get a XmlDocument with the object's main details
        /// </summary>
        /// <param name="xmlObject">XmlDocument</param>
        /// <returns>XmlDocument</returns>
        protected XmlDocument GetNode(XmlDocument xmlObject)
        {
            XmlAttribute xmlX = xmlObject.CreateAttribute("x");
            XmlAttribute xmlY = xmlObject.CreateAttribute("y");
            xmlX.Value = this.X.ToString(CI.NumberFormat);
            xmlY.Value = this.Y.ToString(CI.NumberFormat);
            xmlObject.FirstChild.Attributes.Append(xmlX);
            xmlObject.FirstChild.Attributes.Append(xmlY);
            return xmlObject;
        }

        #endregion Methods
    }
}