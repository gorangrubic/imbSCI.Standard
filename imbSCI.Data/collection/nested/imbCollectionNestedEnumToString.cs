// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbCollectionNestedEnumToString.cs" company="imbVeles" >
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
// Project: imbSCI.Data
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Data.collection.nested
{
    #region imbVeles using

    using System;
    using System.Collections;
    using System.Collections.Generic;

    #endregion imbVeles using

    /// <summary>
    /// The <see cref="imbCollectionNestedEnum{TC, TI, TIV}"/> applied for String values. It is thread-safe.
    /// </summary>
    /// <typeparam name="TC">Enum type defining the first or X axis (aka: categories)</typeparam>
    /// <typeparam name="TI">Enum type defining the second or Y axis (aka: options)</typeparam>
    /// <remarks>
    /// <para>To access the value use indexers, i.e.: collection[X][Y] = "something"</para>
    /// <para>This application of the <see cref="imbCollectionNestedEnum{TC, TI, TIV}"/> allows you to set initial string value for each cell in the 2D matrix</para>
    /// </remarks>
    public class imbCollectionNestedEnumToString<TC, TI> : imbCollectionNestedEnum<TC, TI, string>
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="defaultToString">Da li da upiše ime enuma kao podrazumevanu vrednost polja ili da ostane prazan string</param>
        public imbCollectionNestedEnumToString(Boolean defaultToString = false)
        {
            deploy(defaultToString);
        }

        private void deploy(Boolean defaultToString = false)
        {
            try
            {
                IList itts = Enum.GetValues(typeof(TI)) as IList;
                foreach (TI ti in itts) itemKeys.Add(ti);

                IList cats = Enum.GetValues(typeof(TC)) as IList;
                foreach (TC tc in cats) categories.Add(tc);

                foreach (TC md in cats)
                {
                    if (!ContainsKey(md))
                    {
                        Add(md, new Dictionary<TI, String>());
                    }
                    else
                    {
                    }
                    foreach (TI qw in itts)
                    {
                        if (!this[md].ContainsKey(qw))
                        {
                            if (defaultToString)
                            {
                                this[md].Add(qw, qw.ToString());
                            }
                            else
                            {
                                this[md].Add(qw, "");
                            }
                        }
                        else
                        {
                            if (defaultToString)
                            {
                                this[md][qw] = qw.ToString();
                            }
                            else
                            {
                                this[md][qw] = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}