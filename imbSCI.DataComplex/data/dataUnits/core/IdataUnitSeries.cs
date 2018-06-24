// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdataUnitSeries.cs" company="imbVeles" >
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
    using System.Collections;
    using System.ComponentModel;
    using System.Data;

    public interface IdataUnitSeries
    {
        IDataUnitSeriesEntry lastDataPair { get; }

        /// <complete>Defines Table that is showint all properties having "complete" in Category description</complete>
        dataUnitPresenter complete_Table { get; }

        /// <summary> </summary>
        dataUnitMap map { get; }

        /// <summary> </summary>
        int lastIteration { get; }

        DataTable GetTableWith(dataUnitPresenter presenter, bool isPreview = false);

        void dataImportComplete();

        ////  IDataUnitSeriesEntry lastDataPair { get; }

        /// <summary>
        /// Builds the custom data table.
        /// </summary>
        /// <param name="instance_items">The instance items.</param>
        /// <param name="presenter">The presenter.</param>
        /// <param name="isPreviewTable">if set to <c>true</c> [is preview table].</param>
        /// <returns></returns>
        DataTable buildCustomDataTable(IEnumerable instance_items, dataUnitPresenter presenter, bool isPreviewTable);

        /// <summary>
        /// Builds the data table shema.
        /// </summary>
        /// <param name="presenter">The presenter.</param>
        /// <returns></returns>
        DataTable buildDataTableShema(dataUnitPresenter presenter = null);

        event PropertyChangedEventHandler PropertyChanged;

        IDataUnitSeriesEntry lastEntry { get; }
        IDataUnitSeriesEntry currentEntry { get; }
    }
}