// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textFormatSetupSize.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.zone
{
    using System;
    using System.ComponentModel;

    public abstract class textFormatSetupSize : selectRange, INotifyPropertyChanged
    {
        protected Int32 _width = 85;
        protected Int32 _height = 1;

        /// <summary>
        /// maksimalna spoljna sirina formata (innerWidth+padding+margin = Windows.width)
        /// </summary>
        public virtual Int32 width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                OnPropertyChanged("width");
            }
        }

        /// <summary>
        /// maksimalna spoljna visina formata (innerHeight+padding+margin=Windows.Height)
        /// </summary>
        public virtual Int32 height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                OnPropertyChanged("height");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}