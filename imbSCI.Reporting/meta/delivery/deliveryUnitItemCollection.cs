// --------------------------------------------------------------------------------------------------------------------
// <copyright file="deliveryUnitItemCollection.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.meta.delivery
{
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Data.enums;
    using imbSCI.Reporting.meta.delivery.items;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Collection of <see cref="deliveryUnitItem"/>s associated to <see cref="deliveryUnit"/> definition
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{imbSCI.Reporting.meta.delivery.deliveryUnitItem}" />
    /// <seealso cref="IDeliveryUnitItem"/>
    public class deliveryUnitItemCollection : IEnumerable<IDeliveryUnitItem>
    {
        /// <summary>
        /// Determines whether the collection contains item with the same sourcefile path.
        /// </summary>
        /// <param name="sourceFileInfo">The source file information.</param>
        /// <returns>
        ///   <c>true</c> if [contains item from sourcefile] [the specified source file information]; otherwise, <c>false</c>.
        /// </returns>
        public bool containsItemFromSourcefile(FileInfo sourceFileInfo)
        {
            foreach (IDeliveryUnitItem item in this)
            {
                if (item is IDeliverySupportFile)
                {
                    IDeliverySupportFile item_IDeliverySupportFile = (IDeliverySupportFile)item;
                    if (item_IDeliverySupportFile.sourceFileInfo.FullName == sourceFileInfo.FullName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Adds the criteria.
        /// </summary>
        /// <param name="opera">The opera.</param>
        /// <param name="pathMatchRule">The path match rule.</param>
        /// <param name="pathCriteria">The path criteria.</param>
        /// <param name="metaElementTypeToMatch">The meta element type to match.</param>
        /// <param name="level">The level.</param>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public metaContentCriteriaTrigger AddCriteria(metaContentTriggerOperator opera, metaModelTargetEnum pathMatchRule = metaModelTargetEnum.scope, string pathCriteria = null, Type metaElementTypeToMatch = null, reportElementLevel level = reportElementLevel.none, IMetaContentNested element = null) => trigs.AddCriteria(opera, pathMatchRule, pathCriteria, metaElementTypeToMatch, level);

        /// <summary>
        /// Adds the units to be executed if evaluation passes
        /// </summary>
        /// <param name="unit">The unit.</param>
        public void Add(IDeliveryUnitItem unit)
        {
            if (!items.Contains(unit)) items.Add(unit);
        }

        /// <summary>
        /// assigned triggers
        /// </summary>
        protected metaContentCriteriaTriggerCollection trigs { get; set; } = new metaContentCriteriaTriggerCollection();

        /// <summary>
        /// assigned units
        /// </summary>
        protected List<IDeliveryUnitItem> items { get; set; } = new List<IDeliveryUnitItem>();

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IDeliveryUnitItem> GetEnumerator()
        {
            return ((IEnumerable<IDeliveryUnitItem>)items).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<IDeliveryUnitItem>)items).GetEnumerator();
        }
    }
}