// --------------------------------------------------------------------------------------------------------------------
// <copyright file="stylePresets.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.style
{
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.geometrics;
    using imbSCI.Core.reporting.style.core;

    /// <summary>
    ///
    /// </summary>
    public static class stylePresets
    {
        #region --- themeCompany ------- Company theme

        private static styleTheme _themeCompany;

        /// <summary>
        /// Company theme
        /// </summary>
        public static styleTheme themeCompany
        {
            get
            {
                if (_themeCompany == null)
                {
                    _themeCompany = new styleTheme(aceBaseColorSetEnum.aceCompany, 16, 10, new fourSideSetting(2), new fourSideSetting(1), enums.aceFont.Impact, enums.aceFont.Helvetica);
                }
                return _themeCompany;
            }
        }

        #endregion --- themeCompany ------- Company theme

        #region --- themeScience ------- scientific theme

        private static styleTheme _themeScience;

        /// <summary>
        /// scientific theme
        /// </summary>
        public static styleTheme themeScience
        {
            get
            {
                if (_themeScience == null)
                {
                    _themeScience = new styleTheme(aceBaseColorSetEnum.imbScience, 20, 12, new fourSideSetting(4), new fourSideSetting(2), enums.aceFont.Courier, enums.aceFont.Courier);
                }
                return _themeScience;
            }
        }

        #endregion --- themeScience ------- scientific theme

        #region --- themeBright ------- Theme bright and strong colors

        private static styleTheme _themeBright;

        /// <summary>
        /// Theme bright and strong colors
        /// </summary>
        public static styleTheme themeBright
        {
            get
            {
                if (_themeBright == null)
                {
                    _themeBright = new styleTheme(aceBaseColorSetEnum.aceBrightAndStrong, 18, 11, new fourSideSetting(3), new fourSideSetting(1), enums.aceFont.Georgia, enums.aceFont.Georgia);
                }
                return _themeBright;
            }
        }

        #endregion --- themeBright ------- Theme bright and strong colors

        #region --- themeSemantics ------- theme for semantic analysis

        private static styleTheme _themeSemantics;

        /// <summary>
        /// theme for semantic analysis
        /// </summary>
        public static styleTheme themeSemantics
        {
            get
            {
                if (_themeSemantics == null)
                {
                    _themeSemantics = new styleTheme(aceBaseColorSetEnum.imbSemantics, 19, 8, new fourSideSetting(4), new fourSideSetting(1), enums.aceFont.Impact, enums.aceFont.Impact);
                }
                return _themeSemantics;
            }
        }

        #endregion --- themeSemantics ------- theme for semantic analysis
    }
}