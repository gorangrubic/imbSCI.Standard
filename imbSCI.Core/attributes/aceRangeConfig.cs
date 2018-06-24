// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceRangeConfig.cs" company="imbVeles" >
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
namespace imbSCI.Core.attributes
{
    using imbSCI.Core.math.range;
    using System;

    /// <summary>
    /// range configuration structure
    /// </summary>
    public class aceRangeConfig
    {
        public aceRangeConfig()
        {
        }

        /// <summary>
        /// Defines range
        /// </summary>
        /// <param name="_min">The minimum.</param>
        /// <param name="_max">The maximum.</param>
        /// <param name="_rule">The rule.</param>
        public aceRangeConfig(IComparable _min, IComparable _max, numberRangeModifyRule _rule)
        {
            min = _min;
            max = _max;
            rule = _rule;
        }

        public IComparable min;
        public IComparable max;
        public numberRangeModifyRule rule;

        /// <summary>
        ///returns min - max value retrieved by ToString calls on <see cref="min"/> and <see cref="max"/>
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public String ToString()
        {
            return min.ToString() + " - " + max.ToString();
        }
    }
}