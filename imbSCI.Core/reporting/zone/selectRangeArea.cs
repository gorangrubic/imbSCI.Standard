// --------------------------------------------------------------------------------------------------------------------
// <copyright file="selectRangeArea.cs" company="imbVeles" >
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

/// <summary>
///
/// </summary>
namespace imbSCI.Core.reporting.zone
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.geometrics;
    using imbSCI.Data;
    using imbSCI.Data.interfaces;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Selection area> with its starting poing, ending point and size. It automatically calculates> x, y, width and height fields
    /// </summary>
    public class selectRangeArea : textFormatSetupSize, IGetCodeName
    {
        public String ToString()
        {
            return TopLeft.x + ":" + TopLeft.y + "-" + BottomRight.x + ":" + BottomRight.y;
        }

        /// <summary>
        /// Four placemaker format expected for: {x}, {y}, {width}, {height}
        /// </summary>
        /// <seealso cref="FORMAT_SPACESEPARATER"/>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public String ToString(String format)
        {
            return String.Format(format, x, y, width, height);
        }

        /// <summary>
        /// Format preset: x,y,w,h
        /// </summary>
        public const String FORMAT_SPACESEPARATER = "{0} {1} {2} {3}";

        public const String FORMAT_DEFAULT = "{0}:{1}-{2}:{3}";

        public override string getCodeName()
        {
            String output = this.GetType().Name.imbGetWordAbbrevation(3, true);

            output = output.add(TopLeft.getCodeName(), "-").add(BottomRight.getCodeName(), "-");

            return output;
        }

        /// <summary>
        /// Returns coordinates of a corner, within the area. <see cref="textCursorZoneCorner"/>
        /// </summary>
        /// <param name="corner">The corner.</param>
        /// <returns></returns>
        public selectRange GetCornerPoint(textCursorZoneCorner corner)
        {
            selectRange output = new selectRange(x, y);

            switch (corner)
            {
                case textCursorZoneCorner.Bottom:
                    output.y = BottomRight.y;
                    output.x = xCenter;
                    break;

                case textCursorZoneCorner.center:
                    output.y = yCenter;
                    output.x = xCenter;
                    break;

                case textCursorZoneCorner.default_corner:

                    break;

                case textCursorZoneCorner.DownLeft:
                    output.y = BottomRight.y;
                    output.x = x;
                    break;

                case textCursorZoneCorner.DownRight:
                    output.y = BottomRight.y;
                    output.x = BottomRight.x;
                    break;

                case textCursorZoneCorner.Left:
                    output.y = yCenter;
                    output.x = x;
                    break;

                case textCursorZoneCorner.none:
                    break;

                case textCursorZoneCorner.Right:

                    output.y = yCenter;
                    output.x = BottomRight.x;
                    break;

                case textCursorZoneCorner.Top:
                    output.y = TopLeft.y;
                    output.x = xCenter;
                    break;

                case textCursorZoneCorner.UpLeft:
                    output.y = y;
                    output.x = x;
                    break;

                case textCursorZoneCorner.UpRight:
                    output.y = y;
                    output.x = BottomRight.x;
                    break;

                default:
                    break;
            }

            return output;
        }

        /// <summary>
        /// Gets the vertical axis.
        /// </summary>
        /// <param name="atX">At x.</param>
        /// <param name="atW">At w.</param>
        /// <returns></returns>
        public selectRangeArea GetVerticalAxis(Int32 atX, Int32 atW = 1)
        {
            selectRangeArea output = new selectRangeArea(atX, y);
            output.x = atX;

            output.y = y;
            output.width = atW;
            output.height = height;
            return output;
        }

        /// <summary>
        /// Gets the horizontal axis.
        /// </summary>
        /// <param name="atY">At y.</param>
        /// <param name="atH">At h.</param>
        /// <returns></returns>
        public selectRange GetHorizontalAxis(Int32 atY, Int32 atH = 1)
        {
            selectRangeArea output = new selectRangeArea(x, atY);
            output.x = x;

            output.y = atY;
            output.width = width;
            output.height = atH;
            return output;
        }

        /// <summary>
        /// The top left - starting point
        /// </summary>
        private selectRange _topLeft = new selectRange();

        /// <summary>
        /// Down right - end point
        /// </summary>
        private selectRange _bottomRight = new selectRange();

        public selectRange BottomRight
        {
            get
            {
                return _bottomRight;
            }

            set
            {
                isChanged = true;
                _bottomRight = value;
            }
        }

        public selectRange TopLeft
        {
            get
            {
                return _topLeft;
            }

            set
            {
                isChanged = true;
                _topLeft = value;
            }
        }

        private bool isChanged = true;

        private selectRange _size = new selectRange();

        public selectRange size
        {
            get
            {
                if (isChanged)
                {
                    _size = new selectRange(BottomRight.x - TopLeft.x, BottomRight.y - TopLeft.y);
                    isChanged = false;
                }
                return _size;
            }
        }

        public override Int32 x
        {
            get
            {
                return TopLeft.x;
            }
        }

        public override Int32 y
        {
            get
            {
                return TopLeft.y;
            }
        }

        public override Int32 width
        {
            get
            {
                return size.x;
            }
            set
            {
                if (_size.x != value) setToChanged();
                _size.x = value;
                BottomRight.x = TopLeft.x + value;
            }
        }

        public override Int32 height
        {
            get
            {
                return size.y;
            }
            set
            {
                if (_size.y != value) setToChanged();
                _size.y = value;
                BottomRight.y = TopLeft.y + value;
            }
        }

        public Int32 xCenter
        {
            get
            {
                return x + (width / 2);
            }
        }

        /// <summary>
        /// Gets the y (vertical) center of area.
        /// </summary>
        /// <value>
        /// The y center.
        /// </value>
        public Int32 yCenter
        {
            get
            {
                return y + (height / 2);
            }
        }

        /// <summary>
        /// Resizes the area to specified width and height. Negative values will be ignored. Use -1 to keep a dimension unchanged.
        /// </summary>
        /// <param name="__width">The width. Negative value will be ignored. Use -1 to keep it unchanged.</param>
        /// <param name="__height">The height. Negative value will be ignored. Use -1 to keep it unchanged.</param>
        public void resize(Int32 __width, Int32 __height = -1)
        {
            if (__width > -1) BottomRight.x = TopLeft.x + __width;
            if (__height > -1) BottomRight.y = TopLeft.y + __height;
            isChanged = true;
        }

        /// <summary>
        /// Sets new position and optionally new size
        /// </summary>
        /// <param name="__x">The x.</param>
        /// <param name="__y">The y.</param>
        /// <param name="__width">The width - leave -1 to keep it unchanged</param>
        /// <param name="__height">The height - leave -1 to keep it unchanged</param>
        public void reset(Int32 __x, Int32 __y, Int32 __width = -1, Int32 __height = -1)
        {
            if (__width < 0) __width = width;
            if (__height < 0) __height = height;
            TopLeft.x = __x;
            TopLeft.y = __y;
            BottomRight.x = TopLeft.x + __width;
            BottomRight.y = TopLeft.y + __height;
            isChanged = true;
        }

        /// <summary>
        /// Sets to changed state
        /// </summary>
        internal void setToChanged()
        {
            isChanged = true;
        }

        /// <summary>
        /// Determines whether the specified x,y coordinates are showing to position within this area.
        /// </summary>
        /// <param name="tX">The t x.</param>
        /// <param name="tY">The t y.</param>
        /// <returns>
        ///   <c>true</c> if x,y is inside, and not on the edge
        /// </returns>
        public virtual Boolean isInside(Int32 tX, Int32 tY)
        {
            if (tX < TopLeft.x) return false;
            if (tX > BottomRight.x) return false;
            if (tY < TopLeft.y) return false;
            if (tY > BottomRight.y) return false;

            return true;
        }

        /// <summary>
        /// Determines whether x,y coordinates are inside or within edge of this area.
        /// </summary>
        /// <param name="tX">The t x.</param>
        /// <param name="tY">The t y.</param>
        /// <param name="edge">The edge thickness. It is applied in both direction: inside and outside from border zone</param>
        /// <returns>
        ///   <c>true</c> when x,y are inside area + area extended by the edge. If edge is 1 than exact size of area is tested.
        /// </returns>
        public virtual Boolean isInsideOrEdge(Int32 tX, Int32 tY, Int32 edge = 1)
        {
            if (tX < TopLeft.x - edge) return false;
            if (tX > BottomRight.x + edge) return false;
            if (tY < TopLeft.y - edge) return false;
            if (tY > BottomRight.y + edge) return false;

            return true;
        }

        /// <summary>
        /// Determines if x,y is near to an edge, corner or center of this area - respecting specified limit distance.
        /// </summary>
        /// <param name="tX">X (horizontal position) to test</param>
        /// <param name="tY">Y (vertical position) to test </param>
        /// <param name="nearLimit">Absolute distance considered to be near.</param>
        /// <returns>Edge, corner or center that is near to x,y. <see cref="textCursorZoneCorner.none"/> is returned if nothing is near within <c>nearLimit</c> range </returns>
        public textCursorZoneCorner isNearAnyCornerOrEdge(Int32 tX, Int32 tY, Int32 nearLimit = 2)
        {
            textCursorZoneCorner output = textCursorZoneCorner.none;
            textCursorZoneCorner outputH = textCursorZoneCorner.none;
            textCursorZoneCorner outputV = textCursorZoneCorner.none;

            if (Math.Abs(BottomRight.x - tX) <= nearLimit) outputH = textCursorZoneCorner.Left;

            if (Math.Abs(TopLeft.x - tX) <= nearLimit) outputH = textCursorZoneCorner.Right;

            if (Math.Abs(y - tY) <= nearLimit) outputV = textCursorZoneCorner.Top;

            if (Math.Abs(BottomRight.y - tY) <= nearLimit) outputV = textCursorZoneCorner.Bottom;

            if (outputH == textCursorZoneCorner.Left)
            {
                if (outputV == textCursorZoneCorner.Top)
                {
                    output = textCursorZoneCorner.UpLeft;
                }
                else if (outputV == textCursorZoneCorner.Bottom)
                {
                    output = textCursorZoneCorner.DownLeft;
                }
                else
                {
                    output = textCursorZoneCorner.Left;
                }
            }
            else if (outputH == textCursorZoneCorner.Right)
            {
                if (outputV == textCursorZoneCorner.Top)
                {
                    output = textCursorZoneCorner.UpRight;
                }
                else if (outputV == textCursorZoneCorner.Bottom)
                {
                    output = textCursorZoneCorner.DownRight;
                }
                else
                {
                    output = textCursorZoneCorner.Right;
                }
            }
            else
            {
                if (isNearToCorner(tX, tY, textCursorZoneCorner.center, nearLimit))
                {
                    output = textCursorZoneCorner.center;
                }
            }

            return output;
        }

        /// <summary>
        /// Absolute distance from corner or edge
        /// </summary>
        /// <param name="tX"></param>
        /// <param name="tY"></param>
        /// <param name="edgeOrCorner"></param>
        /// <returns></returns>
        public Int32 getDistanceFromCorner(Int32 tX, Int32 tY, textCursorZoneCorner edgeOrCorner)
        {
            switch (edgeOrCorner)
            {
                case textCursorZoneCorner.none:
                case textCursorZoneCorner.default_corner:
                    break;

                case textCursorZoneCorner.UpLeft:
                    return (TopLeft.y - tY).calcDiagonal(TopLeft.x - tX);
                    break;

                case textCursorZoneCorner.UpRight:
                    return (TopLeft.y - tY).calcDiagonal(BottomRight.x - tX);
                    break;

                case textCursorZoneCorner.DownLeft:
                    return (BottomRight.y - tY).calcDiagonal(TopLeft.x - tX);
                    break;

                case textCursorZoneCorner.DownRight:
                    return (BottomRight.y - tY).calcDiagonal(BottomRight.x - tX);
                    break;

                case textCursorZoneCorner.Left:
                    return Math.Abs(BottomRight.x - tX);
                    break;

                case textCursorZoneCorner.Right:
                    return Math.Abs(TopLeft.x - tX);
                    break;

                case textCursorZoneCorner.Top:
                    return Math.Abs(y - tY);
                    break;

                case textCursorZoneCorner.Bottom:
                    return Math.Abs(BottomRight.y - tY);
                    break;

                case textCursorZoneCorner.center:
                    return (yCenter - tY).calcDiagonal(xCenter - tX);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return 0;
        }

        /// <summary>
        /// Determines whether x,y coordinates are near to the specified corner or edge of the area
        /// </summary>
        /// <param name="tX">X (horizontal position) to test</param>
        /// <param name="tY">Y (vertical position) to test </param>
        /// <param name="edgeOrCorner">The edge or corner to test against</param>
        /// <param name="nearLimit">Absolute distance considered to be near</param>
        /// <returns>
        ///   <c>true</c> if [is near to corner] [the specified t x]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public Boolean isNearToCorner(Int32 tX, Int32 tY, textCursorZoneCorner edgeOrCorner, Int32 nearLimit = 2)
        {
            switch (edgeOrCorner)
            {
                case textCursorZoneCorner.none:
                case textCursorZoneCorner.default_corner:
                    break;

                case textCursorZoneCorner.UpLeft:
                    return (Math.Abs(TopLeft.y - tY) <= nearLimit) && (Math.Abs(TopLeft.x - tX) <= nearLimit);
                    break;

                case textCursorZoneCorner.UpRight:
                    return (Math.Abs(TopLeft.y - tY) <= nearLimit) && (Math.Abs(BottomRight.x - tX) <= nearLimit);
                    break;

                case textCursorZoneCorner.DownLeft:
                    return (Math.Abs(BottomRight.y - tY) <= nearLimit) && (Math.Abs(TopLeft.x - tX) <= nearLimit);
                    break;

                case textCursorZoneCorner.DownRight:
                    return (Math.Abs(BottomRight.y - tY) <= nearLimit) && (Math.Abs(BottomRight.x - tX) <= nearLimit);
                    break;

                case textCursorZoneCorner.Left:
                    return (Math.Abs(BottomRight.x - tX) <= nearLimit);
                    break;

                case textCursorZoneCorner.Right:
                    return (Math.Abs(TopLeft.x - tX) <= nearLimit);
                    break;

                case textCursorZoneCorner.Top:
                    return (Math.Abs(y - tY) <= nearLimit);
                    break;

                case textCursorZoneCorner.Bottom:
                    return (Math.Abs(BottomRight.y - tY) <= nearLimit);
                    break;

                case textCursorZoneCorner.center:
                    return (Math.Abs(yCenter - tY) <= nearLimit) && (Math.Abs(xCenter - tX) <= nearLimit);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
        }

        /// <summary>
        /// Determines whether X and Y coordinates are exactly on a border of this area
        /// </summary>
        /// <param name="tX">The t x.</param>
        /// <param name="tY">The t y.</param>
        /// <returns>
        ///   <c>true</c> if X,Y coordinates showing exactly border area of this area</c>.
        /// </returns>
        public Boolean isOnEdge(Int32 tX, Int32 tY)
        {
            if (tX < TopLeft.x) return false;
            if (tX > BottomRight.x) return false;
            if (tY < TopLeft.y) return false;
            if (tY > BottomRight.y) return false;

            if (tX == TopLeft.x) return true;
            if (tX == BottomRight.x) return true;
            if (tY == TopLeft.y) return true;
            if (tY == BottomRight.y) return true;

            return false;
        }

        /// <summary>
        /// Expands this rangeArea to have specified rangeArea inside its boundaries
        /// </summary>
        /// <param name="areaToWrap">The area to wrap.</param>
        public void expandToWrap(selectRangeArea areaToWrap)
        {
            if (areaToWrap.BottomRight.x > BottomRight.x)
            {
                BottomRight.x = areaToWrap.BottomRight.x;
            }

            if (areaToWrap.BottomRight.y > BottomRight.y)
            {
                BottomRight.y = areaToWrap.BottomRight.y;
            }

            if (areaToWrap.TopLeft.x < TopLeft.x)
            {
                TopLeft.x = areaToWrap.TopLeft.x;
            }

            if (areaToWrap.TopLeft.y < TopLeft.y)
            {
                TopLeft.y = areaToWrap.TopLeft.y;
            }

            isChanged = true;
        }

        /// <summary>
        /// Expands in the specified direction for given number of steps
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="step">The step.</param>
        public Int32 expand(textCursorZoneCorner direction, Int32 step)
        {
            switch (direction)
            {
                case textCursorZoneCorner.none:
                    break;

                case textCursorZoneCorner.default_corner:
                    break;

                case textCursorZoneCorner.UpLeft:

                    TopLeft.y -= step;
                    BottomRight.x -= step;

                    break;

                case textCursorZoneCorner.UpRight:
                    TopLeft.y -= step;
                    BottomRight.x += step;
                    break;

                case textCursorZoneCorner.DownLeft:
                    TopLeft.x -= step;
                    BottomRight.y += step;
                    break;

                case textCursorZoneCorner.DownRight:
                    BottomRight.x += step;
                    BottomRight.y += step;
                    break;

                case textCursorZoneCorner.Left:
                    TopLeft.x -= step;
                    break;

                case textCursorZoneCorner.Right:
                    BottomRight.x += step;
                    break;

                case textCursorZoneCorner.Top:
                    TopLeft.y -= step;
                    break;

                case textCursorZoneCorner.Bottom:
                    BottomRight.y += step;
                    break;

                case textCursorZoneCorner.center:
                    TopLeft.x -= step;
                    TopLeft.y -= step;
                    BottomRight.x += step;
                    BottomRight.y += step;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            isChanged = true;
            return step;
        }

        /// <summary>
        /// Expands the specified direction.
        /// </summary>
        /// <param name="direction">The direction of expansion</param>
        /// <param name="stepCriteria">If <c>true</c> it will expand one step</param>
        /// <returns>Size of expansion</returns>
        public Int32 expand(textCursorZoneCorner direction, Boolean stepCriteria)
        {
            if (!stepCriteria) return 0;

            return expand(direction, 1);
        }

        /// <summary>
        /// Expands in the specified direction for number of boolean arguments being true
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="stepCriterias">The step criterias.</param>
        public Int32 expand(textCursorZoneCorner direction, Int32 stepPerTrue, params Boolean[] stepCriterias)
        {
            List<Boolean> stepCriteriaList = stepCriterias.getFlatList<Boolean>();
            Int32 step = stepCriteriaList.getCountOf(true) * stepPerTrue;
            return expand(direction, step);
        }

        /// <summary>
        /// Gets the crossection / overlap area / with <c>operant</c>
        /// </summary>
        /// <param name="operand">The operand to get overlap with</param>
        /// <returns>Area of overlap between this and <c>operand</c> area</returns>
        public selectRangeArea getCrossection(selectRangeArea operand)
        {
            selectRangeArea output = new selectRangeArea(this.x, this.y, this.width, this.height);

            if (operand.x > output.x) output.TopLeft.x = operand.x;
            if (operand.y > output.y) output.TopLeft.y = operand.y;
            if (operand.BottomRight.x < output.BottomRight.x) output.BottomRight.x = operand.BottomRight.x;
            if (operand.BottomRight.y < output.BottomRight.y) output.BottomRight.y = operand.BottomRight.y;

            output.setToChanged();

            return output;
        }

        /// <summary>
        /// Two-point constructor: Initializes a new instance of the <see cref="selectRangeArea"/> struct.
        /// </summary>
        /// <param name="topLeft">The top left.</param>
        /// <param name="bottomRight">The bottom right.</param>
        public selectRangeArea(selectRange start, selectRange end)
        {
            TopLeft = new selectRange(start.x, start.y);
            BottomRight = new selectRange(end.x, end.y);
        }

        /// <summary>
        /// Starting point, width and height. Initializes a new instance of the <see cref="selectRangeArea"/> struct.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public selectRangeArea(selectRange start, Int32 width, Int32 height)
        {
            TopLeft = new selectRange(start.x, start.y);
            BottomRight = new selectRange(start.x + width, start.y + height);

            // size = new selectRange(width, height);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="selectRangeArea"/> class.
        /// </summary>
        /// <param name="x_end">Width</param>
        /// <param name="y_end">Height</param>
        public selectRangeArea(Int32 x_end, Int32 y_end)
        {
            TopLeft = new selectRange(0, 0);
            BottomRight = new selectRange(x_end, y_end);
        }

        /// <summary>
        /// Absolute coordinate constructor: TopLeft + BottomRight. Initializes a new instance of the <see cref="selectRangeArea"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="x_end">The x end.</param>
        /// <param name="y_end">The y end.</param>
        public selectRangeArea(Int32 x, Int32 y, Int32 x_end, Int32 y_end)
        {
            TopLeft = new selectRange(x, y);
            BottomRight = new selectRange(x_end, y_end);
            //size = new selectRange(x_end - x, y_end - y);
        }
    }
}