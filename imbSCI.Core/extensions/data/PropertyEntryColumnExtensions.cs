// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyEntryColumnExtensions.cs" company="imbVeles" >
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
namespace imbSCI.Core.extensions.data
{
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    public static class imbDataExtensions
    {
        /// <summary>
        /// Iterprets one CSV line into list of Strings
        /// </summary>
        /// <param name="input"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<string> fromCsvInLine(this String input, String separator = ",")
        {
            List<String> output = new List<string>();
            String[] cs = input.Split(separator.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (String c in cs)
            {
                output.Add(c);
            }
            return output;
        }

        /// <summary>
        /// Renders CSV line from <see cref="object.toStringSafe()"/>, <see cref="separator"/> is removed from item String
        /// </summary>
        /// <param name="input">Collection of strings or objects that support ToString() </param>
        /// <param name="separator">Separator between items</param>
        /// <param name="propertyName">Name of property to read -- if empty object is used directly</param>
        /// <returns>One string</returns>
        public static String toCsvInLine(this IEnumerable input, String separator = ",", String propertyName = "")
        {
            StringBuilder output = new StringBuilder();
            if (input == null)
            {
                return "null";
            }
            Int32 c = 0;
            foreach (object item in input)
            {
                if (item is String)
                {
                    //output = output.add(item as String, separator);
                    output.Append((String)item); //, separator);
                    output.Append(separator);
                }
                else
                {
                    String vl = "";
                    if (!imbSciStringExtensions.isNullOrEmptyString(propertyName))
                    {
                        output.Append(item.imbGetPropertySafe(propertyName, "").ToString());
                    }
                    else
                    {
                        output.Append(item.toStringSafe().Replace(separator, ""));
                        //output = output.add(item.ToString(), separator);
                    }
                    output.Append(separator);
                }
            }

            return imbSciStringExtensions.removeEndsWith(output.ToString(), separator);
        }
    }

    /// <summary>
    /// Extenisons for PropertyEntryColumn
    /// </summary>
    public static class PropertyEntryColumnExtensions
    {
        public static Boolean isKeyValueColumn(this PropertyEntryColumn key)
        {
            if (key is PropertyEntryColumn)
            {
                PropertyEntryColumn key_PropertyEntryColumn = (PropertyEntryColumn)key;

                switch (key_PropertyEntryColumn)
                {
                    case PropertyEntryColumn.entry_key:
                    case PropertyEntryColumn.entry_value:
                    case PropertyEntryColumn.entry_valueAndUnit:
                        return true;
                        break;
                }
            }

            return false;
        }

        public static Boolean isCoreColumn(this Object key)
        {
            if (key is PropertyEntryColumn)
            {
                PropertyEntryColumn key_PropertyEntryColumn = (PropertyEntryColumn)key;

                switch (key_PropertyEntryColumn)
                {
                    case PropertyEntryColumn.entry_key:
                    case PropertyEntryColumn.entry_description:
                    case PropertyEntryColumn.entry_name:
                    case PropertyEntryColumn.entry_unit:
                        return true;
                        break;
                }
            }

            return false;
        }

        /// <summary>
        /// To the column caption.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static String toColumnCaption(this PropertyEntryColumn column, Boolean showException = true)
        {
            switch (column)
            {
                case PropertyEntryColumn.entry_key:
                    return "Key";
                    break;

                case PropertyEntryColumn.entry_name:
                    return "Name";
                    break;

                case PropertyEntryColumn.entry_description:
                    return "Description";
                    break;

                case PropertyEntryColumn.entry_value:
                    return "Value";
                    break;

                case PropertyEntryColumn.entry_valueAndUnit:
                    return "Value";
                    break;

                case PropertyEntryColumn.role_letter:
                    return "Letter";
                    break;

                case PropertyEntryColumn.role_symbol:
                    return "Symbol";
                    break;

                case PropertyEntryColumn.role_name:
                    return "Role";
                    break;

                case PropertyEntryColumn.entry_unit:
                    return "Unit";
                    break;

                case PropertyEntryColumn.entry_unitname:
                    return "Unit";
                    break;

                case PropertyEntryColumn.entry_importance:
                    return "Relevance";
                    break;

                case PropertyEntryColumn.none:
                    return " -- ";
                    break;

                default:
                    if (showException)
                    {
                        throw new ArgumentOutOfRangeException(nameof(column), "toColumnCaption failed to find string for [" + column.ToString() + "] in PropertyEntryColumnExtensions - toColumnCaption()");
                    }
                    return column.ToString();
                    break;
            }
            return column.ToString();
        }

        public static Type toColumnType(this PropertyEntryColumn column)
        {
            switch (column)
            {
                case PropertyEntryColumn.autocount_idcolumn:
                case PropertyEntryColumn.entry_value:
                    return typeof(Int32);
                    break;

                default:
                    return typeof(String);
                    break;
            }
        }

        /// <summary>
        /// To the column caption.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static Int32 toColumnPriority(this PropertyEntryColumn column)
        {
            switch (column)
            {
                case PropertyEntryColumn.entry_name:
                case PropertyEntryColumn.entry_key:
                    return 10;
                    break;

                    break;

                case PropertyEntryColumn.entry_description:
                    return 150;
                    break;

                case PropertyEntryColumn.entry_value:
                    return 50;
                    break;

                case PropertyEntryColumn.entry_valueAndUnit:
                    return 90;

                    break;

                case PropertyEntryColumn.role_letter:
                    return 110;

                    break;

                case PropertyEntryColumn.role_symbol:
                    return 120;

                    break;

                case PropertyEntryColumn.role_name:
                    return 130;
                    break;

                case PropertyEntryColumn.entry_unit:
                case PropertyEntryColumn.entry_unitname:
                    return 110;
                    break;

                case PropertyEntryColumn.entry_importance:
                    return 200;
                    break;

                default:
                    return 100;
            }
            return 100;
        }

        public static String getDescriptionForKey(this PropertyEntryColumn column)
        {
            String desc = "";

            switch (column)
            {
                case PropertyEntryColumn.entry_key:
                    desc = "Internaly used property name - in source code, object model, database, xml, rdf ... ";
                    break;

                case PropertyEntryColumn.entry_name:

                    desc = "Human-readable name of the property";
                    break;

                case PropertyEntryColumn.entry_description:

                    desc = "About / info on the property";
                    break;

                case PropertyEntryColumn.entry_value:

                    desc = "Formated value";
                    break;

                case PropertyEntryColumn.entry_valueAndUnit:

                    desc = "Formated value with unit sufix";
                    break;

                case PropertyEntryColumn.role_letter:

                    desc = "Letter asociated with the role of the property";
                    break;

                case PropertyEntryColumn.role_symbol:

                    desc = "Symbol associated with the role of the property";
                    break;

                case PropertyEntryColumn.role_name:

                    desc = "Descriptive name of the property role";
                    break;

                case PropertyEntryColumn.entry_unit:

                    desc = "The unit short name - abbr./sufix";
                    break;

                case PropertyEntryColumn.entry_unitname:

                    desc = "Name of the property unit";
                    break;

                case PropertyEntryColumn.entry_importance:
                    desc = "Remarks on property relevance and/or on alarming value";

                    break;

                default:
                    return desc.ToString();
                    //imbTypology
                    //desc =
                    break;
            }
            return desc;
        }

        /// <summary>
        /// Sets the data column according to PropertyEntryColumn
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="tableColumn">The table column.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static void setDataColumn(this PropertyEntryColumn column, DataColumn tableColumn)
        {
            tableColumn.Caption = column.toColumnCaption(false);
            //tableColumn.SetHeading(column.toColumnCaption(false));
            tableColumn.ColumnName = "_" + column.ToString();

            tableColumn.ExtendedProperties[templateFieldStyling.text_horizontalAligment] = printHorizontal.left;

            tableColumn.ExtendedProperties[templateFieldStyling.render_isHidden] = false;

            tableColumn.ExtendedProperties[templateFieldDataTable.col_name] = column;

            //tableColumn.ExtendedProperties[templateFieldStyling.color_variationRole] = acePaletteVariationRole.none;
            tableColumn.ExtendedProperties[templateFieldStyling.color_variationAdjustment] = 0;

            String desc = column.getDescriptionForKey();

            switch (column)
            {
                case PropertyEntryColumn.entry_key:
                    tableColumn.DataType = typeof(String);

                    break;

                case PropertyEntryColumn.entry_name:
                    tableColumn.DataType = typeof(String);
                    tableColumn.ExtendedProperties[templateFieldStyling.color_variationAdjustment] = 1;
                    //desc = "Human-readable name of the property";
                    break;

                case PropertyEntryColumn.property_description:
                case PropertyEntryColumn.entry_description:
                    tableColumn.DataType = typeof(String);
                    //desc = "About / info on the property";
                    break;

                case PropertyEntryColumn.entry_value:
                    tableColumn.DataType = typeof(String);
                    tableColumn.ExtendedProperties[templateFieldStyling.color_variationAdjustment] = 2;
                    tableColumn.ExtendedProperties[templateFieldStyling.text_horizontalAligment] = printHorizontal.right;
                    // desc = "Formated value";
                    break;

                case PropertyEntryColumn.entry_valueAndUnit:
                    tableColumn.DataType = typeof(String);

                    tableColumn.ExtendedProperties[templateFieldStyling.text_horizontalAligment] = printHorizontal.right;
                    tableColumn.ExtendedProperties[templateFieldStyling.color_variationAdjustment] = 2;
                    // desc = "Formated value with unit sufix";
                    break;

                case PropertyEntryColumn.role_letter:
                    tableColumn.DataType = typeof(String);
                    // desc = "Letter asociated with the role of the property";
                    break;

                case PropertyEntryColumn.role_symbol:
                    tableColumn.DataType = typeof(String);
                    // desc = "Symbol associated with the role of the property";
                    break;

                case PropertyEntryColumn.role_name:
                    tableColumn.DataType = typeof(String);
                    // desc = "Descriptive name of the property role";
                    break;

                case PropertyEntryColumn.entry_unit:
                    tableColumn.DataType = typeof(String);
                    tableColumn.ExtendedProperties[templateFieldStyling.color_variationAdjustment] = 1;
                    // desc = "The unit short name - abbr./sufix";
                    break;

                case PropertyEntryColumn.entry_unitname:
                    tableColumn.DataType = typeof(String);
                    // desc = "Name of the property unit";
                    break;

                case PropertyEntryColumn.entry_importance:
                    // desc = "Remarks on property relevance and/or on alarming value";
                    tableColumn.DataType = typeof(String);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            tableColumn.ExtendedProperties[templateFieldDataTable.col_desc] = desc;
            // return column.ToString();
        }
    }
}