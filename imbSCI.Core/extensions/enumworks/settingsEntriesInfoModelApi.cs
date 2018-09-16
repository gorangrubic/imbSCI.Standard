// --------------------------------------------------------------------------------------------------------------------
// <copyright file="settingsEntriesInfoModelApi.cs" company="imbVeles" >
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
    using imbSCI.Core.collection;
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.data;
    using imbSCI.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class settingsEntriesInfoModelApi
    {
        public static Dictionary<Object, settingsMemberInfoEntry> getMemberInfoDictionaryForKeys(this IEnumerable<Object> keys)
        {
            Dictionary<Object, settingsMemberInfoEntry> output = new Dictionary<object, settingsMemberInfoEntry>();

            foreach (Object k in keys)
            {
                settingsMemberInfoEntry tmp = null;

                Type kt = k.GetType();
                MemberInfo memberInfo = kt;

                if (k is Enum)
                {
                    memberInfo = kt.GetMember(k.ToString()).First();
                    tmp = new settingsMemberInfoEntry(memberInfo);
                }
                else if (k is String)
                {
                    tmp = new settingsMemberInfoEntry(k);
                }
                if (kt.IsValueType)
                {
                    tmp = new settingsMemberInfoEntry(k);
                }
                else
                {
                    tmp = new settingsMemberInfoEntry(kt);
                }
                output.Add(k, tmp);
            }

            return output;
        }

        /// <summary>
        /// Gets the description dictionary for Enum or other object keys
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        public static Dictionary<Object, String> getDescriptionDictionary(this IEnumerable<Object> keys)
        {
            Dictionary<Object, String> output = new Dictionary<Object, String>();

            foreach (Object k in keys)
            {
                output.Add(k, k.getDescriptionForDictionary());
            }

            return output;
        }

        /// <summary>
        /// Gets the description for dictionary, property table etc
        /// </summary>
        /// <param name="k">The k.</param>
        /// <returns></returns>
        public static String getDescriptionForDictionary(this Object k)
        {
            settingsMemberInfoEntry tmp = null;

            Type kt = k.GetType();
            MemberInfo memberInfo = kt;
            String desc = "";

            if (k is PropertyEntryColumn)
            {
                desc = PropertyEntryColumnExtensions.getDescriptionForKey((PropertyEntryColumn)k);
            }
            else if (k is Enum)
            {
                memberInfo = kt.GetMember(k.ToString()).First();
                tmp = new settingsMemberInfoEntry(memberInfo);
                desc = tmp.description;
                if (tmp.description.isNullOrEmptyString()) desc = tmp.displayName;
            }
            else
            {
                tmp = new settingsMemberInfoEntry(kt);
                desc = tmp.description;
            }
            return desc;
        }
    }
}