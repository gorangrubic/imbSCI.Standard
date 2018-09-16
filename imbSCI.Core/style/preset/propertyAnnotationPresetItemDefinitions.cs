// --------------------------------------------------------------------------------------------------------------------
// <copyright file="propertyAnnotationPresetItemDefinitions.cs" company="imbVeles" >
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
using System;
using System.Collections.Generic;

namespace imbSCI.Core.style.preset
{
    using imbSCI.Core.extensions.enumworks;
    using imbSCI.Core.extensions.text;
    using System.Data;

    /// <summary>
    /// Collection with preset item definitions
    /// </summary>
    public class propertyAnnotationPresetItemDefinitions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="propertyAnnotationPresetItemDefinitions"/> class.
        /// </summary>
        public propertyAnnotationPresetItemDefinitions()
        {
        }

        /// <summary>
        /// Gets or sets the key value pairs.
        /// </summary>
        /// <value>
        /// The key value pairs.
        /// </value>
        public List<propertyAnnotationPresetItemTuple> keyValuePairs { get; set; } = new List<propertyAnnotationPresetItemTuple>();

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(Object key, Object value)
        {
            Type kt = key.GetType();
            Boolean ok = false;

            String strKey = "";
            if (kt.IsEnum)
            {
                strKey = imbEnumExtendBase.getEnumMemberPath(key, true);
                ok = true;
            }
            else
            {
                if (key is String)
                {
                    strKey = (String)key;
                    ok = true;
                }
            }

            if (!ok)
            {
                return;
            }

            ok = false;
            if (value is String) ok = true;
            if (value is Int32) ok = true;
            if (value is Double) ok = true;

            if (ok) keyValuePairs.Add(new propertyAnnotationPresetItemTuple(strKey, value.toStringSafe()));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="propertyAnnotationPresetItemDefinitions"/> to <see cref="PropertyCollection"/>.
        /// </summary>
        /// <param name="definitions">The definitions.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator PropertyCollection(propertyAnnotationPresetItemDefinitions definitions)
        {
            PropertyCollection output = new PropertyCollection();
            foreach (propertyAnnotationPresetItemTuple pair in definitions.keyValuePairs)
            {
                output.Add(pair.key, pair.value);
            }
            return output;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="PropertyCollection"/> to <see cref="propertyAnnotationPresetItemDefinitions"/>.
        /// </summary>
        /// <param name="definitions">The definitions.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator propertyAnnotationPresetItemDefinitions(PropertyCollection definitions)
        {
            propertyAnnotationPresetItemDefinitions output = new propertyAnnotationPresetItemDefinitions();
            foreach (propertyAnnotationPresetItemTuple pair in definitions)
            {
                output.Add(pair.key, pair.value);
            }
            return output;
        }
    }
}