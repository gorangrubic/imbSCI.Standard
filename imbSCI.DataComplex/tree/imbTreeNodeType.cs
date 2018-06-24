// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbTreeNodeType.cs" company="imbVeles" >
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

namespace imbSCI.DataComplex.tree
{
    using System;

    [Flags]
    public enum imbTreeNodeType
    {
        none = 0,
        unknown = 1,

        /// <summary>
        /// leaf koji ima objekat
        /// </summary>
        leaf = 2,

        /// <summary>
        /// leaf koji nema objekat
        /// </summary>
        leafEmpty = 4,

        /// <summary>
        /// Predstalja koren imbTree objekta
        /// </summary>
        root = 8,

        /// <summary>
        /// Predstavlja granu koja ima parent sa jednim detetom i ima jedno dete koje je ili main ili lateral tipa -- imbTreeBranch
        /// </summary>
        main = 16,

        /// <summary>
        /// Grana ciji je parent main a ima samo Branches u childrenu koji nisu end tipa
        /// </summary>
        lateral = 32,

        /// <summary>
        /// poslednja grana do grane cije pod grane imaju listove
        /// </summary>
        lateralLast = 64,

        /// <summary>
        /// Prva grana do maina
        /// </summary>
        lateralFirst = 128,

        /// <summary>
        /// suva grana, nema ni lisce ni druge pod grane
        /// </summary>
        dry = 256,

        /// <summary>
        /// Grana koja ima childrene - neki su leaf a neki su branch
        /// </summary>
        leafed = 512,

        /// <summary>
        /// grana koja ima samo leafove
        /// </summary>
        end = 1024
    }
}