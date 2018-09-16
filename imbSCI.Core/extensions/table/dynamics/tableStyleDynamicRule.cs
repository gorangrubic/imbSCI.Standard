// --------------------------------------------------------------------------------------------------------------------
// <copyright file="tableStyleDynamicRule.cs" company="imbVeles" >
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

namespace imbSCI.Core.extensions.table.core
{
    using imbSCI.Core.extensions.table.style;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Core.style.color;
    using imbSCI.Data;
    using System.Data;

    /// <summary>
    ///
    /// </summary>
    public abstract class tableStyleDynamicRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="tableStyleDynamicRule"/> class.
        /// </summary>
        protected tableStyleDynamicRule()
        {
        }

        /// <summary>
        /// Makes the default response.
        /// </summary>
        /// <param name="evaluationValue">if set to <c>true</c> [evaluation value].</param>
        /// <param name="okStype">The default style.</param>
        /// <returns></returns>
        protected tableStyleSetterResponse makeDefaultResponse(Boolean evaluationValue, dataTableStyleEntry okStype, dataTableStyleEntry noStype)
        {
            try
            {
                if (evaluationValue)
                {
                    return makeDefaultResponse(1, okStype, noStype);
                }
                else
                {
                    return makeDefaultResponse(0, okStype, noStype);
                }
            }
            catch (Exception ex)
            {
                throw new DataException(ex.LogException("StyleRule[" + GetType().Name + "]:makeDefaultResponse", "STYLE", false));
            }
        }

        /// <summary>
        /// Gets or sets the allowed zone.
        /// </summary>
        /// <value>
        /// The allowed zone.
        /// </value>
        public selectRangeAreaLimiter allowedZone { get; set; } = new selectRangeAreaLimiter();

        /// <summary>
        /// Sets the zone.
        /// </summary>
        /// <param name="dcLeft">The dc left.</param>
        /// <param name="dcRight">The dc right.</param>
        /// <param name="isActiveY">if set to <c>true</c> [is active y].</param>
        public void SetZone(DataColumn dcLeft, DataColumn dcRight = null, Boolean isActiveY = false)
        {
            Int32 l = dcLeft.Table.Columns.IndexOf(dcLeft);

            Int32 r = 0;

            if (dcRight != null) r = dcLeft.Table.Columns.IndexOf(dcRight); else r = l + 1;

            allowedZone.x = Math.Min(l, r);
            allowedZone.width = Math.Max(l, r) - allowedZone.x;
            allowedZone.isActiveY = isActiveY;
        }

        /// <summary>
        /// Confirms the zone and criterion.
        /// </summary>
        /// <param name="dr">The dr.</param>
        /// <param name="dc">The dc.</param>
        /// <returns></returns>
        protected Boolean confirmZoneAndCriterion(DataRow dr, DataColumn dc)
        {
            if (confirmZone(dr, dc))
            {
                return confirmCritarion(dr[dc.ColumnName]);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Confirms the zone.
        /// </summary>
        /// <param name="dr">The dr.</param>
        /// <param name="dc">The dc.</param>
        /// <returns></returns>
        protected Boolean confirmZone(DataRow dr, DataColumn dc)
        {
            if (allowedZone == null) return true;

            return allowedZone.isInside(dr.Table.Rows.IndexOf(dr), dc.Table.Columns.IndexOf(dc));
        }

        /// <summary>
        /// Confirms the critarion.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected abstract Boolean confirmCritarion(Object value);

        /// <summary>
        /// Makes the default response.
        /// </summary>
        /// <param name="evaluationValue">The evaluation value.</param>
        /// <param name="okStyle">The default style.</param>
        /// <returns></returns>
        protected tableStyleSetterResponse makeDefaultResponse(Double evaluationValue, dataTableStyleEntry okStyle, dataTableStyleEntry noStype)
        {
            //if (okStyle == null) okStyle = new dataTableStyleEntry();

            tableStyleSetterResponse output = null;
            if (evaluationValue == 0)
            {
                output = new tableStyleSetterResponse(noStype, this);
            }
            else if (evaluationValue > 0)
            {
                if (colorGradientDictionary != null)
                {
                    if (output == null)
                    {
                        output = new tableStyleSetterResponse(okStyle, this);
                    }

                    output.style.Background.Color = ColorWorks.GetColor(colorGradientDictionary.GetColor(evaluationValue));
                    output.style.BackgroundAlt.Color = ColorWorks.GetColor(colorGradientDictionary.GetColor(evaluationValue)); //.ColorFromHex();
                }
                else
                {
                    output = new tableStyleSetterResponse(customStyleEntry, this);
                }
            }

            return output;
        }

        /// <summary>
        /// Evaluates the specified row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="table">The table.</param>
        /// <param name="defaultStyle">The default style.</param>
        /// <returns></returns>
        public abstract tableStyleSetterResponse evaluate(DataRow row, DataTable table, dataTableStyleEntry defaultStyle);

        /// <summary>
        /// If not null - it will be returned if evaluation is positive
        /// </summary>
        /// <value>
        /// The custom style entry.
        /// </value>
        public dataTableStyleEntry customStyleEntry { get; set; } = null;

        /// <summary>
        /// Gets or sets the color gradient dictionary.
        /// </summary>
        /// <value>
        /// The color gradient dictionary.
        /// </value>
        public ColorGradientDictionary colorGradientDictionary { get; set; }
    }
}