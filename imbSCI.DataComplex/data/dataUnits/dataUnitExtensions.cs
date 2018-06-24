// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataUnitExtensions.cs" company="imbVeles" >
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

/// <summary>
/// DataUnits are output sockets mainly designed to work with <see cref="modelRecordBase"/> instances and collections
/// </summary>
namespace imbSCI.DataComplex.data.dataUnits
{
    using imbSCI.Core.attributes;
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Data;
    using imbSCI.DataComplex.data.dataUnits.core;
    using imbSCI.DataComplex.data.dataUnits.enums;
    using System;

    public static class dataUnitExtensions
    {
        public static int getPreviewRowLimit(this dataDeliveryAcquireEnum acquire)
        {
            int output = 0;

            var list = acquire.getEnumListFromFlags<dataDeliveryAcquireEnum>();
            foreach (dataDeliveryAcquireEnum li in list)
            {
                switch (li)
                {
                    case dataDeliveryAcquireEnum.collectionLimitShowCase10:
                        output += 10;
                        break;

                    case dataDeliveryAcquireEnum.collectionLimitShowCase25:
                        output += 25;
                        break;
                }
            }

            return output;
        }

        /// <summary>
        /// Gets the monitoring function.
        /// </summary>
        /// <param name="propertyEntry">The property entry.</param>
        /// <param name="instanceType">Type of the instance.</param>
        /// <returns>null if function not found or set to none</returns>
        public static dataUnitRowMonitoringDefinition getMonitoringFunction(this settingsPropertyEntryWithContext propertyEntry, Type instanceType)
        {
            if (!propertyEntry.attributes.ContainsKey(imbAttributeName.reporting_function)) return null;

            monitoringFunctionEnum function = propertyEntry.attributes.getProperEnum<monitoringFunctionEnum>(monitoringFunctionEnum.none, imbAttributeName.reporting_function);
            if (function == monitoringFunctionEnum.none) return null;
            string toWatch = propertyEntry.attributes.getProperString(imbAttributeName.measure_operand);
            var wpi = instanceType.GetProperty(toWatch);
            dataUnitRowMonitoringDefinition output = new dataUnitRowMonitoringDefinition(wpi, propertyEntry.pi, function);

            return output;
        }
    }
}