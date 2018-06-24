// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DotNode.cs" company="imbVeles" >
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
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;

namespace imbSCI.Graph.DOT
{
    public class DotNode : Node
    {
        [GraphAttribute("shape", DotNodeShape.Ellipse)]
        public DotNodeShape Shape { get; set; } = DotNodeShape.Ellipse;

        [GraphAttribute("label", "\n")]
        public new string Label
        {
            get { return base.Label; }
            set { base.Label = value; }
        }

        [GraphAttribute("fillcolor", DotColor.Lightgrey)]
        public DotColor FillColor { get; set; } = DotColor.Lightgrey;

        [GraphAttribute("fontcolor", DotColor.Black)]
        public DotColor FontColor { get; set; } = DotColor.Black;

        [GraphAttribute("style", DotNodeStyle.Default)]
        public DotNodeStyle Style { get; set; } = DotNodeStyle.Default;

        [GraphAttribute("height", 0.5f)]
        public float Height { get; set; } = 0.5f;

        public DotNode()
        {
        }

        public DotNode(string name)
        {
            this.Id = name.GetIDFromLabel();
            this.Label = name;
        }
    }
}