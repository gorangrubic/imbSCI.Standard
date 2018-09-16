// --------------------------------------------------------------------------------------------------------------------
// <copyright file="tableStyleSetterResponse.cs" company="imbVeles" >
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

namespace imbSCI.Core.extensions.table.style
{
    using imbSCI.Core.extensions.table.core;
    using imbSCI.Core.reporting.zone;

    /// <summary>
    /// Response created by a setter
    /// </summary>
    public class tableStyleSetterResponse
    {
        public static implicit operator dataTableStyleEntry(tableStyleSetterResponse response)
        {
            return response.style;
        }

        public Double weight { get; set; } = 0;

        /// <summary>
        /// Zone - if set - limit application of this style to the zonal borders
        /// </summary>
        /// <value>
        /// The zone.
        /// </value>
        public selectZone zone { get; set; } = new selectZone();

        public dataTableStyleEntry style { get; set; }

        public tableStyleDynamicRule rule { get; set; }

        public List<String> notes { get; set; } = new List<string>();

        public tableStyleSetterResponse(dataTableStyleEntry _style, tableStyleDynamicRule _rule)
        {
            style = _style;
            rule = _rule;
        }

        public tableStyleSetterResponse()
        {
        }
    }
}