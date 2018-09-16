// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbTypeEnumExtensions.cs" company="imbVeles" >
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

/// <summary>
/// Extensions handling the enumeration conversions and so on.
/// </summary>
namespace imbSCI.Core.extensions.enumworks
{
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class imbTypeEnumExtensions
    {
        /// <summary>
        /// Sets the flag specified <c>flag</c>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="en">The enumeration to receive the flag</param>
        /// <param name="flag">The flag to set</param>
        /// <param name="setFlag">if set to <c>true</c> set the flag, if <c>false</c> it will unset</param>
        /// <returns></returns>
        public static T SetFlag<T>(this Enum en, Enum flag, Boolean setFlag = true) where T : IConvertible
        {
            Int32 eni = en.ToInt32();
            if (setFlag)
            {
                if (eni == 0)
                {
                    //en = flag;
                    eni = flag.ToInt32();
                }
                else
                {
                    if (!en.HasFlag(flag)) eni |= flag.ToInt32();
                }
            }
            else
            {
                if (en.HasFlag(flag)) eni &= ~flag.ToInt32();
            }

            return eni.imbToEnumeration<T>();
        }

        /// <summary>
        /// Determines whether the specified en has flags .
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="en">The en.</param>
        /// <param name="onOneFlag">The result to return if there is only one flag,and it is not <c>none</c> or <c>0</c> value</param>
        /// <returns>
        ///   <c>true</c> if the specified en has flags; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean HasFlags(this Enum en, Boolean onOneFlag = false)
        {
            Enum enu = en as Enum;
            if (enu.toStringSafe("none") == "none") return false;

            IList ens = enu.getEnumListFromFlags();
            if (ens.Count == 1) return onOneFlag;
            if (ens.Count > 1) return true;
            return false;
        }

        /// <summary>
        /// Determines whether the <see cref="Type"/> of <c>en</c> has <see cref="FlagsAttribute"/> attribute set
        /// </summary>
        /// <param name="en">The en.</param>
        /// <returns>
        ///   <c>true</c> if [has flag attribute] [the specified en]; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean HasFlagAttribute(this Enum en)
        {
            return (en.GetType().GetInterface("Flags") != null);
        }

        /// <summary>
        /// Converts a Enum type member into another Enum type member with name starting with or being the same as name of the first.
        /// </summary>
        /// <typeparam name="T">Enum jednostavniji</typeparam>
        /// <param name="input"></param>
        /// <remarks>
        /// <c>input</c> name should start with name of a member from <c>Enum</c>
        /// This may be used for same-name Enum to Enum conversion.
        /// <example>
        /// <para>inputEnum => input</para>
        /// <para>input => input</para>
        /// <para>input != inputEnum</para>
        /// </example>
        /// </remarks>
        /// <returns>T member with the same name or name that starts with <c>input</c> name</returns>
        /// \ingroup_disabled ace_ext_enum_highlight
        public static T convertToBasicEnum<T>(this Enum input)
        {
            String name = input.ToString();
            Type iTI = typeof(T);
            Array enumMembers = Enum.GetValues(iTI);

            foreach (var en in enumMembers)
            {
                if (name.StartsWith(Enum.GetName(iTI, en), StringComparison.Ordinal))
                {
                    return (T)en;
                }
            }
            return (T)enumMembers.getFirstSafe();
        }

        public static T imbToEnumeration<T>(this Object input)
        {
            Object res = "";

            if (input is T) return (T)input;
            res = imbToEnumeration(input, typeof(T));
            return (T)res;
        }

        /// <summary>
        /// Convers an value to the best fit Enumeration member
        /// </summary>
        /// <remarks>
        /// Kada input spada u kategorije:
        /// Boolean - ako je true onda mu zadaje onTrueEnum, za False ide prvi enum
        /// Enumeration - koristi findEnumerationMember() method> prvo same, starts, end, contains pa na kraju default int32
        /// Instance - koristi ime klase za findEnumerationMember()
        /// </remarks>
        /// <param name="input"></param>
        /// <param name="enumerationType"></param>
        /// <param name="onTrueEnum"></param>
        /// <returns>Enum member that matches input value</returns>
        public static Enum imbToEnumeration(this Object input, Type enumerationType, Enum onTrueEnum = null)
        {
            //outputTi = enumerationType;
            var enumMembers = Enum.GetValues(enumerationType);

            Enum output = enumMembers.getFirstSafe() as Enum;

            if (onTrueEnum == null)
            {
                if (enumMembers.Length > 1)
                {
                    onTrueEnum = enumMembers.GetValue(1) as Enum;
                }
                else
                {
                    onTrueEnum = output;
                }
            }

            if (input == null) return output;

            Type inputTi = input.GetType();
            MemberInfo same = null;

            //  imbEnumMemberInfo same = null;

            String tmp = "";
            switch (inputTi.getTypeGroup())
            {
                case imbTypeGroup.boolean:
                    Boolean bl = (Boolean)input;
                    if (bl)
                    {
                        output = onTrueEnum;
                    }
                    break;

                case imbTypeGroup.enumeration:
                    if (input.GetType() == enumerationType)
                    {
                        output = input as Enum;
                    }
                    else
                    {
                        String inputName = input.ToString();
                        Int32 inputInt = (Int32)input;
                        //same = enumerationType.GetMember(inputName).getFirstSafe() as MemberInfo;

                        same = enumerationType.findEnumerationMember(inputName, inputInt);
                        // enumMembers.imbFirstSafe(x => x.enumValue.ToString().ToLower() == inputName.ToLower());
                    }
                    break;

                case imbTypeGroup.number:
                    Int32 numValue = Convert.ToInt32(input);
                    output = Enum.ToObject(enumerationType, numValue) as Enum;

                    //same =enumMembers.imbFirstSafe(numValue);
                    //if (same != null)
                    //{
                    //    output = same.enumValue as Enum;
                    //}
                    break;

                case imbTypeGroup.instance:
                    tmp = input.GetType().Name;
                    //same = enumerationType.GetMember(inputName).getFirstSafe() as MemberInfo;

                    same = enumerationType.findEnumerationMember(tmp, 0);
                    break;

                case imbTypeGroup.unknown:
                case imbTypeGroup.text:
                    tmp = input.ToString().ToLower().Trim();
                    if (tmp.Length > 0)
                    {
                        if (Char.IsDigit(tmp[0]))
                        {
                            Int32 tmpNum = (Int32)tmp.imbToNumber(typeof(Int32));
                            output = Enum.ToObject(enumerationType, tmpNum) as Enum;
                        }
                        else
                        {
                            same = enumerationType.findEnumerationMember(tmp, 0);
                        }
                    }

                    break;
            }
            if (same != null)
            {
                output = Enum.Parse(enumerationType, same.Name) as Enum;
                //output = same as Enum;
            }
            return output;
        }

        /// <summary>
        /// Pronalazi imbEnumMemberInfo sa imenom koje odgovara needle-u, ako ne pronadje onda vraca podrazumevani enum na osnovu inputInt
        /// </summary>
        /// <remarks>
        /// Redosled pretrage> 1. ==, 2. starts, 3.ends, 4.contains</remarks>
        /// <param name="outputTi">Tipologija za Enum</param>
        /// <param name="needle">rec koja se trazi</param>
        /// <param name="inputInt">redni broj default enuma</param>
        /// <returns>Pronadjeni ili default enum. Null ako tipologija uopste nije enum</returns>
        public static MemberInfo findEnumerationMember(this Type outputTi, String needle,
                                                              Int32 inputInt = 0)
        {
            MemberInfo output = null;
            List<MemberInfo> enumMembers = outputTi.GetMembers().ToList();

            if (imbSciStringExtensions.isNullOrEmptyString(needle)) enumMembers.getFirstSafe();

            needle = needle.ToLower();

            output = enumMembers.imbFirstSafe(x => x.Name.ToLower() == needle);
            if (output == null)
            {
                output = enumMembers.imbFirstSafe(x => x.Name.ToLower().StartsWith(needle));
            }
            if (output == null)
            {
                output = enumMembers.imbFirstSafe(x => x.Name.ToLower().EndsWith(needle));
            }
            if (output == null)
            {
                output = enumMembers.imbFirstSafe(x => x.Name.ToLower().Contains(needle));
            }
            if (output == null)
            {
                output = enumMembers.imbFirstSafe(inputInt);
            }
            if (output == null)
            {
                output = enumMembers.imbFirstSafe();
            }
            return output;
        }
    }
}