// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cursorZoneCollection.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.zone
{
    using imbSCI.Data.interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    //public enum cursorZoneSubdivisionPreset
    //{
    //    none,
    //    threeColumns,
    //    twoColumns,
    //    headerFooterContent,
    //    headerFooterTwoColumns,

    //}

    /// <summary>
    /// Collection of zones
    /// </summary>
    /// <seealso cref="System.Collections.IEnumerable" />
    public class cursorZoneCollection : IEnumerable, IGetCodeName
    {
        public String getCodeName()
        {
            String output = "";

            return output;
        }

        public cursorZoneCollection(cursorZone __master = null)
        {
            Add(cursorZoneRole.master, __master);
            if (__master != null)
            {
            }
        }

        /// <summary>
        /// Adds a zone into collection. Null <c>zone</c> will be ignored.
        /// </summary>
        /// <remarks>
        /// <para>If role is supports multiple items (column, section) on existing zone for the role it will create indexed key.</para>
        /// <para>If its a singular (footer, header, master) role it will replace any existing.</para>
        /// <para>If <c>master</c> zone is set, all zones will have the <c>master</c> zone set as parent.</para>
        /// <para>You may set the <c>master</c> zone with constructor or any time later with Add() call - it will automatically set parent for existing zones</para>
        /// </remarks>
        /// <param name="role">The role for the zone</param>
        /// <param name="zone">The zone to add into collection</param>
        /// <returns>Key that was finally used for the zone. Possibly important in case of roles supporting multiple entries</returns>
        public String Add(cursorZoneRole role, cursorZone zone)
        {
            if (zone == null) return "";

            String key = role.ToString();

            Int32 c = 0;
            if (role == cursorZoneRole.section || role == cursorZoneRole.column)
            {
                while (items.ContainsKey(key))
                {
                    c++;
                    key = role.ToString() + c.ToString();
                }
            }
            else if (role == cursorZoneRole.master)
            {
                foreach (KeyValuePair<String, cursorZone> pair in items)
                {
                    if (pair.Key != "master")
                    {
                        pair.Value.parent = zone;
                    }
                }
            }

            if (items.ContainsKey("master"))
            {
                var master = this[cursorZoneRole.master];
                if (master != zone)
                {
                    zone.parent = master;
                }
            }

            if (items.ContainsKey(key))
            {
                items[key] = zone;
            }
            else
            {
                items.Add(key, zone);
            }

            return key;
        }

        /// <summary>
        /// Gets the boundary rectangle to hold all subzones with the specified role.
        /// </summary>
        /// <param name="role">The role of subzone or subzones to select</param>
        /// <param name="scope">The scope to look into for each subzone</param>
        /// <returns></returns>
        public selectRangeArea getBoundaryOf(cursorZoneRole role, textCursorZone scope = textCursorZone.innerZone)
        {
            selectRangeArea range = null; //new selectRangeArea(0,0,0,0);
            String key = role.ToString();
            foreach (KeyValuePair<string, cursorZone> z in items)
            {
                if (z.Key.StartsWith(key, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (range == null)
                    {
                        range = z.Value.selectRangeArea(scope);
                    }
                    else
                    {
                        range.expandToWrap(z.Value.selectRangeArea(scope));
                    }
                }
            }
            return range;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }

        /// <summary>
        /// Gets the <see cref="cursorZone"/> with the specified role. If the role supports multiple entries then specify index to choose what zone to get.
        /// </summary>
        /// <value>
        /// The <see cref="cursorZone"/>.
        /// </value>
        /// <param name="role">The role.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public cursorZone this[cursorZoneRole role, Int32 index = 0]
        {
            get
            {
                String key = "";

                if (role == cursorZoneRole.section || role == cursorZoneRole.column)
                {
                    if (index > 0)
                    {
                        key = role.ToString() + index.ToString();
                    }
                    else
                    {
                        key = role.ToString();
                    }

                    return items[key];
                }
                else
                {
                    key = role.ToString();
                    if (items.ContainsKey(key))
                    {
                        return items[key];
                    }
                }

                return null;
            }
        }

        private Dictionary<String, cursorZone> items = new Dictionary<string, cursorZone>();
    }
}