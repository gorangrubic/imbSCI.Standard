// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbCheckedObjects.cs" company="imbVeles" >
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
namespace imbSCI.Core.collection.checkLists
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    /// <summary>
    /// Kolekcija chekiranih Enum objekata
    /// </summary>
    public class imbCheckedObjects
    {
        #region --- values ------- Sve vrednosti

        private ObservableCollection<imbCheckedObject> _values;

        /// <summary>
        /// Sve vrednosti
        /// </summary>
        [XmlIgnore]
        public ObservableCollection<imbCheckedObject> values
        {
            get { return _values; }
            set
            {
                _values = value;
            }
        }

        #endregion --- values ------- Sve vrednosti

        #region --- checkList ------- Lista chekiranih vrednosti

        private List<string> _checkList = new List<string>();

        /// <summary>
        /// Lista chekiranih vrednosti
        /// </summary>
        public List<string> checkList
        {
            get { return _checkList; }
            set
            {
                _checkList = value;
                prepare();
            }
        }

        #endregion --- checkList ------- Lista chekiranih vrednosti

        private String _enumName;

        public imbCheckedObjects()
        {
        }

        /// <summary>
        /// naziv enumeracije koja se prikazuje
        /// </summary>
        public String enumName
        {
            get { return _enumName; }
            set
            {
                _enumName = value;
            }
        }

        public void prepare()
        {
            //ObservableCollection<imbCheckedObject> _values = new ObservableCollection<imbCheckedObject>();

            //Type enType = imbCoreManager.enumerations.getElementSafe(enumName) as Type;
            //    //._enumerations.getItem(enumName, imbCollection.loadType.nullIfNotFound) as Type;

            //if (enType != null)
            //{
            //    String[] names = Enum.GetNames(enType);
            //    foreach (String nm in names) _values.Add(new imbCheckedObject(false, nm));
            //}

            //foreach (String ch in _checkList)
            //{
            //    values.First(x => x.value == ch).isChecked = true;
            //}
        }

        //public imbCheckedObjects( enumeration)
        //{
        //}
    }
}