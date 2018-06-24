// --------------------------------------------------------------------------------------------------------------------
// <copyright file="deliveryUnitItemFlags.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.delivery
{
    using System;

    /// <summary>
    /// Execution flags
    /// </summary>
    [Flags]
    public enum deliveryUnitItemFlags
    {
        none = 0,

        /// <summary>
        /// The item is contained in an archive file
        /// </summary>
        zippedSource = 1,

        /// <summary>
        /// Output filenames are data templates that should be preprocessed before used
        /// </summary>
        filenameIsDataTemplate = 2,

        /// <summary>
        /// It will prevend deliveryUnit to change any predefined filename extension
        /// </summary>
        filenameExtensionIsStatic = 4,

        /// <summary>
        /// It will create proper link within primary content. i.e. for CSS file it will include style tag in head part of HTML output
        /// </summary>
        linkToPrimaryContent = 8,

        useTemplate = 16,

        useCopy = 32,

        deployTemplateOnPrepare = 64,
    }
}