// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStyleRender.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.render
{
    using System;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="ITabLevelControler" />
    /// \ingroup_disabled report_int
    public interface IStyleRender : ITabLevelControler, IRender
    {
        /// <summary>
        /// Renders key-> value pair
        /// </summary>
        /// <param name="key">Property name or collection key</param>
        /// <param name="value">ToString value</param>
        /// <param name="breakLine">should break line </param>
        void AppendPair(String key, Object value, Boolean breakLine = true, String between = " = ");

        /// <summary>
        /// Prefix koji se dodaje ispred svake linije
        /// </summary>
        String linePrefix { get; set; }

        /// <summary>
        /// Vraca sadrzaj u String obliku
        /// </summary>
        /// <returns></returns>
        String ToString();

        /// <summary>
        /// Zatvara sve tagove koji su trenutno otvoreni
        /// </summary>
        //void closeAll();
    }
}