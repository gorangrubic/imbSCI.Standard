// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaServicePage.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.page
{
    using imbSCI.Core.extensions.enumworks;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.format;
    using imbSCI.Reporting.interfaces;

    /// <summary>
    /// Service Pages are lateral document outputs used for: readme.md, readme.html, index.html, theme.css, theme.js ...
    /// </summary>
    /// <remarks>
    /// Service pages are one-page-document with stand alone Style
    /// </remarks>
    /// \ingroup_disabled docCore
    /// \ingroup_disabled docPage
    public abstract class metaServicePage : metaPage, IMetaContentNested, IMetaHasHeader, IMetaServicePage
    {
        protected metaServicePage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="metaServicePage"/>
        /// </summary>
        /// <param name="__description">Mapped to both: description of this page and as extra content of <c>header</c></param>
        /// <param name="__title">Display title that is mapped to <c>header.name</c></param>
        /// <param name="__name">Path-safe name to be set to this page. Leave empty to keep default name</param>
        /// <param name="__priority">The position preference of the page</param>
        protected metaServicePage(string __description = "", string __title = "", string __name = "", int __priority = 100)
        {
            priority = __priority;
            name = __name;
            header.title = __title;
            header.description = __description;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="metaServicePage"/>
        /// </summary>
        /// <param name="__description">Mapped to both: description of this page and as extra content of <c>header</c></param>
        /// <param name="__title">Display title that is mapped to <c>header.name</c></param>
        /// <param name="__name">Path-safe name to be set to this page. Leave empty to keep default name</param>
        /// <param name="__position">The position preference of the page</param>
        protected metaServicePage(string __description, string __title, string __name = "", metaServicePagePosition __position = metaServicePagePosition.atBeginning)
        {
            position = __position;
            priority = position.ToInt32();
            header.title = __title;
            header.description = __description;
        }

        /// <summary>
        ///
        /// </summary>
        public metaServicePagePosition position { get; set; } = metaServicePagePosition.unknown;

        private string _filenameBase;

        /// <summary> </summary>
        public string filenameBase
        {
            get
            {
                return _filenameBase;
            }
            set
            {
                _filenameBase = value;
                OnPropertyChanged("filenameBase");
            }
        }

        #region --- fileformat ------- Service page fileformat - it is used to override render settings

        private reportOutputFormatName _fileformat = reportOutputFormatName.textMdFile;

        /// <summary>
        /// Service page fileformat - it is used to override render settings
        /// </summary>
        public reportOutputFormatName fileformat
        {
            get
            {
                return _fileformat;
            }
            set
            {
                _fileformat = value;
                OnPropertyChanged("fileformat");
            }
        }

        #endregion --- fileformat ------- Service page fileformat - it is used to override render settings
    }
}