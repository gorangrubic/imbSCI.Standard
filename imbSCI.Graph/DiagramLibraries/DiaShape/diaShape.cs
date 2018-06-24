using imbSCI.Core.extensions.io;
using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Core.style.css;
using imbSCI.Data;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace imbSCI.Graph.DiagramLibraries.DiaShape
{
    /// <summary>
    /// Describes instance of Dia shape definition
    /// </summary>
    [XmlRoot("shape", Namespace = "http://www.daa.com.au/~james/dia-shape-ns")]
    public class diaShape
    {
        /// <summary>
        /// Additional XML namespaces that shape format uses
        /// </summary>
        /// <value>
        /// The XMLNS.
        /// </value>
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns { get; set; }

        /// <summary>
        /// Loads the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static diaShape Load(String path)
        {
            diaShape output = objectSerialization.loadObjectFromXML<diaShape>(path);

            return output;
        }

        /// <summary>
        /// Saves the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Save(String path)
        {
            objectSerialization.saveObjectToXML(this, path);
        }

        /// <summary>
        /// Saves the specified folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public String Save(folderNode folder, String filename = "", String description = "")
        {
            if (filename == "") filename = name.getFilename("shape");
            filename = filename.ensureEndsWith(".shape");
            String path = folder.pathFor(filename, Data.enums.getWritableFileMode.newOrExisting, description, true);

            objectSerialization.saveObjectToXML(this, path);
            return path;
        }

        public static cssEntryDefinition defaultStyleForSVGRender { get; set; } = new cssEntryDefinition("default", "fill:#FFFFFF; stroke:#000000; stroke-width:1;");

        /// <summary>
        /// Gets the SVG document that represents Dia shape, attached to the shape definition
        /// </summary>
        /// <param name="defaultStyle">The default style.</param>
        /// <returns></returns>
        public Svg.SvgDocument GetSVG(cssEntryDefinition defaultStyle = null)
        {
            SvgDocument svgDocument = new SvgDocument();

            String svgString = svg.OuterXml;

            if (defaultStyle == null) defaultStyle = defaultStyleForSVGRender;

            String inlineValue = defaultStyle.ToString(cssEntryDefinition.syntaxFormat.htmlStyleFormatInline);

            svgString = svgString.Replace("fill: default", inlineValue);

            throw new NotImplementedException();

            //svgDocument = SvgDocument.FromSvg<SvgDocument>(svgString);

            //if (svgDocument.Bounds.Left < 0)
            //{
            //    svgDocument.Width = Math.Abs(svgDocument.Bounds.Left) + svgDocument.Width;
            //    svgDocument.X = 0;
            //}

            return svgDocument;
        }

        /// <summary>
        /// Renders the icon of SVG shape, to given <c>folder</c> using icon name or shape name
        /// </summary>
        /// <param name="folder">The folder to save icon to</param>
        /// <param name="size">The size of rendered icon, in px</param>
        /// <param name="filename"></param>
        /// <returns>
        /// Path where icon was saved
        /// </returns>
        public String RenderIcon(folderNode folder, Int32 size = 22, Int32 margin = 3, String filename = "")
        {
            if (filename == "") filename = icon;
            if (filename.isNullOrEmpty())
            {
                filename = name.getFilename(".png");
            }
            return RenderIcon(folder.pathFor(filename, Data.enums.getWritableFileMode.newOrExisting, "Icon for Dia shape [" + name + "]"), size, margin);
        }

        /// <summary>
        /// Renders the icon of SVG shape, to given <c>path</c>
        /// </summary>
        /// <param name="path">The path to save icon to</param>
        /// <param name="size">The size of rendered icon, in px</param>
        /// <param name="margin">The margin - how much svg units to add on each side of the drawing</param>
        /// <param name="forceSquare">if set to <c>true</c> [force square].</param>
        /// <returns>
        /// Path where icon was saved
        /// </returns>
        public String RenderIcon(String path, Int32 size = 22, Int32 margin = 3, Boolean forceSquare = false)
        {
            String filename = Path.GetFileNameWithoutExtension(path);
            String fileext = Path.GetExtension(filename);

            ImageFormat format = path.GetImageFormatByExtension();

            SvgDocument svgDocument = GetSVG();

            svgDocument.ViewBox = new SvgViewBox(-margin, -margin, svgDocument.Width + (2 * margin), svgDocument.Height + (2 * margin));

            Int32 sizeW = size;
            Double ratio = 1;
            if (!forceSquare)
            {
                ratio = svgDocument.ViewBox.Height / svgDocument.ViewBox.Width;
            }
            Int32 sizeH = Convert.ToInt32(size * ratio);

            throw new NotImplementedException();

            //Bitmap bmp = svgDocument.Draw(sizeW, sizeH);

            //bmp.Save(path, format);
            return path;
        }

        /// <summary>
        /// Saves the SVG.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public String SaveSVG(folderNode folder, String filename = "")
        {
            if (filename == "") filename = name.getFilename(".svg");
            String p = folder.pathFor(filename, Data.enums.getWritableFileMode.newOrExisting, "Exported SVG of Dia shape [" + name + "]", true);

            throw new NotImplementedException();
            //  File.WriteAllText(p, GetSVG().GetXML());
            return p;
        }

        /// <summary>
        /// Gets the sheet object.
        /// </summary>
        /// <returns></returns>
        public diaSheetObject GetSheetObject()
        {
            diaSheetObject output = new diaSheetObject();
            output.name = name;
            output.descriptions = new xmlTextLocaleEntry[] { new xmlTextLocaleEntry() };
            output.descriptions[0].text = description;
            return output;
        }

        protected void deploy()
        {
            xmlns = new XmlSerializerNamespaces();
            xmlns.Add("svg", "http://www.w3.org/2000/svg");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="diaShape"/> class.
        /// </summary>
        public diaShape()
        {
            deploy();
        }

        public diaShape(String _name, String _icon)
        {
            deploy();
            name = _name;
            icon = _icon;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [XmlElement]
        public String name { get; set; } = "";

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [XmlElement]
        public String description { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>
        /// The icon.
        /// </value>
        [XmlElement]
        public String icon { get; set; } = "";

        /// <summary>
        /// Adds the connection.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="main">if set to <c>true</c> [main].</param>
        /// <returns></returns>
        public diaConnectionPoint AddConnection(Double x, Double y, Boolean main = false)
        {
            diaConnectionPoint d = new diaConnectionPoint(x, y, main);
            connections.Add(d);
            return d;
        }

        /// <summary>
        /// Gets or sets the connections.
        /// </summary>
        /// <value>
        /// The connections.
        /// </value>
        [XmlArray("connections")]
        [XmlArrayItem("point")]
        public List<diaConnectionPoint> connections { get; set; } = new List<diaConnectionPoint>();

        /// <summary>
        /// Gets or sets the aspectratio.
        /// </summary>
        /// <value>
        /// The aspectratio.
        /// </value>
        [XmlElement]
        public diaShapeAspectRatio aspectratio { get; set; }

        /// <summary>
        /// Gets or sets the textbox.
        /// </summary>
        /// <value>
        /// The textbox.
        /// </value>
        [XmlElement]
        public diaTextBox textbox { get; set; } = new diaTextBox();

        /// <summary>
        /// Additional attributes declared in the shape
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        [XmlArray(ElementName = "ext_attributes")]
        [XmlArrayItem(ElementName = "ext_attribute")]
        public List<diaShapeAttribute> attributes { get; set; }

        /// <summary>
        /// Scalable Vector Graphics element
        /// </summary>
        /// <value>
        /// The SVG.
        /// </value>
        [XmlAnyElement(Name = "svg", Namespace = "http://www.w3.org/2000/svg")]
        //    [XmlElement(ElementName ="svg", Namespace = "http://www.w3.org/2000/svg")]
        public XmlElement svg { get; set; }
    }
}