// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbDataExecutorEngine.cs" company="imbVeles" >
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
    using System.Collections.Generic;
    using System.Linq;

    #endregion imbVeles using

    /// <summary>
    /// Proširenje postojećih imbDataExecutor operacija
    /// Dodato prvi put zbog horizontalnog sabiranja brojeva iz stringova
    /// 28. januar 2012
    /// </summary>
    public static class imbDataExecutorEngine
    {
        /// <summary>
        /// Izvršava operaciju nad višelinijskim stringom
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="operandA"></param>
        /// <param name="operandB"></param>
        /// <returns></returns>
        public static String executeMultilineLogic(dataLogic logic, String operandA, String operandB)
        {
            String output = "";

            if (operandB == "") return operandA;

            List<String> listA_el = new List<string>();
            listA_el.AddRange(operandA.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

            List<String> listB_el = new List<string>();
            listB_el.AddRange(operandB.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

            Int32 limit = listA_el.Count();

            if (limit == 0) limit = listB_el.Count();

            switch (logic)
            {
                case dataLogic.mlSumLong:
                    if (listB_el.Count() > limit) limit = listB_el.Count();
                    break;

                case dataLogic.mlSumShort:
                    if (listB_el.Count() < limit) limit = listB_el.Count();
                    break;
            }

            Int32 a;
            Int32 x;
            Int32 y;

            for (a = 0; a < limit; a++)
            {
                try
                {
                    x = Convert.ToInt32(listA_el[a]);
                }
                catch
                {
                    x = 0;
                }

                try
                {
                    y = Convert.ToInt32(listB_el[a]);
                }
                catch
                {
                    y = 0;
                }

                output = output + (x + y).ToString() + Environment.NewLine;
            }

            return output;
        }
    }
}