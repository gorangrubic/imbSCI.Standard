// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cursorVariatorState.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.zone
{
    using System;

    //public static class cursorVariatorTools {
    //    public static Int32

    //}

    /// <summary>
    /// State of coordinates after tested by cursorVariator
    /// </summary>
    public struct cursorVariatorState
    {
        public Boolean isMinor;
        public Boolean isMajor;
        public Boolean isOutside;
        public Boolean isInside;
        public Boolean isLast;
        public Boolean isFirst;
        public Boolean isHeadZone;
        public Boolean isFootZone;

        /// <summary>
        /// Is coordinate inside head zone extended - the zone between headZone and normal rows
        /// </summary>
        public Boolean isHeadZoneExtended;

        public Boolean isFootZoneExtended;
        public Boolean isRightZone;
        public Boolean isLeftZone;
        public Boolean isOdd;
        public Boolean isEven;

        public Int32 x;
        public Int32 y;

        /// <summary>
        /// If TRUE it will use color for layout
        /// </summary>
        public Boolean useLayoutPalette;

        /// <summary>
        /// Applied to select color brightness from pallete
        /// </summary>
        public Int32 useColorIndex;

        /// <summary>
        /// The use inverted - inverts in
        /// </summary>
        public Boolean useInvertedForeground;
    }
}