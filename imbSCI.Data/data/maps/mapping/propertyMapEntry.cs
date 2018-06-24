// --------------------------------------------------------------------------------------------------------------------
// <copyright file="propertyMapEntry.cs" company="imbVeles" >
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

    using System;
    using System.ComponentModel;

    #endregion imbVeles using

    /// <summary>
    /// 2014c> jedan property map unos
    /// </summary>

    public class propertyMapEntry : imbBindable
    {
        #region ----------- Boolean [ isActive ] -------  [Da li je aktivirano trenutno mapiranje]

        private Boolean _isActive = false;

        /// <summary>
        /// Da li je aktivirano trenutno mapiranje
        /// </summary>
        [Category("Switches")]
        [DisplayName("isActive")]
        [Description("Da li je aktivirano trenutno mapiranje")]
        public Boolean isActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnPropertyChanged("isActive");
            }
        }

        #endregion ----------- Boolean [ isActive ] -------  [Da li je aktivirano trenutno mapiranje]

        #region --- sourceProperty ------- naziv propertija koji je izvor podatka

        private String _sourceProperty;

        /// <summary>
        /// naziv propertija koji je izvor podatka
        /// </summary>
        public String sourceProperty
        {
            get { return _sourceProperty; }
            set
            {
                _sourceProperty = value;
                OnPropertyChanged("sourceProperty");
            }
        }

        #endregion --- sourceProperty ------- naziv propertija koji je izvor podatka

        #region --- targetProperty ------- naziv propertija u koji se snima podatak

        private String _targetProperty;

        /// <summary>
        /// naziv propertija u koji se snima podatak
        /// </summary>
        public String targetProperty
        {
            get { return _targetProperty; }
            set
            {
                _targetProperty = value;
                OnPropertyChanged("targetProperty");
            }
        }

        #endregion --- targetProperty ------- naziv propertija u koji se snima podatak
    }
}