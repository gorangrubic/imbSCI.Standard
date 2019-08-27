// --------------------------------------------------------------------------------------------------------------------
// <copyright file="propertyMap.cs" company="imbVeles" >
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
namespace imbSCI.Data.data.maps.mapping
{
    #region imbVeles using

    using imbSCI.Data.collection;
    using imbSCI.Data.interfaces;
    using System;
    using System.Collections.Generic;

    #endregion imbVeles using

    /// <summary>
    /// 2014c> podesavanje mapiranja sa jedne klase na drugu
    /// </summary>
    public class propertyMap : List<propertyMapEntry>, IObjectWithName
    {
        public static String GetPropertyMapName(Type sourceType, Type targetType)
        {
            String typeNameA = sourceType.Name;
            String typeNameB = targetType.Name;

            if (typeNameA.CompareTo(typeNameB) < 0)
            {
                var tb = typeNameB;
                typeNameB = typeNameA;
                typeNameA = tb;
            }

            return typeNameA + "-" + typeNameB;
        }

        public propertyMap(Type sourceType, Type targetType)
        {
            name = GetPropertyMapName(sourceType, targetType);
        }

        public propertyMap(Type targetAndSourceType)
        {
            name = targetAndSourceType.Name;
        }

        

        private String _name;

        /// <summary>
        /// Naziv mapiranja - koristi se kod kesiranja klasa-to-klasa mapiranja
        /// </summary>
        public String name
        {
            get { return _name; }
            set
            {
                _name = value;
                
            }
        }

      
    }
}