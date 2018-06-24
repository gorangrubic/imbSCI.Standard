// --------------------------------------------------------------------------------------------------------------------
// <copyright file="stylePage.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.style.core
{
    using imbSCI.Core.reporting.geometrics;
    using imbSCI.Data.data;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Page style
    /// </summary>
    public class stylePage : imbBindable
    {
        #region -----------  maxSize  -------  [max size of the page]

        private styleSize _maxSize = new styleSize();

        /// <summary>
        /// max size of the page
        /// </summary>
        // [XmlIgnore]
        [Category("stylePage")]
        [DisplayName("maxSize")]
        [Description("max size of the page")]
        public styleSize maxSize
        {
            get
            {
                return _maxSize;
            }
            set
            {
                // Boolean chg = (_maxSize != value);
                _maxSize = value;
                OnPropertyChanged("maxSize");
                // if (chg) {}
            }
        }

        #endregion -----------  maxSize  -------  [max size of the page]

        #region -----------  linePercent  -------  [line height percent]

        private Int32 _linePercent = 100; // = new Int32();

        /// <summary>
        /// line height percent
        /// </summary>
        // [XmlIgnore]
        [Category("stylePage")]
        [DisplayName("linePercent")]
        [Description("line height percent")]
        public Int32 linePercent
        {
            get
            {
                return _linePercent;
            }
            set
            {
                // Boolean chg = (_linePercent != value);
                _linePercent = value;
                OnPropertyChanged("linePercent");
                // if (chg) {}
            }
        }

        #endregion -----------  linePercent  -------  [line height percent]

        #region -----------  margins  -------  [margins of page inside documents]

        private fourSideSetting _margins = new fourSideSetting(10, 10);

        /// <summary>
        /// margins of page inside documents
        /// </summary>
        // [XmlIgnore]
        [Category("stylePage")]
        [DisplayName("margins")]
        [Description("margins of page inside documents")]
        public fourSideSetting margins
        {
            get
            {
                return _margins;
            }
            set
            {
                // Boolean chg = (_margins != value);
                _margins = value;
                OnPropertyChanged("margins");
                // if (chg) {}
            }
        }

        #endregion -----------  margins  -------  [margins of page inside documents]
    }
}