// --------------------------------------------------------------------------------------------------------------------
// <copyright file="diagramElementBase.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using imbSCI.Data.interfaces;

namespace imbSCI.Graph.Diagram.core
{
    /// <summary>
    /// Base class for all elements within <see cref="diagramModel" />
    /// </summary>
    /// <seealso cref="IObjectWithName" />
    public abstract class diagramElementBase : IObjectWithName, IObjectWithNameAndDescription
    {
        /// <summary>
        /// UID name of this diagram element - for Links no need to be unique
        /// </summary>
        public string name { get; set; } = "";

        /// <summary>
        /// Human-readable description text to appear inside or above diagram element
        /// </summary>
        public string description { get; set; } = "";

        /// <summary>
        /// Associated color role - good idea to be sinchronized with: <see cref="imbSCI.Core.reporting.colors.acePaletteRole"/>
        /// </summary>
        public int color { get; set; } = 1;

        /// <summary>
        /// Associated importance level - good idea to be sinchronized with: <see cref="imbSCI.Core.reporting.colors.acePaletteVariationRole"/>
        /// </summary>
        public int importance { get; set; } = 0;

        /// <summary>
        /// Object that is logicaly related to this diagram element
        /// </summary>
        public IObjectWithName relatedObject { get; set; } = null;

        /// <summary>
        /// Reference to <see cref="diagramModel"/> that contains this element
        /// </summary>
        public diagramModel parent { get; set; }
    }
}