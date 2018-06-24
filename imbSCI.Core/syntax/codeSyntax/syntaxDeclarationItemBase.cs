// --------------------------------------------------------------------------------------------------------------------
// <copyright file="syntaxDeclarationItemBase.cs" company="imbVeles" >
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
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// osnovna klasa - zajednicka svim syntaxDeclaration elementima
    /// </summary>
    public abstract class syntaxDeclarationItemBase : INotifyPropertyChanged, ISyntaxDeclarationElement
    {
        #region --- name ------- ime propertija

        private String _name;

        /// <summary>
        /// ime sintaksnog elementa
        /// </summary>
        public String name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                //  OnPropertyChanged("name");
            }
        }

        #endregion --- name ------- ime propertija

        /// <summary>
        /// Tests if this element matches query. Testing: 1. name, 2. alias list, and then if alias list contains willcard *
        /// </summary>
        /// <param name="nameOrAlias"></param>
        /// <returns></returns>
        public Boolean isNameOrAlias(String nameOrAlias)
        {
            if (name == nameOrAlias)
            {
                return true;
            }

            if (aliasList.Contains(nameOrAlias))
            {
                return true;
            }

            if (aliasList.Contains("*"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// set alias using one string definition. i.e.> name1|alias1|alias2|alias3|*
        /// </summary>
        /// <remarks>
        /// Using * will create willcard> this element will become default class for all non mached elements
        /// </remarks>
        /// <param name="__alias"></param>
        public void setAlias(String __alias)
        {
            setAlias(new String[] { __alias });
        }

        /// <summary>
        /// Sets name and aliases for the element
        /// </summary>
        /// <param name="__alias"></param>
        public void setAlias(IEnumerable<String> __alias)
        {
            List<String> names = new List<string>();
            foreach (String al in __alias)
            {
                if (al.Contains("|"))
                {
                    names.AddRange(al.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                }
                else
                {
                    names.Add(al);
                }
            }

            name = names.First();

            foreach (String al in names)
            {
                if (al != name)
                {
                    aliasList.Add(al);
                }
            }
        }

        #region --- aliasList ------- list of alias names can be used

        private List<String> _aliasList = new List<string>();

        /// <summary>
        /// List of alias names can be used. (does not contain name). Don's use this for alias check! This property is public only because of XML serialization.
        /// </summary>
        public List<String> aliasList
        {
            get
            {
                return _aliasList;
            }
            set
            {
                _aliasList = value;
                OnPropertyChanged("aliasList");
            }
        }

        #endregion --- aliasList ------- list of alias names can be used

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}