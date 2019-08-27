// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectedGraph.cs" company="imbVeles" >
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
using imbSCI.Core.data;
using imbSCI.Core.data.descriptors;
using imbSCI.Core.extensions.io;
using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Data.enums;
using imbSCI.Data.interfaces;
using imbSCI.Graph.DGML.collections;
using imbSCI.Graph.DGML.core;
using imbSCI.Graph.DGML.enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace imbSCI.Graph.DGML
{
    /// <summary>
    /// Graph expansion with source objects stored in <see cref="Sources"/>, when <see cref="DirectedGraphExtensions.Populate{TNodeSource, TChildNodeSource}(DirectedGraph, IEnumerable{TNodeSource}, Func{TNodeSource, IEnumerable{TChildNodeSource}}, Func{TNodeSource, string}, Func{TNodeSource, string}, Func{TChildNodeSource, string}, Func{TChildNodeSource, string}, bool, bool)"/> extensions are used for graph construction
    /// </summary>
    /// <seealso cref="imbSCI.Graph.DGML.DirectedGraph" />
    /// <seealso cref="DirectedGraphExtensions"/>
    [XmlRoot(Namespace = "http://schemas.microsoft.com/vs/2009/dgml",
       ElementName = "DirectedGraph",
       IsNullable = true)]
    public class DirectedGraphWithSourceData :DirectedGraph
    {
        
        public ElementSourceDictionary Sources { get; set; } = new ElementSourceDictionary();

        public DirectedGraphWithSourceData():base()
        {

        }
    }
}