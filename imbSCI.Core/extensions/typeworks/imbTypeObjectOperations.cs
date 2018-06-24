// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbTypeObjectOperations.cs" company="imbVeles" >
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
namespace imbSCI.Core.extensions.typeworks
{
    using imbSCI.Core.interfaces;
    using imbSCI.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class imbTypeObjectOperations
    {
        /// <summary>
        /// Sets the object by source, but only ValueTypes and string and only properties declared at top inherence level. Returns dictionary with updated properties (only if value changed)
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        /// <param name="loger">The loger.</param>
        /// <returns></returns>
        public static Dictionary<PropertyInfo, Object> setObjectValueTypesBySource(this Object target, Object source, IAceLogable loger = null)
        {
            Dictionary<PropertyInfo, Object> output = new Dictionary<PropertyInfo, object>();

            Int32 c = 0;
            var vals = target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach (PropertyInfo pi in vals)
            {
                if (pi.PropertyType.IsValueType || pi.PropertyType == typeof(String))
                {
                    Object cur = target.imbGetPropertySafe(pi, null);
                    Object tmp = source.imbGetPropertySafe(pi, cur);
                    if (!tmp.isNullOrEmptyString())
                    {
                        target.imbSetPropertySafe(pi, tmp, true);
                    }

                    if (cur != tmp)
                    {
                        output.Add(pi, tmp);
                        if (loger != null) loger.log("Property [" + pi.Name + "] changed to: " + tmp.ToString());
                    }
                }
            }

            return output;
        }

        #region OBJECT COMPLETE OPERATIONS

        /// <summary>
        /// Sets the object by source.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        /// <param name="ignoreList">List of properties to ignore.</param>
        /// <returns>number of values updated at target object</returns>
        public static Int32 setObjectBySource(this Object target, Object source, params String[] ignoreList)
        {
            List<String> ignore = ignoreList.ToList(); //.getFlatList<String>();
            Int32 c = 0;
            var vals = target.imbGetAllProperties();
            foreach (KeyValuePair<String, PropertyInfo> pair in vals)
            {
                if (!ignore.Contains(pair.Key))
                {
                    Object tmp = source.imbGetPropertySafe(pair.Key, null);
                    if (!imbSciStringExtensions.isNullOrEmptyString(tmp))
                    {
                        c++;
                        target.imbSetPropertySafe(pair.Value, tmp);
                    }
                }
            }

            return c;
        }

        /// <summary>
        /// CORE TECH> pristup propertiju koji treba svi metodi da koriste
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static Object _GetPropertyValue(this PropertyInfo pi, Object source, Object[] indexers = null,
                                                 Boolean staticAccess = false)
        {
            Object output = null;
            if (source == null && !staticAccess) return null;
            ParameterInfo[] indp = pi.GetIndexParameters();
            if (source.GetType() != pi.DeclaringType)
            {
            }
            if (indp.Count() == 0)
            {
                output = pi.GetValue(source, null);
            }
            else
            {
                if (indexers == null)
                {
                    output = null;
                    //source.note(devNoteType.typology,
                    //            "Indexers not passed for indexed property: " + pi.Name + " - prevent this call!",
                    //            devNoteBehaviour.openDevNote, true);
                }
                else
                {
                    output = pi.GetValue(source, indexers);
                }
            }

            return output;
        }

        /// <summary>
        /// CORE TECH> postavljanje propertija koji treba svi metodi da koriste
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pi"></param>
        /// <param name="newValue"></param>
        /// <param name="indexers"></param>
        /// <returns></returns>
        internal static Boolean _SetPropertyValue(this Object source, PropertyInfo pi, Object newValue,
                                                  Object[] indexers = null, Boolean staticAccess = false)
        {
            Boolean output = false;
            if (source == null && !staticAccess) return false;

            ParameterInfo[] indp = pi.GetIndexParameters();

            if (indp.Count() == 0)
            {
                pi.SetValue(source, newValue, null);
                output = true;
            }
            else
            {
                if (indexers == null)
                {
                    output = false;
                }
                else
                {
                    pi.SetValue(source, newValue, indexers);
                    output = true;
                }
            }

            return output;
        }

        #endregion OBJECT COMPLETE OPERATIONS
    }
}