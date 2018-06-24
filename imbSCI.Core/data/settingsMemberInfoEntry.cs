// --------------------------------------------------------------------------------------------------------------------
// <copyright file="settingsMemberInfoEntry.cs" company="imbVeles" >
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
namespace imbSCI.Core.data
{
    using imbSCI.Core.attributes;
    using imbSCI.Core.collection;
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.enumworks;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.math.aggregation;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Reflection;
    using System.Xml.Serialization;

    /// <summary>
    /// Base class exposing extensive meta information on <see cref="MemberInfo"/>s, used to access meta annotation for reporting. <seealso cref="settingsPropertyEntry"/>, <seealso cref="settingsPropertyEntryWithContext"/> and <see cref="settingsEntriesForObject"/>.
    /// </summary>
    /// <remarks>The best way to use this model is to call <see cref="settingsEntriesForObject"/> constructor, that will generate this object for each reflected member</remarks>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class settingsMemberInfoEntry : INotifyPropertyChanged
    {
        public String relevantTypeName { get; set; } = "";
        public List<String> additionalInfo { get; set; } = new List<string>();

        public String info_link { get; set; } = "";

        public String info_helpTips { get; set; } = "";

        public String info_helpTitle { get; set; } = "";

        /// <summary>
        /// Gets or sets a value indicating whether this instance is hidden in report.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is hidden in report; otherwise, <c>false</c>.
        /// </value>
        public Boolean isHiddenInReport { get; set; } = false;

        public String name { get; set; } = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public List<String> CategoryByPriority { get; set; } = new List<string>();

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public settingsMemberInfoEntry(Object __primitive)
        {
            displayName = __primitive.toStringSafe().imbTitleCamelOperation(true);

            description = __primitive.GetType().Name;
        }

        /// <summary>
        /// Descriptor info model -- aceTypology
        /// </summary>
        /// <param name="mi">The mi.</param>
        public settingsMemberInfoEntry(MemberInfo mi, PropertyCollectionExtended pce = null)
        {
            process(mi, pce);
        }

        public DescriptionAttribute descAttribute;
        public DisplayNameAttribute displayNameAttribute;
        public CategoryAttribute catAttribute;

        public MemberInfo memberInfo;

        public Boolean IsXmlIgnore { get; set; }

        /// <summary>
        /// Deploys value <c>dtc_value</c> for <c>dtc</c> attribute name
        /// </summary>
        /// <param name="dtc">The DTC.</param>
        /// <param name="dtc_val">The DTC value.</param>
        public void deploy(imbAttributeName dtc, String dtc_val)
        {
            switch (dtc)
            {
                case imbAttributeName.basicColor:
                    color = dtc_val.toStringSafe("");
                    break;

                case imbAttributeName.cacheInstanceId:
                    break;

                case imbAttributeName.codeCacheMethod:
                    break;

                case imbAttributeName.collectionDefaultMemberName:
                    break;

                case imbAttributeName.collectionDefaultParentName:
                    break;

                case imbAttributeName.collectionDisablePrimaryKey:
                    break;

                case imbAttributeName.collectionPrimaryKey:
                    isPrimaryKey = true;
                    break;

                case imbAttributeName.DataTableExport:
                    break;

                case imbAttributeName.dataTemplateOverride:
                    break;

                case imbAttributeName.defineEditor:
                    break;

                case imbAttributeName.diagnosticContentPreset:
                    break;

                case imbAttributeName.diagnosticContentText:
                    break;

                case imbAttributeName.diagnosticEditorOperationInvoke:
                    break;

                case imbAttributeName.diagnosticMode:
                    break;

                case imbAttributeName.diagnosticModuleOperationInvoke:
                    break;

                case imbAttributeName.diagnosticTestMetodInvoke:
                    break;

                case imbAttributeName.diagnosticValue:
                    break;

                case imbAttributeName.editorDisplayOption:
                    break;

                case imbAttributeName.editorFlowViewMode:
                    break;

                case imbAttributeName.editorHideMenu:

                    break;

                case imbAttributeName.editorModulePropertyMode:
                    break;

                case imbAttributeName.editorRelatedModulesMode:
                    break;

                case imbAttributeName.editorToolSize:
                    break;

                case imbAttributeName.evaluation:
                    break;

                case imbAttributeName.evaluationPropertyRole:
                    break;

                case imbAttributeName.evaluationStrictValues:
                    break;

                case imbAttributeName.fileBackupAndRecovery:
                    break;

                case imbAttributeName.fileEncodingType:
                    break;

                case imbAttributeName.fileFormatDefaultSubdir:
                    break;

                case imbAttributeName.fileFormatExtensions:
                    break;

                case imbAttributeName.fileFormatForType:
                    break;

                case imbAttributeName.fileFormatReaderType:
                    break;

                case imbAttributeName.fileFormatWritterType:
                    break;

                case imbAttributeName.fileFormatZippedReaderType:
                    break;

                case imbAttributeName.fileFormatZippedWritterType:
                    break;

                case imbAttributeName.fileProvideByteOrder:
                    break;

                case imbAttributeName.help:
                case imbAttributeName.helpDescription:
                case imbAttributeName.helpPurpose:
                case imbAttributeName.menuHelp:
                    additionalInfo.Add(dtc_val.toStringSafe(""));
                    break;

                case imbAttributeName.helpTips:
                    info_helpTips = dtc_val.toStringSafe("");
                    break;

                case imbAttributeName.helpTitle:

                    info_helpTitle = dtc_val.toStringSafe("");
                    break;

                case imbAttributeName.hideInMenu:
                    break;

                case imbAttributeName.isElementaryModule:
                    break;

                case imbAttributeName.link:
                    break;

                case imbAttributeName.linkPropertyHost:
                    break;

                case imbAttributeName.linkPropertyName:
                    break;

                case imbAttributeName.measure_calcGroup:
                case imbAttributeName.measure_displayGroup:
                case imbAttributeName.menuGroupPath:
                    groups.Add(dtc_val.toStringSafe(""));
                    if (categoryName.isNullOrEmpty()) categoryName = dtc_val.toStringSafe("").ToUpper();
                    break;

                case imbAttributeName.measure_displayGroupDescripton:
                    break;

                case imbAttributeName.measure_expression:
                    expression = dtc_val.toStringSafe("");
                    break;

                case imbAttributeName.measure_important:
                    importance = dataPointImportance.important;
                    if (dtc_val != null)
                    {
                        importance = dtc_val.imbToEnumeration<dataPointImportance>();
                    }
                    break;

                case imbAttributeName.menuCommandKeyboard:
                    letter = letter.or(dtc_val.toStringSafe(letter));
                    break;

                case imbAttributeName.measure_letter:
                case imbAttributeName.menuCommandKey:
                    letter = dtc_val.toStringSafe();
                    break;

                case imbAttributeName.measure_metaModelName:
                    break;

                case imbAttributeName.measure_metaModelPrefix:
                    break;

                case imbAttributeName.measure_operand:
                    break;

                case imbAttributeName.measure_operation:
                    break;

                case imbAttributeName.measure_optimizeUnit:
                    break;

                case imbAttributeName.measure_setAlarm:
                    break;

                case imbAttributeName.measure_setRange:
                    break;

                case imbAttributeName.measure_setRole:

                    break;

                case imbAttributeName.measure_setUnit:
                    unit = dtc_val.toStringSafe("");
                    break;

                case imbAttributeName.measure_target:
                    break;

                case imbAttributeName.menuAskConfirmation:
                    break;

                case imbAttributeName.menuCommandNode:
                    break;

                case imbAttributeName.menuCommandTitle:
                    break;

                case imbAttributeName.menuDelimiter:
                    break;

                case imbAttributeName.menuEnabledSwitch:
                    break;

                case imbAttributeName.menuIcon:
                    break;

                case imbAttributeName.menuPriority:
                    priority = dtc_val.imbToNumber<Int32>();
                    break;

                case imbAttributeName.menuRelevance:
                    break;

                case imbAttributeName.menuRule_AcceptableRole:
                    break;

                case imbAttributeName.menuRule_AcceptableType:
                    break;

                case imbAttributeName.menuSpecialSubMenu:
                    break;

                case imbAttributeName.menuStyle:
                    break;

                case imbAttributeName.menuStyleImportant:
                    break;

                case imbAttributeName.menuStyleNotRecommanded:
                    break;

                case imbAttributeName.menuStyleVIP:
                    break;

                case imbAttributeName.metaData:
                    break;

                case imbAttributeName.metaValueFromAttribute:
                    break;

                case imbAttributeName.neuralInput:
                    break;

                case imbAttributeName.neuralOutput:
                    break;

                case imbAttributeName.notChildrenResource:
                    break;

                case imbAttributeName.postProcess:
                    break;

                case imbAttributeName.preProcess:
                    break;

                case imbAttributeName.rdfClassName:
                    break;

                case imbAttributeName.rdfDefaultCollectionPath:
                    break;

                case imbAttributeName.rdfIdProperty:
                    break;

                case imbAttributeName.rdfPropertyName:
                    break;

                case imbAttributeName.relatedTo:
                    break;

                case imbAttributeName.relationChildUniqueMatchParentProperty:
                    break;

                case imbAttributeName.relationKeyProperty:
                    break;

                case imbAttributeName.relationMakeNewOnNull:
                    break;

                case imbAttributeName.relationTakeAllBackrefference:
                    break;

                case imbAttributeName.relationTakeFirstBackrefference:
                    break;

                case imbAttributeName.reporting_aggregation:
                    break;

                case imbAttributeName.reporting_agregate_function:
                    break;

                case imbAttributeName.reporting_categoryOrder:
                    CategoryByPriority.AddRange(imbSciStringExtensions.SplitSmart(dtc_val.toStringSafe(""), ",", "", true));
                    break;

                case imbAttributeName.reporting_columnWidth:
                    width = dtc_val.imbToNumber<Int32>();
                    break;

                case imbAttributeName.reporting_escapeoff:
                    escapeValueString = dtc_val.imbToBoolean();
                    break;

                case imbAttributeName.reporting_function:
                    break;

                case imbAttributeName.reporting_hide:
                    isHiddenInReport = true;
                    break;

                case imbAttributeName.reporting_template:
                    template = dtc_val.toStringSafe("");
                    break;

                case imbAttributeName.reporting_valueformat:
                    format = dtc_val.toStringSafe("");
                    break;

                case imbAttributeName.selectionAllowSubTypes:
                    break;

                case imbAttributeName.selectionAllowType:
                    break;

                case imbAttributeName.serializationMode:
                    break;

                case imbAttributeName.serializationOff:
                    break;

                case imbAttributeName.serializationOn:
                    break;

                case imbAttributeName.undefined:
                    break;

                case imbAttributeName.viewPriority:
                    priority = dtc_val.imbToNumber<Int32>();
                    break;

                case imbAttributeName.xmlEntityOutput:
                    break;

                case imbAttributeName.xmlEntityOutputAsString:
                    break;

                case imbAttributeName.xmlMapXpath:
                    break;

                case imbAttributeName.xmlNodeTypeName:
                    break;

                case imbAttributeName.xmlNodeValueProperty:
                    break;
            }
        }

        /// <summary>
        /// Deploys the specified <c>dtc_val</c> value, according to <c>dtc</c> property
        /// </summary>
        /// <param name="dtc">The DTC.</param>
        /// <param name="dtc_val">The DTC value.</param>
        public void deploy(templateFieldDataTable dtc, Object dtc_val)
        {
            switch (dtc)
            {
                case templateFieldDataTable.categoryPriority:
                    break;

                case templateFieldDataTable.col_alignment:

                    break;

                case templateFieldDataTable.col_attributes:
                    break;

                case templateFieldDataTable.col_caption:
                    displayName = dtc_val.toStringSafe(displayName);
                    break;

                case templateFieldDataTable.col_color:
                    color = dtc_val.toStringSafe(color);
                    break;

                case templateFieldDataTable.col_desc:
                    description = dtc_val.toStringSafe(description);
                    break;

                case templateFieldDataTable.col_directAppend:
                    escapeValueString = false;

                    break;

                case templateFieldDataTable.col_even:
                    break;

                case templateFieldDataTable.col_expression:
                    expression = dtc_val.toStringSafe(expression);
                    break;

                case templateFieldDataTable.col_format:
                    format = dtc_val.toStringSafe(format);

                    break;

                case templateFieldDataTable.col_group:
                    categoryName = dtc_val.toStringSafe(categoryName);
                    break;

                case templateFieldDataTable.col_hasLinks:
                    escapeValueString = false;
                    break;

                case templateFieldDataTable.col_hasTemplate:

                    break;

                case templateFieldDataTable.col_id:
                    name = dtc_val.toStringSafe(name);
                    break;

                case templateFieldDataTable.col_imbattributes:
                    break;

                case templateFieldDataTable.col_importance:
                    importance = (dataPointImportance)dtc_val;
                    break;

                case templateFieldDataTable.col_letter:
                    letter = dtc_val.toStringSafe(letter);
                    break;

                case templateFieldDataTable.col_name:
                    name = dtc_val.toStringSafe(name);
                    break;

                case templateFieldDataTable.col_nameClean:
                    break;

                case templateFieldDataTable.col_namePrefix:
                    break;

                case templateFieldDataTable.col_pe:
                    break;

                case templateFieldDataTable.col_priority:
                    priority = (Int32)dtc_val;
                    break;

                case templateFieldDataTable.col_propertyInfo:
                    break;

                case templateFieldDataTable.col_rel:
                    break;

                case templateFieldDataTable.col_spe:
                    break;

                case templateFieldDataTable.col_type:
                    break;

                case templateFieldDataTable.col_unit:
                    unit = dtc_val.toStringSafe(unit);
                    break;

                case templateFieldDataTable.col_width:
                    width = dtc_val.imbToNumber<Int32>();
                    break;

                case templateFieldDataTable.columnEncodeMode:
                    break;

                case templateFieldDataTable.columns:
                    break;

                case templateFieldDataTable.columnWidth:
                    width = dtc_val.imbToNumber<Int32>();
                    break;

                case templateFieldDataTable.columnWrapTag:

                    break;

                case templateFieldDataTable.count_format:
                    break;

                case templateFieldDataTable.data_accesslist:
                    break;

                case templateFieldDataTable.data_additional:
                    break;

                case templateFieldDataTable.data_aggregation_type:
                    break;

                case templateFieldDataTable.data_columncount:
                    break;

                case templateFieldDataTable.data_dbengine:
                    break;

                case templateFieldDataTable.data_dbhost:
                    break;

                case templateFieldDataTable.data_dbname:
                    break;

                case templateFieldDataTable.data_dbuser:
                    break;

                case templateFieldDataTable.data_origin_count:
                    break;

                case templateFieldDataTable.data_origin_type:
                    break;

                case templateFieldDataTable.data_query:
                    break;

                case templateFieldDataTable.data_rowcountselected:
                    break;

                case templateFieldDataTable.data_rowcounttotal:
                    break;

                case templateFieldDataTable.data_table:
                    break;

                case templateFieldDataTable.data_tabledesc:
                    break;

                case templateFieldDataTable.data_tablename:
                    break;

                case templateFieldDataTable.data_tablenamedb:
                    break;

                case templateFieldDataTable.data_tablescount:
                    break;

                case templateFieldDataTable.description:
                    description = dtc_val.toStringSafe(description);
                    break;

                case templateFieldDataTable.renderEmptySpace:
                    break;

                case templateFieldDataTable.row_data:
                    break;

                case templateFieldDataTable.row_even:
                    break;

                case templateFieldDataTable.row_id:
                    break;

                case templateFieldDataTable.shema_classname:

                    break;

                case templateFieldDataTable.shema_dbcount:
                    break;

                case templateFieldDataTable.shema_dictionary:
                    break;

                case templateFieldDataTable.shema_sourceinstance:
                    break;

                case templateFieldDataTable.shema_sourcename:
                    break;

                case templateFieldDataTable.table_extraDesc:
                    break;

                case templateFieldDataTable.table_metacolumns:
                    break;

                case templateFieldDataTable.table_metarows:
                    break;

                case templateFieldDataTable.title:
                    displayName = dtc_val.toStringSafe(displayName);
                    break;
            }
        }

        /// <summary>
        /// Deploys values according to values found in attributes
        /// </summary>
        /// <param name="propAttributes">The property attributes.</param>
        public void deployAttributes(Object[] propAttributes)
        {
            foreach (Object propAtt in propAttributes)
            {
                descAttribute = propAtt as DescriptionAttribute;
                if (descAttribute != null)
                {
                    description = descAttribute.Description;
                }

                displayNameAttribute = propAtt as DisplayNameAttribute;
                if (displayNameAttribute != null)
                {
                    displayName = displayNameAttribute.DisplayName;
                }

                catAttribute = propAtt as CategoryAttribute;
                if (catAttribute != null)
                {
                    categoryName = catAttribute.Category.ToUpper();

                    if (categoryName.Contains(","))
                    {
                        groups.AddRange(categoryName.getStringTokens());
                    }
                    else
                    {
                        groups.Add(categoryName);
                    }
                }

                if (propAtt is DisplayAttribute)
                {
                    DisplayAttribute displayAttribute_DisplayAttribute = (DisplayAttribute)propAtt;
                    description += displayAttribute_DisplayAttribute.Description.toStringSafe("");
                    displayName = displayAttribute_DisplayAttribute.Name.toStringSafe(displayName);
                    letter = displayAttribute_DisplayAttribute.ShortName.toStringSafe(letter);
                    // priority = (int)displayAttribute_DisplayAttribute.Order;
                    categoryName = displayAttribute_DisplayAttribute.GroupName.toStringSafe(categoryName);
                }

                if (propAtt is RangeAttribute)
                {
                    RangeAttribute rng = (RangeAttribute)propAtt;
                    range_defined = true;
                    range_min = Convert.ToDouble((object)rng.Minimum);
                    range_max = Convert.ToDouble((object)rng.Maximum);
                }

                if (propAtt is DisplayFormatAttribute)
                {
                    DisplayFormatAttribute dFormat = (DisplayFormatAttribute)propAtt;
                    escapeValueString = dFormat.HtmlEncode;

                    format = dFormat.DataFormatString;
                }

                if (propAtt is DisplayColumnAttribute)
                {
                    DisplayColumnAttribute dColumn = (DisplayColumnAttribute)propAtt;
                    displayName = dColumn.DisplayColumn;
                }

                if (propAtt is XmlIgnoreAttribute)
                {
                    IsXmlIgnore = true;
                }

                if (propAtt is imbAttribute imbAt)
                {
                    switch (imbAt.nameEnum)
                    {
                        case imbAttributeName.DataTableExport:

                            templateFieldDataTable dtc = (templateFieldDataTable)imbAt.objMsg;
                            Object dtc_val = imbAt.objExtra;
                            deploy(dtc, dtc_val);
                            //propAtt_imbAttribute.objExtra

                            break;

                        case imbAttributeName.reporting_aggregation:
                            aggregation[(dataPointAggregationAspect)imbAt.objExtra] = (dataPointAggregationType)imbAt.objMsg;
                            break;
                    }

                    deploy(imbAt.nameEnum, imbAt.getMessage().toStringSafe());

                    if (!attributes.ContainsKey(imbAt.nameEnum))
                    {
                        attributes.Add(imbAt.nameEnum, imbAt);
                    }
                    else
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Processes the specified mi.
        /// </summary>
        /// <param name="mi">The mi.</param>
        /// <param name="pce">The pce.</param>
        protected void process(MemberInfo mi, PropertyCollectionExtended pce = null)
        {
            pe = null;
            name = mi.Name;

            if (mi is MethodBase)
            {
                MethodBase mb = mi as MethodBase;
                relevantTypeName = mb.MemberType.ToString();

                //mb.MemberType
            }

            if (pce != null)
            {
                pe = pce.entries.Get(mi.Name);
            }

            description = "";
            displayName = "";
            memberInfo = mi;
            Object[] propAttributes = mi.GetCustomAttributes(false);
            deployAttributes(propAttributes);

            if (String.IsNullOrEmpty(description))
            {
                if (pe != null)
                {
                    description = pe[PropertyEntryColumn.entry_description].toStringSafe();
                }
                else
                {
                    description = mi.Name;
                }
            }

            if (String.IsNullOrEmpty(displayName))
            {
                if (pe != null)
                {
                    displayName = pe[PropertyEntryColumn.entry_name].toStringSafe();
                }
                else
                {
                    displayName = mi.Name;
                }
            }

            //imbAttributeCollection coll = imbAttributeTools.getImbAttributeDictionary(mi);
            //helpContent = coll.getHelpContent();
        }

        /// <summary>
        /// Help content for this member
        /// </summary>
        /// <value>
        /// The content of the help.
        /// </value>
        public imbHelpContent helpContent { get; set; } = new imbHelpContent();

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary key.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is primary key; otherwise, <c>false</c>.
        /// </value>
        public Boolean isPrimaryKey { get; set; } = false;

        /// <summary>
        /// Letter or code name for this member
        /// </summary>
        /// <value>
        /// The letter.
        /// </value>
        public String letter { get; set; } = "";

        /// <summary>
        /// Hexadecimal color code or name
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public String color { get; set; } = "";

        /// <summary>
        /// Gets or sets a value indicating whether the range is defined.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [range defined]; otherwise, <c>false</c>.
        /// </value>
        public Boolean range_defined { get; set; } = false;

        /// <summary>
        /// Gets or sets the range minimum.
        /// </summary>
        /// <value>
        /// The range minimum.
        /// </value>
        public Double range_min { get; set; } = 0;

        /// <summary>
        /// Gets or sets the range maximum.
        /// </summary>
        /// <value>
        /// The range maximum.
        /// </value>
        public Double range_max { get; set; } = 1;

        #region --- displayName ------- Naziv varijable za prikaz

        private String _displayName = "";

        /// <summary>
        /// Naziv varijable za prikaz
        /// </summary>
        public String displayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = value;
            }
        }

        #endregion --- displayName ------- Naziv varijable za prikaz

        #region --- categoryName ------- Naziv kategorije propertija

        private String _categoryName = "";

        /// <summary>
        /// Naziv kategorije propertija
        /// </summary>
        public String categoryName
        {
            get
            {
                return _categoryName;
            }
            set
            {
                _categoryName = value;
                OnPropertyChanged("categoryName");
            }
        }

        #endregion --- categoryName ------- Naziv kategorije propertija

        #region --- description ------- Opis

        private String _description = "";

        /// <summary>
        /// Opis
        /// </summary>
        public String description
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

        #endregion --- description ------- Opis

        public dataPointAggregationDefinition aggregation { get; set; } = new dataPointAggregationDefinition();

        private imbAttributeCollection _attributes = new imbAttributeCollection();

        /// <summary>
        ///
        /// </summary>
        public imbAttributeCollection attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        private PropertyEntry _pe;

        /// <summary>
        ///
        /// </summary>
        public PropertyEntry pe
        {
            get { return _pe; }
            set { _pe = value; }
        }

        private String _template;

        /// <summary>
        ///property is a String template, this property contains wrapping template where {{{inner}}} is result of member value: template with placeholders {{{property_name}}}
        /// </summary>
        public String template
        {
            get { return _template; }
            set { _template = value; }
        }

        private String _format = "";

        /// <summary>
        /// Value format to apply
        /// </summary>
        public String format
        {
            get { return _format; }
            set { _format = value; }
        }

        private String _expression = "";

        /// <summary>
        ///
        /// </summary>
        public String expression
        {
            get { return _expression; }
            set { _expression = value; }
        }

        public String unit { get; set; } = "";

        /// <summary>
        /// Message when used asked for value of this property
        /// </summary>
        /// <value>
        /// The prompt.
        /// </value>
        public String prompt { get; set; } = "";

        public Boolean allowEdit { get; set; } = true;

        private dataPointImportance _importance = dataPointImportance.none;

        /// <summary>
        ///
        /// </summary>
        public dataPointImportance importance
        {
            get { return _importance; }
            set { _importance = value; }
        }

        private Int32 _priority = 100;

        /// <summary>
        ///
        /// </summary>
        public Int32 priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        public Int32 width { get; set; } = 20;

        private Boolean _escapeValueString = true;

        /// <summary>
        ///
        /// </summary>
        public Boolean escapeValueString
        {
            get { return _escapeValueString; }
            set { _escapeValueString = value; }
        }

        private List<String> _groups = new List<String>();

        /// <summary>Groups this property is asociated with</summary>
        public List<String> groups
        {
            get
            {
                return _groups;
            }
            protected set
            {
                _groups = value;
                OnPropertyChanged("groups");
            }
        }

        [imb(imbAttributeName.metaValueFromAttribute, templateFieldDataTable.col_alignment)]
        public textCursorZoneCorner Alignment { get; set; } = textCursorZoneCorner.none;

        /// <summary>
        /// Exports the property collection.
        /// </summary>
        /// <param name="extraData">The extra data.</param>
        /// <returns></returns>
        public PropertyCollection exportPropertyCollection(PropertyCollection extraData = null)
        {
            if (extraData == null) extraData = new PropertyCollection();

            extraData[imbAttributeName.reporting_valueformat] = format; //*
            extraData[imbAttributeName.measure_displayGroup] = categoryName; //groups.Join(","); //imb.getProperString(me.categoryName, imbAttributeName.measure_displayGroup, imbAttributeName.menuGroupPath); //*
            extraData[imbAttributeName.measure_important] = importance; // imb.getMessage(imbAttributeName.measure_important, false); //*
            if (memberInfo is PropertyInfo)
            {
                PropertyInfo pi = memberInfo as PropertyInfo;

                extraData[templateFieldDataTable.col_type] = pi.PropertyType;
                extraData[templateFieldDataTable.col_propertyInfo] = pi;
            }

            extraData[templateFieldDataTable.col_directAppend] = !escapeValueString; //imb.ContainsKey(imbAttributeName.reporting_escapeoff);
            extraData[templateFieldDataTable.col_attributes] = attributes;
            extraData[templateFieldDataTable.col_propertyInfo] = memberInfo;
            extraData[templateFieldDataTable.col_desc] = description; // imb.getStringLine(Environment.NewLine, imbAttributeName.menuHelp, imbAttributeName.help, imbAttributeName.helpDescription, imbAttributeName.helpPurpose).add(extraData[name_description].toStringSafe(), Environment.NewLine);
            extraData[templateFieldDataTable.col_format] = format; //*
            extraData[templateFieldDataTable.col_group] = categoryName;
            extraData[templateFieldDataTable.col_priority] = priority;
            extraData[templateFieldDataTable.col_pe] = pe;
            extraData[templateFieldDataTable.col_caption] = displayName;
            extraData[templateFieldDataTable.col_name] = name;
            extraData[templateFieldDataTable.col_expression] = expression;
            extraData[templateFieldDataTable.col_imbattributes] = attributes;
            extraData[templateFieldDataTable.col_unit] = unit;
            extraData[templateFieldDataTable.col_color] = color;
            extraData[imbAttributeName.basicColor] = color;

            extraData[templateFieldDataTable.col_width] = width;
            extraData[templateFieldDataTable.col_alignment] = Alignment;

            // extraData[templateFieldDataTable.col_alignment] = attributes;
            //  extraData.AppendData(pe, existingDataMode.overwriteExisting, false);
            return extraData;
        }

        #region --- acceptableValues ------- prihvatljive vrednosti

        private List<Object> _acceptableValues = new List<object>();

        protected settingsMemberInfoEntry()
        {
        }

        /// <summary>
        /// prihvatljive vrednosti
        /// </summary>
        public List<Object> acceptableValues
        {
            get
            {
                return _acceptableValues;
            }
            set
            {
                _acceptableValues = value;
                OnPropertyChanged("acceptableValues");
            }
        }

        #endregion --- acceptableValues ------- prihvatljive vrednosti
    }
}