// --------------------------------------------------------------------------------------------------------------------
// <copyright file="instanceWithRecordCollection.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.data.modelRecords
{
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex.exceptions;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class instanceWithRecordCollection<T, TRecord> : instanceWithRecordCollectionBase<T, TRecord>, IEnumerable<KeyValuePair<T, TRecord>>
        where T : class, IObjectWithName, IObjectWithDescription, IObjectWithNameAndDescription
        where TRecord : modelRecordBase, IModelRecord
    {
        public instanceWithRecordCollection(string __testRunStamp, params T[] Algorithms) : base(__testRunStamp, Algorithms)
        {
        }

        public int Count
        {
            get
            {
                return records.Count;
            }
        }

        public TRecord GetCurrentRecord()
        {
            if (childIndexAtEnd) throw new dataException("Child index at end [" + childIndexCurrent + "]", null, this, "modelRecordCollection [" + typeof(T).Name + "]:[" + typeof(TRecord) + "]");
            return this[childIndexCurrent];
        }

        public T GetCurrentInstance()
        {
            if (childIndexAtEnd) throw new dataException("Child index at end [" + childIndexCurrent + "]", null, this, "modelRecordCollection [" + typeof(T).Name + "]:[" + typeof(TRecord) + "]");
            return items[childIndexCurrent];
        }

        /// <summary>
        /// Finishes all started children
        /// </summary>
        /// <returns></returns>
        public int FinishAllStarted()
        {
            int c = 0;
            foreach (T tr in items)
            {
                if (recordsForTest[tr].state == modelRecordStateEnum.started)
                {
                    recordsForTest[tr]._recordFinish();
                    c++;
                }
            }
            return c;
        }

        /// <summary>
        /// Moves the child index to next position.
        /// </summary>
        /// <returns>Returns false if childIntex is at end</returns>
        public bool MoveNext()
        {
            childIndexCurrent++;

            return !childIndexAtEnd;
        }

        public bool ResetIndex()
        {
            childIndexCurrent = 0;

            return !childIndexAtEnd;
        }

        public bool SetIndexTo(int _index)
        {
            childIndexCurrent = _index;

            return !childIndexAtEnd;
        }

        public bool childIndexAtEnd
        {
            get
            {
                return childIndexCurrent >= items.Count();
            }
        }

        private int _childIndexCurrent = 0; //= new Int32();

        /// <summary> </summary>
        public int childIndexCurrent
        {
            get
            {
                return _childIndexCurrent;
            }
            protected set
            {
                _childIndexCurrent = value;
                OnPropertyChanged("childIndexCurrent");
            }
        }
    }
}