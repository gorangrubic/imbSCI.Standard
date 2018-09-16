// --------------------------------------------------------------------------------------------------------------------
// <copyright file="syntaxDeclarationElementCollection.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.codeSyntax
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public abstract class syntaxDeclarationElementCollection<T> : List<T> where T : class, ISyntaxDeclarationElement
    {
        private ISyntaxDeclarationElement _currentBase; // = "";

        [XmlIgnore]
        /// <summary>
        /// currently selected element
        /// </summary>

        public ISyntaxDeclarationElement currentBase
        {
            get { return _currentBase; }
        }

        protected T _current
        {
            get
            {
                return _currentBase as T;
            }
            set
            {
                _currentBase = value;
            }
        }

        [XmlIgnore]
        public T current
        {
            get { return _current as T; }
        }

        private ISyntaxDeclarationElement this[String __nameOrAlias]
        {
            get
            {
                return find(__nameOrAlias);
            }
        }

        /// <summary>
        /// Find declaration by name or by alias
        /// </summary>
        /// <param name="nameOrAlias"></param>
        /// <returns></returns>
        public T find(String nameOrAlias)
        {
            foreach (T tmp in this)
            {
                if (tmp.name == nameOrAlias)
                {
                    return tmp;
                }
            }

            foreach (T tmp in this)
            {
                if (tmp.isNameOrAlias(nameOrAlias))
                {
                    return tmp;
                }
            }

            return null;
        }
    }
}