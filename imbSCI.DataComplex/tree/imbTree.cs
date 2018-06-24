// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbTree.cs" company="imbVeles" >
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

namespace imbSCI.DataComplex.tree
{
    #region imbVeles using

    using System;

    #endregion imbVeles using

    /// <summary>
    /// Korenski objekat imb grananja
    /// </summary>
    public class imbTree : imbTreeNodeBranch
    {
        /// <summary>
        /// Automatsko odredjivanje tipa za root objekat
        /// </summary>
        public override imbTreeNodeType type
        {
            get { return imbTreeNodeType.root; }
            set { }
        }

        #region --- buildingFlags ------- flagovi koji odredjuju kako ce se graditi drvo

        private imbTreeBuildingFlag _buildingFlags;

        /// <summary>
        /// flagovi koji odredjuju kako ce se graditi drvo
        /// </summary>
        public imbTreeBuildingFlag buildingFlags
        {
            get { return _buildingFlags; }
            set
            {
                _buildingFlags = value;
                OnPropertyChanged("buildingFlags");
            }
        }

        #endregion --- buildingFlags ------- flagovi koji odredjuju kako ce se graditi drvo

        public const Int32 ITERATION_LIMIT = 100;

        /// <summary>
        /// Izvrsava obradu grananja -- pozivati kada je gotovo sa dodavanjem novih grana
        /// </summary>
        public void postprocessTree()
        {
            this.compressNodes(ITERATION_LIMIT);

            this.removeEmptyChildren();

            this.detectTypes(ITERATION_LIMIT);
        }

        #region CONSTRUCTORS

        public imbTree() : base()
        {
        }

        /// <summary>
        /// Konstruktor tree strukture kojim se prosledjuju i flag instrukcije
        /// </summary>
        /// <param name="treeName"></param>
        /// <param name="_flags"></param>
        public imbTree(String treeName, imbTreeBuildingFlag _flags) : base(treeName)
        {
            buildingFlags = _flags;
        }

        public imbTree(String rootName, IEnumerable<String> elements, imbTreeBuildingFlag _flags)
            : base(rootName)
        {
            buildingFlags = _flags;
            foreach (String se in elements)
            {
                AddNewBranch(se);
            }
        }

        #endregion CONSTRUCTORS
    }
}