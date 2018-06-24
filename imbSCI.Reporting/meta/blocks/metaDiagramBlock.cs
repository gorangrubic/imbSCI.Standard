// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaDiagramBlock.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.meta.blocks
{
    using imbSCI.Graph.Diagram;
    using imbSCI.Graph.Diagram.builders;
    using imbSCI.Graph.Diagram.core;
    using imbSCI.Graph.Diagram.enums;
    using imbSCI.Graph.Diagram.output;
    using imbSCI.Reporting.script;

    public class metaDiagramBlock : MetaContainerNestedBase
    {
        public diagramModel diagram { get; set; }

        public diagramOutputEngineEnum engine { get; set; }

        public metaDiagramBlock(diagramModel __diagram, diagramOutputEngineEnum __engine)
        {
            diagram = __diagram;
        }

        public override docScript compose(docScript script = null)
        {
            script.open("diagram", diagram.name, diagram.description);

            if (diagram != null)
            {
                script.AppendDiagram(diagram, engine);
            }

            script.close();
            return script;
        }

        public override void construct(object[] resources)
        {
        }
    }
}