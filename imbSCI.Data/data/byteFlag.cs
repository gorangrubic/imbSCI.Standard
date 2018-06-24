// --------------------------------------------------------------------------------------------------------------------
// <copyright file="byteFlag.cs" company="imbVeles" >
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
namespace imbSCI.Data.data
{
    using System;

    /// <summary>
    /// Struktura za flag vrednost
    /// </summary>
    public class byteFlag
    {
        public byteFlag()
        {
            value = 0;
        }

        public byteFlag(String input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                if (input.Length > 3)
                {
                    String inval = input.Substring(2);
                    value = Byte.Parse(inval, System.Globalization.NumberStyles.HexNumber);
                }
            }
        }

        public override string ToString()
        {
            String output = "0x";
            output = output + value.ToString("X2");
            return output;
        }

        #region --- value ------- brojcana vrednost

        private Byte _value = 0;

        /// <summary>
        /// brojcana vrednost
        /// </summary>
        public Byte value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                //OnPropertyChanged("value");
            }
        }

        #endregion --- value ------- brojcana vrednost
    }
}