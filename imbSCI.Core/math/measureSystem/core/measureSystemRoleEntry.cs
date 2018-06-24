// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureSystemRoleEntry.cs" company="imbVeles" >
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
namespace imbSCI.Core.math.measureSystem.core
{
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Data;
    using System;

    /// <summary>
    /// Possible application role of a measuteSysteUnitEntry
    /// </summary>
    public class measureSystemRoleEntry : ICloneable
    {
        private String _separator = "";

        /// <summary>
        ///
        /// </summary>
        public String separator
        {
            get { return _separator; }
            set { _separator = value; }
        }

        internal measureSystemRoleEntry()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="measureSystemRoleEntry"/> class.
        /// </summary>
        /// <param name="__letter">The letter.</param>
        /// <param name="__symbol">The symbol.</param>
        /// <param name="__name">The name.</param>
        public measureSystemRoleEntry(String __letter, String __symbol, String __name)
        {
            _letter = __letter;
            _symbol = __symbol;
            _name = __name;
        }

        /// <summary>
        /// Sets format overrides. Leave blank to disable overrides.
        /// </summary>
        /// <param name="__formatForValue">The format to override <see cref="measureSystemUnitEntry.formatForValue"/> value at <see cref="measureSystemUnitEntry"/> .</param>
        /// <param name="__formatForValueAndUnit">The format to override <see cref="measureSystemUnitEntry.formatForValueAndUnit"/> value at <see cref="measureSystemUnitEntry"/></param>
        public measureSystemRoleEntry setFormat(String __formatForValue, String __formatForValueAndUnit = "", String __separator = "")
        {
            valueFormatOverride = __formatForValue;
            valueAndUnitFormatOverride = __formatForValueAndUnit;
            if (valueAndUnitFormatOverride == "*")
            {
                valueAndUnitFormatOverride = imbSciStringExtensions.add(valueFormatOverride, "{1}", " ");
            }
            return this;
        }

        public String checkRoleUnitOverride(String __unit)
        {
            if (imbSciStringExtensions.isNullOrEmptyString(unitOverride))
            {
                return __unit;
            }
            else
            {
                return unitOverride;
            }
        }

        public void setUnitSufixOverride(String __unitOverride)
        {
            unitOverride = __unitOverride;
        }

        public object Clone()
        {
            var tmp = new measureSystemRoleEntry();
            tmp.setObjectBySource(this);
            return tmp;
        }

        private String _unitOverride = "";

        /// <summary>
        ///
        /// </summary>
        public String unitOverride
        {
            get { return _unitOverride; }
            set { _unitOverride = value; }
        }

        private String _valueFormatOverride = "";

        /// <summary>
        /// The value format override (to be applied instead of unit's format)
        /// </summary>
        /// <value>
        /// The value format override.
        /// </value>
        public String valueFormatOverride
        {
            get { return _valueFormatOverride; }
            private set { _valueFormatOverride = value; }
        }

        private String _valueAndUnitFormatOverride = "";

        /// <summary>
        /// The valueAndUnit format override (to be applied instead of unit's format)
        /// </summary>
        /// <value>
        /// The value and unit format override.
        /// </value>
        public String valueAndUnitFormatOverride
        {
            get { return _valueAndUnitFormatOverride; }
            private set { _valueAndUnitFormatOverride = value; }
        }

        private String _letter = "w";

        /// <summary> </summary>
        public String letter
        {
            get
            {
                return _letter;
            }
            set
            {
                _letter = value;
            }
        }

        private String _symbol = "↔";

        /// <summary> </summary>
        public String symbol
        {
            get
            {
                return _symbol;
            }
            set
            {
                _symbol = value;
            }
        }

        private String _name = "width";

        /// <summary> </summary>
        public String name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
    }
}