// --------------------------------------------------------------------------------------------------------------------
// <copyright file="changeBindableBase.cs" company="imbVeles" >
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
namespace imbSCI.Data.data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// Keeps record on properties whose values were changed since last call to <see cref="Accept"/> method
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class changeBindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Sets current state of the object to be Changed (i.e. from now on the object will know it had some changes since last <see cref="Accept"/> call);
        /// </summary>
        public void InvokeChanged()
        {
            if (!HasChanges) Changes.Add("External change invoke");
        }

        /// <summary>
        /// Gets the changes.
        /// </summary>
        /// <param name="andAccept">if set to <c>true</c> [and accept].</param>
        /// <returns></returns>
        public List<String> GetChanges(Boolean andAccept)
        {
            var l2 = Changes.ToList(); //.Clone();
            if (andAccept)
            {
                Changes.Clear();
            }
            return l2;
        }

        /// <summary>
        /// Clears all changes recorded since object creation or last Accept()
        /// </summary>
        /// <returns>true if there were changes</returns>
        protected virtual Boolean Accept()
        {
            if (Changes.Count > 0)
            {
                Changes.Clear();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has changes.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has changes; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public Boolean HasChanges
        {
            get
            {
                return (Changes.Count > 0);
            }
        }

        /// <summary>
        /// The changes
        /// </summary>
        private List<string> _Changes = new List<string>();

        /// <summary>
        /// Gets or sets the changes.
        /// </summary>
        /// <value>
        /// The changes.
        /// </value>
        [XmlIgnore]
        public List<string> Changes
        {
            get
            {
                return _Changes;
            }
            protected set
            {
                _Changes = value;
                OnPropertyChanged("Changes");
            }
        }

        /// <summary>
        /// Kreira event koji obaveštava da je promenjen neki parametar
        /// </summary>
        /// <remarks>
        /// Neće biti kreiran event ako nije spremna aplikacija: imbSettingsManager.current.isReady
        /// </remarks>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            if (!Changes.Contains(name)) Changes.Add(name);

            PropertyChangedEventHandler handler = PropertyChanged;
        }

        /// <summary>
        /// Occurs when a property value is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}