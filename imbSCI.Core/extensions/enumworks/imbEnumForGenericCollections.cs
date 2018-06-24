// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbEnumForGenericCollections.cs" company="imbVeles" >
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
namespace imbSCI.Core.extensions.enumworks
{
    using System;
    using System.Collections.Generic;

    public static class imbEnumForGenericCollections
    {
        /// <summary>
        /// Adds the specified item as key and its String form as Value
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="item">The item.</param>
        public static void Add(this IDictionary<Object, String> target, Enum item)
        {
            if ((item.ToInt32()) == 0)
            {
                target.Add(item, "");
            }
            else
            {
                target.Add(item, item.ToString());
            }
        }

        /// <summary>
        /// Add all enum values of the specified enum type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        public static void AddEnumAll<T>(this IDictionary<Object, String> target) where T : IConvertible
        {
            var enums = Enum.GetValues(typeof(T));
            foreach (Object en in enums)
            {
                target.Add(en as Enum);
            }
        }

        /// <summary>
        /// Adds all enum values from the type but as numeric string coresponding to the Enum member Int32 value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        public static void AddEnumAsNumericAll<T>(this IDictionary<Object, String> target) where T : IConvertible
        {
            var enums = Enum.GetValues(typeof(T));
            foreach (Enum en in enums)
            {
                Int32 n = en.ToInt32();

                target.Add(en, n.ToString());
            }
        }
    }
}