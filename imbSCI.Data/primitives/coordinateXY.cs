// --------------------------------------------------------------------------------------------------------------------
// <copyright file="coordinateXY.cs" company="imbVeles" >
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
// Project: imbSCI.Data
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Data.primitives
{
    using System;

    /// <summary>
    /// 2D selection information
    /// </summary>
    public class coordinateXY
    {
        private Int32 _x;

        /// <summary>
        /// x position
        /// </summary>
        public virtual Int32 x
        {
            get { return _x; }
            set { _x = value; }
        }

        private Int32 _y;

        /// <summary>
        ///
        /// </summary>
        public virtual Int32 y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="coordinateXY"/> class.
        /// </summary>
        public coordinateXY()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="coordinateXY"/> class.
        /// </summary>
        /// <param name="__x">The x.</param>
        /// <param name="__y">The y.</param>
        public coordinateXY(Int32 __x, Int32 __y)
        {
            x = __x;
            y = __y;
        }
    }
}