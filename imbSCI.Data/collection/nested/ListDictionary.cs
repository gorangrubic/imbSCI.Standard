using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace imbSCI.Data.collection.nested
{

    /// <summary>
    /// Dictionary with List{TItem} instances for {TKey}. 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public class ListDictionaryBase<TKey, TItem> : IEnumerable<KeyValuePair<TKey, List<TItem>>>
    {

        // protected Dictionary<TKey, List<TItem>> dictionary { get; set; } = new Dictionary<TKey, List<TItem>>();

        public ListDictionaryBase()
        {

        }

        public XmlSchema GetSchema()
        {
            return null;
        }


        public const String NODENAME_Entries = "Entries";
        public const String NODENAME_Entry = "Entry";
        public const String NODENAME_Key = "Key";
        public const String NODENAME_Values = "Values";
        public const String NODENAME_Value = "Value";



        protected List<TItem> Get(TKey key)
        {
            if (!dictionary.ContainsKey(key))
            {
                lock (_dictionaryGet_lock)
                {

                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary.Add(key, new List<TItem>());
                        // add here if any additional initialization code is required
                    }
                }
            }
            return dictionary[key];
        }

        public IEnumerator<KeyValuePair<TKey, List<TItem>>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, List<TItem>>>)dictionary).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, List<TItem>>>)dictionary).GetEnumerator();
        }

        private Object _dictionaryGet_lock = new Object();
        private Dictionary<TKey, List<TItem>> _dictionary = new Dictionary<TKey, List<TItem>>();
        /// <summary>
        /// Dictionary keeping all List instances 
        /// </summary>
        protected Dictionary<TKey, List<TItem>> dictionary
        {
            get
            {
                return _dictionary;
            }
        }

        public Int32 Count
        {
            get
            {
                return dictionary.Sum(x => x.Value.Count);
            }
        }

        public Int32 CountKeys(Boolean onlyNonEmpty = true)
        {
            if (onlyNonEmpty)
            {
                return dictionary.Count(x => x.Value.Any());
            }
            else
            {
                return dictionary.Count;
            }

        }

        /// <summary>
        /// Removes all entries having no {TItem}s in its list.
        /// </summary>
        /// <returns>List of removed keys</returns>
        public List<TKey> RemoveEmptyLists()
        {
            List<TKey> toRemove = new List<TKey>();
            foreach (var pair in this)
            {
                if (!this[pair.Key].Any())
                {
                    toRemove.Add(pair.Key);
                }
            }

            foreach (var key in toRemove)
            {
                dictionary.Remove(key);
            }
            return toRemove;
        }

        public void AddRange(TKey key, List<TItem> items, Boolean unique = true)
        {
            foreach (TItem item in items)
            {
                if (unique)
                {
                    if (!this[key].Contains(item))
                    {
                        this[key].Add(item);
                    }
                }
                else
                {
                    this[key].Add(item);
                }
            }

        }

        public void AddRange(IEnumerable<KeyValuePair<TKey, List<TItem>>> source, Boolean AddUniqueValue = true)
        {
            foreach (var pair in source)
            {
                foreach (var v in pair.Value)
                {
                    if (!this[pair.Key].Contains(v) || !AddUniqueValue)
                    {
                        this[pair.Key].Add(v);
                    }
                }
               
            }
        }

        /// <summary>
        /// Gets the <see cref="List{TItem}"/> under the specified key. It will autocreate List{TItem} under specified <c>key</c> if not existing. Thread-safe.
        /// </summary>
        /// <value>
        /// The <see cref="List{TItem}"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public List<TItem> this[TKey key]
        {
            get
            {
                return Get(key);
            }
        }

    }
}