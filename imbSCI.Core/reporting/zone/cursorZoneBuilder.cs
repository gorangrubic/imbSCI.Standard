// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cursorZoneBuilder.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.zone
{
    using imbSCI.Core.extensions.data;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Helps with constructing subzones inside target zone
    /// </summary>
    public class cursorZoneBuilder
    {
        /// <summary>
        /// Builder to create subzones in speficied target. Target is <see cref="cursorZoneBuilder"/> class
        /// </summary>
        /// <param name="__target">The target.</param>
        public cursorZoneBuilder(cursorZone __target)
        {
            target = __target;
            c = new cursor(__target, textCursorMode.scroll, textCursorZone.outterZone);
        }

        /// <summary>
        /// Builds the columns in targeted zone - respecting provided relative withs
        /// </summary>
        /// <param name="numberOfColumns">Number of columns to build inside the zone. Use small numbers.</param>
        /// <param name="relWidth">Relative width ratio for each column. If less is provided than <c>numberOfColumns</c> the rest of columns will have relWidth=1</param>
        /// <remarks>
        /// <para>Widths are relative to each other. If you want first column to be double in width than each of the other two use:  2,1,1</para>
        /// </remarks>
        public void buildColumns(Int32 numberOfColumns, params Int32[] relWidth)
        {
            List<Int32> widthList = relWidth.getFlatList<Int32>();

            Int32 widthsMissing = numberOfColumns - widthList.Count();
            if (widthsMissing > 0)
            {
                for (int i = 0; i < widthsMissing; i++)
                {
                    widthList.Add(1);
                }
            }

            Int32 widthTotal = widthList.Sum();

            if (widthTotal == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(relWidth), "Total relative width is 0 -- ");
            }

            selectRange zw = c.selectToCorner(textCursorZoneCorner.Right);

            decimal widthUnit = Convert.ToDecimal(zw.x / widthTotal);

            c.moveToCorner(textCursorZoneCorner.Left);

            var zSize = c.selectZone();

            Int32 height = zSize.y;

            cursorZone sub = null;
            String key = "";

            foreach (Int32 cWidth in widthList)
            {
                sub = new cursorZone();
                sub.padding.Learn(target.padding);

                sub.margin.left = c.x;
                sub.margin.top = c.y;
                sub.margin.right = 0;
                sub.margin.bottom = 0;
                c.moveInDirection(textCursorZoneCorner.Right, Convert.ToInt32(Math.Ceiling(cWidth * widthUnit)));

                sub.width = c.x;
                sub.height = height;
                key = target.subzones.Add(cursorZoneRole.column, sub);
            }

            if (sub.innerRightPosition < target.innerRightPosition)
            {
                sub.width += (target.innerRightPosition - sub.innerRightPosition);
            }
        }

        private cursor _cursor;

        /// <summary>
        ///
        /// </summary>
        protected cursor c
        {
            get { return _cursor; }
            set { _cursor = value; }
        }

        private cursorZone _target;

        /// <summary>
        ///
        /// </summary>
        protected cursorZone target
        {
            get { return _target; }
            set { _target = value; }
        }
    }
}