// --------------------------------------------------------------------------------------------------------------------
// <copyright file="instanceCountPipeLine.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataComplex.special
{
    using imbSCI.DataComplex.exceptions;
    using System.Collections.Generic;

    /// <summary>
    /// Parent-connected instance counter with ability to "pump" all new counts trough upstram parent chain
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="instanceCountCollection{T}" />
    public class instanceCountPipeLine<T>
    {
        /// <summary>
        /// Gets a value indicating whether this instance has any record.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has any record; otherwise, <c>false</c>.
        /// </value>
        public bool HasAnyRecord
        {
            get
            {
                return (Count != 0);
            }
        }

        /// <summary>
        /// Gets the count of <see cref="self"/>
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get
            {
                return self.Count;
            }
        }

        /// <summary> </summary>
        public instanceCountCollection<T> self { get; protected set; } = new instanceCountCollection<T>();

        /// <summary>
        /// Locks the source.
        /// </summary>
        /// <param name="sourceId">The source identifier.</param>
        /// <returns></returns>
        public bool LockSource(string sourceId)
        {
            if (!lockedSourceIds.Contains(sourceId))
            {
                lockedSourceIds.Add(sourceId);
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary> </summary>
        public bool propagationPaused { get; protected set; } = false;

        /// <summary>
        /// Sets pipe-line to pause
        /// </summary>
        public void HoldOn()
        {
            propagationPaused = true;
        }

        /// <summary>
        /// To be called after all instances were passed to <see cref="self"/>
        /// </summary>
        /// <param name="sourceID">The source identifier.</param>
        public void FlushAndLock(string sourceID)
        {
            if (lockedSourceIds.Contains(sourceID)) return;
            propagationPaused = false;

            LockSource(sourceID);
            if (parent != null) parent.Add(self, sourceID);
        }

        public bool Add(IEnumerable<T> source, string sourceID)
        {
            if (lockedSourceIds.Contains(sourceID)) return false;

            if (!propagationPaused) LockSource(sourceID);

            self.AddInstanceRange(source);

            if (!propagationPaused)
            {
                foreach (T it in source)
                {
                    sendUpstream(it, sourceID);
                }
            }
            return true;
        }

        /// <summary>
        /// Prefered way of piping Copies complete score list to <see cref="self"/> and automatically lock this source
        /// </summary>
        /// <param name="source">The source: spider record model element that sent these count results</param>
        /// <param name="sourceID">The source (origin of these statistics) identifier.</param>
        public bool Add(instanceCountCollection<T> source, string sourceID)
        {
            if (lockedSourceIds.Contains(sourceID)) return false;

            if (!propagationPaused) LockSource(sourceID);

            self.AddInstanceRange(source);

            if (!propagationPaused)
            {
                foreach (T it in source.Keys)
                {
                    sendUpstream(it, sourceID, source[it]);
                }
            }
            return true;
        }

        /// <summary>
        /// Iterative way of pipeing. Use <see cref="Add(instanceCountCollection{T}"/> if possible
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="sourceID">The source identifier.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Add(T item, string sourceID, int value = 1)
        {
            if (lockedSourceIds.Contains(sourceID)) return false;

            self.AddInstance(item, value);
            if (!propagationPaused) sendUpstream(item, sourceID, value);
            return true;
        }

        /// <summary> </summary>
        public List<string> lockedSourceIds { get; protected set; } = new List<string>();

        protected bool sendUpstream(T item, string sourceID, int value = 1)
        {
            if (parent == null) return false;
            parent.Add(item, sourceID, value);
            return true;
        }

        private instanceCountPipeLine<T> _parent;

        /// <summary> </summary>
        public instanceCountPipeLine<T> parent
        {
            get
            {
                return _parent;
            }
            set
            {
                if (value == this) throw new dataException("Parent to self call in Set operation", null, this, "Parent to self call");
                _parent = value;
            }
        }
    }
}