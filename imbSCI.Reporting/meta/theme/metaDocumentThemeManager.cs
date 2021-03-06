﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaDocumentThemeManager.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.meta.theme
{
    using imbSCI.Core.reporting.style.enums;

    /// <summary>
    /// Maganer for document themes
    /// </summary>
    public static class metaDocumentThemeManager
    {
        /// <summary>
        /// nepotreban metod - moze odmah da se poziva preset[]
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static metaDocumentTheme getTheme(string name)
        {
            return preset[name];
        }

        /// <summary>
        /// Called on startup
        /// </summary>
        internal static void prepare()
        {
            preset.makeStandardTheme("standard", "#143436", "#232323", "#FF9900", aceFont.Helvetica, aceFont.Impact);
        }

        #region --- themes ------- static and autoinitiated object

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static metaDocumentThemeCollection preset { get; } = new metaDocumentThemeCollection();

        #endregion --- themes ------- static and autoinitiated object
    }
}