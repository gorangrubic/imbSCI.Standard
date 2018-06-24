// --------------------------------------------------------------------------------------------------------------------
// <copyright file="objectTableBase.cs" company="imbVeles" >
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
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.files;
    using imbSCI.Core.files.folders;
    using imbSCI.Core.math.aggregation;
    using imbSCI.Core.reporting;
    using imbSCI.Data;
    using imbSCI.Data.data;
    using imbSCI.Data.enums;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex.exceptions;
    using imbSCI.DataComplex.extensions.data.operations;
    using imbSCI.DataComplex.extensions.data.schema;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Base class for typed DataTable collection
    /// </summary>
    /// <seealso cref="imbSCI.Data.data.changeBindableBase" />
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithName" />
    public abstract class objectTableBase : changeBindableBase, IObjectWithName, IObjectWithNameAndDescription
    {
        protected objectTableBase()
        {
        }

        private string _description;

        /// <summary> </summary>
        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged("description");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether it already wrote warning when someone tried to write new data
        /// </summary>
        /// <value>
        /// <c>true</c> if [read only mode warning issued]; otherwise, <c>false</c>.
        /// </value>
        protected bool ReadOnlyModeWarningIssued { get; set; } = false;

        /// <summary>
        /// If true it prevents updating the table but increases performances drastically and provides full thread-safety
        /// </summary>
        /// <value>
        ///   <c>true</c> if [read only mode]; otherwise, <c>false</c>.
        /// </value>
        public bool ReadOnlyMode { get; set; } = false;

        protected bool WriteOnlyModeWarningIssued { get; set; } = false;

        /// <summary>
        /// If true avoids existing item check procedure - drastically increases performance
        /// </summary>
        /// <value>
        ///   <c>true</c> if [write only mode]; otherwise, <c>false</c>.
        /// </value>
        public bool WriteOnlyMode { get; set; } = false;

        protected bool ReadOnlyEnforce()
        {
            if (ReadOnlyMode)
            {
                if (!ReadOnlyModeWarningIssued)
                {
#if DEBUG
                    Console.WriteLine("READ-ONLY MODE [" + name + "] (" + GetType().Name + ") --- MODIFICATION OF THE TABLE IS DISABLED");
#endif
                    ReadOnlyModeWarningIssued = true;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        protected bool WriteOnlyEnforce()
        {
            if (WriteOnlyMode)
            {
                if (!WriteOnlyModeWarningIssued)
                {
#if DEBUG
                    Console.WriteLine("WRITE-ONLY MODE [" + name + "] (" + GetType().Name + ") --- UPDATE on AddOrUpdate and CREATE on GetOrCreate are disabled");
#endif
                    WriteOnlyModeWarningIssued = true;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the copy of DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataTable(DataSet dataset = null, string tableName = "")
        {
            DataTable output = table.GetClonedShema<DataTable>();

            output.SetClassName(type.Name);
            output.SetClassType(type);

            if (tableName.isNullOrEmpty()) tableName = name;

            output.SetTitle(tableName);
            output.SetDescription(description);

            output.SetCategoryPriority(settings.CategoryByPriority);
            if (Enumerable.Count<string>(settings.CategoryByPriority) > 0)
            {
            }
            output.SetAdditionalInfo(table.GetAdditionalInfo());

            output.SetAggregationAspect(dataPointAggregationAspect.none);

            output.CopyRowsFrom(table);

            if (dataset != null)
            {
                dataset.AddTable(table);
            }
            return output;
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get
            {
                return table.Rows.Count;
            }
        }

        private object ContainsKeyLock = new object();

        private ConcurrentBag<string> _keyCache = new ConcurrentBag<string>();

        /// <summary> </summary>
        protected ConcurrentBag<string> keyCache
        {
            get
            {
                return _keyCache;
            }
            set
            {
                _keyCache = value;
                OnPropertyChanged("keyCache");
            }
        }

        /// <summary>
        /// Determines whether the specified key is contained
        /// </summary>
        /// <param name="keyValue">The key value.</param>
        /// <returns>
        ///   <c>true</c> if the specified key value contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(string keyValue)
        {
            DataRow row = null;
            if (keyCache.Contains(keyValue)) return true;

            //lock (ContainsKeyLock)
            //{
            if (UsePrimaryKey)
            {
                row = table.Rows.Find(keyValue);
            }
            else
            {
                string select_exp = GetPrimaryKeySelect(keyValue);
                row = tableSelect(select_exp).FirstOrDefault();

                //row = //table.Select(GetPrimaryKeySelect(keyValue)).FirstOrDefault();
            }
            //}
            if (row == null) return false;

            bool output = true; // row.Any();

            if (output) keyCache.Add(keyValue);

            return output;
        }

        /// <summary>
        /// Loads external <c>inputTable</c>
        /// </summary>
        /// <param name="inputTable">The input table.</param>
        /// <param name="loger">The loger.</param>
        /// <param name="policy">The policy.</param>
        /// <returns>True if any row was loaded</returns>
        public int Load(DataTable inputTable, ILogBuilder loger, objectTableUpdatePolicy policy = objectTableUpdatePolicy.overwrite)
        {
            if (Count > 0)
            {
                if (ReadOnlyEnforce()) return 0;
            }
            // inputTable = checkTableShema(inputTable);

            int i = 0;
            int c = 0;
            int imax = inputTable.Rows.Count / 10;

            bool headingRowFound = false;
            foreach (DataRow dr in inputTable.Rows)
            {
                bool skipThisRow = false;
                if (!headingRowFound)
                {
                    foreach (DataColumn dc in inputTable.Columns)
                    {
                        if (dr[dc.ColumnName].toStringSafe() == dc.ColumnName)
                        {
                            headingRowFound = true;
                            skipThisRow = true;
                            break;
                        }
                    }
                }

                if (dr[primaryKeyName].toStringSafe("") == "")
                {
                    skipThisRow = true;
                }
                else
                {
                }

                if (!skipThisRow)
                {
                    string keyValue = dr[primaryKeyName].toStringSafe("");

                    if (!keyCache.Contains(keyValue)) keyCache.Add(keyValue); //.toStringSafe("");

                    if (!keyValue.isNullOrEmpty())
                    {
                        object item = GetOrCreate(keyValue);

                        SetObjectByCustomRow(dr, item);

                        AddOrUpdate(item, policy);
                    }

                    c++;
                }

                if (i > imax)
                {
                    if (loger != null)
                    {
                        double ratio = (double)c / (double)inputTable.Rows.Count;
                        loger.log("External table loaded: " + ratio.ToString("P2"));
                        i = 0;
                    }
                }

                i++;
            }

            return c;
        }

        public Boolean LoadFailed { get; set; } = false;

        /// <summary>
        /// Loads the content from specified path
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public virtual bool Load(string path = "", bool autoBuildOnFail = true)
        {
            if (!path.isNullOrEmpty())
            {
                info = new FileInfo(path);
            }
            if (info == null)
            {
                throw new dataException("objectTableBase.Load() can't be called before [info]:FileInfo initiated.", null, this, GetType().Name + ".Load(\"\") - filepath never specified");
            }

            if (info.Exists) LoadFailed = false;

            if (!LoadFailed)
            {
                try
                {
                    DataTable in_table = objectSerialization.loadObjectFromXML(info.FullName, typeof(DataTable)) as DataTable;
                    table = checkTableShema(in_table);
                    return true;
                }
                catch (Exception ex)
                {
                    String message = "Table load failed [" + info.FullName + "] " + path + " ex: " + ex.Message + "   at  ObjectTable<" + type.FullName + "> failed to load " + path;
                    message = Environment.NewLine + ex.StackTrace;

                    Console.WriteLine(message);

                    var p = path.getWritableFile(getWritableFileMode.autoRenameExistingToBack);

                    folderNode fn = p.Directory;
                    String ef = fn.pathFor("error_" + name + ".txt", getWritableFileMode.autoRenameThis, "Error report on failed Load(" + path + ") attempt for [" + name + "]");

                    ef = ef.getWritableFile(getWritableFileMode.autoRenameExistingToBack).FullName;
                    message.saveStringToFile(ef, getWritableFileMode.autoRenameExistingToBack);

                    String d = p.Directory.FullName;

                    File.Delete(path);

                    //imbSCI.Core.screenOutputControl.logToConsoleControl.writeToConsole()

                    LoadFailed = true;
                }
            }

            if (LoadFailed)
            {
                if (autoBuildOnFail)
                {
                    buildTable();
                    return Save();
                }
            }
            return LoadFailed;
        }

        /// <summary>
        /// Saves the table on specified path. According to <see cref="aceCommonTypes.enums.getWritableFileMode"/> mode selected
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        public virtual bool SaveAs(string path, getWritableFileMode mode = getWritableFileMode.newOrExisting)
        {
            info = path.getWritableFile(mode);

            return Save();
        }

        /// <summary>
        /// Saves if changed. Returns <c>true</c> if there were changes
        /// </summary>
        /// <returns></returns>
        public bool SaveIfChanged()
        {
            if (HasChanges)
            {
                return Save();
            }
            return false;
        }

        private object SaveLinkedLock = new object();

        /// <summary>
        /// Updates the base.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="key">The key.</param>
        /// <exception cref="aceCommonTypes.core.exceptions.dataException">Instance [" + key + "] wasn't found in the instance collection - null - objectTableBase.UpdateBase() instance not found</exception>
        protected virtual void UpdateBase(object item, string key = null)
        {
            if (key.isNullOrEmpty()) key = GetKeyValue(item);

            Boolean _exists = false;
            DataRow[] rows = null;
            if (!WriteOnlyEnforce())
            {
                string select_exp = GetPrimaryKeySelect(key);
                rows = tableSelect(select_exp);
                _exists = rows.Any();
            }
            else
            {
                return;
            }

            //var rows = table.Select(GetPrimaryKeySelect(key));

            if (_exists)
            {
                SetRowWithObject(rows.First(), item, objectTableUpdatePolicy.overwrite);
                /*
                if (instanceRegistry.ContainsKey(key))
                {
                    instanceRegistry.TryUpdate(key, item as IObjectTableEntry, item as IObjectTableEntry);
                } else
                {
                    LinkBase(item);
                }*/
            }
            else
            {
                throw new dataException("Instance [" + key + "] wasn't found in the instance collection", null, this, "objectTableBase.UpdateBase() instance not found");
            }
        }

        /// <summary>
        /// Links the <c>item</c> to the <see cref="instanceRegistry"/> (overwrite any existing)
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="key">The key.</param>
        /// <exception cref="ArgumentOutOfRangeException">item - The ObjectTable is not in linkable mode - IsInstanceLinkActive = false</exception>
        protected virtual void LinkBase(object item, string key = null)
        {
            if (!IsInstanceLinkActive) throw new ArgumentOutOfRangeException("item", item, "The ObjectTable is not in linkable mode - IsInstanceLinkActive = false");
            if (item == null) return;

            if (item is IObjectTableEntry) if (instanceRegistry.ContainsKey(key))
                {
                    instanceRegistry.TryAdd(key, item as IObjectTableEntry);
                }
                else
                {
                    if (key.isNullOrEmpty()) key = GetKeyValue(item);
                    IObjectTableEntry removedItem = null;
                    instanceRegistry.TryRemove(key, out removedItem);
                    instanceRegistry.TryAdd(key, item as IObjectTableEntry);
                }
        }

        /// <summary>
        /// Removes the <c>item</c> from instanceRegistry but not from the table. Works only if <see cref="IsInstanceLinkActive"/> is true
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="key">The key (optional)</param>
        /// <exception cref="ArgumentOutOfRangeException">item - The ObjectTable is not in linkable mode - IsInstanceLinkActive = false</exception>
        protected virtual void UnlinkBase(object item, string key = null)
        {
            if (!IsInstanceLinkActive) throw new ArgumentOutOfRangeException("item", item, "The ObjectTable is not in linkable mode - IsInstanceLinkActive = false");
            if (item == null) return;
            if (instanceRegistry.ContainsKey(key))
            {
                if (item is IObjectTableEntry)
                {
                    if (key.isNullOrEmpty()) key = GetKeyValue(item);
                    IObjectTableEntry removedItem = null;
                    instanceRegistry.TryRemove(key, out removedItem);
                } // (key, item as IObjectTableEntry, item as IObjectTableEntry);
            }
        }

        protected virtual object CloneBase(object item, bool linkToNew)
        {
            if (!IsInstanceLinkActive) throw new ArgumentOutOfRangeException("item", item, "The ObjectTable is not in linkable mode - IsInstanceLinkActive = false");
            if (item == null) return null;
            string key = GetKeyValue(item);
            string expression = GetPrimaryKeySelect(key);
            var rows = tableSelect(expression);

            //var rows = table.Select(GetPrimaryKeySelect(keyValue));
            object clone = type.getInstance();
            foreach (DataRow dr in rows)
            {
                SetObjectByCustomRow(dr, type.getInstance());
                break;
            }
            if (linkToNew)
            {
                UnlinkBase(item, key);
                LinkBase(clone, key);
            }
            return clone;
        }

        /// <summary>
        /// Saves this instance. Returns <c>true</c> on success
        /// </summary>
        /// <returns>TRUE if saved sucessfully</returns>
        /// <exception cref="aceCommonTypes.core.exceptions.dataException">Can't just call Save() when no FileInfo instance ever set - null - Save() failed, call SaveAs() first</exception>
        public virtual bool Save(getWritableFileMode mode = getWritableFileMode.newOrExisting)
        {
            if (info != null)
            {
                if (!ReadOnlyMode) Monitor.Enter(SaveLinkedLock);
                try
                {
                    //if (IsInstanceLinkActive)
                    //{
                    //    foreach (var pair in instanceRegistry)
                    //    {
                    //        UpdateBase(pair.Value, pair.Key);
                    //    }

                    //}

                    table.AcceptChanges();
                    info = info.FullName.getWritableFile(mode);

                    if (table.TableName.isNullOrEmpty()) table.TableName = name;
                    if (table.TableName.isNullOrEmpty()) table.TableName = info.Name.Replace(".", "");

                    objectSerialization.saveObjectToXML(table, info.FullName);
                }
                finally
                {
                    if (!ReadOnlyMode) Monitor.Exit(SaveLinkedLock);
                }
                return true;
            }
            else
            {
                throw new dataException("Can't just call Save() when no FileInfo instance ever set", null, this, "Save() failed, call SaveAs() first");
                return false;
            }
        }

        protected bool ContainsObject(object input)
        {
            if (input == null) return false;
            if (!input.GetType().isCompatibileWith(type)) return false;

            string keyValue = GetKeyValue(input);

            //  var rows = table.Select(GetPrimaryKeySelect(keyValue));

            return ContainsKey(keyValue);
        }

        /// <summary>
        /// Information about the file where table data is stored
        /// </summary>
        /// <value>
        /// The information.
        /// </value>
        public FileInfo info { get; protected set; }

        public string name { get; set; }

        public Type type { get; protected set; }

        public string primaryKeyName { get; protected set; }

        protected DataColumn primaryKey { get; set; }

        public void Clear()
        {
            if (ReadOnlyEnforce()) return;

            lock (ContainsKeyLock)
            {
                table.Clear();
            }
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected int IndexOf(object item)
        {
            if (item == null) return -1;
            int output = 0;

            string keyValue = GetKeyValue(item);
            string exp = GetPrimaryKeySelect(keyValue);

            var rows = tableSelect(exp);

            if (rows.Any())
            {
                foreach (DataRow dr in rows)
                {
                    output = table.Rows.IndexOf(dr);
                }
                return output;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected bool Remove(object item)
        {
            if (ReadOnlyEnforce()) return false;
            if (ContainsObject(item))
            {
                string keyValue = GetKeyValue(item);

                if (keyCache.Contains(keyValue))
                {
                    keyCache.TryTake(out keyValue);
                }

                string select_exp = GetPrimaryKeySelect(keyValue);
                DataRow[] rows = tableSelect(select_exp);

                if (rows.Any())
                {
                    lock (ContainsKeyLock)
                    {
                        foreach (DataRow dr in rows)
                        {
                            table.Rows.Remove(dr);
                        }
                    }

                    if (IsInstanceLinkActive)
                    {
                        IObjectTableEntry removedItem = null;
                        instanceRegistry.TryRemove(keyValue, out removedItem);
                        return removedItem != null;
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        protected settingsEntriesForObject settings { get; set; }

        /// <summary>
        /// Prepares the collection table shema
        /// </summary>
        /// <param name="__type">The type.</param>
        protected void prepare(Type __type, string __primaryKeyPropertyName, string __name, bool buildTheTable = true)
        {
            name = __name;
            type = __type;

            if (type.isCompatibileWith(typeof(IObjectTableEntry)))
            {
                IsInstanceLinkActive = true;
                instanceRegistry = new ConcurrentDictionary<string, IObjectTableEntry>();
            }

            primaryKeyName = __primaryKeyPropertyName;
            settings = new settingsEntriesForObject(type, false);

            if (imbSciStringExtensions.isNullOrEmptyString(primaryKeyName))
            {
                foreach (settingsPropertyEntry spe in settings.spes.Values)
                {
                    if (spe.isPrimaryKey)
                    {
                        primaryKeyName = spe.pi.Name;
                    }
                }
            }

            if (buildTheTable)
            {
                buildTable();
            }
        }

        protected void buildTable()
        {
            table = new DataTable(name);
            table.SetClassType(type);
            table.SetClassName(type.Name);
            table.SetDescription(description);
            table.SetCategoryPriority(settings.CategoryByPriority);

            table.PrimaryKey = new DataColumn[] { };

            foreach (settingsPropertyEntry spe in settings.spes.Values)
            {
                if (!spe.IsXmlIgnore)
                {
                    var column = table.Add(spe);
                    if (spe.pi.Name == primaryKeyName)
                    {
                        primaryKey = column;
                        UsePrimaryKey = true;
                        table.PrimaryKey = new DataColumn[] { primaryKey };
                    }
                }
            }
        }

        protected bool UsePrimaryKey { get; set; } = false;

        /// <summary>
        /// Checks the table shema: returns the table with right shema
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        protected DataTable checkTableShema(DataTable input)
        {
            DataTable output = input.GetClonedShema<DataTable>();

            // <--- removing non existant
            foreach (DataColumn dc in input.Columns)
            {
                if (settings.spes.ContainsKey(dc.ColumnName))
                {
                    // <---- column ok
                }
                else
                {
                    output.Columns.Remove(dc.ColumnName);
                }
            }
            // <---- adding one that missing
            foreach (settingsPropertyEntry spe in settings.spes.Values)
            {
                if (!spe.IsXmlIgnore)
                {
                    if (input.Columns.Contains(spe.pi.Name))
                    {
                        output.Columns[(string)spe.pi.Name].SetSPE(spe);
                    }
                    else
                    {
                        output.Add(spe);
                        //utput.Add(spe.pi.Name, spe.description, spe.letter, spe.type, spe.importance, spe.format, spe.displayName);
                    }
                }
            }

            output.CopyRowsFrom(input);

            return output;
        }

        /// <summary>
        /// If TRUE: instances stay linked to the object table, their properties are loaded back to the data table automatically on save
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is instance link active; otherwise, <c>false</c>.
        /// </value>
        public bool IsInstanceLinkActive { get; protected set; }

        protected ConcurrentDictionary<string, IObjectTableEntry> instanceRegistry { get; set; }

        private object AddOrUpdateLock2 = new object();

        /// <summary>
        /// Adds the or updates data table with objects specified. Returns number of new items added into collection
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        protected int AddOrUpdate(IEnumerable<object> input, objectTableUpdatePolicy policy = objectTableUpdatePolicy.overwrite)
        {
            if (ReadOnlyEnforce()) return 0;
            int output = 0;
            lock (AddOrUpdateLock2)
            {
                foreach (object inp in input)
                {
                    if (AddOrUpdate(inp, policy))
                    {
                        output++;
                    }

                    if (IsInstanceLinkActive)
                    {
                        UpdateBase(input as IObjectTableEntry);
                    }
                }
            }

            return output;
        }

        protected virtual int SELECT_LIMIT_RETRY { get; set; } = 10;

        /// <summary>
        /// Gets the existing row or autocreates new. If <see cref="IsInstanceLinkActive"/> is true it returns registrated instance
        /// </summary>
        /// <param name="keyValue">The key value.</param>
        /// <returns></returns>
        protected object GetOrCreate(string keyValue)
        {
            object output = null;

            if (IsInstanceLinkActive)
            {
                if (instanceRegistry.ContainsKey(keyValue))
                {
                    return instanceRegistry[keyValue];
                }
            }

            Boolean _exists = false;
            DataRow[] rows = null;
            if (!WriteOnlyEnforce())
            {
                string select_exp = GetPrimaryKeySelect(keyValue);
                rows = tableSelect(select_exp);
                _exists = rows.Any();
            }

            //string select_exp = GetPrimaryKeySelect(keyValue);
            //DataRow[] rows = tableSelect(select_exp);

            if (_exists)
            {
                output = GetObjectFromRow(rows.First());
            }
            else
            {
                if (ReadOnlyEnforce()) return 0;

                output = type.getInstance();
                output.imbSetPropertySafe(primaryKeyName, keyValue, true, null, false);
                AddOrUpdate(output, objectTableUpdatePolicy.overwrite);
            }
            return output;
        }

        /// <summary>
        /// Returns the list of instances for the specified expression <see cref="DataTable.Select(string)"/>
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="limit">The limit.</param>
        /// <returns></returns>
        protected List<object> GetWhere(string expression, int limit = -1, string sortColumn = "", objectTableSortEnum sortType = objectTableSortEnum.none)
        {
            List<object> output = new List<object>();
            if (IsInstanceLinkActive)
            {
                foreach (object item in instanceRegistry)
                {
                    UpdateBase(item);
                }
            }

            DataRow[] rows = null;

            if (sortType != objectTableSortEnum.none)
            {
                sortColumn = imbSciStringExtensions.removeEndsWith(sortColumn, " ASC");
                sortColumn = imbSciStringExtensions.removeEndsWith(sortColumn, " asc");
                sortColumn = imbSciStringExtensions.removeEndsWith(sortColumn, " DESC");
                sortColumn = imbSciStringExtensions.removeEndsWith(sortColumn, " desc");
                sortColumn = imbSciStringExtensions.add(sortColumn, sortType.ToString(), " ");
            }

            rows = tableSelect(expression, sortColumn);

            if (rows != null)
            {
                if (rows.Any())
                {
                    if (limit > 0)
                    {
                        limit = Math.Min(rows.Count(), limit);
                        rows = rows.Take(limit).ToArray();
                    }

                    output = GetObjectFromRows(rows);
                }
            }

            return output;
        }

        //private Object AddOrUpdateLock = new Object();

        protected virtual string GetPrimaryKeySelect(string keyValue)
        {
            return primaryKeyName + " = '" + keyValue + "'";
        }

        /// <summary>
        /// Transforms the key (dummy)
        /// </summary>
        /// <param name="keyValue">The key value.</param>
        /// <returns></returns>
        protected virtual string TransformKey(string keyValue)
        {
            return keyValue;
        }

        private object SelectTableLock = new object();

        private bool ImmediateLockDown = false;

        protected DataRow[] tableSelect(string query, string sortColumn = "")
        {
            int ri = 0;
            bool re = true;
            DataRow[] rows = new DataRow[] { };

            while (re)
            {
                if (!ReadOnlyMode) Monitor.Enter(SelectTableLock);
                try
                {
                    if (query.isNullOrEmpty())
                    {
                        rows = table.Select();
                    }
                    else if (sortColumn.isNullOrEmpty())
                    {
                        rows = table.Select(query);
                    }
                    else
                    {
                        rows = table.Select(query, sortColumn);
                    }

                    re = false;
                }
                catch (Exception ex)
                {
                    ri++;
                    Thread.Sleep(10);

                    if (ri > (SELECT_LIMIT_RETRY / 2))
                    {
                        Console.WriteLine("Object Table [" + name + "] query [" + query + "] retried: " + ri.ToString());
                    }
                    if (ri > SELECT_LIMIT_RETRY)
                    {
                        re = false;
                        throw;
                    }
                }
                finally
                {
                    if (!ReadOnlyMode) Monitor.Exit(SelectTableLock);
                }
            }

            return rows;
        }

        /// <summary>
        /// Adds the or updates. Returns <c>true</c> if new row was added
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        protected bool AddOrUpdate(object input, objectTableUpdatePolicy policy = objectTableUpdatePolicy.overwrite)
        {
            if (ReadOnlyEnforce()) return false;
            if (input == null) return false;

            if (!input.GetType().isCompatibileWith(type)) return false;

            bool output = false;

            string keyValue = GetKeyValue(input);

            //if (keyValue.isNullOrEmpty())
            //{
            //     aceLog.log("keyValue is null => " + name + " : " + GetType().Name);
            //}

            if (IsInstanceLinkActive)
            {
                if (instanceRegistry.ContainsKey(keyValue))
                {
                    IObjectTableEntry item = input as IObjectTableEntry;
                    if (instanceRegistry.TryUpdate(keyValue, item, item))
                    {
                        output = false;
                    }
                }
            }

            Boolean _exists = false;
            DataRow[] rows = null;
            if (!WriteOnlyEnforce())
            {
                string select_exp = GetPrimaryKeySelect(keyValue);
                rows = tableSelect(select_exp);
                _exists = rows.Any();
            }

            if (_exists)
            {
                SetRowWithObject(rows.First(), input, policy);
                output = false;
            }
            else
            {
                SetRowWithObject(null, input, objectTableUpdatePolicy.overwrite);
                output = true;
            }

            if (IsInstanceLinkActive)
            {
                if (!instanceRegistry.ContainsKey(keyValue))
                {
                    IObjectTableEntry item = input as IObjectTableEntry;
                    if (instanceRegistry.TryAdd(keyValue, item)) output = true;
                }
            }

            if (output)
            {
                lastEntry = input;
            }

            return output;
        }

        /// <summary>
        /// Gets the key value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        protected string GetKeyValue(object input)
        {
            object vl = input.imbGetPropertySafe(primaryKeyName, null);
            return vl.toStringSafe();
        }

        /// <summary>
        /// Sets the row with object. Specify row as <c>null</c> to create new row
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="input">The input.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        protected DataRow SetRowWithObject(DataRow row, object input, objectTableUpdatePolicy policy)
        {
            bool addTheRow = false;

            if (row == null)
            {
                if (ReadOnlyEnforce())
                {
                    return null;
                }
            }

            if (!ReadOnlyMode) Monitor.Enter(SaveLinkedLock);
            try
            {
                if (!ReadOnlyMode) Monitor.Enter(SelectTableLock);
                try
                {
                    if (row == null)
                    {
                        row = table.NewRow();
                        addTheRow = true;
                    }

                    foreach (settingsPropertyEntry spe in settings.spes.Values)
                    {
                        if (!spe.IsXmlIgnore)
                        {
                            if (spe.isPrimaryKey)
                            {
                                string primKey = row[primaryKeyName].toStringSafe("");
                                if (!keyCache.Contains(primKey)) keyCache.Add(primKey);
                            }

                            #region SWITCH

                            switch (policy)
                            {
                                default:
                                case objectTableUpdatePolicy.none:
                                case objectTableUpdatePolicy.overwrite:
                                    row[(string)spe.pi.Name] = input.imbGetPropertySafe(spe.pi, spe.pi.PropertyType.GetDefaultValue());
                                    break;

                                case objectTableUpdatePolicy.updateIfLower:
                                case objectTableUpdatePolicy.updateIfHigher:
                                    bool doUpdate = false;
                                    if (spe.pi.PropertyType.isNumber())
                                    {
                                        if (spe.pi.PropertyType == typeof(int))
                                        {
                                            int vlInt32 = (int)row[(string)spe.pi.Name].imbConvertValueSafeTyped<int>();
                                            int obInt32 = input.imbGetPropertySafe<int>(spe.pi);

                                            if (policy == objectTableUpdatePolicy.updateIfHigher) doUpdate = (obInt32 > vlInt32);
                                            else doUpdate = (obInt32 < vlInt32);
                                        }
                                        else if (spe.pi.PropertyType == typeof(double))
                                        {
                                            double vlDouble = (double)row[(string)spe.pi.Name].imbConvertValueSafeTyped<double>();
                                            double obDouble = input.imbGetPropertySafe<double>(spe.pi);
                                            if (policy == objectTableUpdatePolicy.updateIfHigher) doUpdate = (obDouble > vlDouble);
                                            else doUpdate = (obDouble < vlDouble);
                                        }
                                        else
                                        {
                                            doUpdate = true;
                                        }
                                    }
                                    if (doUpdate) row[(string)spe.pi.Name] = input.imbGetPropertySafe(spe.pi, spe.pi.PropertyType.GetDefaultValue());
                                    break;
                            }

                            #endregion SWITCH
                        }
                    }

                    if (addTheRow)
                    {
                        table.Rows.Add(row);
                    }
                }
                finally
                {
                    if (!ReadOnlyMode) Monitor.Exit(SelectTableLock);
                }
            }
            finally
            {
                if (!ReadOnlyMode) Monitor.Exit(SaveLinkedLock);
            }

            InvokeChanged();

            return row;
        }

        /// <summary>
        /// Gets or sets the last entry that was touched
        /// </summary>
        /// <value>
        /// The last entry.
        /// </value>
        protected object lastEntry { get; set; }

        protected DataTable table { get; set; }

        protected List<object> GetObjectList(string expression)
        {
            var result = tableSelect(expression);

            return GetObjectFromRows(result);
        }

        protected List<object> GetObjectList()
        {
            var result = tableSelect("");
            return GetObjectFromRows(result);
        }

        protected List<object> GetObjectFromRows(IEnumerable<DataRow> result)
        {
            List<object> output = new List<object>();

            foreach (DataRow dr in result)
            {
                output.Add(GetObjectFromRow(dr));
            }

            return output;
        }

        /// <summary>
        /// Sets the object by custom row (missing columns are also supported)
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="item">The item.</param>
        /// <returns>The object provided through <c>item</c></returns>
        protected object SetObjectByCustomRow(DataRow row, object item)
        {
            foreach (settingsPropertyEntry spe in settings.spes.Values)
            {
                if (spe.isPrimaryKey)
                {
                    string primKey = row[primaryKeyName].toStringSafe("");
                    if (!keyCache.Contains(primKey)) keyCache.Add(primKey);
                }

                if (!spe.IsXmlIgnore)
                {
                    if (row.Table.Columns.Contains(spe.pi.Name))
                    {
                        object vl = row[(string)spe.pi.Name];
                        item.imbSetPropertySafe(spe.pi, vl.imbConvertValueSafe(spe.type), true, null, false);
                    }
                }
            }

            //if (IsInstanceLinkActive)
            //{
            //    LinkBase(item as IObjectTableEntry);
            //}
            return item;
        }

        /// <summary>
        /// Gets the object from row --- basic and low level stuff
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        protected object GetObjectFromRow(DataRow row)
        {
            object output = type.getInstance();

            foreach (settingsPropertyEntry spe in settings.spes.Values)
            {
                if (spe.isPrimaryKey)
                {
                    string primKey = row[primaryKeyName].toStringSafe("");
                    if (!keyCache.Contains(primKey)) keyCache.Add(primKey);
                }

                if (!spe.IsXmlIgnore)
                {
                    object vl = row[(string)spe.pi.Name];
                    output.imbSetPropertySafe(spe.pi, vl.imbConvertValueSafe(spe.type), true, null, false);
                }
            }

            if (IsInstanceLinkActive)
            {
                LinkBase(output as IObjectTableEntry);
            }

            return output;
        }
    }
}