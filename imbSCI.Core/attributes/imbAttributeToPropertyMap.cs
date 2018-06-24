// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbAttributeToPropertyMap.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.typeworks;

    #region imbVeles using

    using System;
    using System.Collections.Generic;
    using System.Reflection;

    #endregion imbVeles using

    /// <summary>
    /// Sadrzi podatke o mapiranju Attribute To Property
    /// </summary>
    public class imbAttributeToPropertyMap : Dictionary<PropertyInfo, imbAttributeName>
    {
        #region --- targetType ------- Tip za mapiranje

        private Type _targetType;

        /// <summary>
        /// Tip za mapiranje
        /// </summary>
        public Type targetType
        {
            get { return _targetType; }
            set { _targetType = value; }
        }

        #endregion --- targetType ------- Tip za mapiranje

        public imbAttributeToPropertyMap(Type __targetType)
        {
            PropertyInfo[] pi = new PropertyInfo[] { };
            targetType = __targetType;

            pi = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo p in pi)
            {
                if (p.CanWrite)
                {
                    imbAttributeCollection attributes = imbAttributeTools.getImbAttributeDictionary(p);
                    if (attributes.ContainsKey(imbAttributeName.metaValueFromAttribute))
                    {
                        imbAttribute att = attributes[imbAttributeName.metaValueFromAttribute];
                        Add(p, (imbAttributeName)att.objMsg);
                    }
                }
            }
        }

        public void writeToObject(Object target, imbAttributeCollection attributes, Boolean onlyDeclared = false,
                                  Boolean avoidOverwrite = false)
        {
            foreach (KeyValuePair<PropertyInfo, imbAttributeName> pair in this)
            {
                Boolean go = true;
                if (avoidOverwrite)
                {
                    if (pair.Key.GetValue(target, null) != null) go = false;
                }
                if (onlyDeclared)
                {
                    go = (pair.Key.DeclaringType == targetType);
                }

                if (go)
                {
                    if (attributes.ContainsKey(pair.Value))
                    {
                        target.imbSetPropertyConvertSafe(pair.Key, attributes[pair.Value].getMessage());
                        //pair.Key.SetValue(target, attributes[pair.Value].getMessage(), null);
                    }
                }
            }
        }
    }
}