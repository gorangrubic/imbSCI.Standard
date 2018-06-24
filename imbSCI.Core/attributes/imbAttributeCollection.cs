// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbAttributeCollection.cs" company="imbVeles" >
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
namespace imbSCI.Core.attributes
{
    using imbSCI.Core.extensions.typeworks;

    #region imbVeles using

    using System;
    using System.Collections.Generic;
    using System.Data;

    #endregion imbVeles using

    public class imbAttributeCollection : PropertyCollection //Dictionary<imbAttributeName, imbAttribute>
    {
        public imbAttribute this[Object key]
        {
            get
            {
                imbAttribute att = (imbAttribute)base[key];
                return att;
            }
            set
            {
                base[key] = value;
            }
        }

        /// <summary>
        /// dodaje novi attribut u kolekciju
        /// </summary>
        /// <param name="newAttribute"></param>
        public void AddNewAttribute(imbAttribute newAttribute)
        {
            if (ContainsKey(newAttribute.nameEnum))
            {
                this[newAttribute.nameEnum] = newAttribute;
            }
            else
            {
                Add(newAttribute.nameEnum, newAttribute);
            }
        }

        public imbAttribute getAttribute(imbAttributeName name)
        {
            if (ContainsKey(name))
            {
                return (imbAttribute)this[name];
            }
            else
            {
                return null;
            }
        }

        public void setMessage<T>(imbAttributeName name, T defValue, out T target)
        {
            if (ContainsKey(name))
            {
                target = this[name].imbConvertValueSafeTyped<T>();
            }
            else
            {
                target = defValue;
            }
        }

        public Object getMessageObj(imbAttributeName name, Object output = null)
        {
            if (ContainsKey(name))
            {
                return this[name].getMessage();
            }
            else
            {
                return output;
            }
        }

        /// <summary>
        /// Bezbedno preuzimanje poruke
        /// </summary>
        /// <param name="name"></param>
        /// <param name="output">Podrazumevani output</param>
        /// <returns></returns>
        public T getMessage<T>(imbAttributeName name, T output = default(T))
        {
            if (ContainsKey(name))
            {
                return this[name].imbConvertValueSafeTyped<T>();
            }
            else
            {
                return output;
            }
        }

        /// <summary>
        /// Bezbedno preuzimanje poruke
        /// </summary>
        /// <param name="name"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public String getMessage(imbAttributeName name, String output = "")
        {
            if (ContainsKey(name))
            {
                return this[name].getMessage().ToString();
            }
            else
            {
                return output;
            }
        }

        #region --- selfDeclaredAttributes ------- Attributi koje je samostalno deklarisao tip

        private Dictionary<imbAttributeName, imbAttribute> _selfDeclaredAttributes =
            new Dictionary<imbAttributeName, imbAttribute>();

        /// <summary>
        /// Attributi koje je samostalno deklarisao tip
        /// </summary>
        public Dictionary<imbAttributeName, imbAttribute> selfDeclaredAttributes
        {
            get { return _selfDeclaredAttributes; }
            set { _selfDeclaredAttributes = value; }
        }

        #endregion --- selfDeclaredAttributes ------- Attributi koje je samostalno deklarisao tip
    }
}