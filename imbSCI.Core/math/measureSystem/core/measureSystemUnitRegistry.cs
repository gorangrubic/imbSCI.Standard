// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureSystemUnitRegistry.cs" company="imbVeles" >
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
namespace imbSCI.Core.math.measureSystem.core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class measureSystemUnitRegistry
    {
        public Int32 Count
        {
            get
            {
                return items.Count;
            }
        }

        public Int32 IndexOf(measureSystemUnitEntry item)
        {
            return sortedByFactor.IndexOf(item);
        }

        private List<measureSystemUnitEntry> _items = new List<measureSystemUnitEntry>();

        /// <summary>
        ///
        /// </summary>
        protected List<measureSystemUnitEntry> items
        {
            get { return _items; }
            set { _items = value; }
        }

        private Dictionary<measureSystemUnitEntry, Double> _byFactor = new Dictionary<measureSystemUnitEntry, double>();

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<measureSystemUnitEntry, Double> byFactor
        {
            get { return _byFactor; }
            set { _byFactor = value; }
        }

        public void Add(measureSystemUnitEntry unit)
        {
            items.Add(unit);
            byFactor.Add(unit, unit.factor);
        }

        public measureSystemUnitEntry getRoot()
        {
            if (items.Any())
            {
                return items[0];
            }
            else
            {
                return null;
            }
        }

        private List<measureSystemUnitEntry> _sortedByFactor = new List<measureSystemUnitEntry>();

        /// <summary>
        ///
        /// </summary>
        protected List<measureSystemUnitEntry> sortedByFactor
        {
            get { return _sortedByFactor; }
            set { _sortedByFactor = value; }
        }

        public void prepare()
        {
            sortedByFactor = byFactor.Keys.ToList();

            // byFactor. .toSortedList<measureSystemUnitEntry>();
        }

        public Boolean ContainsKey(String key)
        {
            var tmp = this[key];
            return (tmp != null);
        }

        public measureSystemUnitEntry this[Int32 key]
        {
            get
            {
                if (key < 0) key = 0;
                if (key >= sortedByFactor.Count) key = sortedByFactor.Count - 1;

                if (key == -1) return null;

                return sortedByFactor[key];
            }
        }

        public measureSystemUnitEntry this[String key]
        {
            get
            {
                foreach (measureSystemUnitEntry u in items)
                {
                    if (u.nameSingular == key)
                    {
                        return u;
                    }
                    if (u.namePlural == key)
                    {
                        return u;
                    }
                    if (u.unit == key)
                    {
                        return u;
                    }
                }
                return null;
            }
        }
    }
}