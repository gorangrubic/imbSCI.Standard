// --------------------------------------------------------------------------------------------------------------------
// <copyright file="rangeFinder.cs" company="imbVeles" >
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
using imbSCI.Core.attributes;
using imbSCI.Core.data;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace imbSCI.Core.math.range.finder
{
    public static class rangeFinderExtensions
    {

        public static List<rangeFinderCollectionForMetrics<T>> BuildRangersFromNonAlligned<T>(this IEnumerable<T> input) where T : class, IObjectWithName
        {
            var dict = input.GetAllignedByName<T>(x => x.name);


            return dict.Values.BuildRangersFromAlligned<T>();
        }


        public static List<rangeFinderCollectionForMetrics<T>> BuildRangersFromAlligned<T>(this IEnumerable<IEnumerable<T>> input) where T : class, IObjectWithName
        {
            List<rangeFinderCollectionForMetrics<T>> output = new List<rangeFinderCollectionForMetrics<T>>();

            foreach (IEnumerable<T> inner in input)
            {
                if (inner.Any())
                {
                    var term_finder = new rangeFinderCollectionForMetrics<T>();
                    var first = inner.First();
                    term_finder.name = first.name;

                    term_finder.Learn(inner);
                    output.Add(term_finder);
                }
            }

            return output;
        }


        public static Dictionary<String, DataTable> BuildDataTableSplits<T>(this IEnumerable<rangeFinderCollectionForMetrics<T>> input, Int32 numberOfSplits, String name = "", String description = "") where T : class
        {
            var first = input.First();

            String name_prototype = name.or(first.name);
            String desc_prototype = description.or("Value ranges for type [" + typeof(T).Name + "]");

            Dictionary<String, DataTable> output = new Dictionary<string, DataTable>();

            Int32 c = 0;
            Int32 b = 0;
            Int32 block = input.Count() / numberOfSplits;

            DataTable table = first.SetDataTable();
            table.SetTitle(name_prototype);
            table.SetDescription(desc_prototype);


            foreach (rangeFinderCollectionForMetrics<T> metrics in input)
            {
                var dr = table.NewRow();

                metrics.SetDataRow(dr);

                table.Rows.Add(dr);
                c++;
                if (c > block)
                {
                    String n = table.TableName;

                    if (output.ContainsKey(n))
                    {
                        n = n + b.ToString("D3");
                        table.TableName = n;
                        table.SetTitle(n);
                    }
                    else
                    {

                    }
                    output.Add(n, table);
                    table = first.SetDataTable();
                    table.TableName = name_prototype + "_" + output.Count.ToString();
                    table.SetDescription(desc_prototype + ".Block [" + output.Count.ToString() + "]");
                    c = 0;
                    b++;
                }
            }

            return output;

        }



        public static DataTable BuildDataTable<T>(this IEnumerable<rangeFinderCollectionForMetrics<T>> input, String name = "", String description = "") where T : class
        {

            return input.BuildDataTableSplits<T>(1, name, description).First().Value;

        }
    }
}