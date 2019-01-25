// --------------------------------------------------------------------------------------------------------------------
// <copyright file="histogramModelExtensions.cs" company="imbVeles" >
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
using System;
using System.Collections.Generic;
using System.Data;

namespace imbSCI.Core.math.range.histogram
{
    public static class histogramModelExtensions
    {
        /// <summary>
        /// Gets the histogram model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceSet">The source set.</param>
        /// <param name="name">The name.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="bins">The bins.</param>
        /// <returns></returns>
        public static histogramModel GetHistogramModel<T>(this IEnumerable<T> sourceSet, String name, Func<T, double> selector, Int32 bins = 10)
        {
            histogramModel model = new histogramModel(bins, name);
            foreach (var s in sourceSet)
            {
                model.ranger.Learn(selector(s));
            }

            model.processData();
            return model;
        }


        public static DataTable BlendHistogramModels(this IEnumerable<histogramModel> histograms, String name)
        {
            DataTable output = new DataTable(name);

            output.SetTitle(name);

            var cn_name = output.Columns.Add(histogramModel.DEFAULT_COLUMN_NAME);
            Int32 c = 0;
            Int32 lim = 0;
            Dictionary<DataColumn, histogramModel> dict = new Dictionary<DataColumn, histogramModel>();
            foreach (histogramModel model in histograms)
            {
                var dc = output.Columns.Add(model.name + c.ToString("D2"));
                dict.Add(dc, model);
                if (lim == 0)
                {
                    lim = model.bins.Count;
                }
                else
                {
                    lim = Math.Min(lim, model.bins.Count);
                }
                c++;
            }


            //var cn_value = output.Columns.Add(DEFAULT_COLUMN_VALUE);

            for (int i = 0; i < lim; i++)
            {
                Boolean first = true;
                var dr = output.NewRow();
                foreach (var pair in dict)
                {

                    var dc = pair.Key;


                    var bin = pair.Value.bins[i];

                    if (first)
                    {
                        dr[cn_name] = bin.Label;
                        first = false;
                    }

                    dr[dc] = bin.values.Count;



                }
                output.Rows.Add(dr);
            }



            return output;


        }

        /// <summary>
        /// Processes the data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model.</param>
        /// <param name="sourceSet">The source set.</param>
        /// <param name="selector">The selector.</param>
        public static void ProcessData<T>(this histogramModel model, IEnumerable<T> sourceSet, Func<T, double> selector)
        {
            foreach (var s in sourceSet)
            {
                model.ranger.Learn(selector(s));
            }

            model.processData();
        }
    }
}