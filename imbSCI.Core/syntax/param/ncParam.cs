// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ncParam.cs" company="imbVeles" >
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
    using imbSCI.Data.data;
    using System;
    using System.ComponentModel;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Opisuje jednu instancu NC parametra koja ima svoj format, svoje mesto u liniji i mozda svoje slovo
    /// </summary>
    public class ncParam : dataBindableBase
    {
        /// <summary>
        /// Vraca String vrednost parametra
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String output = "";
            if (!String.IsNullOrEmpty(key))
            {
                output += key;
            }

            switch (valueType)
            {
                default:
                    output += strValue;
                    break;

                case ncParamValueType.label:
                    output += strValue;
                    break;

                case ncParamValueType.numeric:
                    output += decValue.ToString(format);
                    break;

                case ncParamValueType.error:
                    output += "!" + strValue + "!";
                    break;
            }
            if (output == "")
            {
            }
            return output;
        }

        /// <summary>
        /// Podesava parametar uz pomoc ulaznog stringa. Pokretati pre ubacivanja u kolekciju - da bi parametar bio dostupan preko string kljuca
        /// </summary>
        /// <param name="inputString">Moze imati space na pocetku i na kraju. Moze biti brojni ili string</param>
        /// <param name="__index">Redni broj parametra, ostaviti -1 ako ne treba da modifikuje indeks</param>
        public void setFromString(String inputString, Int32 __index = -1)
        {
            originalSourceCode = inputString;

            if (__index > -1) index = __index;

            if (String.IsNullOrEmpty(inputString))
            {
                valueType = ncParamValueType.empty;
                strValue = "";
                decValue = 0;
                return;
            }

            inputString = inputString.Trim();

            String parStr = Regex.Replace(inputString, "[^0-9.]", ""); // ako je parStr empty or null onda je label u pitanju

            if (String.IsNullOrEmpty(parStr))
            {
                strValue = parStr;
                valueType = ncParamValueType.label;
                return;
            }

            String parLet = inputString.Replace(parStr, "");

            Boolean isNegativeSign = parLet.Contains("-");

            parLet = parLet.Replace("-", "").Trim();

            if (parLet.Length > 0)
            {
                if (parLet.Length == 1)
                {
                    key = parLet;
                    valueType = ncParamValueType.numeric;
                }
                if (parLet.Length > 1)
                {
                    valueType = ncParamValueType.label;
                    strValue = inputString;
                }
            }
            else
            {
                valueType = ncParamValueType.numeric;
            }

            if (valueType == ncParamValueType.numeric)
            {
                if (Decimal.TryParse(parStr, out _decValue))
                {
                    if (isNegativeSign)
                    {
                        decValue = -decValue;
                    }

                    format = parStr.getFormatFromExample(0);
                }
                else
                {
                    strValue = "Failed to Parse [" + parStr + "] to Decimal value. parLet was: [" + parLet + "]";
                    valueType = ncParamValueType.error;
                }
            }
        }

        #region --- index ------- Indeks u kolekciji

        private Int32 _index = -1;

        /// <summary>
        /// Indeks u kolekciji
        /// </summary>
        public Int32 index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                OnPropertyChanged("index");
            }
        }

        #endregion --- index ------- Indeks u kolekciji

        #region --- key ------- Pronadjen kljuc

        private String _key = "";

        /// <summary>
        /// Pronadjen kljuc
        /// </summary>
        public String key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
                OnPropertyChanged("key");
            }
        }

        #endregion --- key ------- Pronadjen kljuc

        #region --- valueType ------- tip nc param value

        private ncParamValueType _valueType = ncParamValueType.label;

        /// <summary>
        /// tip nc param value
        /// </summary>
        internal ncParamValueType valueType
        {
            get
            {
                return _valueType;
            }
            set
            {
                _valueType = value;
                OnPropertyChanged("valueType");
            }
        }

        #endregion --- valueType ------- tip nc param value

        #region --- strValue ------- smestanje string vrednosti

        private String _strValue = "";

        /// <summary>
        /// smestanje string vrednosti
        /// </summary>
        internal String strValue
        {
            get
            {
                return _strValue;
            }
            set
            {
                _strValue = value;
                OnPropertyChanged("strValue");
            }
        }

        #endregion --- strValue ------- smestanje string vrednosti

        #region --- decValue ------- smestanje decimalne vrednosti

        private Decimal _decValue;

        /// <summary>
        /// smestanje decimalne vrednosti
        /// </summary>
        internal Decimal decValue
        {
            get
            {
                return _decValue;
            }
            set
            {
                _decValue = value;
                OnPropertyChanged("decValue");
            }
        }

        #endregion --- decValue ------- smestanje decimalne vrednosti

        #region --- originalSourceCode ------- izvodni kod

        private String _originalSourceCode = "";

        /// <summary>
        /// izvodni kod
        /// </summary>
        protected String originalSourceCode
        {
            get
            {
                return _originalSourceCode;
            }
            set
            {
                _originalSourceCode = value;
                OnPropertyChanged("originalSourceCode");
            }
        }

        #endregion --- originalSourceCode ------- izvodni kod

        #region -----------  format  -------  [Format kojim je bio zapisan parametar]

        private String _format = "#";// = new String();

        /// <summary>
        /// Format kojim je bio zapisan parametar
        /// </summary>
        // [XmlIgnore]
        [Category("ncParam")]
        [DisplayName("format")]
        [Description("Format kojim je bio zapisan parametar")]
        internal String format
        {
            get
            {
                return _format;
            }
            set
            {
                // Boolean chg = (_format != value);
                _format = value;
                OnPropertyChanged("format");
                // if (chg) {}
            }
        }

        #endregion -----------  format  -------  [Format kojim je bio zapisan parametar]
    }
}