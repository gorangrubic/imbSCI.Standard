// --------------------------------------------------------------------------------------------------------------------
// <copyright file="paramPair.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.data.core
{
    using imbSCI.Data.data;
    using System;

    public class paramPair : dataBindableBase
    {
        #region --- isComment ------- Da li je u pitanju komentar

        private Boolean _isComment;

        /// <summary>
        /// Da li je u pitanju komentar
        /// </summary>
        public Boolean isComment
        {
            get
            {
                return _isComment;
            }
            set
            {
                _isComment = value;
                OnPropertyChanged("isComment");
            }
        }

        #endregion --- isComment ------- Da li je u pitanju komentar

        #region --- name ------- ime parametra

        private String _name;

        /// <summary>
        /// ime parametra
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

        #endregion --- name ------- ime parametra

        #region --- value ------- vrednost parametra

        private String _value;

        /// <summary>
        /// vrednost parametra
        /// </summary>
        public String value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged("value");
            }
        }

        #endregion --- value ------- vrednost parametra

        #region --- config ------- Config meta definicija

        private configEntry _config;

        /// <summary>
        /// Config meta definicija
        /// </summary>
        public configEntry config
        {
            get
            {
                return _config;
            }
            set
            {
                _config = value;
                OnPropertyChanged("config");
            }
        }

        #endregion --- config ------- Config meta definicija
    }
}