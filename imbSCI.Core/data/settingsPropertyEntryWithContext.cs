// --------------------------------------------------------------------------------------------------------------------
// <copyright file="settingsPropertyEntryWithContext.cs" company="imbVeles" >
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
namespace imbSCI.Core.data
{
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Data;
    using System;
    using System.Collections;
    using System.Reflection;

    public class settingsPropertyEntryWithContext : settingsPropertyEntry
    {
        public settingsPropertyEntryWithContext(PropertyInfo __pi, Object __host, PropertyCollectionExtended pce = null) : base(__pi)
        {
            host = __host;
            Object defVal = null;

            if (pi.PropertyType.IsValueType)
            {
                defVal = pi.PropertyType.GetDefaultValue();
            }

            if (host != null)
            {
                value = host.imbGetPropertySafe(pi, defVal);
            }
            else
            {
                value = defVal;
            }
            //displayName = __pi.Name;

            //value = pi.imbGetPropertySafe(pi, pi.PropertyType.getInstance());
        }

        public settingsPropertyEntryWithContext(Object __i, IList __host, PropertyCollectionExtended pce = null)
            : base(__host.IndexOf(__i))
        {
            host = __host;
            value = __i;
            // index = __index;

            if (__host is IList)
            {
                IList l = __host as IList;

                Object i = __i;

                String iname = "[" + index.ToString() + "]";
                type = i.GetType();

                displayName = iname;
                description = imbSciStringExtensions.add(iname, "member [" + type.Name + "] of the collection [" + __host.GetType().Name + "]");
                categoryName = "items";

                //pis.Add(iname, null);
                //settingsPropertyEntryWithContext spe = new settingsPropertyEntryWithContext(pro, target);
                //  spes.Add(pro.Name, spe);
            }
        }

        #region --- host ------- referenca prema objektu

        private Object _host;

        /// <summary>
        /// referenca prema objektu
        /// </summary>
        public Object host
        {
            get
            {
                return _host;
            }
            set
            {
                _host = value;
                OnPropertyChanged("host");
            }
        }

        #endregion --- host ------- referenca prema objektu

        #region --- value ------- trenutna vrednost

        private Object _value;

        /// <summary>
        /// trenutna vrednost
        /// </summary>
        public Object value
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

        #endregion --- value ------- trenutna vrednost
    }
}