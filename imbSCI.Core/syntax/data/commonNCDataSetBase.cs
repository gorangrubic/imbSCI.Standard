// --------------------------------------------------------------------------------------------------------------------
// <copyright file="commonNCDataSetBase.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.data
{
    using imbSCI.Core.syntax.data.core;
    using imbSCI.Core.syntax.data.files;
    using System;
    using System.Linq;

    public abstract class commonNCDataSetBase : textDataSetWithComments<paramPair>
    {
        /// <summary>
        /// Obradjuje jednu liniju
        /// </summary>
        /// <param name="_line"></param>
        /// <param name="i"> </param>
        /// <returns></returns>
        public override paramPair processLine(string _line, int i)
        {
            paramPair output = new paramPair();
            _line = _line.Trim();

            var lne = _line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (lne.Length == 0)
            {
                if (!String.IsNullOrEmpty(_line))
                {
                    lne = new string[] { _line };
                }
            }

            if (lne.Any())
            {
                var lnCommand = lne[0].Trim().ToUpper();
                output.name = lnCommand;
                output.value = "";
                for (int li = 1; li < lne.Length; li++)
                {
                    output.value += " " + lne[li];
                }
            }

            return output;
        }
    }
}