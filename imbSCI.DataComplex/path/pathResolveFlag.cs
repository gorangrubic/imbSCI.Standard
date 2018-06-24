// --------------------------------------------------------------------------------------------------------------------
// <copyright file="pathResolveFlag.cs" company="imbVeles" >
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
namespace imbSCI.DataComplex.path
{
    /// <summary>
    /// Odredjuje na koji način se obradjuje path
    /// </summary>
    public enum pathResolveFlag
    {
        autorenameCollectionIndexer,

        /// <summary>
        /// Uvek ce vracati property info path umesto vrednosti - odnosno za resolve ce vracati propertyInfo objekat umesto vrednosti
        /// </summary>
        returnPropertyInfo,

        returnNullIfPathEmpty,

        removeTypeAndRelationFilters,

        /// <summary>
        /// Preferira koriscenje property-namea
        /// </summary>
        preferPropertyName,

        /// <summary>
        /// Preferira koriscenje imena objekta umesto propertyRealName-a
        /// </summary>
        preferObjectName,

        /// <summary>
        /// Preferira koriscenje integer ID indexera [5]
        /// </summary>
        preferIntegerIndexer,

        /// <summary>
        /// Preferira koriscenje String name indexera - ["ime"]
        /// </summary>
        preferStringNameIndexer,

        /// <summary>
        /// Aktivira debug mode
        /// </summary>
        debugMode,

        /// <summary>
        /// null je prihvatljiva vrednost -- nece praviti gresku
        /// </summary>
        nullIsAcceptable,

        /// <summary>
        /// iskljucuje pravljenje DebugDoomyObjekta
        /// </summary>
        disableDoomyObject,

        /// <summary>
        /// Normalno radi
        /// </summary>
        none,

        /// <summary>
        /// Putanja pocinje da se izvrsava od imbProject objekta -- analogno postavljanju : simbola na pocetku putanje
        /// </summary>
        startFromProjectRoot,
    }
}