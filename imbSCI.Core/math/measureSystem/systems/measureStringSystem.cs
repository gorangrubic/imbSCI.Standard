// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureStringSystem.cs" company="imbVeles" >
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
    using System;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="measureDecadeSystem" />
    public class measureStringSystem : measureDecadeSystem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="measureStringSystem"/> class.
        /// </summary>
        public measureStringSystem() : base(measureSystemsEnum.text)
        {
            AddRole(measureStringRoleEnum.simple, "T", "⌨").setFormat("{0}", "'{0}'");

            AddRole(measureStringRoleEnum.tokenline, "S", "⎌").setFormat("{0}", "{0}", "-");
            AddRole(measureStringRoleEnum.wrapped, "W", "⌨").setFormat("{0}", "{0}", "-");

            AddRole(measureStringRoleEnum.trimmed, "t", "").setFormat("{0}", "{0}", "-");

            AddRole(measureStringRoleEnum.name, "ID", "").setFormat("{0}", "'{0}'");

            AddRole(measureStringRoleEnum.frequency, "frequency", "").setFormat("{0} ({1})", "{0} ({1})", Environment.NewLine);

            AddUnit(".", measureStringEnum.singleline).setFormat("{0}", "{0}");
            AddUnit(".", measureStringEnum.multiline).setFormat("{0}", "{0}");
            //AddUnit(measureStringEnum.xml).setFormat("{0}", "{0}");

            AddUnit(".", measureStringEnum.keyValue).setFormat("{0}", "{0}"); // ", 0, "keyValue", "keyValues").setFormat("{0}", "{0}({1})");
            //AddUnit()
        }
    }
}