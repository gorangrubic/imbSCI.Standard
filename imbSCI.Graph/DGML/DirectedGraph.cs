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

    [XmlRoot(Namespace = "http://schemas.microsoft.com/vs/2009/dgml",
     ElementName = "DirectedGraph",
     IsNullable = true)]
    public class DirectedGraph : IObjectWithName, IXmlSerializable, IElementWithProporties, IFreeGraph
    {
        [XmlIgnore]
        public List<String> ConversionErrors { get; set; } = new List<string>();

        public DirectedGraph()
        {
            Nodes = new NodeCollection(this);
            Links = new LinkCollection(this);

            PropertyDeclaration = new PropertyList();
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Id { get; set; } = "DGML";

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
        public void Save(String path, getWritableFileMode mode = getWritableFileMode.overwrite)
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

        
        public XmlSchema GetSchema()
        {
            return null;
        }


        public void ReadXml(XmlReader reader)
        {

            TypeXmlAttributes xmlInfo_graph = TypeDescriptorTools.GetXmlInfo(typeof(DirectedGraph));
            TypeXmlAttributes xmlInfo_node = TypeDescriptorTools.GetXmlInfo(typeof(Node));
            TypeXmlAttributes xmlInfo_link = TypeDescriptorTools.GetXmlInfo(typeof(Link));
            TypeXmlAttributes xmlInfo_category = TypeDescriptorTools.GetXmlInfo(typeof(Category));
            TypeXmlAttributes xmlInfo_property = TypeDescriptorTools.GetXmlInfo(typeof(Property));

            

            DirectedGraphXmlExtensions.ReadElementAttributes(this, reader, xmlInfo_graph);

            reader.Read();

            while(reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.Name)
                        {
                            case nameof(Node):
                                var tmpNode = new Node();
                                tmpNode.ReadXml(reader, xmlInfo_node);
                                Nodes.Add(tmpNode);
                                break;
                            case nameof(Link):
                                var newLink = new Link();

                                newLink.ReadXml(reader, xmlInfo_link);
                                Links.Add(newLink);
                                break;
                            case nameof(Category):
                                var newCategory = new Category();
                                newCategory.ReadXml(reader, xmlInfo_category);
                                Categories.Add(newCategory);
                                break;
                            case nameof(Property):
                                var newProperty = new Property();
                                newProperty.ReadXml(reader, xmlInfo_property);
                                PropertyDeclaration.Add(newProperty);
                                break;

                            //case nameof(Nodes):
                            //    do
                            //    {
                            //        var newNode = new Node();
                            //        newNode.ReadXml(reader, xmlInfo_node);
                            //        Nodes.Add(newNode);

                            //    } while (reader.ReadToNextSibling(nameof(Node)));

                            //    break;
                            //case nameof(Links):
                            //    do
                            //    {
                            //        var newLink = new Link();

                            //        newLink.ReadXml(reader, xmlInfo_link);
                            //        Links.Add(newLink);

                            //    } while (reader.ReadToNextSibling(nameof(Link)));
                            //    reader.ReadEndElement();
                            //    break;
                            //case nameof(Categories):
                            //    do
                            //    {
                            //        var newCategory = new Category();
                            //        newCategory.ReadXml(reader, xmlInfo_category);
                            //        Categories.Add(newCategory);

                            //    } while (reader.ReadToNextSibling(nameof(Category)));
                            //    break;
                            //case nameof(Properties):
                            //    do
                            //    {
                            //        var newProperty = new Property();
                            //        newProperty.ReadXml(reader, xmlInfo_property);
                            //        PropertyDeclaration.Add(newProperty);

                            //    } while (reader.ReadToNextSibling(nameof(Property)));

                            //    break;
                            default:

                                break;
                        }
                        break;
                    default:
                    case XmlNodeType.Text:
                        break;
                    case XmlNodeType.CDATA:
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        break;
                    case XmlNodeType.Comment:
                        break;
                    case XmlNodeType.XmlDeclaration:
                        break;
                    case XmlNodeType.Document:
                        break;
                    case XmlNodeType.DocumentType:
                        break;
                    case XmlNodeType.EntityReference:
                        break;
                    case XmlNodeType.EndElement:
                        break;
                }
            }

            //reader.ReadStartElement(nameof(Nodes));
            
            //reader.ReadEndElement();


            //reader.ReadStartElement(nameof(Links));
           

            //reader.
            //reader.ReadStartElement(nameof(Categories));
            
            //reader.ReadEndElement();

            //reader.ReadStartElement(nameof(Properties));
          
            //reader.ReadEndElement();

        }

        public void WriteXml(XmlWriter writer)
        {

            TypeXmlAttributes xmlInfo_graph = TypeDescriptorTools.GetXmlInfo(typeof(DirectedGraph));
            TypeXmlAttributes xmlInfo_node = TypeDescriptorTools.GetXmlInfo(typeof(Node));
            TypeXmlAttributes xmlInfo_link = TypeDescriptorTools.GetXmlInfo(typeof(Link));
            TypeXmlAttributes xmlInfo_category = TypeDescriptorTools.GetXmlInfo(typeof(Category));
            TypeXmlAttributes xmlInfo_property = TypeDescriptorTools.GetXmlInfo(typeof(Property));

            
            

          //  writer.WriteStartDocument();

            //writer.WriteStartElement(nameof(DirectedGraph));

                this.WriteElementAttributes(writer, xmlInfo_graph);

                writer.WriteStartElement(nameof(Nodes));
                foreach (Node n in Nodes)
                {
                    n.WriteXml(writer, xmlInfo_node);
                }
                writer.WriteEndElement();

                writer.WriteStartElement(nameof(Links));
                foreach (Link n in Links)
                {
                    n.WriteXml(writer, xmlInfo_link);
                }
                writer.WriteEndElement();

                writer.WriteStartElement(nameof(Categories));
                foreach (Category n in Categories)
                {
                    n.WriteXml(writer, xmlInfo_category);
                }
                writer.WriteEndElement();

                writer.WriteStartElement(nameof(Properties));
                foreach (Property n in PropertyDeclaration)
                {
                    n.WriteXml(writer, xmlInfo_property);
                }
                writer.WriteEndElement();

         //   writer.WriteEndElement();

         //   writer.WriteEndDocument();
        }

        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        [XmlIgnore]
        public LinkCollection Links { get; set; } 

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        [XmlIgnore]
        public NodeCollection Nodes { get; set; } 

       

        /// <summary>
        /// Declaration of properties
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public PropertyList PropertyDeclaration { get; set; } = new PropertyList();

        [XmlIgnore]
        public CategoryCollection Categories { get; set; } = new CategoryCollection();

        [XmlIgnore]
        public reportExpandedData Properties { get; set; } = new reportExpandedData();

        IEnumerable<IFreeGraphNode> IFreeGraph.Nodes => Nodes;

        IEnumerable<IFreeGraphLink> IFreeGraph.Links => Links;

        string IObjectWithUID.UID => Id;
    }
}