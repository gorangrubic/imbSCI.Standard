// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbTypologyPropertyGetSet.cs" company="imbVeles" >
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
    #region imbVeles using

    using System;
    using System.Collections.Generic;
    using System.Reflection;

    #endregion imbVeles using

    /// <summary>
    /// Skup ekstenzija za smestanje i preuzimanje podataka iz propertija
    /// </summary>
    public static class imbTypologyPropertyGetSet
    {
        /// <summary>
        /// Vraca vrednost statickog polja iz type-a (const, static property its)
        /// </summary>
        /// <param name="name">ime statickog polja</param>
        /// <returns></returns>
        public static Object ReadStaticField(this Type type, string name)
        {
            FieldInfo field = type.GetField(name, BindingFlags.Public | BindingFlags.Static);
            if (field == null)
            {
                return null;
            }
            return field.GetValue(null);
        }

        #region getSetCheckFlags enum

        /// <summary>
        /// Flagovi koji aktiviraju odredjena ponasanja kod checkGetSetPropertyCompatibility
        /// </summary>
        public enum getSetCheckFlags
        {
            /// <summary>
            /// Proverava da li newValue moze da se dodeli propertyInfo-u
            /// </summary>
            checkForSetting,

            /// <summary>
            /// Proverava da li tip propertyInfo-a moze da se dodeli objektu kao sto je newValue
            /// </summary>
            checkForGetting,

            /// <summary>
            /// Ignorisace write zastitu kod propertyInfoa
            /// </summary>
            ignoreWriteProtection,

            ///// <summary>
            ///// Ako je property statican javice fail
            ///// </summary>
            failOnStatic,

            /// <summary>
            /// Napravice exception i baciti ga
            /// </summary>
            throwException,

            /// <summary>
            /// Ako nije kompatibilno onda samo log
            /// </summary>
            justLog,

            /// <summary>
            /// Prosledice exception note logu
            /// </summary>
            noteException,

            /// <summary>
            /// Napravice exception i tiho ga zabeleziti
            /// </summary>
            noteScilent,

            /// <summary>
            /// Izvestavanje se iskljucuje
            /// </summary>
            reportingOff,

            /// <summary>
            /// Vratice Boolean
            /// </summary>
            returnBoolean,

            /// <summary>
            /// Vratice String izvestaj
            /// </summary>
            returnReport,
        }

        #endregion getSetCheckFlags enum

        /// <summary>
        /// Vraca sve vrednosti iz svih propertija koji su Public i Instance
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Dictionary<String, Object> imbGetAllValues(this Object target)
        {
            Dictionary<String, Object> output = new Dictionary<string, Object>();
            Dictionary<string, PropertyInfo> propdict = target.imbGetAllProperties();

            foreach (KeyValuePair<string, PropertyInfo> pair in propdict)
            {
                Object vlo = target.imbGetPropertySafe(pair.Value, null);
                if (vlo != null)
                {
                    output.Add(pair.Key, vlo);
                }
                else
                {
                    output.Add(pair.Key, pair.Value.PropertyType.getInstance());
                }
            }

            return output;
        }

        /// <summary>
        /// Vraca sve propertije iz objekta koji su Public i Instance. Vraca i nasledjene propertije ali ako su pregazeni onda vraca samo najnoviji
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Dictionary<String, PropertyInfo> imbGetAllProperties(this Object target)
        {
            Dictionary<String, PropertyInfo> output = new Dictionary<string, PropertyInfo>();
            PropertyInfo[] list = target.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (PropertyInfo pi in list)
            {
                if (output.ContainsKey(pi.Name))
                {
                    if (pi.DeclaringType == target.GetType())
                    {
                        output[pi.Name] = pi;
                    }
                }
                else
                {
                    output.Add(pi.Name, pi);
                }
            }

            return output;
        }

        /// <summary>
        /// Algoritam sa konverzijom koja podrzava i Collection objekta koji imaju Add method.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="p"></param>
        /// <param name="vl"></param>
        public static void imbSetPropertyConvertSafe(this Object target, PropertyInfo p, Object vl,
                                                     Object[] indexers = null, Boolean staticAccess = false)
        {
            Type vlTt = null;

            if (vl != null) vlTt = vl.GetType();

            if (!p.PropertyType.isCompatibileWith(vlTt))
            {
                MethodInfo minf = p.PropertyType.GetMethod("Add", BindingFlags.Public | BindingFlags.Instance);
                if (minf != null)
                {
                    Object collection = target.imbGetPropertySafe(p.Name, null);
                    if (collection == null) collection = Activator.CreateInstance(p.PropertyType);

                    vl = vl.imbConvertValueSafe(p.PropertyType.GetElementType());

                    //vl = imbTypologyExtensions.imbConvertValueSafe(vl, p.PropertyType.GetElementType());

                    minf.Invoke(collection, new Object[] { vl });
                    vl = collection;
                }
                else
                {
                    vl = vl.imbConvertValueSafe(p.PropertyType); //imbTypologyExtensions.imbConvertValueSafe(vl, p.PropertyType);
                }
            }
            else
            {
            }

            target.imbSetPropertySafe(p, vl, false, indexers, staticAccess);

            //p.SetValue(target, vl, null);
        }

        /// <summary>
        /// 2013> Sigurano postavljanje vrednosti objekta
        /// </summary>
        /// <param name="input"></param>
        /// <param name="propertyName"></param>
        /// <param name="newValue"></param>
        public static void imbSetPropertySafe(this Object input, String propertyName, Object newValue = null,
                                              Boolean OnlyIfReadWrite = false, Object[] indexers = null,
                                              Boolean staticAccess = false)
        {
            if (input == null && !staticAccess) return;
            PropertyInfo pi = null;
            pi = input.GetType().GetProperty(propertyName);
            imbSetPropertySafe(input, pi, newValue, OnlyIfReadWrite, indexers, staticAccess);
        }

        /// <summary>
        /// 2013> Sigurano postavljanje vrednosti objekta
        /// </summary>
        /// <param name="input"></param>
        /// <param name="propertyName"></param>
        /// <param name="newValue"></param>
        public static void imbSetPropertySafe(this Object input, PropertyInfo property, Object newValue,
                                              Boolean OnlyIfReadWrite = false, Object[] indexers = null,
                                              Boolean staticAccess = false)
        {
            if (input == null && !staticAccess) return;
            if (!property.CanWrite) return;
            try
            {
                input._SetPropertyValue(property, newValue, indexers, staticAccess);
            }
            catch (Exception ex)
            {
                throw new ArgumentOutOfRangeException(nameof(property), ex);
                //devNoteManager.note(input,
                //                    "Safe property SET fail :: " + property.toStringSafe() + "->" + input.toStringSafe() +
                //                    " :: Exception> " + logSystem.makeMessageFromException(ex), "imbSetPropertySafe", ex);
                //logSystem.log(, logType.ExecutionError);
            }
        }

        /*
        /// <summary>
        /// sigurno postavljanje vrednosti propertija
        /// </summary>
        /// <param name="input"></param>
        /// <param name="propertyInfo"></param>
        /// <param name="newValue"></param>
        public static void imbSetPropertySafe(this Object input, PropertyInfo propertyInfo, Object newValue,
                                              Boolean OnlyIfReadWrite = false, Object[] indexers = null,
                                              Boolean staticAccess = false)
        {
            Boolean _isStatic = propertyInfo.isStatic;

            if (staticAccess) _isStatic = true;

            if (input == null && !propertyInfo.isStatic) return;

            if (newValue == null)
            {
                if (propertyInfo.type.IsEnum)
                {
                    newValue = propertyInfo.type.GetDefaultValue();
                }
            }

            if (checkGetSetCompatibility(propertyInfo, newValue, getSetCheckFlags.checkForSetting))
            {
                imbTypologyExtensions._SetPropertyValue(input, propertyInfo.propertyInfoSource, newValue, indexers,
                                                        _isStatic);
            }

            /*
            try
            {
                if (propertyInfo.isReadWrite || (!OnlyIfReadWrite))
                {
                    if (propertyInfo.propertyInfoSource.CanWrite)
                    {
                        if (newValue != null)
                        {
                            if (newValue.GetType() != propertyInfo.propertyTypeInfo.type)
                            {
                                if (!newValue.GetType().IsSubclassOf(propertyInfo.propertyTypeInfo.type))
                                {
                                    //String shortNote = "Type mismatch: " + propertyInfo.propertyTypeInfo.type + " <== " +
                                    //                   input.GetType().Name + "";

                                    //logSystem.log();
                                }
                            }
                        }

                        //.SetValue(input, newValue, null);
                    }
                }
            }
            catch (Exception ex)
            {
                //String msg = newValue.ToString()

                logSystem.log("Safe property SET fail :: " + propertyInfo.name + "->" + input.GetType().Name + " :: Exception> " + logSystem.makeMessageFromException(ex), logType.ExecutionError);
            } */
        //    }

        /// <summary>
        /// Sigurno iscitavanje stringa
        /// </summary>
        /// <param name="input"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultOutput"></param>
        /// <returns></returns>
        public static String imbPropertyToString(this Object input, String propertyName, String defaultOutput = "null")
        {
            String output = defaultOutput;
            Object val = input.imbGetPropertySafe(propertyName);
            if (val != null)
            {
                output = val.ToString();
            }
            return output;
        }

        /// <summary>
        /// 2013> Sigurno preuzimanje vrednosti objekta - podržava path:
        /// </summary>
        /// <param name="input"></param>
        /// <param name="propertyName">Predstavlja putanju ka proportiju - podrzava .</param>
        /// <param name="defaultOutput"></param>
        /// <returns></returns>
        public static Object imbGetPropertySafe(this Object input, String propertyName, Object defaultOutput = null,
                                                String spliter = ".")
        {
            if (input == null) return defaultOutput;
            if (String.IsNullOrEmpty(propertyName)) return defaultOutput;
            Object output = defaultOutput; // = new Object();
            try
            {
                Type inputType = input.GetType();

                PropertyInfo pi = inputType.GetProperty(propertyName);

                //imbPropertyContext pi = imbTypology.getPropertyContext(input, propertyName, spliter);
                if (pi != null)
                {
                    return input.imbGetPropertySafe(pi, defaultOutput);
                    //return pi.propertyValue;
                }
            }
            catch (Exception ex)
            {
                //logSystem.log(
                //    "Safe property GET fail :: " + propertyName + "->" + input.GetType().Name + " :: Exception> " +
                //    logSystem.makeMessageFromException(ex), logType.ExecutionError);
            }

            return output;
        }

        public static Object imbGetPropertySafe(this Object input, PropertyInfo property, Object defaultOutput,
                                                Boolean OnlyIfReadWrite, Object[] indexers,
                                                Boolean staticAccess)
        {
            if (input == null && !staticAccess) return defaultOutput;

            try
            {
                if (property.CanRead || (!OnlyIfReadWrite))
                {
                    return property._GetPropertyValue(input, indexers, staticAccess);
                    //    //property.GetValue(input, null);
                }
            }
            catch (Exception ex)
            {
                //logSystem.log(
                //    "Safe property GET fail :: " + property.Name + "->" + input.GetType().Name + " :: Exception> " +
                //    logSystem.makeMessageFromException(ex), logType.ExecutionError);
            }

            return defaultOutput;
        }

        /// <summary>
        /// 2013> Sigurno preuzimanje vrednosti objekta - podržava path:
        /// </summary>
        /// <param name="input"></param>
        /// <param name="propertyName">Predstavlja putanju ka proportiju - podrzava .</param>
        /// <param name="defaultOutput"></param>
        /// <returns></returns>
        public static Object imbGetPropertySafe(this Object input, PropertyInfo property, Object defaultOutput = null,
                                                Boolean OnlyIfReadWrite = false, Object[] indexers = null)
        {
            if ((input == null) && !property.isStatic()) return defaultOutput;

            if (property == null)
            {
                // logSystem.log("PropertyInfo is null - GET failed", logType.ExecutionError);
                return defaultOutput;
            }

            try
            {
                if (property.isReadWrite() || (!OnlyIfReadWrite))
                {
                    return property._GetPropertyValue(input, indexers, property.isStatic());

                    //   return imbTypologyExtensions._GetPropertyValue(property.propertyInfoSource, input, indexers,
                    //                                        property.isStatic);
                    // return property.propertyInfoSource.GetValue(input, null);
                }
            }
            catch (Exception ex)
            {
                //imbStringBuilder isb = new imbStringBuilder(0);
                //isb.AppendLine("GET fail :: " + property.parent.typeName + "." + property.propertyRealName + ":",
                //               htmlTagName.p);
                //isb.AppendPairs(property, "Target property", "propertyRealName", "name", "typeName", "isIndexer",
                //                "isReadWrite", "typeGroup");
                //isb.AppendPair("Host object", input.toStringSafe());

                //isb.AppendPair("Exception message:", ex.Message);

                ////String cls = input.GetType().Name + ".imbGetPropertySafe(" + property.propertyRealName + ")";
                //Exception exd = new aceGeneralException(isb.ToString(), ex);
                //devNoteManager.note(input, exd, devNoteType.typology, isb.ToString());

                //logSystem.log(msg + " class:" + cls, logType.ExecutionError);
            }

            return defaultOutput;
        }

        /// <summary>
        /// 2014:Maj - uzima property i odmah vrsi bezbednu konverziju!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static T imbGetPropertySafe<T>(this Object input, PropertyInfo property)
        {
            Object val = input.imbGetPropertySafe(property, null, false);
            return val.imbConvertValueSafeTyped<T>();
        }
    }
}