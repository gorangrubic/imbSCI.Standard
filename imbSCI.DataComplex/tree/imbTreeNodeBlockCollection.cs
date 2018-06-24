// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbTreeNodeBlockCollection.cs" company="imbVeles" >
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
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.DataComplex.tree
{
    #region imbVeles using

    using System;

    #endregion imbVeles using

    public class imbTreeNodeBlockCollection : List<imbTreeNodeBlock>
    {
        public imbTreeNodeBlockCollection()
        {
            newBlock();
        }

        public imbTreeNodeBlockCollection(String firstBlockName)
        {
            newBlock(firstBlockName);
        }

        #region --- current ------- trenutni blok u kolekciji

        private imbTreeNodeBlock _current;

        /// <summary>
        /// trenutni blok u kolekciji
        /// </summary>
        public imbTreeNodeBlock current
        {
            get { return this.Last(); }
        }

        #endregion --- current ------- trenutni blok u kolekciji

        public imbTreeNodeBlock newBlock(String newblockName = "")
        {
            if (String.IsNullOrEmpty(newblockName))
            {
                newblockName = "Block_" + Count.ToString("D3");
            }
            imbTreeNodeBlock block = new imbTreeNodeBlock(newblockName);
            Add(block);
            return block;
        }

        /// <summary>
        /// Uklanja blokove koji nemaju iteme
        /// </summary>
        public void removeEmptyBlocks()
        {
            List<imbTreeNodeBlock> toRemove = this.Where(x => x.Count == 0).ToList();
            foreach (var itb in toRemove)
            {
                Remove(itb);
            }
        }
    }
}