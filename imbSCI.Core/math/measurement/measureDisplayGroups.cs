// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureDisplayGroups.cs" company="imbVeles" >
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
// Project: imbSCI.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Core.math.measurement
{
    using imbSCI.Core.interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class measureDisplayGroups
    {
        protected Dictionary<String, measureDisplayGroup> items = new Dictionary<String, measureDisplayGroup>();

        /// <summary>
        /// Exports all groups to list
        /// </summary>
        /// <returns></returns>
        public List<measureDisplayGroup> export()
        {
            List<measureDisplayGroup> output = new List<measureDisplayGroup>();

            foreach (measureDisplayGroup gr in items.Values)
            {
                output.Add(gr);
            }
            return output;
        }

        public IMeasure find(String key)
        {
            foreach (measureDisplayGroup gr in items.Values)
            {
                IMeasure ot = gr.Values.First(x => x.name == key) as IMeasure;
                if (ot != null) return ot;
            }
            return null;
        }

        public measureDisplayGroup this[String key]
        {
            get
            {
                if (!items.ContainsKey(key))
                {
                    items.Add(key, new measureDisplayGroup(key, ""));
                }
                return items[key];
            }
        }
    }
}