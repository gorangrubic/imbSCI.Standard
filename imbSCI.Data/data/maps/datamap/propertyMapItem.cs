// --------------------------------------------------------------------------------------------------------------------
// <copyright file="propertyMapItem.cs" company="imbVeles" >
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
    using System.Reflection;
    using System.Text;
    using System.Xml.Serialization;

    #endregion imbVeles using

    /// <summary>
    /// 2014c> Collection item: evaluationMapItem, part of evaluationMapItemCollection
    /// </summary>

    public class propertyMapItem : imbBindable, IImbMapItem
    {
        private string _path;

        public propertyMapItem()
        {
        }

        public propertyMapItem(String __name)
        {
            name = __name;
            path = __name;
        }

        #region -----------  name  -------  [Naziv ]

        private String _name; // = new String();

        /// <summary>
        /// cela putanja - onako kako je prosledjeno prilikom definisanja
        /// </summary>
        public string path
        {
            get { return _path; }
            set { _path = value; }
        }

        /// <summary>
        /// ujedno i naziv propertija
        /// </summary>
        public String name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        #endregion -----------  name  -------  [Naziv ]

        #region IImbMapItem Members

        /// <summary>
        /// Da li je map itemu dodeljeno pi
        /// </summary>
        public Boolean isActivated
        {
            get { return (pi != null); }
        }

        #endregion IImbMapItem Members

        #region --- pi ------- property info

        private PropertyInfo _pi;

        /// <summary>
        /// property info
        /// </summary>
        [XmlIgnore]
        public PropertyInfo pi
        {
            get { return _pi; }
            set
            {
                _pi = value;
                OnPropertyChanged("pi");
            }
        }

        /// <summary>
        /// Dijagnosticki ispis naziva ovog mapiranog itema
        /// </summary>
        /// <returns></returns>
        public string getMapItemLabel()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(name);
            if (pi == null)
            {
                sb.Append(" - not connected");
            }
            else
            {
                sb.Append(name);// + " (" + pi.")");
            }

            return sb.ToString();
        }

        #endregion --- pi ------- property info
    }
}