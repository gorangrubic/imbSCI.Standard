// --------------------------------------------------------------------------------------------------------------------
// <copyright file="diagramMermaidOutput.cs" company="imbVeles" >
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
using imbSCI.Core.extensions.data;
using imbSCI.Core.style.color;
using imbSCI.Data;
using System;

namespace imbSCI.DataComplex.diagram.output
{
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.render;
    using imbSCI.DataComplex.diagram.core;
    using imbSCI.DataComplex.diagram.enums;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="diagramOutputBase" />
    public class diagramMermaidOutput : diagramOutputBase
    {
        /// <summary>
        /// Gets the output.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="colorProvider">The color provider.</param>
        /// <returns></returns>
        public override string getOutput(diagramModel model, acePaletteProvider colorProvider)
        {
            string output = "";
            string newLine = "";

            output += "<div class=\"mermaid\" id=\"monotone\">" + Environment.NewLine + Environment.NewLine;

            output += model.diagramClassName + " " + model.direction.ToString() + ";" + Environment.NewLine;

            output += getOutputNodesDeclaration(model) + Environment.NewLine;

            output += getOutputLinksDeclaration(model) + Environment.NewLine;

            if (colorProvider != null)
            {
                output += getOutputStyleDeclaration(model, colorProvider) + Environment.NewLine + Environment.NewLine;
            }

            output += "</div>";

            return output;
        }

        /// <summary>
        /// Gets the link declaration.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <returns></returns>
        public override string getLinkDeclaration(diagramLink link)
        {
            string fromPart = "N";
            string toPart = "N";
            string output = "";
            string middle = "";

            fromPart = fromPart + link.from.name;

            if (link.isFromDirected) middle = middle + "<";

            if (link.description.isNullOrEmpty())
            {
                switch (link.type)
                {
                    case diagramLinkTypeEnum.dotted:
                        middle += "-.-";
                        break;

                    default:
                    case diagramLinkTypeEnum.normal:
                        middle += "--";
                        break;

                    case diagramLinkTypeEnum.thick:
                        middle += "==";
                        break;
                }
            }
            else
            {
                switch (link.type)
                {
                    case diagramLinkTypeEnum.dotted:
                        middle += "-." + link.description + ".-";
                        break;

                    default:
                    case diagramLinkTypeEnum.normal:
                        middle += "--" + link.description + "--";
                        break;

                    case diagramLinkTypeEnum.thick:
                        middle += "==" + link.description + "==";
                        break;
                }
            }

            if (link.isToDirected) middle = middle + ">";
            toPart = toPart + link.to.name;

            output = fromPart.add(middle).add(toPart);
            output = output + ";";
            return "    " + output;
        }

        /// <summary>
        /// Gets the node declaration.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public override string getNodeDeclaration(diagramNode node)
        {
            string output = "";
            string name = "N" + node.name;
            string insert = "";

            if (!node.description.isNullOrEmpty())
            {
                insert = "\"" + node.description + "\"";
            }
            else
            {
                insert = "\"" + node.name + "\"";
            }

            switch (node.shapeType)
            {
                case diagramNodeShapeEnum.circle:
                    insert = "((" + insert + "))";
                    break;

                case diagramNodeShapeEnum.flagToLeft:
                    insert = "[" + insert + "<";
                    break;

                case diagramNodeShapeEnum.flagToRight:
                    insert = ">" + insert + "]";
                    break;

                default:
                case diagramNodeShapeEnum.normal:
                    insert = "[" + insert + "]";
                    break;

                case diagramNodeShapeEnum.rhombus:
                    insert = "{" + insert + "}";
                    break;

                case diagramNodeShapeEnum.rounded:
                    insert = "(" + insert + ")";
                    break;
            }

            output = name + insert;
            return "    " + output + ";";
        }

        /// <summary>
        /// Gets the output links declaration.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public override string getOutputLinksDeclaration(diagramModel model)
        {
            string output = "";
            string newLine = "";

            // link declaration
            foreach (diagramLink link in model.links)
            {
                newLine = getLinkDeclaration(link);
                output = output.add(newLine, Environment.NewLine);
            }

            return output;
        }

        /// <summary>
        /// Gets the output nodes declaration.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public override string getOutputNodesDeclaration(diagramModel model)
        {
            string output = "";
            string newLine = "";

            // node declaration
            foreach (diagramNode node in model.nodes.Values)
            {
                newLine = getNodeDeclaration(node);
                output = output.add(newLine, Environment.NewLine);
            }

            return output;
        }

        /// <summary>
        /// Gets the output style declaration.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="colorProvider">The color provider.</param>
        /// <returns></returns>
        public override string getOutputStyleDeclaration(diagramModel model, acePaletteProvider colorProvider)
        {
            string output = "";
            string newLine = "";

            // node declaration
            foreach (diagramNode node in model.nodes.Values)
            {
                newLine = "style N" + node.name + " ";

                colorProvider.setVariation(node.importance, false, -1, false, (acePaletteRole)node.color);

                newLine += " fill:" + colorProvider.bgColor.ColorToHex(); //.toHexColor(true);

                //   newLine += ";";
                //acePaletteVariationRole varRole = node.color;

                output = output.add(newLine, Environment.NewLine);
            }

            return output;
        }

        public override void render(diagramModel model, ITextRender render)
        {
            throw new NotImplementedException();
        }
    }
}