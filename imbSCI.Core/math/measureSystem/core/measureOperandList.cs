// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureOperandList.cs" company="imbVeles" >
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
namespace imbSCI.Core.math.measureSystem.core
{
    using imbSCI.Core.enums;
    using imbSCI.Core.interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///
    /// </summary>
    public class measureOperandList
    {
        private List<measureOperand> _items = new List<measureOperand>();

        /// <summary>
        ///
        /// </summary>
        public List<measureOperand> items
        {
            get { return _items; }
            set { _items = value; }
        }

        // public void reconnect(I)

        public IMeasure execute(IMeasure operandA)
        {
            IMeasure output = null;
            foreach (measureOperand mo in items)
            {
                output = output.calculate(mo.operand, mo.operationWithOperand) as IMeasure;
            }
            return output;
        }

        /// <summary>
        /// Adds the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="opera">The opera.</param>
        /// <param name="__operand">The operand.</param>
        /// <returns></returns>
        public measureOperand Add(Int32 index, operation opera, IMeasure __operand)
        {
            var output = new measureOperand(__operand.name, opera);
            output.operand = __operand;

            if (index > items.Count() - 1) index = items.Count() - 1;
            if (index < 0)
            {
                items.Add(output);
            }
            else
            {
                items.Insert(index, output);
            }
            return output;
        }

        /// <summary>
        /// Adds the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="opera">The opera.</param>
        /// <param name="operandName">Name of the operand.</param>
        /// <returns></returns>
        public measureOperand Add(Int32 index, operation opera, String operandName)
        {
            var output = new measureOperand(operandName, opera);
            if (index > items.Count() - 1) index = items.Count() - 1;
            if (index < 0)
            {
                items.Add(output);
            }
            else
            {
                items.Insert(index, output);
            }
            return output;
        }
    }
}