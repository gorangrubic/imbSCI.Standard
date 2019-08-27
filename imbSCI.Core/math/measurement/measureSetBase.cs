// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureSetBase.cs" company="imbVeles" >
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
namespace imbSCI.Core.math.measurement
{
    using imbSCI.Core.attributes;
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.math.measureSystem.core;
    using imbSCI.Data;
    using imbSCI.Data.interfaces;
    using interfaces;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    ///
    /// </summary>
    public abstract class measureSetBase
    {
        protected measureSetBase(Type target, String name, String desc)
        {
            doAutoSet(target.GetProperties(BindingFlags.Instance | BindingFlags.GetProperty));
        }

        protected measureSetBase(String __name, String __description)
        {
            name = __name;
            description = __description;

            doAutoScan();
        }

#pragma warning disable CS1574 // XML comment has cref attribute 'IMeasure' that could not be resolved
#pragma warning disable CS1574 // XML comment has cref attribute 'IMeasure' that could not be resolved
        /// <summary>
        /// Gets the <see cref="aceCommonTypes.interfaces.IMeasure"/> with the specified path.
        /// </summary>
        /// <value>
        /// The <see cref="aceCommonTypes.interfaces.IMeasure"/>.
        /// </value>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public IMeasure this[String path]
#pragma warning restore CS1574 // XML comment has cref attribute 'IMeasure' that could not be resolved
#pragma warning restore CS1574 // XML comment has cref attribute 'IMeasure' that could not be resolved
        {
            get
            {
                IMeasure ot = null;
                List<String> pths = path.SplitSmart("."); //.getPathParts();
                if (pths.Count > 1)
                {
                    ot = displayGroups[pths[0]][pths[1]];
                    return ot;
                }
                else
                {
                    if (displayGroup != null)
                    {
                        ot = displayGroup[path];
                    }
                    if (ot != null) return ot;

                    return displayGroups.find(path);
                }

                return null;
            }
        }

#pragma warning disable CS1574 // XML comment has cref attribute 'IMeasure' that could not be resolved
#pragma warning disable CS1574 // XML comment has cref attribute 'IMeasure' that could not be resolved
        /// <summary>
        /// Gets the <see cref="aceCommonTypes.interfaces.IMeasure"/> with the specified group.
        /// </summary>
        /// <value>
        /// The <see cref="aceCommonTypes.interfaces.IMeasure"/>.
        /// </value>
        /// <param name="group">The group.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public IMeasure this[String group, String key]
#pragma warning restore CS1574 // XML comment has cref attribute 'IMeasure' that could not be resolved
#pragma warning restore CS1574 // XML comment has cref attribute 'IMeasure' that could not be resolved
        {
            get
            {
                IMeasure ot = displayGroups[group][key];

                //if (!items.ContainsKey(key))
                //{
                //    items.Add(key, new measureDisplayGroup(key, ""));
                //}
                return ot;
            }
        }

        private String _name;

        /// <summary>
        ///
        /// </summary>
        public String name
        {
            get { return _name; }
            set { _name = value; }
        }

        private String _description;

        /// <summary>
        ///
        /// </summary>
        public String description
        {
            get { return _description; }
            set { _description = value; }
        }

        protected Dictionary<PropertyInfo, basicDisplayItem> bdis = new Dictionary<PropertyInfo, basicDisplayItem>();

        protected measureCalculationGroups calcGroups = new measureCalculationGroups();

        protected measureDisplayGroups displayGroups = new measureDisplayGroups();

        //private measureCollection _measures = new measureCollection();

        ///// <summary> </summary>
        //public measureCollection measures
        //{
        //    get
        //    {
        //        return _measures;
        //    }
        //    protected set
        //    {
        //        _measures = value;
        //      //  OnPropertyChanged("measures");
        //    }
        //}

        //protected IMeasure getItem(String propKey)
        //{
        //    if (!ContainsKey(propKey))
        //    {
        //        PropertyInfo pi = GetType().GetProperty(propKey, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);

        //        setItem(pi);
        //    }

        //    return this[propKey] as IMeasure;
        //}

        protected measureDisplayGroup displayGroup;

        protected Int32 calcGroupId = 0;

        /// <summary>
        /// Sets the item.
        /// </summary>
        /// <param name="pi">The pi.</param>
        protected void setItem(PropertyInfo pi)
        {
            basicDisplayItem displayInfo = new basicDisplayItem(pi);
            bdis.Add(pi, displayInfo);

            if (displayGroup == null)
            {
                displayGroup = displayGroups["Main"];
            }

            if (displayInfo.attributes.ContainsKey(imbAttributeName.measure_displayGroup))
            {
                displayGroup = displayGroups[displayInfo.attributes.getMessage(imbAttributeName.measure_displayGroup, "Main")];

                if (displayGroup.description.isNullOrEmptyString()) { displayGroup.description = displayInfo.attributes.getMessage(imbAttributeName.measure_displayGroupDescripton, "."); }
            }
            else
            {
                //pi.getAttributeValueOrDefault<DescriptionAttribute>(displayGroup, pi)
            }

            Int32 priority = displayInfo.attributes.getMessage<Int32>(imbAttributeName.viewPriority, 100);

            calcGroupId = displayInfo.attributes.getMessage<Int32>(imbAttributeName.measure_calcGroup, calcGroupId);

            Object tmp = pi.PropertyType.getInstance();

            if (tmp is IObjectWithNameAndDescription)
            {
                IObjectWithNameAndDescription tmp_IObjectWithName = (IObjectWithNameAndDescription)tmp;
                tmp_IObjectWithName.name = displayInfo.displayName;
                tmp_IObjectWithName.description = displayInfo.description;
            }

            if (tmp is IValueWithImportanceInfo)
            {
                IValueWithImportanceInfo tmp_IValueWithImportanceInfo = (IValueWithImportanceInfo)tmp;
                tmp_IValueWithImportanceInfo.relevance = displayInfo.attributes.getMessage<dataPointImportance>(imbAttributeName.measure_important, dataPointImportance.normal);
            }

            if (tmp is IMeasure)
            {
                IMeasure im = (IMeasure)tmp;

                im.isAlarmTurnedOn = displayInfo.attributes.ContainsKey(imbAttributeName.measure_setAlarm);
                im.valueRange.setValueRange(displayInfo.attributes.getMessage<aceRangeConfig>(imbAttributeName.measure_setRange));
                im.alarmCriteria.setCriteria(displayInfo.attributes.getMessage<aceCriterionConfig>(imbAttributeName.measure_setAlarm));

                im.metaModelName = displayInfo.attributes.getMessage(imbAttributeName.measure_metaModelName, im.name);
                im.metaModelPrefix = displayInfo.attributes.getMessage(imbAttributeName.measure_metaModelPrefix, "MN");

                im.doUnitOptimization = displayInfo.attributes.getMessage<Boolean>(imbAttributeName.measure_optimizeUnit, false);

                Tuple<Enum, String, String, String, String> tup = displayInfo.attributes.getMessageObj(imbAttributeName.measure_setRole) as Tuple<Enum, String, String, String, String>;
                if (tup != null)
                {
                    //im.setCustomRoleEntry(bDI.attributes.getMessageObj(imbAttributeName.measure_setRole) as Tuple<Enum, String, String, String, String>)
                    im.info.unit = im.getUnitEntry(displayInfo.attributes.getMessage<measureSystemUnitEntry>(imbAttributeName.measure_setUnit));
                    im.info.role = im.getRoleEntry(displayInfo.attributes.getMessage<measureSystemUnitEntry>(imbAttributeName.measure_setRole));
                }

                //displayGroup.AddAtEnd(im);

                //calcGroups[calcGroupId].AddAtEnd(im);
            }

            //Add(pi.Name, tmp);
        }

        protected void calculate(PropertyInfo pi)
        {
        }

        /// <summary>
        /// Calculates all.
        /// </summary>
        protected void calculateAll()
        {
            foreach (var clg in calcGroups)
            {
                foreach (KeyValuePair<int, IMeasure> cl in clg.Value)
                {
                    cl.Value.calculateTasks.execute(cl.Value);
                }
            }

            foreach (var clg in calcGroups)
            {
                foreach (KeyValuePair<int, IMeasure> cl in clg.Value)
                {
                    cl.Value.convertToOptimalUnitLevel();
                }
            }
        }

        /// <summary>
        /// Connects the specified pi.
        /// </summary>
        /// <param name="pi">The pi.</param>
        protected void connect(PropertyInfo pi)
        {
            IMeasure im = this[pi.Name] as IMeasure;
            if (im == null) return;

            var bDI = bdis[pi];
            Int32 priority = bDI.attributes.getMessage<Int32>(imbAttributeName.viewPriority, 100);

            String operand = bDI.attributes.getMessage<String>(imbAttributeName.measure_operand, "");
            if (!imbSciStringExtensions.isNullOrEmptyString(operand))
            {
                operation opera = bDI.attributes.getMessage<operation>(imbAttributeName.measure_operation, operation.none);
                if (opera != operation.none)
                {
                    IMeasure cm = this[pi.Name] as IMeasure;
                    cm.calculateTasks.Add(priority, opera, im);
                }
            }
        }

        protected void reconnect(PropertyInfo pi)
        {
            //IMeasure im = this[pi.Name] as IMeasure;
            //if (im == null) return;

            //foreach (measureOperand mo in im.calculateTasks.items)
            //{
            //    if (mo.operand == null)
            //    {
            //        mo.operand = this.get(mo.operandName, null) as IMeasure;
            //    }
            //}
        }

        /// <summary>
        /// Performes automatic setup of the properties
        /// </summary>
        /// <param name="props">The props.</param>
        protected void doAutoSet(PropertyInfo[] props)
        {
            foreach (PropertyInfo pi in props)
            {
                setItem(pi);
            }
        }

        /// <summary>
        /// Does the automatic scan.
        /// </summary>
        protected void doAutoScan(PropertyInfo[] props = null)
        {
            if (props == null)
            {
                props = GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            }
            foreach (PropertyInfo pi in props)
            {
                setItem(pi);
            }

            foreach (PropertyInfo pi in props)
            {
                connect(pi);
            }

            foreach (PropertyInfo pi in props)
            {
                reconnect(pi);
            }
        }
    }
}