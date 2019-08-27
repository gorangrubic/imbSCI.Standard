// --------------------------------------------------------------------------------------------------------------------
// <copyright file="styleTextShotProvider.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.style.shot
{
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Data.enums.appends;
    using System;

#pragma warning disable CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    /// <summary>
    /// Provider for text styling shots
    /// </summary>
    /// <seealso cref="aceCommonTypes.primitives.imbBindable" />
    public class styleTextShotProvider
#pragma warning restore CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    {
        protected styleTheme theme;

        public styleTextShotProvider(styleTheme _theme)
        {
            theme = _theme;
        }

        public styleTextShot getShotSet(appendRole role, appendType type = appendType.none)
        {
            String key = styleTextShot.getCodeName(role, type, theme);
            if (!shots.ContainsKey(key))
            {
                styleTextShot tmp = new styleTextShot(role, type, theme);
                shots.Add(key, tmp);
            }
            return shots[key];
        }

        protected styleTextShotCollection shots = new styleTextShotCollection();
    }
}