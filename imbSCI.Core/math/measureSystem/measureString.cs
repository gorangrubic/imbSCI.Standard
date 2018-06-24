// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureString.cs" company="imbVeles" >
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
    using imbSCI.Core.math.measureSystem.core;
    using imbSCI.Core.math.range;
    using imbSCI.Data.primitives;
    using System;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="measure{TValue}.String}" />
    /// <seealso cref="System.ICloneable" />
    /// <seealso cref="IMeasure" />
    public class measureString : measure<string>, ICloneable, IMeasure
    {
        private coordinateXY _formatZone = new coordinateXY();

        /// <summary>
        ///
        /// </summary>
        public coordinateXY formatZone
        {
            get { return _formatZone; }
            set { _formatZone = value; }
        }

        public measureString() : base()
        {
            //valueTypeGroup = valueType.getTypeGroup();

            // info = measureSystemManager.getMeasureInfo(roleEnum, unitEnum, measureSystemsEnum.booleans);
            // setDefaultValue(defaultValue, true);

            //strTrue = info.unit.checkValueMap(true).toStringSafe();
            //strFalse = info.unit.checkValueMap(false).toStringSafe();
        }

        /// <summary>
        /// Calculates the specified second.
        /// </summary>
        /// <param name="second">The second.</param>
        /// <param name="op">The op.</param>
        /// <returns></returns>
        public override IMeasureBasic calculate(IMeasure second, operation op)
        {
            if (second is measureString)
            {
                measureString second_measureString = (measureString)second;

                primValue.calculate(op, second_measureString.primValue);
            }

            return this;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override void convertToUnit(measureSystemUnitEntry targetUnit)
        {
            //throw new NotImplementedException();
        }
    }
}