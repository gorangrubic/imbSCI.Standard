// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFunctionGenerator.cs" company="imbVeles" >
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
using imbSCI.Core.math.range;
using System;

namespace imbSCI.Core.math.functions.core
{
    /// <summary>
    /// Interface for function classes, used for procedural generation of [whatever]s :)
    /// </summary>
    public interface IFunctionGenerator
    {
        /// <summary>
        /// Output scalling range adjusts function output before returning final result from <see cref="GetOutput(double)"/> and <see cref="GetOutput(int)"/> methods
        /// </summary>
        /// <value>
        /// The output range.
        /// </value>
        imbNumberScale outputRange { get; set; }

        /// <summary>
        /// Gets the output.
        /// </summary>
        /// <param name="alpha">The alpha.</param>
        /// <returns></returns>
        Double GetOutput(Double alpha = 0);

        /// <summary>
        /// Gets the output.
        /// </summary>
        /// <param name="alpha">The alpha.</param>
        /// <returns></returns>
        Double GetOutput(Int32 alpha);
    }
}