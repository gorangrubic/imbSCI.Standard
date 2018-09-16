// --------------------------------------------------------------------------------------------------------------------
// <copyright file="temporalMeasureSystem.cs" company="imbVeles" >
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
    /// System for time measurement
    /// </summary>
    /// <seealso cref="measureDecadeSystem" />
    public class temporalMeasureSystem : measureDecadeSystem
    {
        public temporalMeasureSystem() : base(measureSystemsEnum.period)
        {
            //AddRole(measureRoleEnum.date, "d", "⌚");
            //AddRole(measureRoleEnum.time, "t", "⌚");
            //AddRole(measureRoleEnum.datetime, "t", "⌚");
            AddRole(measureRoleEnum.period, "T", "⌛");
            AddRole(measureRoleEnum.duration, "T", "⌛");

            AddUnit("s", 0, "second", "seconds").setFormat("{0:#,##0.000}", "{0:#,##0.000}{1}");
            AddUnit(decadeLevel.mili).setFormat("{0:#,##0.000}", "{0:#,##0.000}{1}");
            AddUnit(decadeLevel.micro).setFormat("{0:#,##0.000}", "{0:#,##0.000}{1}");
            AddUnit(decadeLevel.nano).setFormat("{0:#,##0.000}", "{0:#,##0.000}{1}");

            AddUnit("m", 60, "minut", "minutes").setFormat("{0:#,##0.000}", "{0:#,##0.000}{1}");
            AddUnit("h", 3600, "hour", "hours").setFormat("{0:#,##0.000}", "{0:#,##0.000}{1}");
            AddUnit("d", 86400, "day", "days").setFormat("{0:#,##0.000", "{0:#,##0.000}{1}");
            AddUnit("M", 2592000, "month", "months").setFormat("{0:#,##0.000}", "{0:#,##0.000}{1}");
            AddUnit("Y", 31536000, "year", "years").setFormat("{0:#,##0.000}", "{0:#,##0.000}{1}");

            doFinalSetup();
        }
    }
}