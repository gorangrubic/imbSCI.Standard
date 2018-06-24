// --------------------------------------------------------------------------------------------------------------------
// <copyright file="modelRecordMode.cs" company="imbVeles" >
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

    /// <summary>
    /// Enumeration used by model execution records
    /// </summary>
    [Flags]
    public enum modelRecordMode
    {
        none = 0,
        single = 1,
        multi = 2,
        starter = 4,
        singleStarter = single | starter,
        multiStarter = multi | starter,
        nonStarter = 8,

        particularScope = 16,
        summaryScope = 32,

        obligationInverseOption = 64,

        /// <summary>
        /// The obligation related to RecordStart operation
        /// </summary>
        obligationOnStart = 512,

        /// <summary>
        /// The obligation related to RecordInit operation
        /// </summary>
        obligationOnInit = 1024,

        /// <summary>
        /// The obligation related to RecordFinish operation
        /// </summary>
        obligationOnFinish = 2048,

        obligationDataSet = 128,
        obligationInitBeforeStart = 4096,

        obligationStartBeforeInit = 8192,

        obligationStartBeforeFinish = 16384,

        obligationPropertyList = 32768,
        obligationBuildSummaryStatistics = 65536,
        callDataSetBuildOnFinish = 131072,

        // particular = particularScope | singleStarter | obligationDataSet,
        // summary = summaryScope | multiStarter,
        // special = globalScope | nonStarter,
    }
}