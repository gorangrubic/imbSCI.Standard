// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyEntry.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.interfaces;
    using imbSCI.Data;
    using imbSCI.Data.enums.fields;
    using imbSCI.Data.interfaces;
    using System;
    using System.Data;

    /// <summary>
    /// Meta information about property/entry in <see cref="PropertyCollectionExtended"/>
    /// </summary>
    /// <seealso cref="System.Data.PropertyCollection" />
    /// <seealso cref="PropertyCollectionExtended"/>
    /// <seealso cref="PropertyEntryDictionary"/>
    /// <seealso cref="PropertyEntryDictionary"/>
    public class PropertyEntry : PropertyCollection
    {
        private Int32 _priority;

        /// <summary>
        ///
        /// </summary>
        public Int32 priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private String _keyName;

        /// <summary>
        ///
        /// </summary>
        public String keyName
        {
            get
            {
                if (_keyName.isNullOrEmptyString())
                {
                    _keyName = this[PropertyEntryColumn.entry_key].toStringSafe().Replace(" ", ""); //.getCleanPropertyName();
                }
                return _keyName;
            }
            set { _keyName = value; }
        }

        private void add(Object key, Object value, Boolean replace = true)
        {
            if (ContainsKey(key))
            {
                if (replace) this[key] = value;
            }
            else
            {
                Add(key, value);
            }
        }

        public PropertyEntry Set(PropertyEntryColumn column, Object value)
        {
            //this[templateFieldDataTable.col_type] = value;
            add(column, value, false);
            return this;
        }

        public PropertyEntry SetUnit(String value)
        {
            // this[PropertyEntryColumn.entry_unit] = value;
            this.add(PropertyEntryColumn.entry_unit, value, false);
            return this;
        }

        public PropertyEntry SetLetter(String value)
        {
            //  this[PropertyEntryColumn.role_letter] = value;
            this.add(PropertyEntryColumn.role_letter, value, false);
            return this;
        }

        public PropertyEntry SetType(Type value)
        {
            // this[templateFieldDataTable.col_type] = value;
            this.add(templateFieldDataTable.col_type, value, false);
            return this;
        }

        public PropertyEntry SetFormat(String value)
        {
            //this[templateFieldDataTable.col_format] = value;
            this.add(templateFieldDataTable.col_format, value, false);
            return this;
        }

        /// <summary>
        /// Gets the columm value - applying special procedure for certain <c>PropertyEntryColumn</c>
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public Object getColumn(PropertyEntryColumn column, Object input)
        {
            if (imbSciStringExtensions.isNullOrEmptyString(input)) input = "";
            switch (column)
            {
                case PropertyEntryColumn.entry_importance:

                    if (input is IValueWithImportanceInfo)
                    {
                        IValueWithImportanceInfo input_IValueWithImportanceInfo = (IValueWithImportanceInfo)input;
                        if (input_IValueWithImportanceInfo.isValueInAlarmRange)
                        {
                            return dataPointImportance.alarm;
                        }
                    }

                    return this[PropertyEntryColumn.entry_importance];

                    break;

                case PropertyEntryColumn.entry_value:

                    if (input is IValueWithToString)
                    {
                        IValueWithToString input_IValueWithToString = (IValueWithToString)input;
                        return input_IValueWithToString.GetFormatedValue();
                    }

                    return input.toStringSafe();

                    break;

                case PropertyEntryColumn.entry_valueAndUnit:
                    if (input is IValueWithToString)
                    {
                        IValueWithToString input_IValueWithToString = (IValueWithToString)input;
                        return input_IValueWithToString.GetFormatedValueAndUnit();
                    }

                    return imbSciStringExtensions.add(input.toStringSafe(), this[PropertyEntryColumn.entry_unit].ToString(), " ");

                    break;

                default:
                    return this[column];
                    break;
            }

            return "";
        }

        public override object this[object key]
        {
            get
            {
                if (!ContainsKey(key))
                {
                    Add(key, "");
                }
                return base[key];
            }

            set
            {
                if (!ContainsKey(key))
                {
                    Add(key, value);
                }
                else
                {
                    base[key] = value;
                }
            }
        }

        /// <summary>
        /// Key associated with the entry
        /// </summary>
        public Object EntryKey
        {
            get { return this[PropertyEntryColumn.entry_key]; }
            set { this[PropertyEntryColumn.entry_key] = value; }
        }

        /// <summary>
        /// Gets or sets the value associated with this property
        /// </summary>
        /// <value>
        /// The entry value1.
        /// </value>
        public Object EntryValue
        {
            get { return this[PropertyEntryColumn.entry_value]; }
            set { this[PropertyEntryColumn.entry_value] = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyEntry"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="name">Initial value for Display Name</param>
        /// <param name="description">Initial description - overide of description found in <c>value</c> object</param>
        /// <param name="relevance">The relevance - override of info found in <c>value</c></param>
        /// <seealso cref="imbSCI.Core.interfaces.IValueWithRoleInfo"/>
        /// <seealso cref="imbSCI.Core.interfaces.IValueWithUnitInfo"/>
        /// <seealso cref="imbSCI.Core.interfaces.IValueWithImportanceInfo"/>
        /// <seealso cref="IObjectWithNameAndDescription"/>
        public PropertyEntry(Object key, Object value, String name = "", String description = "", dataPointImportance relevance = dataPointImportance.normal)
        {
            // this.setFromEnumType(typeof(PropertyEntryColumn));

            this[PropertyEntryColumn.entry_key] = key;

            Object input = value;

            this[PropertyEntryColumn.entry_name] = name;
            this[PropertyEntryColumn.entry_description] = description;
            this[PropertyEntryColumn.entry_importance] = relevance.ToString();

            this[PropertyEntryColumn.entry_value] = value;

            if (value is IValueWithUnitInfo)
            {
                IValueWithUnitInfo value_IValueWithUnitInfo = (IValueWithUnitInfo)value;
                this[PropertyEntryColumn.entry_unit] = value_IValueWithUnitInfo.unit_sufix;
                this[PropertyEntryColumn.entry_unitname] = value_IValueWithUnitInfo.unit_name;
            }

            if (input is IValueWithRoleInfo)
            {
                IValueWithRoleInfo input_IValueWithRoleInfo = (IValueWithRoleInfo)input;
                this[PropertyEntryColumn.role_letter] = input_IValueWithRoleInfo.role_letter;
                this[PropertyEntryColumn.role_name] = input_IValueWithRoleInfo.role_name;
                this[PropertyEntryColumn.role_symbol] = input_IValueWithRoleInfo.role_symbol;
            }

            if (input is IObjectWithNameAndDescription)
            {
                IObjectWithNameAndDescription input_IObjectWithNameAndDescription = (IObjectWithNameAndDescription)input;

                this[PropertyEntryColumn.entry_name] = input_IObjectWithNameAndDescription.name;
                this[PropertyEntryColumn.entry_description] = input_IObjectWithNameAndDescription.description;
            }

            if (input is IValueWithImportanceInfo)
            {
                IValueWithImportanceInfo input_IValueWithImportanceInfo = (IValueWithImportanceInfo)input;
                this[PropertyEntryColumn.entry_importance] = input_IValueWithImportanceInfo.relevance.ToString();
            }

            if (imbSciStringExtensions.isNullOrEmptyString(this[PropertyEntryColumn.entry_name]))
            {
                this[PropertyEntryColumn.entry_name] = key.ToString().imbTitleCamelOperation(true);
            }

            if (imbSciStringExtensions.isNullOrEmptyString(this[PropertyEntryColumn.entry_description]))
            {
                //this[PropertyEntryColumn.entry_description] = this.  this.getStringLine(",", PropertyEntryColumn.role_name, PropertyEntryColumn.entry_unitname, PropertyEntryColumn.role_letter);
            }
        }

        public PropertyEntry relevance(dataPointImportance level)
        {
            this[PropertyEntryColumn.entry_importance] = level.ToString();
            return this;
        }
    }
}