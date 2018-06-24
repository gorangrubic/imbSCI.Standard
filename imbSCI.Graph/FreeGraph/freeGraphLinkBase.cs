// --------------------------------------------------------------------------------------------------------------------
// <copyright file="freeGraphLinkBase.cs" company="imbVeles" >
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
using System;

namespace imbSCI.Graph.FreeGraph
{
    public class freeGraphLinkBase
    {
        public freeGraphLinkBase()
        {
        }

        public Int32 type { get; set; } = 0;

        public Double weight { get; set; } = 1;

        public String nodeNameA { get; set; } = "";

        public String nodeNameB { get; set; } = "";

        public String linkLabel { get; set; } = "";

        public freeGraphLinkBase GetClone()
        {
            freeGraphLinkBase link = new freeGraphLinkBase();
            link.type = type;
            link.weight = weight;
            link.nodeNameA = nodeNameA;
            link.nodeNameB = nodeNameB;
            link.linkLabel = linkLabel;
            return link;
        }
    }
}