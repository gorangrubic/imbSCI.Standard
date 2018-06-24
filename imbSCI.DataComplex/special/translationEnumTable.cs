// --------------------------------------------------------------------------------------------------------------------
// <copyright file="translationEnumTable.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.special
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Serializable]
    public class translationEnumTable : Dictionary<object, string>
    {
        /// <summary>
        /// Gets the keys for value.
        /// </summary>
        /// <param name="stringForm">The string form.</param>
        /// <returns></returns>
        public List<object> GetKeysForValue(string stringForm)
        {
            List<object> output = new List<object>();
            foreach (KeyValuePair<object, string> pair in this)
            {
                if (pair.Value == stringForm)
                {
                    output.Add(pair.Key);
                }
            }
            return output;
        }

        /// <summary>
        /// Gets the enum having the stringForm equal to the specified
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stringForm">The string form.</param>
        /// <returns></returns>
        public T GetEnum<T>(string stringForm)
        {
            var keys = GetKeysForValue(stringForm);
            return keys.getFirstOfType<T>(false, default(T), true);
        }

        /// <summary>
        /// Gets the enums.
        /// </summary>
        /// <param name="types">The types</param>
        /// <param name="stringForm">The string form with multiple keys.</param>
        /// <returns></returns>
        public List<object> GetEnums(IEnumerable<Type> types, string stringForm)
        {
            List<object> output = new List<object>();
            //List<Object> candida
            string input = stringForm;
            foreach (Type t in types)
            {
                foreach (KeyValuePair<object, string> pair in this)
                {
                    if (pair.Key.GetType() == t)
                    {
                        if (stringForm.StartsWith(pair.Value))
                        {
                            if (pair.Key != "none")
                            {
                                output.Add(pair.Key);
                            }

                            stringForm = stringForm.removeStartsWith(pair.Value);
                        }
                    }
                }
            }

            return output;
        }

        public object GetEnum(Type type, string stringForm)
        {
            var keys = GetKeysForValue(stringForm);
            if (keys.Any(x => x.GetType() == type))
            {
                return keys.Where(x => x.GetType() == type)?.First();
            }
            else
            {
                return type.GetDefaultValue();
            }
        }
    }
}