// --------------------------------------------------------------------------------------------------------------------
// <copyright file="selectZone.cs" company="imbVeles" >
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
// Project: imbSCI.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Core.reporting.zone
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// Defines selection zone
    /// </summary>
    public struct selectZone
    {

        public selectZone(Int32 _x, Int32 _y, Int32 _w, Int32 _h)
        {
            x = _x;
            y = _y;
            width = _w;
            height = _h;
            //isDefined = true;
        }

        /// <summary>
        /// Horizontalna pozicija
        /// </summary>
        [XmlAttribute]
        public Int32 x;

        /// <summary>
        /// Vertikalna pozicija
        /// </summary>
        [XmlAttribute]
        public Int32 y;

        /// <summary>
        /// Sirina
        /// </summary>
        [XmlAttribute]
        public Int32 width;

        /// <summary>
        /// Visina
        /// </summary>
        [XmlAttribute]
        public Int32 height;

        [XmlIgnore]
        public Boolean isDefined
        {
            get { return (width > 0); }
        }
    }
}