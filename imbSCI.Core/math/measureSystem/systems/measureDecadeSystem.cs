// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureDecadeSystem.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.math.measureSystem.core;
    using imbSCI.Core.math.measureSystem.enums;
    using imbSCI.Data;
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public class measureDecadeSystem
    {
        private String _name;

        /// <summary>
        ///
        /// </summary>
        public String name
        {
            get { return _name; }
            set { _name = value; }
        }

        protected void doFinalSetup()
        {
            units.prepare();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="measureDecadeSystem"/> class.
        /// </summary>
        /// <param name="__name">The name.</param>
        public measureDecadeSystem(measureSystemsEnum __name)
        {
            name = __name.ToString();

            if (name == measureSystemsEnum.unknown.ToString())
            {
                name = this.GetType().Name;
            }
        }

        //public measureDecadeSystem()
        //{
        //
        //}
        /// <summary>
        /// Gets the unit by short name or sufix. i.e.: <c>mm</c> for milimeters
        /// </summary>
        /// <param name="unitSufix">The unit sufix.</param>
        /// <returns></returns>
        public measureSystemUnitEntry GetUnitByName(String name)
        {
            if (units.ContainsKey(name))
            {
                return units[name];
            }
            else
            {
                return rootUnit;
            }
        }

        /// <summary>
        /// Gets the unit relative from root
        /// </summary>
        /// <param name="levelFromRoot">The level from root.</param>
        /// <returns></returns>
        public measureSystemUnitEntry GetUnit(Int32 levelFromRoot = 0)
        {
            if (levelFromRoot == 0) return rootUnit;

            return GetUnit(rootUnit, levelFromRoot);
        }

        private Int32 _targetStringLength = 5;

        /// <summary>
        /// Defines desired maximum string length used for <see cref="measure{TValue}.unitLevelQuality()"/> evaluation
        /// </summary>
        public Int32 targetStringLength
        {
            get { return _targetStringLength; }
            set { _targetStringLength = value; }
        }

        /// <summary>
        /// Gets the or make unit.
        /// </summary>
        /// <param name="tup">The tup.</param>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public measureSystemUnitEntry GetOrMakeUnit(Tuple<Enum, string, string, string, string> tup, measureSystemUnitEntry unit)
        {
            if (tup == null) return unit;

            String __name = tup.Item2.toStringSafe();
            if (imbSciStringExtensions.isNullOrEmptyString(__name)) __name = tup.Item1.toStringSafe();

            if (units.ContainsKey(__name))
            {
                return units[tup.Item1.ToString()];
            }
            else
            {
                measureSystemUnitEntry nr = null;
                if (unit == null)
                {
                    nr = new measureSystemUnitEntry(tup.Item2, __name, imbSciStringExtensions.add(__name, "s"));
                    nr.system = this;
                }
                else
                {
                    nr = unit.Clone() as measureSystemUnitEntry;
                    nr.nameSingular = tup.Item4;
                    nr.unit = tup.Item3;
                }
                nr.system = this;
                nr.factor = 0;
                nr.setFormat(tup.Item4, tup.Item5);

                return nr;
            }
        }

        /// <summary>
        /// Gets the or make role.
        /// </summary>
        /// <param name="tup">The tup.</param>
        /// <param name="mainRole">The main role.</param>
        /// <returns></returns>
        public measureSystemRoleEntry GetOrMakeRole(Tuple<Enum, String, String, String, String> tup, measureSystemRoleEntry mainRole = null)
        {
            if (tup == null) return mainRole;

            if (roles.ContainsKey(tup.Item1.ToString()))
            {
                return roles[tup.Item1.ToString()];
            }
            else
            {
                measureSystemRoleEntry nr = null;
                if (mainRole == null)
                {
                    nr = new measureSystemRoleEntry(tup.Item2, tup.Item3, tup.Item1.toStringSafe());
                }
                else
                {
                    nr = mainRole.Clone() as measureSystemRoleEntry;
                    nr.letter = tup.Item2;
                    nr.symbol = tup.Item3;
                    nr.name = tup.Item1.ToString();
                }
                nr.setFormat(tup.Item4, tup.Item5);

                return nr;
            }
        }

        /// <summary>
        /// Gets the unit.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="levelFromCurrent">How much levels from the current unit? same factors are ignored</param>
        /// <returns></returns>
        public measureSystemUnitEntry GetUnit(measureSystemUnitEntry current, Int32 levelFromCurrent)
        {
            measureUnitLevelChange direction = measureUnitLevelChange.optimum;
            measureSystemUnitEntry candidate = current;
            measureSystemUnitEntry next = null;
            if (levelFromCurrent < 0) direction = measureUnitLevelChange.goLower;
            if (levelFromCurrent > 0) direction = measureUnitLevelChange.goHigher;

            if (direction == measureUnitLevelChange.optimum) return current;

            levelFromCurrent = Math.Abs(levelFromCurrent);

            do
            {
                next = GetUnit(candidate, direction);
                levelFromCurrent--;
                if (candidate == next) break;
                candidate = next;
            } while (levelFromCurrent > 0);

            return candidate;
        }

        /// <summary>
        /// Gets next unit in specified direction <see cref="measureUnitLevelChange"/>
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="direction">The direction to get next unit</param>
        /// <returns>Next defined unit level with different factor</returns>
        public measureSystemUnitEntry GetUnit(measureSystemUnitEntry current, measureUnitLevelChange direction)
        {
            var cindex = units.IndexOf(current);
            double cfactor = current.factor;
            measureSystemUnitEntry candidate = current;

            do
            {
                if (direction == measureUnitLevelChange.goLower)
                {
                    if (cindex > 0)
                    {
                        cindex--;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (direction == measureUnitLevelChange.goHigher)
                {
                    if (cindex < (units.Count - 1))
                    {
                        cindex++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

                candidate = units[cindex];

                if (candidate == null)
                {
                    break;
                }
            } while (candidate.factor == cfactor);

            return candidate;
        }

        //public virtual convertToUnit()

        /// <summary>
        /// Gets the optimal unit.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public measureSystemUnitEntry GetOptimalUnit(measureSystemUnitEntry current, Int32 length)
        {
            measureSystemUnitEntry candidate = current;
            measureSystemUnitEntry next = null;
            double factorDistance = 1;
            Int32 factorStrDistance = 0;
            Int32 estLength = length;
            Int32 difference = length - targetStringLength;

            if (length > targetStringLength)
            {
                while (estLength > targetStringLength)
                {
                    next = GetUnit(candidate, measureUnitLevelChange.goHigher);
                    if (candidate == next) break;

                    factorStrDistance = next.GetFactorDistance(current).GetFactorStrLenChange();

                    estLength = length - factorStrDistance;
                    difference = estLength - targetStringLength;
                    if (difference < factorStrDistance)
                    {
                        break;
                    }
                    candidate = next;
                }
            }
            else if (length < targetStringLength)
            {
                while (estLength < targetStringLength)
                {
                    next = GetUnit(candidate, measureUnitLevelChange.goLower);
                    if (candidate == next) break;

                    factorStrDistance = next.GetFactorDistance(current).GetFactorStrLenChange();

                    estLength = length + factorStrDistance;
                    difference = targetStringLength - estLength;
                    if (difference < factorStrDistance)
                    {
                        break;
                    }
                    candidate = next;
                }
            }

            return candidate;
        }

        /// <summary>
        /// Gets the factor distance.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        public double GetFactorDistance(measureSystemUnitEntry current, measureUnitLevelChange direction)
        {
            measureSystemUnitEntry next = GetUnit(current, direction);

            return current.GetFactorDistance(next);
        }

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public measureSystemRoleEntry GetRole(Enum roleName)
        {
            return roles[roleName.ToString()];
        }

        /// <summary>
        /// Adds the unit.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        public measureSystemUnitEntry AddUnit(decadeLevel level = decadeLevel.mili)
        {
            if (rootUnit == null) throw new ArgumentException("rootUnit", "Root unit not set - cant use short form of AddUnit()");
            return AddUnit(imbSciStringExtensions.add(level.toLetter(), rootUnit.unit), level.toFactor(), imbSciStringExtensions.add(level.toPrefix(), rootUnit.nameSingular), imbSciStringExtensions.add(level.toPrefix(), rootUnit.namePlural));
        }

        /// <summary>
        /// Auto detects the root unit
        /// </summary>
        protected void rootUnitAutofind()
        {
            if (_rootUnit == null)
            {
                _rootUnit = units.getRoot();
            }
        }

        /// <summary>
        /// Adds the unit.
        /// </summary>
        /// <param name="sufix">The sufix.</param>
        /// <param name="nameEnum">The name enum.</param>
        /// <returns></returns>
        public measureSystemUnitEntry AddUnit(String sufix, Enum nameEnum)
        {
            return AddUnit(sufix, 0, nameEnum.ToString());
        }

        /// <summary>
        /// Adds the unit.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <param name="level">The level.</param>
        /// <param name="name">The name.</param>
        /// <param name="namePlural">The name plural.</param>
        /// <returns></returns>
        public measureSystemUnitEntry AddUnit(String sufix, Double factor = 0, String name = "milimeter", String namePlural = "*")
        {
            measureSystemUnitEntry output = new measureSystemUnitEntry(sufix, factor, name, this, namePlural);

            units.Add(output);

            //unitByFactor.Add(output.factor, output);

            return output;
        }

        //public measureInfo AddStringFormat(Enum name, String itemFormat, Int32 wrapWidth, Int32 wrapHeight, String sufix)
        //{
        //}

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="letter">The letter.</param>
        /// <param name="symbol">The symbol.</param>
        /// <returns></returns>
        public measureSystemRoleEntry AddRole(String name = "width", String letter = "w", String symbol = "↔")
        {
            measureSystemRoleEntry output = new measureSystemRoleEntry(letter, symbol, name);

            roles.Add(name, output);

            return output;
        }

        /// <summary>
        /// Adds the role for numberic measures
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="letter">The letter.</param>
        /// <param name="symbol">The symbol.</param>
        /// <returns></returns>
        public measureSystemRoleEntry AddRole(Enum name, String letter = "w", String symbol = "↔")
        {
            return AddRole(name.ToString(), letter, symbol);
        }

        protected measureSystemUnitRegistry units = new measureSystemUnitRegistry();

        protected Dictionary<String, measureSystemRoleEntry> roles = new Dictionary<String, measureSystemRoleEntry>();

        //private List<measureSystemUnitEntry> _unitByFactor = new List<measureSystemUnitEntry>();
        ///// <summary> </summary>
        //protected List<measureSystemUnitEntry> unitByFactor
        //{
        //    get
        //    {
        //        return _unitByFactor;
        //    }
        //    private set
        //    {
        //        _unitByFactor = value;

        //    }
        //}

        private measureSystemUnitEntry _rootUnit;

        /// <summary>
        ///
        /// </summary>
        public measureSystemUnitEntry rootUnit
        {
            get
            {
                rootUnitAutofind();

                return _rootUnit;
            }
            set { _rootUnit = value; }
        }

        private measureUnitType _measureType = measureUnitType.space;

        /// <summary>
        /// Gets or sets the type of the measure.
        /// </summary>
        /// <value>
        /// The type of the measure.
        /// </value>
        public measureUnitType measureType
        {
            get { return _measureType; }
            protected set { _measureType = value; }
        }

        public double FACTOR_PRECISION
        {
            get
            {
                return 0.0000000001;
            }
        }
    }
}