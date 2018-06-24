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
using System;
using System.Collections.Generic;

namespace imbSCI.Core.math.range.histogram
{
    public static class histogramModelExtensions
    {
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