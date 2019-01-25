// --------------------------------------------------------------------------------------------------------------------
// <copyright file="operation.cs" company="imbVeles" >
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
using System.Linq;

namespace imbSCI.Core.enums
{
public static class operationExtensions
    {
        /// <summary>
        /// Compresses the numeric vector.
        /// </summary>
        /// <param name="nVector">The n vector.</param>
        /// <param name="operation">Allowed operations: <see cref="operation.multiplication"/>, <see cref="operation.max"/>, <see cref="operation.min"/>, <see cref="operation.plus"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Operation [" + operation.ToString() + "] not supported by this method. - operation</exception>
        public static Double CompressNumericVector(this Double[] dimensions, operation operation)
        {
            if (dimensions.Length == 0) return 0;

            switch (operation)
            {

                case operation.multiplication:
                    Double o = 1;
                    for (int i = 0; i < dimensions.Length; i++)
                    {
                        o = o * dimensions[i];
                    }
                    return o;
                    break;
                case operation.avg:
                    return dimensions.Average();
                    break;
                case operation.max:
                    return dimensions.Max();
                    break;
                case operation.min:
                    return dimensions.Min();
                    break;
                case operation.plus:
                    return dimensions.Sum();
                    break;
                default:

                    Double d = dimensions[0];
                    for (int i = 1; i < dimensions.Length; i++)
                    {
                        d = numberRange.compute(operation, d, dimensions[i]);
                    }

                    return d;


                    //  throw new ArgumentOutOfRangeException("Operation [" + operation.ToString() + "] not supported by this method.", nameof(operation));
                    break;
            }
        }

    }
}