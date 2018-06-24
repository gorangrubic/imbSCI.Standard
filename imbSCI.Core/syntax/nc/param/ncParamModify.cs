// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ncParamModify.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.nc.param
{
    using imbSCI.Core.syntax.param;
    using imbSCI.Data.data;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// How parameter should be modified
    /// </summary>
    public class ncParamModify : dataBindableBase
    {
        internal Int32 parIndex = -1;

        #region -----------  parameterTarget  -------  [Key or ordinal zero-index (0 for the first parameter) pointing to parameter to change]

        private String _parameterTarget = "1"; // = new String();

        /// <summary>
        /// Key or ordinal zero-index (0 for the first parameter) pointing to parameter to change
        /// </summary>
        // [XmlIgnore]
        [Category("ncParamModify")]
        [DisplayName("Parameter target")]
        [Description("Key (case ignored) or ordinal zero-index (1 for the first parameter) pointing to parameter in a NC line. Avoid using string key (like: X, Y or C) - for legacy support 1, 2 or 3 are preferred.")]
        public String parameterTarget
        {
            get
            {
                return _parameterTarget;
            }
            set
            {
                // Boolean chg = (_parameterTarget != value);
                _parameterTarget = value;
                Int32 _parIndex = -1;
                if (Int32.TryParse(_parameterTarget, out _parIndex))
                {
                    parIndex = _parIndex;
                }
                else
                {
                    parIndex = -1;
                }

                OnPropertyChanged("parameterTarget");
                // if (chg) {}
            }
        }

        #endregion -----------  parameterTarget  -------  [Key or ordinal zero-index (0 for the first parameter) pointing to parameter to change]

        #region -----------  modificationType  -------  [Way of parameter modification]

        private paramValueModificationType _modificationType = paramValueModificationType.nochange; // = new ncToolModifyType();

        /// <summary>
        /// Way of parameter modification
        /// </summary>
        // [XmlIgnore]
        [Category("ncParamModify")]
        [DisplayName("Modification type")]
        [Description("Way of parameter modification: nochange - ignore, increase - calculate new value, setvalue - set param to value without calculation")]
        public paramValueModificationType modificationType
        {
            get
            {
                return _modificationType;
            }
            set
            {
                // Boolean chg = (_modificationType != value);
                _modificationType = value;
                OnPropertyChanged("modificationType");
                // if (chg) {}
            }
        }

        #endregion -----------  modificationType  -------  [Way of parameter modification]

        #region -----------  modificationValue  -------  [Value to apply for modification]

        private String _modificationValue = "0.000"; // = new String();

        /// <summary>
        /// Value to apply for modification
        /// </summary>
        // [XmlIgnore]
        [Category("ncParamModify")]
        [DisplayName("Value")]
        [Description("Value to apply for modification - both decimal and literal are allowed.")]
        public String modificationValue
        {
            get
            {
                return _modificationValue;
            }
            set
            {
                // Boolean chg = (_modificationValue != value);
                _modificationValue = value;
                OnPropertyChanged("modificationValue");
                // if (chg) {}
            }
        }

        #endregion -----------  modificationValue  -------  [Value to apply for modification]

        #region -----------  doEnforceFormat  -------  [Should parameter accept format from Value?]

        private Boolean _doEnforceFormat = false;// = new Boolean();

        /// <summary>
        /// Should parameter accept format from Value?
        /// </summary>
        // [XmlIgnore]
        [Category("ncParamModify")]
        [DisplayName("Enforce format")]
        [Description("(deprecated) Should parameter accept format from Value?")]
        public Boolean doEnforceFormat
        {
            get
            {
                return _doEnforceFormat;
            }
            set
            {
                // Boolean chg = (_doEnforceFormat != value);
                _doEnforceFormat = value;
                OnPropertyChanged("doEnforceFormat");
                // if (chg) {}
            }
        }

        #endregion -----------  doEnforceFormat  -------  [Should parameter accept format from Value?]

        #region -----------  doAddOnMissing  -------  [If parameter not found on Target key/index - it will be added]

        private Boolean _doAddOnMissing = false; // = new Boolean();

        /// <summary>
        /// If parameter not found on Target key/index - it will be added
        /// </summary>
        // [XmlIgnore]
        [Category("ncParamModify")]
        [DisplayName("Add missing")]
        [Description("(UNDERDEVELOPMENT) If parameter not found on Target key/index - it will be added")]
        public Boolean doAddOnMissing
        {
            get
            {
                return _doAddOnMissing;
            }
            set
            {
                // Boolean chg = (_doAddOnMissing != value);
                _doAddOnMissing = value;
                OnPropertyChanged("doAddOnMissing");
                // if (chg) {}
            }
        }

        #endregion -----------  doAddOnMissing  -------  [If parameter not found on Target key/index - it will be added]
    }
}