// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConcurrentList.cs" company="imbVeles" >
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
namespace imbSCI.Data.collection.math
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Concurrent List by Brian Murphy-Booth @ stackoverflow
    /// </summary>
    /// <remarks>
    /// <para>Original source for this class: https://stackoverflow.com/a/23446622/4034192 </para>
    /// <para>Author: Brian Murphy-Booth, https://stackoverflow.com/users/3023288/brian-murphy-booth </para>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.IList{T}" />
    /// <seealso cref="System.IDisposable" />
    public class ConcurrentList<T> : IList<T>, IDisposable
    {
        #region Fields

        private readonly List<T> _list;
        private readonly ReaderWriterLockSlim _lock;

        #endregion Fields

        #region Constructors

        public ConcurrentList()
        {
            this._lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            this._list = new List<T>();
        }

        public ConcurrentList(int capacity)
        {
            this._lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            this._list = new List<T>(capacity);
        }

        public ConcurrentList(IEnumerable<T> items)
        {
            this._lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            this._list = new List<T>(items);
        }

        #endregion Constructors

        #region Methods

        public void Add(T item)
        {
            try
            {
                this._lock.EnterWriteLock();
                this._list.Add(item);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        public void Insert(int index, T item)
        {
            try
            {
                this._lock.EnterWriteLock();
                this._list.Insert(index, item);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        public bool Remove(T item)
        {
            try
            {
                this._lock.EnterWriteLock();
                return this._list.Remove(item);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        public void RemoveAt(int index)
        {
            try
            {
                this._lock.EnterWriteLock();
                this._list.RemoveAt(index);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        public int IndexOf(T item)
        {
            try
            {
                this._lock.EnterReadLock();
                return this._list.IndexOf(item);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        public void Clear()
        {
            try
            {
                this._lock.EnterWriteLock();
                this._list.Clear();
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        public bool Contains(T item)
        {
            try
            {
                this._lock.EnterReadLock();
                return this._list.Contains(item);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            try
            {
                this._lock.EnterReadLock();
                this._list.CopyTo(array, arrayIndex);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ConcurrentEnumerator<T>(this._list, this._lock);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ConcurrentEnumerator<T>(this._list, this._lock);
        }

        ~ConcurrentList()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
                GC.SuppressFinalize(this);

            this._lock.Dispose();
        }

        #endregion Methods

        #region Properties

        public T this[int index]
        {
            get
            {
                try
                {
                    this._lock.EnterReadLock();
                    return this._list[index];
                }
                finally
                {
                    this._lock.ExitReadLock();
                }
            }
            set
            {
                try
                {
                    this._lock.EnterWriteLock();
                    this._list[index] = value;
                }
                finally
                {
                    this._lock.ExitWriteLock();
                }
            }
        }

        public int Count
        {
            get
            {
                try
                {
                    this._lock.EnterReadLock();
                    return this._list.Count;
                }
                finally
                {
                    this._lock.ExitReadLock();
                }
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion Properties
    }
}