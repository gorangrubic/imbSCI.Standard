// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWeightTableTerm.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex
{
    using imbSCI.Data.interfaces;
    using System.Collections.Generic;

    /// <summary>
    /// Term that is subject of <see cref="IWeightTable"/>
    /// </summary>
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithName" />
    public interface IWeightTableTerm : IObjectWithName
    {
        /// <summary>
        /// Defines term name and nominal form
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="nominalForm">The nominal form.</param>
        void Define(string name, string nominalForm);

        string nominalForm { get; }

        List<string> GetAllForms(bool includingNominalForm = true);

        /// <summary>
        /// Sets semantic instances for this term
        /// </summary>
        /// <param name="instances">The instances.</param>
        void SetOtherForms(IEnumerable<string> instances);

        /// <summary>
        /// Determines whether the specified <c>other</c> <see cref="IWeightTableTerm"/> is match with this one (meaning their frequencies are summed)
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>
        ///   <c>true</c> if the specified other is match; otherwise, <c>false</c>.
        /// </returns>
        bool isMatch(IWeightTableTerm other);

        /// <summary>
        /// Frequency points that should be added to the term
        /// </summary>
        /// <value>
        /// a freq points.
        /// </value>
        int AFreqPoints { get; }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        double weight { get; set; }

        int Count { get; }
    }
}