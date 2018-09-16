// --------------------------------------------------------------------------------------------------------------------
// <copyright file="proportionMeasureSystem.cs" company="imbVeles" >
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
namespace imbSCI.Core.math.measureSystem.systems
{
    using imbSCI.Core.math.measureSystem.enums;

    /// <summary>
    /// Measure system for> percentage, permile, ratios
    /// </summary>
    /// <seealso cref="measureDecadeSystem" />
    public class proportionMeasureSystem : measureDecadeSystem
    {
        public proportionMeasureSystem() : base(measureSystemsEnum.proportion)
        {
            AddRole(measureRoleEnum.progress, "P", "⤏");
            AddRole(measureRoleEnum.proportion, "p", ":");
            AddRole(measureRoleEnum.ratio, "r", "÷");
            //AddRole(measureRoleEnum.percent, "P", "÷");
            // AddRole(measureRoleEnum.page, "P", "⤏");

            AddUnit("", 0, "time", "times").setFormat("{0:#.00}", "{0:#.00}");
            AddUnit("", 0, "ratio", "ratio").setFormat("{0:0.0000}", "{0:0.0000}{1}");
            AddUnit("%", 0.01, "percent", "percents").setFormat("{0:#,##0.00}", "{0:#,##0.00} {1}");
            AddUnit("‰", 0.001, "promile", "primiles").setFormat("{0:#,##0.00}", "{0:#,##0.00} {1}");

            doFinalSetup();
        }
    }
}