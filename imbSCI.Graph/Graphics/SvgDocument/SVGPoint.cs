// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SVGPoint.cs" company="imbVeles" >
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

///// This code is based on:
//AUTHOR	: GERARD CASTELLÓ
//DATE		: 17/JUN/2010

using imbSCI.Graph.Graphics.SvgAPI;
using imbSCI.Graph.Graphics.SvgAPI.Core;
using System;

namespace imbSCI.Graph.Graphics.SvgDocument
{
    /// <summary>
    /// Structure, representing a point in SVG document
    /// </summary>
    public class SVGPoint : ISVGInlineArgument
    {
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override String ToString()
        {
            return x.ToString() + "," + y.ToString();
        }

        /// <summary>
        /// Froms the string.
        /// </summary>
        /// <param name="input">The input.</param>
        public void FromString(String input)
        {
            var p = input.GetPointFromString();
            if (p != null)
            {
                X = p.X;
                Y = p.Y;
            }
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SVGPoint"/> class.
        /// </summary>
        /// <param name="_x">The x.</param>
        /// <param name="_y">The y.</param>
        public SVGPoint(double _x, double _y)
        {
            this.x = _x;
            this.y = _y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SVGPoint"/> class.
        /// </summary>
        public SVGPoint() { }

        #endregion Constructors

        #region Attributes

        private double x;
        private double y;

        #endregion Attributes

        #region Properties

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public double X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        #endregion Properties
    }
}