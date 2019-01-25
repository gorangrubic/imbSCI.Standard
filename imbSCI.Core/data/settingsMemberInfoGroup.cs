// --------------------------------------------------------------------------------------------------------------------
// <copyright file="settingsMemberInfoGroup.cs" company="imbVeles" >
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
using imbSCI.Core.extensions.table;
using imbSCI.Core.reporting;
using System;
using System.Collections.Generic;
using System.Data;

namespace imbSCI.Core.data
{
    /// <summary>
    /// Group of properties described trough <see cref="settingsMemberInfoEntry"/> instances
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{imbSCI.Core.data.settingsMemberInfoEntry}" />
    public class settingsMemberInfoGroup : List<settingsMemberInfoEntry>
    {

        ///// <summary>
        ///// Adds columns and transfers formating and other meta information specified in the <see cref="settingsPropertyEntry"/> entries. 
        ///// </summary>
        ///// <param name="table">The table to write columns into. If not specified, it will create new with name of the group.</param>
        ///// <remarks>The method will declare only <see cref="settingsPropertyEntry"/>s, not the entries of other type</remarks>
        ///// <returns></returns>
        //public DataTable SetDataTable(DataTable table = null)
        //{
        //    if (table == null) table = new DataTable(GroupName);

        //    return settingsEntriesForObjectExtensions.SetDataTable(this, table); 
        //}

        //public Dictionary<String, Object> GetDictionary(Object data, String prefix = "")
        //{
        //    foreach (settingsMemberInfoEntry entry in this)
        //    {
        //        if ()
        //    }
        //}

        //public DataRow SetDataRow(DataRow dr, DataTable table, Object data, String prefix = "", bool addRowBeforeEnd = true)
        //{
        //    if (dr == null) dr = table.NewRow(); // new DataTable(name);

        //    var dict = GetDictionary();

        //    if (!table.Columns.Contains("Name"))
        //    {
        //        dr["Name"] = name;
        //    }

        //    foreach (var p in dict)
        //    {
        //        dr[p.Key] = p.Value;
        //    }


        //    if (addRowBeforeEnd)
        //    {
        //        table.Rows.Add(dr);
        //    }

        //    return dr;
        //}



        public settingsMemberInfoGroup()
        {
        }

        public settingsMemberInfoGroup(String groupName)
        {
            GroupName = groupName;
        }

        public String GroupName { get; set; } = "";
    }
}