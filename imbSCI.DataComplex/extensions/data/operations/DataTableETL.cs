// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTableETL.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.extensions.data.operations
{
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.typeworks;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    public static class DataTableETL
    {
        /*Converts List To DataTable*/

        /// <summary>
        /// To the data table: https://www.codeproject.com/Tips/784090/Conversion-Between-DataTable-and-List-in-Csharp
        /// Adjusted code: introduced custom flags
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static DataTable ToDataTable<TSource>(this IList<TSource> data, BindingFlags flags = BindingFlags.Default)
        {
            if (flags == BindingFlags.Default) flags = defaultPropertySelector;
            DataTable dataTable = new DataTable(typeof(TSource).Name);
            PropertyInfo[] props = typeof(TSource).GetProperties(flags);
            foreach (PropertyInfo prop in props)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ??
                    prop.PropertyType);
            }

            foreach (TSource item in data)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static BindingFlags defaultPropertySelector = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        ///*Converts DataTable To List*/
        ///// <summary>
        ///// To the list: https://www.codeproject.com/Tips/784090/Conversion-Between-DataTable-and-List-in-Csharp
        ///// </summary>
        ///// <typeparam name="TSource">The type of the source.</typeparam>
        ///// <param name="dataTable">The data table.</param>
        ///// <returns></returns>
        //public static List<TSource> ToList<TSource>(this DataTable dataTable, BindingFlags flags=BindingFlags.Default) where TSource : new()
        //{
        //    var dataList = new List<TSource>();

        //    if (flags == BindingFlags.Default) flags = defaultPropertySelector;

        //    var objFieldNames = Enumerable.ToList((from PropertyInfo aProp in typeof(TSource).GetProperties(flags)
        //        select new
        //               {
        //                   Name = aProp.Name,
        //                   Type = Nullable.GetUnderlyingType(aProp.PropertyType) ??
        //                          aProp.PropertyType
        //               }));
        //    var dataTblFieldNames = Enumerable.ToList((from DataColumn aHeader in dataTable.Columns
        //        select new
        //               {
        //                   Name = aHeader.ColumnName,
        //                   Type = aHeader.DataType
        //               }));

        //    var commonFields = Enumerable.ToList(Enumerable.Intersect<TSource>(objFieldNames, dataTblFieldNames));

        //    foreach (DataRow dataRow in Enumerable.ToList(dataTable.AsEnumerable()))
        //    {
        //        var aTSource = new TSource();
        //        foreach (var aField in commonFields)
        //        {
        //            PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);
        //            var value = (dataRow[(string) aField.Name] == DBNull.Value) ?
        //            null : dataRow[(string) aField.Name]; //if database field is nullable
        //            propertyInfos.SetValue(aTSource, value, null);
        //        }
        //        dataList.Add(aTSource);
        //    }
        //    return dataList;
        //}

        /// <summary>
        /// Creates the nested data table. http://stackoverflow.com/questions/33823820/convert-generic-nested-list-to-datatable
        /// </summary>
        /// <typeparam name="TOuter">The type of the outer.</typeparam>
        /// <typeparam name="TInner">The type of the inner.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="innerListPropertyName">Name of the inner list property.</param>
        /// <returns></returns>
        public static DataTable CreateNestedDataTable<TOuter, TInner>(this IEnumerable<TOuter> list, string innerListPropertyName)
        {
            PropertyInfo[] outerProperties = Enumerable.ToArray(Enumerable.Where(typeof(TOuter).GetProperties(), pi => pi.Name != innerListPropertyName));
            PropertyInfo[] innerProperties = typeof(TInner).GetProperties();
            MethodInfo innerListGetter = typeof(TOuter).GetProperty(innerListPropertyName).GetGetMethod(); //.GetMethod;

            // set up columns
            DataTable table = new DataTable();
            foreach (PropertyInfo pi in outerProperties)
                table.Columns.Add(pi.Name, Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType);
            foreach (PropertyInfo pi in innerProperties)
                table.Columns.Add(pi.Name, Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType);

            // iterate through outer items
            foreach (TOuter outerItem in list)
            {
                var innerList = innerListGetter.Invoke(outerItem, null) as IEnumerable<TInner>;
                if (innerList == null || Enumerable.Count(innerList) == 0)
                {
                    // outer item has no inner items
                    DataRow row = table.NewRow();
                    foreach (PropertyInfo pi in outerProperties)
                        row[pi.Name] = outerItem.imbGetPropertySafe(pi) ?? DBNull.Value;
                    table.Rows.Add(row);
                }
                else
                {
                    // iterate through inner items
                    foreach (object innerItem in innerList)
                    {
                        DataRow row = table.NewRow();
                        foreach (PropertyInfo pi in outerProperties)
                            row[pi.Name] = outerItem.imbGetPropertySafe(pi) ?? DBNull.Value;
                        foreach (PropertyInfo pi in innerProperties)
                            row[pi.Name] = innerItem.imbGetPropertySafe(pi) ?? DBNull.Value;
                        table.Rows.Add(row);
                    }
                }
            }

            return table;
        }

        private static readonly IDictionary<Type, ICollection<PropertyInfo>> _Properties =
        new Dictionary<Type, ICollection<PropertyInfo>>();

        private static DataTable currentDT;

        /// <summary>
        /// Converts a DataTable to a list with generic objects. credits: https://codereview.stackexchange.com/questions/30714/converting-datatable-to-list-of-class
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static IEnumerable<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                var objType = typeof(T);
                ICollection<PropertyInfo> properties;

                lock (_Properties)
                {
                    if (!_Properties.TryGetValue(objType, out properties))
                    {
                        properties = Enumerable.ToList(Enumerable.Where(objType.GetProperties(), property => property.CanWrite));
                        _Properties.Add(objType, properties);
                    }
                }

                var list = new List<T>(table.Rows.Count);

                foreach (DataRow row in Enumerable.Skip(table.Rows.ToList(), 1))
                {
                    var obj = new T();

                    foreach (PropertyInfo prop in properties)
                    {
                        try
                        {
                            var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                            var safeValue = row[prop.Name] == null ? null : Convert.ChangeType(row[prop.Name], propType);

                            prop.SetValue(obj, safeValue, null);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// Gets a Inverted DataTable
        /// </summary>
        /// <param name="table">Provided DataTable</param>
        /// <param name="columnX">X Axis Column</param>
        /// <param name="columnY">Y Axis Column</param>
        /// <param name="columnZ">Z Axis Column (values)</param>
        /// <param name="columnsToIgnore">Whether to ignore some column, it must be
        /// provided here</param>
        /// <param name="nullValue">null Values to be filled</param>
        /// <returns>C# Pivot Table Method  - Felipe Sabino</returns>
        public static DataTable GetInversedDataTable(DataTable table, string columnX,
             string columnY, string columnZ, string nullValue, bool sumValues)
        {
            //Create a DataTable to Return
            DataTable returnTable = new DataTable();

            if (columnX == "")
                columnX = table.Columns[0].ColumnName;

            //Add a Column at the beginning of the table
            returnTable.Columns.Add(columnY);

            //Read all DISTINCT values from columnX Column in the provided DataTale
            List<string> columnXValues = new List<string>();

            foreach (DataRow dr in table.Rows)
            {
                string columnXTemp = dr[columnX].ToString();
                if (!columnXValues.Contains(columnXTemp))
                {
                    //Read each row value, if it's different from others provided, add to
                    //the list of values and creates a new Column with its value.
                    columnXValues.Add(columnXTemp);
                    returnTable.Columns.Add(columnXTemp);
                }
            }

            //Verify if Y and Z Axis columns re provided
            if (columnY != "" && columnZ != "")
            {
                //Read DISTINCT Values for Y Axis Column
                List<string> columnYValues = new List<string>();

                foreach (DataRow dr in table.Rows)
                {
                    if (!columnYValues.Contains(dr[columnY].ToString()))
                        columnYValues.Add(dr[columnY].ToString());
                }

                //Loop all Column Y Distinct Value
                foreach (string columnYValue in columnYValues)
                {
                    //Creates a new Row
                    DataRow drReturn = returnTable.NewRow();
                    drReturn[0] = columnYValue;
                    //foreach column Y value, The rows are selected distincted
                    DataRow[] rows = table.Select(columnY + "='" + columnYValue + "'");

                    //Read each row to fill the DataTable
                    foreach (DataRow dr in rows)
                    {
                        string rowColumnTitle = dr[columnX].ToString();

                        //Read each column to fill the DataTable
                        foreach (DataColumn dc in returnTable.Columns)
                        {
                            if (dc.ColumnName == rowColumnTitle)
                            {
                                //If Sum of Values is True it try to perform a Sum
                                //If sum is not possible due to value types, the value
                                // displayed is the last one read
                                if (sumValues)
                                {
                                    try
                                    {
                                        drReturn[rowColumnTitle] =
                                             Convert.ToDecimal(drReturn[rowColumnTitle]) +
                                             Convert.ToDecimal(dr[columnZ]);
                                    }
                                    catch
                                    {
                                        drReturn[rowColumnTitle] = dr[columnZ];
                                    }
                                }
                                else
                                {
                                    drReturn[rowColumnTitle] = dr[columnZ];
                                }
                            }
                        }
                    }
                    returnTable.Rows.Add(drReturn);
                }
            }
            else
            {
                throw new Exception("The columns to perform inversion are not provided");
            }

            //if a nullValue is provided, fill the datable with it
            if (nullValue != "")
            {
                foreach (DataRow dr in returnTable.Rows)
                {
                    foreach (DataColumn dc in returnTable.Columns)
                    {
                        if (dr[dc.ColumnName].ToString() == "")
                            dr[dc.ColumnName] = nullValue;
                    }
                }
            }

            return returnTable;
        }

        /// <summary>
        /// Gets a Inverted DataTable
        /// </summary>
        /// <param name="table">DataTable do invert</param>
        /// <param name="columnX">X Axis Column</param>
        /// <param name="nullValue">null Value to Complete the Pivot Table</param>
        /// <param name="columnsToIgnore">Columns that should be ignored in the pivot
        /// process (X Axis column is ignored by default)</param>
        /// <returns>C# Pivot Table Method  - Felipe Sabino</returns>
        public static DataTable GetInversedDataTable(this DataTable table, string columnX,
                                                     params string[] columnsToIgnore)
        {
            //Create a DataTable to Return
            DataTable returnTable = new DataTable();
            if (!table.validateTable()) return returnTable;

            if (columnX == "")
                columnX = table.Columns[0].ColumnName;

            //Add a Column at the beginning of the table

            returnTable.Columns.Add(columnX);

            //Read all DISTINCT values from columnX Column in the provided DataTale
            List<string> columnXValues = new List<string>();

            //Creates list of columns to ignore
            List<string> listColumnsToIgnore = new List<string>();
            if (columnsToIgnore.Length > 0)
                listColumnsToIgnore.AddRange(columnsToIgnore);

            if (!listColumnsToIgnore.Contains(columnX))
                listColumnsToIgnore.Add(columnX);

            foreach (DataRow dr in table.Rows)
            {
                string columnXTemp = dr[columnX].ToString();
                //Verify if the value was already listed
                if (!columnXValues.Contains(columnXTemp))
                {
                    //if the value id different from others provided, add to the list of
                    //values and creates a new Column with its value.
                    columnXValues.Add(columnXTemp);
                    returnTable.Columns.Add(columnXTemp);
                }
                else
                {
                    //Throw exception for a repeated value
                    throw new Exception("The inversion used must have " +
                                        "unique values for column " + columnX);
                }
            }

            //Add a line for each column of the DataTable

            foreach (DataColumn dc in table.Columns)
            {
                if (!columnXValues.Contains(dc.ColumnName) &&
                    !listColumnsToIgnore.Contains(dc.ColumnName))
                {
                    DataRow dr = returnTable.NewRow();
                    dr[0] = dc.ColumnName;
                    returnTable.Rows.Add(dr);
                }
            }

            //Complete the datatable with the values
            for (int i = 0; i < returnTable.Rows.Count; i++)
            {
                for (int j = 1; j < returnTable.Columns.Count; j++)
                {
                    returnTable.Rows[i][j] =
                      table.Rows[j - 1][returnTable.Rows[i][0].ToString()].ToString();
                }
            }

            return returnTable;
        }
    }
}