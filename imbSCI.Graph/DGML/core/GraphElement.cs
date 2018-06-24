// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphElement.cs" company="imbVeles" >
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
using imbSCI.Data.interfaces;

//using System.Windows;
using imbSCI.Graph.DGML.enums;
using System;
using System.Xml.Serialization;

#if NETFULL

using imbSCI.Core.style;

#endif

namespace imbSCI.Graph.DGML.core
{
    /// <summary>
    ///
    /// </summary>
    public abstract class GraphElement : IGraphElement
    {
        [XmlAttribute]
        public String Stroke { get; set; }

        [XmlAttribute]
        public Int32 StrokeThinkness { get; set; } = 1;

        [XmlAttribute]
        public String StrokeDashArray { get; set; }

        [XmlAttribute]
        public String Label { get; set; } = "";

        [XmlAttribute]
        public String Category { get; set; }

        [XmlAttribute]
        public GroupEnum Group { get; set; } = GroupEnum.Collapsed;

        [XmlAttribute]
        public imbSCI.Core.process.Visibility Visibility { get; set; } = imbSCI.Core.process.Visibility.Visible;

        private String _id;

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public virtual String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        string IObjectWithName.name { get { return Label; } set { Label = value; } }
    }
}