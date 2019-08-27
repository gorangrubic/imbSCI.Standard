// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cursorZoneExecutionSettings.cs" company="imbVeles" >
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

namespace imbSCI.Core.reporting.zone
{
#pragma warning disable CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    /// <summary>
    /// Settings about behaviour of cursor inside a zone
    /// </summary>
    /// <seealso cref="aceCommonTypes.primitives.imbBindable" />
    public class cursorZoneExecutionSettings : imbBindable
#pragma warning restore CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    {
        #region --- zoneChangeOnNewDocument ------- zone to be applied upod entering or creating new document

        private textCursorZone _pageScopeInZone = textCursorZone.unknownZone;

        /// <summary>
        /// zone to be applied upod entering or creating new document
        /// </summary>
        public textCursorZone pageScopeInZone
        {
            get
            {
                return _pageScopeInZone;
            }
            set
            {
                _pageScopeInZone = value;
                OnPropertyChanged("zoneChangeOnNewDocument");
            }
        }

        #endregion --- zoneChangeOnNewDocument ------- zone to be applied upod entering or creating new document

        #region --- cursorMode ------- Way that cursor should hangle vertical spacing

        private textCursorMode _cursorMode = textCursorMode.scroll;

        /// <summary>
        /// Way that cursor should hangle vertical spacing
        /// </summary>
        public textCursorMode cursorMode
        {
            get
            {
                return _cursorMode;
            }
            set
            {
                _cursorMode = value;
                OnPropertyChanged("cursorMode");
            }
        }

        #endregion --- cursorMode ------- Way that cursor should hangle vertical spacing

        #region --- pageScopeInMove ------- Cursor move once new or existing page is scoped

        private textCursorZoneCorner _pageScopeInMove = textCursorZoneCorner.UpLeft;

        /// <summary>
        /// Cursor move once new or existing page is scoped
        /// </summary>
        public textCursorZoneCorner pageScopeInMove
        {
            get
            {
                return _pageScopeInMove;
            }
            set
            {
                _pageScopeInMove = value;
                OnPropertyChanged("pageScopeInMove");
            }
        }

        #endregion --- pageScopeInMove ------- Cursor move once new or existing page is scoped

        #region --- scopeInMove ------- What movement of cursor it should do on scoping inside child MetaContent

        private textCursorZoneCorner _scopeInMove = textCursorZoneCorner.Left;

        /// <summary>
        /// What movement of cursor it should do <c>just before</c> scoping inside child MetaContent
        /// </summary>
        public textCursorZoneCorner scopeInMove
        {
            get
            {
                return _scopeInMove;
            }
            set
            {
                _scopeInMove = value;
                OnPropertyChanged("scopeInMove");
            }
        }

        #endregion --- scopeInMove ------- What movement of cursor it should do on scoping inside child MetaContent

        #region --- scopeOutMove ------- Bindable property

        private textCursorZoneCorner _scopeOutMove = textCursorZoneCorner.Right;

        /// <summary>
        ///  What movement of cursor it should do <c>just before</c> scoping <c>outside</c> child MetaContent
        /// </summary>
        public textCursorZoneCorner scopeOutMove
        {
            get
            {
                return _scopeOutMove;
            }
            set
            {
                _scopeOutMove = value;
                OnPropertyChanged("scopeOutMove");
            }
        }

        #endregion --- scopeOutMove ------- Bindable property
    }
}