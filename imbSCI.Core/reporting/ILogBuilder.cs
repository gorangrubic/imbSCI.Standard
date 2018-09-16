// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogBuilder.cs" company="imbVeles" >
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
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.render;
    using System;

    /// <summary>
    /// Interface for loggers
    /// </summary>
    /// <seealso cref="imbSCI.Core.interfaces.ILogable" />
    /// <seealso cref="imbSCI.Core.interfaces.IAceLogable" />
    /// <seealso cref="imbSCI.Core.reporting.render.ITextRender" />
    /// <seealso cref="imbSCI.Core.reporting.IConsoleControl" />
    public interface ILogBuilder : ILogable, IAceLogable, ITextRender, IConsoleControl
    {
        ILogBuilder logStartPhase(String title, String message);

        ILogBuilder logEndPhase();

        /// <summary>
        /// Sets the alternative color mode for console output. Use set exact to set exactly the value for alternative color, otherwise it works in Toggle mode.
        /// </summary>
        /// <param name="altChange">The alt change.</param>
        /// <param name="setExact">if set to <c>true</c> [set exact].</param>
        void consoleAltColorToggle(Boolean setExact = false, Int32 altChange = -1);

        /// <summary>
        /// Path where to save so far logged content
        /// </summary>
        /// <param name="destination_path">The destination path.</param>
        void save(String destination_path = "");
    }
}