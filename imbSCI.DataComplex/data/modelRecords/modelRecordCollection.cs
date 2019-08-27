// --------------------------------------------------------------------------------------------------------------------
// <copyright file="modelRecordCollection.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.data.modelRecords
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Data;
    using imbSCI.Data.data;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex.exceptions;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

#pragma warning disable CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    /// <summary>
    /// Dictionary of <see cref="IModelRecord"/> records, indexed by key of any type <see cref="object"/>
    /// </summary>
    /// <typeparam name="T">Any object - String preferably</typeparam>
    /// <typeparam name="TRecord">The type of the record.</typeparam>
    /// <seealso cref="aceCommonTypes.primitives.imbBindable" />
    /// <seealso cref="System.Collections.Generic.IEnumerable{T}" />
    public abstract class modelRecordCollection<T, TRecord> : imbBindable, IEnumerable<KeyValuePair<T, TRecord>>
#pragma warning restore CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
        where TRecord : modelRecordBase, IModelRecord
    {
        /// <summary>
        /// TRUE: it will throw exceptions on iregular condition
        /// </summary>
        public const bool DO_EXCEPTIONS = true;

        protected modelRecordCollection()
        {
        }

        protected modelRecordCollection(string __testRunStamp, params T[] keys)
        {
            testRunStamp = __testRunStamp;
            foreach (T key in keys)
            {
                Add(key);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected string testRunStamp { get; set; } = "";

        /// <summary>
        /// Gets test (global) level of <see cref="IModelRecord"/> for the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="IModelRecord"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public TRecord this[T key]
        {
            get
            {
                return recordsForTest[key];
            }
        }

        public TRecord this[int key]
        {
            get
            {
                return records[key];
            }
        }

        public bool Remove(TRecord record)
        {
            var instance = GetInstance(record);

            RemoveRecord(record);
            return RemoveInstance(instance);
        }

        public bool RemoveInstance(T instance)
        {
            return items.Remove(instance);
        }

        public bool RemoveRecord(TRecord record)
        {
            return records.Remove(record);
        }

        public virtual TRecord GetRecord(int i)
        {
            if (i >= records.Count) throw new dataException("GetRecord failed i:[" + i + "] c:[" + records.Count + "]", null, this, "Out of range");
            return records[i];
        }

#pragma warning disable CS1574 // XML comment has cref attribute 'dataException' that could not be resolved
        /// <summary>
        /// Gets the record using one of supplied arguments.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="autoCreateOnMissing">if set to <c>true</c> [automatic create on missing].</param>
        /// <returns>Record associated with the key</returns>
        /// <exception cref="aceCommonTypes.core.exceptions.dataException">No valid arguments supplied: autoCreateOnMissing[" + autoCreateOnMissing + "] but " + nameof(key) + " not found.</exception>
        public virtual TRecord GetRecord(T key, bool autoCreateOnMissing = false)
#pragma warning restore CS1574 // XML comment has cref attribute 'dataException' that could not be resolved
        {
            if (!key.isNullOrEmptyString())
            {
                if (!recordsForTest.ContainsKey(key))
                {
                    if (autoCreateOnMissing)
                    {
                        TRecord item = Add(key);
                        return item;
                    }
                }
                else
                {
                    return recordsForTest[key];
                }
            }

            if (DO_EXCEPTIONS) throw new dataException("No valid arguments supplied: autoCreateOnMissing[" + autoCreateOnMissing + "] but " + nameof(key) + " not found.");

            return null;
        }

#pragma warning disable CS1574 // XML comment has cref attribute 'dataException' that could not be resolved
        /// <summary>
        /// Seaches for record using supplied <c>key</c> or <c>instanceID</c>
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="instanceID">Instance identifier - it will be tested against key's <see cref="IObjectWithName.name"/>, <see cref="Type"/> name and against <see cref="IModelRecord.instanceID"/>, <see cref="IModelRecord.UID"/> </param>
        /// <returns></returns>
        /// <exception cref="aceCommonTypes.core.exceptions.dataException">No valid arguments supplied. At least one argument must be valid [null or empty string] to find the record in the collection</exception>
        public virtual TRecord SeachForRecord(T key, string instanceID = "")
#pragma warning restore CS1574 // XML comment has cref attribute 'dataException' that could not be resolved
        {
            if (key.isNullOrEmpty()) return this[key];
            if (!instanceID.isNullOrEmpty())
            {
                foreach (KeyValuePair<T, TRecord> kpair in this)
                {
                    if (kpair.Value.instanceID == instanceID) return kpair.Value;
                    if (kpair.Value.UID == instanceID) return kpair.Value;
                }
            }

            if (DO_EXCEPTIONS) throw new dataException("No valid arguments supplied. At least one argument must be valid [null or empty string] to find the record in the collection");

            return null;
        }

        /// <summary>
        /// Gets or sets the records for test.
        /// </summary>
        /// <value>
        /// The records for test.
        /// </value>
        protected Dictionary<T, TRecord> recordsForTest { get; set; } = new Dictionary<T, TRecord>();

        private List<TRecord> _records = new List<TRecord>();

        /// <summary> </summary>
        protected List<TRecord> records
        {
            get
            {
                return _records;
            }
            set
            {
                _records = value;
                OnPropertyChanged("records");
            }
        }

        private List<T> _items = new List<T>();

        /// <summary> </summary>
        protected List<T> items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                OnPropertyChanged("items");
            }
        }

        private object GetInstanceLock = new object();

        /// <summary>
        /// Gets the instance of the record supplied
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns></returns>
        public virtual T GetInstance(TRecord record)
        {
            var kList = recordsForTest.Keys.ToList();

            for (int i = 0; i < kList.Count; i++)
            {
                var kValue = recordsForTest[kList[i]];
                if (kValue == record) return kList[i];
                //KeyValuePair<T, TRecord> kpair = recordsForTest.ElementAt(i);
            }

            return default(T);
        }

        /// <summary>
        /// Creating a record instance based on the instance.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected abstract TRecord CreateRecord(T item);

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">key instance is already part of collection - item</exception>
        public virtual TRecord Add(T item)
        {
            if (items.Contains(item))
            {
                if (DO_EXCEPTIONS) throw new ArgumentException("key instance is already part of collection", "item");
            }
            //imbTypeInfo iTI = item.getTypology();

            TRecord record = CreateRecord(item);
            //record.initiate(item.name + item.language.iso2Code + iTI.getTypeSignature(), testRunStamp);

            recordsForTest.Add(item, record);
            records.Add(record);
            items.Add(item);
            return record;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection of <see cref="KeyValuePair{T, TRecord}"/>
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<KeyValuePair<T, TRecord>> IEnumerable<KeyValuePair<T, TRecord>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<T, TRecord>>)recordsForTest).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection of <see cref="KeyValuePair{T, TRecord}"/>
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<T, TRecord>>)recordsForTest).GetEnumerator();
        }
    }
}