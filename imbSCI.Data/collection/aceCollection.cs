// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceCollection.cs" company="imbVeles" >
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
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    /// <summary>
    /// Tipizirana kolekcija
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class aceCollection<T> : ObservableCollection<T>, IAceCollection<T>
    {
        public Boolean Any()
        {
            return Count > 0;
        }

        protected System.Type itemType;

        protected aceCollection()
        {
            _autoInit();
        }

        /// <summary>
        /// Inicijalizacija koja se pokrece sama nakon instanciranja
        /// </summary>
        public virtual void _autoInit()
        {
            itemType = typeof(T);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public IEnumerator GetEnumerator()
        {
            return base.GetEnumerator();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}