// --------------------------------------------------------------------------------------------------------------------
// <copyright file="propertyAnnotationPresetItemTuple.cs" company="imbVeles" >
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

namespace imbSCI.Core.style.preset
{
    using imbSCI.Core.extensions.enumworks;

    /// <summary>
    /// Preset item tuple
    /// </summary>
    public class propertyAnnotationPresetItemTuple
    {
        public propertyAnnotationPresetItemTuple()
        {
        }

        public propertyAnnotationPresetItemTuple(String _key, String _value)
        {
            key = _key;

            value = _value;
        }

        private Object _resolvedKey;

        /// <summary> Key in proper enum type</summary>
        public Object resolvedKey
        {
            get
            {
                if (_resolvedKey == null)
                {
                    _resolvedKey = ResolveKey();
                }
                return _resolvedKey;
            }
        }

        internal Object ResolveKey()
        {
            Object _key = null;
            if (key.Contains("."))
            {
                _key = imbEnumExtendBase.getEnumMemberByPath(key, propertyAnnotationPreset.supportedEnumTypes);
            }
            else
            {
                _key = (String)key;
            }

            if (_key == null)
            {
                if (key.Contains("."))
                {
                    throw new Exception("propertyAnnotationPresetItemTuple.key [" + key + "] type, not recognized as supported enum type");
                }
                else
                {
                }
            }

            return _key;
        }

        public String key { get; set; } = "";

        public String value { get; set; } = null;
    }
}