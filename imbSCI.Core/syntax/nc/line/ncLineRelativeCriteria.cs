// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ncLineRelativeCriteria.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.nc.line
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Criteria, relative to the primary criteria
    /// </summary>
    public class ncLineRelativeCriteria : ncLineCriteria
    {
        #region -----------  relativePosition  -------  [Targeting relative position from the current ncLine. [0] is disabled, 1 is line after, -1 is line before, 5 is five lines after]

        private Int32 _relativePosition = 1; // = new Int32();

        /// <summary>
        /// Targeting relative position from the current ncLine. [0] is disabled, 1 is line after, -1 is line before, 5 is five lines after
        /// </summary>
        // [XmlIgnore]
        [Category("ncLineRelativeCriteria")]
        [DisplayName("Relative Position")]
        [Description("Targeting relative position from the current ncLine. [0] is disabled, 1 is line after, -1 is line before, 5 is five lines after")]
        public Int32 relativePosition
        {
            get
            {
                return _relativePosition;
            }
            set
            {
                // Boolean chg = (_relativePosition != value);
                _relativePosition = value;
                OnPropertyChanged("relativePosition");
                // if (chg) {}
            }
        }

        #endregion -----------  relativePosition  -------  [Targeting relative position from the current ncLine. [0] is disabled, 1 is line after, -1 is line before, 5 is five lines after]

        #region -----------  relativeType  -------  [Type of relative criteria: ]

        private ncLineRelativeCriteriaType _relativeType = ncLineRelativeCriteriaType.onExactPosition; // = new ncLineRelativeCriteriaType();

        /// <summary>
        /// Type of relative criteria:
        /// </summary>
        // [XmlIgnore]
        [Category("ncLineRelativeCriteria")]
        [DisplayName("relativeType")]
        [Description("Type of relative criteria: [onExactPosition] tests against ncLine on exact relative position based on Relative Position value; [anywhereWithin] tests against all lines from the current to Relative Position, including line on Relative Position; [disabled] turns off this test")]
        public ncLineRelativeCriteriaType relativeType
        {
            get
            {
                return _relativeType;
            }
            set
            {
                // Boolean chg = (_relativeType != value);
                _relativeType = value;
                OnPropertyChanged("relativeType");
                // if (chg) {}
            }
        }

        #endregion -----------  relativeType  -------  [Type of relative criteria: ]
    }
}