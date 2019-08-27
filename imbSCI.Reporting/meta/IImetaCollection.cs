// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IImetaCollection.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta
{
    using imbSCI.Core.interfaces;
    using System.Collections;
    using System.Collections.Generic;

    public interface IImetaCollection : IEnumerable<IMetaContentNested>
    {
        int Count { get; }

        bool Any();

        /// <summary>
        /// Deploy
        /// </summary>
        void Sort();

#pragma warning disable CS1574 // XML comment has cref attribute 'aceReportException' that could not be resolved
        /// <summary>
        /// Discovers the common parent or sets the one provided in arguments
        /// </summary>
        /// <param name="__parent">The parent.</param>
        /// <returns></returns>
        /// <exception cref="aceReportException">Can't discover the parent when the collection is empty!! - null - discoverCommonParent exception</exception>
        IMetaContentNested discoverCommonParent(IMetaContentNested __parent = null);
#pragma warning restore CS1574 // XML comment has cref attribute 'aceReportException' that could not be resolved

        /// <summary>
        /// Method to register new page in collection - you must get new instance from parent object
        /// </summary>
        /// <param name="newChild"></param>
        /// <returns></returns>
        int Add(IMetaContentNested newChild, IMetaContentNested __parent = null);

        int IndexOf(IMetaContentNested child);

        /// <summary>
        /// Set parent
        /// </summary>
        /// <param name="__parent">The parent.</param>
        void setParentToItems(IMetaContentNested __parent);

        IMetaContentNested this[string key] { get; }

        IEnumerator GetEnumerator();

        IMetaContentNested this[int key] { get; }
    }
}