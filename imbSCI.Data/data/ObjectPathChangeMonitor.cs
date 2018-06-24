// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectPathChangeMonitor.cs" company="imbVeles" >
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
    using imbSCI.Data.interfaces;
    using System;

    /// <summary>
    /// Helper class to monitor path change
    /// </summary>
    /// <seealso cref="imbBindable" />
    public class ObjectPathChangeMonitor : imbBindable
    {
        private String lastPath = "";
        private String newPath = "";

        /// <summary>
        /// New instance with initial target
        /// </summary>
        /// <param name="target">The target.</param>
        public ObjectPathChangeMonitor(IObjectWithPath target)
        {
            update(target);
        }

        public virtual Boolean update(IObjectWithPath target)
        {
            _target = target;
            if (_target == null)
            {
                newPath = "";
            }
            newPath = _target.path;
            return getPathChange(false);
        }

        protected IObjectWithPath _target;

        /// <summary>
        /// TEels if scope path was changed than last call with <c>markAsUnchanged</c> set TRUE
        /// </summary>
        /// <param name="markAsUnchanged">if set to <c>true</c> if will change state to unchange</param>
        /// <returns>TRUE if scope-s path is different than last call with <c>markAsUnchanged</c> set TRUE</returns>
        public Boolean getPathChange(Boolean markAsUnchanged = true)
        {
            if (lastPath != newPath)
            {
                if (markAsUnchanged) lastPath = newPath;
                return true;
            }
            return false;
        }
    }
}