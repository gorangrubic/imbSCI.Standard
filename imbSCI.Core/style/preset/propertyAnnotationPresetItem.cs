namespace imbSCI.Core.style.preset
{
    using imbSCI.Core.attributes;
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.reporting;
    using imbSCI.Data.enums.fields;
    using imbSCI.Data.interfaces;
    using System;
    using System.Data;
    using System.Reflection;

    /// <summary>
    /// Wrapped serializable indexed collection item
    /// </summary>
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithName" />
    public class propertyAnnotationPresetItem : IObjectWithName
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="propertyAnnotationPresetItem"/> class.
        /// </summary>
        public propertyAnnotationPresetItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="propertyAnnotationPresetItem"/>, with data annotations copied from the column
        /// </summary>
        /// <param name="column">The column.</param>
        public propertyAnnotationPresetItem(DataColumn column)
        {
            SetFrom(column, false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="propertyAnnotationPresetItem"/>, with data annotations copied from the property
        /// </summary>
        /// <param name="property">The property.</param>
        public propertyAnnotationPresetItem(PropertyInfo property)
        {
            SetFrom(property, false);
        }

        /// <summary>
        /// Sets data annotation from the column
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="skipExisting">if set to <c>true</c> [skip existing].</param>
        public void SetFrom(DataColumn column, Boolean skipExisting = true)
        {
            settingsPropertyEntry sPE = column.GetSPE();

            SetFrom(sPE, skipExisting);
        }

        /// <summary>
        /// Sets data annotation from the column
        /// </summary>
        public void SetFrom(PropertyInfo property, Boolean skipExisting = true)
        {
            settingsPropertyEntry sPE = new settingsPropertyEntry(property);

            SetFrom(sPE, skipExisting);
        }

        /// <summary>
        /// Sets data annotation from the column
        /// </summary>
        public void SetFrom(settingsPropertyEntry sPE, Boolean skipExisting = true)
        {
            name = sPE.name;
            PropertyCollection pce = sPE.exportPropertyCollection(definitions);
            foreach (var p in pce.Keys)
            {
                definitions.Add(p, pce[p]);
            }
        }

        /// <summary>
        /// Builds a <see cref="settingsPropertyEntry"/> from contained data
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="skipExisting">if set to <c>true</c> [skip existing].</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        public settingsPropertyEntry BuildPCE(DataColumn column, Boolean skipExisting = true, ILogBuilder log = null)
        {
            settingsPropertyEntry pce = new settingsPropertyEntry(column);

            PropertyCollection pc = pce.exportPropertyCollection();

            foreach (var pair in definitions.keyValuePairs)
            {
                if (skipExisting)
                {
                    if (pc.ContainsKey(pair.resolvedKey))
                    {
                        if (!pc[pair.resolvedKey].toStringSafe().isNullOrEmpty())
                        {
                            if (log != null) log.log(" Deploy [" + pair.key + "] = false, because destination is not empty (skipExisting=" + skipExisting.ToString() + ")");
                            continue;
                        }
                    }
                }

                switch (pair.resolvedKey)
                {
                    //case imbAttributeName.measure_calcGroup:
                    //case imbAttributeName.measure_displayGroup:

                    //    column.SetGroup(pair.value.toStringSafe());
                    //    if (log != null) log.log("Set[" + pair.key + "] = " + pair.value.toStringSafe());
                    //    break;
                    //case imbAttributeName.reporting_columnWidth:
                    //case templateFieldDataTable.columnWidth:
                    //    column.SetWidth(pair.value.imbConvertValueSafeTyped<Int32>());
                    //    if (log != null) log.log("Set[" + pair.key + "] = " + pair.value.toStringSafe());
                    //    break;
                    //case templateFieldDataTable.col_caption:
                    //case templateFieldDataTable.title:
                    //    column.Caption = pair.value.toStringSafe(column.ColumnName.imbTitleCamelOperation(true));
                    //    if (log != null) log.log("Set[" + pair.key + "] = " + pair.value.toStringSafe());
                    //    break;
                    //case imbAttributeName.basicColor:
                    //case templateFieldDataTable.col_color:
                    //    column.SetDefaultBackground(pair.value.toStringSafe());

                    //    if (log != null) log.log("Set[" + pair.key + "] = " + pair.value.toStringSafe());
                    //    break;
                    default:
                        if (pair.resolvedKey is imbAttributeName attName)
                        {
                            pce.deploy(attName, pair.value);
                            if (log != null) log.log("Set[" + pair.key + "] = " + pair.value.toStringSafe());
                        }
                        else if (pair.resolvedKey is templateFieldDataTable tfdt)
                        {
                            pce.deploy(tfdt, pair.value);
                            if (log != null) log.log("Set[" + pair.key + "] = " + pair.value.toStringSafe());
                        }
                        else
                        {
                            if (log != null) log.log(column.Table.GetTitle() + "." + column.ColumnName + " <- entry not recognized [" + pair.key + "]");
                        }
                        break;
                }
            }

            return pce;
        }

        /// <summary>
        /// Deploys contained definitions onto DataColumn
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="skipExisting">if set to <c>true</c> [skip existing].</param>
        public void DeployTo(DataColumn column, Boolean skipExisting = true, ILogBuilder log = null)
        {
            if (log != null) log.log(column.Table.GetTitle() + "." + column.ColumnName + " <- deploying preset [" + name + "]");

            settingsPropertyEntry pce = BuildPCE(column, skipExisting, log);
            column.SetSPE(pce);

            /*

            foreach (var pair in definitions.keyValuePairs)
            {
                Boolean deploy = true;

                if (skipExisting)
                {
                    if (column.ExtendedProperties.ContainsKey(pair.resolvedKey))
                    {
                        if (!column.ExtendedProperties[pair.resolvedKey].toStringSafe("").isNullOrEmpty())
                        {
                            deploy = false;
                            if (log != null) log.log(" Deploy [" + pair.key + "] = false, because destination is not empty (skipExisting=" + skipExisting.ToString() + ")");
                        }
                    }
                }

                if (deploy)
                {
                    String valueCheck = pair.value.toStringSafe("");
                    if (valueCheck.isNullOrEmpty())
                    {
                        if (log != null) log.log(" Deploy [" + pair.key + "] = false, because value is empty");
                        deploy = false;
                    }
                }

                if (deploy)
                {
                    if (column.ExtendedProperties.ContainsKey(pair.resolvedKey))
                    {
                        column.ExtendedProperties[pair.resolvedKey] = pair.value;
                        if (log != null) log.log("Set[" + pair.key + "] = " + pair.value.toStringSafe());
                    }
                    else
                    {
                        column.ExtendedProperties.Add(pair.resolvedKey, pair.value);
                        if (log != null) log.log("Add[" + pair.key + "] = " + pair.value.toStringSafe());
                    }
                }
            }
            */
        }

        /// <summary>
        /// Data annotation definitions,
        /// </summary>
        /// <value>
        /// The definitions.
        /// </value>
        public propertyAnnotationPresetItemDefinitions definitions { get; set; } = new propertyAnnotationPresetItemDefinitions();

        /// <summary>
        /// Initializes a new instance of the <see cref="propertyAnnotationPresetItem"/> class.
        /// </summary>
        /// <param name="_name">The name.</param>
        public propertyAnnotationPresetItem(string _name)
        {
            name = _name;
        }

        /// <summary>
        /// Name of property or column, that should receive data annotation preset
        /// </summary>
        public string name { get; set; } = "";
    }
}