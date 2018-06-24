// --------------------------------------------------------------------------------------------------------------------
// <copyright file="reportLink.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.links
{
    using imbSCI.Core.enums;
    using imbSCI.Core.interfaces;
    using imbSCI.Data;
    using imbSCI.Data.data;
    using imbSCI.Data.enums.fields;
    using imbSCI.Reporting.links.groups;
    using imbSCI.Reporting.links.reportRegistry;

    //public class reportingRegistryQuery
    //{
    //}

    /// <summary>
    /// Report item link
    /// </summary>
    public class reportLink : imbBindable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="reportLink"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public reportLink(reportLink source, bool copyPathToo = false)
        {
            linkTitle = source.linkTitle;
            linkDescription = source.linkDescription;
            if (copyPathToo) linkPath = source.linkPath;
            priority = source.priority;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="reportLink"/> class.
        /// </summary>
        /// <param name="__title">The title.</param>
        /// <param name="__description">The description.</param>
        /// <param name="__path">The path.</param>
        public reportLink(string __title, string __description, string __path)
        {
            linkTitle = __title;
            linkDescription = __description;
            linkPath = __path;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="reportLink"/> class.
        /// </summary>
        /// <param name="__title">The title.</param>
        /// <param name="__description">The description.</param>
        /// <param name="__query">The query.</param>
        public reportLink(string __title, string __description, reportingRegistryQuery __query)
        {
            linkTitle = __title;
            linkDescription = __description;
            registryQuery = __query;
        }

        private reportLinkState _state = reportLinkState.undefined;

        /// <summary> </summary>
        public reportLinkState state
        {
            get
            {
                return _state;
            }
            internal set
            {
                _state = value;
                OnPropertyChanged("state");
            }
        }

        private reportingRegistryQuery _registryQuery;

        /// <summary> </summary>
        public reportingRegistryQuery registryQuery
        {
            get
            {
                return _registryQuery;
            }
            protected set
            {
                _registryQuery = value;
                OnPropertyChanged("registryQuery");
            }
        }

        private dataPointImportance _importance = dataPointImportance.normal;

        /// <summary> </summary>
        public dataPointImportance importance
        {
            get
            {
                return _importance;
            }
            set
            {
                _importance = value;
                OnPropertyChanged("importance");
            }
        }

        private IMetaContentNested _element;

        /// <summary> </summary>
        public IMetaContentNested element
        {
            get
            {
                return _element;
            }
            internal set
            {
                _element = value;
                OnPropertyChanged("element");
            }
        }

        /// <summary>Priority used in menu sorting</summary>
        public int effectivePriority
        {
            get
            {
                return (group.priority * 100) + priority;
            }
        }

        private int _priority = 100;

        /// <summary> </summary>
        public int priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
                OnPropertyChanged("priority");
            }
        }

        public void convertToRelative()
        {
            string start = "{{{" + templateFieldBasic.root_relpath + "}}}";

            bool startsWithPlaceholder = linkPath.StartsWith(start);

            linkPath = linkPath.removeStartsWith(start);

            linkPath = linkPath.removeStartsWith("/");
            linkPath = linkPath.removeStartsWith("\\");
            if (startsWithPlaceholder)
            {
                linkPath = start + linkPath;
            }
        }

        ///// <summary>
        ///// Converting to relative link path
        ///// </summary>
        ///// <param name="parent"></param>
        //public void convertToRelative(reportSimpleBase parent)
        //{
        //    string pth = linkPath;
        //    //linkPath = linkPath.removeUrlShema();
        //    linkPath = linkPath.removeStartsWith(parent.paths.directory.FullName);
        //    linkPath = linkPath.removeStartsWith("\\");
        //    if (pth.Length == linkPath.Length)
        //    {
        //    }
        //}

        #region --- linkTitle ------- Naslov koji se prikazuje u linku ka ovom izvestaju. Ako je naslov empty onda nece biti linkovanja

        private string _linkTitle = "";

        /// <summary>
        /// Naslov koji se prikazuje u linku ka ovom izvestaju. Ako je naslov empty onda nece biti linkovanja
        /// </summary>
        public string linkTitle
        {
            get
            {
                return _linkTitle;
            }
            set
            {
                _linkTitle = value;
                OnPropertyChanged("linkTitle");
            }
        }

        #endregion --- linkTitle ------- Naslov koji se prikazuje u linku ka ovom izvestaju. Ako je naslov empty onda nece biti linkovanja

        #region --- linkDescription ------- Kratak opis ispod linka.

        private string _linkDescription = "";

        /// <summary>
        /// Kratak opis ispod linka.
        /// </summary>
        public string linkDescription
        {
            get
            {
                return _linkDescription;
            }
            set
            {
                _linkDescription = value;
                OnPropertyChanged("linkDescription");
            }
        }

        #endregion --- linkDescription ------- Kratak opis ispod linka.

        #region --- linkPath ------- relativna putanja koju koristi link

        private string _linkPath = "";

        /// <summary>
        /// relativna putanja koju koristi link
        /// </summary>
        public string linkPath
        {
            get
            {
                return _linkPath;
            }
            set
            {
                _linkPath = value;
                OnPropertyChanged("linkPath");
            }
        }

        #endregion --- linkPath ------- relativna putanja koju koristi link

        #region --- group ------- referenca prema grupi

        private reportInPackageGroup _group = null;

        /// <summary>
        /// referenca prema grupi
        /// </summary>
        public reportInPackageGroup group
        {
            get
            {
                return _group;
            }
            set
            {
                _group = value;
                OnPropertyChanged("group");
            }
        }

        #endregion --- group ------- referenca prema grupi
    }
}