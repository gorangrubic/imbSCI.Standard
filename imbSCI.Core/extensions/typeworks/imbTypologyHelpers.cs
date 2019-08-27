// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbTypologyHelpers.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Data;


    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;


    /// <summary>
    /// Advanced reflection operations
    /// </summary>
    public static class imbTypologyHelpers
    {
        public static List<Type> CollectTypesOfProperties(this Type hostType)
        {
            List<Type> output = new List<Type>();
            var hostTypes = new List<Type>() { hostType };

            return hostTypes.CollectTypesOfProperties();
        }


        public static List<Type> CollectTypesOfProperties(this IEnumerable<Type> hostTypes)
        {
            List<Type> output = new List<Type>();

            foreach (Type host in hostTypes)
            {
                var props = host.GetProperties(BindingFlags.Instance | BindingFlags.Public);

                foreach (var prop in props)
                {
                    if (!output.Contains(prop.PropertyType))
                    {
                        output.Add(prop.PropertyType);
                    }
                }
            }

            return output;
        }


        /// <summary>
        /// Collects the types that are discovered around <c>hostType</c>, according to rules defined with <c>flags</c>
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="hostAssembly">The host assembly.</param>
        /// <param name="hostType">Type of the host.</param>
        /// <returns>List of types that are discovered around <c>hostType</c>, according to rules defined with <c>flags</c></returns>
        /// <seealso cref="CollectTypeFlags"/>
        public static List<Type> CollectTypes(CollectTypeFlags flags, Assembly hostAssembly = null, Type hostType = null)
        {
            List<Type> output = new List<Type>();
            String namespaceString = "";

            if (hostType != null)
            {
                namespaceString = hostType.Namespace;

                if (flags.HasFlag(CollectTypeFlags.ofTypeProperties))
                {
                    output.AddRange(CollectTypesOfProperties(new List<Type>() { hostType }));
                }
            }

            if (!flags.HasFlag(CollectTypeFlags.ofThisAssembly) || flags.HasFlag(CollectTypeFlags.ofAllAssemblies))
            {
                flags |= CollectTypeFlags.ofThisAssembly;
            }

            if (flags.HasFlag(CollectTypeFlags.ofThisAssembly))
            {
                if (hostAssembly == null)
                {
                    if (hostType != null) hostAssembly = hostType.Assembly;
                }

                if (hostAssembly == null)
                {
                    hostAssembly = Assembly.GetExecutingAssembly();
                }

                output.AddRange(hostAssembly.GetTypes());
            }
            else if (flags.HasFlag(CollectTypeFlags.ofAllAssemblies))
            {
                foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (flags.HasFlag(CollectTypeFlags.includeNonImbAssemblies) || ass.FullName.StartsWith("imb"))
                    {
                        output.AddRange(ass.GetTypes());
                    }
                }
            }

            output = FilterTypeList(output, flags);
            if (!namespaceString.isNullOrEmpty()) output = FilterByNamespace(output, namespaceString, flags);

            return output;
        }

        /// <summary>
        /// Filters the type list.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        private static List<Type> FilterTypeList(List<Type> types, CollectTypeFlags flags)
        {
            List<Type> output = new List<Type>();

            foreach (Type t in types)
            {
                if (flags.HasFlag(CollectTypeFlags.includeClassTypes)) if (t.IsClass) output.Add(t);
                if (flags.HasFlag(CollectTypeFlags.includeEnumTypes)) if (t.IsEnum) output.Add(t);
                if (flags.HasFlag(CollectTypeFlags.includeValueTypes)) if (t.IsValueType) output.Add(t);
                if (flags.HasFlag(CollectTypeFlags.includeGenericTypes)) if (t.IsGenericType) output.Add(t);
            }

            if (!output.Any()) output = types;

            return output;
        }

        /// <summary>
        /// Filters the by namespace.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <param name="namespaceString">The namespace string.</param>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        private static List<Type> FilterByNamespace(List<Type> types, String namespaceString, CollectTypeFlags flags)
        {
            List<Type> output = new List<Type>();

            String namespaceParent = namespaceString.getPathVersion(-1);

            foreach (Type t in types)
            {
                if (!t.Name.StartsWith("<"))
                {
                    if (t.Namespace != null)
                    {
                        if (flags.HasFlag(CollectTypeFlags.ofSameNamespace)) if (t.Namespace == namespaceString) output.Add(t);
                        if (flags.HasFlag(CollectTypeFlags.ofChildNamespaces)) if (t.Namespace.StartsWith(namespaceString)) output.Add(t);
                        if (flags.HasFlag(CollectTypeFlags.ofParentNamespace)) if (t.Namespace.StartsWith(namespaceParent)) output.Add(t);
                    }
                }
            }

            if (!output.Any()) output = types;

            return output;
        }

        /// <summary>
        /// Converts a list of <see cref="Type"/> to Dictionary having <see cref="Type.Name"/> as key
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static Dictionary<String, Type> TypeListToDictionary(this List<Type> list)
        {
            Dictionary<String, Type> output = new Dictionary<string, Type>();

            foreach (Type pet in list)
            {
                if (!output.ContainsKey(pet.Name)) output.Add(pet.Name, pet);
            }

            return output;
        }

        /// <summary>
        /// Collects the types around the <c>hostType</c> - according to specified flags
        /// </summary>
        /// <param name="hostType">Type of the host.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>Dictionary of collected types, keys are <see cref="Type.Name"/>s.</returns>
        public static Dictionary<String, Type> CollectTypes(this Type hostType, CollectTypeFlags flags)
        {
            Dictionary<String, Type> output = new Dictionary<string, Type>();

            List<Type> list = CollectTypes(flags, hostType.Assembly, hostType);

            foreach (Type pet in list)
            {
                if (!output.ContainsKey(pet.Name)) output.Add(pet.Name, pet);
            }

            return output;
        }

        /// <summary>
        /// Gets the name of the type from.
        /// </summary>
        /// <param name="typename">The typename.</param>
        /// <param name="defaultResult">The default result.</param>
        /// <returns></returns>
        /// \ingroup_disabled ace_ext_type_highlight
        public static Type getTypeFromName(this String typename, Type defaultResult = null)
        {
            if (typename == "type") return typeof(String);

            if (!typename.Contains("."))
            {
                typename = typename.ensureStartsWith("System.");
            }

            Type output = Type.GetType(typename, false, true);

            if (output == null)
            {
                output = defaultResult;
            }

            return output;
        }


        private static Object _REGEX_typename_lock = new Object();
        private static Regex _REGEX_typename;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static Regex REGEX_typename
        {
            get
            {
                if (_REGEX_typename == null)
                {
                    lock (_REGEX_typename_lock)
                    {

                        if (_REGEX_typename == null)
                        {
                            _REGEX_typename = new Regex(@"([\w\d]+)");
                            
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _REGEX_typename;
            }
        }


        public static String GetCleanTypeName(this Type type, Boolean showGenerics = true)
        {
            var generics = type.GetGenericArguments();
            if (generics.Any())
            {
                String output = REGEX_typename.Match(type.Name).Value;
                if (showGenerics)
                {
                    output = output + "<";
                    String insert = "";
                    foreach (Type g in generics)
                    {
                        insert = insert.add(g.Name, ",");
                    }
                    output += insert;
                    output = output + ">";
                }
                return output;
            } else
            {
                return type.Name;
            }
        }
            

        /// <summary>
        /// Gets the clean full name the type.
        /// </summary>
        /// <param name="typeFullName">Full name of the type.</param>
        /// <returns></returns>
        public static String getCleanTypeFullName(this String typeFullName)
        {
            String output = typeFullName;
            output = output.Replace("+", ".");
            output = output.Replace("`1", "<>");
            output = output.Replace("`2", "<>");
            output = output.Replace("`3", "<>");
            return output;
        }

        /// <summary>
        /// Gets the type path filter.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static String getTypePathFilter(this Type type)
        {
            if (type == null) return "";
            String output = type.Name;
            if (type.ContainsGenericParameters)
            {
                Type[] tg;
                tg = type.GetGenericArguments();
                foreach (var t in tg)
                {
                    output += "&" + t.Name;
                }
            }
            return output;
        }

        //internal static Boolean entityCollectionPropertyCheck(imbPropertyInfo iPI)
        //{
        //    if (iPI.type.Name.EndsWith("Collection"))
        //    {
        //        if (iPI.propertyTypeInfo.interfaces.Contains(typeof (IRelationEnabledCollection)))
        //        {
        //            if (iPI.propertyInfoSource.GetAccessors().Any())
        //            {
        //                if (!iPI.propertyInfoSource.Name.StartsWith("_"))
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}

        /// <summary>
        /// Describes the types.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static String describeTypes(this IEnumerable<Type> types, String separator = "; ")
        {
            StringBuilder sb = new StringBuilder();
            foreach (var t in types)
            {
                sb.Append(t.Name + separator);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns all types that are in parent chain of the specified one
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="callCount">The call count.</param>
        /// <returns></returns>
        private static List<Type> GetBaseTypes(this Type type, Int32 callCount = 0)
        {
            List<Type> output = new List<Type>();
            if (type == null) return output;
            if (callCount < 100)
            {
                Type head = type;
                Int32 c = 0;
                while (head != null)
                {
                    c++;
                    if (!output.Contains(head))
                    {
                        output.Add(head);
                    }
                    head = head.BaseType;

                    if (c > 100)
                    {
                        break;
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Returns list of all inherited types, in order of inherence: classBase.classInherited
        /// </summary>
        /// <param name="type">Tip za koji se vracaju nasledjeni tipovi</param>
        /// <param name="includeSelf">da li da na kraju liste ubaci i sebe</param>
        /// <param name="uniqueTypes">da li da budu samo unikatni tipovi</param>
        /// <param name="untilClass">Do koje klase da ide duboko u nasledjivanju? ako je null onda sve</param>
        /// <param name="callCount">The call count.</param>
        /// <returns>
        /// Listu nasledjivanja
        /// </returns>
        /// \ingroup_disabled ace_ext_type_highlight
        public static List<Type> GetBaseTypeList(this Type type, Boolean includeSelf = false,
                                                 Boolean uniqueTypes = false, Type untilClass = null, Int32 callCount = 0)
        {
            String signature = type.AssemblyQualifiedName + includeSelf.ToString() + uniqueTypes.ToString();
            if (untilClass != null) signature += untilClass.Name;

            List<Type> baseTypes = new List<Type>();
            Type[] tmp;

            baseTypes.AddRange(type.GetBaseTypes(callCount++), uniqueTypes);

            baseTypes.Reverse();
            if (includeSelf)
            {
                if (!baseTypes.Contains(type)) baseTypes.Add(type);
            } else
            {
                baseTypes.Remove(type);
            }

            if (untilClass != null)
            {
                Int32 ind = baseTypes.IndexOf(untilClass) + 1;
                if (ind > 0)
                {
                    baseTypes = baseTypes.GetRange(ind, baseTypes.Count - ind);
                }
            }
            // imbTypologyInternal.baseTypeListRegistry.Add(signature, baseTypes);
            return baseTypes;
        }

        /// <summary>
        /// Vraca tipiziranu podrazumevanu vrednost
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host"></param>
        /// <returns></returns>
        public static T GetDefaultValue<T>(this Object host)
        {
            return (T)typeof(T).GetDefaultValue();
        }

        /// <summary>
        /// Returns default value for type. It may be new instance if <c>t</c> is class type.
        /// </summary>
        /// <param name="t">The Type to get value for.</param>
        /// <returns>Proper default value</returns>
        /// \ingroup_disabled ace_ext_type_highlight
        public static object GetDefaultValue(this Type t)
        {
            Type iTI = t;

            if (iTI == null) iTI = typeof(String);
            if (iTI.isBoolean())
            {
                return false;
            }

            if (iTI.isText())
            {
                return "";
            }
            else if (iTI.IsPrimitive)
            {
                return 0;
            }
            else if (iTI.IsEnum)
            {
                foreach (object en in Enum.GetValues(t))
                {
                    return en;
                }
            }
            else if (iTI.IsClass)
            {
                return iTI.getInstance();
            }
            else
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// Determines whether Type has parameterless constructor
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [has parameterless constructor] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean hasParameterlessConstructor(this Type type)
        {
            Boolean output = false;
            ConstructorInfo[] cis = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            foreach (ConstructorInfo ci in cis)
            {
                ParameterInfo[] pari = ci.GetParameters();

                if (pari.Count() == 0)
                {
                    output = true;
                    break;
                }
            }
            return output;
        }

        /// <summary>
        /// Determines whether is Type a simple (String, number, enum...) one that can be serialized ToString() way easy.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is imb serializable simple] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean isImbSerializableSimple(this Type type)
        {
            Boolean output = false; // isImbSerializableQuick(type);

            if (type.IsNested)
            {
                if (!type.IsNestedPublic) return false;
            }
            else
            {
                if (type.IsNotPublic) return false;
            }

            if (type.ContainsGenericParameters)
            {
                return false;
            }

            if (type.IsPrimitive) return true;
            if (type.IsValueType) return true;
            if (type == typeof(String)) return true;

            output = type.hasParameterlessConstructor();

            return output;
        }
    }
}