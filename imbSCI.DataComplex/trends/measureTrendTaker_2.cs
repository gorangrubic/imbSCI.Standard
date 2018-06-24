// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureTrendTaker_2.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.trends
{
    using imbSCI.Data.interfaces;
    using System;

    public class measureTrendTaker<T> : measureTrendTaker where T : class, IPerformanceTake, new()
    {
        public Func<T, double> selector { get; set; }

        public measureTrendTaker(Func<T, double> __selector, string __name, string __unit, int __macroSampleSize, int __microSampleSize = -1, int __spearSampleSize = -1, double __zeroMargin = -1) : base(__name, __unit, __macroSampleSize, __microSampleSize, __spearSampleSize, __zeroMargin)
        {
            selector = __selector;
        }
    }
}