// --------------------------------------------------------------------------------------------------------------------
// <copyright file="diagramOutputBase.cs" company="imbVeles" >
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

namespace imbSCI.Graph.Diagram.output
{
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.render;
    using imbSCI.Graph.Diagram.core;

    public abstract class diagramOutputBase
    {
        public abstract string getOutput(diagramModel model, acePaletteProvider colorProvider);

        public abstract string getOutputNodesDeclaration(diagramModel model);

        public abstract string getOutputLinksDeclaration(diagramModel model);

        public abstract string getOutputStyleDeclaration(diagramModel model, acePaletteProvider colorProvider);

        public abstract void render(diagramModel model, ITextRender render);

        //public abstract String getLinkType(diagramLinkTypeEnum type);

        public abstract string getNodeDeclaration(diagramNode node);

        public abstract string getLinkDeclaration(diagramLink link);

        //public abstract String get
    }
}