// --------------------------------------------------------------------------------------------------------------------
// <copyright file="folderNodeToDirectedGraphConverter.cs" company="imbVeles" >
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
//using System.Web.UI.WebControls;
//using Accord;
using imbSCI.Core.extensions.data;
using imbSCI.Core.files.folders;
using imbSCI.Core.style.color;
using imbSCI.Data;
using imbSCI.Graph.DGML;
using imbSCI.Graph.DGML.core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace imbSCI.Graph.Converters
{
    /// <summary>
    ///
    /// </summary>
    public static class StaticConverters
    {
        public const String CAT_FOLDERNODE = "folderNode";
        public const String CAT_FOLDERNODEDESC = "folderNodeDesc";
        public const String CAT_FILE = "folderNodeFile";
        public const String CAT_FILEDESC = "folderNodeFileDesc";

        /// <summary>
        /// Gets the directed graph from <see cref="folderNode"/>
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="doFolderDescription">if set to <c>true</c> [do folder description].</param>
        /// <param name="doFileEntries">if set to <c>true</c> [do file entries].</param>
        /// <param name="doFileDescriptions">if set to <c>true</c> [do file descriptions].</param>
        /// <param name="limit">The limit.</param>
        /// <returns></returns>
        public static DirectedGraph GetDirectedGraph(this folderNode folder, Boolean doFolderDescription, Boolean doFileEntries, Boolean doFileDescriptions, Int32 limit = 100)
        {
            DirectedGraph output = new DirectedGraph();
            output.Title = folder.name;

            output.Categories.AddOrGetCategory(CAT_FOLDERNODE, "Folder", "").Background = Color.Orange.ColorToHex(); //.toHexColor();
            output.Categories.AddOrGetCategory(CAT_FOLDERNODEDESC, "Folder Description", "").Background = Color.LightGray.ColorToHex(); //.toHexColor();
            output.Categories.AddOrGetCategory(CAT_FILE, "File", "").Background = Color.LightSteelBlue.ColorToHex(); //.toHexColor();
            output.Categories.AddOrGetCategory(CAT_FILEDESC, "File Description", "").Background = Color.LightGray.ColorToHex(); //.toHexColor();

            List<folderNode> nextSet = new List<folderNode>();
            nextSet.Add(folder);
            Int32 i = 0;

            while (nextSet.Any())
            {
                i++;
                List<folderNode> newNextSet = new List<folderNode>();

                foreach (folderNode parent in nextSet)
                {
                    var parentNode = output.Nodes.AddNode(parent.path, parent.name);
                    parentNode.Category = CAT_FOLDERNODE;

                    if (parent.parent != null)
                    {
                        folderNode parentFolder = parent.parent as folderNode;
                        Link l = new Link(parentFolder.path, parentNode.Id, true);
                        l.Stroke = Color.OrangeRed.ColorToHex(); //.toHexColor();
                        output.Links.Add(l);
                    }

                    foreach (var pair in parent)
                    {
                        newNextSet.AddUnique(pair.Value);
                    }

                    if (doFolderDescription)
                    {
                        if (!parent.description.isNullOrEmpty())
                        {
                            Node descNode = output.Nodes.AddNode(parent.path + "_DESC", parent.description);
                            descNode.Category = CAT_FOLDERNODEDESC;
                            output.Links.AddLink(parentNode, descNode, "About").StrokeDashArray = "2,5,2,5";
                        }
                    }

                    if (doFileEntries)
                    {
                        foreach (var f in parent.AdditionalFileEntries)
                        {
                            Node fileNode = output.Nodes.AddNode(parent.path + f.Key, f.Value.description);
                            fileNode.Category = CAT_FILE;
                            var fileLink = output.Links.AddLink(parentNode, fileNode, f.Value.filename);
                            fileLink.StrokeDashArray = "2,2,5,2";
                            fileLink.Stroke = Color.LightGray.ColorToHex(); //.toHexColor();

                            //if (doFileDescriptions)
                            //{
                            //    Node fileInfoNode = output.Nodes.AddNode(parent.path + f.Key + "_DESC", );
                            //    fileInfoNode.Category = CAT_FILEDESC;
                            //    var fileInfoLink = output.Links.AddLink(fileNode, fileInfoNode, "");
                            //    fileLink.StrokeDashArray = "2,2,5,2";
                            //    fileLink.Stroke = Color.LightGray.toHexColor();
                            //}
                        }
                    }
                }

                if (i > limit) break;
                nextSet = newNextSet;
            }

            return output;
        }
    }
}