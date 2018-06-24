// --------------------------------------------------------------------------------------------------------------------
// <copyright file="paramPairs.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.data.core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Skup parametara
    /// </summary>
    public class paramPairs : Dictionary<string, paramPair>, IEnumerable<paramPair>
    {
        public void Add(paramPair input)
        {
            if (input == null) return;
            if (String.IsNullOrEmpty(input.name)) return;
            Add(input.name, input);
        }

        /// <summary>
        /// Vraca vrednost parametra. Ako ne nadje pod tim imenom pokusava ToUpper i ToLower varijante. Ako ne uspe vraca notFound vrednost
        /// </summary>
        /// <param name="paramName">Ime parametra koji se trazi. Automatski ce uraditi trim.</param>
        /// <param name="_notFound">Vrednost koju treba da vrati ako nije nadjen</param>
        /// <returns>Trimovana vrednost parametra</returns>
        public String getParamValue(String paramName, String _notFound = "")
        {
            if (String.IsNullOrEmpty(paramName)) return _notFound;
            paramName = paramName.Trim();
            if (String.IsNullOrEmpty(paramName)) return _notFound;

            if (!ContainsKey(paramName)) paramName = paramName.ToUpper();
            if (!ContainsKey(paramName)) paramName = paramName.ToLower();
            if (!ContainsKey(paramName)) return _notFound;

            String output = this[paramName].value;
            output = output.Replace(";", "");

            if (!String.IsNullOrEmpty(output)) output = output.Trim();
            return output;
        }

        public Double getParamValue(String paramName, Double _notFound = 0)
        {
            String _val = getParamValue(paramName, _notFound.ToString());
            return Double.Parse(_val);
        }

        public Int32 getIntValue(String paramName)
        {
            String _val = getParamValue(paramName, "0");
            return Int32.Parse(_val);
        }

        public Double getDoubleValue(String paramName)
        {
            String _val = getParamValue(paramName, "0");
            return Double.Parse(_val);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<paramPair> GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }
    }
}