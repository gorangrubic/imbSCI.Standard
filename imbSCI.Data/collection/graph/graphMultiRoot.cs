using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Data.collection.graph
{
    using System.Collections;

    /// <summary>
    /// Invisible (not part of the node <see cref="IGraphNode.path"/>) root node for multiple, separate, graph trees.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{System.String, T}}" />
    public class graphMultiRoot<T> : IEnumerable<KeyValuePair<String, T>> where T : class, IGraphNode, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="graphMultiRoot{T}"/> class.
        /// </summary>
        public graphMultiRoot()
        {
        }

        private T instance { get; set; } = new T();

        /// <summary>
        /// Gets the path separator used in this path format - if its not set it will look for parent's default path separator to set it. If there is no parent, it will use <see cref="defaultPathSeparator"/>
        /// </summary>
        /// <value>
        /// The path separator.
        /// </value>
        public virtual string pathSeparator
        {
            get
            {
                return instance.pathSeparator;
            }
        }

        /// <summary>
        /// Putanja objekta
        /// </summary>
        public virtual string path
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public Boolean ContainsKey(String key)
        {
            return items.ContainsKey(key);
        }

        /// <summary>
        /// Removes by the key specified
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(String key)
        {
            if (items.ContainsKey(key)) items.Remove(key);
        }

        /// <summary>
        /// Gets or sets the <see cref="IGraphNode"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="IGraphNode"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T this[String key]
        {
            get
            {
                return items[key];
            }
            set
            {
                if (ContainsKey(key))
                {
                    Remove(key);
                }

                Add(value);
            }
        }

        /// <summary>
        /// Adds the specified <c>newChild</c>, if its name is not already occupied
        /// </summary>
        /// <param name="newChild">The new child.</param>
        /// <returns></returns>
        public bool Add(T newChild)
        {
            if (ContainsKey(newChild.name))
            {
                return false;
            }
            else
            {
                items.Add(newChild.name, newChild);
                return true;
            }
        }

        /// <summary>
        /// Builds graph defined by <c>path</c> or selecte existing graphnode, as pointed by path
        /// </summary>
        /// <param name="path">Path to construct from.</param>
        /// <param name="splitter">The splitter - by default: directory separator.</param>
        /// <returns>Leaf instance</returns>
        public T AddOrGetByPath(String path, String splitter = "")
        {
            if (splitter == "") splitter = pathSeparator;

            List<string> pathParts = imbSciStringExtensions.SplitSmart(path, splitter);

            String first = pathParts.FirstOrDefault();

            if (first.isNullOrEmpty()) return null;

            pathParts.RemoveAt(0);

            T head = null;

            if (!ContainsKey(first))
            {
                head = new T();
                head.name = first;
                Add(head);
            }
            else
            {
                head = this[first];
            }

            T parent = null;

            foreach (string part in pathParts)
            {
                if (head == null)
                {
                    parent = new T();
                    parent.name = part;
                    head = parent;
                }
                else
                {
                    if (head.ContainsKey(part))
                    {
                        head = head[part] as T;
                    }
                    else
                    {
                        T sp = new T();
                        sp.name = part;
                        if (head.Add(sp))
                        {
                            head = sp;
                        };
                    }
                }
            }

            return head;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<String, T>> GetEnumerator() => items.GetEnumerator();

        /// <summary>
        /// Counts this instance.
        /// </summary>
        /// <returns></returns>
        public Int32 Count() => items.Count;

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="autocreate">if set to <c>true</c> [autocreate].</param>
        /// <returns></returns>
        public T Get(String key, Boolean autocreate)
        {
            if (items.ContainsKey(key))
            {
                return items[key];
            }
            return null;
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        public void Add(String key, T item)
        {
            if (!items.ContainsKey(key)) items.Add(key, item);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, T>>)items).GetEnumerator();
        }

        private Dictionary<String, T> _items = new Dictionary<String, T>();

        /// <summary>
        /// Dictionary of the rooted nodes
        /// </summary>
        protected Dictionary<String, T> items
        {
            get
            {
                //if (_items == null)_items = new Dictionary<String, T>();
                return _items;
            }
            set { _items = value; }
        }
    }
}