// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cursorZoneSpatialPreset.cs" company="imbVeles" >
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
    public enum cursorZoneSpatialPreset
    {
        /// <summary>
        /// The sheet normal: t=1, wc=12, hc=120, sp=160px
        /// </summary>
        sheetNormal,

        /// <summary>
        /// 20x20px cell dimension
        /// </summary>
        sheetSquareCell,

        /// <summary>
        /// The text page - t=8, wc=100, hc=60, sp=10
        /// </summary>
        textPage,

        /// <summary>
        /// The console - wc85 x hc43, t=4
        /// </summary>
        console,

        /// <summary>
        /// The wide console - wc=160, hc=78
        /// </summary>
        wideConsole,

        /// <summary>
        /// The a4 on font size 10pt
        /// </summary>
        a4OnFont10pt,

        longTextLog,
    }
}