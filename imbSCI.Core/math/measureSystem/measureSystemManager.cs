// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureSystemManager.cs" company="imbVeles" >
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
    using imbSCI.Core.math.measureSystem.core;
    using imbSCI.Core.math.measureSystem.enums;
    using imbSCI.Core.math.measureSystem.systems;
    using System;

    /// <summary>
    ///
    /// </summary>
    public static class measureSystemManager
    {
        #region --- registry ------- static and autoinitiated object

        private static measureDecadeSystemRegistry _registry;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static measureDecadeSystemRegistry registry
        {
            get
            {
                if (_registry == null)
                {
                    _registry = new measureDecadeSystemRegistry();

                    _registry.Add(new memoryMeasureSystem());
                    _registry.Add(new spatialMeasureSystem());
                    _registry.Add(new temporalMeasureSystem());
                    _registry.Add(new generalNumericMeasureSystem());
                    _registry.Add(new proportionMeasureSystem());
                    _registry.Add(new measureBooleanSystem());
                    _registry.Add(new measureStringSystem());
                    _registry.Add(new ordinalMeasureSystem());
                    _registry.Add(new enumMeasureSystem());
                }
                return _registry;
            }
        }

        #endregion --- registry ------- static and autoinitiated object

        /// <summary>
        /// Gets the factor distance.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public static double GetFactorDistance(this measureSystemUnitEntry current, measureSystemUnitEntry other)
        {
            double div = current.factor;
            if (div == 0.000000) div = 1;
            double distance = other.factor / div;
            if (double.IsInfinity(distance)) return other.factor;
            return distance;
        }

        /// <summary>
        /// Gets expected string length change after factor distance applied
        /// </summary>
        /// <param name="distance">The distance.</param>
        /// <returns></returns>
        public static Int32 GetFactorStrLenChange(this double distance)
        {
            if (distance < 1)
            {
                return -(distance.ToString().Length - 1);
            }
            else if (distance == 1)
            {
                return 0;
            }
            else
            {
                return distance.ToString().Length;
            }
            return 0;
        }

        /// <summary>
        /// Gets the measure information.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="unitName">Name of the unit.</param>
        /// <param name="system">The system.</param>
        /// <returns></returns>
        public static measureInfo getMeasureInfo(Enum roleName, Enum unitName, measureSystemsEnum system)
        {
            measureInfo output = new measureInfo();
            var decsystem = registry[system];
            var unitdef = decsystem.GetUnitByName(unitName.ToString());
            var roledef = decsystem.GetRole(roleName);
            output.setup(unitdef, roledef, decsystem);
            return output;
        }

        /// <summary>
        /// Gets the measure information.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="level">The level.</param>
        /// <param name="system">The system.</param>
        /// <returns></returns>
        public static measureInfo getMeasureInfo(Enum role, Int32 level, String system)
        {
            measureInfo output = new measureInfo();
            var decsystem = registry[system];
            var unitdef = decsystem.GetUnit(level);
            var roledef = decsystem.GetRole(role);
            output.setup(unitdef, roledef, decsystem);
            return output;
        }

        /// <summary>
        /// Gets the measure information.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="level">The level.</param>
        /// <param name="system">The system.</param>
        /// <returns></returns>
        public static measureInfo getMeasureInfo(Enum role, Int32 level, measureSystemsEnum system)
        {
            measureInfo output = new measureInfo();
            var decsystem = registry[system];
            var unitdef = decsystem.GetUnit(level);
            var roledef = decsystem.GetRole(role);
            output.setup(unitdef, roledef, decsystem);
            return output;
        }

        //public static decadeLevel getBaseLevel(meas)

        //public static measureDecadeSystem getSystem(measureSystemsEnum system)
        //{
        //    measureDecadeSystem output = spatialSystem;

        //    switch (system)
        //    {
        //        case measureSystemsEnum.spatial:
        //            return spatialSystem;
        //            break;
        //        default:
        //            break;
        //    }

        //    return output;
        //}
    }
}