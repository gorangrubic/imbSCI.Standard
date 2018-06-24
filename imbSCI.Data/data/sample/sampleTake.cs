// --------------------------------------------------------------------------------------------------------------------
// <copyright file="sampleTake.cs" company="imbVeles" >
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
// Project: imbSCI.Data
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

namespace imbSCI.Data.data.sample
{
    /// <summary>
    /// Portion of the population
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.List{T}" />
    public class sampleTake<T> : List<T>
    {
        /// <summary>
        /// Populates sample take according to settings
        /// </summary>
        /// <param name="_source">The source.</param>
        /// <param name="_settings">The settings.</param>
        public sampleTake(List<T> _source, samplingSettings _settings)
        {
            sampleSource = _source;

            if (sampleSource.Count == 0) return;

            settings = _settings;

            if (settings == null) return;

            Int32 limit = -1;

            Int32 skip = -1;

            Int32 step = 1;

            if (limit < 1) limit = sampleSource.Count;
            if (skip < 1) skip = 0;

            if (settings.parts > 1)
            {
                limit = Math.Min(sampleSource.Count / settings.parts, limit);

                switch (settings.takeOrder)
                {
                    case samplingOrderEnum.ordinal:
                        if (settings.skip > 0)
                        {
                            skip = settings.skip * limit;
                        }
                        break;

                    case samplingOrderEnum.randomSuffle:
                        skip = 0;
                        break;

                    case samplingOrderEnum.everyNth:
                        skip = settings.skip;
                        step = settings.parts;
                        break;
                }
            }
            else
            {
                skip = settings.skip;
            }

            Boolean go = true;
            Random rnd = new Random();
            Int32 index = skip;

            List<Int32> taken = new List<int>();

            Int32 i = 0;

            Int32 iLimit = sampleSource.Count * 5;

            while (go)
            {
                i++;
                switch (settings.takeOrder)
                {
                    case samplingOrderEnum.ordinal:
                    case samplingOrderEnum.everyNth:
                        index += step;
                        if (taken.Contains(index))
                        {
                            index++;
                        }
                        break;

                    case samplingOrderEnum.randomSuffle:
                        index = rnd.Next(0, sampleSource.Count);
                        break;
                }

                if (index >= sampleSource.Count)
                {
                    index = index - sampleSource.Count;
                }

                if (settings.onlyUnique)
                {
                    taken.Add(index);

                    if (!Contains(sampleSource[index])) Add(sampleSource[index]);
                }
                else
                {
                    Add(sampleSource[index]);
                }

                if (Count >= limit) { go = false; }

                if (i > iLimit) { go = false; }
            }
        }

        /// <summary>
        /// Settings used for this take
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public samplingSettings settings { get; protected set; }

        public List<T> sampleSource { get; protected set; }

        public List<T> GetRestOfSource()
        {
            List<T> output = sampleSource.ToList();

            foreach (T ev in this)
            {
                output.Remove(ev);
            }

            return output;
        }

        public Double takeToSourceRatio
        {
            get
            {
                return ((Double)Count) / ((Double)sampleSource.Count);
            }
        }
    }
}