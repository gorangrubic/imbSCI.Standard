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
using imbSCI.Core.extensions.io;
using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Data.enums;
using imbSCI.Data.interfaces;
using imbSCI.Graph.DGML.collections;
using imbSCI.Graph.DGML.core;
using imbSCI.Graph.DGML.enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace imbSCI.Graph.DGML
{
    [XmlRoot(Namespace = "http://schemas.microsoft.com/vs/2009/dgml",
     ElementName = "DirectedGraph",
     IsNullable = true)]
    public class DirectedGraph : IObjectWithName
    {
        [XmlIgnore]
        public List<String> ConversionErrors { get; set; } = new List<string>();

        public DirectedGraph()
        {
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Loads the specified path.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static T Load<T>(String path) where T : DirectedGraph
        {
            FileInfo fi = path.getWritableFile(getWritableFileMode.existing);
            if (fi.Exists)
            {
                return objectSerialization.loadObjectFromXML<T>(path);
            }
            return default(T);
        }

        /// <summary>
        /// Saves the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mode">The mode.</param>
        public void Save(String path, getWritableFileMode mode = Data.enums.getWritableFileMode.overwrite)
        {
            if (!path.EndsWith(".dgml"))
            {
                path = path + ".dgml";
            }

            FileInfo fi = path.getWritableFile(mode);

            objectSerialization.saveObjectToXML(this, fi.FullName);
            if (ConversionErrors.Any())
            {
                String errFileName = Path.GetFileNameWithoutExtension(path) + "_conversionErrors.txt";
                folderNode fl = fi.Directory;
                String errFilePath = fl.pathFor(errFileName, getWritableFileMode.overwrite);
                File.WriteAllLines(errFilePath, ConversionErrors);
            }
        }

        /// <summary>
        /// Called when [before save].
        /// </summary>
        /// <param name="folder">The folder.</param>
        public virtual void OnBeforeSave(folderNode folder)
        {
        }

        /// <summary>
        /// Called when [after load].
        /// </summary>
        /// <param name="folder">The folder.</param>
        public virtual void OnAfterLoad(folderNode folder)
        {
        }

        /// <summary>
        /// Gets or sets the graph direction.
        /// </summary>
        /// <value>
        /// The graph direction.
        /// </value>
        [XmlAttribute]
        public GraphDirectionEnum GraphDirection { get; set; } = GraphDirectionEnum.TopToBottom;

        /// <summary>
        /// Gets or sets the layout.
        /// </summary>
        /// <value>
        /// The layout.
        /// </value>
        [XmlAttribute]
        public GraphLayoutEnum Layout { get; set; } = GraphLayoutEnum.Sugiyama;

        /// <summary>
        /// Gets or sets the neighborhood distance.
        /// </summary>
        /// <value>
        /// The neighborhood distance.
        /// </value>
        [XmlAttribute]
        public Int32 NeighborhoodDistance { get; set; } = 25;

        /// <summary>
        /// Gets or sets the background.
        /// </summary>
        /// <value>
        /// The background.
        /// </value>
        [XmlAttribute]
        public String Background { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [XmlAttribute]
        public String Title { get; set; }

        [XmlIgnore]
        string IObjectWithName.name
        {
            get
            {
                return Title;
            }
            set
            {
                Title = value;
            }
        }

        /// <summary>
        /// Gets the linked.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="inverse">if set to <c>true</c> [inverse].</param>
        /// <returns></returns>
        public List<Node> GetLinked(Node node, Boolean inverse = false)
        {
            List<Node> output = new List<Node>();
            foreach (Link lnks in Links.Where(x => x.Source == node.Id))
            {
                if (Nodes.Any(x => x.Id == lnks.Target))
                {
                    output.Add(Nodes.First(x => x.Id == lnks.Target));
                    //T cNode = new T();
                    //cNode.name = lnks.Target;
                    //nodeNames.Add(lnks.Target);
                    //newnext.Add(source.Nodes.First(x => x.Id == lnks.Target));

                    //tNode.Add(cNode);
                }
            }
            return output;
        }

        public LinkCollection Links { get; set; } = new LinkCollection();

        public NodeCollection Nodes { get; set; } = new NodeCollection();

        public PropertyList Properties { get; set; } = new PropertyList();

        public CategoryCollection Categories { get; set; } = new CategoryCollection();
        //    string IObjectWithName.name {
        //        get {
        //            return Title; }
        //        set
        //        {
        //            Title = value;
        //        }
        //    }
    }
}