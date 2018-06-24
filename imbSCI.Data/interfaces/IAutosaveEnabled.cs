// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAutosaveEnabled.cs" company="imbVeles" >
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
namespace imbSCI.Data.interfaces
{
    using System;

    /// <summary>
    /// Classes that support autosave registration
    /// </summary>
    public interface IAutosaveEnabled
    {
        /// <summary>
        /// Gets a value indicating whether the instance should be registered for autosave call on application close
        /// </summary>
        /// <value>
        /// <c>true</c> if [variable register for autosave]; otherwise, <c>false</c>.
        /// </value>
        Boolean VAR_RegisterForAutosave { get; }

        /// <summary>
        /// Gets the variable filename prefix to be used
        /// </summary>
        /// <value>
        /// The variable filename prefix.
        /// </value>
        String VAR_FilenamePrefix { get; }

        /// <summary>
        /// The root base of filename (without extension) for autosave.
        /// </summary>
        /// <remarks>
        /// This should be an abstract property in abstract base classes
        /// </remarks>
        /// <value>
        /// The variable filename base.
        /// </value>
        String VAR_FilenameBase { get; }

        /// <summary>
        /// Gets the variable filename extension.
        /// </summary>
        /// <value>
        /// The variable filename extension.
        /// </value>
        String VAR_FilenameExtension { get; }

        /// <summary>
        /// Gets the variable folder path for autosave.
        /// </summary>
        /// <value>
        /// Path (from app. root) to store the record on autosave. Also used as default on regular save call.
        /// </value>
        String VAR_FolderPathForAutosave { get; }

        /// <summary>
        /// Gets the content of the log.
        /// </summary>
        /// <value>
        /// The content of the log.
        /// </value>
        String logContent { get; }
    }
}