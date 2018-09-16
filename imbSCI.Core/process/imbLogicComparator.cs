// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbLogicComparator.cs" company="imbVeles" >
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
namespace imbSCI.Core.process
{
    #region imbVeles using

    using System;

    #endregion imbVeles using

    public static class imbLogicComparator
    {
        #region imbLogicalOperator enum

        /// <summary>
        /// način poređenja
        /// </summary>
        public enum imbLogicalOperator
        {
            AND,
            NOT,
            OR,
            EQUAL
        }

        #endregion imbLogicalOperator enum

        /// <summary>
        /// Upoređuje dva Booleana
        /// </summary>
        /// <param name="operantA"></param>
        /// <param name="operantB"></param>
        /// <param name="comparator"></param>
        /// <returns></returns>
        public static Boolean compareBooleanValues(Object operantA, Object operantB, imbLogicalOperator comparator)
        {
            Boolean output = false;
            Boolean opA;
            Boolean opB;

            if (operantA.GetType() == operantB.GetType())
            {
                if (operantA.GetType() == typeof(Boolean))
                {
                    opA = (Boolean)operantA;
                    opB = (Boolean)operantB;

                    switch (comparator)
                    {
                        case imbLogicalOperator.AND:
                            output = opA && opB;
                            break;

                        case imbLogicalOperator.EQUAL:
                            output = (opA == opB);
                            break;

                        case imbLogicalOperator.NOT:
                            output = (opA == !opB);
                            break;

                        case imbLogicalOperator.OR:
                            output = (opA || opB);
                            break;
                    }
                }
                else
                {
                    output = false;
                }
            }
            else
            {
                output = false;
            }

            return output;
        }
    }
}