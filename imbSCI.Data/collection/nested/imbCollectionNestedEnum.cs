// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbCollectionNestedEnum.cs" company="imbVeles" >
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
    using System;
    using System.Collections;
    using System.Collections.Generic;

#pragma warning disable CS1570 // XML comment has badly formed XML -- 'End tag 'para' does not match the start tag 'see'.'
#pragma warning disable CS1570 // XML comment has badly formed XML -- 'End tag 'para' does not match the start tag 'paga'.'
#pragma warning disable CS1570 // XML comment has badly formed XML -- 'Expected an end tag for element 'remarks'.'
#pragma warning disable CS1570 // XML comment has badly formed XML -- 'End tag 'remarks' does not match the start tag 'para'.'
    /// <summary>
    /// Two dimensional collection where categories (X or the first axis) and options (Y or the second axis) are <see cref="Enum"/> types.
    /// </summary>
    /// <remarks>
    /// <para>The collection automatically creates "room" for all members defined at the generic <see cref="Enum">s</para>
    /// <paga>While based on the <see cref="Dictionary{TKey, TValue}"/>, this collection is thread-safe because complete 2D structure is built in advance</para>
    /// </remarks>
    /// <typeparam name="TC">Enum type that enumerates categories i.e. the first axis</typeparam>
    /// <typeparam name="TI">Enum type that enumerates particular options</typeparam>
    /// <typeparam name="TIV">The class of value to be stored in this collection</typeparam>
    public class imbCollectionNestedEnum<TC, TI, TIV> : Dictionary<TC, Dictionary<TI, TIV>>
#pragma warning restore CS1570 // XML comment has badly formed XML -- 'End tag 'remarks' does not match the start tag 'para'.'
#pragma warning restore CS1570 // XML comment has badly formed XML -- 'Expected an end tag for element 'remarks'.'
#pragma warning restore CS1570 // XML comment has badly formed XML -- 'End tag 'para' does not match the start tag 'paga'.'
#pragma warning restore CS1570 // XML comment has badly formed XML -- 'End tag 'para' does not match the start tag 'see'.'
    {
        public imbCollectionNestedEnum()
        {
            TIV defaultValue = default(TIV); // this.GetDefaultValue<TIV>();
            deploy(defaultValue);
        }

        /// <summary>
        /// Konstruktor koji postavlja podrazumevanu vrednost na svako polje
        /// </summary>
        /// <param name="defaultValue"></param>
        public imbCollectionNestedEnum(TIV defaultValue)
        {
            deploy(defaultValue);
        }

        /// <summary>
        /// Defines the specified value.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <param name="category">The category.</param>
        /// <param name="forItem">For item.</param>
        public void define(TIV val, TC category, params TI[] forItem)
        {
            foreach (TI ti in forItem)
            {
                this[category][ti] = val;
            }
        }

        /// <summary>
        /// Sets fields of all categories  <c>val</c>
        /// </summary>
        /// <param name="val">The value.</param>
        /// <param name="forItem">For item.</param>
        public void define(TIV val, params TI[] forItem)
        {
            foreach (TC ct in categories)
            {
                foreach (TI ti in forItem)
                {
                    this[ct][ti] = val;
                }
            }
        }

        protected void deploy(TIV defaultValue)
        {
            if (typeof(TI) == typeof(System.Enum))
            {
                return;
            }

            IList cats = Enum.GetValues(typeof(TC)) as IList;
            foreach (TC c in cats) categories.Add(c);

            if (!typeof(TI).IsEnum)
            {
                foreach (TC md in cats)
                {
                    Add(md, new Dictionary<TI, TIV>());
                }
                return;
            }

            IList itts = Enum.GetValues(typeof(TI)) as IList;
            foreach (TI it in cats) itemKeys.Add(it);

            foreach (TC md in cats)
            {
                Add(md, new Dictionary<TI, TIV>());
                foreach (TI qw in itts)
                {
                    this[md].Add(qw, defaultValue);
                }
            }
        }

        #region --- categories ------- allCateogriesValues

        private List<TC> _categories = new List<TC>();

        /// <summary>
        /// allCateogriesValues
        /// </summary>
        protected List<TC> categories
        {
            get { return _categories; }
            set { _categories = value; }
        }

        #endregion --- categories ------- allCateogriesValues

        #region --- itemKeys ------- za sve iteme

        private List<TI> _itemKeys = new List<TI>();

        /// <summary>
        /// za sve iteme
        /// </summary>
        protected List<TI> itemKeys
        {
            get { return _itemKeys; }
            set { _itemKeys = value; }
        }

        #endregion --- itemKeys ------- za sve iteme
    }
}