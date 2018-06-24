// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataUnitPresenter.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataComplex.data.dataUnits.core
{
    using imbSCI.Core.collection;
    using imbSCI.Data.data;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex.data.dataUnits.enums;
    using System.Collections.Generic;

    /// <summary>
    /// Describes a way to render dataUnit to Table, Graph or text
    /// </summary>
    /// <typeparam name="TUnit">The type of the unit.</typeparam>
    public class dataUnitPresenter : imbBindable, IObjectWithNameAndDescription
    {
        /// <summary>
        ///
        /// </summary>
        public dataUnitBase parent { get; set; }

        private string _filenamebase;

        /// <summary>filename without extension</summary>
        public string filenamebase
        {
            get
            {
                return _filenamebase;
            }
            protected set
            {
                _filenamebase = value;
                OnPropertyChanged("filenamebase");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string key { get; protected set; }

        /// <summary>
        /// Name for this instance
        /// </summary>
        public string name { get; set; } = "";

        /// <summary>
        /// Human-readable description of object instance
        /// </summary>
        public string description { get; set; } = "";

        public dataUnitPresenter(string query, string __title, string __description)
        {
            key = query;
            name = __title;
            description = __description;
        }

        public void setFlags(dataDeliveryPresenterTypeEnum __presentationType, dataDeliverFormatEnum __formatFlags, dataDeliverAttachmentEnum __attachmentFlags)
        {
            formatFlags = __formatFlags;
            presentationType = __presentationType;
            attachmentFlags = __attachmentFlags;
        }

        /// <summary>
        ///
        /// </summary>
        public dataDeliverAttachmentEnum attachmentFlags { get; set; } = dataDeliverAttachmentEnum.attachCSV | dataDeliverAttachmentEnum.attachJSON | dataDeliverAttachmentEnum.attachExcel;

        private dataDeliveryPresenterTypeEnum _presentationType = dataDeliveryPresenterTypeEnum.tableTwoColumnParam;

        /// <summary> </summary>
        public dataDeliveryPresenterTypeEnum presentationType
        {
            get
            {
                return _presentationType;
            }
            protected set
            {
                _presentationType = value;
                OnPropertyChanged("presentationType");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public PropertyEntryColumn extraColumns { get; set; } = PropertyEntryColumn.entry_name | PropertyEntryColumn.entry_description;

        /// <summary>
        ///
        /// </summary>
        public dataDeliverFormatEnum formatFlags { get; set; } = dataDeliverFormatEnum.includeAttachment | dataDeliverFormatEnum.countColumn;

        /// <summary>
        ///
        /// </summary>
        public string propertyNameRegex { get; set; } = "";

        private List<string> _fields;

        /// <summary> </summary>
        public List<string> fields
        {
            get
            {
                return _fields;
            }
            protected set
            {
                _fields = value;
                OnPropertyChanged("fields");
            }
        }
    }
}