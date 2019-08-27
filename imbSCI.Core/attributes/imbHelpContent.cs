// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbHelpContent.cs" company="imbVeles" >
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
namespace imbSCI.Core.attributes
{
    #region imbVeles using

    using System;

    #endregion imbVeles using

    /// <summary>
    /// Pomocne informacije
    /// </summary>
    public class imbHelpContent
    {

        public imbHelpContent()
        {

        }

        private String _description = "";
        private String _hints = "";
        private String _title = "";


        public String link { get; set; } = "";

        #region --- purpose ------- svrha opisane klase

        private String _purpose;

        /// <summary>
        /// svrha opisane klase
        /// </summary>
        [imb(imbAttributeName.metaValueFromAttribute, imbAttributeName.helpPurpose)]
        public String purpose
        {
            get
            {
                if (_purpose == null) return "";
                return _purpose;
            }
            set { _purpose = value; }
        }

        /// <summary>
        /// Hints
        /// </summary>
        [imb(imbAttributeName.metaValueFromAttribute, imbAttributeName.helpTips)]
        public string hints
        {
            get { return _hints; }
            set { _hints = value; }
        }

        /// <summary>
        /// Description - i.e. main help content
        /// </summary>
        [imb(imbAttributeName.metaValueFromAttribute, imbAttributeName.menuHelp)]
        [imb(imbAttributeName.metaValueFromAttribute, imbAttributeName.helpDescription)]
        public string description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Full title
        /// </summary>
        [imb(imbAttributeName.metaValueFromAttribute, imbAttributeName.menuCommandTitle)]
        [imb(imbAttributeName.metaValueFromAttribute, imbAttributeName.helpTitle)]
        public string title
        {
            get { return _title; }
            set { _title = value; }
        }

        #endregion --- purpose ------- svrha opisane klase
    }
}