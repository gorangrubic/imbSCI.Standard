﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="collectionAggregationResult.cs" company="imbVeles" >
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
    using imbSCI.Core.math.aggregation;
    using System.Collections.Generic;

    /// <summary>
    /// Results of collection aggregation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.Dictionary{aceCommonTypes.math.aggregation.dataPointAggregationType, T}" />
    public class collectionAggregationResult<T> : Dictionary<dataPointAggregationType, T> where T : class, new()
    {
        public T firstItem { get; set; }
        public T lastItem { get; set; }

        public int Count { get; set; } = 0;

        public dataPointAggregationType type { get; set; }
        public dataPointAggregationAspect aspect { get; set; }
    }
}