// --------------------------------------------------------------------------------------------------------------------
// <copyright file="resourcePathGenerator.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataComplex.path
{
    #region imbVeles using

    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Data;
    using imbSCI.Data.interfaces;
    using System.Collections;
    using System.Reflection;

    //using aceCommonTypes.extensions;

    #endregion imbVeles using

    public static class resourcePathGenerator
    {
        /// <summary>
        /// 2014c: Univerzalni konstruktor putanje
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string getPathForObject(this object input)
        {
            //if (input is imbProjectResourceBase)
            //{
            //    imbProjectResourceBase input_imbProjectResourceBase = (imbProjectResourceBase) input;
            //    return input_imbProjectResourceBase.path;
            //}

            if (input is IObjectWithParent)
            {
                IObjectWithParent input_IObjectWithParent = (IObjectWithParent)input;
                return getPathForObjectWithParent(input_IObjectWithParent);
            }

            if (input is IObjectWithName)
            {
                IObjectWithName input_IObjectWithName = (IObjectWithName)input;
                return input_IObjectWithName.name;
            }

            return "";
        }

        /// <summary>
        /// Vraca putanju za property
        /// </summary>
        /// <param name="input">objekat za koji se vraca putanja ka propertiju</param>
        /// <param name="iPI"></param>
        /// <param name="returnPropertyPath"></param>
        /// <returns></returns>
        public static string getPathForProperty(this IObjectWithParent input, PropertyInfo iPI,
                                                bool returnPropertyPath = false)
        {
            string output = input.getPathForObjectWithParent();
            string key = imbProjectResourceBase.prefix_PROPERTY_PATH;
            string needle = iPI.Name;
            if (returnPropertyPath)
            {
                key = imbProjectResourceBase.prefix_PROPERTYINFO;
            }
            else
            {
                key = imbProjectResourceBase.prefix_PROPERTY_PATH;
                object relObj = input.imbGetPropertySafe(iPI, null);

                if (relObj is IObjectWithName)
                {
                    IObjectWithName relObj_IObjectWithName = (IObjectWithName)relObj;
                    needle = relObj_IObjectWithName.name;
                }
            }
            output = output.add(needle, key);
            return output;
        }

        /// <summary>
        /// Univerzalni konstruktor putanje za bilo koji tip i parent
        /// </summary>
        /// <param name="input"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static string getPathForObjectAndParent(this object input, object parent)
        {
            string output = "";
            string key = "";
            string prefix = imbProjectResourceBase.prefix_PROPERTY_PATH;

            string line = _getPathForParent(parent);
            key = _getPathKey(input, parent, out prefix);
            output = resourcePathResolver.prefixVsFormat[prefix].makePath(key, line);

            return output;
        }

        /// <summary>
        /// konstruise Path string za prosledjeni objekat
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string getPathForObjectWithParent(this IObjectWithParent input, string __key = "")
        {
            string output = "";
            string key = "";
            string prefix = imbProjectResourceBase.prefix_PROPERTY_PATH;
            object parent = input.parent;
            if (parent == null) return prefix = "";
            string line = _getPathForParent(parent);
            if (string.IsNullOrEmpty(__key)) __key = key = _getPathKey(input, parent, out prefix);

            output = resourcePathResolver.prefixVsFormat[prefix].makePath(key, line);

            return output;

            //return getPathForObjectAndParent(input, input.parent);
        }

        private static string _getPathForParent(object parent)
        {
            string line = "";
            if (parent == null) return "";

            #region PARENT PATH ------

            //if (parent is imbApplicationProject)
            //{
            //    line.Append(imbProjectResourceBase.prefix_PROJECT_PATH);
            //}
            //else
            if (parent is IObjectWithPath)
            {
                IObjectWithPath input_parent_IObjectWithPath = (IObjectWithPath)parent;
                line = line.add(input_parent_IObjectWithPath.path);
            }
            else
            {
                if (parent is IObjectWithName)
                {
                    IObjectWithName input_parent_IObjectWithName = (IObjectWithName)parent;
                    line = line.add(input_parent_IObjectWithName.name);
                }
                //else if (parent.GetType() == typeof(imbTypeInfo))
                //{
                //    imbTypeInfo parent_imbTypeInfo = (imbTypeInfo) parent;
                //    if (parent_imbTypeInfo.isStatic)
                //    {
                //        line.Append("~" + parent_imbTypeInfo.type.Name);
                //    }
                //}
            }

            #endregion PARENT PATH ------

            return line;
        }

        private static string _getPathKey(object input, object parent, out string prefix)
        {
            string key = "";
            prefix = imbProjectResourceBase.prefix_PROPERTY_PATH;
            if (parent is ICollection)
            {
                //if (parent is ICollectionWithKeyProperty)
                //{
                //    ICollectionWithKeyProperty input_parent_ICollectionWithKeyProperty =
                //        (ICollectionWithKeyProperty) parent;
                //    key = input_parent_ICollectionWithKeyProperty.getKeyForInstance(input);
                //}

                if (string.IsNullOrEmpty(key))
                {
                    if (parent is IList)
                    {
                        IList il = parent as IList;
                        key = il.IndexOf(input).ToString();
                    }
                }

                if (string.IsNullOrEmpty(key))
                {
                    if (input is IObjectWithName)
                    {
                        IObjectWithName input_IObjectWithName = (IObjectWithName)input;
                        key = input_IObjectWithName.name;
                    }
                }

                prefix = imbProjectResourceBase.prefix_COLLECTION_INDEX_ACCESS;
            }
            else
            {
                //oneOrMore<imbPropertyInfoWithValue> relations = parent.getAllPropertyInfoWithValue(input);
                //if (relations.isSomething)
                //{
                //    imbPropertyInfoWithValue iPIV = relations.First();
                //    prefix = iPIV.property.relationType.toPrefixString(imbProjectResourceBase.prefix_PROPERTY_PATH);
                //    key = iPIV.property.name;
                //}
                //else
                //{
                //}
            }
            return key;
        }
    }
}