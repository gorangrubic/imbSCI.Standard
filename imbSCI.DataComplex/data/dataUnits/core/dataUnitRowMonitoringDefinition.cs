// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataUnitRowMonitoringDefinition.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.data.dataUnits.core
{
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.DataComplex.special;
    using System;
    using System.Reflection;

    /// <summary>
    ///
    /// </summary>
    public class dataUnitRowMonitoringDefinition
    {
        /// <summary>
        ///
        /// </summary>
        public instanceCountCollection<int> freqCount { get; set; }

        public dataUnitRowMonitoringDefinition(PropertyInfo __toWatch, PropertyInfo __toStore, monitoringFunctionEnum __function)
        {
            pi_toWatch = __toWatch;
            pi_toStore = __toStore;
            function = __function;
        }

        public void prepare()
        {
            storedResult = 0;
            newResult = 0;
            storedValue = 0;
            newValue = 0;
        }

        /// <summary>
        /// Reads the value and resolves
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public int readValueAndResolveInt32(object instance, bool saveToProperty = true)
        {
            int output = 0;
            storedValue = newValue;

            newValue = Convert.ToInt32(pi_toWatch.GetValue(instance, null));

            switch (function)
            {
                case monitoringFunctionEnum.stability:
                    if ((storedValue == newValue) && (newValue > 0))
                    {
                        output = storedResult + 1;
                    }
                    else
                    {
                        output = 0;
                    }
                    break;

                case monitoringFunctionEnum.change:
                    output = storedValue - newValue;
                    break;

                case monitoringFunctionEnum.minFreq:
                    freqCount.AddInstance(newValue, "readValueAndResolveInt32() at dataUnitRowMonitoringDefinition");
                    newValue = freqCount[newValue];
                    output = Math.Min(storedResult, newValue);
                    break;

                case monitoringFunctionEnum.maxFreq:
                    freqCount.AddInstance(newValue, "readValueAndResolveInt32() at dataUnitRowMonitoringDefinition");
                    newValue = freqCount[newValue];
                    output = Math.Max(storedResult, newValue);
                    break;

                case monitoringFunctionEnum.summary:

                    output = storedResult + newValue;
                    break;

                case monitoringFunctionEnum.min:
                    output = Math.Min(storedResult, newValue);
                    break;

                case monitoringFunctionEnum.max:
                    output = Math.Max(storedResult, newValue);
                    break;

                case monitoringFunctionEnum.final:
                    output = newValue;
                    break;
            }
            newResult = output;
            storedResult = output;

            if (saveToProperty)
            {
                instance.imbSetPropertySafe(pi_toStore, storedResult, true);
            }

            return output;
        }

        /// <summary>
        ///
        /// </summary>
        public int newValue { get; set; }

        /// <summary> Last value</summary>
        public int storedValue { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public int storedResult { get; set; } = 0;

        /// <summary>
        ///
        /// </summary>
        public int newResult { get; set; }

        /// <summary> </summary>
        public PropertyInfo pi_toWatch { get; protected set; }

        /// <summary> </summary>
        public PropertyInfo pi_toStore { get; protected set; }

        /// <summary> </summary>
        public monitoringFunctionEnum function { get; protected set; } = monitoringFunctionEnum.none;
    }
}