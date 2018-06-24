// --------------------------------------------------------------------------------------------------------------------
// <copyright file="memberRegistryTools.cs" company="imbVeles" >
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
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace imbSCI.Core.data.metadata
{
    /// <summary>
    /// Conversion tools
    /// </summary>
    public static class memberRegistryTools
    {
        public const String STR_F = "F";
        public const String STR_M = "M";
        public const String STR_N = "N";
        public const String STR_T = "T";
        public const String STR_E = "E";

        //public const String STR_NT = "+";
        public const String STR_P = "P";

        public const String STR_U = "U";

        /// <summary>
        /// Gets the enum.
        /// </summary>
        /// <param name="letter">The letter.</param>
        /// <returns></returns>
        public static memberRegistryEntryType GetEnum(String letter)
        {
            switch (letter)
            {
                case STR_F:
                    return memberRegistryEntryType.entry_field;
                    break;

                case STR_M:
                    return memberRegistryEntryType.entry_method;
                    break;

                default:

                    return memberRegistryEntryType.entry_unknown;
                    break;

                case STR_N:
                    return memberRegistryEntryType.entry_namespace;
                    break;

                case STR_T:
                    return memberRegistryEntryType.entry_type;
                    break;

                case STR_P:
                    return memberRegistryEntryType.entry_property;
                    break;
            }
        }

        /// <summary>
        /// Gets the name of the XML member.
        /// </summary>
        /// <param name="memberInfo">The member information.</param>
        /// <param name="addPrefixCode">If <c>true</c> adds 1-letter member type code (M,T,P,F...)</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Unknown member type - member</exception>
        public static String GetXMLMemberName(this MemberInfo memberInfo, Boolean addPrefixCode = true)
        {
            // this method is implemented using instructions from> https://www.brad-smith.info/blog/archives/220
            // Author: Bradley Smith, Nov 2010

            String prefixCode;
            string memberName = (memberInfo is Type)
                ? ((Type)memberInfo).FullName                               // member is a Type
                : (memberInfo.DeclaringType.FullName + "." + memberInfo.Name);  // member belongs to a Type

            switch (memberInfo.MemberType)
            {
                case MemberTypes.Constructor:
                    memberName = memberName.Replace(".ctor", "#ctor");
                    goto case MemberTypes.Method;
                case MemberTypes.Method:
                    prefixCode = memberRegistryTools.STR_M;
                    string paramTypesList = String.Join(
                        ",",
                        ((MethodBase)memberInfo).GetParameters()
                            .Cast<ParameterInfo>()
                            .Select(x => x.ParameterType.FullName
                        ).ToArray()
                    );
                    if (!String.IsNullOrEmpty(paramTypesList)) memberName += "(" + paramTypesList + ")";
                    break;

                case MemberTypes.Event: prefixCode = memberRegistryTools.STR_E; break;
                case MemberTypes.Field: prefixCode = memberRegistryTools.STR_F; break;

                case MemberTypes.NestedType:
                    memberName = memberName.Replace('+', '.');
                    goto case MemberTypes.TypeInfo;
                case MemberTypes.TypeInfo:
                    prefixCode = memberRegistryTools.STR_T;
                    break;

                case MemberTypes.Property: prefixCode = memberRegistryTools.STR_P; break;

                default:
                    throw new ArgumentException("Unknown member type", "member");
            }

            if (addPrefixCode)
            {
                return String.Format("{0}:{1}", prefixCode, memberName);
            }
            else
            {
                return memberName;
            }
        }

        ///// <summary>
        ///// The regex selectmethod
        ///// </summary>
        //public const string REGEX_SELECTMETHOD = @"M:([a - zA - Z.#<>_'`\d]*)\((.*)\)";

        ///// <summary>
        ///// The regex selectname
        ///// </summary>
        //public const string REGEX_SELECTNAME = @"([\w+]):([a-zA-Z\.#<>_'`\d]*)";

        //public static Regex regex_SelectMemberTypeAndPath = new Regex(@"(\w{1}):(.*)");

        /// <summary>
        /// Selects two or three groups from the member XML name attribute. First group is member type, then namespace/type path and 3rd is method brackets content
        /// </summary>
        public static Regex regex_SelectMethodPath = new Regex(@"(\w{1}):([\w\d\.\#\'\`\{\}]+)\(?([\w\d\.\,\s\'\`\{\}]*)\)?");

        /// <summary>
        /// Performs an implicit conversion from <see cref="memberRegistryEntryType"/> to <see cref="MemberTypes"/>.
        /// </summary>
        /// <param name="entryType">Type of the entry.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static MemberTypes ToMemberType(this memberRegistryEntryType entryType)
        {
            switch (entryType)
            {
                case memberRegistryEntryType.entry_field:
                    return MemberTypes.Field;
                    break;

                case memberRegistryEntryType.entry_method:
                    return MemberTypes.Method;
                    break;

                case memberRegistryEntryType.entry_namespace:
                    return MemberTypes.Custom;
                    break;

                case memberRegistryEntryType.entry_property:
                    return MemberTypes.Property;
                    break;

                case memberRegistryEntryType.entry_type:
                    return MemberTypes.TypeInfo;
                    break;

                default:
                case memberRegistryEntryType.entry_unknown:
                    return MemberTypes.Custom;
                    break;
            }
        }

        public static memberRegistryEntryType ToMemberType(this MemberTypes entryType)
        {
            switch (entryType)
            {
                case MemberTypes.Field:
                    return memberRegistryEntryType.entry_field;
                    break;

                case MemberTypes.Method:
                    return memberRegistryEntryType.entry_method;

                    break;

                case MemberTypes.Custom:

                    return memberRegistryEntryType.entry_namespace;
                    break;

                case MemberTypes.Property:
                    return memberRegistryEntryType.entry_property;

                    break;

                case MemberTypes.TypeInfo:
                    return memberRegistryEntryType.entry_type;

                    break;

                default:

                    return memberRegistryEntryType.entry_unknown;
                    break;
            }
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="entryType">Type of the entry.</param>
        /// <returns></returns>
        public static String GetString(this memberRegistryEntryType entryType)
        {
            switch (entryType)
            {
                case memberRegistryEntryType.entry_field:
                    return STR_F;
                    break;

                case memberRegistryEntryType.entry_method:
                    return STR_M;
                    break;

                default:
                case memberRegistryEntryType.entry_namespace:
                    return STR_N;
                    break;

                case memberRegistryEntryType.entry_property:
                    return STR_P;
                    break;

                case memberRegistryEntryType.entry_type:
                    return STR_T;
                    break;

                case memberRegistryEntryType.entry_unknown:
                    return STR_U;
            }
        }
    }
}