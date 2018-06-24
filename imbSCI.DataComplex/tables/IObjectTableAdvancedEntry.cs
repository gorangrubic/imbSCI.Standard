// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IObjectTableAdvancedEntry.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.tables
{
    /// <summary>
    /// Za kasniju implementaciju (ideja)
    /// </summary>
    /// <seealso cref="IObjectTableEntry" />
    public interface IObjectTableAdvancedEntry : IObjectTableEntry
    {
        /// <summary>
        /// Linked to
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        objectTableBase GetParent();

        /// <summary>
        /// Set the parent (do not c
        /// </summary>
        /// <param name="__parent">The parent.</param>
        void SetParent(objectTableBase __parent);

        /// <summary>
        /// Updates the row in the linked object table
        /// </summary>
        void UpdateRow();

        /// <summary>
        /// Unlinks this instance from the current parent
        /// </summary>
        void Unlink();

        /// <summary>
        /// Links to the specified parent -- and removes link with the previous parent
        /// </summary>
        /// <param name="parent">The parent.</param>
        void Link(objectTableBase parent);

        /// <summary>
        /// Removes this instance from the linked parent
        /// </summary>
        void Remove();

        /// <summary>
        /// Clones the entry -- optionally transfering the link to the newly created
        /// </summary>
        /// <param name="linkWithNew">if set to <c>true</c> [link with new].</param>
        void Clone(bool linkWithNew);

        /// <summary>
        /// Gets the key for this entry
        /// </summary>
        /// <returns></returns>
        string GetTableKey();
    }
}