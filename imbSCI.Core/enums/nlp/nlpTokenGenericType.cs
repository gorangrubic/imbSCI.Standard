// --------------------------------------------------------------------------------------------------------------------
// <copyright file="nlpTokenGenericType.cs" company="imbVeles" >
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
// Project: imbNLP.Data
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------

namespace imbNLP.Data.enums
{
    using imbSCI.Core.attributes;

    /// <summary>
    /// Generički tip tokena
    /// </summary>
    public enum nlpTokenGenericType
    {
        /// <summary>
        /// ništa nije znano o ovom tokenu
        /// </summary>
        [imb(imbAttributeName.basicColor, "#7c7c7c")] unknown,

        /// <summary>
        /// Poznata reč pronađena u rečniku za odabrani jezik - badf48
        /// </summary>
        [imb(imbAttributeName.basicColor, "#badf48")] knownWord,

        /// <summary>
        /// Nepoznata reč - nije pronađen u rečniku za odabrani jezik
        /// </summary>
        unknownWord,

        /// <summary>
        /// skracenica
        /// </summary>
        [imb(imbAttributeName.basicColor, "#badf48")] wordAbrevation,

        /// <summary>
        /// Moguce da je u pitanju ime, prezime, naziv
        /// </summary>
        [imb(imbAttributeName.basicColor, "#50ea8a")] possibleName,

        /// <summary>
        /// Moguće da je akronim u pitanju
        /// </summary>
        [imb(imbAttributeName.basicColor, "#7feba8")] possibleAcronim,

        /// <summary>
        /// broj - može da ima , i . ali ne i ostale znake
        /// </summary>
        [imb(imbAttributeName.basicColor, "#1d82cf")] number,

        /// <summary>
        /// redni broj
        /// </summary>
        [imb(imbAttributeName.basicColor, "#106eb5")] numberOrdinal,

        /// <summary>
        /// broj koji ima određeno formatiranje:  ":/-" itd
        /// </summary>
        [imb(imbAttributeName.basicColor, "#4080b0")] numberFormated,

        /// <summary>
        /// skup neodređenih simbola
        /// </summary>
        [imb(imbAttributeName.basicColor, "#530577")] symbols,

        /// <summary>
        /// Mešavina slova i simbola
        /// </summary>
        [imb(imbAttributeName.basicColor, "#683181")] mixedAlfasymbolic,

        /// <summary>
        /// Pomešani alfanumerički token
        /// </summary>
        [imb(imbAttributeName.basicColor, "#694879")] mixedAlfanumeric,

        /// <summary>
        /// email adresa
        /// </summary>
        [imb(imbAttributeName.basicColor, "#9280ff")] email,

        /// <summary>
        /// Prazan podatak - nista
        /// </summary>
        [imb(imbAttributeName.basicColor, "#666666")] empty,
    }
}