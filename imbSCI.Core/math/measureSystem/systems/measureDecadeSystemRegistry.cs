// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureDecadeSystemRegistry.cs" company="imbVeles" >
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
namespace imbSCI.Core.math.measureSystem.systems
{
    using imbSCI.Core.math.measureSystem.enums;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// measure system registry
    /// </summary>
    /// <seealso cref="System.Collections.Generic.Dictionary{System.String, aceCommonTypes.math.measureSystem.measureDecadeSystem}" />
    public class measureDecadeSystemRegistry : Dictionary<string, measureDecadeSystem>
    {
        /// <summary>
        /// Adds the specified system.
        /// </summary>
        /// <param name="system">The system.</param>
        public void Add(measureDecadeSystem system)
        {
            Add(system.name, system);
        }

        /// <summary>
        /// Gets the <see cref="measureDecadeSystem"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="measureDecadeSystem"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">System not found in the registry! "+key</exception>
        public new measureDecadeSystem this[String key]
        {
            get
            {
                if (!ContainsKey(key))
                {
                    throw new ArgumentOutOfRangeException(nameof(key), "System not found in the registry! " + key);
                }
                return base[key];
            }
            internal set
            {
                Add(key, value);
            }
        }

        /// <summary>
        /// Gets the <see cref="measureDecadeSystem"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="measureDecadeSystem"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public measureDecadeSystem this[measureSystemsEnum key]
        {
            get
            {
                return base[key.ToString()];
            }
            internal set
            {
                Add(key.ToString(), value);
            }
        }
    }
}