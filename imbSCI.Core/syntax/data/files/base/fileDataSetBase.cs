// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileDataSetBase.cs" company="imbVeles" >
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
using imbSCI.Data.data;

namespace imbSCI.Core.syntax.data.files.@base
{
    /// <summary>
    /// Polazna klasa za sve objekte koji su nastali citanjem fajlova
    /// </summary>
    public abstract class fileDataSetBase : dataBindableBase, IAceDataFile
    {
        #region --- path ------- source path

        private string _path;

        /// <summary>
        /// source path
        /// </summary>
        public string path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                OnPropertyChanged("path");
            }
        }

        #endregion --- path ------- source path

        /// <summary>
        /// Proverava da li je prosledjena putanja
        /// </summary>
        /// <param name="_path"></param>
        /// <returns></returns>
        protected string _checkPath(string _path)
        {
            if (string.IsNullOrEmpty(_path))
            {
            }
            else
            {
                path = _path;
            }
            return path;
        }

        public abstract bool load(string _path = "");

        public abstract bool afterLoad();

        public abstract bool processToObject(object _target);

        public abstract bool processToDictionary();

        public abstract bool beforeSave();

        public abstract bool save(string _path = "");
    }
}