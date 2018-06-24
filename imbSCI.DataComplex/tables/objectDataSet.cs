// --------------------------------------------------------------------------------------------------------------------
// <copyright file="objectDataSet.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.files.folders;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// DataSet of object tables
    /// </summary>
    /// <typeparam name="TTable">The type of the table.</typeparam>
    /// <typeparam name="T"></typeparam>
    public class objectDataSet<TTable, T> : IEnumerable<TTable>
        where TTable : objectTable<T>, new()
        where T : class, new()
    {
        protected folderNode fileFolder { get; set; }

        public objectDataSet()
        {
            name = GetType().Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="objectDataSet{TTable, T}"/> class.
        /// </summary>
        /// <param name="__dataSetName">Name of the data set.</param>
        /// <param name="__dataSetDescription">The data set description.</param>
        public objectDataSet(string __dataSetName, string __dataSetDescription, folderNode __fileFolder = null)
        {
            name = __dataSetName;
            description = __dataSetDescription;
            fileFolder = __fileFolder;
        }

        private object getDataSetLock = new object();

        /// <summary>
        /// Gets the data set.
        /// </summary>
        /// <returns></returns>
        public DataSet GetDataSet()
        {
            DataSet output = new DataSet(name);

            lock (getDataSetLock)
            {
                output.SetDesc(description);

                foreach (TTable item in objectTables.Values)
                {
                    item.GetDataTable(output);
                }
            }
            return output;
        }

        /// <summary>
        /// Gets the new objectTable
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public TTable GetNew(string key)
        {
            TTable output = new TTable();

            int c = 0;
            string tn = key;
            string tnn = tn;
            while (objectTables.Keys.Contains(tn))
            {
                tn = tnn + c.ToString("D2");
                c++;
            }

            output.name = key;

            objectTables.TryAdd(key, output);
            return output;
        }

        public IEnumerator<TTable> GetEnumerator()
        {
            return objectTables.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return objectTables.Values.GetEnumerator();
        }

        public TTable this[string key]
        {
            get
            {
                if (!objectTables.ContainsKey(key))
                {
                    TTable tmp = new TTable();
                    tmp.name = key;
                    objectTables.TryAdd(key, tmp);
                }

                TTable output = null;

                objectTables.TryGetValue(key, out output);

                return output;
            }
            set
            {
                if (!objectTables.ContainsKey(key))
                {
                    objectTables.TryAdd(key, value);
                }
                else
                {
                    objectTables[key] = value;
                }
            }
        }

        /// <summary>
        /// Name for this instance
        /// </summary>
        public string name { get; set; } = "";

        /// <summary>
        /// Human-readable description of object instance
        /// </summary>
        public string description { get; set; } = "";

        /// <summary> </summary>
        public ConcurrentDictionary<string, TTable> objectTables { get; protected set; } = new ConcurrentDictionary<string, TTable>();
    }
}