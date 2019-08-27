// --------------------------------------------------------------------------------------------------------------------
// <copyright file="modelSideRecordSetCollection.cs" company="imbVeles" >
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
    using imbSCI.Data.data;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{TSideInstance, System.Collections.Generic.List{TSideRecord}}}'
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    /// <summary>
    /// Collection used to store side records tracked by <see cref="modelRecordSummaryBase{TInstance, TSideInstance, TSideRecord}"/>
    /// </summary>
    /// <typeparam name="TSideInstance">The type of the side instance.</typeparam>
    /// <typeparam name="TSideRecord">The type of the side record.</typeparam>
    /// <seealso cref="aceCommonTypes.primitives.imbBindable" />
    /// <seealso cref="System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{TSideInstance, System.Collections.Generic.List{TSideRecord}}}" />
    public class modelSideRecordSetCollection<TSideInstance, TSideRecord> : imbBindable, IEnumerable<KeyValuePair<TSideInstance, List<TSideRecord>>>
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{TSideInstance, System.Collections.Generic.List{TSideRecord}}}'
#pragma warning restore CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    where TSideRecord : modelRecordBase, IModelRecord
    {
        private List<TSideInstance> _sideInstances = new List<TSideInstance>();

        /// <summary> </summary>
        public List<TSideInstance> sideInstances
        {
            get
            {
                return _sideInstances;
            }
            protected set
            {
                _sideInstances = value;
                OnPropertyChanged("sideInstances");
            }
        }

        public void AddRecord(TSideInstance __instance, TSideRecord __record)
        {
            var records = GetRecords(__instance);
            records.Add(__record);
            sideInstances.Add(__instance);
        }

        public List<TSideRecord> GetRecords(TSideInstance __instance)
        {
            if (!items.ContainsKey(__instance))
            {
                items.Add(__instance, new List<TSideRecord>());
            }
            return items[__instance];
        }

        public List<TSideInstance> GetInstances()
        {
            return sideInstances.ToList();

            //return items[__instance];
        }

        private Dictionary<TSideInstance, List<TSideRecord>> _items = new Dictionary<TSideInstance, List<TSideRecord>>();

        /// <summary> </summary>
        public Dictionary<TSideInstance, List<TSideRecord>> items
        {
            get
            {
                return _items;
            }
            protected set
            {
                _items = value;
                OnPropertyChanged("items");
            }
        }

        public IEnumerator<KeyValuePair<TSideInstance, List<TSideRecord>>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TSideInstance, List<TSideRecord>>>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TSideInstance, List<TSideRecord>>>)items).GetEnumerator();
        }
    }
}