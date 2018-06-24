// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAceCollection.cs" company="imbVeles" >
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
namespace imbSCI.Data.collection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;

    public interface IAceCollection : INotifyPropertyChanged, IList, ICollection
    {
        /*

        Boolean Any();

        event PropertyChangedEventHandler PropertyChanged;
        void Move(int oldIndex, int newIndex);
        event NotifyCollectionChangedEventHandler CollectionChanged;
        void Add(Object item);
        void Clear();
        void CopyTo(object[] array, int index);
        bool Contains(Object item);
        IEnumerator GetEnumerator();
        int IndexOf(Object item);
        void Insert(int index, Object item);
        bool Remove(Object item);
        void RemoveAt(int index);
        int Count { get; }
        Object this[int index] { get; set; }*/
    }

    public interface IAceCollection<T> : INotifyPropertyChanged, IList<T>, ICollection<T>
    {
        Boolean Any();

        /// <summary>
        /// Inicijalizacija koja se pokrece sama nakon instanciranja
        /// </summary>
        void _autoInit();

        event PropertyChangedEventHandler PropertyChanged;

        void Move(int oldIndex, int newIndex);

        event NotifyCollectionChangedEventHandler CollectionChanged;

        void Add(T item);

        void Clear();

        void CopyTo(T[] array, int index);

        bool Contains(T item);

        IEnumerator GetEnumerator();

        int IndexOf(T item);

        void Insert(int index, T item);

        bool Remove(T item);

        void RemoveAt(int index);

        int Count { get; }
        T this[int index] { get; set; }
    }
}