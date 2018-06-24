// --------------------------------------------------------------------------------------------------------------------
// <copyright file="propertyAnnotationPresetEntry.cs" company="imbVeles" >
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
namespace imbSCI.Core.style.preset
{
    using imbSCI.Core.attributes;
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.files;
    using imbSCI.Core.files.folders;
    using imbSCI.Core.reporting;
    using imbSCI.Data;
    using imbSCI.Data.enums.fields;
    using imbSCI.Data.interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Preset with static formatting / styling column/property data annotation definitions
    /// </summary>
    public class propertyAnnotationPreset : IObjectWithNameAndDescription
    {
        /// <summary>
        /// Data annotation preset name
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String name { get; set; } = "Preset";

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public String description { get; set; } = "Custom data annotation preset";

        /// <summary>
        /// Saves the specified folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="filenameOverride">The filename override.</param>
        /// <returns></returns>
        public String Save(folderNode folder, String filenameOverride = "")
        {
            String output = "";

            if (filenameOverride.isNullOrEmpty()) filenameOverride = name.add("xml", ".");
            output = folder.pathFor(filenameOverride, Data.enums.getWritableFileMode.newOrExisting, description, true);
            objectSerialization.saveObjectToXML(this, output);
            return output;
        }

        /// <summary>
        /// Loads preset from the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        public static propertyAnnotationPreset Load(String filename, ILogBuilder log)
        {
            return objectSerialization.loadObjectFromXML<propertyAnnotationPreset>(filename, log);
        }

        private static Object _supportedEnumTypes_lock = new object();

        private static List<Type> _supportedEnumTypes;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static List<Type> supportedEnumTypes
        {
            get
            {
                if (_supportedEnumTypes == null)
                {
                    lock (_supportedEnumTypes_lock)
                    {
                        if (_supportedEnumTypes == null)
                        {
                            _supportedEnumTypes = new List<Type>();

                            var dict = imbTypologyHelpers.CollectTypes(typeof(imbSCI.Data.enums.fields.dataSource),
                                CollectTypeFlags.includeEnumTypes | CollectTypeFlags.ofSameNamespace | CollectTypeFlags.ofThisAssembly);

                            _supportedEnumTypes.AddRange(imbTypologyHelpers.CollectTypes(CollectTypeFlags.ofThisAssembly | CollectTypeFlags.includeEnumTypes));

                            _supportedEnumTypes.Add(typeof(imbAttributeName));

                            _supportedEnumTypes.AddRange(dict.Values);
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _supportedEnumTypes;
            }
        }

        /// <summary>
        /// Constructor required for XML Serialization
        /// </summary>
        public propertyAnnotationPreset() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="propertyAnnotationPreset"/>, and runs <see cref="SetFrom(Type, bool)"/> with specified type
        /// </summary>
        /// <param name="type">Type to learn data annotations from</param>
        public propertyAnnotationPreset(Type type)
        {
            SetFrom(type);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="propertyAnnotationPreset"/>, and runs <see cref="SetFrom(DataTable, bool)"/> with specified table.
        /// </summary>
        /// <param name="table">Table to learn data annotations from</param>
        public propertyAnnotationPreset(DataTable table)
        {
            SetFrom(table);
        }

        /// <summary>
        /// Items collection - for serialization
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<propertyAnnotationPresetItem> items { get; set; } = new List<propertyAnnotationPresetItem>();

        /// <summary>
        /// DataAnnotation to be deployed for property or data column that is not expliclitly defined by the preset
        /// </summary>
        /// <value>
        /// The default item.
        /// </value>
        public propertyAnnotationPresetItem defaultItem { get; set; } = new propertyAnnotationPresetItem();

        /// <summary>
        /// Deploys contained <see cref="propertyAnnotationPresetItem"/> to specified <c>table</c>
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="skipExisting">if set to <c>true</c> it will not overwrite formatting properties that were already defined.</param>
        public void DeployTo(DataTable table, Boolean skipExisting = true, ILogBuilder log = null)
        {
            if (log != null) log.log("Starting with DeployTo(" + table.GetTitle() + ") of preset [" + name + "] (skipExisting=" + skipExisting.ToString() + ")");
            foreach (DataColumn column in table.Columns)
            {
                var i = GetAnnotationPresetItem(column);

                if (i == defaultItem)
                {
                    if (log != null) log.log("Default column selected [" + column.ColumnName + "]");
                }

                i.DeployTo(column, skipExisting, log);
            }
            table.SetCategoryPriority(CategoryByPriority);
        }

        /// <summary>
        /// Sets or updates the preset using data annotations from <c>table</c>
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="skipExistingAnnotations">if set to <c>true</c>, it will not update already defined data annotation rule of a property.</param>
        public void SetFrom(DataTable table, Boolean skipExistingAnnotations = true)
        {
            name = table.GetTitle();
            foreach (DataColumn column in table.Columns)
            {
                SetFrom(column, skipExistingAnnotations);
            }
            CategoryByPriority = table.GetCategoryPriority();
            description = description.add("Imported from data table [" + table.GetTitle() + "]", ". ");
        }

        /// <summary>
        /// Sets or updates the preset using data annotations from <c>type</c>
        /// </summary>
        /// <param name="type">Type to read data annotation attributes from</param>
        /// <param name="skipExistingAnnotations">if set to <c>true</c>, it will not update already defined data annotation rule of a property.</param>
        public void SetFrom(Type type, Boolean skipExistingAnnotations = true)
        {
            name = type.Name;
            settingsEntriesForObject sEO = new settingsEntriesForObject(type, false);
            foreach (KeyValuePair<string, settingsPropertyEntryWithContext> pce in sEO.spes)
            {
                SetFrom(pce.Value, skipExistingAnnotations);
            }
            CategoryByPriority = sEO.CategoryByPriority;
            description = description.add("Imported from data type [" + type.Name + "]", ". ");
        }

        public List<String> CategoryByPriority { get; set; } = new List<string>();

        /// <summary>
        /// Sets or creates new preset item, with data annotations taken from the specified column
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="skipExistingAnnotations">if set to <c>true</c> [skip existing annotations].</param>
        /// <returns>Created or updated preset item</returns>
        public propertyAnnotationPresetItem SetFrom(DataColumn column, Boolean skipExistingAnnotations = true)
        {
            var item = GetAnnotationPresetItem(column);
            if (item == defaultItem)
            {
                item = new propertyAnnotationPresetItem(column);

                items.Add(item);
                return item;
            }
            else
            {
                item.SetFrom(column, skipExistingAnnotations);
                return item;
            }
        }

        /// <summary>
        /// Sets or creates new preset item, with data annotations taken from the specified column
        /// </summary>
        /// <param name="property">The property entry to learn from.</param>
        /// <param name="skipExistingAnnotations">if set to <c>true</c> [skip existing annotations].</param>
        /// <returns>Created or updated preset item</returns>
        public propertyAnnotationPresetItem SetFrom(settingsPropertyEntry property, Boolean skipExistingAnnotations = true)
        {
            var item = GetAnnotationPresetItem(property.name);
            if (item == defaultItem)
            {
                item = new propertyAnnotationPresetItem();
                item.SetFrom(property, skipExistingAnnotations);
                items.Add(item);

                return item;
            }
            else
            {
                item.SetFrom(property, skipExistingAnnotations);
                return item;
            }
        }

        /// <summary>
        /// Manual creation of new preset item definition
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="displayName">The display name for the property.</param>
        /// <param name="description">The description, attached to the property</param>
        /// <returns></returns>
        public propertyAnnotationPresetItem AddAnnotationPresetItem(String propertyName, String displayName = "", String description = "")
        {
            propertyAnnotationPresetItem output = new propertyAnnotationPresetItem()
            {
                name = propertyName
            };

            if (displayName.isNullOrEmpty()) displayName = propertyName.imbTitleCamelOperation(true);

            if (!description.isNullOrEmpty()) output.definitions.Add(templateFieldDataTable.col_desc, description);
            output.definitions.Add(templateFieldDataTable.col_caption, displayName);
            return output;
        }

        /// <summary>
        /// Gets the annotation preset item for specified <c>id</c>.
        /// </summary>
        /// <remarks>
        /// If no direct match is found, the method will try with lower case and no space version, and finally it will return <see cref="defaultItem"/> if no match found
        /// </remarks>
        /// <param name="id">The identifier.</param>
        /// <returns>Preset with matched id, or default: <see cref="defaultItem"/></returns>
        public propertyAnnotationPresetItem GetAnnotationPresetItem(String id)
        {
            Boolean isDefined = index.ContainsKey(id);
            if (!isDefined)
            {
                id = id.ToLower();
                isDefined = index.ContainsKey(id);
            }
            if (!isDefined)
            {
                id = id.Replace(" ", "");
                isDefined = index.ContainsKey(id);
            }
            if (isDefined) return index[id];
            return defaultItem;
        }

        /// <summary>
        /// Gets the annotation preset item for specified <c>column</c>.
        /// </summary>
        /// <remarks>
        /// If no direct match is found for column name and caption, the method will try with lower case and no space version, and finally it will return <see cref="defaultItem"/> if no match found
        /// </remarks>
        /// <param name="id">The identifier.</param>
        /// <returns>Preset with matched id, or default: <see cref="defaultItem"/></returns>
        public propertyAnnotationPresetItem GetAnnotationPresetItem(DataColumn column)
        {
            String id = column.ColumnName;

            Boolean isDefined = index.ContainsKey(id);
            if (!isDefined)
            {
                id = id.ToLower();
                isDefined = index.ContainsKey(id);
            }
            if (!isDefined)
            {
                id = column.Caption.ToLower();
                isDefined = index.ContainsKey(id);
            }
            if (!isDefined)
            {
                id = id.Replace(" ", "");
                isDefined = index.ContainsKey(id);
            }
            if (isDefined) return index[id];
            return defaultItem;
        }

        /// <summary>
        /// Lock object - used for tread-safe index lazy initialization
        /// </summary>
        private object _index_lock { get; set; } = new object();

        private Dictionary<string, propertyAnnotationPresetItem> _index;

        /// <summary>
        /// Lazy initialized index
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        private Dictionary<string, propertyAnnotationPresetItem> index
        {
            get
            {
                if (_index != null)
                {
                    if (_index.Count != items.Count) _index = null;
                }
                if (_index == null)
                {
                    lock (_index_lock)
                    {
                        if (_index == null)
                        {
                            _index = new Dictionary<string, propertyAnnotationPresetItem>();

                            foreach (var i in items)
                            {
                                if (!i.name.isNullOrEmpty())
                                {
                                    if (_index.ContainsKey(i.name))
                                    {
                                        _index.Remove(i.name);
                                    }
                                    _index.Add(i.name, i);
                                }
                            }
                        }
                    }
                }

                return _index;
            }
        }
    }
}