// --------------------------------------------------------------------------------------------------------------------
// <copyright file="modelRecordRemarkFlags.cs" company="imbVeles" >
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
    /// Pointing to important events and/or facts of this record
    /// </summary>
    [Flags]
    public enum modelRecordRemarkFlags
    {
        /// <summary>
        /// Irregular operation: the algorithm this record is following had minor irregularities during operation
        /// </summary>
        irregularOperation,

        /// <summary>
        /// Broken: one or more major errors occoured during operation of this algorithm
        /// </summary>
        broken,

        /// <summary>
        /// Significant: result this algorithm achieved is significant
        /// </summary>
        significant,

        /// <summary>
        /// Insteresting: algorithm log should be examined by researched
        /// </summary>
        insteresting,

        /// <summary>
        /// The algorithm failed on the evaluation criteria
        /// </summary>
        testCriteriaFail,

        /// <summary>
        /// The algorithm passed the evaluation criteria
        /// </summary>
        testCriteriaPass,
    }
}