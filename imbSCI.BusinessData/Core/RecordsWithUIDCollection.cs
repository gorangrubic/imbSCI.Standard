using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace imbSCI.BusinessData.Core
{
    public abstract class RecordsWithUIDCollection<T> : IRecordsWithUIDCollection where T : class,IRecordWithGetUID
    {
        public object this[int index]
        {
            get
            {
                return items[index];
            }
            set
            {
                items[index] = value as T;
            }
        }

        public List<T> items { get; set; } = new List<T>();

        public Type ItemType { get { return typeof(T); } }

        [XmlIgnore]
        public string itemTypeName
        {
            get
            {
                return typeof(T).Name;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public int Count => items.Count;

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        private object SyncRootObject = new object();

        public object SyncRoot => SyncRootObject;

        IList IRecordsWithUIDCollection.items => items; //throw new System.NotImplementedException();

        public int Add(object value)
        {
            T item = value as T;
            if (item != null)
            {
                var ritem = AddOrReplace(item);
                return items.IndexOf(item);
            }
            return -1;
        }

        /// <summary>
        /// Adds item and replaces if any existing item is identified by same <see cref="IRecordWithGetUID.GetUID"/>.
        /// </summary>
        /// <param name="item">Item to add or replace</param>
        /// <returns>Existing item that was removed from collection. If no existing item found, returns null</returns>
        public T AddOrReplace(T item)
        {
            var existing = items.FirstOrDefault(x => x.Equals(item));
            if (existing != null)
            {
                items.Remove(existing);

                items.Add(item);
            } else
            {
                items.Add(item);
                
            }

            return existing;
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(object value)
        {
            return items.Contains(value as T);
        }

        public void CopyTo(Array array, int index)
        {
            T[] t_array = new T[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                t_array[i] = array.GetValue(i) as T;
            }

            items.CopyTo(t_array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public int IndexOf(object value)
        {
            return items.IndexOf(value as T);
        }

        public void Insert(int index, object value)
        {
            T item = value as T;
            if (item != null) items.Insert(index, item);
        }

        public void Remove(object value)
        {
            items.Remove(value as T);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }
    }
}