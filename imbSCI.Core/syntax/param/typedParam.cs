// --------------------------------------------------------------------------------------------------------------------
// <copyright file="typedParam.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.param
{
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Parametar sa dodeljenom vrednoscu
    /// </summary>
    public class typedParam : INotifyPropertyChanged
    {
        public typedParam()
        {
        }

        public typedParam(typedParamInfo __info, String input)
        {
            _info = __info;

            input = input.Trim('\"');
            _value = input.imbConvertValueSafe(info.type);
        }

        public void setValue(String valueString)
        {
            _value = valueString.imbConvertValueSafe(info.type);
        }

        /// <summary>
        /// Gets the string representation of the parameter.
        /// </summary>
        /// <param name="declaration">if set to <c>true</c> it adds the Type declaration.</param>
        /// <param name="addDotComma">if set to <c>true</c> it adds dot comma character at the end.</param>
        /// <returns></returns>
        public String getString(Boolean declaration, Boolean addDotComma = false, Boolean explicitForm = true)
        {
            String output = "";
            if (explicitForm) output = info.name + "=";
            if (value != null)
            {
                if (info.type.isText())
                {
                    output += "\"" + value.toStringSafe() + "\"";
                }
                else
                {
                    output += value.toStringSafe();
                }
            }
            if (declaration)
            {
                output += info.type.Name;
            }
            if (addDotComma) output += ";";
            return output;
        }

        /// <summary>
        /// Dodeljena vrednost - obradjen
        /// </summary>
        private Object _value;

        /// <summary>
        /// Dodeljena vrednost - obradjen
        /// </summary>
        public Object value
        {
            get
            {
                return _value;
            }
        }

        public void setValueDirect(Object val)
        {
            _value = val;
        }

        /// <summary>
        /// info
        /// </summary>
        private typedParamInfo _info;

        /// <summary>
        /// info
        /// </summary>
        public typedParamInfo info
        {
            get
            {
                return _info;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}