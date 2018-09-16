// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyCollectionExtended.cs" company="imbVeles" >
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
// Project: imbSCI.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Core.collection
{
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.enumworks;
    using imbSCI.Core.extensions.text;
    using imbSCI.Data;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// PropertyCollection with extended meta information about it self and about Property entries
    /// </summary>
    /// <seealso cref="System.Data.PropertyCollection" />
    public class PropertyCollectionExtended : PropertyCollection
    {
        public static String PAIRFORM = "{0,16} = {1}" + Environment.NewLine;

        /// <summary>
        /// PCE overriden ToString() shows name, description and parameter values
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(PAIRFORM, "PCE name", name);
            sb.AppendFormat(PAIRFORM, "PCE info", description);
            sb.AppendLine("-------------");
            foreach (KeyValuePair<object, PropertyEntry> p in entries)
            {
                sb.AppendFormat(PAIRFORM, p.Value.keyName, this[p.Key].toStringSafe());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Builds dictionary with specified columns as key and value
        /// </summary>
        /// <param name="keyColumn">The key column.</param>
        /// <param name="valueColumn">The value column.</param>
        /// <returns></returns>
        public Dictionary<String, String> GetStringDictionary(PropertyEntryColumn keyColumn = PropertyEntryColumn.entry_key, PropertyEntryColumn valueColumn = PropertyEntryColumn.entry_value)
        {
            Dictionary<String, String> output = new Dictionary<string, string>();

            foreach (KeyValuePair<object, PropertyEntry> p in entries)
            {
                output.Add(p.Value[keyColumn].toStringSafe(), p.Value[valueColumn].toStringSafe());
            }

            return output;
        }

        /// <summary>
        /// Appends the vertically.
        /// </summary>
        /// <param name="pData">The p data.</param>
        /// <param name="externalDescriptionLibrary">The external description library.</param>
        /// <param name="excludeCoreColumns">if set to <c>true</c> [exclude core columns].</param>
        public void AppendVertically(PropertyEntry pData, Dictionary<Object, String> externalDescriptionLibrary = null, Boolean excludeCoreColumns = true)
        {
            // if (externalDescriptionLibrary == null) externalDescriptionLibrary = pData.Keys.

            foreach (Object key in pData.Keys)
            {
                Boolean ok = true;
                if (excludeCoreColumns)
                {
                    if (key.isCoreColumn())
                    {
                        ok = false;
                    }
                }
                if (ok) addAsObjectKey(key, pData[key], true, externalDescriptionLibrary);
            }
        }

        /// <summary>
        /// Adds as object key -- automatically designates title, uses <c>externalDescriptionLibrary</c> for definitions for non <see cref="PropertyEntryColumn"/> keys
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="v1">The v1.</param>
        /// <param name="externalDescriptionLibrary">The external description library.</param>
        public void addAsObjectKey(object key, object v1, Boolean autodescribe = true, Dictionary<object, string> externalDescriptionLibrary = null)
        {
            if (externalDescriptionLibrary == null) externalDescriptionLibrary = new Dictionary<object, string>();
            String desc = "";

            if (autodescribe)
            {
                if (externalDescriptionLibrary.ContainsKey(key))
                {
                    desc = externalDescriptionLibrary[key];
                }
                else
                {
                    desc = key.getDescriptionForDictionary();
                }
            }
            Add(key, v1, key.ToString().imbTitleCamelOperation(), desc);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="PropertyCollectionExtended"/> to <see cref="DataTable"/>.
        /// </summary>
        /// <param name="pce">The pce.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator DataTable(PropertyCollectionExtended pce)
        {
            DataTable output = new DataTable(pce.name);
            output = pce.getDataTable(PropertyEntryColumn.entry_name, PropertyEntryColumn.entry_value, PropertyEntryColumn.entry_description);

            return output;
        }

        //public static implicit operator PropertyCollectionExtended(PropertyCollection pc)
        //{
        //    PropertyCollectionExtended pce = new PropertyCollectionExtended();
        //    pce.AppendData(pc, existingDataMode.overwriteExistingIfEmptyOrNull, false);
        //    return pce;

        //}

        private Boolean _autoIncludeImportance = false;

        /// <summary>
        ///
        /// </summary>
        public Boolean autoIncludeImportance
        {
            get { return _autoIncludeImportance; }
            set { _autoIncludeImportance = value; }
        }

        private PropertyEntryColumn _defaultTableColumn = PropertyEntryColumn.entry_name | PropertyEntryColumn.entry_value | PropertyEntryColumn.entry_description;

        /// <summary>
        ///
        /// </summary>
        public PropertyEntryColumn defaultTableColumn
        {
            get { return _defaultTableColumn; }
            //  set { _defaultTableColumn = value; }
        }

        public void addAndDescribeKey(Object key, Object data, Boolean skipExistingKeys = false, Boolean descriptionFromKey = true)
        {
        }

        /// <summary>
        /// Builds the data table with columns specified - order of columns as specified
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <returns></returns>
        public DataTable getDataTable(params PropertyEntryColumn[] columns)
        {
            DataTable output = new DataTable();
            DataColumn dc_importance = null;
            output.TableName = name;

            //output.SetTitle(name);
            output.ExtendedProperties[templateFieldDataTable.description] = description;

            output.ExtendedProperties[templateFieldDataTable.data_rowcounttotal] = Count;

            // output.ExtendedProperties[templateFieldStyling.color_paletteRole] = palete;

            List<PropertyEntryColumn> columnList = columns.ToList();
            if ((!columnList.Any()) || columnList.Contains(PropertyEntryColumn.none))
            {
                columnList = defaultTableColumn.getEnumListFromFlags<PropertyEntryColumn>();
            }

            Int32 ci = 0;
            foreach (PropertyEntryColumn column in columnList)
            {
                if (column != PropertyEntryColumn.none)
                {
                    DataColumn dc = output.Columns.Add();
                    column.setDataColumn(dc);
                    if (column == PropertyEntryColumn.entry_importance)
                    {
                        dc_importance = dc;
                    }
                }
            }
            if (autoIncludeImportance)
            {
                if (dc_importance == null)
                {
                    DataColumn dc = output.Columns.Add();
                    PropertyEntryColumn.entry_importance.setDataColumn(dc);
                    dc.ExtendedProperties[templateFieldStyling.render_isHidden] = true;
                    dc_importance = dc;
                }
            }

            PropertyEntry entryMeta = null;
            String autocount_format = "D2";
            if (Count > 99)
            {
                autocount_format = "D3";
            }

            List<PropertyEntry> pel = entries.Values.ToList();
            pel.Sort((x, y) => x.priority.CompareTo(y.priority));
            Int32 c = 1;
            foreach (PropertyEntry entry in pel)
            {
                DataRow dr = output.NewRow();

                //entryMeta = entries[entry[PropertyEntryColumn.entry_key];

                foreach (DataColumn dc in output.Columns)
                {
                    PropertyEntryColumn column = (PropertyEntryColumn)dc.ExtendedProperties[templateFieldDataTable.col_name];
                    Object vl = "--";

                    switch (column)
                    {
                        case PropertyEntryColumn.autocount_idcolumn:
                            vl = c.ToString(autocount_format);
                            break;

                        default:
                            vl = entry.getColumn(column, this[entry[PropertyEntryColumn.entry_key]]);
                            break;
                    }

                    if (imbSciStringExtensions.isNullOrEmptyString(vl))
                    {
                        vl = "    ";
                    }

                    if (column == PropertyEntryColumn.autocount_idcolumn)
                    {
                        vl = c;
                    }

                    dr[dc] = vl;
                }
                output.Rows.Add(dr);
                c++;
            }

            return output;
        }

        //private acePaletteRole _palete = acePaletteRole.colorA;
        ///// <summary>
        /////
        ///// </summary>
        //public acePaletteRole palete
        //{
        //    get { return _palete; }
        //    set { _palete = value; }
        //}

        private String _name;

        /// <summary>Display title of this collection</summary>
        public String name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private String _description;

        /// <summary> </summary>
        public String description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        private PropertyEntryDictionary _entries = new PropertyEntryDictionary();

        /// <summary>
        ///
        /// </summary>
        public PropertyEntryDictionary entries
        {
            get { return _entries; }
            set { _entries = value; }
        }

        public override void Clear()
        {
            base.Clear();
            entries.Clear();
        }

        public override void Remove(object key)
        {
            base.Remove(key);
            entries.Remove(key);
        }

        /// <summary>
        /// Adds the range. When <c>skipExisting</c> booleans are <c>true</c> it applies <see cref="aceCommonTypes.enums.existingDataMode.overwriteExistingIfEmptyOrNull"/> for smarter data gathering
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="skipExistingValues">if set to <c>true</c> [skip existing values].</param>
        /// <param name="skipExistingMeta">if set to <c>true</c> it will keep existing <c>PropertyEntry</c> when overlapped with source</param>
        /// <param name="strictMetaKeyMatch">If set to <c>true</c> it will not introduce new columns into data set. If <c>false</c> it will upgrade data set with not existing columns found at <c>source</c></param>
        public void AddRange(PropertyCollectionExtended source, Boolean skipExistingValues, Boolean skipExistingMeta, Boolean strictMetaKeyMatch)
        {
            foreach (KeyValuePair<Object, PropertyEntry> pePair in source.entries)
            {
                PropertyEntry pe = pePair.Value;

                if (ContainsKey(pePair.Key))
                {
                    if (!skipExistingValues)
                    {
                        this[pePair.Key] = source[pePair.Key];
                    }
                    else
                    {
                        if (imbSciStringExtensions.isNullOrEmptyString(this[pePair.Key])) this[pePair.Key] = source[pePair.Key];
                    }
                }
                else
                {
                    base.Add(pePair.Key, pePair.Value);
                }
                if (entries.ContainsKey(pePair.Key))
                {
                    PropertyEntry oldPe = entries[pePair.Key];
                    if (skipExistingMeta)
                    {
                        oldPe.AppendData(pePair.Value, existingDataMode.overwriteExistingIfEmptyOrNull);
                    }
                    else
                    {
                        oldPe.AppendData(pePair.Value, existingDataMode.overwriteExisting);
                    }

                    // update
                }
                else
                {
                    if (!strictMetaKeyMatch) entries.Add(pePair.Key, pePair.Value);
                    // insert
                }
            }
        }

        public void Add(PropertyEntry pe, Boolean disableValueTransfer = false)
        {
            if (pe == null) return;

            entries[pe.EntryKey].AppendData(pe, existingDataMode.overwriteExistingIfEmptyOrNull);

            if (disableValueTransfer)
            {
                base.Add(pe.EntryKey, "");
            }
            else
            {
                base.Add(pe.EntryKey, pe.EntryValue);
            }
        }

        public void AddMetaRangeFrom(PropertyCollectionExtended pce)
        {
            foreach (KeyValuePair<Object, PropertyEntry> pePair in pce.entries)
            {
                AddMetaFrom(pePair.Value, false, true);
            }
        }

        /// <summary>
        /// Copis meta descriptions on the matching key. If <see cref="PropertyEntry"/> not defined it adds this
        /// </summary>
        /// <param name="pe">The pe.</param>
        /// <param name="skipExisting">if set to <c>true</c> [skip existing].</param>
        public void AddMetaFrom(PropertyEntry pe, Boolean skipExisting, Boolean disableValueTransfer = false)
        {
            if (!entries.ContainsKey(pe.EntryKey))
            {
                Add(pe, disableValueTransfer);
            }
            else
            {
                entries[pe.EntryKey].AddRange(GetMetaForKey(pe.EntryKey), skipExisting);
            }
        }

        /// <summary>
        /// Redefines meta descriptions for <c>key</c> entry. Empty or null entries are ignored
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="__unit">The unit.</param>
        /// <param name="__letter">The letter.</param>
        /// <param name="__symbol">The symbol.</param>
        /// <param name="__name">The name.</param>
        public void AddMetaFor(Object key, String __unit = "", String __letter = "", String __symbol = "", String __name = "", Boolean skipExisting = false)
        {
            var pe = entries[key];
            if (!__name.isNullOrEmpty()) pe.add(PropertyEntryColumn.entry_name, __name, skipExisting);
            if (!__unit.isNullOrEmpty()) pe.add(PropertyEntryColumn.entry_unit, __unit, skipExisting); // = __unit;
            if (!__letter.isNullOrEmpty()) pe.add(PropertyEntryColumn.role_letter, __letter, skipExisting);  //= __letter;
            if (!__symbol.isNullOrEmpty()) pe.add(PropertyEntryColumn.role_symbol, __symbol, skipExisting); // = __symbol;
        }

        /// <summary>
        /// Get all non value (<see cref="aceCommonTypes.extensions.data.PropertyEntryColumnExtensions.isKeyValueColumn(imbSCI.Data.collection.PropertyEntryColumn)"/> columns into <see cref="PropertyCollection"/>
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public PropertyCollection GetMetaForKey(Object key)
        {
            var pe = entries[key];
            Object input = null;
            PropertyCollection output = new PropertyCollection();
            foreach (DictionaryEntry pair in pe)
            {
                input = pair.Value;
                Boolean copyOk = true;
                if (input is PropertyEntryColumn)
                {
                    PropertyEntryColumn input_PropertyEntryColumn = (PropertyEntryColumn)input;
                    if (input_PropertyEntryColumn.isKeyValueColumn()) copyOk = false;
                }
                if (copyOk)
                {
                    output.Add(pair.Key, pair.Value);
                }
            }
            return output;
        }

        /// <summary>
        /// Maps to multiple keys
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="__name">The name.</param>
        /// <param name="__description">The description.</param>
        /// <returns></returns>
        public PropertyEntry Add(Array key, object value, String __name, String __description)
        {
            PropertyEntry pe = new PropertyEntry(key, value, __name, __description);
            foreach (Object k in key)
            {
                if (ContainsKey(k))
                {
                    this[k] = value;
                }
                else
                {
                    base.Add(k, value);
                }
            }

            foreach (PropertyEntryColumn pekey in pe.Keys)
            {
                if (!defaultTableColumn.HasFlag(pekey))
                {
                    _defaultTableColumn |= pekey;
                }
            }

            foreach (Object k in key)
            {
                pe.priority = entries.Count();
                entries.Add(k, pe);
            }
            return pe;
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="__name">The name.</param>
        /// <param name="__description">The description.</param>
        /// <param name="__unit">The unit.</param>
        /// <param name="__letter">The letter.</param>
        /// <param name="__symbol">The symbol.</param>
        /// <returns></returns>
        public PropertyEntry Add(object key, object value, String __name, String __description, String __unit = "", String __letter = "", String __symbol = "")
        {
            if (key == "")
            {
                // key = __name.getFilename();
            }
            if (ContainsKey(key))
            {
                this[key] = value;
            }
            else
            {
                base.Add(key, value);
            }
            PropertyEntry pe = new PropertyEntry(key, value, __name, __description);
            if (!__unit.isNullOrEmpty()) pe[PropertyEntryColumn.entry_unit] = __unit;
            if (!__letter.isNullOrEmpty()) pe[PropertyEntryColumn.role_letter] = __letter;
            if (!__symbol.isNullOrEmpty()) pe[PropertyEntryColumn.role_symbol] = __symbol;

            foreach (PropertyEntryColumn pekey in pe.Keys)
            {
                if (!defaultTableColumn.HasFlag(pekey))
                {
                    _defaultTableColumn |= pekey;
                }
            }

            pe.priority = entries.Count();
            entries.Add(key, pe);
            return pe;
        }

        public override void Add(object key, object value)
        {
            base.Add(key, value);
            entries.Add(key, new PropertyEntry(key, value));
        }

        public override Object this[Object key]
        {
            get
            {
                if (!base.ContainsKey(key)) return null;
                return base[key];
            }
            set
            {
                if (!entries.ContainsKey(key))
                {
                    entries.Add(key, new PropertyEntry(key, value));
                }
                if (!ContainsKey(key))
                {
                    base.Add(key, value);
                }
                else
                {
                    base[key] = value;
                }

                // entries[key] = new PropertyEntry(key, value);
            }
        }
    }
}