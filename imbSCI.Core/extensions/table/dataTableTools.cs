// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataTableTools.cs" company="imbVeles" >
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

namespace imbSCI.Core.extensions.table
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.lowLevelApi;
    using imbSCI.Data;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    public static class dataTableTools
    {
        public static Boolean validationDisabled = false;


        /// <summary>
        /// Gets the cloned shema.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static T GetClonedShema<T>(this DataTable table, bool disablePrimaryKey = true, String[] categories = null) where T : DataTable, new()
        {
            DataTable clone = table.Clone();
            T output = new T();

            ColumnGroups groupsOfColumns = clone.getGroupsOfColumns();

            if (categories != null)
            {
                List<String> matched = new List<string>();
                foreach (String c in categories)
                {
                    var cg = groupsOfColumns.FirstOrDefault(x => x.name.Equals(c, StringComparison.InvariantCultureIgnoreCase));
                    if (cg == null)
                    {
                        if (clone.Columns.Contains(c))
                        {
                            matched.Add(c);
                        }
                    }
                    else
                    {
                        matched.AddRange(cg.Select(x => x.ColumnName));
                    }

                }

                List<DataColumn> dcdel = new List<DataColumn>();
                foreach (DataColumn dc in clone.Columns)
                {
                    if (!matched.Contains(dc.ColumnName))
                    {
                        dcdel.Add(dc);
                    }
                    //String g = dc.GetGroup();
                    //if (!matched.Any())
                    //{
                    //    if (!matched.Contains(dc.ColumnName))
                    //    {

                    //    }
                    //}
                    //else
                    //{
                    //    if (!matched.Contains(g))
                    //    {
                    //        dcdel.Add(dc);
                    //    }
                    //}
                }

                foreach (DataColumn dc in dcdel)
                {
                    clone.Columns.Remove(dc);
                }

            }

            table.ExtendedProperties.copyInto(output.ExtendedProperties);

            output.SetTitle(table.GetTitle());
            output.SetDescription(table.GetDescription());
            output.SetClassName(table.GetClassName());
            output.SetClassType(table.GetClassType());
            output.SetAggregationAspect(table.GetAggregationAspect());
            output.SetAggregationOriginCount(table.GetAggregationOriginCount());
            output.SetAdditionalInfo(table.GetAdditionalInfo());
            output.SetExtraDesc(table.GetExtraDesc());

            output.SetCategoryPriority(table.GetCategoryPriority());

            output.SetStyleSet(table.GetStyleSet());
            output.SetColumnMetaSet(table.GetColumnMetaSet());
            output.SetRowMetaSet(table.GetRowMetaSet());
            //output.SetClassType(table.Get)






            List<string> catPri = table.GetCategoryPriority();

            if (disablePrimaryKey) output.PrimaryKey = new DataColumn[0];

            foreach (DataColumn dc in clone.Columns)
            {
                dc.GetSPE();
            }

            foreach (DataColumn dc in clone.Columns)
            {
                DataColumn dce = output.Columns.Add(dc.ColumnName);
                dce.SetSPE(dc.GetSPE());

            }
            if (catPri.Any())
            {
                output.Columns.OrderByCategoryPriority(catPri);
            }

            if (output.TableName.isNullOrEmpty()) output.TableName = "datatable_" + imbStringGenerators.getRandomString(4);

            return output;
        }

        /// <summary>
        /// Renders the table into text
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static String GetTextTable(this DataTable source)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Title \t\t\\tt [" + source.GetTitle() + "]");
            sb.AppendLine("Description \t\t\t [" + source.GetDescription() + "]");
            sb.AppendLine("---------------------------------------------------------");

            sb.AppendLine(source.markdownTable(true));

            sb.AppendLine("---------------------------------------------------------");

            var extra = source.GetExtraDesc();
            foreach (String ln in extra)
            {
                sb.AppendLine(ln);
            }

            return sb.ToString();
        }


        /// <summary>
        /// Validates the table.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static Boolean validateTable(this DataTable source)
        {
            if (validationDisabled) return true;
            if (source == null) return false;
            if (source.Columns.Count == 0) return false;
            if (source.Rows.Count == 0) return false;

            if (source.TableName.isNullOrEmpty())
            {
                source.TableName = source.GetTitle().getFilename();
                if (source.TableName.isNullOrEmpty())
                {
                    source.TableName = "DataTable_" + imbStringGenerators.getRandomString(8);
                    return true;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }
    }
}