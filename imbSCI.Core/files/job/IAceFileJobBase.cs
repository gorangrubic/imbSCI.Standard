// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAceFileJobBase.cs" company="imbVeles" >
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
namespace imbSCI.Core.files.job
{
    using imbSCI.Core.syntax.data.files;
    using imbSCI.Data.interfaces;
    using System;
    using System.ComponentModel;

    public interface IAceFileJobBase : IObjectWithName, INotifyPropertyChanged, IAceProjectBase
    {
        /// <summary>
        /// Name for this Job - used for filename and for menu navigation
        /// </summary>
// [XmlIgnore]
        String name { get; set; }

        /// <summary>
        /// Podesavanja skeniranja fajla
        /// </summary>
// [XmlIgnore]
        fileTargetListSettings scanFiles { get; set; }

        /// <summary>
        /// Number of files to be processed in one processing take; 0 and -1 will set program default
        /// </summary>
// [XmlIgnore]
        Int32 processTakeSize { get; set; }

        /// <summary>
        /// Generise string izvestaj o trenutnom poslu
        /// </summary>
        /// <returns></returns>
        String explainJob();

        /// <summary>
        /// Kreira event koji obaveštava da je promenjen neki parametar
        /// </summary>
        /// <remarks>
        /// Neće biti kreiran event ako nije spremna aplikacija: imbSettingsManager.current.isReady
        /// </remarks>
        /// <param name="name"></param>
        void OnPropertyChanged(string name);

        event PropertyChangedEventHandler PropertyChanged;
    }
}