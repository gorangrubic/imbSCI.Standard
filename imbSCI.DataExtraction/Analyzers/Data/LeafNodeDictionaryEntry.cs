using HtmlAgilityPack;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Data;
using imbSCI.Data.extensions;
using imbSCI.Data.extensions.data;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.Extractors;
using imbSCI.DataExtraction.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Analyzers.Data
{
    [Flags]
    public enum NodeInTemplateRole
    {
        none =0,
        Static=1,
        Dynamic=2,
        Permanent=4,
        Optional=8,
        Structural=16,
        Functional=32,
        StaticPermanent = Static | Permanent,
        DynamicPermanent = Dynamic | Permanent,
        StaticOptional = Static | Optional,
        DynamicOptional = Dynamic | Optional,
        Undefined = 64

    }

    [Serializable]
    public class LeafNodeDictionaryEntry : IObjectWithName, IEquatable<LeafNodeDictionaryEntry>
    {
        public String name { get; set; }

        public LeafNodeDictionaryEntry() { }

        public LeafNodeDictionaryEntry(HtmlNode _node)
        {

            node = _node;
            ContentHash = md5.GetMd5Hash(node.InnerText);
            XPath = node.XPath;
            Content = node.GetInnerText();
            name = XPath.getPathVersion(-1, "/");
        }

        [XmlIgnore]
        [NonSerialized]
        public HtmlNode node  = null;

        [XmlIgnore]
        [NonSerialized]
        public Object MetaData = null;

        [XmlIgnore]
        public String Content { get; set; } = "";

        public String XPath { get; set; } = "";

        public String ContentHash { get; set; }

        /// <summary>
        /// Rate at which the node is <see cref="NodeInTemplateRole.Permanent"/>. Value 1 means that the node was found on each document in the training set - value leaning to 0 means that node is optional, found on few pages
        /// </summary>
        /// <value>
        /// The presence.
        /// </value>
        public Double Presence { get; set; } = 1;

        public Double Staticness { get; set; } = 1;


        public NodeInTemplateRole Category
        {
            get; set;
        } = NodeInTemplateRole.Undefined;

        /// <summary>
        /// Indicates whether the current object is equal to another object by comparing <see cref="ContentHash"/>
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public Boolean Equals(LeafNodeDictionaryEntry other)
        {
            return ContentHash.Equals(other.ContentHash);
        }


    }
}