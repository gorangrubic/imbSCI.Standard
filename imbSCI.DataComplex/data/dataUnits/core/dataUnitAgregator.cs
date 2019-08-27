// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataUnitAgregator.cs" company="imbVeles" >
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
    using imbSCI.DataComplex.data.dataUnits.enums;

#pragma warning disable CS1574 // XML comment has cref attribute 'DataTable' that could not be resolved
#pragma warning disable CS1574 // XML comment has cref attribute 'dataUnitBase' that could not be resolved
    /// <summary>
    /// The agregator data units have ability to process an external <see cref="DataTable"/> and to populate their fields according to <see cref="imbSCI.Core.attributes.imbAttributeName.reporting_agregate_function"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="imbReportingCore.data.dataUnits.core.dataUnitBase" />
    public class dataUnitAgregator<T> : dataUnitBase
#pragma warning restore CS1574 // XML comment has cref attribute 'dataUnitBase' that could not be resolved
#pragma warning restore CS1574 // XML comment has cref attribute 'DataTable' that could not be resolved
    {
        public dataUnitAgregator() : base(typeof(T))
        {
        }

        private dataUnitPresenter _complete_Table;

        /// <complete>Defines Table that is showint all properties having "complete" in Category description</complete>
        public override dataUnitPresenter complete_Table
        {
            get
            {
                if (_complete_Table == null)
                {
                    _complete_Table = new dataUnitPresenter("complete", "Complete comparative table", "Comparative table between ");
                    _complete_Table.setFlags(
                        dataDeliveryPresenterTypeEnum.tableVertical,
                        dataDeliverFormatEnum.includeAttachment | dataDeliverFormatEnum.globalAttachment,
                        dataDeliverAttachmentEnum.attachCSV | dataDeliverAttachmentEnum.attachExcel | dataDeliverAttachmentEnum.attachJSON);
                    presenters[nameof(complete_Table)] = _complete_Table;
                }
                return _complete_Table;
            }
        }
    }
}