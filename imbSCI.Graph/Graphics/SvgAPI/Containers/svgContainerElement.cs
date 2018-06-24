using imbSCI.Graph.Graphics.SvgAPI.Core;
using imbSCI.Graph.Graphics.SvgAPI.Meta;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace imbSCI.Graph.Graphics.SvgAPI.Containers
{
    /// <summary>
    /// SVG element that can contain sub elements
    /// </summary>
    /// <seealso cref="imbSCI.Graph.Graphics.SvgAPI.svgGraphicElementBase" />
    public abstract class svgContainerElement : svgGraphicElementBase
    {
        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public override XmlNode ToXml()
        {
            var output = ToXmlBase();

            foreach (svgGraphicElementBase c in children)
            {
                XmlNode node = output.OwnerDocument.ImportNode(c.ToXml(), true);
                output.AppendChild(node);
            }

            return output;
        }

        ///// <summary>
        ///// To the XML string.
        ///// </summary>
        ///// <returns></returns>
        //public override string ToXmlString()
        //{
        //    return ToXml().OuterXml;
        //}

        /// <summary>
        /// Adds the specified child element, and places it at designated coordinates
        /// </summary>
        /// <param name="child">The child.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public svgGraphicElementBase Add(svgGraphicElementBase child, Int32 x = Int32.MinValue, Int32 y = Int32.MinValue)
        {
            if (!children.Contains(child)) children.Add(child);

            if (x != Int32.MinValue) child.x = x;
            if (y != Int32.MinValue) child.y = y;
            return child;
        }

        /// <summary>
        /// Adds use of <see cref="svgSymbol" />
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="useID">The use identifier.</param>
        /// <returns></returns>
        public svgUse AddUse(svgSymbol symbol, Int32 x, Int32 y, String useID = "")
        {
            svgUse use = new svgUse();
            use.id = useID;
            use.x = x;
            use.y = y;
            use.href = "#" + symbol.id;
            children.Add(use);
            return use;
        }

        /// <summary>
        /// Contained elements
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        [XmlIgnore]
        public List<svgGraphicElementBase> children { get; set; } = new List<svgGraphicElementBase>();
    }
}