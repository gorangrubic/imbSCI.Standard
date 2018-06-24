// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbTreeNodeBlock.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------

namespace imbSCI.DataComplex.tree
{
    #region imbVeles using

    using System;
    using System.Text;

    #endregion imbVeles using

    public class imbTreeNodeBlock : imbTreeNodeCollection
    {
        public imbTreeNodeBlock(String __name)
        {
            name = __name;
        }

        #region --- name ------- ime bloka

        private String _name = "";

        /// <summary>
        /// ime bloka
        /// </summary>
        public String name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        public String getContent()
        {
            StringBuilder sb = new StringBuilder();
            Int32 i = 0;
            foreach (var pair in this)
            {
                // XmlNode node = pair.Value as XmlNode;
                // sb.AppendLine("--- Paragraph [" + i + "] ----");
                //sb.AppendLine(node.InnerText);
                // sb.AppendLine("--------------------------");
                i++;
            }
            return sb.ToString();
        }

        //public String getXPaths(Boolean ofHtmlNodes)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    Int32 i = 0;
        //    foreach (var pair in this)
        //    {
        //       XmlNode node = pair.Value as XmlNode;
        //       // sb.AppendLine("--- BLOCK [" + i + "] ----");
        //        if (ofHtmlNodes)
        //        {
        //            sb.AppendLine(pair.sourcePath);

        //        } else
        //        {
        //            sb.AppendLine(pair.path);
        //        }
        //        sb.AppendLine("--------------------------");
        //        i++;
        //    }
        //    return sb.ToString();
        //}

        //public String getHtmlContent(Boolean innerHtml=false)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    Int32 i = 0;
        //    sb.AppendLine("<block>");

        //    foreach (var pair in this)
        //    {
        //        sb.AppendLine("<paragraph id=\"" + i + "\">");
        //        sb.AppendLine("<data id=\"" + i + "\">");
        //       XmlNode node = pair.value asXmlNode;

        //        sb.AppendLine("<xpath>");

        //        if (node != null)
        //        {
        //            sb.AppendLine(node.getFullXPath());
        //        }
        //            sb.AppendLine("</xpath>");

        //        sb.AppendLine("<spath>");

        //        if (node != null)
        //        {
        //            sb.AppendLine(pair.sourcePath);
        //        }
        //        sb.AppendLine("</spath>");

        //        sb.AppendLine("<nodeType>");

        //        if (node != null)
        //        {
        //            sb.AppendLine(node.NodeType.ToString());
        //        }
        //        sb.AppendLine("</nodeType>");

        //        sb.AppendLine("</data>");

        //     //  XmlNode node = pair.value asXmlNode;
        //        sb.AppendLine();
        //        if (innerHtml)
        //        {
        //            sb.AppendLine(node.InnerHtml);
        //        }
        //        else
        //        {
        //            sb.AppendLine(node.OuterHtml);
        //        }
        //        sb.AppendLine("</paragraph>");
        //        i++;
        //    }
        //    sb.AppendLine("</block>");
        //    return sb.ToString();
        //}

        //public List<HtmlNode> getHtmlNodes()
        //{
        //    List<HtmlNode> nodes = new List<HtmlNode>();
        //    foreach (var pair in this)
        //    {
        //       XmlNode node = pair.value asXmlNode;

        //        nodes.Add(node);
        //    }
        //    return nodes;
        //}

        #endregion --- name ------- ime bloka

        //#region --- parentNode ------- Node koji je parent ovom bloku
        //private String _parentNode;
        ///// <summary>
        ///// Node koji je parent ovom bloku
        ///// </summary>
        //public String parentNode
        //{
        //    get
        //    {
        //        return _parentNode;
        //    }
        //    set
        //    {
        //        _parentNode = value;
        //        OnPropertyChanged("parentNode");
        //    }
        //}
        //#endregion
    }
}