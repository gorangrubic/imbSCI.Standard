// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMapShemaBasic.cs" company="imbVeles" >
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
namespace imbSCI.Data.data.maps.datamap
{
    #region imbVeles using

    using imbSCI.Data.data.maps.mapping;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    #endregion imbVeles using

    public interface IMapShemaBasic : IValuePairs
    {
        IEnumerable<string> mapNames { get; }
        IEnumerable<PropertyInfo> properties { get; }

        /// <summary>
        /// podaci o izvornom tipu
        /// </summary>
        Type mappedType { get; set; }

        PropertyInfo getProperty(String name);

        IImbMapItem getMapItem(String name);

        String getShemaLabel();

        /// <summary>
        /// Proverava da li propertz treba ubaciti u shemu 2014c
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="sh"></param>
        /// <returns></returns>
        Boolean checkProperty(PropertyInfo pi);
    }
}