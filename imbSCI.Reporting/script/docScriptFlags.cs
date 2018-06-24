// --------------------------------------------------------------------------------------------------------------------
// <copyright file="docScriptFlags.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.script
{
    using System;

    [Flags]
    public enum docScriptFlags
    {
        none = 0,
        ignoreNavigation = 1,

        /// <summary>
        /// It will not report error on compilation failure
        /// </summary>
        ignoreCompilationFails = 2, //

        ignoreArgumentValueNull = 4, //
        nullDirectoryToCurrent = 8,

        /// <summary>
        /// It will allow exection of <see cref="docScriptInstructionCompiled"/> even in case of compilation failure
        /// </summary>
        allowFailedInstructions = 16,

        /// <summary>
        /// The disable global collection call on <see cref="deliveryInstance.executePrepare(meta.documentSet.metaDocumentSet, string, PropertyCollection)"/>
        /// </summary>
        disableGlobalCollection = 32,

        /// <summary>
        /// It will allow AppendDataFields call on newScope when <see cref="deliveryInstance.x_scopeAutoCreate(IObjectWithPathAndChildSelector)"/> performed
        /// </summary>
        enableLocalCollection = 64,

        useDataDictionaryForLocalData = 128,
    }
}