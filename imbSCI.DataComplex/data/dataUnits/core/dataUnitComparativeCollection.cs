// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataUnitComparativeCollection.cs" company="imbVeles" >
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
    using imbSCI.Core.collection;
    using imbSCI.DataComplex.data.dataUnits.enums;
    using System;
    using System.Collections.Generic;

    ///// <summary>
    ///// Base data unit for collection representation, ordered optionally
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //public abstract class dataUnitCollection<T>:dataUnitBase
    //{
    //    public dataUnitCollection(T instance):base(typeof(T))
    //    {
    //    }

    //}

    public class dataUnitComparativeCollection<T, TCrossItem> : dataUnitBase
    {
        /// <summary>
        /// Instaces of items to cross table with
        /// </summary>
        public List<TCrossItem> crossItems { get; set; }

        /// <summary>
        ///
        /// </summary>
        public PropertyEntryDictionary crossItemColumns { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Type crossItemType { get; set; }

        public dataUnitComparativeCollection() : base(typeof(T))
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
                    _complete_Table = new dataUnitPresenter("complete", "Complete comparative table", "Comparative table between " + typeof(T).Name + " and " + typeof(TCrossItem));
                    _complete_Table.setFlags(
                        dataDeliveryPresenterTypeEnum.tableHorizontal,
                        dataDeliverFormatEnum.includeAttachment | dataDeliverFormatEnum.globalAttachment,
                        dataDeliverAttachmentEnum.attachCSV | dataDeliverAttachmentEnum.attachExcel | dataDeliverAttachmentEnum.attachJSON);
                    presenters[nameof(complete_Table)] = _complete_Table;
                }
                return _complete_Table;
            }
        }
    }
}