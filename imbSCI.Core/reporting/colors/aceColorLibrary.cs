// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceColorLibrary.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.colors
{
    #region imbVeles using

    using imbSCI.Data.data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    #endregion imbVeles using

    /// <summary>
    /// 2013c: LowLevel resurs - GUI framework to store multiple aceColor definitions
    /// </summary>
    public class aceColorLibrary : imbBindable
    {
        #region -----------  collection  -------  [Skup boja prema hexadecimalnoj vrednosti i parametrima modulacije]

        private Dictionary<String, aceColorEntry> _collection = new Dictionary<String, aceColorEntry>();

        /// <summary>
        /// Skup boja prema hexadecimalnoj vrednosti i parametrima modulacije
        /// </summary>
        // [XmlIgnore]
        [Category("aceColorLibrary")]
        [DisplayName("collection")]
        [Description("Skup boja prema hexadecimalnoj vrednosti i parametrima modulacije")]
        public Dictionary<String, aceColorEntry> collection
        {
            get { return _collection; }
            set
            {
                // Boolean chg = (_collection != value);
                _collection = value;
                OnPropertyChanged("collection");
                // if (chg) {}
            }
        }

        #endregion -----------  collection  -------  [Skup boja prema hexadecimalnoj vrednosti i parametrima modulacije]
    }
}