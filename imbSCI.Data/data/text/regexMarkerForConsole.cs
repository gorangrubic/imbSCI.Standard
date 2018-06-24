// --------------------------------------------------------------------------------------------------------------------
// <copyright file="regexMarkerForConsole.cs" company="imbVeles" >
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
// Project: imbSCI.Data
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Data.data.text
{
    using imbSCI.Data.enums;
    using System;

    /// <summary>
    /// Regex marker applied to parse some markdown-like annotation in the text message to be displayed at Console Application output
    /// </summary>
    /// <seealso cref="aceCommonTypes.data.text.regexMarker{aceCommonTypes.data.text.consoleStyleMarkerEnum}" />
    public class regexMarkerForConsole : regexMarker<consoleStyleMarkerEnum>
    {
        /// <summary>
        /// Gets or sets the foreground color to be attached to the token matching the <see cref="regexMarker{T}.test"/>
        /// </summary>
        /// <value>
        /// The foreground.
        /// </value>
        public ConsoleColor foreground { get; set; } = ConsoleColor.White;

        /// <summary>
        /// Gets or sets the background color attached to the token matching the <see cref="regexMarker{T}.test"/>
        /// </summary>
        /// <value>
        /// The background.
        /// </value>
        public ConsoleColor background { get; set; } = ConsoleColor.DarkGray;

        /// <summary>
        /// Initializes a new instance of the <see cref="regexMarkerForConsole"/> parser rule
        /// </summary>
        /// <param name="regex">The regex test for tokens</param>
        /// <param name="__m">The style marker it represents</param>
        /// <param name="fore">The foreground color, <see cref="foreground"/></param>
        /// <param name="back">The background color, <see cref="background"/>.</param>
        public regexMarkerForConsole(string regex, consoleStyleMarkerEnum __m, ConsoleColor fore, ConsoleColor back) : base(regex, __m)
        {
            foreground = fore;
            background = back;
        }
    }
}