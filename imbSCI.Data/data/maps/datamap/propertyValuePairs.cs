// --------------------------------------------------------------------------------------------------------------------
// <copyright file="propertyValuePairs.cs" company="imbVeles" >
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

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    #endregion imbVeles using

    public class propertyValuePairs : propertyValuePairsBase<object>, IValuePairs
    //imbCollectionMeta<Object>, IValuePairs //Dictionary<string, object>, IValuePairs
    {
        #region --- origin ------- kako je instancirana kolekcija - sta je bio izvor strukture

        private propertyValuePairsOrigin _origin = propertyValuePairsOrigin.unknown;

        /// <summary>
        /// kako je instancirana kolekcija - sta je bio izvor strukture
        /// </summary>
        public propertyValuePairsOrigin origin
        {
            get { return _origin; }
            set
            {
                _origin = value;
                OnPropertyChanged("origin");
            }
        }

        #endregion --- origin ------- kako je instancirana kolekcija - sta je bio izvor strukture

        public propertyValuePairs()
        {
        }

        public propertyValuePairs(IEnumerable<String> fields)
        {
            foreach (var p in fields)
            {
                Add(p, "");
            }
            origin = propertyValuePairsOrigin.stringFields;
        }

        public propertyValuePairs(IEnumerable<KeyValuePair<string, object>> source)
        {
            foreach (var p in source)
            {
                Add(p.Key, p.Value);
            }
            origin = propertyValuePairsOrigin.keyValuePairStringObject;
        }

        public propertyValuePairs(IEnumerable<PropertyInfo> fields)
        {
            foreach (var p in fields)
            {
                Add(p.Name, null);
            }
            origin = propertyValuePairsOrigin.imbPropertyInfos;
        }

        public propertyValuePairs(IEnumerable<PropertyInfo> fields, Object source)
        {
            foreach (var p in fields)
            {
                Add(p.Name, null);
            }
            getValues(source);
            origin = propertyValuePairsOrigin.imbPropertyInfosWithSource;
        }

        ///bool ContainsKey(string name);
        ///
        ///
        /// <summary>
        /// Vraca pairs koji predstavljaju rezultat operacije nad skupom
        /// </summary>
        /// <remarks>
        /// Vraca vrednosti is second-a
        /// </remarks>
        /// <param name="second"></param>
        /// <param name="cst"></param>
        /// <returns></returns>
        public IValuePairs getCrossSection(IValuePairs second, crossSectionType cst)
        {
            return IValuePairsExtension.getCrossSection(this, second, cst);
        }

        // <summary>
#pragma warning disable CS1570 // XML comment has badly formed XML -- 'End tag was not expected at this location.'
        /// Vraca sve vrednosti iz niza
        /// </summary>
        /// <returns></returns>
        public List<Object> getValues()
#pragma warning restore CS1570 // XML comment has badly formed XML -- 'End tag was not expected at this location.'
        {
            return Enumerable.ToList<object>(Values);
        }

        IList<string> IValuePairs.Keys
        {
            get { return Keys as IList<string>; }
        }
    }
}