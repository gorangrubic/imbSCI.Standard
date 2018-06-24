// --------------------------------------------------------------------------------------------------------------------
// <copyright file="resourceRelationTypes.cs" company="imbVeles" >
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
    /// Tip odnosa property-a i objekta
    /// </summary>
    public enum resourceRelationTypes
    {
        /// <summary>
        /// Property koji je ujedno i pod objekat
        /// ovo može da se koristi kada je potrebno da se koriste tipizirani Childrens
        /// </summary>
        integratedResource,

        /// <summary>
        /// Property je read-only interfejs prema objektu koji se smesta u Childrens
        /// koristi se kada objekat ne treba da koristi svoj childrens
        /// Veoma je korisno jer se automatski izvrsava loadDeploy i slicne operacije
        /// </summary>
        nestedResource,

        /// <summary>
        /// Property koji predstavlja ustvari pointer ka drugom resursu unutar projekta
        /// Kada je resurs linkovan on se nece snimati dva puta u save fajlu
        /// </summary>
        linkedResource,

        /// <summary>
        /// Predstavlja children item
        /// </summary>
        childResource,

        /// <summary>
        /// Property koji je klasa ali nije imbProjectResource naslednik
        /// obradjuje ga kao simpleObject ako je XmlIgnore odsutan
        /// editor ce biti PropertyGrid
        /// </summary>
        integratedSimpleObject,

        /// <summary>
        /// Privremeni simple object i imbProjectResource object
        /// </summary>
        temporaryObjects,

        /// <summary>
        /// Property koji je osnovnog tipa
        /// </summary>
        simpleProperties,

        /// <summary>
        /// Item u generickoj kolekciji
        /// </summary>
        indexerItem,

        /// <summary>
        /// Integrisana Entity kolekcija
        /// </summary>
        integratedEntityCollection,
    }
}