// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataTableShemaExtensions.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataComplex.extensions.data.schema
{
    using imbSCI.Core.data;
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.text;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Data;

    public static class dataTableShemaExtensions
    {
        /// <summary>
        /// Gets the cloned shema.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
     /*   public static T GetClonedShema<T>(this DataTable table, bool disablePrimaryKey = true, String[] categories = null) where T : DataTable, new()
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
                DataColumn dce = output.Add(dc.GetSPE());
               // dce.SetValueType(dc.GetValueType()); //<--------- ad hok hack --- da vidim

                //clone.Columns.Remove(dc);
                //output.Columns.Add(dc);

                // output.Columns[dc.ColumnName].ExtendedProperties.AppendData(dc.ExtendedProperties, existingDataMode.overwriteExisting);
            }
            if (catPri.Any())
            {
                output.Columns.OrderByCategoryPriority(catPri);
            }

            if (output.TableName.isNullOrEmpty()) output.TableName = "datatable_" + imbStringGenerators.getRandomString(4);

            return output;
        }*/
        /*
        public static void AddColumns(this DataTable table, Type type, params string[] columns)
        {
            settingsEntriesForObject seo = new settingsEntriesForObject(type, false);

            foreach (string col in columns)
            {
                if (seo.spes.ContainsKey(col))
                {
                    table.Add(col, seo.spes[col].description, "", seo.pis[col].PropertyType, seo.spes[col].importance, seo.spes[col].format, seo.spes[col].displayName);
                }
                else
                {
                }
            }
        }*/

        /// <summary>
        /// Adds column using meta information of <see cref="imbSCI.Core.data.settingsPropertyEntry"/>
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="spe">The spe.</param>
        /// <returns>newly added column</returns>
        public static DataColumn Add(this DataTable table, settingsPropertyEntry spe)
        {
            DataColumn output = Add(table, (string)spe.name, spe.description, spe.letter, spe.type, spe.importance, spe.format, spe.displayName);
            output.SetClassName(spe.type.Name);

            // output.SetPriority(spe.priority);
            output.SetSPE(spe);
            // output.SetGroup(spe.categoryName);
            output.SetHasLinks(spe.attributes.containsAnyOfKeys(templateFieldDataTable.col_hasLinks));
            output.SetHasTemplate(spe.attributes.containsAnyOfKeys(templateFieldDataTable.col_hasTemplate));

            return output;
        }

        public static DataColumn Add(this DataTable table, string columnName, string description = "", string letterCode = "", Type valueType = null, dataPointImportance importance = dataPointImportance.normal, string format = "", string displayName = "")
        {
            if (table.Columns.Contains(columnName)) return table.Columns[columnName];

            DataColumn dc = table.Columns.Add(columnName);
            if (displayName.isNullOrEmpty())
            {
                dc.SetHeading(columnName.imbTitleCamelOperation(true));
            }
            else
            {
                dc.SetHeading(displayName);
            }
            if (!description.isNullOrEmpty()) dc.SetDesc(description);
            if (!letterCode.isNullOrEmpty()) dc.SetLetter(letterCode);
            if (valueType == null) valueType = typeof(string);

            dc.SetValueType(valueType);
            dc.SetImportance(importance);
            dc.SetFormat(format);
            return dc;
        }

        public static DataColumn Add(this DataTable table, Enum columnEnum, string description = "", string letterCode = "", Type valueType = null, dataPointImportance importance = dataPointImportance.normal, string format = "", string displayName = "")
        {
            return table.Add(columnEnum.ToString(), description, letterCode, valueType, importance, format, displayName);
        }
    }
}