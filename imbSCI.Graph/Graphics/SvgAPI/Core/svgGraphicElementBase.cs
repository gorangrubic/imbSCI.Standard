using imbSCI.Core.extensions.data;
using imbSCI.Core.reporting.zone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace imbSCI.Graph.Graphics.SvgAPI.Core
{
    /// <summary>
    /// Basic graphic element: described by coordinates and style parameters
    /// </summary>
    public abstract class svgGraphicElementBase
    {
        /// <summary>
        /// XML/SVG node name
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public abstract String name { get; }

        public String textContent { get; set; } = "";

        /// <summary>
        /// Optional identifier for SVG element
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public String id { get; set; } = "";

        ///// <summary>
        ///// To the XML string.
        ///// </summary>
        ///// <returns></returns>
        //public String ToXmlString()
        //{
        //    return ToXml().OuterXml;
        //}

        public abstract XmlNode ToXml();

        public String ToXmlString()
        {
            return ToXml().OuterXml;
        }

        /// <summary>
        /// Get a XmlNode with all details of this object
        /// </summary>
        /// <returns>
        /// XmlNode
        /// </returns>
        protected XmlNode ToXmlBase()
        {
            XmlDocument element = new XmlDocument(); //output.CreateElement(name);

            String xmlString = ToXmlOpenTag() + Environment.NewLine + textContent + Environment.NewLine + ToXmlCloseTag();
            try
            {
                element.LoadXml(xmlString);
            }
            catch (Exception ex)
            {
            }

            return element.FirstChild;
        }

        /// <summary>
        /// To the XML close tag.
        /// </summary>
        /// <returns></returns>
        protected String ToXmlCloseTag()
        {
            return "</" + name + ">" + Environment.NewLine;
        }

        /// <summary>
        /// To the XML open tag.
        /// </summary>
        /// <returns></returns>
        protected String ToXmlOpenTag()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<" + name + " ");

            attributes.Set(nameof(id), id);

            if (x != Double.MinValue)
            {
                attributes.Set(nameof(x), x.ToString());
            }

            if (y != Double.MinValue)
            {
                attributes.Set(nameof(y), y.ToString());
            }

            if (point.width != Double.MinValue)
            {
                attributes.Set(nameof(point.width), point.width.ToString());
            }

            if (point.height != Double.MinValue)
            {
                attributes.Set(nameof(point.height), point.height.ToString());
            }

            if (style.Any)
            {
                sb.Append("style =\"" + style.ToString() + "\" ");
            }

            if (classes.Any())
            {
                sb.Append("class =\"" + classes.toCsvInLine(" ") + "\" ");
            }

            if (attributes.Any)
            {
                sb.Append(attributes.ToString() + " ");
            }

            return sb.ToString() + " >";
        }

        protected svgGraphicElementBase()
        {
        }

        /// <summary>
        /// Sets the view box.
        /// </summary>
        /// <param name="box">The box.</param>
        public void SetViewBox(selectRangeArea box = null)
        {
            if (box == null)
            {
                box = point;
            }

            viewbox = box;
        }

        /// <summary>
        /// Removes the view box.
        /// </summary>
        public void RemoveViewBox()
        {
            viewbox = null;
        }

        /// <summary>
        /// Gets or sets the viewbox.
        /// </summary>
        /// <value>
        /// The viewbox.
        /// </value>
        public selectRangeArea viewbox { get; set; } = null;

        /// <summary>
        /// Gets or sets the point.
        /// </summary>
        /// <value>
        /// The point.
        /// </value>
        public selectRangeArea point { get; set; } = new selectRangeArea(Int32.MinValue, Int32.MinValue);

        // public selectRangeArea area { get; set; } = new selectRangeArea(0, 0, 0, 0);

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public Int32 x
        {
            get
            {
                return this.point.x;
            }
            set
            {
                this.point.x = value;
            }
        }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public Int32 y
        {
            get
            {
                return this.point.y;
            }
            set
            {
                this.point.y = value;
            }
        }

        //public static System.Globalization.CultureInfo CI
        //{
        //    get
        //    {
        //        System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
        //        System.Globalization.NumberFormatInfo ni = new System.Globalization.NumberFormatInfo();
        //        ni.NumberDecimalDigits = 2;
        //        ci.NumberFormat = ni;
        //        return ci;
        //    }
        //}

        ///// <summary>
        ///// Get a XmlDocument with the object's main details
        ///// </summary>
        ///// <param name="xmlObject">XmlDocument</param>
        ///// <returns>XmlDocument</returns>
        //protected XmlDocument GetNode(XmlDocument xmlObject)
        //{
        //    XmlAttribute xmlX = xmlObject.CreateAttribute("x");
        //    XmlAttribute xmlY = xmlObject.CreateAttribute("y");
        //    xmlX.Value = this.X.ToString(CI.NumberFormat);
        //    xmlY.Value = this.Y.ToString(CI.NumberFormat);
        //    xmlObject.FirstChild.Attributes.Append(xmlX);
        //    xmlObject.FirstChild.Attributes.Append(xmlY);
        //    return xmlObject;
        //}

        /// <summary>
        /// Provides style information
        /// </summary>
        /// <value>
        /// The style.
        /// </value>
        public inlineDictionary style { get; set; } = new inlineDictionary(inlineDictionary.styleFormat.htmlStyleFormat);

        /// <summary>
        /// Set of additional attributes
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public inlineDictionary attributes { get; set; } = new inlineDictionary(inlineDictionary.styleFormat.xmlAttribute);

        /// <summary>
        /// Gets or sets the classes.
        /// </summary>
        /// <value>
        /// The classes.
        /// </value>
        public List<String> classes { get; set; } = new List<string>();
    }
}