// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataColumnRenderingSetup.cs" company="imbVeles" >
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
namespace imbSCI.Core.extensions.table
{
    using imbSCI.Core.attributes;
    using imbSCI.Core.data;
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.math.aggregation;
    using imbSCI.Core.style.color;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;

    /// <summary>
    /// Extension controling properties of data column reporting
    /// </summary>
    public static class dataColumnRenderingSetup
    {
        /// <summary>
        /// Gets the groups of columns - grouped by <see cref="SetGroup(DataColumn, string)"/>
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static List<List<DataColumn>> getGroupsOfColumns(this DataTable table)
        {
            Dictionary<String, DataColumn> registry = new Dictionary<string, DataColumn>();

            List<List<DataColumn>> groups = new List<List<DataColumn>>();
            List<DataColumn> group = new List<DataColumn>();

            Boolean startOfTheGroup = false;
            String lastKey = "";
            foreach (DataColumn c in table.Columns)
            {
                String key = c.GetGroup().ToUpper();

                if (lastKey != "" && lastKey != key)
                {
                    startOfTheGroup = true;
                }

                if (startOfTheGroup)
                {
                    if (group.Any())
                    {
                        groups.Add(group);
                    }
                    group = new List<DataColumn>();
                    startOfTheGroup = false;
                }

                group.Add(c);

                lastKey = key;
            }
            groups.Add(group);

            return groups;
        }

        public static Color DefaultBackground(this DataColumn dc, Color default_col_color)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_color))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.col_color, default_col_color);
            }
            return ColorWorks.GetColorSafe(dc.ExtendedProperties[templateFieldDataTable.col_color]);
        }

        public static Color GetDefaultBackground(this DataColumn dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_color))
            {
                return Color.LightGray;
            }
            return ColorWorks.GetColorSafe(dc.ExtendedProperties[templateFieldDataTable.col_color]);
        }

        public static Boolean HasDefaultBackground(this DataColumn dc)
        {
            return dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_color);
        }

        public static DataColumn SetDefaultBackground(this DataColumn dc, String col_color)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.col_color, col_color);
            ///   aceColorConverts.getColorFromHex(col_color));
            return dc;
        }

        public static DataColumn SetDefaultBackground(this DataColumn dc, Color col_color)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.col_color, ColorWorks.ColorToHex(col_color));
            return dc;
        }

        /// <summary>
        /// Formats the specified default col format.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="default_col_format">The default col format.</param>
        /// <returns></returns>
        public static String Format(this DataColumn dc, String default_col_format)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_format))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.col_format, default_col_format);
            }
            return dc.ExtendedProperties[templateFieldDataTable.col_format].toStringSafe();
        }

        /// <summary>
        /// Gets the format.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <returns></returns>
        public static String GetFormat(this DataColumn dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_format))
            {
                return default(String);
            }
            return dc.ExtendedProperties[templateFieldDataTable.col_format].toStringSafe();
        }

        /// <summary>
        /// Gets the format for excel cell
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <returns></returns>
        public static String GetFormatForExcel(this DataColumn dc)
        {
            String format = dc.GetFormat();
            Int32 formatN = 0;
            if (format.isNullOrEmpty()) return null;
            if (format.Contains("P"))
            {
                format = format.Replace("P", "");
                formatN = 0;

                if (!Int32.TryParse(format, out formatN))
                {
                    formatN = 3;
                }
                format = "0." + "0".Repeat(formatN) + "%";
            }
            else if (format.Contains("D"))
            {
                format = format.Replace("D", "");

                if (!Int32.TryParse(format, out formatN))
                {
                    formatN = 3;
                }

                format = "0".Repeat(formatN);
            }
            else if (format.Contains("F"))
            {
                format = format.Replace("F", "");

                if (!Int32.TryParse(format, out formatN))
                {
                    formatN = 3;
                }

                format = "0." + "0".Repeat(formatN);
            }
            else if (format.Contains("T"))
            {
                format = format.Replace("T", "h:mm:ss");
            }
            else
            {
            }

            return format;
        }

        /// <summary>
        /// Sets the format.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="col_format">The col format.</param>
        /// <returns></returns>
        public static DataColumn SetFormat(this DataColumn dc, String col_format)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.col_format, col_format);
            dc.ExtendedProperties.add(imbAttributeName.reporting_valueformat, col_format);
            return dc;
        }

        public static toDosCharactersMode EncodeMode(this DataColumn dc, toDosCharactersMode default_columnEncodeMode)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.columnEncodeMode))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.columnEncodeMode, default_columnEncodeMode);
            }
            return (toDosCharactersMode)dc.ExtendedProperties[templateFieldDataTable.columnEncodeMode];
        }

        public static toDosCharactersMode GetEncodeMode(this DataColumn dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.columnEncodeMode))
            {
                return default(toDosCharactersMode);
            }
            return (toDosCharactersMode)dc.ExtendedProperties[templateFieldDataTable.columnEncodeMode];
        }

        public static DataColumn SetEncodeMode(this DataColumn dc, toDosCharactersMode columnEncodeMode)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.columnEncodeMode, columnEncodeMode);
            return dc;
        }

        public static String WrapTag(this DataColumn dc, String default_columnWrapTag)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.columnWrapTag))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.columnWrapTag, default_columnWrapTag);
            }
            return dc.ExtendedProperties[templateFieldDataTable.columnWrapTag].toStringSafe();
        }

        public static String GetWrapTag(this DataColumn dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.columnWrapTag))
            {
                return default(String);
            }
            return dc.ExtendedProperties[templateFieldDataTable.columnWrapTag].toStringSafe();
        }

        public static DataColumn SetWrapTag(this DataColumn dc, String columnWrapTag)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.columnWrapTag, columnWrapTag);
            return dc;
        }

        /// <summary>
        /// The default column width
        /// </summary>
        public const Int32 DEFAULT_WIDTH = 20;

        public static Int32 Width(this DataColumn dc, Int32 default_columnWidth)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.columnWidth))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.columnWidth, default_columnWidth);
            }
            return dc.ExtendedProperties[templateFieldDataTable.columnWidth].imbConvertValueSafeTyped<Int32>();
        }

        public static Int32 GetWidth(this DataColumn dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.columnWidth))
            {
                return DEFAULT_WIDTH;
            }
            return dc.ExtendedProperties[templateFieldDataTable.columnWidth].imbConvertValueSafeTyped<Int32>();
        }

        public static DataColumn SetWidth(this DataColumn dc, Int32 columnWidth)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.columnWidth, columnWidth);
            return dc;
        }

        /// <summary>
        /// Instructs the rendering engine that the content should be directly appended
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="setValue">if set to <c>true</c> [set value].</param>
        /// <returns></returns>
        public static DataColumn Direct(this DataColumn dc, Boolean setValue = true)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.col_directAppend, setValue);
            return dc;
        }

        /// <summary>
        /// Classes the name.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="default_shema_classname">The default shema classname.</param>
        /// <returns></returns>
        public static String ClassName(this DataColumn dc, String default_shema_classname)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.shema_classname))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.shema_classname, default_shema_classname);
            }
            return dc.ExtendedProperties[templateFieldDataTable.shema_classname].toStringSafe();
        }

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <returns></returns>
        public static String GetClassName(this DataColumn dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.shema_classname))
            {
                return default(String);
            }
            return dc.ExtendedProperties[templateFieldDataTable.shema_classname].toStringSafe();
        }

        /// <summary>
        /// Sets the name of the class.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="shema_classname">The shema classname.</param>
        /// <returns></returns>
        public static DataColumn SetClassName(this DataColumn dc, String shema_classname)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.shema_classname, shema_classname);
            return dc;
        }

        /// <summary>
        /// Spes the specified default col spe.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="default_col_spe">The default col spe.</param>
        /// <returns></returns>
        public static settingsPropertyEntry SPE(this DataColumn dc, settingsPropertyEntry default_col_spe)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_spe))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.col_spe, default_col_spe);
                dc.ExtendedProperties.add(templateFieldDataTable.col_pe, default_col_spe.pe);
                dc.ExtendedProperties.add(templateFieldDataTable.col_propertyInfo, default_col_spe.pi);
            }
            return dc.ExtendedProperties[templateFieldDataTable.col_spe] as settingsPropertyEntry;
        }

        /// <summary>
        /// Gets <see cref="settingsMemberInfoEntry"/>
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <returns></returns>
        public static settingsPropertyEntry GetSPE(this DataColumn dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_spe))
            {
                settingsPropertyEntry spe = new settingsPropertyEntry(dc);
                // if (col_spe.isHiddenInReport) dc.ExtendedProperties.add(imbAttributeName.reporting_hide, true, false);
                dc.SetSPE(spe);
                return spe;
            }
            return dc.ExtendedProperties[templateFieldDataTable.col_spe] as settingsPropertyEntry;
        }

        /// <summary>
        /// Sets and deploys SPE
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="col_spe">The col spe.</param>
        /// <returns></returns>
        public static DataColumn SetSPE(this DataColumn dc, settingsPropertyEntry col_spe)
        {
            dc.SetWidth(col_spe.width);
            dc.SetUnit(col_spe.unit);
            dc.SetDesc(col_spe.description);
            dc.SetGroup(col_spe.categoryName);
            dc.SetDefaultBackground(col_spe.color);
            dc.SetHeading(col_spe.displayName);
            dc.ColumnName = col_spe.name;
            if (col_spe.type.isNullable())
            {
                dc.DataType = typeof(Object);
            }
            else
            {
                dc.DataType = col_spe.type;
            }
            dc.SetFormat(col_spe.format);
            dc.SetLetter(col_spe.letter);
            dc.SetAggregation(col_spe.aggregation);
            dc.SetPriority(col_spe.priority); //<--- prioritet
            dc.SetImportance(col_spe.importance);
            dc.Expression = col_spe.expression;

            dc.ExtendedProperties.add(templateFieldDataTable.col_spe, col_spe);
            dc.ExtendedProperties.add(templateFieldDataTable.col_pe, col_spe.pe);
            dc.ExtendedProperties.add(templateFieldDataTable.col_propertyInfo, col_spe.pi);
            if (col_spe.isHiddenInReport) dc.ExtendedProperties.add(imbAttributeName.reporting_hide, true, false);
            //dc.ExtendedProperties.add(imbAttributeName.reporting_hide);
            return dc;
        }

        /*
        public static DataColumn Importance(this DataColumn dc, dataPointImportance importance)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.col_importance, importance);
            dc.ExtendedProperties.add(imbAttributeName.measure_important, importance);

            return dc;
        } */

        /// <summary>
        /// Sets the column importance.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="default_col_importance">The default col importance.</param>
        /// <returns></returns>
        public static dataPointImportance Importance(this DataColumn dc, dataPointImportance default_col_importance)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_importance))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.col_importance, default_col_importance);
                dc.ExtendedProperties.add(imbAttributeName.measure_important, default_col_importance);
            }
            return (dataPointImportance)dc.ExtendedProperties[templateFieldDataTable.col_importance];
        }

        /// <summary>
        /// Gets the importance.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <returns></returns>
        public static dataPointImportance GetImportance(this DataColumn dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_importance))
            {
                return default(dataPointImportance);
            }
            return (dataPointImportance)dc.ExtendedProperties[templateFieldDataTable.col_importance];
        }

        /// <summary>
        /// Sets the importance.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="col_importance">The col importance.</param>
        /// <returns></returns>
        public static DataColumn SetImportance(this DataColumn dc, dataPointImportance col_importance)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.col_importance, col_importance);
            dc.ExtendedProperties.add(imbAttributeName.measure_important, col_importance);
            return dc;
        }

        public static dataPointAggregationDefinition Aggregation(this DataColumn dc, dataPointAggregationDefinition default_data_aggregation_type)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.data_aggregation_type))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.data_aggregation_type, default_data_aggregation_type);
            }
            return (dataPointAggregationDefinition)dc.ExtendedProperties[templateFieldDataTable.data_aggregation_type];
        }

        public static dataPointAggregationDefinition GetAggregation(this DataColumn dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.data_aggregation_type))
            {
                dataPointAggregationDefinition output = new dataPointAggregationDefinition();
                dc.SetAggregation(output);
                return output;
            }
            return (dataPointAggregationDefinition)dc.ExtendedProperties[templateFieldDataTable.data_aggregation_type];
        }

        public static DataColumn SetAggregation(this DataColumn dc, dataPointAggregationDefinition data_aggregation_type)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.data_aggregation_type, data_aggregation_type);
            return dc;
        }

        /// <summary>
        /// Units the specified default col unit.
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="default_col_unit">The default col unit.</param>
        /// <returns></returns>
        public static String Unit(this DataColumn dc, String default_col_unit)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_unit))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.col_unit, default_col_unit);
            }
            return dc.ExtendedProperties[templateFieldDataTable.col_unit].toStringSafe();
        }

        public static String GetUnit(this DataColumn dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_unit))
            {
                if (dc.DataType == typeof(Int32))
                {
                    return "n";
                }
                if (dc.DataType == typeof(String))
                {
                    return "-";
                }
                return "";
            }
            return dc.ExtendedProperties[templateFieldDataTable.col_unit].toStringSafe();
        }

        public static DataColumn SetUnit(this DataColumn dc, String col_unit)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.col_unit, col_unit);
            return dc;
        }

        /// <summary>
        /// Gets the real Value Type of source DataColumn -- this is NOT same as <see cref="DataColumn.DataType"/>
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="default_col_type">Default type of the col.</param>
        /// <returns></returns>
        public static Type ValueType(this DataColumn dc, Type default_col_type)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_type))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.col_type, default_col_type);
                // dc.DataType = default_col_type;
            }
            return (Type)dc.ExtendedProperties[templateFieldDataTable.col_type];
        }

        /// <summary>
        /// Gets the real Value Type of source DataColumn -- this is NOT same as <see cref="DataColumn.DataType"/>
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <returns></returns>
        public static Type GetValueType(this DataColumn dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_type))
            {
                return typeof(String);
            }
            return (Type)dc.ExtendedProperties[templateFieldDataTable.col_type];
        }

        /// <summary>
        /// Sets the real Value Type of source DataColumn -- this is NOT same as <see cref="DataColumn.DataType" />
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="col_type">Type of the col.</param>
        /// <returns></returns>
        public static DataColumn SetValueType(this DataColumn dc, Type col_type)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.col_type, col_type);
            dc.DataType = typeof(Object);

            //if (col_type.checkIfDataTypeIsAllowed())
            //{
            //    dc.DataType = col_type;
            //} else
            //{
            //    dc.DataType = typeof(String);
            //}

            //dc.DataType = col_type;
            return dc;
        }

        /// <summary>
        /// Sets the real Value Type of source DataColumn -- this is NOT same as <see cref="DataColumn.DataType" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc">The dc.</param>
        /// <returns></returns>
        public static DataColumn SetValueType<T>(this DataColumn dc)
        {
            Type col_type = typeof(T);
            dc.ExtendedProperties.add(templateFieldDataTable.col_type, col_type);

            dc.DataType = typeof(Object);

            //if (col_type.checkIfDataTypeIsAllowed())
            //{
            //    dc.DataType = col_type;
            //}
            //else
            //{
            //    dc.DataType = typeof(String);
            //}
            //dc.DataType = typeof(Object);
            //dc.DataType = col_type;
            return dc;
        }

        public static Int32 Priority(this DataColumn dc, Int32 default_col_priority)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_priority))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.col_priority, default_col_priority);
            }
            return (Int32)dc.ExtendedProperties[templateFieldDataTable.col_priority];
        }

        public static Int32 GetPriority(this DataColumn dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_priority))
            {
                return dc.Ordinal;
            }
            return (Int32)dc.ExtendedProperties[templateFieldDataTable.col_priority];
        }

        public static DataColumn SetPriority(this DataColumn dc, Int32 col_priority)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.col_priority, col_priority);

            return dc;
        }

        public static Boolean HasLinks(this DataColumn dc, Boolean default_col_hasLinks)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_hasLinks))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.col_hasLinks, default_col_hasLinks);
            }
            return dc.ExtendedProperties[templateFieldDataTable.col_hasLinks].imbToBoolean();
        }

        public static Boolean GetHasLinks(this DataColumn dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_hasLinks))
            {
                return default(Boolean);
            }
            return dc.ExtendedProperties[templateFieldDataTable.col_hasLinks].imbToBoolean();
        }

        public static DataColumn SetHasLinks(this DataColumn dc, Boolean col_hasLinks)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.col_hasLinks, col_hasLinks);
            return dc;
        }

        public static Boolean HasTemplate(this DataColumn dc, Boolean default_col_hasTemplate)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_hasTemplate))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.col_hasTemplate, default_col_hasTemplate);
            }
            return dc.ExtendedProperties[templateFieldDataTable.col_hasTemplate].imbToBoolean();
        }

        public static Boolean GetHasTemplate(this DataColumn dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_hasTemplate))
            {
                return default(Boolean);
            }
            return dc.ExtendedProperties[templateFieldDataTable.col_hasTemplate].imbToBoolean();
        }

        public static DataColumn SetHasTemplate(this DataColumn dc, Boolean col_hasTemplate)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.col_hasTemplate, col_hasTemplate);
            return dc;
        }

        /// <summary>
        /// Defines the letter/code associated with the table column
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="default_col_letter">The default col letter.</param>
        /// <returns></returns>
        public static String Letter(this DataColumn dc, String default_col_letter)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_letter))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.col_letter, default_col_letter);
            }
            return dc.ExtendedProperties[templateFieldDataTable.col_letter].toStringSafe();
        }

        public static String GetLetter(this DataColumn dc)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_letter))
            {
                return default(String);
            }
            return dc.ExtendedProperties[templateFieldDataTable.col_letter].toStringSafe();
        }

        public static DataColumn SetLetter(this DataColumn dc, String col_letter)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.col_letter, col_letter);

            return dc;
        }

        public static String GetDesc(this DataColumn dc)
        {
            return dc.ExtendedProperties.getProperString(templateFieldDataTable.col_desc);
        }

        public static DataColumn SetDesc(this DataColumn dc, String description)
        {
            dc.ExtendedProperties.add(templateFieldDataTable.col_desc, description);
            return dc;
        }

        public static String GetHeading(this DataColumn dc)
        {
            if (dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_caption))
            {
                return dc.ExtendedProperties[templateFieldDataTable.col_caption] as String;
            }
            if (dc.Caption.isNullOrEmpty()) dc.Caption = dc.ColumnName.imbTitleCamelOperation();

            return dc.Caption; //.ExtendedProperties.getProperString(dc.ColumnName.imbTitleCamelOperation(), templateFieldDataTable.col_caption);
        }

        /// <summary>
        /// Sets the heading content of the column
        /// </summary>
        /// <param name="dc">The dc.</param>
        /// <param name="caption">The caption.</param>
        /// <returns></returns>
        public static DataColumn SetHeading(this DataColumn dc, String caption)
        {
            if (dc.ColumnName.isNullOrEmpty()) dc.ColumnName = caption.getFilename();
            dc.Caption = caption;
            dc.ExtendedProperties.add(templateFieldDataTable.col_caption, caption);
            return dc;
        }

        public static String Group(this DataColumn dc, String default_col_group)
        {
            if (!dc.ExtendedProperties.ContainsKey(templateFieldDataTable.col_group))
            {
                dc.ExtendedProperties.add(templateFieldDataTable.col_group, default_col_group);
            }
            return dc.ExtendedProperties[templateFieldDataTable.col_group].toStringSafe();
        }

        public static String GetGroup(this DataColumn dc)
        {
            return dc.ExtendedProperties.getProperString(templateFieldDataTable.col_group, imbAttributeName.measure_calcGroup, imbAttributeName.measure_displayGroup).toStringSafe("Data").ToUpper();
        }

        public static DataColumn SetGroup(this DataColumn dc, String col_group)
        {
            if (col_group == null) col_group = "";
            col_group = col_group.ToUpper();
            dc.ExtendedProperties.add(templateFieldDataTable.col_group, col_group);

            dc.ExtendedProperties.add(imbAttributeName.measure_displayGroup, col_group);

            return dc;
        }
    }
}