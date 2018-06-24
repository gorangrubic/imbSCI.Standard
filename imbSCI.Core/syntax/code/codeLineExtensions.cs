// --------------------------------------------------------------------------------------------------------------------
// <copyright file="codeLineExtensions.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.code
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public static class codeLineExtensions
    {
        public static Regex _splitSourceCode = new Regex(@"([\w])\r|\s", RegexOptions.Multiline);

        public static Regex _getAlfabetPart = new Regex(@"[A-ZČŠĆŽĐa-zžđščć]{1,}");

        /// <summary>
        /// Tokenizuje ulazni source code - koristi univerzalnu tokenizaciju
        /// </summary>
        /// <param name="code">Izvodni kod koji obradjuje</param>
        /// <param name="removeEmptyTokens">Da li da izbaci prazne tokene?</param>
        /// <returns></returns>
        public static List<string> tokenizeCodeLine(this String code, Boolean removeEmptyTokens)
        {
            List<String> output = new List<string>();
            var tkns = _splitSourceCode.Split(code);

            foreach (var rkn in tkns)
            {
                if (!removeEmptyTokens || !String.IsNullOrEmpty(rkn))
                {
                    output.Add(rkn);
                }
            }
            return output;
        }
    }
}