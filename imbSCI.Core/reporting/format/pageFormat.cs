// --------------------------------------------------------------------------------------------------------------------
// <copyright file="pageFormat.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.format
{
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.data;
    using System.Drawing;

    /// <summary>
    /// Describes formatring of an page
    /// </summary>
    /// <seealso cref="aceCommonTypes.primitives.imbBindable" />
    /// <seealso cref="cursorZoneExecutionSettings"/>
    public class pageFormat : imbBindable
    {
        private acePaletteRole _mainColor = acePaletteRole.colorA;

        /// <summary>
        /// Sets color of tab and backgrouns
        /// </summary>
        public acePaletteRole mainColor
        {
            get { return _mainColor; }
            set { _mainColor = value; }
        }

        private Color _tabHeadColor = Color.White;

        /// <summary> </summary>
        public Color tabHeadColor
        {
            get
            {
                return _tabHeadColor;
            }
            set
            {
                _tabHeadColor = value;
                OnPropertyChanged("tabHeadColor");
            }
        }

        #region --- zoneSpatialPreset ------- Preset with padding, margin and other spatial configuration applicable on a page

        private cursorZoneSpatialPreset _zoneSpatialPreset = cursorZoneSpatialPreset.sheetNormal;

        /// <summary>
        /// Preset with padding, margin and other spatial configuration applicable on a page
        /// </summary>
        public cursorZoneSpatialPreset zoneSpatialPreset
        {
            get
            {
                return _zoneSpatialPreset;
            }
            set
            {
                _zoneSpatialPreset = value;
                OnPropertyChanged("zoneSpatialPreset");
            }
        }

        #endregion --- zoneSpatialPreset ------- Preset with padding, margin and other spatial configuration applicable on a page

        #region --- zoneLayoutPreset ------- Preset to be applied once new page is started

        private cursorZoneLayoutPreset _zoneLayoutPreset = cursorZoneLayoutPreset.twoColumn;

        /// <summary>
        /// Preset to be applied once new page is started
        /// </summary>
        public cursorZoneLayoutPreset zoneLayoutPreset
        {
            get
            {
                return _zoneLayoutPreset;
            }
            set
            {
                _zoneLayoutPreset = value;
                OnPropertyChanged("zoneLayoutPreset");
            }
        }

        #endregion --- zoneLayoutPreset ------- Preset to be applied once new page is started
    }
}