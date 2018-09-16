// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyCollectionCategoryList.cs" company="imbVeles" >
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
    using imbSCI.Core.attributes;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.math.measurement;
    using imbSCI.Data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    /// <summary>
    /// DataField Shemas, divided by Category specifications
    /// </summary>
    /// <remarks>
    /// <para>Auto-scanning of this level of inherence, properties splitted to categories.</para>
    /// <para>Supported <see cref="Attribute"/>: <see cref="DisplayNameAttribute"/>, <see cref="CategoryAttribute"/>, <see cref="DescriptionAttribute"/></para>
    /// <para>Supported <see cref="imbSCI.Core.attributes.imbAttribute"/>: <see cref="imbSCI.Core.attributes.imbAttributeName.measure_displayGroup"/>, <see cref="imbSCI.Core.attributes.imbAttributeName.measure_displayGroupDescripton"/>...</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// [DisplayName("Basic stage control")]
    /// [Description("Basic stage host with a  simple general objective solutions: iteration limit")]
    /// public class spiderStageControl : IAppendDataFieldsExtended
    /// </code>
    /// </example>
    /// <example>
    /// <code>
    /// [DisplayName("Sample size")]
    /// [Description("Sampled population count")]
    /// [imb(imbAttributeName.measure_displayGroup, "Sample")]
    /// [imb(imbAttributeName.measure_displayGroupDescripton, "Sample statictics")]
    /// [imb(imbAttributeName.measure_metaModelName, "SampleSize")]
    /// [imb(imbAttributeName.measure_metaModelPrefix, "SM01")]
    /// public Int32 sampleSize
    /// </code>
    /// <para>Recommanded attributes for meta description</para>
    /// </example>
    /// <value>
    /// List of PropertyCollectionExtended representing group/categories of properties
    /// </value>
    public class PropertyCollectionCategoryList<T> : PropertyCollectionExtendedList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCollectionCategoryList{T}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <exception cref="NotImplementedException">-- not implemented --</exception>
        public PropertyCollectionCategoryList(String name = "", String description = "")
        {
            Type source = typeof(T);
            String __name = source.getAttributeValueOrDefault<DisplayNameAttribute>(name, false);
            String __description = source.getAttributeValueOrDefault<DisplayNameAttribute>(description, false);

            if (__name.isNullOrEmpty()) __name = typeof(T).Name;

            measureSetExternal setExternal = new measureSetExternal(source, __name, __description);
            throw new NotImplementedException("-- not implemented --");
            //setExternal.export(this);
        }

        /// <summary>
        /// Builds the data fields for category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public PropertyCollectionExtended buildDataFieldsForCategory(String category, T instance)
        {
            PropertyCollectionExtended pce = new PropertyCollectionExtended();
            PropertyCollectionExtended shema = this[category];

            pce.name = shema.name;
            pce.description = shema.description;
            pce.AddMetaRangeFrom(shema);

            pce.setFromObject(instance);

            return pce;
        }

        /// <summary>
        /// Builds the data table for category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">-- not implemented --</exception>
        public DataTable buildDataTableForCategory(String category, T instance)
        {
            PropertyCollectionExtended pce = new PropertyCollectionExtended();
            PropertyCollectionExtended shema = this[category];

            pce.name = shema.name;
            pce.description = shema.description;
            pce.AddMetaRangeFrom(shema);

            pce.setFromObject(instance);

            throw new NotImplementedException("-- not implemented --");

            return null; // pce.buildDataTableVertical(category, shema.description);
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="instanceNameProperty">The instance name property.</param>
        /// <returns></returns>
        private String getName(Object instance, String instanceNameProperty)
        {
            var pi = instance.GetType().GetProperty(instanceNameProperty);
            String iName = pi.GetValue(instance, null).ToString();
            return iName;
        }

        /// <summary>
        /// Builds the property collection extended list.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="instances">The instances.</param>
        /// <param name="instanceNameProperty">The instance name property.</param>
        /// <returns></returns>
        public PropertyCollectionExtendedList buildPropertyCollectionExtendedList(String category, IEnumerable<T> instances, String instanceNameProperty)
        {
            PropertyCollectionExtended shema = this[category];

            PropertyCollectionExtendedList output = new PropertyCollectionExtendedList();

            foreach (T instance in instances)
            {
                PropertyCollectionExtended pce = new PropertyCollectionExtended();
                pce.name = shema.name;
                pce.description = shema.description;
                pce.AddMetaRangeFrom(shema);
                // String iName =
                //instance.GetPropertyValue().toStringSafe();
                pce.name = getName(instance, instanceNameProperty);

                output.Add(pce, pce.name, true);
            }

            return output;
        }

        /// <summary>
        /// Builds the data table.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="instances">The instances.</param>
        /// <param name="instanceNameProperty">The instance name property.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">-- not implemented yet --</exception>
        public DataTable buildDataTable(String category, IEnumerable<T> instances, String instanceNameProperty)
        {
            PropertyCollectionExtended shema = this[category];

            DataTable output = shema.getDataTable();

            foreach (T instance in instances)
            {
                PropertyCollectionExtended pce = new PropertyCollectionExtended();
                pce.name = shema.name;
                pce.description = shema.description;
                pce.AddMetaRangeFrom(shema);
                pce.name = getName(instance, instanceNameProperty);

                // output.Add(pce, pce.name, true);

                throw new NotImplementedException("-- not implemented yet --");

                // output.AddDataTableRow(instance, imbDataTableExtensions.buildDataTableOptions.none);
            }

            return output;
        }
    }
}