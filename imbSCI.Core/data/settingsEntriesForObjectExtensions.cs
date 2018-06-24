// --------------------------------------------------------------------------------------------------------------------
// <copyright file="settingsEntriesForObjectExtensions.cs" company="imbVeles" >
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
using imbSCI.Core.extensions.data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Core.data
{
    /// <summary>
    /// Extensions for <see cref="settingsEntriesForObject"/>
    /// </summary>
    public static class settingsEntriesForObjectExtensions
    {
        /// <summary>
        /// Builds member info group dictionary
        /// </summary>
        /// <param name="objectInfo">The object information.</param>
        /// <returns></returns>
        public static settingsMemberInfoGroupDictionary GetMemberInfoGroupDictionary(this settingsEntriesForObject objectInfo)
        {
            List<String> groups = new List<string>();
            settingsMemberInfoGroupDictionary output = new settingsMemberInfoGroupDictionary();

            if (objectInfo.CategoryByPriority.Any())
            {
                groups = objectInfo.CategoryByPriority;
            }
            else
            {
                foreach (var pp in objectInfo.spes)
                {
                    if (!groups.Contains(pp.Value.categoryName))
                    {
                        groups.Add(pp.Value.categoryName);
                    }
                }
            }

            foreach (String g in groups)
            {
                output.Add(g, new settingsMemberInfoGroup(g));
            }

            foreach (var pp in objectInfo.spes)
            {
                if (!pp.Value.categoryName.isNullOrEmpty())
                {
                    if (groups.Contains(pp.Value.categoryName))
                    {
                        output[pp.Value.categoryName].Add(pp.Value);
                    }
                }
            }

            return output;
        }
    }
}