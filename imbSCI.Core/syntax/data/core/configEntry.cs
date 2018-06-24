// --------------------------------------------------------------------------------------------------------------------
// <copyright file="configEntry.cs" company="imbVeles" >
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

// // using System.Linq;

// using System.Threading.Tasks;

namespace imbSCI.Core.syntax.data.core
{
    using imbSCI.Data.data;
    using System;

    /// <summary>
    /// Opis jedne konfiguracione varijable - ne sadrzi vrednost
    /// </summary>
    public class configEntry : dataBindableBase
    {
        #region --- name ------- Short flag name of configuration entry

        private String _name;

        /// <summary>
        /// Short flag name of configuration entry
        /// </summary>
        public String name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        #endregion --- name ------- Short flag name of configuration entry

        #region --- address ------- Int32 adresa u SYNCHRO, VT ili TECO registru

        private Int32 _address;

        /// <summary>
        /// Int32 adresa u SYNCHRO, VT ili TECO registru
        /// </summary>
        public Int32 address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                OnPropertyChanged("address");
            }
        }

        #endregion --- address ------- Int32 adresa u SYNCHRO, VT ili TECO registru

        #region --- description ------- Opis varijable

        private String _description = "";

        /// <summary>
        /// Opis varijable
        /// </summary>
        public String description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged("description");
            }
        }

        #endregion --- description ------- Opis varijable

        #region --- flags ------- Primenjene oznake

        private configEntryFlag _flags;

        /// <summary>
        /// Primenjene oznake
        /// </summary>
        public configEntryFlag flags
        {
            get
            {
                return _flags;
            }
            set
            {
                _flags = value;
                OnPropertyChanged("flags");
            }
        }

        #endregion --- flags ------- Primenjene oznake
    }
}