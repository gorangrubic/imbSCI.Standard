// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbTreeNodeLeaf.cs" company="imbVeles" >
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
    #region imbVeles using

    using imbSCI.Core.attributes;
    using imbSCI.Data.interfaces;
    using System;

    #endregion imbVeles using

    /// <summary>
    /// Element treenodea koji nosi konkretan objekat
    /// </summary>
    [imb(imbAttributeName.xmlNodeTypeName, "leaf")]
    public class imbTreeNodeLeaf : imbTreeNode
    {
        public imbTreeNodeLeaf()
        {
        }

        public imbTreeNodeLeaf(IObjectWithName __value)
        {
            _nameBase = __value.name;
            value = __value;
            // UID = imbStringGenerators.getRandomString(32);
        }

        public imbTreeNodeLeaf(String __name, Object __value)
        {
            _nameBase = __name;
            value = __value;
            //UID = imbStringGenerators.getRandomString(32);
        }

        /// <summary>
        /// Automatsko odredjivanje tipa za leaf objekat
        /// </summary>
        public override imbTreeNodeType type
        {
            get
            {
                if (value == null)
                {
                    return imbTreeNodeType.leafEmpty;
                }
                return imbTreeNodeType.leaf;
            }
            set { }
        }
    }
}