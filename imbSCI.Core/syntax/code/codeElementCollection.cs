// --------------------------------------------------------------------------------------------------------------------
// <copyright file="codeElementCollection.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.text;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Kolekcija elemenata koda - može da sadrži propertije ili pod blokove
    /// </summary>
    public class codeElementCollection : INotifyPropertyChanged
    {
        #region ----- CONSTRUCTOR AND SETTINGS -----------------------

        public codeElementCollection(ICodeElement __parent, params Type[] itemTypes)
        {
            _parent = __parent;
            if (itemTypes != null)
            {
                foreach (Type t in itemTypes)
                {
                    allowedElements.Add(t);
                }
            }
            else
            {
            }
        }

        protected List<Type> allowedElements = new List<Type>();

        private ICodeElement _parent; // = "";

        /// <summary>
        /// Ref to object containing this collection - parent
        /// </summary>
        public ICodeElement parent
        {
            get { return _parent; }
        }

        #endregion ----- CONSTRUCTOR AND SETTINGS -----------------------

        private List<ICodeElement> _items = new List<ICodeElement>(); // = "";

        /// <summary>
        /// Bindable property
        /// </summary>
        public List<ICodeElement> items
        {
            get { return _items; }
        }

        private ICodeElement _current; // = "";

        /// <summary>
        /// Pointer to the current element
        /// </summary>
        public ICodeElement current
        {
            get
            {
                //if (!isReady) return null;
                if (isEmpty) return null;
                if (_current == null)
                {
                    _current = items.First();
                }
                return _current;
            }
        }

        #region INTERNAL OPERATIONS ------

        protected Boolean _isAllowed(ICodeElement item)
        {
            //Boolean output = false;
            foreach (Type ta in allowedElements)
            {
                if (item.GetType() == ta)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds element in the collectiona and sets it as current element
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected Int32 _register(ICodeElement item)
        {
            Int32 i = -1;

            if (!items.Contains(item))
            {
                items.Add(item);
            }
            else
            {
            }

            codeElementBase itemBase = item as codeElementBase;
            itemBase._parent = parent;

            i = items.IndexOf(item);

            _current = item;

            return i;
        }

        #endregion INTERNAL OPERATIONS ------

        #region OPERATIONS ---------------------------------------------------

        /// <summary>
        /// Index of provided element
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Int32 IndexOf(ICodeElement item)
        {
            if (!isReady) return -1;

            if (!items.Contains(item))
            {
                return -1;
            }
            return items.IndexOf(item);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Int32 Add(ICodeElement item)
        {
            Int32 i = -1;

            if (_isAllowed(item))
            {
                i = _register(item);
            }

            return i;
        }

        public T getElement<T>(String nameOrId) where T : ICodeElement
        {
            return (T)__getElement(nameOrId);
        }

        private ICodeElement __getElement(String nameOrId)
        {
            if (isEmpty) return null;

            if (String.IsNullOrEmpty(nameOrId))
            {
                return null;
            }
            nameOrId = nameOrId.Trim();
            nameOrId = nameOrId.ToUpper();

            ICodeElement output = null;

            if (nameOrId.isNumber())
            {
                output = items[Int32.Parse(nameOrId)];
            }
            else
            {
                output = items.Find(x => x.name.ToUpper() == nameOrId);
            }

            return output;
        }

        public ICodeElement getElement(String name)
        {
            return __getElement(name);
        }

        /// <summary>
        /// Pretrazuje i Alias listu
        /// </summary>
        /// <param name="nameOrAlias"></param>
        /// <returns></returns>
        public ICodeElement findElement(String nameOrAlias)
        {
            ICodeElement output = getElement(nameOrAlias);

            if (output == null)
            {
                foreach (ICodeElement el in items)
                {
                    // el.declarationBase.
                    if (el.declarationBase.isNameOrAlias(nameOrAlias))
                    {
                        return el;
                    }
                }
            }

            return output;
        }

        public T getElement<T>(Int32 i) where T : ICodeElement
        {
            return (T)getElement(i);
        }

        public ICodeElement getElement(Int32 i)
        {
            if (isEmpty) return null;
            return items[i];
        }

        public Boolean hasElement(String name)
        {
            if (isEmpty) return false;
            return getElement(name) != null;
        }

        #endregion OPERATIONS ---------------------------------------------------

        #region ------------- IS OUTPUTS -------------------

        /// <summary>
        /// Returns if Ready: items collection is declared, parent object defined
        /// </summary>
        public Boolean isReady
        {
            get
            {
                //Boolean output = true;
                if (items == null) return false;
                if (parent == null) return false;
                if (allowedElements.Count == 0) return false;
                return true;
            }
        }

        /// <summary>
        /// Returns if Empty : TRUE if not Ready or if collection containes only one child
        /// </summary>
        public Boolean isEmpty
        {
            get
            {
                if (!isReady) return true;

                //Boolean output = false;
                // if ( ) { output = true; }
                return (items.Count == 0);
            }
        }

        /// <summary>
        /// Returns if OneChild : TRUE if collection containes only one child, FALSE if not ready or counts more or 0
        /// </summary>
        public Boolean isOneChild
        {
            get
            {
                if (!isReady) return false;
                //Boolean output = false;
                // if ( ) { output = true; }
                return (items.Count == 1);
            }
        }

        #endregion ------------- IS OUTPUTS -------------------

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        //List<codeElementBase>
    }
}