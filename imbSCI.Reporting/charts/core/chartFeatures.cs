// --------------------------------------------------------------------------------------------------------------------
// <copyright file="chartFeatures.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.charts.core
{
    using System;

    [Flags]
    public enum chartFeatures
    {
        none = 0,
        showData = 1,
        urlJSON = 2,
        urlCSV = 4,
        axisY2 = 8,
        showY1AxisLabel = 16,
        showY2AxisLabel = 32,
        /// <summary>
        /// The generated C3 code will be only JavaScript part
        /// </summary>
        withoutHtml = 64,
        skipFirstRow = 128,
        /// <summary>
        /// DEPRICATED
        /// </summary>
        bindto = 256,
        /// <summary>
        /// it will transpose (rotate columns and rows) table, to show data horizontally
        /// </summary>
        transposeTable = 512

        ///// <summary>
        ///// The types: one line template for types insertation
        ///// </summary>
        //types = 128,
    }
}