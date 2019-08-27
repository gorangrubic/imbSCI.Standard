// --------------------------------------------------------------------------------------------------------------------
// <copyright file="freeGraphQueryResult.cs" company="imbVeles" >
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Graph.FreeGraph
{
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.List{imbNLP.PartOfSpeech.TFModels.semanticCloud.core.freeGraphNodeBase}'
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    /// <summary>
    /// Result of a query over <see cref="freeGraph"/> collection, contains clones of  matched graphs
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{imbNLP.PartOfSpeech.TFModels.semanticCloud.core.freeGraphNodeBase}" />
    public class freeGraphQueryResult : List<freeGraphNodeBase>
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.List{imbNLP.PartOfSpeech.TFModels.semanticCloud.core.freeGraphNodeBase}'
    {
        public freeGraphQueryResult()
        {
        }

        public freeGraphQueryResult(freeGraphNodeBase centralNode)
        {
            queryNodes.Add(centralNode);
        }

        public freeGraphQueryResult(IEnumerable<freeGraphNodeBase> centralNodes)
        {
            queryNodes.AddRange(centralNodes);
        }

        public Boolean graphNotReady { get; set; } = false;

        public freeGraphNodeBase queryNode
        {
            get
            {
                return queryNodes.FirstOrDefault();
            }
        }

        public List<freeGraphNodeBase> queryNodes { get; protected set; } = new List<freeGraphNodeBase>();

        public Boolean graphNodeNotFound
        {
            get
            {
                return queryNode == null;
            }
        }

        public Boolean includeBtoALinks { get; set; }

        /// <summary>
        /// Adds nodes if they are not already inside, and returns the ones that were new fot this result
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <returns></returns>
        public List<freeGraphNodeBase> AddNewNodes(IEnumerable<freeGraphNodeBase> nodes)
        {
            List<freeGraphNodeBase> newNodes = new List<freeGraphNodeBase>();
            foreach (freeGraphNodeBase node in nodes)
            {
                if (!queryNodes.Any(x => x.name == node.name))
                {
                    if (!this.Any(x => x.name == node.name))
                    {
                        Add(node);
                        newNodes.Add(node);
                    }
                }
            }
            return newNodes;
        }
    }
}