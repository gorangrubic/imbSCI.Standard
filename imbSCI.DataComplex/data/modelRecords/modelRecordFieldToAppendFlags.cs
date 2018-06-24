// --------------------------------------------------------------------------------------------------------------------
// <copyright file="modelRecordFieldToAppendFlags.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataComplex.data.modelRecords
{
    using System;

    [Flags]
    public enum modelRecordFieldToAppendFlags
    {
        /// <summary>
        /// The identification: record title, record description, instance classname, <see cref="IModelRecord.state"/>, runstamp, UID, instace-ID, log size...
        /// </summary>
        identification = 1,

        /// <summary>
        /// The model record common data, <see cref="IModelRecord"/> properties: i.e. start time, end time, duration, <see cref="IModelRecord.remarkFlags"/>
        /// </summary>
        modelRecordCommonData = 2,

        /// <summary>
        /// Data from <see cref="imbACE.Core.core.builderForLog.AppendDataFields(imbSCI.Data.collection.PropertyCollectionExtended)"/> : log size statistics and exceptions detected
        /// </summary>
        modelRecordLogData = 4,

        /// <summary>
        /// The model record instance data
        /// </summary>
        modelRecordInstanceData = 8,

        /// <summary>
        /// The common data:
        /// </summary>
        commonData = 16,

        /// <summary>
        /// The algorithm shared:
        /// </summary>
        algorithmShared = 32,

        algorithmSpecifics = 64,

        algorithmCalculated = 128,
        all = identification | modelRecordCommonData | modelRecordInstanceData | commonData | algorithmShared | algorithmSpecifics | algorithmCalculated,
    }
}