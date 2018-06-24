// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceRelationMatrixTriple.cs" company="imbVeles" >
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
namespace imbSCI.Data.collection.math
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TXAxis">The type of the x axis.</typeparam>
    /// <typeparam name="TYAxis">The type of the y axis.</typeparam>
    /// <typeparam name="TRelation">The type of the relation.</typeparam>
    public class aceRelationMatrixTriple<TXAxis, TYAxis, TRelation>
    {
        private TXAxis _itemX;

        /// <summary>
        ///
        /// </summary>
        public TXAxis itemX
        {
            get { return _itemX; }
            set { _itemX = value; }
        }

        private TYAxis _itemY;

        /// <summary>
        ///
        /// </summary>
        public TYAxis itemY
        {
            get { return _itemY; }
            set { _itemY = value; }
        }

        private TRelation _Value;

        /// <summary>
        ///
        /// </summary>
        public TRelation Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        public aceRelationMatrixTriple(TXAxis __itemX, TYAxis __itemY, TRelation __Value) : base()
        {
            itemX = __itemX;
            itemY = __itemY;
            Value = __Value;
        }
    }
}