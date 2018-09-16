// --------------------------------------------------------------------------------------------------------------------
// <copyright file="functionBase.cs" company="imbVeles" >
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

namespace imbSCI.Core.math.functions.core
{
    /// <summary>
    /// Base of function generator class
    /// </summary>
    public abstract class functionBase : IFunctionGenerator
    {
        protected functionBase()
        {
        }

        protected functionBase(numberRangePresetEnum preset)
        {
            outputRange = new imbNumberScale(preset);
        }

        /// <summary>
        /// Gets or sets the output range.
        /// </summary>
        /// <value>
        /// The output range.
        /// </value>
        public imbNumberScale outputRange { get; set; } = new imbNumberScale(0, 1);

        ///// <summary>
        ///// Gets or sets the alpha dimension.
        ///// </summary>
        ///// <value>
        ///// The alpha dimension.
        ///// </value>
        //public imbNumberScale alphaDimension { get; set; } = new imbNumberScale(0, 1);

        /// <summary>
        /// Gets the output.
        /// </summary>
        /// <param name="alpha">The alpha.</param>
        /// <returns></returns>
        public abstract double GetOutput(double alpha = 0);

        /// <summary>
        /// Gets the output.
        /// </summary>
        /// <param name="alpha">The alpha.</param>
        /// <returns></returns>
        public abstract double GetOutput(int alpha);
    }
}