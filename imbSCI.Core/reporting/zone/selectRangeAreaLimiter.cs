// --------------------------------------------------------------------------------------------------------------------
// <copyright file="selectRangeAreaLimiter.cs" company="imbVeles" >
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
// Project: imbSCI.Core
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

namespace imbSCI.Core.reporting.zone
{
    public class selectRangeAreaLimiter : selectRangeArea
    {
        public Boolean isActiveX { get; set; } = false;
        public Boolean isActiveY { get; set; } = false;

        public selectRangeAreaLimiter() : base(0, 0)
        {
            isActiveX = false;
            isActiveY = false;
        }

        public override bool isInside(int tX, int tY)
        {
            if (!isActiveY) base.isInside(tX, y + 1);
            if (!isActiveX) base.isInside(x - 1, tY);
            if (!isActiveY && !isActiveX) return true;
            return base.isInside(tX, tY);
        }

        public override bool isInsideOrEdge(int tX, int tY, int edge = 1)
        {
            if (!isActiveY) base.isInsideOrEdge(tX, y + 1, edge);
            if (!isActiveX) base.isInsideOrEdge(x - 1, tY, edge);
            if (!isActiveY && !isActiveX) return true;
            return base.isInsideOrEdge(tX, tY, edge);
        }
    }
}