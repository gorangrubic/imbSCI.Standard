// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyExpression.cs" company="imbVeles" >
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
// Project: imbSCI.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Core.data
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Data.collection.graph;
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Graph-tree structure providing access to simple-type (string, numbers, boolean, enums) values (get/set/describe) properties of complex objects
    /// </summary>
    /// <seealso cref="imbSCI.Data.collection.graph.graphNode" />
    public class PropertyExpression : graphNode
    {
        public const String PATH_SPLITER = PropertyExpressionTools.EXPRESSION_PATH_DELIMITER;

        private String _unresolvedPart = "";

        /// <summary>
        /// Part of the expression path that left unresolved
        /// </summary>
        /// <value>
        /// The undesolved part.
        /// </value>
        public String undesolvedPart
        {
            get
            {
                if (parent == null)
                {
                    return _unresolvedPart;
                }
                else
                {
                    PropertyExpression pe = parent as PropertyExpression;
                    return pe.undesolvedPart;
                }
            }
            set
            {
                if (parent == null)
                {
                    _unresolvedPart = value;
                }
                else
                {
                    PropertyExpression pe = parent as PropertyExpression;
                    pe.undesolvedPart = value;
                }
            }
        }

        /// <summary>
        /// Parent expression node
        /// </summary>
        /// <value>
        /// The parent expression.
        /// </value>
        public PropertyExpression parentExpression
        {
            get
            {
                return parent as PropertyExpression;
            }
        }

        private PropertyExpressionStateEnum _state = PropertyExpressionStateEnum.none;

        /// <summary>
        /// State of this expression node, indicating if it was solved or not
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public PropertyExpressionStateEnum state
        {
            get
            {
                if (parentExpression == null) return _state;
                return parentExpression.state;
            }
            set
            {
                if (parentExpression == null)
                {
                    _state = value;
                }
                else
                {
                    parentExpression.state = value;
                }
            }
        }

        private Type _hostType;

        /// <summary>Type of the host instance at this expression node</summary>
        public Type hostType
        {
            get
            {
                return _hostType;
            }
            protected set
            {
                _hostType = value;
            }
        }

        private Object _host;

        /// <summary>Host instance at this expression node </summary>
        public Object host
        {
            get
            {
                return _host;
            }
            protected set
            {
                _host = value;
            }
        }

        private PropertyInfo _property;

        /// <summary>Property information about this expression node</summary>
        public PropertyInfo property
        {
            get
            {
                return _property;
            }
            protected set
            {
                _property = value;
            }
        }

        /// <summary>
        /// Type of the value that is pointed by property expression
        /// </summary>
        /// <value>
        /// The type of the value.
        /// </value>
        public Type valueType { get; protected set; }

        /// <summary>
        /// Opposite to root -- the last child
        /// </summary>
        /// <value>
        /// The leaf.
        /// </value>
        public PropertyExpression Leaf
        {
            get
            {
                if (children.Count > 0)
                {
                    PropertyExpression pe = getFirst() as PropertyExpression;

                    return pe.Leaf;
                }
                else
                {
                    return this;
                }
            }
        }

        public T getTypedValue<T>(T ifNotFoundReturn = default(T))
        {
            return (T)getValue(ifNotFoundReturn);
        }

        public void setTypedValue<T>(T val)
        {
            setValue(val);
        }

        /// <summary>
        /// Gets the value at this expression node
        /// </summary>
        /// <param name="ifNotFoundReturn">If not found return.</param>
        /// <returns></returns>
        public Object getValue(Object ifNotFoundReturn = null)
        {
            if (!IsReady) return ifNotFoundReturn;
            if (isInsideList)
            {
                if (hostList.Count > intIndexKey) return hostList[intIndexKey];
            }
            else if (isInsideDictionary)
            {
                if (hostDictionary.Contains(name)) return hostDictionary[name];
            }
            else if (property != null)
            {
                try
                {
                    //PropertyExpression pe = parent as PropertyExpression;

                    //Object value = property.GetValue(parentExpression.host, null); // host.imbGetPropertySafe(property,null);

                    if (children.Count == 0) return host;
                    //Object value = host.imbGetPropertySafe(property.Name, null);
                    //if (hostType.IsValueType) return host;

                    return property.GetValue(host, null);
                    //return value;
                }
                catch (Exception ex)
                {
                    return ex.Message + " property: " + property.PropertyType.Name + " [" + valueType.Name + "]  host[" + hostType.Name + "]";
                    throw;
                }
            }
            return ifNotFoundReturn;
        }

        private Object CollectionAddLock = new Object();

        private Object DictionaryAddLock = new Object();

        /// <summary>
        /// Sets the value at this expression node
        /// </summary>
        /// <param name="val">The value.</param>
        /// <param name="allowAddToCollection">if set to <c>true</c> [allow add to collection].</param>
        public void setValue(Object val, Boolean allowAddToCollection = false)
        {
            if (!IsWritable) return;

            if (Count() == 0)
            {
                //if (parentExpression != null)
                //{
                //    if (parentExpression.host != null)
                //    {
                //        property.SetValue(parentExpression.host, val, null);
                //    }
                //    else
                //    {
                //        property.SetValue(host, val, null);
                //    }

                //    //parentExpression.host
                //}
                //else
                //{
                host = val;
                //}

                return;
            }

            Object vl = val.imbConvertValueSafe(valueType);
            if (isInsideList)
            {
                if (hostList.Count > intIndexKey) { hostList[intIndexKey] = vl; }
                else if (allowAddToCollection)
                {
                    lock (CollectionAddLock)
                    {
                        if (hostList.Count > intIndexKey) { hostList[intIndexKey] = vl; }
                        else if (allowAddToCollection)
                        {
                            intIndexKey = hostList.Add(vl);
                        }
                    }
                }
            }
            else if (isInsideDictionary)
            {
                if (hostDictionary.Contains(name))
                {
                    hostDictionary[name] = vl;
                }
                else
                {
                    lock (DictionaryAddLock)
                    {
                        if (hostDictionary.Contains(name))
                        {
                            hostDictionary[name] = vl;
                        }
                        else
                        {
                            if (allowAddToCollection) hostDictionary.Add(name, vl);
                        }
                    }
                }
            }
            else
            {
                if (Count() == 0)
                {
                    host = vl;
                }
                else
                {
                    property.SetValue(host, vl, null);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is writable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is writable; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsWritable
        {
            get
            {
                if (property != null)
                {
                    return property.CanWrite;
                }
                return IsReady;
            }
        }

        protected IList hostList
        {
            get { return host as IList; }
        }

        protected IDictionary hostDictionary { get { return host as IDictionary; } }

        /// <summary>
        /// Gets a value indicating whether this instance is ready for reading value
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ready; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsReady
        {
            get
            {
                if (host == null) return false;
                if (isInsideList)
                {
                    if (hostList == null) return false;
                    //if (hostList.Count > intIndexKey)
                    //{
                    //    return true;
                    //} else
                    //{
                    //    return false;
                    //}
                }
                else if (isInsideDictionary)
                {
                    if (hostDictionary == null) return false;

                    //return hostDictionary.Contains(name);
                }
                else if (property == null) return false;
                return true;
            }
        }

        public Boolean isInsideDictionary { get; protected set; }
        public Boolean isInsideList { get; protected set; }

        protected Int32 intIndexKey { get; set; } = 0;

        public override string name
        {
            get
            {
                if (isInsideList)
                {
                    return intIndexKey.ToString();
                }
                return base.name;
            }
            set
            {
                if (isInsideList)
                {
                    if (value.isNumber())
                    {
                        intIndexKey = Int32.Parse(value);
                    }
                }
                base.name = value;
            }
        }

        /// <summary>
        /// Expands this property expression with sub properties, until specified <c>maxDepth</c> <see cref="IGraphNode.level"/>
        /// </summary>
        /// <param name="maxDepth">The maximum depth.</param>
        /// <param name="IncludeReadOnly">if set to <c>true</c> [include read only].</param>
        /// <param name="IncludeListsAndDictionaries">if set to <c>true</c> [include lists].</param>
        public void Expand(Int32 maxDepth = 100, Boolean IncludeReadOnly = false, Boolean IncludeListsAndDictionaries = true)
        {
            if (level < maxDepth)
            {
                if (hostType.IsClass)
                {
                    if (IncludeListsAndDictionaries)
                    {
                        if (parent == null || ((PropertyExpression)parent).host != host)
                        {
                            if (host is IList)
                            {
                                for (int i = 0; i < hostList.Count; i++)
                                {
                                    var subp = new PropertyExpression(hostList, i, this);
                                }
                            }
                            else if (host is IDictionary)
                            {
                                foreach (Object k in hostDictionary)
                                {
                                    if (k != null)
                                    {
                                        if (k is String)
                                        {
                                            var subp = new PropertyExpression(hostDictionary, k as String, this);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //var t = valueType;
                    //if (valueType == null)
                    //{
                    //    t = hostType;
                    //}

                    PropertyInfo[] sub_props = hostType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

                    if (false) //!IsReady)
                    {
                        sub_props = valueType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                        foreach (var sp in sub_props)
                        {
                            if (sp.CanWrite || IncludeReadOnly)
                            {
                                Add(sp.Name);
                            }
                        }
                    }
                    else
                    {
                        Object sh = getValue();

                        foreach (var sp in sub_props)
                        {
                            if (sp.CanWrite || IncludeReadOnly)
                            {
                                Object subhost = host.imbGetPropertySafe(sp, null);

                                //var p = hostType.GetProperty(__pathPart);

                                var subpe = new PropertyExpression(subhost, sp, this);//Add(sp.Name); //
                                //subpe.host = host;

                                subpe.Expand(maxDepth, IncludeReadOnly, IncludeListsAndDictionaries);
                                //var subpe = host.GetExpressionResolved(name + "." +sp.Name); //Add(sp.Name);
                                //subpe.name = sp.Name;
                                //subpe.RegisterWithParent(this);
                            }
                        }
                    }

                    foreach (PropertyExpression ch in children.Values)
                    {
                        ch.Expand(maxDepth, IncludeReadOnly, IncludeListsAndDictionaries);
                    }
                }
            }
        }

        /// <summary>
        /// Includes new property in the tree
        /// </summary>
        /// <param name="__pathPart">The path part.</param>
        /// <returns></returns>
        public PropertyExpression Add(String __pathPart, Object subhost = null)
        {
            //if (valueType == null) valueType = hostType.GetProperty(name)

            //if (subhost == null) subhost = getValue();
            if (subhost == null) subhost = host.imbGetPropertySafe(name, null); //getValue();
            //Object subhost = getValue();//property.GetValue(host, null);// getValue();
            //  if (__pathPart == name) return this;

            // var p = valueType.GetProperty(__pathPart);  //property.PropertyType.GetProperty(__pathPart);//hostType.GetProperty(__pathPart);
            // var subsubhost = subhost.imbGetPropertySafe(__pathPart, null);
            var p = hostType.GetProperty(__pathPart);
            var output = new PropertyExpression(subhost, p, this);

            return output;
        }

        internal void Deploy(Object __host, String __pathPath, PropertyInfo __prop, PropertyExpression __parent, Boolean __isInIndexer = false, Int32 indexInList = -1)
        {
            host = __host;
            if (host != null) hostType = host.GetType();
            if (hostType != null)
            {
                if (__prop != null)
                {
                    property = __prop;
                    valueType = property.PropertyType;
                }
                else if (!__pathPath.isNullOrEmpty())
                {
                    property = hostType.GetProperty(__pathPath);

                    if (property != null)
                    {
                        valueType = property.PropertyType;
                    }
                }
                else if (hostType.IsGenericType && __isInIndexer)
                {
                    valueType = hostType.GetGenericArguments().FirstOrDefault();
                }
            }

            if (__isInIndexer)
            {
                if (indexInList > 0)
                {
                    isInsideList = true;
                    intIndexKey = indexInList;
                }
                else
                {
                    isInsideDictionary = true;
                    name = __pathPath;
                }
                RegisterWithParent(__parent);
            }

            if (property != null)
            {
                name = property.Name;
                RegisterWithParent(__parent);
            }
        }

        protected internal void RegisterWithParent(PropertyExpression __parent)
        {
            if (__parent != null)
            {
                parent = __parent;
                if (name.isNullOrEmpty()) name = "root";
                if (!__parent.children.Contains(this.name))
                {
                    __parent.children.Add(this.name, this);
                }
            }
        }

        /// <summary>
        /// Creates root expression - cannot set value directy, used for expression tree building
        /// </summary>
        /// <param name="__host">The host.</param>
        public PropertyExpression(Object __host, String __name, Int32 expandDepth, Boolean IncludeReadOnly, Boolean IncludeCollections)
        {
            Deploy(__host, __name, null, null, false);
            PropertyInfo[] sub_props = __host.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var sp in sub_props)
            {
                if (sp.CanWrite || IncludeReadOnly)
                {
                    Object subhost = __host.imbGetPropertySafe(sp, null);

                    //var p = hostType.GetProperty(__pathPart);

                    var subpe = new PropertyExpression(subhost, sp, this);//Add(sp.Name); //

                    //var subpe = host.GetExpressionResolved(name + "." +sp.Name); //Add(sp.Name);
                    //subpe.name = sp.Name;
                    //subpe.RegisterWithParent(this);
                }
            }

            foreach (PropertyExpression ch in children.Values)
            {
                ch.Expand(expandDepth, IncludeReadOnly, IncludeCollections);
            }

            // Expand(expandDepth, IncludeReadOnly, IncludeCollections);
            //if (__host is IObjectWithName)
            //{
            //    IObjectWithName __host_IObjectWithName = (IObjectWithName)__host;
            //    name = __host_IObjectWithName.name;
            //}
            //else
            //{
            //    name = "[" + __host.GetType().Name + "]";
            //}
        }

        public PropertyExpression()
        {
        }

        /// <summary>
        /// Adds sub PropertyExpression for property inside <see cref="IList"/>
        /// </summary>
        /// <param name="__host">The host.</param>
        /// <param name="__index">The index.</param>
        /// <param name="__parent">The parent.</param>
        public PropertyExpression(IDictionary __host, String __key, PropertyExpression __parent = null)
        {
            Deploy(__host, __key, null, __parent, true);
        }

        /// <summary>
        /// Adds sub PropertyExpression for property inside <see cref="IList"/>
        /// </summary>
        /// <param name="__host">The host.</param>
        /// <param name="__index">The index.</param>
        /// <param name="__parent">The parent.</param>
        public PropertyExpression(IList __host, Int32 __index, PropertyExpression __parent = null)
        {
            Deploy(__host, null, null, __parent, true, __index);
        }

        protected PropertyExpression(Object __host, PropertyInfo __property, PropertyExpression __parent = null)
        {
            Deploy(__host, null, __property, __parent);
        }

        public PropertyExpression(Object __host, String __pathPart, PropertyExpression __parent = null)
        {
            Deploy(__host, __pathPart, null, __parent);
        }
    }
}