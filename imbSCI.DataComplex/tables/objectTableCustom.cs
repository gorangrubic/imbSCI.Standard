// --------------------------------------------------------------------------------------------------------------------
// <copyright file="objectTableCustom.cs" company="imbVeles" >
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
using System;

namespace imbSCI.DataComplex.tables
{
    using imbSCI.Core.files.fileDataStructure;
    using imbSCI.Data.interfaces;

    /// <summary>
    /// Object table that supports auto load and auto save, as called from the <see cref="fileDataDescriptorBase.LoadDataFile(string, Core.reporting.ILogBuilder, Type)"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="imbSCI.DataComplex.tables.objectTable{T}" />
    /// <seealso cref="imbSCI.Data.interfaces.ISupportLoadSave" />
    public abstract class objectTableCustom<T> : objectTable<T>, ISupportLoadSave where T : class, new()
    {
        public abstract String keyPropertyName { get; }

        public Boolean LoadFrom(String path)
        {
            getReady();
            return Load(path, true);
        }

        protected void getReady()
        {
            primaryKeyName = keyPropertyName;

            prepare(typeof(T), keyPropertyName, name, true);
        }

        /// <summary>
        /// Constructor for normal initialization
        /// </summary>
        /// <param name="__name">The name.</param>
        protected objectTableCustom(String __name)
        {
            name = __name;
            getReady();
        }

        /// <summary>
        /// Constructor used only for automatic instance creation
        /// </summary>
        protected objectTableCustom()
        {
        }
    }
}