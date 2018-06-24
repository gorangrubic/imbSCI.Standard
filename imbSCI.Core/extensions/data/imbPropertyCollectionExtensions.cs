// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbPropertyCollectionExtensions.cs" company="imbVeles" >
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
namespace imbSCI.Core.extensions.data
{
    using imbSCI.Core.collection;
    using imbSCI.Core.data;
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Extensions to process and access data from or to PropertyCollection
    /// </summary>
    /// \ingroup_disabled ace_ext_datastructs
    public static class imbPropertyCollectionExtensions
    {
        /// <summary>
        /// From IEnumerable{Enum} makes <see cref="PropertyCollection" /> where Key is <see cref="Type.Name" /> and Value is the <see cref="Enum" /> or <see cref="Enum.ToString(string)" /> if <c>format</c> is specified
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="format">The format for .ToString() conversion of the <see cref="Enum"/> value. If it is not null but <c>empty</c> string: the value will be <see cref="Enum.ToString()"/>.</param>
        /// <returns></returns>
        public static PropertyCollection toPropertyCollection(this IEnumerable<Enum> source, String format = null)
        {
            PropertyCollection output = new PropertyCollection();

            foreach (Enum en in source)
            {
                if (format != null)
                {
                    if (format == "")
                    {
                        output.Add(en.GetType().Name, en.toStringSafe());
                    }
                    else
                    {
                        output.Add(en.GetType().Name, en.ToString(format));
                    }
                }
                else
                {
                    output.Add(en.GetType().Name, en.toStringSafe());
                }
            }

            return output;
        }

        public static PropertyCollection toPropertyCollection(this IDictionary source)
        {
            PropertyCollection output = new PropertyCollection();

            var en = source.GetEnumerator();
            do
            {
                if (imbSciStringExtensions.isNullOrEmptyString(en.Key)) return output;
                output.Add(en.Key, en.Value);
            } while (en.MoveNext());

            return output;
        }

        /// <summary>
        /// Copies all members of one collection to another. Use <c>AppendData</c> for smarter operations.
        /// </summary>
        /// <seealso cref="imbPropertyCollectionExtensions.AppendData(PropertyCollection, PropertyCollection, existingDataMode)"/>
        /// <param name="target">Collection to copy into</param>
        /// <param name="source">Collection to copy from</param>
        public static void copyInto(this PropertyCollection target, PropertyCollection source)
        {
            foreach (Object k in source.Keys)
            {
                target[k] = source[k];
            }
        }

        /// <summary>
        /// Sets all where key without prefix matches property name
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns>Number of properties ser</returns>
        public static Int32 setToObject(this PropertyCollection source, Object target, String prefix = "")
        {
            Int32 c = 0;
            foreach (Enum key in source.Keys)
            {
                if (source.setToObject(target, key, prefix))
                {
                    c++;
                }
            }
            return c;
        }

        /// <summary>
        /// Sets all where key without prefix matches property name
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns>Number of properties ser</returns>
        public static Int32 setFromObject(this PropertyCollection source, Object target)
        {
            Int32 c = 0;
            foreach (Object key in source.Keys)
            {
                String key_string = key.toStringSafe();
                source[key] = target.imbGetPropertySafe(key_string, "");
                c++;
            }
            return c;
        }

        /// <summary>
        /// Reads values from source object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="skipExisting">if set to <c>true</c> [skip existing].</param>
        /// <returns></returns>
        public static Int32 addOrSetFromObject(this PropertyCollection target, Object source, Type enumType = null, Boolean skipExisting = false)
        {
            target.setFromEnumType(enumType);
            Int32 c = 0;
            foreach (Enum key in target.Keys)
            {
                if (target.addOrSetFromObject(source, key, skipExisting))
                {
                    c++;
                }
            }
            return c;
        }

        public static Boolean addOrSetFromObject(this PropertyCollection target, Object source, Enum field, Boolean skipExisting = false)
        {
            String fname = imbSciStringExtensions.SplitOnce(field.ToString(), "_").LastOrDefault();
            return target.add(field, source.imbGetPropertySafe(fname), skipExisting);
        }

        /// <summary>
        /// Sets to object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="field">The field.</param>
        /// <param name="prefix">The prefix that will help more efficient execution</param>
        /// <returns></returns>
        public static Boolean setToObject(this PropertyCollection source, Object target, Enum field, String prefix = "")
        {
            if (!source.ContainsKey(field)) return false;
            String fname = "";
            var fieldName = field.ToString();
            if (prefix.isNullOrEmpty())
            {
                var prts = imbSciStringExtensions.SplitOnce(fieldName, "_");
                fname = prts.LastOrDefault();
                prefix = prts.FirstOrDefault();
            }

            if (fname == "" && fieldName.StartsWith(prefix))
            {
                fname = imbSciStringExtensions.removeStartsWith(field.ToString(), prefix);
            }

            if (fname.isNullOrEmpty())
            {
                return false;
            }

            target.imbSetPropertySafe(fname, source.get(field));

            return false;
        }

        /// <summary>
        /// Populates the collection with empt
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="enumType">Type of the enum.</param>
        public static void setFromEnumType(this PropertyCollection target, Type enumType, Boolean skipExisting = true)
        {
            if (enumType == null) return;
            if (!enumType.isEnum()) return;

            var enums = Enum.GetValues(enumType);
            foreach (var en in enums)
            {
                target.add((Enum)en, null, skipExisting);
            }
        }

        //-------------------

        /// <summary>
        /// Gets the and remove proper string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        public static String getAndRemoveProperString(this PropertyCollection source, params Enum[] fields)
        {
            return getAndRemoveProperField(source, fields).toStringSafe();
        }

        /// <summary>
        /// Combines <c>getProperString()</c> results and separator string to create output.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        /// \ingroup_disabled ace_ext_collections_highlight
        public static String getStringLine(this PropertyCollection source, String separator = "-", params Enum[] fields)
        {
            String outStr = "";
            for (int i = 0; i < fields.Length; i++)
            {
                if (source.ContainsKey(fields[i]))
                {
                    outStr = outStr.add(fields[i].toStringSafe(), separator);
                }
            }

            return outStr;
        }

        public static Object getAndRemove(this PropertyCollection source, String field, String defaultValue = "")
        {
            Object output = defaultValue;
            if (source.ContainsKey(field))
            {
                output = source[field];
                source.Remove(field);
            }
            return output;
        }

        /// <summary>
        /// Prefered overload - finds the first proper value (not empty, not null) in <c>source</c> using fiven <c>fields</c> or returns <c>defaultValue</c> if no field was found.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="fields">The fields to test for proper value. Follows the given order</param>
        /// <returns>
        /// First proper value found or <c>defaultValue</c> if nothing found
        /// </returns>
        /// \ingroup_disabled ace_ext_collections
        public static String getProperString(this PropertyCollection source, String defaultValue, params Enum[] fields)
        {
            String output = getProperField(source, fields).toStringSafe();
            if (imbSciStringExtensions.isNullOrEmptyString(output)) output = defaultValue;
            return output;
        }

        /// <summary>
        /// Finds the first proper value (not empty, not null) in <c>source</c> using fiven <c>fields</c>
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="fields">The fields to test for proper value. Follows the given order</param>
        /// <returns>First proper value found</returns>
        /// \ingroup_disabled ace_ext_collections
        public static String getProperString(this PropertyCollection source, params Enum[] fields)
        {
            return getProperField(source, fields).toStringSafe();
        }

        private static Dictionary<Object, String> _EnumToStringValues = new Dictionary<Object, String>();

        /// <summary>
        ///
        /// </summary>
        private static Dictionary<Object, String> EnumToStringValues
        {
            get
            {
                return _EnumToStringValues;
            }
            set
            {
                _EnumToStringValues = value;
            }
        }

        public static String toString(this Enum val)
        {
            if (!EnumToStringValues.ContainsKey(val)) EnumToStringValues[val] = val.ToString();

            return EnumToStringValues[val];
        }

        public static String keyToString(this Object val)
        {
            if (!EnumToStringValues.ContainsKey(val)) EnumToStringValues[val] = val.ToString();

            return EnumToStringValues[val];
        }

        /// <summary>
        /// Gets value using the specified key or its <c>ToString()</c> form if not found on first try
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Value for the key or default value</returns>
        /// \ingroup_disabled ace_ext_collections
        public static Object get(this PropertyCollection source, Object key, Object defaultValue = null)
        {
            Object output = null;
            if (source.ContainsKey(key))
            {
                return source[key];
            }
            else
            {
                String kstr = key.keyToString();
                if (source.ContainsKey(kstr))
                {
                    return source[kstr];
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the and remove proper object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        public static T getAndRemoveProperObject<T>(this PropertyCollection source, params Enum[] fields) where T : class, new()
        {
            fields = fields.getFlatArray<Enum>();

            foreach (Enum field in fields)
            {
                Object str = source.get(field);

                if (str != null)
                {
                    if (str is T)
                    {
                        T strs = str as T;
                        if (strs != null)
                        {
                            source.Remove(field);
                            return strs;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the field in proper <c>T</c> type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="fields">The fields.</param>
        /// <returns>Typed object</returns>
        public static T getProperObject<T>(this PropertyCollection source, params Enum[] fields) where T : class
        {
            fields = fields.getFlatArray<Enum>();

            if (!fields.Any())
            {
                return source.getFirstOfType<T>(false, null, true);
            }
            for (int i = 0; i < fields.Length; i++)
            {
                if (source.ContainsKey(fields[i]))
                {
                    Object obj = source[fields[i]];
                    return (T)obj;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the first proper value (not empty, not null) in <c>source</c> using given <c>fields</c> and removes original data from the source
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="fields">The fields to test for proper value. Follows the given order</param>
        /// <returns>First proper value found</returns>
        /// \ingroup_disabled ace_ext_collections_highlight
        public static Object getAndRemoveProperField(this PropertyCollection source, params Enum[] fields)
        {
            fields = fields.getFlatArray<Enum>();

            foreach (Enum field in fields)
            {
                Object str = source.get(field);

                if (str != null)
                {
                    if (str is String)
                    {
                        String strs = str as String;
                        if (!imbSciStringExtensions.isNullOrEmptyString(strs))
                        {
                            source.Remove(field);
                            return str;
                        }
                    }
                    else
                    {
                        source.Remove(field);
                        return str;
                    }
                }
            }
            return "";
        }

        public static T getProperEnum<T>(this PropertyCollection source, T defaultValue, params Enum[] fields) where T : struct, IConvertible
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (source.ContainsKey(fields[i]))
                {
                    Object output = fields[i];
                    return (T)output;
                }
            }
            return defaultValue;

            /*
            Object result = source.getProperField(fields);
            if (result == null)
            {
                return source.getFirstOfType<T>(false, defaultValue);
            }

            if (result is Enum)
            {
                return (T)result;
            } else
            {
                return defaultValue;
            }*/
        }

        /// <summary>
        /// Gets the proper boolean: first entry found using <c>fields</c> -- encapsulates <see cref="getProperField(PropertyCollection, Enum[])"/>
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="defaultValue">Result on not found or if value is string</param>
        /// <param name="fields">The fields to seach</param>
        /// <returns></returns>
        public static Boolean getProperBoolean(this PropertyCollection source, Boolean defaultValue = false, params Enum[] fields)
        {
            Object result = source.getProperField(fields);
            if (result is String) return defaultValue;
            if (result is Boolean) return (Boolean)result;
            return Convert.ToBoolean(result);
        }

        /// <summary>
        /// Gets the proper int32.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        public static Int32 getProperInt32(this PropertyCollection source, Int32 defaultValue = -1, params Enum[] fields)
        {
            Object result = source.getProperField(fields);
            if (result is String) return defaultValue;
            if (result is Int32) return (Int32)result;
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Finds the first proper value (not empty, not null) in <c>source</c> using fiven <c>fields</c>
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="fields">The fields to test for proper value. Follows the given order</param>
        /// <returns>First proper value found</returns>
        /// \ingroup_disabled ace_ext_collections_highlight
        public static Object getProperField(this PropertyCollection source, params Enum[] fields)
        {
            //fields = fields.getFlatArray<Enum>();
            for (int i = 0; i < fields.Length; i++)
            {
                if (source.ContainsKey(fields[i]))
                {
                    return source[fields[i]];
                }
            }

            for (int i = 0; i < fields.Length; i++)
            {
                String str = fields[i].keyToString();
                if (source.ContainsKey(str))
                {
                    return source[fields[i]];
                }
            }

            return "";
        }

        // -----------------------------CONTAINs KEYS

        /// <summary>
        /// Determines whether the specified key contains key - by testing ToString() value it direct test returns false
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean containsKey(this PropertyCollection source, Enum key)
        {
            Boolean output = false;
            if (source.ContainsKey(key))
            {
                output = true;
            }
            else
            {
                String str = key.keyToString();
                if (source.ContainsKey(str))
                {
                    output = true;
                }
            }
            return output;
        }

        /// <summary>
        /// Determines whether collection contains at least one key - field.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="fields">The fields.</param>
        /// <returns>
        ///   <c>true</c> if [contains any of keys] [the specified fields]; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean containsAnyOfKeys(this PropertyCollection source, params Enum[] fields)
        {
            fields = fields.getFlatArray<Enum>();

            foreach (Enum field in fields)
            {
                if (source.containsKey(field))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether contains all of keys - the specified fields.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="fields">The fields.</param>
        /// <returns>
        ///   <c>true</c> if [contains all of keys] [the specified fields]; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean containsAllOfKeys(this PropertyCollection source, params Enum[] fields)
        {
            fields = fields.getFlatArray<Enum>();
            foreach (Enum field in fields)
            {
                if (!source.containsKey(field))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether contains values for each of the specified types.
        /// </summary>
        /// <param name="source">The source collection</param>
        /// <param name="types">The types that require values</param>
        /// <returns>
        ///   <c>true</c> if contains values for each of the specified types; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean containsAllOfTypes(this PropertyCollection source, params Type[] types)
        {
            Type[] typ = types.getFlatArray<Type>();

            foreach (var vl in source.Values)
            {
                if (!typ.Contains<Type>(vl.GetType()))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether contains any value being one the specified types.
        /// </summary>
        /// <param name="source">The source collection</param>
        /// <param name="types">The types to test values against</param>
        /// <returns>
        ///   <c>true</c> if [contains any of types] [the specified types]; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean containsAnyOfTypes(this PropertyCollection source, params Type[] types)
        {
            Type[] typ = types.getFlatArray<Type>();

            foreach (var vl in source.Values)
            {
                if (typ.Contains<Type>(vl.GetType()))
                {
                    return true;
                }
            }

            return false;
        }

        // -------------------------- ADD and APPEND

        /// <summary>
        /// Appends the specified key and value accordint the specified <see cref="aceCommonTypes.enums.existingDataMode"/>. Returns TRUE if <c>newValueCandidate</c> was written as result of the <c>policy</c> specified.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="key">The key.</param>
        /// <param name="newValueCandidate">The new value candidate - it will be or not written under specified <c>key</c> depending on <c>policy</c> and current <c>data</c> </param>
        /// <param name="policy">The policy on handling existing entry for the <c>key</c> specified</param>
        /// <returns>FALSE if <c>newValueCandidate</c> was not written into <c>data</c> collection</returns>
        public static Boolean Append(this PropertyCollection data, Object key, Object newValueCandidate, existingDataMode policy)
        {
            if (newValueCandidate == null) return false;
            switch (policy)
            {
                case existingDataMode.clearExisting:
                    data.Clear();
                    data.Add(key, data);
                    break;

                case existingDataMode.filterAndLeaveExisting:
                    if (data.ContainsKey(key))
                    {
                        //data[key] = newValueCandidate;
                        return false;
                    }
                    else
                    {
                        if (key is Enum)
                        {
                            data.add((Enum)key, newValueCandidate, false);
                        }
                        else
                        {
                            data.Add(key.toStringSafe(), newValueCandidate);
                        }
                    }
                    break;

                case existingDataMode.filterNewOverwriteExisting:
                    if (data.ContainsKey(key))
                    {
                        data[key] = newValueCandidate;
                    }
                    return false;
                    break;

                case existingDataMode.leaveExisting:
                    return false;
                    break;

                case existingDataMode.overwriteExistingIfEmptyOrNull:

                    if (data.ContainsKey(key))
                    {
                        if (imbSciStringExtensions.isNullOrEmptyString(data[key]))
                        {
                            data[key] = newValueCandidate;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        data.Add(key, newValueCandidate);
                    }

                    break;

                case existingDataMode.overwriteExisting:

                    if (data.ContainsKey(key))
                    {
                        data[key] = newValueCandidate;
                    }
                    else
                    {
                        data.Add(key, newValueCandidate);
                    }

                    break;

                case existingDataMode.sumWithExisting:

                    if (data.ContainsKey(key))
                    {
                        Object result = data[key].sumValues(newValueCandidate);
                    }
                    else
                    {
                        data.Add(key, newValueCandidate);
                    }

                    break;
            }
            return true;
        }

        /// <summary>
        /// Combines two PropertyCollections according <c>policy</c> specified
        /// </summary>
        /// <param name="existing">Collection that will be changed</param>
        /// <param name="data">New data to append</param>
        /// <param name="policy">How to manage key duplicates</param>
        /// \ingroup_disabled ace_ext_collections_highlight
        public static void AppendData(this PropertyCollection existing, PropertyCollection data, existingDataMode policy, Boolean showException = true)
        {
            PropertyCollection temp = new PropertyCollection();
            if ((data == null) && showException)
            {
                throw new ArgumentNullException("data", "AppendData failed because data is null");
                return;
            }
            if (data == null) return;

            switch (policy)
            {
                case existingDataMode.clearExisting:
                    existing.Clear();
                    existing.AddRange(data);
                    break;

                case existingDataMode.filterAndLeaveExisting:
                    foreach (DictionaryEntry input in data)
                    {
                        if (existing.ContainsKey(input.Key))
                        {
                            temp.Add(input.Key, existing[input.Key]);
                        }
                    }
                    existing.Clear();
                    existing.AddRange(temp);
                    break;

                case existingDataMode.filterNewOverwriteExisting:
                    foreach (DictionaryEntry input in data)
                    {
                        if (existing.ContainsKey(input.Key))
                        {
                            existing[input.Key] = input.Value;
                        }
                    }
                    break;

                case existingDataMode.leaveExisting:
                    existing.AddRange(data, true);
                    break;

                case existingDataMode.overwriteExistingIfEmptyOrNull:
                    foreach (DictionaryEntry input in data)
                    {
                        if (existing.ContainsKey(input.Key))
                        {
                            if (imbSciStringExtensions.isNullOrEmptyString(existing[input.Key].toStringSafe("")))
                            {
                                existing[input.Key] = input.Value;
                            }
                        }
                        else
                        {
                            existing.Add(input.Key, input.Value);
                        }
                    }
                    break;

                case existingDataMode.overwriteExisting:
                    foreach (DictionaryEntry input in data)
                    {
                        if (existing.ContainsKey(input.Key))
                        {
                            existing[input.Key] = input.Value;
                        }
                        else
                        {
                            existing.Add(input.Key, input.Value);
                        }
                    }
                    break;

                case existingDataMode.sumWithExisting:
                    foreach (DictionaryEntry input in data)
                    {
                        if (existing.ContainsKey(input.Key))
                        {
                            Object result = existing[input.Key].sumValues(input.Value);
                        }
                        else
                        {
                            existing.Add(input.Key, input.Value);
                        }
                    }
                    break;

                default:
                    throw new NotImplementedException(imbStringFormats.toStringSafe(policy) + " called but not implemented");
                    break;
            }
        }

        /// <summary>
        /// Adds the object to multiple keys
        /// </summary>
        /// <param name="existing">The existing.</param>
        /// <param name="data">The data.</param>
        /// <param name="skipExistingKeys">if set to <c>true</c> [skip existing keys].</param>
        /// <param name="keys">The keys to set this </param>
        /// <returns>Number of fields updated</returns>
        public static Int32 addObjectToMultikeys(this PropertyCollection existing, Object data, Boolean skipExistingKeys, params Enum[] keys)
        {
            List<Enum> keyList = keys.getFlatList<Enum>();
            Int32 c = 0;
            foreach (Enum key in keyList)
            {
                if (existing.ContainsKey(key))
                {
                    if (!skipExistingKeys)
                    {
                        //c++;
                        existing[key] = data;
                        c++;
                    }
                }
                else
                {
                    existing.Add(key, data);
                    c++;
                }
            }
            return c;
        }

        /// <summary>
        /// Adds the string to multikeys.
        /// </summary>
        /// <param name="existing">The existing.</param>
        /// <param name="data">String to write. If its null or empty nothing is done</param>
        /// <param name="skipExistingKeys">if set to <c>true</c> [skip existing keys].</param>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        public static Int32 addStringToMultikeys(this PropertyCollection existing, String data, Boolean skipExistingKeys, params Enum[] keys)
        {
            if (imbSciStringExtensions.isNullOrEmptyString(data)) return 0;
            return existing.addObjectToMultikeys(data, skipExistingKeys, keys);
        }

        /// <summary>
        /// Safe way to add or update
        /// </summary>
        /// <param name="existing">The existing.</param>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="skipExistingKeys">if set to <c>true</c> [skip existing keys].</param>
        /// <returns>TRUE if it was added, and FALSE if criteria never met</returns>
        ///  \ingroup_disabled ace_ext_collections
        public static Boolean add(this PropertyCollection existing, Enum key, Object data, Boolean skipExistingKeys = false)
        {
            if (existing.ContainsKey(key))
            {
                if (!skipExistingKeys)
                {
                    //c++;
                    existing[key] = data;
                    return true;
                }
            }
            else
            {
                existing.Add(key, data);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds or replaces values of existing with values from data. Overwrite is off if <c>skipExistingKeys</c> is TRUE
        /// </summary>
        /// <param name="existing">Collection to modify</param>
        /// <param name="data">input data</param>
        /// <param name="skipExistingKeys">on TRUE it will not replace existing values</param>
        /// <returns>Number of newly added key/value pairs. -1 if <c>data</c> is <c>null</c></returns>
        /// \ingroup_disabled ace_ext_collections
        public static Int32 AddRange(this PropertyCollection existing, PropertyCollection data, Boolean skipExistingKeys = false)
        {
            Int32 c = 0;
            if (data == null)
            {
                return -1;
            }
            if (existing == data)
            {
                return -1;
            }
            foreach (DictionaryEntry input in data)
            {
                if (existing.ContainsKey(input.Key))
                {
                    if (!skipExistingKeys)
                    {
                        //c++;
                        existing[input.Key] = input.Value;
                    }
                }
                else
                {
                    c++;
                    existing.Add(input.Key, input.Value);
                }
            }
            return c;
        }

        // ------------------- BUILDS

        /// <summary>
        /// Builds DataTable with two columns: Property name and Value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// \ingroup_disabled ace_ext_collections_highlight
        public static DataTable buildDataTable<T>(this PropertyCollection source) where T : PropertyCollection, new()
        {
            Type t = typeof(T);

            String title = t.Name.imbTitleCamelOperation(true);

            //DataTable output = new DataTable(title);
            //var propertyName = output.Columns.Add("Property");
            //var propertyValue = output.Columns.Add("Value");

            //foreach (DictionaryEntry prop in source)
            //{
            //    var rw = output.Rows.Add();
            //    rw[propertyName] = prop.Key;
            //    rw[propertyValue] = prop.Value;
            //}
            return source.buildDataTable(title);
        }

        public static DataTable buildDataTable(this PropertyCollection source, String title)
        {
            DataTable output = new DataTable(title);
            var propertyName = output.Columns.Add("Property");
            var propertyValue = output.Columns.Add("Value");

            foreach (DictionaryEntry prop in source)
            {
                var rw = output.Rows.Add();
                rw[propertyName] = prop.Key;
                rw[propertyValue] = prop.Value;
            }
            return output;
        }

        /// <summary>
        /// Builds the property collection list from <c>IEnumerable</c> source collection
        /// </summary>
        /// <param name="source">Any enumerable collection of any object with public properties</param>
        /// <param name="doOnlyWithDisplayName">if set to <c>true</c> it will only include properties with declared <c>DisplayNameAttribute</c> .</param>
        /// <param name="doInherited">if set to <c>true</c> it will get inherited propertis too.</param>
        /// <param name="filedName_prefix">Name prefix to be applied before property name in order to be compatibile with templateField... enum type members</param>
        /// <param name="output">List with one <c>PropertyCollection</c> per each <c>Object</c> in source</param>
        /// <param name="fieldsOrCategoriesToShow">The fields or categories to show.</param>
        /// <returns>List of <c>PropertyCollection</c> with data obtained from <c>source</c> collection objects.</returns>
        /// \ingroup_disabled ace_ext_collections_highlight
        public static PropertyCollectionList buildPropertyCollectionList(this IEnumerable source, Boolean doOnlyWithDisplayName, Boolean doInherited, String filedName_prefix = "", PropertyCollectionList output = null, params String[] fieldsOrCategoriesToShow)
        {
            String[] filters = fieldsOrCategoriesToShow.getFlatArray<String>();

            if (output == null) output = new PropertyCollectionList();

            foreach (var item in source)
            {
                PropertyCollection itemProps = item.buildPropertyCollection<PropertyCollection>(doOnlyWithDisplayName, doInherited, filedName_prefix, null, filters);
                output.Add(itemProps);
            }

            return output;
        }

        /// <summary>
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="doInherited">if set to <c>true</c> [do inherited].</param>
        /// <param name="filedName_prefix">The filed name prefix.</param>
        /// <param name="output">The output.</param>
        /// <param name="fieldsOrCategoriesToShow">The fields or categories to show.</param>
        /// <returns></returns>
        public static PropertyCollectionExtended buildPCE(this Object source, Boolean doInherited, PropertyCollectionExtended output = null, params String[] fieldsOrCategoriesToShow)
        {
            List<String> filter = fieldsOrCategoriesToShow.getFlatList<String>();
            if (output == null) output = new PropertyCollectionExtended();

            var spo = new settingsEntriesForObject(source);

            // DataTable dt = source.buildDataTable("Properties", false, true, doInherited, null);

            return spo.pce;
        }

        /// <summary>
        /// Create or update property collection out of object properties
        /// </summary>
        /// <remarks>
        /// <para>Key is original property name - not displayName</para>
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="doOnlyWithDisplayName"></param>
        /// <param name="doInherited"></param>
        /// <param name="filedName_prefix"></param>
        /// <param name="output"></param>
        /// <param name="fieldsOrCategoriesToShow"></param>
        /// <returns></returns>
        /// \ingroup_disabled ace_ext_collections_highlight
        public static T buildPropertyCollection<T>(this Object source, Boolean doOnlyWithDisplayName, Boolean doInherited, String filedName_prefix = "", T output = null, params String[] fieldsOrCategoriesToShow) where T : PropertyCollection, new()
        {
            if (output == null) output = new T();

            if (source == null)
            {
                throw new ArgumentNullException("firstItem", "firstItem is null -- can't propertyCollection!");
                return output;
            }

            BindingFlags flags = BindingFlags.Instance;

            List<String> filters = fieldsOrCategoriesToShow.getFlatList<String>();
            //List<String> filters = fieldsOrCategoriesToShow.ToList();

            if (!doInherited)
            {
                flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
            }
            else
            {
                flags = BindingFlags.Public | BindingFlags.Instance;
            }

            Type itemType = source as Type;
            if (itemType == null) itemType = source.GetType();

            PropertyInfo[] propList = itemType.GetProperties(flags);

            foreach (PropertyInfo pi in propList)
            {
                // PropertyCollection extraData = new PropertyCollection();

                object[] atts = pi.GetCustomAttributes(true);

                List<String> tokenList = new List<string>();
                tokenList.Add(pi.Name);
                tokenList.Add(pi.Name.ToLower());
                Boolean pass = false;

                foreach (object att in atts)
                {
                    if (att is DisplayNameAttribute)
                    {
                        DisplayNameAttribute attDN = att as DisplayNameAttribute;
                        tokenList.Add(attDN.DisplayName);
                        tokenList.Add(attDN.DisplayName.ToLower());
                        //extraData[name_display] = attDN.DisplayName;
                    }

                    if (att is CategoryAttribute)
                    {
                        CategoryAttribute attCN = att as CategoryAttribute;
                        tokenList.Add(attCN.Category);
                        tokenList.Add(attCN.Category.ToLower());
                        //extraData[name_category] = attCN.Category;
                    }

                    if (att is DescriptionAttribute)
                    {
                        DescriptionAttribute attCN = att as DescriptionAttribute;
                        // extraData[name_description] = attCN.Description;
                    }
                }

                if (doOnlyWithDisplayName)
                {
                    pass = (tokenList.Count() > 2);
                }
                else
                {
                    pass = true;
                }

                if (pass)
                {
                    if (filters.Any())
                    {
                        pass = filters.Any(x => tokenList.Contains(x));
                    }
                }

                if (pass)
                {
                    if (pi.PropertyType.isSimpleInputEnough() || pi.PropertyType.isEnum())
                    {
                    }
                    else
                    {
                        Type IInterface = pi.PropertyType.GetInterface("IGetToSetFromString");
                        if (IInterface == null)
                        {
                            pass = false;
                        }
                        else
                        {
                            pass = true;
                        }
                    }
                }
                if (pass)
                {
                    output[filedName_prefix.add(pi.Name, "_")] = source.imbGetPropertySafe(pi);
                }
            }

            return output;
        }
    }
}