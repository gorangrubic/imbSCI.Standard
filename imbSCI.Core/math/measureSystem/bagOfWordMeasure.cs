// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bagOfWordMeasure.cs" company="imbVeles" >
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
    ///// <summary>
    /////
    ///// </summary>
    ///// <seealso cref="aceCommonTypes.math.measureSystem.core.measure{aceCommonTypes.primitives.bagOfWords}" />
    ///// <seealso cref="System.ICloneable" />
    //public class bagOfWordMeasure : measure<bagOfWords>, ICloneable, IMeasure
    //{
    //    public override IMeasureBasic calculate(IMeasure second, operation op)
    //    {
    //        if (second is IMeasure)
    //        {
    //            IMeasure second_IMeasure = (IMeasure)second;
    //            second_IMeasure.convertToUnit(info.unit);

    //            primValue.calculate(second.getValue<Object>(), op);

    //        }

    //        return this;
    //    }

    //    /// <summary>
    //    /// Creates a new object that is a copy of the current instance.
    //    /// </summary>
    //    /// <returns>
    //    /// A new object that is a copy of this instance.
    //    /// </returns>
    //    public object Clone()
    //    {
    //        bagOfWordMeasure output = this.MemberwiseClone() as bagOfWordMeasure;

    //        output.primValue = primValue.Clone() as bagOfWords;

    //        return output;
    //    }

    //    public override void convertToUnit(measureSystemUnitEntry targetUnit)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}