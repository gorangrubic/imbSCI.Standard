// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataUnitRowMonitoring.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.data;
    using imbSCI.Data;
    using imbSCI.DataComplex.special;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Monitors for change in the associated Row
    /// </summary>
    /// <seealso cref="imbSCI.DataComplex.special.instanceCountCollection{T}.String}" />
    public class dataUnitRowMonitoring : instanceCountCollection<string>
    {
        /// <summary>
        /// Checks if data were received on every property
        /// </summary>
        /// <returns>
        /// TRUE if data input ok, otherwise FALSE/EXCEPTION
        /// </returns>
        public bool checkData(IDataUnitRow instance)
        {
            string msg = "";
            foreach (var item in instance.parent.map.columns)
            {
                string mn = item.pi.Name;
                if (!Contains(mn))
                {
                    if (msg.isNullOrEmpty()) msg = "dataUnitRow < " + instance.GetType().Name + " > received no input on:";
                    msg = msg.add(mn, ", ");
                }
            }
            if (msg.isNullOrEmpty())
            {
                Clear();
                runFunctions(this);
                return true;
            }
            else
            {
                throw new ArgumentException("Uncomplete data for dataUnitRow", nameof(instance));
            }
            return false;
        }

        /// <summary>
        /// Prepares this instance.
        /// </summary>
        public void prepare()
        {
            foreach (KeyValuePair<string, dataUnitRowMonitoringDefinition> pair in targets)
            {
                pair.Value.prepare();
            }
            unlock();
        }

        /// <summary>
        /// Gets or sets the targets.
        /// </summary>
        /// <value>
        /// The targets.
        /// </value>
        public Dictionary<string, dataUnitRowMonitoringDefinition> targets { get; protected set; } = new Dictionary<string, dataUnitRowMonitoringDefinition>();

        /// <summary>
        /// Runs the functions.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void runFunctions(object instance)
        {
            foreach (var pair in targets)
            {
                pair.Value.readValueAndResolveInt32(instance);
            }
        }

        /// <summary>
        /// Unlocks monitoring for data overrite
        /// </summary>
        internal void unlock()
        {
            Clear();
        }
    }
}