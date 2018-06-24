// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbStringGenerators.cs" company="imbVeles" >
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
namespace imbSCI.Core.extensions.text
{
    #region imbVeles using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion imbVeles using

    /// <summary>
    /// Ekstenzije koje generisu stringove za razlicite namene> univerzalni ID ili tako nesto
    /// </summary>
    /// \ingroup_disabled ace_ext_string
    public static class imbStringGenerators
    {
        /// <summary>
        /// Generise "serijski" broj - tj. na broj dodaje leading 0 i spliter koji mu se zada
        /// </summary>
        /// <param name="number">Ulazni broj koji se formatira u serijsku formu</param>
        /// <param name="spaces">Broj karaktera u broju</param>
        /// <param name="spliter"></param>
        /// <returns>Formatiran broj</returns>
        public static String getSerial(this Int32 number, Int32 spaces = 3, String spliter = "_")
        {
            String numberMark;
            numberMark = Convert.ToString(number);
            numberMark = numberMark.PadLeft(spaces, Convert.ToChar("0"));
            numberMark = spliter + numberMark + "";
            return numberMark;
        }

        public static Random random = new Random();

        /// <summary>
        /// Generise random string zadate duzine
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string getRandomString(Int32 length = 32)
        {
            String output = "";
            int randNumber;
            // Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                if (random.Next(1, 3) == 1)
                    randNumber = random.Next(97, 123); //char {a-z}
                else
                    randNumber = random.Next(48, 58); //int {0-9}

                output = output + (char)randNumber;
            }

            return output;
        }

        public delegate Boolean IsNameTaken(String nameProposal);

        /// <summary>
        /// Creates unique string name. Refusal delegate returns true
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <param name="test">The test.</param>
        /// <param name="format">The format.</param>
        /// <param name="limit">The limit.</param>
        /// <returns></returns>
        public static String makeUniqueName(this String candidate, IsNameTaken test, String format = "D3", Int32 limit = 1000)
        {
            Int32 id = 0;
            String output = candidate;

            while (test(output))
            {
                id++;
                if (id > 0)
                {
                    output = candidate + id.ToString(format);
                }
                if (id > limit) return output;
            }

            return output;
        }

        /// <summary>
        /// Pravi unikatno ime na osnovu kandidata i prosledjene liste postojecih imena
        /// </summary>
        /// <param name="candidate">String koji je trenutno kandidat za ime</param>
        /// <param name="used">Lista imena koja su koriscena</param>
        /// <param name="format">String format za formatiranje broja</param>
        /// <param name="limit">Na koliko pokusaja je limitiran proces</param>
        /// <param name="disableAutoAddToIList">if set to <c>true</c> it wil not automatically add the result to <c>used</c>.</param>
        /// <remarks>
        /// <para>Finds proper unique adding using <c>candidate</c> count sufix in provided <c>format</c></para>
        /// <para>If the <c>used</c> is <see cref="IList{T}"/> and <c>disableAutoAddToIList</c> is <c>false</c> it will <see cref="List{T}.Add(T)"/> the result. </para>
        /// </remarks>
        /// <returns>
        /// Unikatno ime koje se sastoji od kandidata i dodatog broja na kraju
        /// </returns>
        public static String makeUniqueName(this string candidate, IEnumerable<String> used, String format = "D3",
                                            Int32 limit = 1000, Boolean disableAutoAddToIList = true)
        {
            Int32 id = 0;
            String output = candidate;

            while (used.Contains(output))
            {
                id++;
                if (id > 0)
                {
                    output = candidate + id.ToString(format);
                }
                if (id > limit) return output;
            }

            if (used is IList<String>)
            {
                IList<String> usedList = (IList<String>)used;
                usedList.Add(candidate);
            }

            return output;
        }
    }
}