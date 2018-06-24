// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataUnitIntegratedModel.cs" company="imbVeles" >
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
    using System;
    using System.Collections;

    /// <summary>
    /// Integrated data deliveryInstance model keeping deliveryInstance unit and data in the same object
    /// </summary>
    /// <seealso cref="dataUnitBase" />
    /// <seealso cref="IDataUnitRow" />
    public abstract class dataUnitIntegratedModel : dataUnitBase, IDataUnitRow
    {
        public dataUnitIntegratedModel(Type __instanceType) : base(__instanceType)
        {
        }

        public int iteration => lastIteration;

        /// <summary>
        ///
        /// </summary>
        public dataUnitBase parent { get; set; }

        /// <summary>
        /// Checks the data.
        /// </summary>
        /// <param name="monitor">The monitor.</param>
        /// <returns></returns>
        public bool checkData(dataUnitRowMonitoring monitor)
        {
            return monitor.checkData(this);
        }

        /// <summary>
        /// Prepares the specified monitor.
        /// </summary>
        /// <param name="__monitor">The monitor.</param>
        /// <param name="__parent">The parent.</param>
        public void prepare(dataUnitRowMonitoring __monitor, dataUnitBase __parent = null)
        {
        }

        /// <summary>
        /// Sets the agregate result.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void setAgregateResult(IEnumerable source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the data row.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void setDataRow(object source)
        {
            throw new NotImplementedException();
        }
    }
}