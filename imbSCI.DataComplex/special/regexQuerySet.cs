// --------------------------------------------------------------------------------------------------------------------
// <copyright file="regexQuerySet.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataComplex.special
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Resolver - returns aggregate results
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <seealso cref="aceCommonTypes.collection.special.translationTableMulti{System.Text.RegularExpressions.Regex, TValue}" />
    public class regexQuerySet<TValue> : translationTableMulti<Regex, TValue>
    {
        /// <summary>
        ///
        /// </summary>
        public int minQL { get; set; }

        //private Int32 _maxQL;
        ///// <summary>
        /////
        ///// </summary>
        //public Int32 maxQL
        //{
        //    get { return _maxQL; }
        //    set { _maxQL = value; }
        //}

        public instanceCountCollection<TValue> resolveQuerySet(IEnumerable<string> inputs)
        {
            var queries = GetKeys();
            minQL = queries.Min(x => x.ToString().Length);
            //maxQL = queries.Max(x => x.ToString().Length);

            instanceCountCollection<TValue> output = new instanceCountCollection<TValue>();

            foreach (string input in inputs)
            {
                foreach (Regex rg in GetKeys())
                {
                    if (rg.IsMatch(input))
                    {
                        int points = (rg.ToString().Length - minQL) * 2;
                        GetByKey(rg).ForEach(x => output.AddInstance(x, points));
                    }
                }
            }
            return output;
        }

        public instanceCountCollection<TValue> resolveQuery(string input)
        {
            var queries = GetKeys();
            minQL = queries.Min(x => x.ToString().Length);

            instanceCountCollection<TValue> output = new instanceCountCollection<TValue>();
            foreach (Regex rg in GetKeys())
            {
                if (rg.IsMatch(input))
                {
                    int points = 1 + ((rg.ToString().Length - minQL) * 2);
                    GetByKey(rg).ForEach(x => output.AddInstance(x, points));
                }
            }
            return output;
        }

        public void Add(string regexPattern, params TValue[] results)
        {
            Regex rg = new Regex(regexPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (TValue tv in results) Add(rg, tv);
        }

        public void AddRange(string regexPattern, IEnumerable<TValue> results)
        {
            Regex rg = new Regex(regexPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (TValue tv in results) Add(rg, tv);
        }
    }
}