// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMeasure.cs" company="imbVeles" >
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
    using imbSCI.Core.math.measureSystem.core;
    using imbSCI.Core.math.measureSystem.enums;
    using imbSCI.Core.math.range;
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <seealso cref="IMeasure" />
    public interface IMeasure<TValue> : IMeasure where TValue : IComparable
    {
        void setAlarmExact(TValue _alarmValue, Boolean _alarmOn);

        /// <summary>
        ///
        /// </summary>
        rangeCriteria<TValue> alarmCriteria { get; }

        /// <summary>
        ///
        /// </summary>
        TValue defValue { get; set; }

        /// <summary>
        ///
        /// </summary>
        rangeValueBase<TValue> valueRange { get; set; }

        /// <summary>
        ///
        /// </summary>
        TValue primValue { get; set; }

        /// <summary>
        ///
        /// </summary>
        TValue baseValue { get; set; }

        void setDefaultValue(TValue defaultValue, TValue defaultBaseValue);
    }

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="IMeasure" />
    public interface IMeasure : IAceMathMeasure, IMeasureBasic, IValueWithRoleInfo, IValueWithUnitInfo
    {
        measureSystemUnitEntry setCustomUnitEntry();

        measureSystemRoleEntry setCustomRoleEntry(String __letter = "", String __symbol = "", String __name = "");

        measureSystemRoleEntry getRoleEntry(Object input);

        measureSystemUnitEntry getUnitEntry(Object input);

        measureInfo info { get; }

        Boolean doUnitOptimization { get; set; }

        /// <summary>
        /// Gets a value indicating whether the current value is plural.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is value plural; otherwise, <c>false</c>.
        /// </value>
        Boolean isValuePlural { get; }

        /// <summary>
        /// TRUE if current value is in alarmant value range
        /// </summary>
        //public virtual Boolean isValueInAlarmRange {
        //    get
        //    {
        //        return alarmCriteria.testCriteria(primValue);
        //    }
        //}

        void convertToOptimalUnitLevel();

        void convertToUnit(measureSystemUnitEntry targetUnit);

        /// <summary>
        /// Returns a string that represents the meassure - according to
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        String ToString();

        /// <summary>
        /// Gets the informational content about this measure
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        List<Object> GetContent(measureToStringContent content);

        String ToString(measureToStringContent content);
    }
}