using imbSCI.Core.files.folders;
using imbSCI.Data;
using imbSCI.Graph.Graphics.SvgAPI.Containers;
using imbSCI.Graph.Graphics.SvgAPI.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace imbSCI.Graph.Graphics.SvgAPI
{
    /// <summary>
    /// imbSCI.Graph SVG document model
    /// </summary>
    /// <seealso cref="imbSCI.Graph.Graphics.SvgAPI.svgContainerElement" />
    public class svgDocument : svgContainerElement
    {
        public svgDocument()
        {
            deploy();
            attributes.Set("xmlns", "http://www.w3.org/2000/svg");
        }

        protected void deploy()
        {
            attributes.Set("xmlns", "http://www.w3.org/2000/svg");
            attributes.Set("xmlns:xlink", "http://www.w3.org/1999/xlink");
            attributes.Set("xmlns:xml", "http://www.w3.org/XML/1998/namespace");
            attributes.Set("xml:space", "default");
            attributes.Set("version", "1.1");
        }

        public svgDocument(Int32 width, Int32 height)
        {
            deploy();
            point.width = width;
            point.height = height;
        }

        [XmlIgnore]
        public override String name { get { return "svg"; } }

        /// <summary>
        /// Saves the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Save(String path, Boolean formatString = false)
        {
            XmlNode xdoc = ToXml();

            String o = xdoc.OuterXml;
            if (formatString)
            {
                o = o.Replace(">", ">" + Environment.NewLine);
            }

            File.WriteAllText(path, o);
        }

        /// <summary>
        /// Saves the specified folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="description">The description.</param>
        /// <param name="formatString">if set to <c>true</c> [format string].</param>
        public void Save(folderNode folder, String filename, String description = "", Boolean formatString = false)
        {
            filename = filename.add("svg", ".");
            String p = folder.pathFor(filename, Data.enums.getWritableFileMode.overwrite, description, true);
            Save(p, formatString);
        }

        /// <summary>
        /// Gets or sets the definitions.
        /// </summary>
        /// <value>
        /// The definitions.
        /// </value>
        [XmlIgnore]
        public List<svgGraphicElementBase> definitions { get; protected set; } = new List<svgGraphicElementBase>();

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public override XmlNode ToXml()
        {
            var output = base.ToXml();

            XmlNode defs = output.OwnerDocument.CreateNode(XmlNodeType.Element, "defs", output.NamespaceURI);

            foreach (var d in definitions)
            {
                defs.AppendChild(d.ToXml());
            }

            output.PrependChild(defs);
            return output;
        }
    }
}