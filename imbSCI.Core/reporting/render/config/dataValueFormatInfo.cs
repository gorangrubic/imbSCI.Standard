// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataValueFormatInfo.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.render.config
{
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Data;

    public class dataValueFormatInfo
    {
        private Boolean _directAppend;

        /// <summary>
        /// Avoid markdown, XML or html escaping
        /// </summary>
        public Boolean directAppend
        {
            get { return _directAppend; }
            set { _directAppend = value; }
        }

        private String _valueFormat = "";

        /// <summary>
        /// String format to use for value to string transformation
        /// </summary>
        public String valueFormat
        {
            get { return _valueFormat; }
            set { _valueFormat = value; }
        }

        private printHorizontal _position = printHorizontal.left;

        /// <summary>
        ///
        /// </summary>
        public printHorizontal position
        {
            get { return _position; }
            set { _position = value; }
        }

        private dataPointImportance _importance;

        /// <summary>
        ///
        /// </summary>
        public dataPointImportance importance
        {
            get { return _importance; }
            set { _importance = value; }
        }

        public dataValueFormatInfo(DataColumn dc)
        {
            directAppend = dc.ExtendedProperties.getProperBoolean(false, templateFieldDataTable.col_directAppend);
            valueFormat = dc.GetFormat();
            importance = dc.GetImportance(); // ExtendedProperties.getProperEnum<dataPointImportance>(dataPointImportance.normal, imbAttributeName.measure_important);

            Type type = dc.DataType;
            if (dc.ExtendedProperties.Contains(templateFieldDataTable.col_type)) type = (Type)dc.ExtendedProperties[templateFieldDataTable.col_type];

            if (dc.DataType.isNumber()) position = printHorizontal.right;
        }
    }
}