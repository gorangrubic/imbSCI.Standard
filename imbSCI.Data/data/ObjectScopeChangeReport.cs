// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectScopeChangeReport.cs" company="imbVeles" >
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

    /// <summary>
    /// Describes changes found by ObjectPathParentAndRootMonitor
    /// </summary>
    public class ObjectScopeChangeReport
    {
        private Boolean isChanged;
        private Boolean isPathChanged;
        private Boolean isRootChanged;
        private Boolean isTargetChanged;
        private Boolean isParentChanged;
        private Boolean isChildrenCountChanged;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is path changed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is path changed; otherwise, <c>false</c>.
        /// </value>
        public bool IsPathChanged
        {
            get
            {
                return isPathChanged;
            }

            set
            {
                if (isPathChanged != value) isChanged = true;
                isPathChanged = value;
            }
        }

        /// <summary>
        /// If any of properties was changed
        /// </summary>
        public bool IsChanged
        {
            get
            {
                return isChanged;
            }

            set
            {
                isChanged = value;
            }
        }

        public bool IsRootChanged
        {
            get
            {
                return isRootChanged;
            }

            set
            {
                if (isRootChanged != value) isChanged = true;
                isRootChanged = value;
            }
        }

        public bool IsTargetChanged
        {
            get
            {
                return isTargetChanged;
            }

            set
            {
                if (isTargetChanged != value) isChanged = true;
                isTargetChanged = value;
            }
        }

        public bool IsParentChanged
        {
            get
            {
                return isParentChanged;
            }

            set
            {
                if (isParentChanged != value) isChanged = true;
                isParentChanged = value;
            }
        }

        public bool IsChildrenCountChanged
        {
            get
            {
                return isChildrenCountChanged;
            }

            set
            {
                if (isChildrenCountChanged != value) isChanged = true;
                isChildrenCountChanged = value;
            }
        }
    }
}