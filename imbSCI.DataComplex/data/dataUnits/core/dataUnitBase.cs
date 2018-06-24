// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataUnitBase.cs" company="imbVeles" >
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

//using imbAnalyticsEngine.webProfile.profiles;

namespace imbSCI.DataComplex.data.dataUnits.core
{
    using imbSCI.Core.attributes;
    using imbSCI.Core.collection;
    using imbSCI.Core.data;
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Data;
    using imbSCI.Data.data;
    using imbSCI.Data.enums.fields;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex.data.dataUnits.enums;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// dataUnit contains a set of properties that should be extracted and delivered to reporting module
    /// </summary>
    public abstract class dataUnitBase : imbBindable
    {
        protected IEnumerable _items;

        protected dataUnitPresenter _complete_Table;

        /// <complete>Defines Table that is showint all properties having "complete" in Category description</complete>
        public abstract dataUnitPresenter complete_Table { get; }

        /// <summary> </summary>
        public dataUnitPresenterDictionary presenters { get; protected set; } = new dataUnitPresenterDictionary();

        private dataUnitPresenter _globalAttachmentProvider;

        /// <summary> </summary>
        public dataUnitPresenter globalAttachmentProvider
        {
            get
            {
                return _globalAttachmentProvider;
            }
            protected set
            {
                _globalAttachmentProvider = value;
                OnPropertyChanged("globalAttachmentProvider");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public DataTable globalAttachmentTable { get; set; }

        private dataUnitMap _map;

        /// <summary> </summary>
        public dataUnitMap map
        {
            get
            {
                return _map;
            }
            protected set
            {
                _map = value;
                OnPropertyChanged("map");
            }
        }

        private int _lastIteration = -1;

        /// <summary> </summary>
        public int lastIteration
        {
            get
            {
                return _lastIteration;
            }
            protected set
            {
                _lastIteration = value;
                OnPropertyChanged("lastIteration");
            }
        }

        protected void prepare()
        {
            map.monitor.prepare();
        }

        public void dataImportComplete()
        {
            tableShema = buildDataTableShema(complete_Table);

            bool hasGlobalAttachment = presenters.items.Any(x => x.Value.attachmentFlags.HasFlag(dataDeliverFormatEnum.globalAttachment));

            if (hasGlobalAttachment)
            {
                globalAttachmentProvider = presenters.items.First(x => x.Value.attachmentFlags.HasFlag(dataDeliverFormatEnum.globalAttachment)).Value;
                globalAttachmentTable = buildCustomDataTable(_items, globalAttachmentProvider, globalAttachmentProvider.formatFlags.HasFlag(dataDeliverFormatEnum.collectionLimitForAttachment));
            }

            foreach (KeyValuePair<object, dataUnitPresenter> pair in presenters.items)
            {
            }
        }

        /// <summary>
        /// Builds the custom data table.
        /// </summary>
        /// <param name="instance_items">The instance items.</param>
        /// <param name="presenter">The presenter.</param>
        /// <param name="isPreviewTable">if set to <c>true</c> [is preview table].</param>
        /// <returns></returns>
        public DataTable buildCustomDataTable(IEnumerable instance_items, dataUnitPresenter presenter, bool isPreviewTable)
        {
            if (presenter == complete_Table)
            {
                if (tableShema.Columns.Count == 0) tableShema = buildDataTableShema(presenter);
            }
            DataTable selectedShema = selectDataTableShema(tableShema, presenter);
            DataTable output = selectedShema;

            switch (presenter.presentationType)
            {
                case dataDeliveryPresenterTypeEnum.tableVertical:

                    output = new DataTable(presenter.name);
                    output.ExtendedProperties.copyInto(selectedShema.ExtendedProperties);

                    List<DataColumn> value_columns = new List<DataColumn>();
                    List<PropertyEntryColumn> extra_columns = new List<PropertyEntryColumn>();
                    //addStandardColumnShema(output, presenter, PropertyEntryColumn.entry_name);

                    PropertyCollectionExtended pce = null;
                    int i = 0;
                    foreach (object instance in instance_items)
                    {
                        pce = new PropertyCollectionExtended();
                        pce.setFromObject(instance);

                        var dc = addStandardColumnShema(output, presenter, PropertyEntryColumn.entry_value);

                        if (instance is IObjectWithName)
                        {
                            IObjectWithName input_IObjectWithName = (IObjectWithName)instance;
                            dc.Caption = input_IObjectWithName.name;
                        }

                        if (instance is IObjectWithDescription)
                        {
                            IObjectWithDescription instanceIObjectWithDescription = (IObjectWithDescription)instance;
                            dc.ExtendedProperties[templateFieldDataTable.col_desc] = instanceIObjectWithDescription.description;
                        }

                        dc.ExtendedProperties[imbAttributeName.menuRelevance] = dataPointImportance.important;
                        dc.ExtendedProperties[imbAttributeName.metaData] = pce;

                        dc.ColumnName = "value_" + i.ToString();
                        value_columns.Add(dc);

                        i++;
                    }

                    addStandardColumnShema(output, presenter, PropertyEntryColumn.entry_description);

                    foreach (PropertyEntryColumn epec in presenter.extraColumns.getEnumListFromFlags<PropertyEntryColumn>())
                    {
                        if ((epec != PropertyEntryColumn.entry_name) && (epec != PropertyEntryColumn.entry_description))
                        {
                            extra_columns.Add(epec);
                            addStandardColumnShema(output, presenter, epec);
                        }
                    }

                    // <----------------------------------- izgradnja redova

                    foreach (string cns in map.fieldsByNeedle[presenter.key])
                    {
                        DataColumn dcShema = null;

                        if (selectedShema.Columns.Contains(cns)) dcShema = selectedShema.Columns[cns];

                        DataRow dr = output.NewRow();

                        // dr[output.Columns[PropertyEntryColumn.entry_name.ToString()]] = dcShema.Caption;

                        int vi = 0;
                        object val = null;
                        foreach (object instance in instance_items)
                        {
                            var dc = value_columns[vi];
                            dr[dc] = instance.imbGetPropertySafe(cns, "--");

                            if (dr[dc] != null) val = dr[dc];

                            vi++;
                        }

                        //dr[output.Columns[PropertyEntryColumn.entry_description.ToString()]] = dcShema.ExtendedProperties[templateFieldDataTable.col_desc];

                        if (extra_columns.Any())
                        {
                            foreach (PropertyEntryColumn epec in extra_columns)
                            {
                                dr[output.Columns[epec.ToString()]] = pce[output.Columns[epec.ToString()]];
                            }
                        }
                    }

                    break;

                case dataDeliveryPresenterTypeEnum.tableTwoColumnParam:
                    object ins = instance_items.imbGetItemAt(0);

                    PropertyCollectionExtended pc2 = new PropertyCollectionExtended(); // instance.buildPropertyCollection(true, true, "");
                                                                                       //pc2.setFromObject(ins);

                    // pc2 = ins.buildPropertyCollection<PropertyCollectionExtended>(doOnlyWithDisplayName:false, doInherited:true, filedName_prefix:"", output:null, fieldsOrCategoriesToShow:map.fieldsByNeedle[presenter.key]);

                    foreach (var pairs in pc2.entries)
                    {
                    }

                    break;

                case dataDeliveryPresenterTypeEnum.lineGraph:
                case dataDeliveryPresenterTypeEnum.pieGraph:
                case dataDeliveryPresenterTypeEnum.barGraph:
                case dataDeliveryPresenterTypeEnum.tableHorizontal:

                    output = getDataTable(instance_items, presenter, isPreviewTable, false);
                    break;
            }

            return output;
        }

        /// <summary>
        /// Gets the data table fpr presenter
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="presenter">The presenter.</param>
        /// <param name="isPreviewTable">if set to <c>true</c> [is preview table].</param>
        /// <param name="runMonitorCheck">if set to <c>true</c> [run monitor check].</param>
        /// <returns></returns>
        internal virtual DataTable getDataTable(IEnumerable items, dataUnitPresenter presenter, bool isPreviewTable, bool runMonitorCheck = false)
        {
            if (presenter == complete_Table)
            {
                if (tableShema.Columns.Count == 0) tableShema = buildDataTableShema(presenter);
            }

            DataTable output = selectDataTableShema(tableShema, presenter);
            if (runMonitorCheck) map.monitor.prepare();
            int rowLimit = -1;

            if (isPreviewTable) rowLimit = output.ExtendedProperties.getProperInt32(-1, templateFieldDataUnit.previewRowLimit);
            int c = 0;

            foreach (object item in items)
            {
                DataRow dr = output.NewRow();
                if (runMonitorCheck) map.monitor.runFunctions(item);
                foreach (DataColumn dc in output.Columns)
                {
                    settingsPropertyEntryWithContext sce = dc.ExtendedProperties.getProperField(templateFieldDataTable.col_propertyInfo) as settingsPropertyEntryWithContext;

                    dr[dc] = sce.pi.GetValue(item, null);
                }
                if (runMonitorCheck) map.monitor.unlock();
                output.Rows.Add(dr);

                c++;
                if ((rowLimit > 0) && (c > rowLimit)) break;
            }

            return output;
        }

        protected virtual DataTable selectTwoColumnShema(DataTable shema, dataUnitPresenter presenter)
        {
            if (presenter == null) presenter = complete_Table;
            DataTable output = new DataTable();
            output.TableName = presenter.name;
            output.ExtendedProperties.copyInto(shema.ExtendedProperties); // = presenter.name;

            foreach (string cns in map.fieldsByNeedle[presenter.key])
            {
                DataColumn dcShema = shema.Columns[cns];
                DataColumn dcTable = output.Columns.Add(dcShema.ColumnName);
                dcTable.Caption = dcShema.Caption;
                dcTable.Expression = dcShema.Expression;
                dcTable.DataType = dcShema.DataType;
                dcTable.ExtendedProperties.copyInto(dcTable.ExtendedProperties); // = dcShema.DataType;
            }
            return output;
        }

        protected virtual DataColumn addStandardColumnShema(DataTable shema, dataUnitPresenter presenter, PropertyEntryColumn pec)
        {
            string name = pec.toColumnCaption(false);
            string desc = pec.getDescriptionForKey();

            DataColumn dc = shema.Columns.Add(pec.toStringSafe());
            dc.ColumnName = pec.toStringSafe();
            dc.Caption = name;
            dc.DataType = pec.toColumnType();

            dc.ExtendedProperties[templateFieldDataTable.col_caption] = name;
            dc.ExtendedProperties[templateFieldDataTable.col_desc] = desc;
            dc.ExtendedProperties[templateFieldDataTable.col_format] = "";
            dc.ExtendedProperties[templateFieldDataTable.col_group] = "";
            dc.ExtendedProperties[templateFieldDataTable.col_priority] = pec.toColumnPriority();
            dc.ExtendedProperties[templateFieldDataTable.col_type] = typeof(string);

            return dc;
        }

        /// <summary>
        /// Selects the data table shema.
        /// </summary>
        /// <param name="shema">The shema.</param>
        /// <param name="presenter">The presenter.</param>
        /// <returns></returns>
        protected virtual DataTable selectDataTableShema(DataTable shema, dataUnitPresenter presenter)
        {
            if (presenter == null) presenter = complete_Table;
            DataTable output = new DataTable();
            output.TableName = presenter.name;
            output.ExtendedProperties.copyInto(shema.ExtendedProperties); // = presenter.name;

            foreach (string cns in map.fieldsByNeedle[presenter.key])
            {
                DataColumn dcShema = shema.Columns[cns];
                DataColumn dcTable = output.Columns.Add(dcShema.ColumnName);
                dcTable.Caption = dcShema.Caption;
                dcTable.Expression = dcShema.Expression;
                dcTable.DataType = dcShema.DataType;
                dcTable.ExtendedProperties.copyInto(dcTable.ExtendedProperties); // = dcShema.DataType;
            }
            return output;
        }

        protected void setExtendedProperties(DataTable output, dataUnitPresenter presenter)
        {
            output.ExtendedProperties[templateFieldDataTable.shema_classname] = instanceType.FullName;
            output.ExtendedProperties[templateFieldDataTable.data_tablename] = presenter.name;
            output.ExtendedProperties[templateFieldDataTable.data_tabledesc] = presenter.description;
            output.ExtendedProperties[templateFieldDataUnit.map] = map;
            output.ExtendedProperties[templateFieldDataUnit.complete_Table] = complete_Table;
            output.ExtendedProperties[templateFieldDataUnit.dataAcquireFlags] = dataAcquireFlags;
            output.ExtendedProperties[templateFieldDataUnit.dataUnitPresenter] = presenter;
            output.ExtendedProperties[templateFieldDataUnit.previewRowLimit] = dataAcquireFlags.getPreviewRowLimit();
            output.ExtendedProperties[templateFieldDataUnit.autocountRow] = dataAcquireFlags.getPreviewRowLimit();
        }

        /// <summary>
        /// Builds the data table shema.
        /// </summary>
        /// <param name="presenter">The presenter.</param>
        /// <returns></returns>
        public virtual DataTable buildDataTableShema(dataUnitPresenter presenter = null)
        {
            if (presenter == null) presenter = complete_Table;

            DataTable output = new DataTable(presenter.name);
            setExtendedProperties(output, presenter);

            bool insertCountRow = dataAcquireFlags.HasFlag(dataDeliveryAcquireEnum.insertCountRow);

            foreach (settingsPropertyEntryWithContext sce in map.columns)
            {
                if (insertCountRow)
                {
                    DataColumn dci = output.Columns.Add("i", typeof(int));
                    dci.Caption = "#";
                    dci.Expression = "Count(" + sce.pi.Name + ")";
                    insertCountRow = false;
                }

                DataColumn dc = output.Columns.Add(sce.pi.Name, sce.pi.PropertyType);
                dc.Caption = sce.displayName;
                if (!sce.expression.isNullOrEmpty()) dc.Expression = sce.expression;
                if (!sce.escapeValueString) dc.ExtendedProperties[templateFieldDataTable.col_directAppend] = true;
                dc.ExtendedProperties[templateFieldDataTable.col_caption] = sce.displayName;
                dc.ExtendedProperties[templateFieldDataTable.col_desc] = sce.description;
                dc.ExtendedProperties[templateFieldDataTable.col_format] = sce.format;
                dc.ExtendedProperties[templateFieldDataTable.col_group] = sce.categoryName;
                dc.ExtendedProperties[templateFieldDataTable.col_imbattributes] = sce.attributes;
                dc.ExtendedProperties[templateFieldDataTable.col_propertyInfo] = sce;
                dc.ExtendedProperties[templateFieldDataTable.col_priority] = sce.priority;
                dc.ExtendedProperties[imbAttributeName.viewPriority] = sce.priority;
                dc.ExtendedProperties[templateFieldDataTable.col_type] = sce.pi.PropertyType;
                output.ExtendedProperties[imbAttributeName.measure_important] = sce.importance;
            }

            return output;
        }

        private DataTable _tableShema = new DataTable();

        /// <summary> </summary>
        public DataTable tableShema
        {
            get
            {
                return _tableShema;
            }
            protected set
            {
                _tableShema = value;
                OnPropertyChanged("table");
            }
        }

        public dataUnitBase(Type __instanceType)
        {
            instanceType = __instanceType;
            unitType = GetType();

            //  typePropertyDictionary = instanceType.buildPropertyCollection(true, false, "in_");
        }

        protected void buildMap()
        {
            map = dataUnitMap.getDataUnitMap(instanceType, unitType);
            var unitProps = unitType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

            presenters = new dataUnitPresenterDictionary();

            foreach (PropertyInfo pi in unitProps)
            {
                if (pi.PropertyType == typeof(dataUnitPresenter))
                {
                    dataUnitPresenter presObj = pi.GetValue(this, null) as dataUnitPresenter;
                    if (presObj != null)
                    {
                        if (!presenters.items.ContainsKey(presObj.key)) presenters.items.Add(presObj.key, presObj);
                        presObj.parent = this;
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected Type unitType { get; set; }

        /// <summary>Instance that will transfer property valus into dataUnit</summary>
        protected Type instanceType { get; set; }

        /// <summary>
        ///
        /// </summary>
        public dataDeliveryAcquireEnum dataAcquireFlags { get; set; }
    }
}