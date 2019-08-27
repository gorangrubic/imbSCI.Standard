// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyCollectionExtendedList.cs" company="imbVeles" >
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
namespace imbSCI.Core.collection
{
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Data;
    using imbSCI.Data.interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;

#pragma warning disable CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    /// <summary>
    /// Collection of <see cref="imbSCI.Core.collection.PropertyCollectionExtended"/> instances, with its own name and description
    /// </summary>
    /// <seealso cref="aceCommonTypes.primitives.imbBindable" />
    public class PropertyCollectionExtendedList : IObjectWithNameAndDescription, IEnumerable, IEnumerable<KeyValuePair<string, PropertyCollectionExtended>>
#pragma warning restore CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    {
        /// <summary>
        /// Name for this instance
        /// </summary>
        public String name { get; set; } = "";

        /// <summary>
        /// Human-readable description of object instance
        /// </summary>
        public String description { get; set; } = "";

        /// <summary>
        /// Gets the <see cref="PropertyCollectionExtended"/> with the specified instance identifier.
        /// </summary>
        /// <value>
        /// The <see cref="PropertyCollectionExtended"/>.
        /// </value>
        /// <param name="instanceID">The instance identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">instanceID - No entry found under this name</exception>
        public PropertyCollectionExtended this[String instanceID]
        {
            get
            {
                if (items.ContainsKey(instanceID))
                {
                    return items[instanceID];
                }
                throw new ArgumentOutOfRangeException(nameof(instanceID), "No entry found under this name");
            }
        }

        /// <summary>
        /// Counts this instance.
        /// </summary>
        /// <returns></returns>
        public Int32 Count() => items.Count; //.Values.ToList().Count();

        /// <summary>
        /// The get column value default
        /// </summary>
        public const String getColumnValue_Default = "--";

        /// <summary>
        /// Takes Attribute values to set name and description for the collection
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="instanceID">The instance identifier.</param>
        /// <returns></returns>
        public String Add(PropertyCollectionExtended item, Enum instanceID)
        {
            settingsMemberInfoEntry mie = new settingsMemberInfoEntry(instanceID);
            if (!imbSciStringExtensions.isNullOrEmptyString(mie.displayName)) item.name = mie.displayName;
            if (!imbSciStringExtensions.isNullOrEmptyString(mie.description)) item.description = mie.displayName;
            return Add(item, item.name, true);
        }

        /// <summary>
        /// Adds the <see cref="imbSCI.Core.collection.PropertyCollectionExtended" /> under Key supplied with <c>instanceID</c>.
        /// </summary>
        /// <param name="item">The collection to be included</param>
        /// <param name="instanceID">The instance identifier. If not supplied it will take <see cref="imbSCI.Core.collection.PropertyCollectionExtended.name" /></param>
        /// <param name="synchronizeID">if set to <c>true</c> it will make sure that <c>item</c> <see cref="imbSCI.Core.collection.PropertyCollectionExtended.name" /> has the final <c>instanceID</c> value.</param>
        /// <returns>
        /// Used version of <c>instanceID</c>. It will differ from the specified if the collection already contained item with such name
        /// </returns>
        /// <remarks>
        /// <para>If <c>instanceID</c> is already used it will call <see cref="imbSCI.Core.extensions.text.imbStringGenerators.makeUniqueName(string,System.Collections.Generic.IEnumerable{string},string,int,bool)" /> to get unique version.</para>
        /// <para>If the <c>item</c> has no name set and <c>instanceID</c> specified - it is automatically set</para>
        /// </remarks>
        public String Add(PropertyCollectionExtended item, String instanceID = "", Boolean synchronizeID = false)
        {
            Boolean assignName = false;
            if (instanceID.isNullOrEmpty())
            {
                instanceID = item.name;
            }
            else
            {
                if (item.name.isNullOrEmpty())
                {
                    assignName = true;
                }
            }

            if (items.ContainsKey(instanceID))
            {
                instanceID = instanceID.makeUniqueName(items.Keys);
            }

            items.Add(instanceID, item);

            if (synchronizeID)
            {
                if (item.name != instanceID) assignName = true;
            }

            if (assignName)
            {
                item.name = instanceID;
            }
            return instanceID;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<string, PropertyCollectionExtended>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, PropertyCollectionExtended>>)items).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, PropertyCollectionExtended>>)items).GetEnumerator();
        }

        private Dictionary<String, PropertyCollectionExtended> _items = new Dictionary<string, PropertyCollectionExtended>();

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        protected Dictionary<string, PropertyCollectionExtended> items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
            }
        }
    }
}