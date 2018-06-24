// --------------------------------------------------------------------------------------------------------------------
// <copyright file="reportOutputUnit.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.delivery
{
    using imbSCI.Core.reporting.format;
    using imbSCI.Data.enums;

    /// <summary>
    /// One unit of output
    /// </summary>
    /// <seealso cref="imbSCI.Core.reporting.format.reportElementFormSet" />
    public class reportOutputUnit : reportElementFormSet
    {
        public reportOutputUnit()
        {
        }

        public reportOutputUnit(string __path, string __name, reportOutputFormatName __format, reportOutputForm __form, object __output, reportElementLevel __level)
        {
            path = __path;
            name = __name;
            fileformat = __format;
            form = __form;
            // builder = __builder;
            output = __output;
            level = __level;
        }

        public void saveOutput()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public string name { get; set; }

        #region --- path ------- Logical level path - used for search and data binding

        private string _path;

        /// <summary>
        /// Logical level path - used for search and data binding
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

        #endregion --- path ------- Logical level path - used for search and data binding

        #region --- directoryPath ------- path - directory part

        private string _directoryRelativePath = "";

        /// <summary>
        /// path - directory part
        /// </summary>
        public string directoryRelativePath
        {
            get
            {
                return _directoryRelativePath;
            }
            set
            {
                _directoryRelativePath = value;
                OnPropertyChanged("directoryRelativePath");
            }
        }

        #endregion --- directoryPath ------- path - directory part

        #region --- filenamePath ------- path part - with filename

        private string _filenamePath;

        /// <summary>
        /// path part - with filename
        /// </summary>
        public string filenamePath
        {
            get
            {
                return _filenamePath;
            }
            set
            {
                _filenamePath = value;
                OnPropertyChanged("filenamePath");
            }
        }

        #endregion --- filenamePath ------- path part - with filename

        /// <summary>
        ///
        /// </summary>
        public reportOutputFormatName fileformat { get; set; } = reportOutputFormatName.unknown;

        #region --- level ------- level

        private reportElementLevel _level;

        /// <summary>
        /// level
        /// </summary>
        public reportElementLevel level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
                OnPropertyChanged("level");
            }
        }

        #endregion --- level ------- level

        /// <summary>
        ///
        /// </summary>
        public reportOutputForm form { get; set; } = reportOutputForm.unknown;

        //private IDocumentRender _builder;
        ///// <summary>
        ///// reference to builder
        ///// </summary>
        //public IDocumentRender builder
        //{
        //    get { return _builder; }
        //    set { _builder = value; }
        //}

        #region --- output ------- typed output made by IRender instance

        private object _output;

        /// <summary>
        /// typed output made by IRender instance
        /// </summary>
        public object output
        {
            get
            {
                return _output;
            }
            set
            {
                _output = value;
                OnPropertyChanged("output");
            }
        }

        #endregion --- output ------- typed output made by IRender instance
    }
}