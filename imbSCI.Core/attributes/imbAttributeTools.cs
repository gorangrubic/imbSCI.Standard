// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbAttributeTools.cs" company="imbVeles" >
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
namespace imbSCI.Core.attributes
{
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;

    #region imbVeles using

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    #endregion imbVeles using

    /// <summary>
    /// Skup alata za rad sa atributima> selektovanje, primena, itd. Kako imbAtributima tako i sa opstim atributima
    /// </summary>
    public static class imbAttributeTools
    {
        public static imbHelpContent getHelpContent(this imbAttributeCollection attribs)
        {
            imbHelpContent output = new imbHelpContent();

            foreach (Object k in attribs.Keys)
            {
                if (k is imbAttributeName)
                {
                    imbAttribute att = attribs[k];

                    switch (att.nameEnum)
                    {
                        case imbAttributeName.menuCommandTitle:
                        case imbAttributeName.helpTitle:
                            output.title = att.getMessage().toStringSafe();
                            break;

                        case imbAttributeName.helpTips:
                            output.hints = att.msg.toStringSafe();
                            break;

                        case imbAttributeName.helpPurpose:
                            output.purpose = att.msg.toStringSafe();
                            break;

                        case imbAttributeName.menuHelp:
                        case imbAttributeName.helpDescription:
                            output.description = att.msg.toStringSafe();
                            break;
                    }
                }
            }

            return output;
        }

        public static Boolean logNotificationsHere = false;

        public static imbAttributeCollection getImbAttributeDictionary(this MemberInfo input)
        {
            return getImbAttributeDictionary(input, false, false, false);
        }

        /// <summary>
        /// GLAVNI attribute dictionary konstruktor - koristiti ga sto vise
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static imbAttributeCollection getImbAttributeDictionary(this MemberInfo input, Boolean allowOverWrite,
                                                                       Boolean includeInheritedForType = false,
                                                                       Boolean forceNewCollection = false)
        {
            imbAttributeCollection output = new imbAttributeCollection();
            try
            {
                if (imbAttributeProfileEngine.attributeCollectionByMemberInfo.ContainsKey(input) && !forceNewCollection)
                {
                    //if (logNotificationsHere)
                    //    logSystem.log("Attribute collection for :: " + input.Name + " found in cache",
                    //                  logType.Notification);
                    return imbAttributeProfileEngine.attributeCollectionByMemberInfo[input];
                }

                if (input is Type)
                {
                    Type inputType = input as Type;
                    List<Type> bl = inputType.GetBaseTypeList(true, true);
                    bl.Reverse();
                    foreach (Type b in bl)
                    {
                        List<imbAttribute> tl = b.getAttributes<imbAttribute>(includeInheritedForType);
                        foreach (imbAttribute tli in tl)
                        {
                            if (!output.ContainsKey(tli.nameEnum) || allowOverWrite)
                            {
                                output.Add(tli.nameEnum, tli);
                                if (b == inputType)
                                {
                                    output.selfDeclaredAttributes.Add(tli.nameEnum, tli);
                                }
                            }
                        }
                    }
                }
                else
                {
                    List<imbAttribute> attributes = input.getAttributes<imbAttribute>(true);
                    foreach (imbAttribute at in attributes)
                    {
                        if (!output.ContainsKey(at.nameEnum) || allowOverWrite)
                        {
                            output.Add(at.nameEnum, at);
                        }
                    }
                }
                if (!forceNewCollection) imbAttributeProfileEngine.attributeCollectionByMemberInfo.Add(input, output);
            }
            catch (Exception ex)
            {
                //devNoteManager.note("GRESNA U STVARANU ATRIBUTA", ex, devNoteType.addAttributes);
            }
            return output;
        }

        /// <summary>
        /// Vraca ili kreira novu Attribute mapu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static imbAttributeToPropertyMap getImbAttributeMap(this Type input)
        {
            imbAttributeToPropertyMap output = null;

            if (imbAttributeProfileEngine.attributeToPropertyMapsByType.ContainsKey(input))
            {
                if (logNotificationsHere)
                    // logSystem.log("Attribute map for :: " + input.Name + " found in cache", logType.Notification);
                    return imbAttributeProfileEngine.attributeToPropertyMapsByType[input];
            }
            output = new imbAttributeToPropertyMap(input);
            if (!imbAttributeProfileEngine.attributeToPropertyMapsByType.ContainsKey(input))
            {
                imbAttributeProfileEngine.attributeToPropertyMapsByType.Add(input, output);
            }

            return output;
        }

        /// <summary>
        /// Prebacuje vrednost iz atributa u propertije - u skladu sa podesenim mapiranjem (imbAttributeName.metaValueFromAttribute)
        /// </summary>
        /// <param name="target">Objekat ciji se propertiji podesavaju</param>
        /// <param name="attribs">Atributi iz kojih izvlaci vrednost</param>
        /// <param name="onlyDeclared">Da li samo propertije target klase ili i nasledjene propertije</param>
        /// <param name="avoidOverWrite">Da li da preskoci ako Property vec ima vrednost</param>
        public static void imbAttributeToProperties(this Object target, imbAttributeCollection attribs,
                                                    Boolean onlyDeclared = false, Boolean avoidOverWrite = false)
        {
            if (target == null)
            {
                //  logSystem.log("Target object not supplied", logType.FatalError);
                return;
            }

            if (attribs == null)
            {
                //logSystem.log("Attributes to write values not supplied", logType.FatalError);
                return;
            }

            imbAttributeToPropertyMap map = getImbAttributeMap(target.GetType());
            map.writeToObject(target, attribs, onlyDeclared, avoidOverWrite);
        }

        //if (target == null) return;

        //if (target.GetType().Name == "linkCollectionSeed")
        //{
        //}

        //if (attribs == null)
        //{
        //    logSystem.log("Attributes collection is null!", logType.FatalError);
        //}

        //Type targetType = target.GetType();

        //PropertyInfo[] pi = new PropertyInfo[] { };

        //if (onlyDeclared)
        //{
        //    pi = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        //}
        //else
        //{
        //    pi = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //}

        //foreach (PropertyInfo p in pi)
        //{
        //    Boolean go = true;
        //    if (avoidOverWrite)
        //    {
        //        if (p.GetValue(target, null) != null)
        //        {
        //            go = false;
        //        }
        //    }

        //    if (go)
        //    {
        //        imbAttribute setAt = p.getImbAttribute(imbAttributeName.metaValueFromAttribute);
        //        if (setAt != null)
        //        {
        //            imbAttributeName attn = (imbAttributeName)setAt.objMsg;
        //            if (attribs.ContainsKey(attn))
        //            {
        //                imbAttribute valAt = attribs[attn];

        //                Object vl = valAt.msg;
        //                if (valAt.objMsg != null) vl = valAt.objMsg;
        //                target.imbSetPropertyConvertSafe(p, vl);
        //            }

        //            //foreach (imbAttribute valAt in attribs)
        //            //{
        //            //    if (valAt.nameEnum == (imbAttributeName) setAt.objMsg)
        //            //    {
        //            //        //logSystem.log("Write to:" + p.Name + " value from: " + setAt.msg, logType.Notification);
        //            //        Object vl = valAt.msg;
        //            //        if (valAt.objMsg != null) vl = valAt.objMsg;
        //            //        target.imbSetPropertyConvertSafe(p, vl);

        //            //    }
        //            //}
        //        }
        //    }
        //}

        /// <summary>
        /// Vraca prvi atribut po redu - datog tipa
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="includeInherited"></param>
        /// <returns></returns>
        public static T getAttribute<T>(this MemberInfo input, Boolean includeInherited = false) where T : Attribute
        {
            List<T> list = getAttributes<T>(input, includeInherited);
            if (list.Count > 0)
            {
                return list.First();
            }
            else
            {
                return null;
            }
        }

        public static String getAttributeValueOrDefault<T>(this MemberInfo input, String defaultValue, Boolean includeInherited = false) where T : Attribute
        {
            T attribute = input.getAttribute<T>(includeInherited);
            if (attribute == null) return defaultValue;

            if (attribute is DescriptionAttribute)
            {
                DescriptionAttribute da = attribute as DescriptionAttribute;
                return da.Description;
            }

            if (attribute is DisplayNameAttribute)
            {
                DisplayNameAttribute attribute_DisplayNameAttribute = attribute as DisplayNameAttribute;
                return attribute_DisplayNameAttribute.DisplayName;
            }

            if (attribute is CategoryAttribute)
            {
                CategoryAttribute attribute_CategoryAttribute = attribute as CategoryAttribute;
                return attribute_CategoryAttribute.Category;
            }
            return defaultValue;
        }

        /// <summary>
        /// Vraća sve atribute u tipu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="includeInherited"></param>
        /// <returns></returns>
        public static List<T> getAttributes<T>(this PropertyInfo input, Boolean includeInherited = false)
            where T : Attribute
        {
            List<T> output = new List<T>();
            Type attType = typeof(T);

            object[] attObj = input.GetCustomAttributes(includeInherited);
            return getList<T>(attObj);
        }

        internal static List<T> getList<T>(Object[] input) where T : Attribute
        {
            List<T> output = new List<T>();
            foreach (object atObj in input)
            {
                T tmpObj = atObj as T;
                if (tmpObj != null)
                {
                    output.Add(tmpObj);
                }
                else
                {
                }
            }
            return output;
        }

        /// <returns></returns>
        public static List<T> getAttributes<T>(this MemberInfo input, Boolean includeInherited = false)
            where T : Attribute
        {
            List<T> output = new List<T>();
            Type attType = typeof(T);

            object[] attObj = input.GetCustomAttributes(includeInherited);
            return getList<T>(attObj);
        }

        ///// <summary>
        ///// Vraća sve atribute u tipu
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="input"></param>
        ///// <param name="includeInherited"></param>
        ///// <returns></returns>
        //public static List<T> getAttributes<T>(this Type input, Boolean includeInherited = false) where T : Attribute
        //{
        //    List<T> output = new List<T>();
        //    Type attType = typeof (T);

        //    object[] attObj = input.GetCustomAttributes(includeInherited);
        //    return getList<T>(attObj);
        //}

        ///// <summary>
        ///// Vraća imb atribut koji ima zadato ime, ako ga nema onda vraca null
        ///// </summary>
        ///// <param name="input"></param>
        ///// <param name="name"></param>
        ///// <param name="includeInherited"></param>
        ///// <returns></returns>
        //public static imbAttribute getImbAttribute(this PropertyInfo input,
        //                                           imbAttributeName name = imbAttributeName.undefined,
        //                                           Boolean includeInherited = false)
        //{
        //    List<imbAttribute> listSource = input.getAttributes<imbAttribute>(includeInherited);
        //    return filterList(listSource, name, includeInherited);
        //}

        ///// <summary>
        ///// Vraća imb atribut koji ima zadato ime, ako ga nema onda vraca null
        ///// </summary>
        ///// <param name="input"></param>
        ///// <param name="name"></param>
        ///// <param name="includeInherited"></param>
        ///// <returns></returns>
        //public static imbAttribute getImbAttribute(this MemberInfo input,
        //                                           imbAttributeName name = imbAttributeName.undefined,
        //                                           Boolean includeInherited = false)
        //{
        //    List<imbAttribute> listSource = input.getAttributes<imbAttribute>(includeInherited);
        //    return filterList(listSource, name, includeInherited);
        //}

        ///// <summary>
        ///// Vraća atribute koji imaju imena navedena u names polju
        ///// </summary>
        ///// <param name="input"></param>
        ///// <param name="names"></param>
        ///// <param name="includeInherited"></param>
        ///// <returns></returns>
        //public static List<imbAttribute> getImbAttributes(this MemberInfo input,
        //                                   imbAttributeName[] names,
        //                                   Boolean includeInherited = false)
        //{
        //    //List<imbAttribute> listSource = input.getAttributes<imbAttribute>(includeInherited);
        //    //return filterList(listSource, names, includeInherited);
        //}

        ///// <summary>
        ///// Vraća atribute koji imaju imena navedena u names polju
        ///// </summary>
        ///// <param name="input"></param>
        ///// <param name="names"></param>
        ///// <param name="includeInherited"></param>
        ///// <returns></returns>
        //public static List<imbAttribute> getImbAttributes(this Type input,
        //                                   imbAttributeName[] names,
        //                                   Boolean includeInherited = false)
        //{
        //    List<imbAttribute> listSource = input.getAttributes<imbAttribute>(includeInherited);
        //    return filterList(listSource, names, includeInherited);
        //}

        public static String getImbAttributeMessage(this MemberInfo input,
                                                    imbAttributeName name = imbAttributeName.undefined,
                                                    String defaultResponse = "",
                                                    Boolean includeInherited = false)
        {
            return input.getImbAttributeDictionary().getMessage(name, defaultResponse);
        }

        /// <summary>
        /// Vraća imb atribut koji ima zadato ime, ako ga nema onda vraca null
        /// </summary>
        /// <param name="input"></param>
        /// <param name="name"></param>
        /// <param name="includeInherited"></param>
        /// <returns></returns>
        public static imbAttribute getImbAttribute(this MemberInfo input,
                                                   imbAttributeName name = imbAttributeName.undefined,
                                                   Boolean includeInherited = false)
        {
            return input.getImbAttributeDictionary().getAttribute(name);
        }

        public static Boolean hasAttribute(this MemberInfo input, imbAttributeName name = imbAttributeName.undefined)
        {
            return input.getImbAttributeDictionary().ContainsKey(name);
        }

        //private static imbAttribute filterList(List<imbAttribute> input, imbAttributeName name, Boolean includeInherited = false)
        //{
        //    String toFind = name.ToString();
        //    imbAttribute output = null;

        //    if (input.Count > 0)
        //    {
        //        foreach (imbAttribute at in input)
        //        {
        //            if (at.name == toFind)
        //            {
        //                output = at;
        //                break;
        //            }
        //        }
        //    }

        //    return output;
        //}

        //private static List<imbAttribute> filterList(List<imbAttribute> input, imbAttributeName[] names, Boolean includeInherited=false)
        //{
        //   // String toFind = name.ToString();

        //    List<string> nameList = new List<string>();
        //    foreach (imbAttributeName nm in names)
        //    {
        //        nameList.Add(nm.ToString());
        //    }

        //    //imbAttribute output = null;

        //    List<imbAttribute> output = new List<imbAttribute>();

        //    if (input.Count > 0)
        //    {
        //        foreach (imbAttribute at in input)
        //        {
        //            if (nameList.Contains(at.name))
        //            {
        //                output.Add(at);
        //            }
        //        }
        //    }

        //    return output;
        //}

        /// <summary>
        /// V3.1: Vraca Attribut koji je trazen.
        /// Ako nepostoji, onda vraca null
        /// </summary>
        /// <typeparam name="T">Tip attributa koji treba pronaci</typeparam>
        /// <param name="pi"></param>
        /// <returns></returns>
        public static T imbPropertyAttribute<T>(this PropertyInfo pi) where T : Attribute
        {
            T output = null;
            Object[] attArr = pi.GetCustomAttributes(typeof(T), false);
            if (attArr.Length > 0)
            {
                output = attArr[0] as T;
            }
            return output;
        }
    }
}