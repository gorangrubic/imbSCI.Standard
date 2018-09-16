// --------------------------------------------------------------------------------------------------------------------
// <copyright file="builderSettings.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.render.config
{
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.data;
    using imbSCI.Data.enums.appends;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Define way build process is performed
    /// </summary>
    public class builderSettings : imbBindable
    {
        public builderSettings()
        {
            foreach (appendType ap in Enum.GetValues(typeof(appendType)))
            {
                supportedAppends.Add(ap);
            }

            //supportedAppends.Add(appendType.)
        }

        protected reportOutputSupport _formats; // = new reportOutputSupport();

        /// <summary>
        /// Gets the output support definition for this report kind
        /// </summary>
        /// <value>
        /// The object containing output support info
        /// </value>
        /// \ingroup_disabled renderapi_service
        public reportOutputSupport formats
        {
            get
            {
                return _formats;
            }
            set
            {
                _formats = value;
            }
        }

        #region --- api ------- this builder is part of what api

        private reportAPI _api;

        /// <summary>
        /// this builder is part of what api
        /// </summary>
        public reportAPI api
        {
            get
            {
                return _api;
            }
            set
            {
                _api = value;
                OnPropertyChanged("api");
            }
        }

        #endregion --- api ------- this builder is part of what api

        #region --- cursorBehaviour ------- way cursor and zone behaves

        private cursorZoneExecutionSettings _cursorBehaviour = new cursorZoneExecutionSettings();

        /// <summary>
        /// way cursor and zone behaves
        /// </summary>
        public cursorZoneExecutionSettings cursorBehaviour
        {
            get
            {
                return _cursorBehaviour;
            }
            set
            {
                _cursorBehaviour = value;
                OnPropertyChanged("cursorBehaviour");
            }
        }

        #endregion --- cursorBehaviour ------- way cursor and zone behaves

        private List<appendType> _supportedAppends = new List<appendType>();

        /// <summary>
        ///
        /// </summary>
        public List<appendType> supportedAppends
        {
            get { return _supportedAppends; }
            set { _supportedAppends = value; }
        }

        #region --- forms ------- Bindable property

        private reportOutputFormat _forms = new reportOutputFormat(elementLevelFormPreset.none);

        /// <summary>
        /// Bindable property
        /// </summary>
        public reportOutputFormat forms
        {
            get
            {
                return _forms;
            }
            set
            {
                _forms = value;
                OnPropertyChanged("forms");
            }
        }

        #endregion --- forms ------- Bindable property
    }
}