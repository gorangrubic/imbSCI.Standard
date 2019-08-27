// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbSciEnumExtensions.cs" company="imbVeles" >
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
// Project: imbSCI.Data
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace imbSCI.Data
{
    using System.Collections;

    /// <summary>
    /// Enum type extensions
    /// </summary>
    public static class imbSciEnumExtensions
    {
        /// <summary>
        /// Gets the enum values list from the same enum flags property
        /// </summary>
        /// <typeparam name="T">Enum type to test text against</typeparam>
        /// <param name="text">The text to test</param>
        /// <returns>List of detected enums</returns>
        /// <exception cref="ArgumentException">Type must be Enum! T = " + enumType.Name</exception>
        public static List<T> getEnumsDetectedInString<T>(this String text)
        {
            List<T> output = new List<T>();
            if (text.isNullOrEmptyString()) return output;
            Type enumType = typeof(T);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type must be Enum! T = " + enumType.Name);
            }

            foreach (T en in Enum.GetValues(enumType))
            {
                if (text.Contains(en.ToString()))
                {
                    output.Add(en);
                }
            }
            return output;
        }

        public static List<T> EnumToType<T>(this IEnumerable<Enum> enums)
        {
            List<T> output = new List<T>();
            foreach (Enum e in enums)
            {
                T value = (T)Enum.ToObject(typeof(T), Convert.ToInt32(e));
            }
            return output;
        }

        public static List<Enum> TypeToEnum<T>(this IEnumerable<T> enums)
        {
            List<Enum> output = new List<Enum>();
            var dict = getEnumDictionary<T>();
            foreach (T e in enums)
            {
                output.Add(dict[e.ToString()]);
            }
            return output;
        }

        // public static Dictionary<T, Enum> getEnumDictionary<T>()
        //{
        //    Dictionary<T, Enum> output = new Dictionary<String, Enum>();

        //    foreach (Enum en in Enum.GetValues(typeof(T)))
        //    {
        //        output.Add(en.ToString(),en);
        //    }

        //    return output;

        //}

        public static Dictionary<String, Enum> getEnumDictionary<T>()
        {
            Dictionary<String, Enum> output = new Dictionary<String, Enum>();

            foreach (Enum en in Enum.GetValues(typeof(T)))
            {
                output.Add(en.ToString(),en);
            }

            return output;

        }


        public static List<T> getEnumList<T>()
        {
            List<T> output = new List<T>();

            foreach (T en in Enum.GetValues(typeof(T)))
            {
                output.Add(en);
            }

            return output;

        }
        
        
        /// <summary>
        /// Gets base flag enumeration values 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        public static List<T> getBaseFlags<T>()
        {
            List<T> output = new List<T>();
            List<T> allEnums = getEnumList<T>();

            foreach (Enum en in Enum.GetValues(typeof(T)))
            {
                List<T> contains = getEnumListFromFlags<T>((Enum)en);
                if (contains.Count == 1)
                {
                    output.AddRange(contains);
                }
            }
            return output;
        }

        /// <summary>
        /// Gets the enum values list from the same enum flags property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        public static List<T> getEnumListFromFlags<T>(this Enum flags)
        {
            List<T> output = new List<T>();

            foreach (T en in Enum.GetValues(flags.GetType()))
            {
                Object e = en;
                //Enum e = (Int32)en;

                if (0 != (Int32)e)
                {
                    if (flags.HasFlag(en as Enum))
                    {
                        output.Add(en);
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Untyped variation of <see cref="getEnumListFromFlags{T}(Enum)"/>
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        public static IList getEnumListFromFlags(this Enum flags)
        {
            IList output = new List<Object>();
            //var values = Enum.GetValues(flags.GetType());
            //for (int i = 0; i < values.Length; i++)
            //{
            //    Object en = values[i];
            //}
            foreach (Object en in Enum.GetValues(flags.GetType()))
            {
                if (0 != (Int32)en)
                {
                    if (flags.HasFlag(en as Enum))
                    {
                        output.Add(en);
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Vraća vrednosti enumeraci
        /// </summary>
        /// <param name="EnumType"></param>
        /// <returns></returns>
        public static Object[] getEnumList(this Type EnumType)
        {
            Object[] output;
            List<Object> tmpOut = new List<object>();

            foreach (Object it in Enum.GetValues(EnumType))
            {
                tmpOut.Add(it);
            }

            return tmpOut.ToArray();
        }
    }
}