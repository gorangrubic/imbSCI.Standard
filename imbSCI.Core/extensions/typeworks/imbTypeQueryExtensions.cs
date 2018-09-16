// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbTypeQueryExtensions.cs" company="imbVeles" >
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
    using imbSCI.Core.data;
    using imbSCI.Core.enums;
    using imbSCI.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Collection of extension methods for Type object: is...() tests and getProperty by path
    /// </summary>
    public static class imbTypeQueryExtensions
    {
        public static settingsPropertyEntryWithContext getSPEC(this Object input, String propertyPath)
        {
            //   PropertyInfo pi = input.getProperty(propertyPath);

            List<string> propPath = propertyPath.SplitSmart("."); //.getPropertyPathElements(".");
            Type ti = input.GetType(); //.getTypology(); // pt = input.GetType();
            PropertyInfo c = null;
            PropertyInfo pi = null;
            Object parent = input;
            foreach (String pe in propPath)
            {
                pi = ti.GetProperty(pe);
                if (pi != null)
                {
                    ti = pi.PropertyType;

                    if (propPath.Last() != pe)
                    {
                        parent = parent.imbGetPropertySafe(pi.Name); //parent.GetPropertyValue(pi.Name);
                    }
                }
                c = pi;

                //c.property = ti.getPropertyInfo(pe);
                //c.parentInstance = c.propertyValue;
                //ti = c.property.type.getTypology();
            }

            settingsPropertyEntryWithContext sPEWC = new settingsPropertyEntryWithContext(pi, parent);
            return sPEWC;
        }

        /// <summary>
        /// Retrieves PropertyInfo found on path specified
        /// </summary>
        /// <param name="input"></param>
        /// <param name="propertyPath"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static PropertyInfo getProperty(this Object input, String propertyPath, String spliter = ".")
        {
            // imbPropertyContext c = new imbPropertyContext();
            //if (String.IsNullOrEmpty(propertyPath)) return new imbPropertyContext(input, )

            List<string> propPath = propertyPath.SplitSmart(spliter); //.getPropertyPathElements(spliter);
            Type ti = input.GetType(); //.getTypology(); // pt = input.GetType();
            PropertyInfo c = null;
            foreach (String pe in propPath)
            {
                var pi = ti.GetProperty(pe);
                if (pi != null)
                {
                    ti = pi.PropertyType;
                }
                c = pi;

                //c.property = ti.getPropertyInfo(pe);
                //c.parentInstance = c.propertyValue;
                //ti = c.property.type.getTypology();
            }
            return c;
        }

        /// <summary>
        /// Determines whether the <see cref="MemberInfo"/> <c>mi</c> is <see cref="Type.IsAssignableFrom(Type)"/> the specified <c>targetType</c>
        /// </summary>
        /// <param name="mi">The mi.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns>
        ///   <c>true</c> if [is compatibile with] [the specified target type]; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean isCompatibileWith(this MemberInfo mi, Type targetType)
        {
            Type type = mi.getRelevantType();
            return type.IsAssignableFrom(targetType);
        }

        /// <summary>
        /// Gets the <see cref="Type"/> that is relevant for specified <see cref="MemberInfo"/>. If the <c>mi</c> is Type its ok too
        /// </summary>
        /// <param name="mi">The mi.</param>
        /// <returns></returns>
        public static Type getRelevantType(this MemberInfo mi)
        {
            Type type = null;
            if (mi is Type)
            {
                type = mi as Type;
            }
            if (mi is PropertyInfo)
            {
                PropertyInfo pi = mi as PropertyInfo;

                type = pi.PropertyType;
            }

            return type;
        }

        /// <summary>
        /// Determines whether the specified type is nullable.
        /// </summary>
        /// <param name="ti">Type to test.</param>
        /// <returns>
        ///   <c>true</c> if the specified ti is nullable; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean isNullable(this Type ti)
        {
            if (Nullable.GetUnderlyingType(ti) != null)
            {
                // It's nullable
                return true;
            }
            return false;
        }

        public static Boolean isReadWrite(this PropertyInfo pi)
        {
            var mt = pi.GetSetMethod();
            return mt != null;
        }

        public static Boolean isStatic(this MemberInfo mi)
        {
            Type type = mi.getRelevantType();
            return (type.IsAbstract && type.IsSealed);
        }

        /// <summary>
        /// Returns imbTypeGroup for supplied type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static imbTypeGroup getTypeGroup(this Type type)
        {
            if (type.isEnum()) return imbTypeGroup.enumeration;
            if (type.isBoolean()) return imbTypeGroup.boolean;
            if (type.isText()) return imbTypeGroup.text;
            if (type.isNumber()) return imbTypeGroup.number;
            if (type.IsClass) return imbTypeGroup.instance;
            return imbTypeGroup.unknown;
        }

        /// <summary>
        /// Da li je tip Broj
        /// </summary>
        public static Boolean isNumber(this Type type)
        {
            if (type == typeof(int)) return true;
            if (type == typeof(Int32)) return true;
            if (type == typeof(uint)) return true;
            if (type == typeof(long)) return true;

            if (type == typeof(Int16)) return true;

            if (type == typeof(Int64)) return true;
            if (type == typeof(UInt16)) return true;
            if (type == typeof(UInt32)) return true;
            if (type == typeof(UInt64)) return true;
            if (type == typeof(Double)) return true;
            if (type == typeof(float)) return true;
            if (type == typeof(Single)) return true;
            if (type == typeof(Decimal)) return true;
            if (type == typeof(decimal)) return true;
            if (type == typeof(ulong)) return true;
            if (type == typeof(short)) return true;
            if (type == typeof(ushort)) return true;
            if (type == typeof(byte)) return true;
            if (type == typeof(sbyte)) return true;
            return false;
        }

        /// <summary>
        /// Da li je tip tekst ili char
        /// </summary>
        public static Boolean isText(this Type type)
        {
            if (type == typeof(string)) return true;
            if (type == typeof(String)) return true;
            if (type == typeof(char)) return true;
            if (type == typeof(Char)) return true;
            return false;
        }

        /// <summary>
        /// Returns TRUE if the type is Boolean or bool
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>TRUE if it is boolean</returns>
        public static Boolean isBoolean(this Type type)
        {
            if (type == typeof(Boolean)) return true;
            if (type == typeof(bool)) return true;
            return false;
        }

        /// <summary>
        /// Ovo se koristi u smislu> da li je dozvoljeno slobodno upisivanje nove vrednosti kroz InputLine
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Boolean isTextOrNumber(this Type type)
        {
            if (type.isText()) return true;
            if (type.isNumber()) return true;
            return false;
        }

        /// <summary>
        /// Returns TRUE if the type is simple enough to be edited with simple text input field
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>TRUE if its editable by text or FALSE if the type is not that simple</returns>
        public static Boolean isSimpleInputEnough(this Type type)
        {
            if (type.isText()) return true;
            if (type.isNumber()) return true;
            if (type.isCollection()) return false;
            if (type.isToggleValue()) return false;
            return false;
        }

        /// <summary>
        /// Returns TRUE if the type has IEnumerable interface implemented
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>TRUE if it is array, collection, list or any other IEnumerable type</returns>
        public static Boolean isCollection(this Type type)
        {
            if (type.isText()) return false;

            return type.GetInterface("IEnumerable") != null;
        }

        /// <summary>
        /// Returns TRUE if type is: enum, boolean or implements ILimitedValueRange interface
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>TRUE if the type is a kind of toggle value</returns>
        public static Boolean isToggleValue(this Type type)
        {
            if (type.IsEnum) return true;
            if (type.isBoolean()) return true;
            return type.GetInterface("ILimitedValueRange") != null;
            //if (type.isCompatibileWith(typeof(ILimitedValueRange))) return true;
        }

        /// <summary>
        /// Returns TRUE if the type is DBNull
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>TRUE if it is DBNull, FALSE in other case</returns>
        public static Boolean isNull(this Type type)
        {
            if (type == typeof(DBNull)) return true;

            return false;
        }

        /// <summary>
        /// Returns TRUE if type is enum. Basicaly it just wraps Type.IsEnum value.
        /// </summary>
        /// <param name="type">Type to test</param>
        /// <returns>The value of Type.IsEnum</returns>
        public static Boolean isEnum(this Type type)
        {
            return type.IsEnum;
        }
    }
}