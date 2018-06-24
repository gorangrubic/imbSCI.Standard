// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceRelationValue.cs" company="imbVeles" >
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
    /// Delegate that determinate value for a relationship between two items in the <see cref="aceRelationMatrix{TXAxis, TYAxis, TRelation}"/>
    /// </summary>
    /// <typeparam name="TXAxis">The type of the x axis.</typeparam>
    /// <typeparam name="TYAxis">The type of the y axis.</typeparam>
    /// <typeparam name="TRelation">The type of the relation.</typeparam>
    /// <param name="itemX">The item x.</param>
    /// <param name="itemY">The item y.</param>
    /// <returns>An object or value that describes the relation between <c>itemX</c> and <c>itemY</c></returns>
    public delegate TRelation aceRelationValue<TXAxis, TYAxis, TRelation>(TXAxis itemX, TYAxis itemY);
}