// --------------------------------------------------------------------------------------------------------------------
// <copyright file="objectTable.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.tables
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Data;
    using imbSCI.Data.collection.nested;
    using imbSCI.Data.enums;
    using imbSCI.DataComplex.exceptions;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;

    /// <summary>
    /// Applied object table
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    /// <seealso cref="objectTableBase" />
    /// <seealso cref="System.Collections.Generic.ICollection{T}" />
    public class objectTable<T> : objectTableBase, ICollection<T> where T : class, new()
    {
        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        protected objectTable()
        {
        }

        /// <summary>
        /// New object table with unspecified filepath
        /// </summary>
        /// <param name="__keyProperty">The key property.</param>
        /// <param name="__tableName">Name of the table.</param>
        public objectTable(string __keyProperty, string __tableName)
        {
            name = __tableName;
            prepare(typeof(T), __keyProperty, __tableName, true);
        }

        /// <summary>
        /// Loads the <see cref="objectTable{T}"/> from <c>__filePath</c> specified. Filename is set as tablename. In this scenario the primary key must be set in the T class via imbAttribute <see cref="imbSCI.Core.attributes.imbAttributeName.collectionPrimaryKey"/>
        /// </summary>
        /// <param name="__filePath">The file path.</param>
        public objectTable(string __filePath, bool autoLoad, string __primaryKey = "", string __tableName = "")
        {
            if (__tableName.isNullOrEmpty()) __tableName = Path.GetFileNameWithoutExtension(__filePath);
            __filePath = imbSciStringExtensions.ensureEndsWith(__filePath, ".xml");

            name = __tableName;

            prepare(typeof(T), __primaryKey, name, !autoLoad);

            if (primaryKeyName.isNullOrEmpty())
            {
                throw new dataException("No primary key was declared in type: " + type.Name + " by valid imbAttributeName.collectionPrimaryKey", null, this, "No imbAttributeName.collectionPrimaryKey found in " + type.Name);
            }

            if (autoLoad)
            {
                Load(__filePath);
            }
            else
            {
                SaveAs(__filePath, getWritableFileMode.autoRenameThis);
            }
        }

        /// <summary>
        /// Index of the item
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            return base.IndexOf(item);
        }

        /// <summary>
        /// Gets the instance under keyValue or create new instance if not existing
        /// </summary>
        /// <param name="keyValue">The key value.</param>
        /// <returns></returns>
        public virtual T GetOrCreate(string keyValue)
        {
            T output = base.GetOrCreate(keyValue) as T;
            return output;
        }

        /// <summary>
        /// Returns objects groupped by the same value of the specified column
        /// </summary>
        /// <typeparam name="TGroup">Value type of the column</typeparam>
        /// <param name="column">The column to use for groups</param>
        /// <param name="expression">Optional row filter expression</param>
        /// <returns></returns>
        public aceDictionarySet<TGroup, T> GetGroups<TGroup>(string column, string expression = "")
        {
            if (!table.Columns.Contains(column)) throw new ArgumentException("There is no property [" + column + "] in the table row type [" + type.Name + "]");

            aceDictionarySet<TGroup, T> output = new aceDictionarySet<TGroup, T>();

            List<T> list = new List<T>();
            DataRow[] rows = null;
            if (expression.isNullOrEmpty())
            {
                rows = table.Select();
            }
            else
            {
                rows = table.Select(expression);
            }

            foreach (DataRow dr in rows)
            {
                TGroup key = (TGroup)dr[column];
                output.Add(key, (T)GetObjectFromRow(dr));
            }
            return output;
        }

        public List<T> GetWhere(string expression, int limit = -1, string sortColumn = "", objectTableSortEnum sortType = objectTableSortEnum.none)
        {
            List<T> output = new List<T>();
            var items = base.GetWhere(expression, limit, sortColumn, sortType);
            foreach (object item in items)
            {
                output.Add((T)item);
            }
            return output;
        }

        /// <summary>
        /// Gets the first entry that meets the criteria from the expression
        /// </summary>
        /// <param name="expression">The expression like: "LastName = 'Jones'",  "Price = 50.00"</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortType">Type of the sort.</param>
        /// <returns></returns>
        public T GetFirstWhere(string expression, string sortColumn = "", objectTableSortEnum sortType = objectTableSortEnum.none)
        {
            T output = default(T);
            var items = base.GetWhere(expression, 1, sortColumn, sortType);
            foreach (object item in items)
            {
                output = (T)item;
                break;
            }
            return output;
        }

#pragma warning disable CS1723 // XML comment has cref attribute 'T' that refers to a type parameter
#pragma warning disable CS1723 // XML comment has cref attribute 'T' that refers to a type parameter
        /// <summary>
        /// Gets or creates <see cref="T"/>, alias of <see cref="GetOrCreate(string)"/> call
        /// </summary>
        /// <value>
        /// The <see cref="T"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T this[string key]
#pragma warning restore CS1723 // XML comment has cref attribute 'T' that refers to a type parameter
#pragma warning restore CS1723 // XML comment has cref attribute 'T' that refers to a type parameter
        {
            get
            {
                return GetOrCreate(key);
            }
            //set
            //{
            //    AddOrUpdate(value, objectTableUpdatePolicy.overwrite);
            //}
        }

        /// <summary>
        /// Gets the last n or less entries.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public List<T> GetLastNEntries(int count)
        {
            List<T> output = new List<T>();
            int ct = table.Rows.Count;
            int c = Math.Min(count, ct);

            List<DataRow> rows = new List<DataRow>();
            for (int i = ct - c; i < ct; i++)
            {
                rows.Add(table.Rows[i]);
            }
            var objs = GetObjectFromRows(rows);
            objs.ForEach(x => output.Add(x as T));
            return output;
        }

        /// <summary>
        /// Gets the last entry: last entry added
        /// </summary>
        /// <returns></returns>
        public T GetLastEntry()
        {
            int lastIndex = table.Rows.Count - 1;
            if (lastIndex < 0) return null;

            DataRow rw = table.Rows[table.Rows.Count - 1];
            return GetObjectFromRow(rw) as T;
        }

        public T GetLastEntryTouched()
        {
            return lastEntry as T;
        }

        /// <summary>
        /// Adds object or update if existing row was found. Returns true if new row was created
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        public bool AddOrUpdate(T input, objectTableUpdatePolicy policy = objectTableUpdatePolicy.overwrite)
        {
            return base.AddOrUpdate(input, policy);
        }

        /// <summary>
        /// Adds the or updates data table with objects specified. Returns number of new items added into collection
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        public int AddOrUpdate(IEnumerable<T> input, objectTableUpdatePolicy policy = objectTableUpdatePolicy.overwrite)
        {
            return base.AddOrUpdate(input, policy);
        }

        public void Add(T item)
        {
            base.AddOrUpdate(item, objectTableUpdatePolicy.overwrite);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            return ContainsObject(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            int i = 0;
            foreach (T item in array)
            {
                if (i >= arrayIndex)
                {
                    AddOrUpdate(item, objectTableUpdatePolicy.overwrite);
                }
                i++;
            }
        }

        public int Remove(IEnumerable<T> items)
        {
            int i = 0;
            foreach (T it in items)
            {
                if (base.Remove(it)) i++;
            }
            return i;
        }

        public bool Remove(T item)
        {
            return base.Remove(item);
        }

        public bool Remove(string keyValue)
        {
            object item = GetPrimaryKeySelect(keyValue);
            return base.Remove(item);
        }

        /// <summary>
        /// Gets the list of object
        /// </summary>
        /// <returns></returns>
        public List<T> GetList()
        {
            List<T> output = new List<T>();
            var list = GetObjectList();
            foreach (object obj in list)
            {
                T item = obj as T;
                if (item != null)
                {
                    output.Add(item);
                }
            }
            return output;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetList().GetEnumerator();
        }
    }
}