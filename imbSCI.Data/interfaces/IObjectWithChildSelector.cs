// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IObjectWithChildSelector.cs" company="imbVeles" >
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
namespace imbSCI.Data.interfaces
{
    using System;
    using System.Collections;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="IObjectWithName" />
    public interface IObjectWithChildSelector : IObjectWithName, IEnumerable
    {
        IEnumerator GetEnumerator();

        Object this[Int32 key] { get; }

        /// <summary>
        /// Index of supplied child - in his collection
        /// </summary>
        /// <returns>-1 if not found</returns>
        Int32 indexOf(IObjectWithChildSelector child);

        /// <summary>
        /// Number of child items
        /// </summary>
        /// <returns></returns>
        Int32 Count();

        /// <summary>
        /// Gets the <see cref="Object"/> with the specified child name.
        /// </summary>
        /// <value>
        /// The <see cref="Object"/>.
        /// </value>
        /// <param name="childName">Name of the child.</param>
        /// <returns></returns>
        Object this[String childName]
        {
            get;
        }
    }
}