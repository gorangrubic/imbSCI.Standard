// --------------------------------------------------------------------------------------------------------------------
// <copyright file="diagramNode.cs" company="imbVeles" >
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

namespace imbSCI.DataComplex.diagram.core
{
    using imbSCI.DataComplex.diagram.enums;

#pragma warning disable CS1574 // XML comment has cref attribute 'diagramElementBase' that could not be resolved
    /// <summary>
    /// Node that is part of the <see cref="diagramModel"/>
    /// </summary>
    /// <seealso cref="imbReportingCore.diagram.diagramElementBase" />
    public class diagramNode : diagramElementBase
#pragma warning restore CS1574 // XML comment has cref attribute 'diagramElementBase' that could not be resolved
    {
        /// <summary>
        ///
        /// </summary>
        public diagramNodeShapeEnum shapeType { get; set; } = diagramNodeShapeEnum.normal;

        internal diagramNode()
        {
        }
    }
}