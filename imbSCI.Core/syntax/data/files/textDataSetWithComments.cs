// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textDataSetWithComments.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.data.files
{
    using imbSCI.Core.syntax.data.core;
    using imbSCI.Core.syntax.data.files.@base;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public abstract class textDataSetWithComments<T> : textDataSetBase
    {
        protected Regex paramLine_set = new Regex(@"(.*)[\s]*[=][\s]*(.*)");
        protected Regex paramLine_tab = new Regex(@"(\w*)[\t\s]*(.*)");

        /// <summary>
        /// Obradjuje sve linije
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<T> processLines();

        /// <summary>
        /// Obradjuje jednu liniju
        /// </summary>
        /// <param name="_line"></param>
        /// <param name="i"> </param>
        /// <returns></returns>
        public abstract T processLine(string _line, int i);

        #region --- headerComments ------- Skup komentara na pocetku fajla

        private List<string> _headerComments = new List<string>();

        /// <summary>
        /// Skup komentara na pocetku fajla
        /// </summary>
        public List<string> headerComments
        {
            get
            {
                return _headerComments;
            }
            set
            {
                _headerComments = value;
                OnPropertyChanged("headerComments");
            }
        }

        #endregion --- headerComments ------- Skup komentara na pocetku fajla

        #region --- loadedParams ------- Bindable property

        private paramPairs _loadedParams;

        /// <summary>
        /// Bindable property
        /// </summary>
        public core.paramPairs loadedParams
        {
            get
            {
                return _loadedParams;
            }
            set
            {
                _loadedParams = value;
                OnPropertyChanged("loadedParams");
            }
        }

        #endregion --- loadedParams ------- Bindable property
    }
}