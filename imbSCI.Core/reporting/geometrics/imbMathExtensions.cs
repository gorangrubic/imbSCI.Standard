// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbMathExtensions.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.geometrics
{
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.enums;
    using System;
    using System.Collections.Generic;

    public static class imbMathExtensions
    {
        public static List<Double> getDoubleValues(this IEnumerable<Int32> source)
        {
            List<Double> output = new List<double>();
            foreach (Int32 it in source)
            {
                output.Add(Convert.ToDouble(it));
            }
            return output;
        }

        public static selectRangeArea normalizeRange(this selectRangeArea target)
        {
            String last = target.ToString();
            Int32 sX = Math.Min(target.TopLeft.x, target.BottomRight.x);
            Int32 sY = Math.Min(target.TopLeft.y, target.BottomRight.y);

            Int32 eX = Math.Max(target.TopLeft.x, target.BottomRight.x);
            Int32 eY = Math.Max(target.TopLeft.y, target.BottomRight.y);

            var output = new selectRangeArea(sX, sY, eX, eY);
            String now = output.ToString();
            if (last != now)
            {
            }

            return output;
        }

        public static selectRangeArea takeColumnGroup(this selectRangeArea area, Int32 y, Int32 yT, Int32 leftZone, Int32 rightZone, printHorizontal horizontal)
        {
            selectRangeArea output = new selectRangeArea(area.x, y, area.BottomRight.x, y + yT);
            switch (horizontal)
            {
                case printHorizontal.left:
                    output = new selectRangeArea(area.x, y, area.x + leftZone, y + yT);
                    break;

                case printHorizontal.middle:
                    output = new selectRangeArea(area.x + leftZone, y, area.BottomRight.x - rightZone, y + yT);
                    break;

                case printHorizontal.right:
                    output = new selectRangeArea(area.BottomRight.x - rightZone, y, area.BottomRight.x, y + yT);
                    break;

                case printHorizontal.hide:

                    break;
            }

            //  output = area.getCrossection(output);

            return output;
        }

        public static selectRangeArea takeRowSlice(this selectRangeArea area, Int32 yStart, Int32 yThickness = 1)
        {
            selectRangeArea output = new selectRangeArea(area.x, yStart, area.BottomRight.x, yStart + yThickness);
            //output = area.getCrossection(output);

            return output;
        }

        /// <summary>
        /// Moves the resulting inner box by vector. X affects left-right, Y affects top-bottom
        /// </summary>
        /// <param name="padding">The fourSideSetting i.e. padding</param>
        /// <param name="vector">The vector.</param>
        public static void moveInnerByVector(this fourSideSetting padding, selectRange vector)
        {
            padding.left += vector.x;
            padding.right -= vector.x;
            padding.top += vector.y;
            padding.bottom -= vector.y;
        }

        /// <summary>
        /// Affects Top and Left values of four side settings
        /// </summary>
        /// <param name="padding">The fourSideSetting i.e. padding</param>
        /// <param name="vector">The vector.</param>
        public static void moveTopLeftByVector(this fourSideSetting padding, selectRange vector, Boolean isRelative = false)
        {
            if (!isRelative)
            {
                padding.left = vector.x;
                padding.top = vector.y;
            }
            else
            {
                padding.left += vector.x;
                padding.top += vector.y;
            }
        }

        public static void moveByVector(this selectRange position, selectRange vector, Boolean isRelative = false)
        {
            if (!isRelative)
            {
                position.x = vector.x;
                position.y = vector.y;
            }
            else
            {
                position.x += vector.x;
                position.y += vector.y;
            }
            if (position is cursor)
            {
                cursor cp = position as cursor;
                cp.checkPositions();
            }
        }

        public static void moveByVector(this selectRangeArea position, selectRange vector, Boolean isRelative = false)
        {
            if (!isRelative)
            {
                position.x = vector.x;
                position.y = vector.y;
            }
            else
            {
                position.x += vector.x;
                position.y += vector.y;
            }
        }

        public static void resizeByVector(this selectRangeArea position, selectRange vector, Boolean isRelative = false)
        {
            if (!isRelative)
            {
                position.resize(vector.x, vector.y);
            }
            else
            {
                position.resize(position.width + vector.x, position.height + vector.y);
            }
        }

        /// <summary>
        /// Transforms direction to vector
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="force">The force.</param>
        /// <param name="vector">The vector.</param>
        /// <returns></returns>
        public static selectRange toVector(this textCursorZoneCorner direction, Int32 force = 1, selectRange vector = null)
        {
            if (vector == null) vector = new selectRange();
            switch (direction)
            {
                case textCursorZoneCorner.Left:
                    vector.x = -force;
                    break;

                case textCursorZoneCorner.Right:
                    vector.x = force;
                    break;

                case textCursorZoneCorner.Top:
                    vector.y = -force;
                    break;

                case textCursorZoneCorner.Bottom:
                    vector.y = force;
                    break;

                case textCursorZoneCorner.UpLeft:
                    vector.y = -force;
                    vector.x = -force;
                    break;

                case textCursorZoneCorner.UpRight:
                    vector.y = -force;
                    vector.x = force;

                    break;

                case textCursorZoneCorner.DownLeft:
                    vector.y = force;
                    vector.x = -force;
                    break;

                case textCursorZoneCorner.DownRight:
                    vector.y = force;
                    vector.x = force;
                    break;
            }
            return vector;
        }

        /// <summary>
        /// Calculates the diagonal from two arcs with 90' between
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static Int32 calcDiagonal(this Int32 a, Int32 b)
        {
            Int32 tmp = (a * a) + (b * b);
            return Convert.ToInt32(Math.Sqrt(tmp));
        }
    }
}