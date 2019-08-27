// --------------------------------------------------------------------------------------------------------------------
// <copyright file="layerCollection.cs" company="imbVeles" >
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
namespace imbSCI.Data.collection.layers
{
    using imbSCI.Data.interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

#pragma warning disable CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.IList{System.Object}'
#pragma warning disable CS1574 // XML comment has cref attribute 'IObjectWithName' that could not be resolved
    /// <summary>
    /// Instance of layer inside <see cref="layerStack"/>
    /// </summary>
    /// <seealso cref="aceCommonTypes.primitives.imbBindable" />
    /// <seealso cref="aceCommonTypes.interfaces.IObjectWithName" />
    /// <seealso cref="System.Collections.Generic.IList{System.Object}" />
    public class layerCollection : IObjectWithName, IList<object>
#pragma warning restore CS1574 // XML comment has cref attribute 'IObjectWithName' that could not be resolved
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.IList{System.Object}'
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="layerCollection"/> class.
        /// </summary>
        /// <param name="__name">The name.</param>
        /// <param name="__description">The description.</param>
        /// <param name="__parent">The parent.</param>
        /// <param name="__id">The identifier.</param>
        public layerCollection(String __name, String __description, layerStack __parent, Int32 __id)
        {
            name = __name; description = __description; parent = __parent; id = __id;
        }

        /// <summary>
        /// To the string.
        /// </summary>
        /// <returns></returns>
        public String ToString()
        {
            String output = "layer[" + name + "] = [" + items.Count() + "]";
            return output;
        }

        /// <summary>True when the layer is empty</summary>
        public Boolean isEmpty
        {
            get
            {
                return !items.Any();
            }
        }

        /// <summary>
        /// Pushes the specified items, and returns the items refused (already inside - or - by other criterion)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public List<T> Push<T>(IEnumerable<T> input) where T : class
        {
            List<T> output = new List<T>();

            foreach (T el in input)
            {
                if (items.Contains(el))
                {
                    output.Add(el);
                }
                else
                {
                    items.Add(el);
                }
            }

            return output;
        }

        /// <summary>
        /// If already contained returns false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="el">The el.</param>
        /// <returns></returns>
        public Boolean Push<T>(T el) where T : class
        {
            if (items.Contains(el))
            {
                return false;
            }
            else
            {
                items.Add(el);
                return true;
            }
        }

        /// <summary>
        /// Pulls the content (and removes it from the layer) specified pull limit.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pullLimit">The pull limit.</param>
        /// <returns></returns>
        public List<T> Pull<T>(Int32 pullLimit = -1)
        {
            List<T> output = new List<T>();
            if (pullLimit == -1) pullLimit = items.Count();
            if (pullLimit > items.Count()) pullLimit = items.Count();

            var pl = items.Where(x => x is T);
            foreach (T el in pl)
            {
                output.Add(el);
                if (output.Count() >= pullLimit)
                {
                    break;
                }
            }
            output.ForEach(x => items.Remove(x));

            return output;
        }

        /// <summary>
        /// Pulls all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> PullAll<T>()
        {
            List<T> output = new List<T>();
            var pl = items.Where(x => x is T);
            foreach (T el in pl)
            {
                output.Add(el);
            }
            items.Clear();

            return output;
        }

        private layerStack _parent;

        /// <summary>Reference to parent stack </summary>
        public layerStack parent
        {
            get
            {
                return _parent;
            }
            protected set
            {
                _parent = value;
            }
        }

        private Int32 _id = 0;

        /// <summary>ID of the layer, 0 is the surface</summary>
        public Int32 id
        {
            get
            {
                return _id;
            }
            protected set
            {
                _id = value;
            }
        }

        private List<Object> _items = new List<Object>();

        /// <summary> </summary>
        protected List<Object> items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                //OnPropertyChanged("items");
            }
        }

        private String _name = "";

        /// <summary>
        /// Name for this instance
        /// </summary>
        public String name
        {
            get { return _name; }
            set { _name = value; }
        }

        private String _description = "";

        /// <summary>
        /// Human-readable description of object instance
        /// </summary>
        public String description
        {
            get { return _description; }
            set { _description = value; }
        }

        public int Count
        {
            get
            {
                return ((IList<object>)items).Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IList<object>)items).IsReadOnly;
            }
        }

        public object this[int index]
        {
            get
            {
                return ((IList<object>)items)[index];
            }

            set
            {
                ((IList<object>)items)[index] = value;
            }
        }

        public int IndexOf(object item)
        {
            return ((IList<object>)items).IndexOf(item);
        }

        public void Insert(int index, object item)
        {
            ((IList<object>)items).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            ((IList<object>)items).RemoveAt(index);
        }

        public void Add(object item)
        {
            ((IList<object>)items).Add(item);
        }

        public void Clear()
        {
            ((IList<object>)items).Clear();
        }

        public bool Contains(object item)
        {
            return ((IList<object>)items).Contains(item);
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            ((IList<object>)items).CopyTo(array, arrayIndex);
        }

        public bool Remove(object item)
        {
            return ((IList<object>)items).Remove(item);
        }

        public IEnumerator<object> GetEnumerator()
        {
            return ((IList<object>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<object>)items).GetEnumerator();
        }
    }
}