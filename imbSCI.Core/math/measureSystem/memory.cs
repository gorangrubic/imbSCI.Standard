// --------------------------------------------------------------------------------------------------------------------
// <copyright file="memory.cs" company="imbVeles" >
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
namespace imbSCI.Core.math.measureSystem
{
    using imbSCI.Core.enums;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.math.measureSystem.enums;
    using imbSCI.Core.math.range;
    using System;

    public class memory : measureDecimal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="memory"/> class.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        public memory(decimal defaultValue = 0) : base(measureRoleEnum.size, measureSystemsEnum.memory, defaultValue)
        {
        }

        /// <summary>
        /// Calculates the specified second.
        /// </summary>
        /// <param name="second">The second.</param>
        /// <param name="op">The op.</param>
        /// <returns></returns>
        public IMeasureBasic calculate(IMeasure second, operation op)
        {
            if (second is IMeasure)
            {
                IMeasure second_IMeasure = (IMeasure)second;
                second_IMeasure.convertToUnit(info.unit);

                primValue = primValue.calculate(op, second.getValue<Decimal>());
            }

            return this;
        }
    }
}