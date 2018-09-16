// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ncLineSelector.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.nc.line
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlInclude(typeof(ncLineRelativeCriteria))]
    [XmlInclude(typeof(ncLineCriteria))]

    /// <summary>
    /// Sastoji se od kolekcije relativnih kriterijuma i mainLine kriterijuma
    /// </summary>
    ///
    public class ncLineSelector : ncLineCriteria, IList<ncLineRelativeCriteria>
    {
        public Int32 criteriaCount()
        {
            return 1 + relativeCriterias.Count;
        }

        #region --- relativeCriterias ------- kolekcija relativnih kriterijuma

        private ncLineRelativeCriteriaCollection _relativeCriterias = new ncLineRelativeCriteriaCollection();

        /// <summary>
        /// kolekcija relativnih kriterijuma
        /// </summary>
        public ncLineRelativeCriteriaCollection relativeCriterias
        {
            get
            {
                return _relativeCriterias;
            }
            set
            {
                _relativeCriterias = value;
                OnPropertyChanged("relativeCriterias");
            }
        }

        #endregion --- relativeCriterias ------- kolekcija relativnih kriterijuma

        #region -----------  mainCriteria  -------  [Criteria to select the current line - main]

        //private ncLineCriteria _mainCriteria = new ncLineCriteria();

        public ncLineSelector()
        {
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        public int IndexOf(ncLineRelativeCriteria item)
        {
            return relativeCriterias.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void Insert(int index, ncLineRelativeCriteria item)
        {
            relativeCriterias.Insert(index, item);
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void RemoveAt(int index)
        {
            relativeCriterias.RemoveAt(index);
        }

        /// <summary>
        /// Pristup relativnim kriterijumima
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ncLineRelativeCriteria this[Int32 key]
        {
            get
            {
                return relativeCriterias[key];
            }
            set
            {
                relativeCriterias[key] = value;
            }
        }

        /// <summary>
        /// Dodaje relativne kriterijume u skladu sa brojem
        /// </summary>
        /// <param name="nRelative"></param>
        public ncLineSelector(Int32 nRelative)
        {
            for (Int32 a = 0; a < nRelative; a++)
            {
                ncLineRelativeCriteria nlRC = new ncLineRelativeCriteria();
                if (a == 0)
                {
                    nlRC.commandCriteria = ncLineCommandPredefined.RIPOSIZIONA;
                    nlRC.relativePosition = 1;
                    nlRC.relativeType = ncLineRelativeCriteriaType.onExactPosition;
                    relativeCriterias.Add(nlRC);
                }
                else
                {
                    nlRC.commandCriteria = ncLineCommandPredefined.any;
                    nlRC.relativePosition = 0;
                    nlRC.relativeType = ncLineRelativeCriteriaType.disabled;
                    relativeCriterias.Add(nlRC);
                }
            }
        }

        /// <summary>
        /// pristup glavnom kriterijumu kao ncLineCriteria
        /// </summary>
        [XmlIgnore]
        public ncLineCriteria mainCriteria
        {
            get { return this as ncLineCriteria; }
        }

        #endregion -----------  mainCriteria  -------  [Criteria to select the current line - main]

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        IEnumerator<ncLineRelativeCriteria> IEnumerable<ncLineRelativeCriteria>.GetEnumerator()
        {
            return relativeCriterias.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public IEnumerator GetEnumerator()
        {
            return relativeCriterias.GetEnumerator();
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(ncLineRelativeCriteria item)
        {
            if (relativeCriterias == null) relativeCriterias = new ncLineRelativeCriteriaCollection();
            relativeCriterias.Add(item);
        }

        public void Add(Object item)
        {
            if (relativeCriterias == null) relativeCriterias = new ncLineRelativeCriteriaCollection();
            relativeCriterias.Add(item as ncLineRelativeCriteria);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        public void Clear()
        {
            relativeCriterias.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(ncLineRelativeCriteria item)
        {
            return relativeCriterias.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <paramref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
        public void CopyTo(ncLineRelativeCriteria[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        bool ICollection<ncLineRelativeCriteria>.Remove(ncLineRelativeCriteria item)
        {
            return relativeCriterias.Remove(item);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get { return relativeCriterias.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get { return false; }
        }
    }
}