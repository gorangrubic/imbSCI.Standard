// --------------------------------------------------------------------------------------------------------------------
// <copyright file="codeElementBase.cs" company="imbVeles" >
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
    using imbSCI.Core.syntax.codeSyntax;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Osnovna klasa za sve elemente code DOM-a date instance
    /// </summary>
    public abstract class codeElementBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Sets name, source and declarationBase --- sets only non-empty/null parameters
        /// </summary>
        /// <param name="__name"></param>
        /// <param name="__source"></param>
        /// <param name="__declarationBase"></param>
        internal void baseSetup(String __name, String __source, ISyntaxDeclarationElement __declarationBase)
        {
            if (!String.IsNullOrEmpty(__name)) _name = __name.Trim();
            if (!String.IsNullOrEmpty(__source)) _source = __source;
            if (__declarationBase != null) _declarationBase = __declarationBase;
        }

        protected ISyntaxDeclarationElement _declarationBase; // = "";

        /// <summary>
        /// Ref to syntax declaration base
        /// </summary>
        public ISyntaxDeclarationElement declarationBase
        {
            get { return _declarationBase; }
        }

        public Int32 level
        {
            get
            {
                if (parent == null) return 0;
                return parent.level + 1;
            }
        }

        internal ICodeElement _parent; // = "";

        /// <summary>
        /// Ref to code element having this element as parent
        /// </summary>
        public ICodeElement parent
        {
            get { return _parent; }
        }

        /// <summary>
        /// Index at parent collection --- returns -1 if no parent, or parent collection not ready
        /// </summary>
        public Int32 index
        {
            get
            {
                if (parent == null)
                {
                    return -1;
                }

                return parent.children.IndexOf(this as ICodeElement);
            }
        }

        //private ICodeElement _root; // = "";
        /// <summary>
        /// ref to root code element - the one without parent
        /// </summary>
        public ICodeElement root
        {
            get
            {
                if (parent != null)
                {
                    return parent.root as ICodeElement;
                }
                else
                {
                    return this as ICodeElement;
                }
            }
        }

        public ICodeElement this[Int32 __index]
        {
            get
            {
                return children.getElement(__index);
            }
        }

        public ICodeElement this[String __name]
        {
            get
            {
                return children.findElement(__name);
            }
        }

        public Object value
        {
            get
            {
                Type t = this.GetType();
                if (t == typeof(codeLineToken))
                {
                    return objectValue;
                }
                if (t == typeof(codeLine))
                {
                    codeLine cl = this as codeLine;

                    if (cl.children.hasElement("value"))
                    {
                        return cl["value"].value;
                    }
                    else
                    {
                        List<Object> output = new List<object>();
                        foreach (var ct in cl.children.items)
                        {
                            output.Add(ct.value);
                        }
                        return output;
                    }
                }
                if (t == typeof(codeBlock))
                {
                    Dictionary<String, Object> outDict = new Dictionary<string, object>();
                    foreach (var ct in children.items)
                    {
                        if (outDict.ContainsKey(ct.name))
                        {
                            outDict[ct.name] = ct.value;
                        }
                        else
                        {
                            outDict.Add(ct.name, ct.value);
                        }
                    }
                    return outDict;
                }
                return children.items;
            }
            set
            {
                Type t = this.GetType();
                if (t == typeof(codeLineToken))
                {
                    _objectValue = value;
                }
                if (t == typeof(codeLine))
                {
                    codeLine cl = this as codeLine;

                    if (cl.children.hasElement("value"))
                    {
                        var ctk = cl["value"] as codeLineToken;
                        ctk.objectValue = value;

                        //return cl["value"].value;
                    }
                    else
                    {
                        //List<Object> output = new List<object>();
                        //foreach (var ct in cl.children.items)
                        //{
                        //    output.Add(ct.value);
                        //}
                        //return output;
                    }
                }
                //if (t == typeof(codeBlock))
                //{
                //    Dictionary<String, Object> outDict = new Dictionary<string, object>();
                //    foreach (var ct in children.items)
                //    {
                //        if (outDict.ContainsKey(ct.name))
                //        {
                //            outDict[ct.name] = ct.value;
                //        }
                //        else
                //        {
                //            outDict.Add(ct.name, ct.value);
                //        }
                //    }
                //    return outDict;
                //}
                //return children.items;
            }
        }

        internal String _name; // = "";

        /// <summary>
        /// name or key of the code element (if has any)
        /// </summary>
        public String name
        {
            get { return _name; }
        }

        internal String _source; // = "";

        /// <summary>
        /// Source code loaded from the file - or - prepared for save
        /// </summary>
        public String source
        {
            get { return _source; }
        }

        #region --- objectValue ------- Value or object attached to this code element

        private Object _objectValue;

        /// <summary>
        /// Value or object attached to this code element
        /// </summary>
        protected Object objectValue
        {
            get
            {
                return _objectValue;
            }
            set
            {
                _objectValue = value;
                OnPropertyChanged("objectValue");
            }
        }

        #endregion --- objectValue ------- Value or object attached to this code element

        #region --- children ------- sub blocks and other code elements

        protected codeElementCollection _children;

        /// <summary>
        /// sub blocks and other code elements
        /// </summary>
        public codeElementCollection children
        {
            get
            {
                return _children;
            }
        }

        #endregion --- children ------- sub blocks and other code elements

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}