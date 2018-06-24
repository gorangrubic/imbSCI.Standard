// --------------------------------------------------------------------------------------------------------------------
// <copyright file="generalNumericMeasureSystem.cs" company="imbVeles" >
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

    public class generalNumericMeasureSystem : measureDecadeSystem
    {
        public generalNumericMeasureSystem() : base(measureSystemsEnum.generalNumeric)
        {
            AddRole(measureRoleEnum.numeric, "n", "#");
            AddRole(measureRoleEnum.items, "Ic", "№");
            AddRole(measureRoleEnum.taskCount, "Tc", "⋆");
            AddRole(measureRoleEnum.sampleCount, "Sc", "⎆");
            AddRole(measureRoleEnum.general, "c", "№");
            AddRole(measureRoleEnum.count, "c", "№").setFormat("{0:#,##0}", "{0:#,###} {1}");
            AddRole(measureRoleEnum.elementCount, "Ic", "№").setFormat("{0:#,##0}", "{0:#,###} {1}").setUnitSufixOverride("pcs");

            AddUnit("", 1, "number", "number").setFormat("{0:#,##0.###}", "{0:#,##0.###}{1}");
            AddUnit("", 1, "#", "#").setFormat("{0:#,##0}", "{0:#,##0}{1}");
            AddUnit("", 1, "pc", "pcs").setFormat("{0:#,##0}", "{0:#,##0} {1}");
            AddUnit("d", 10, "decade", "decades").setFormat("{0:#,##0.#}", "{0:#,##0.#}{1}");
            AddUnit("c", 100, "hundred", "hundreds").setFormat("{0:#,##0.#}", "{0:#,##0.#}{1}");
            AddUnit("k", 1000, "thousand", "thousands").setFormat("{0:#,##0.#}", "{0:#,##0.#}{1}");
            AddUnit("M", 1000000, "million", "millions").setFormat("{0:#,##0.#}", "{0:#,##0.#}{1}");
            AddUnit("B", 1000000000, "billion", "billions").setFormat("{0:#,##0.###}", "{0:#,##0.###}{1}");

            doFinalSetup();
        }
    }
}