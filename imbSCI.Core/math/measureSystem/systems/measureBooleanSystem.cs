// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureBooleanSystem.cs" company="imbVeles" >
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
    ///
    /// </summary>
    /// <seealso cref="measureDecadeSystem" />
    public class measureBooleanSystem : measureDecadeSystem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="measureBooleanSystem"/> class.
        /// </summary>
        public measureBooleanSystem() : base(measureSystemsEnum.booleans)
        {
            AddRole(measureBooleanRoles.found, "f", "").setUnitSufixOverride("found");
            AddRole(measureBooleanRoles.tested, "t", "").setUnitSufixOverride("tested");
            AddRole(measureBooleanRoles.executed, "e", "").setUnitSufixOverride("executed");
            AddRole(measureBooleanRoles.copied, "c", "").setUnitSufixOverride("copied");
            AddRole(measureBooleanRoles.triggered, "t", "").setUnitSufixOverride("triggered");
            AddRole(measureBooleanRoles.check, "ch", "").setUnitSufixOverride("checked");

            AddUnit("", measureBooleanPreset.TrueFalse).setValueMap(true, "True").setValueMap(false, "False");
            AddUnit("", measureBooleanPreset.YesNo).setValueMap(true, "Yes").setValueMap(false, "No");
            AddUnit("", measureBooleanPreset.OnOff).setValueMap(true, "On").setValueMap(false, "Off");
            AddUnit("", measureBooleanPreset.IsNot).setValueMap(true, "Is").setValueMap(false, "Not");
            AddUnit("", measureBooleanPreset.Number).setValueMap(true, "1").setValueMap(false, "0");
            AddUnit("", measureBooleanPreset.DoNot).setValueMap(true, "Do").setValueMap(false, "Don't");
            AddUnit("", measureBooleanPreset.PlayStop).setValueMap(true, "Play").setValueMap(false, "Stop");
            AddUnit("", measureBooleanPreset.MinMax).setValueMap(true, "Max").setValueMap(false, "Min");
            AddUnit("", measureBooleanPreset.SuccessFailed).setValueMap(true, "Success").setValueMap(false, "Failed");
            //AddUnit("", 0, "Checking").setValueMap(true, "Checked").setValueMap(false, "Not checked");
            //AddUnit("", 0, "Testing").setValueMap(true, "Tested").setValueMap(false, "Not tested");
            //AddUnit("test", 0, "doTest").setValueMap(true, "Do").setValueMap(false, "Don't");
            //AddUnit("run", 0, "doRun").setValueMap(true, "Do").setValueMap(false, "Don't");
            //AddUnit("", 0, "Done").setValueMap(true, "Done").setValueMap(false, "To do");
            //AddUnit("", 0, "Find").setValueMap(true, "Found").setValueMap(false, "Not found");
            //AddUnit("", 0, "Digital out").setValueMap(true, "HIGH").setValueMap(false, "LOW");
            //AddUnit("", 0, "Voltage").setValueMap(true, "HIGH").setValueMap(false, "LOW");

            doFinalSetup();
        }
    }
}