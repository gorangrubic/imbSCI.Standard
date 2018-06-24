// --------------------------------------------------------------------------------------------------------------------
// <copyright file="pathElementFormatCollection.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.path
{
    #region imbVeles using

    using imbSCI.Core.extensions.data;
    using imbSCI.Data.collection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    #endregion imbVeles using

    /// <summary>
    /// imbCollectionMeta namenska kolekcija za  pathElementFormat
    /// </summary>
    internal class pathElementFormatCollection : aceDictionaryCollection<pathElementFormat>
    {
        internal pathElementFormatCollection()
        // : base("prefix")
        {
        }

        internal void autosetup(Type type)
        {
            Clear();
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public |
                                        BindingFlags.GetField);

            var fields_prefix = fields.imbFirstOrMore(x => x.Name.StartsWith("prefix_"));

            var fields_sufix = fields.imbFirstOrMore(x => x.Name.StartsWith("sufix_"));

            foreach (FieldInfo fi in fields_prefix)
            {
                var tmp = new pathElementFormat(fi);
                FieldInfo sfi = fields_sufix.imbFirstSafe(x => x.Name == "sufix_" + tmp.cleanName);
                if (sfi != null) tmp.sufix = sfi.GetValue(null).ToString();
                Add(tmp.prefix, tmp);
            }
        }

        /// <summary>
        /// Vraca sve prefixe koji se pojavljuju u pathElementFormatiranju
        /// </summary>
        /// <returns></returns>
        internal char[] allPrefixes()
        {
            List<char> output = new List<char>();
            Keys.ToList().ForEach(x => output.AddRange(x.ToCharArray()));

            return output.ToArray();
        }

        internal void AddFormat(string prefix, string format = "")
        {
            pathElementFormat element = new pathElementFormat();
            element.prefix = prefix;
        }
    }
}