// --------------------------------------------------------------------------------------------------------------------
// <copyright file="typedParamInfoCollection.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.param
{
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.interfaces;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Skup typedParamInfo deklaracija (bez dodeljenih vrednosti).
    /// </summary>
    public class typedParamInfoCollection : List<typedParamInfo>, IGetToSetFromString
    {
        public typedParamInfoCollection()
        {
        }

        public typedParamInfoCollection(String paramList, typedParamDeclarationType declaration = typedParamDeclarationType.nameDoubleDotType)
        {
            setFromString(paramList, declaration);
        }

        /// <summary>
        /// Makes string declaration of the param;
        /// </summary>
        /// <param name="declaration">Declaration format</param>
        /// <returns></returns>
        public String getToString(typedParamDeclarationType declaration = typedParamDeclarationType.nameDoubleDotType)
        {
            String output = "";
            String separator = declaration.getSeparator();

            foreach (var pr in this)
            {
                output = imbSciStringExtensions.add(output, pr.getToString(declaration), separator); //, (pr != this[Count - 1]));
            }
            return output;
        }

        /// <summary>
        /// Deprecated
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="declaration"></param>
        /// <returns></returns>
        public void setFromString(String paramList, typedParamDeclarationType declaration = typedParamDeclarationType.nameDoubleDotType)
        {
            String separator = declaration.getSeparator();

            paramList = paramList.Trim();
            if (paramList.Contains(separator))
            {
                var parts = paramList.Split(separator.ToCharArray());
                foreach (var pr in parts)
                {
                    Add(new typedParamInfo(pr.Trim(), declaration));
                }
            }
            else
            {
                Add(new typedParamInfo(paramList, declaration));
            }
            //return Count - c;
        }
    }
}