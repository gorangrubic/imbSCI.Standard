// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphElementCollection.cs" company="imbVeles" >
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
using imbSCI.Graph.DGML.core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Graph.DGML.collections
{
    public abstract class GraphElementCollection<T> : List<T> where T : IGraphElement, new()
    {
        protected GraphElementCollection(DirectedGraph graph)
        {
            Graph = graph;
        }

        protected DirectedGraph Graph { get; set; }

        /// <summary>
        /// Gets the <see cref="T"/> with the specified identifier. Returns null if not found
        /// </summary>
        /// <value>
        /// The <see cref="T"/>.
        /// </value>
        /// <param name="id">The identifier.</param>
        /// <returns>Null if not found</returns>
        public T this[String id]
        {
            get
            {
                return this.FirstOrDefault(x => x.Id == id);
            }
        }

         public T AddOrGet(String i_id, String Label="", Boolean returnOnlyNewNodes=true)
        {

            T n = this.FirstOrDefault(x => x.Id == i_id);
            if (n== null)
            {
                n = new T()
                {
                    Id = i_id,
                    Label = Label
                };
                Add(n);

                if (returnOnlyNewNodes) return n;
            } else
            {
                if (!returnOnlyNewNodes) return n;
            }
            return n;

        }

        /// <summary>
        /// Adds or gets elements from the list
        /// </summary>
        /// <param name="id_list">The identifier list.</param>
        /// <param name="returnOnlyNewNodes">if set to <c>true</c> [return only new nodes].</param>
        /// <returns></returns>
        public List<T> Get(IEnumerable<String> id_list)
        {
            List<T> output = new List<T>();
            foreach (String i in id_list)
            {
                T n = this.FirstOrDefault(x => x.Id == i);
                if (n == null)
                {

                }
                else
                {
                     output.Add(n);
                }
            }
            return output;
        }


        /// <summary>
        /// Adds or gets elements from the list
        /// </summary>
        /// <param name="id_list">The identifier list.</param>
        /// <param name="returnOnlyNewNodes">if set to <c>true</c> [return only new nodes].</param>
        /// <returns></returns>
        public List<T> AddOrGet(IEnumerable<String> id_list, Boolean returnOnlyNewNodes=true)
        {
            List<T> output = new List<T>();
            foreach (String i in id_list)
            {
                T n = this.FirstOrDefault(x => x.Id == i);
                if (n== null)
                {
                    n = new T()
                    {
                        Id = i
                    };
                    Add(n);

                    if (returnOnlyNewNodes) output.Add(n);
                } else
                {
                    if (!returnOnlyNewNodes) output.Add(n);
                }
            }
            return output;
        }

        /// <summary>
        /// Adds the node.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public T AddNode(String id, String name)
        {
            var output = this.FirstOrDefault(x => x.Id == id);
            //if (EqualityComparer<T>.Default.Equals(output, default(T)))
            //{
            if (EqualityComparer<T>.Default.Equals(output, default(T)))
            {
                output = new T();
                output.Label = name;
                //output.name = name;
                output.Id = id;
                Add(output);
            }
            //}
            return output;
        }

        /// <summary>
        /// Adds the node.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public T AddNode(String name)
        {
            var output = this.FirstOrDefault(x => x.Id == name);
            if (output == null)
            {
                output = new T();
                output.Label = name;
                output.Id = name.GetIDFromLabel();
                Add(output);
            }
            return output;
        }
    }
}