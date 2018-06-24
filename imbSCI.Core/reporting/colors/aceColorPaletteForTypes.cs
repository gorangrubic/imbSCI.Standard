// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceColorPaletteForTypes.cs" company="imbVeles" >
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
    using imbSCI.Data.data;

    #region imbVeles using

    using System;
    using System.Collections.Generic;

    #endregion imbVeles using

    /// <summary>
    /// Part of GUI framework
    /// </summary>
    /// <seealso cref="aceCommonTypes.primitives.imbBindable" />
    public class aceColorPaletteForTypes : imbBindable
    {
        #region -----------  defaultPalette  -------  [Podrazumevana paleta za null itd]

        private aceColorPalette _defaultPalette; // = new aceColorPalette();

        /// <summary>
        /// Podrazumevana paleta za null itd
        /// </summary>
        public aceColorPalette defaultPalette
        {
            get
            {
                if (_defaultPalette == null)
                {
                    if (collection.ContainsKey("defaultPalete"))
                    {
                        _defaultPalette = collection["defaultPalete"];
                    }
                    else
                    {
                        _defaultPalette = new aceColorPalette("#F2F2F2", 0.1F, 0, 0.05F, 0.05F, 5, true, "defaultPalete");
                        collection.Add("defaultPalete", _defaultPalette);
                    }
                }
                return _defaultPalette;
            }
            set { _defaultPalette = value; }
        }

        #endregion -----------  defaultPalette  -------  [Podrazumevana paleta za null itd]

        #region -----------  paletteForType  -------  [Recnik paleta za zadati tip]

        private Dictionary<String, aceColorPalette> _collection = new Dictionary<String, aceColorPalette>();

        /// <summary>
        /// Recnik paleta za zadati tip
        /// </summary>
        // [XmlIgnore]
        public Dictionary<String, aceColorPalette> collection
        {
            get { return _collection; }
            set { _collection = value; }
        }

        #endregion -----------  paletteForType  -------  [Recnik paleta za zadati tip]
    }
}