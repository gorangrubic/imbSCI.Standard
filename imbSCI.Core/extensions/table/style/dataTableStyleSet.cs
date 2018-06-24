// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataTableStyleSet.cs" company="imbVeles" >
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
using System;

namespace imbSCI.Core.extensions.table.style
{
    using imbSCI.Core.enums;
    using System.Xml.Serialization;

    /// <summary>
    ///
    /// </summary>
    public class dataTableStyleSet
    {
        public String name { get; set; } = "default";

        public dataTableStyleSet()
        {
            // theme = stylePresets.themeScience;
            rowStyles.DeployDefaults();
            rowImportanceStyles.DeployDefaults();
        }

        //public styleTheme theme { get; set; }

        public dataTableStyleEntry GetStyle(Object key)
        {
            if (key is DataRowInReportTypeEnum)
            {
                return rowStyles[(DataRowInReportTypeEnum)key];
            }

            if (key is dataPointImportance)
            {
                return rowImportanceStyles[(dataPointImportance)key];
            }

            return rowStyles[DataRowInReportTypeEnum.data];
        }

        [XmlIgnore]
        public dataTableStyleDictionary<dataPointImportance> rowImportanceStyles { get; set; } = new dataTableStyleDictionary<dataPointImportance>();

        [XmlIgnore]
        public dataTableStyleDictionary<DataRowInReportTypeEnum> rowStyles { get; set; } = new dataTableStyleDictionary<DataRowInReportTypeEnum>();

        public void initiateDefaultStyles()
        {
        }

        //public string fontName { get; set; } = "Cambria"; //"Times New Roman";
        [XmlIgnore]
        public System.Drawing.Color columnCaption { get; set; } = System.Drawing.Color.SteelBlue;

        [XmlIgnore]
        public System.Drawing.Color extraEven { get; set; } = System.Drawing.Color.LightSlateGray;

        [XmlIgnore]
        public System.Drawing.Color extraEvenOther { get; set; } = System.Drawing.Color.LightSteelBlue;

        [XmlIgnore]
        public System.Drawing.Color extraOdd { get; set; } = System.Drawing.Color.SlateGray;

        [XmlIgnore]
        public System.Drawing.Color dataOdd { get; set; } = System.Drawing.Color.WhiteSmoke;

        [XmlIgnore]
        public System.Drawing.Color dataEven { get; set; } = System.Drawing.Color.Snow;
    }
}