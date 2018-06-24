// --------------------------------------------------------------------------------------------------------------------
// <copyright file="samplingSettings.cs" company="imbVeles" >
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
// Project: imbSCI.Data
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
using System.ComponentModel;

namespace imbSCI.Data.data.sample
{
    /// <summary>
    /// Configuration for <see cref="sampleTake{T}"/>
    /// </summary>
    public class samplingSettings
    {
        /// <summary> Number of items to skip </summary>
        [Category("Count")]
        [DisplayName("skip")]
        [Description("Number of items to skip, or ID of the part to take")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Int32 skip { get; set; } = 0;

        /// <summary>
        /// Total number of items allowed to get into sample
        /// </summary>
        /// <value>
        /// The limit.
        /// </value>
        public Int32 limit { get; set; } = -1;

        /// <summary>
        /// Defines number of equal parts for sampling algorithms like: n-fold, every n-th
        /// </summary>
        /// <value>
        /// The parts.
        /// </value>
        public Int32 parts { get; set; } = 1;

        ///// <summary>
        ///// Which block/part was taken
        ///// </summary>
        ///// <value>
        ///// The part identifier.
        ///// </value>
        //public Int32 partID { get; set; } = -1;

        /// <summary>
        /// It will return only unique instances to be collected, no overlap
        /// </summary>
        /// <value>
        ///   <c>true</c> if [only unique]; otherwise, <c>false</c>.
        /// </value>
        public Boolean onlyUnique { get; set; } = true;

        public samplingOrderEnum takeOrder { get; set; } = samplingOrderEnum.ordinal;
    }
}