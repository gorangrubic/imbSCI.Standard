// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataUnitSeries.cs" company="imbVeles" >
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
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Base data unit for time/ordinal series representation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class dataUnitSeries<T, TInstance> : dataUnitBase, IdataUnitSeries where T : dataUnitRow<TInstance>, IDataUnitSeriesEntry, new()
        where TInstance : class
    {
        /// <summary> </summary>
        protected List<T> items
        {
            get
            {
                return _items as List<T>;
            }
            set
            {
                _items = value;
            }
        }

        /// <summary>
        /// Gets the current entry - if exists
        /// </summary>
        /// <value>
        /// The current entry.
        /// </value>
        public IDataUnitSeriesEntry currentEntry
        {
            get
            {
                return items.Last();
            }
        }

        public IDataUnitSeriesEntry GetFirstEntry()
        {
            if (items.Count < 1) return null;
            return items[0];
        }

        /// <summary>
        /// Gets the last entry - if there are less then two entries it will return null
        /// </summary>
        /// <value>
        /// The last entry.
        /// </value>
        public IDataUnitSeriesEntry lastEntry
        {
            get
            {
                if (items.Count < 2) return null;
                return items[items.Count() - 2];
            }
        }

        public IDataUnitSeriesEntry lastDataPair
        {
            get
            {
                if (items.Count > 1)
                {
                    return this[items.Count() - 2];
                }
                return null;
            }
        }

        public T CreateEntry(TInstance source = null, int iSync = 0)
        {
            T entry = null;
            int iter = 0;
            do
            {
                iter = items.Count();

                entry = new T();

                items.Add(entry);
                entry.iteration = iter;
                entry.prepare(map.monitor, this);

                if (source != null)
                {
                    entry.setData(source);
                }
                else
                {
                }
                map.monitor.unlock();
            } while (iter < iSync);

            return entry;
        }

        public List<T> GetData()
        {
            return items.ToList();
        }

        public DataTable GetTableWith(dataUnitPresenter presenter, bool isPreview = false)
        {
            return buildCustomDataTable(items, presenter, isPreview);
        }

#pragma warning disable CS1723 // XML comment has cref attribute 'T' that refers to a type parameter
#pragma warning disable CS1723 // XML comment has cref attribute 'T' that refers to a type parameter
        /// <summary>
        /// Gets the <see cref="T"/> with the specified iteration.
        /// </summary>
        /// <value>
        /// The <see cref="T"/>.
        /// </value>
        /// <param name="iteration">The iteration.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">iteration - Larger then timeseries</exception>
        public T this[int iteration]
#pragma warning restore CS1723 // XML comment has cref attribute 'T' that refers to a type parameter
#pragma warning restore CS1723 // XML comment has cref attribute 'T' that refers to a type parameter
        {
            get
            {
                if (items.Count() < iteration) throw new ArgumentOutOfRangeException("iteration", "Larger then timeseries");
                return items[iteration];
            }
        }

        public dataUnitSeries(dataDeliveryAcquireEnum acquire, int __length) : base(typeof(TInstance))
        {
            dataAcquireFlags = acquire;

            items = new List<T>();
        }
    }
}