// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMeasureBasic.cs" company="imbVeles" >
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
namespace imbSCI.Core.interfaces
{
    using imbSCI.Core.enums;
    using imbSCI.Core.math.measureSystem.core;
    using imbSCI.Data.interfaces;
    using System;

    public interface IMeasureBasic : IObjectWithNameAndDescription, IValueWithFormat, IValueWithImportanceInfo, IValueWithToString, IValueComparable, IObjectWithMetaModelNameAndGroup, IComparable
    {
        string GetFormatedValue();

        string GetFormatedValueAndUnit();

        IRangeValue valueRange { get; }

        IRangeCriteria alarmCriteria { get; }

        TC getValue<TC>();

        IMeasureBasic calculate(IMeasure second, operation op);

        measureOperandList calculateTasks { get; }

        /// <summary>
        /// If true it will use alarm range to fire alarm
        /// </summary>
        Boolean isAlarmTurnedOn { get; set; }

        bool isValueInAlarmRange { get; }

        /// <summary>
        /// Gets the format for value or value-and-unit if forValueAndUnit is TRUE.
        /// </summary>
        /// <param name="forValueAndUnit">if set to <c>true</c> returns format for value and unit.</param>
        /// <returns>Format string</returns>
        String GetFormatForValue(Boolean forValueAndUnit = false);

        /// <summary>
        /// TRUE if current value is same as default value specified with constructor
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is value default; otherwise, <c>false</c>.
        /// </value>
        Boolean isValueDefault { get; }
    }
}