// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConcurrentEnumerator.cs" company="imbVeles" >
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Concurrent Enumerator by Brian Murphy-Booth @ stackoverflow
    /// </summary>
    /// <remarks>
    /// <para>Original source for this class: https://stackoverflow.com/a/23446622/4034192 </para>
    /// <para>Author: Brian Murphy-Booth, https://stackoverflow.com/users/3023288/brian-murphy-booth </para>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.IList{T}" />
    /// <seealso cref="System.IDisposable" />
    public class ConcurrentEnumerator<T> : IEnumerator<T>
    {
        #region Fields

        private readonly IEnumerator<T> _inner;
        private readonly ReaderWriterLockSlim _lock;

        #endregion Fields

        #region Constructor

        public ConcurrentEnumerator(IEnumerable<T> inner, ReaderWriterLockSlim @lock)
        {
            this._lock = @lock;
            this._lock.EnterReadLock();
            this._inner = inner.GetEnumerator();
        }

        #endregion Constructor

        #region Methods

        public bool MoveNext()
        {
            return _inner.MoveNext();
        }

        public void Reset()
        {
            _inner.Reset();
        }

        public void Dispose()
        {
            this._lock.ExitReadLock();
        }

        #endregion Methods

        #region Properties

        public T Current
        {
            get { return _inner.Current; }
        }

        object IEnumerator.Current
        {
            get { return _inner.Current; }
        }

        #endregion Properties
    }
}