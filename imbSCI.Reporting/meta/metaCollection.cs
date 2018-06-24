// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaCollection.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.meta
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.interfaces;
    using imbSCI.Data;
    using imbSCI.Reporting.exceptions;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Base class for all collections inside META structure
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// \ingroup_disabled docElementCore
    public class metaCollection<T> : ImetaCollection<T> where T : class, IMetaContentNested
    {
        /// <summary>
        /// The items
        /// </summary>
        private List<T> _items = new List<T>();

        /// <summary>
        ///
        /// </summary>
        protected List<T> items
        {
            get { return _items; }
            set { _items = value; }
        }

        /// <summary>
        /// Gets the <see cref="IMetaContentNested"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="IMetaContentNested"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T this[string key]
        {
            get
            {
                foreach (IMetaContentNested item in items)
                {
                    if (item.name == key) return (T)item;
                }

                //this.First(x => x.name == key);

                return null;// output;
            }
        }

        /// <summary>
        /// The name search iteration limit
        /// </summary>
        internal static int nameSearchIterationLimit = 5000;

        public int Count
        {
            get
            {
                return items.Count();
            }
        }

        /// <summary>
        /// Creates unique name for member
        /// </summary>
        /// <param name="nameProposal"></param>
        /// <returns></returns>
        internal string makeItemUniqueName(string nameProposal)
        {
            nameProposal = nameProposal.imbCodeNameOperation();// nameProposal.ToLower().Replace(" ", "_").Trim();
            string original = nameProposal;

            // Int32 hash = nameProposal.GetHashCode();
            int l = Count;
            do
            {
                l++;
                if (l > (nameSearchIterationLimit + Count))
                {
                    throw new aceReportException("Name search limit" + nameSearchIterationLimit.ToString() + " exceeded");
                    break;
                }
                nameProposal = original.add(l.ToString("D5"), "_");
            } while (this.Any(x => x.name == nameProposal));

            // if (reportingCoreManager.doVerboseLog) aceLog.log("--- entity [" + original + "] got new name [" + nameProposal + "] at parent [" + parent.name + ":" + parent.GetType() + "]");

            return nameProposal;
        }

        /// <summary>
        /// Deploy
        /// </summary>
        public void Sort()
        {
            if (Count == 0) return;
            items.Sort((x, y) => x.priority.CompareTo(y.priority));
        }

        /// <summary>
        /// Comparing priority of items
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        internal int compare(T a, T b)
        {
            if ((a == null) && (b == null)) return 0;
            if (a == null) return -1;
            if (b == null) return 1;

            if (a.priority > b.priority)
            {
                return 1;
            }
            else if (b.priority > a.priority)
            {
                return -1;
            }

            return 0;
        }

        //public new void Add(T item)
        //{
        //    throw new aceReportException("Unacceptable Add(T item) call to " + this.GetType().Name + ":metaCollection");
        //}

        /// <summary>Reference to common parent</summary>
        protected IMetaContentNested parent { get; set; }

        int IImetaCollection.Count
        {
            get
            {
                return items.Count;
            }
        }

        IMetaContentNested IImetaCollection.this[int key]
        {
            get
            {
                return this[key];
            }
        }

        IMetaContentNested IImetaCollection.this[string key]
        {
            get
            {
                return this[key];
            }
        }

        T ImetaCollection<T>.this[int key]
        {
            get
            {
                return this[key];
            }
        }

        T ImetaCollection<T>.this[string key]
        {
            get
            {
                return this[key];
            }
        }

        /// <summary>
        /// Discovers the common parent or sets the one provided in arguments
        /// </summary>
        /// <param name="__parent">The parent.</param>
        /// <returns></returns>
        /// <exception cref="aceReportException">Can't discover the parent when the collection is empty!! - null - discoverCommonParent exception</exception>
        public IMetaContentNested discoverCommonParent(IMetaContentNested __parent = null)
        {
            if (__parent != null)
            {
                parent = __parent;
                return parent;
            }
            if (parent != null) return parent;

            if (Count > 0)
            {
                foreach (IMetaContentNested child in this)
                {
                    if (child.parent != null)
                    {
                        parent = child.parent;
                        return parent;
                    }
                }
            }
            if ((Count == 0) && (parent == null))
            {
                throw new aceReportException("Can't discover the parent when the collection is empty!! " + "discoverCommonParent exception");
            }
            return null;
        }

        /// <summary>
        /// Method to register new page in collection - you must get new instance from parent object
        /// </summary>
        /// <param name="newChild"></param>
        /// <returns></returns>
        public int Add(IMetaContentNested newChild, IMetaContentNested __parent = null)
        {
            if (newChild == null)
            {
                // aceLog.loger.AppendLine("AddChild([null]) at [" + GetType().Name + "]");
                throw new aceReportException("AddChild got [null] object " + "metaCollection got null");
                return -1;
            }

            if (__parent == null)
            {
            }
            else
            {
                discoverCommonParent(__parent);
            }

            if ((__parent == null) && (parent == null))
            {
                discoverCommonParent(__parent); // <- this will throw exception if it was impossible to find out the parent
            }

            if (__parent != null)
            {
                newChild.parent = __parent;
            }

            if (parent != null)
            {
                newChild.parent = parent;
            }

            newChild.parent = parent;

            string nameProposal = newChild.name;
            int ci = 0;
            while (this.Any(x => x.name == nameProposal))
            {
                ci++;
                nameProposal = makeItemUniqueName(newChild.name);
                if (ci > 50)
                {
                    break;
                }
            }

            newChild.name = nameProposal;
            // newChild.id = items.Count;
            // Add(newPage);
            items.Add((T)newChild);

            //newChild.priority = Count;
            return items.Count;
        }

        public int IndexOf(IMetaContentNested child)
        {
            if (Count == 0) return -1;
            if (child == null)
            {
                throw new aceReportException("IndexOf([null]) is invalid call - btw. this collection is for: " + typeof(T).Name + " Wrong child type: " + GetType().Name);
            }
            if (child is T)
            {
                return items.IndexOf((T)child);
            }
            else
            {
                throw new aceReportException("Can't work with a child of type [" + child.GetType().Name + "] - this collection is for: " + typeof(T).Name + "Wrong child type in: " + GetType().Name);
            }
        }

        /// <summary>
        /// Set parent
        /// </summary>
        /// <param name="__parent">The parent.</param>
        public void setParentToItems(IMetaContentNested __parent)
        {
            //  throw new aceReportException("This method is deprecated", null, this, "Don-t use this");

            foreach (IMetaContentNested item in this)
            {
                if (item != null)
                {
                    if (item.parent == null)
                    {
                        throw new aceReportException("This item had no parent[" + item.name + "]:[" + item.GetType().Name + "]" + "Item have no parent");
                    }
                    else
                    {
                        item.parent = __parent;
                    }
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<T>)items).GetEnumerator();
        }

        IEnumerator<IMetaContentNested> IEnumerable<IMetaContentNested>.GetEnumerator()
        {
            return ((IEnumerable<IMetaContentNested>)items).GetEnumerator();
        }

        IEnumerator<T> ImetaCollection<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)items).GetEnumerator();
        }

        public bool Any()
        {
            return (Count > 0);
        }

        public T this[int key]
        {
            get
            {
                if (items.Count > key) return items[key];
                throw new aceReportException("This collection has no item under [" + key + "]" + " Out of range in: " + GetType().Name);
            }
        }
    }
}