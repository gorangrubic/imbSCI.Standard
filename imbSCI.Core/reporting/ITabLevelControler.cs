// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITabLevelControler.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting
{
    using imbSCI.Core.reporting.render.core;
    using System;

    public interface ITabLevelControler
    {
        /// <summary>
        /// Prefix koji se dodaje ispred teksta -- tabovi
        /// </summary>
        String tabInsert { get; }

        /// <summary>
        /// nivo na kome je tab sada
        /// </summary>
        Int32 tabLevel { get; set; }

        /// <summary>
        /// Prefix koji se dodaje ispred svake linije
        /// </summary>
        String linePrefix { get; set; }

        void Clear();

        /// <summary>
        /// Prelazi u sledeci tab level
        /// </summary>
        void nextTabLevel();

        /// <summary>
        /// Prebacuje u prethodni tabLevel
        /// </summary>
        void prevTabLevel();

        void rootTabLevel();

        /// <summary>
        /// Vraca sadrzaj u String obliku
        /// </summary>
        /// <returns></returns>
        String ToString();

        // void open(String tag, String title = "", String description = "");

        tagBlock open(String tag, String title = "", String description = "");

        /// <summary>
        /// Dodaje HTML zatvaranje taga -- zatvara poslednji koji je otvoren
        /// </summary>
        /// <remarks>
        /// Ako je prosledjeni tag none onda zatvara poslednji tag koji je otvoren.
        /// </remarks>
        /// <param name="tag"></param>
        tagBlock close(String tag = "none");

        /// <summary>
        /// Zatvara sve tagove koji su trenutno otvoreni
        /// </summary>
        void closeAll();
    }
}