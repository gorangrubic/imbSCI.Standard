// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cursorVariator.cs" company="imbVeles" >
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
    using imbSCI.Core.enums;
    using imbSCI.Data.data;
    using imbSCI.Data.enums;
    using System;

    /// <summary>
    /// Helper class used for easier table styling
    /// </summary>
    public class cursorVariator : imbBindable
    {
        private long position = 0;

        private Int32 isOdd = -1;

        private Boolean doOdd = true;

        protected Int32 _minor = -1; // = "";

        /// <summary>
        /// Bindable property
        /// </summary>
        public Int32 minor
        {
            get { return _minor; }
        }

        protected Int32 _major = -1; // = "";

        /// <summary>
        /// Bindable property
        /// </summary>
        public Int32 major
        {
            get { return _major; }
        }

        protected Int32 _headZone = -1; // = "";

        /// <summary>
        /// Bindable property
        /// </summary>
        public Int32 headZone
        {
            get { return _headZone; }
        }

        protected Int32 _headZoneExtension = -1;

        /// <summary>
        ///
        /// </summary>
        public Int32 headZoneExtension
        {
            get { return _headZoneExtension; }
            set { _headZoneExtension = value; }
        }

        protected Int32 _footZoneExtension = -1; // = "";

        /// <summary>
        /// Bindable property
        /// </summary>
        public Int32 footZoneExtension
        {
            get { return _footZoneExtension; }
        }

        protected Int32 _footZone = -1; // = "";

        /// <summary>
        /// Bindable property
        /// </summary>
        public Int32 footZone
        {
            get { return _footZone; }
        }

        protected Int32 _leftZone = -1; // = "";

        /// <summary>
        /// Bindable property
        /// </summary>
        public Int32 leftZone
        {
            get { return _leftZone; }
        }

        protected Int32 _rightZone = -1; // = "";

        /// <summary>
        /// Bindable property
        /// </summary>
        public Int32 rightZone
        {
            get { return _rightZone; }
        }

        protected selectRangeArea _area = new selectRangeArea(1, 1); // = "";

        /// <summary>
        /// Bindable property
        /// </summary>
        public selectRangeArea area
        {
            get { return _area; }
        }

        /// <summary>
        /// Defines variator zone with odd/even, normal/major/minor vertical, head/headExtension/foot/footExtension, normal/left/right horizontal detection.
        /// </summary>
        /// <param name="__area">The area.</param>
        /// <param name="minorLine">Minor line is every Nth. 0 will disable this alternation</param>
        /// <param name="majorLine">Major line is every Nth. 0 will disable this alternation</param>
        /// <param name="__headZone">The head zone - the top N rows</param>
        /// <param name="__footZone">The foot zone - the last N rows</param>
        /// <param name="__headZoneExtension">The head zone extension are N rows after headZone</param>
        /// <param name="__footZoneExtension">The foot zone extension are N rows before footZone</param>
        /// <param name="__leftZone">The left zone is N columns from left</param>
        /// <param name="__rightZone">The right zone is N columns from right</param>
        public cursorVariator(selectRangeArea __area, Int32 minorLine = 5, Int32 majorLine = 0, Int32 __headZone = 1, Int32 __footZone = 1, Int32 __headZoneExtension = 0, Int32 __footZoneExtension = 0, Int32 __leftZone = 0, Int32 __rightZone = 0)
        {
            _minor = minorLine;
            _major = majorLine;
            _headZone = __footZone;
            _footZone = __footZone;
            _leftZone = __leftZone;
            _headZoneExtension = __headZoneExtension;
            _footZoneExtension = __footZoneExtension;
            _rightZone = __rightZone;
            _area = __area;
        }

        public cursorVariator()
        {
            // _area = __area;
        }

        /// <summary>
        /// Sets from flags.
        /// </summary>
        /// <param name="flags">The flags.</param>
        public void setFromFlags(cursorVariatorOddEvenFlags flags)
        {
            if ((flags & cursorVariatorOddEvenFlags.doOddEven) == cursorVariatorOddEvenFlags.doOddEven)
            {
                doOdd = true;
            }

            _minor = 0;

            if ((flags & cursorVariatorOddEvenFlags.doMinorOn5) == cursorVariatorOddEvenFlags.doMinorOn5)
            {
                _minor += 5;
            }
            if ((flags & cursorVariatorOddEvenFlags.doMinorOn10) == cursorVariatorOddEvenFlags.doMinorOn10)
            {
                _minor += 10;
            }
            if ((flags & cursorVariatorOddEvenFlags.doMinorOn20) == cursorVariatorOddEvenFlags.doMinorOn20)
            {
                _minor += 20;
            }

            _major = 0;

            if ((flags & cursorVariatorOddEvenFlags.doMajorOn2Minor) == cursorVariatorOddEvenFlags.doMajorOn2Minor)
            {
                _major += _minor * 2;
            }

            if ((flags & cursorVariatorOddEvenFlags.doMajorOn5Minor) == cursorVariatorOddEvenFlags.doMajorOn5Minor)
            {
                _major += _minor * 5;
            }

            if ((flags & cursorVariatorOddEvenFlags.doMajorOn10Minor) == cursorVariatorOddEvenFlags.doMajorOn10Minor)
            {
                _major += _minor * 10;
            }

            if ((flags & cursorVariatorOddEvenFlags.doMajorOn20Minor) == cursorVariatorOddEvenFlags.doMajorOn20Minor)
            {
                _major += _minor * 20;
            }
        }

        /// <summary>
        /// Sets Head and foot flags
        /// </summary>
        /// <param name="flags">The flags.</param>
        public void setFromFlags(cursorVariatorHeadFootFlags flags)
        {
            if ((flags & cursorVariatorHeadFootFlags.doFootZone) == cursorVariatorHeadFootFlags.doFootZone)
            {
                _footZone = 1;
            }

            if ((flags & cursorVariatorHeadFootFlags.doFootZoneBig) == cursorVariatorHeadFootFlags.doFootZoneBig)
            {
                _footZone = 2;
            }

            if ((flags & cursorVariatorHeadFootFlags.doFootExtendedZone) == cursorVariatorHeadFootFlags.doFootExtendedZone)
            {
                _footZoneExtension = 1;
            }

            if ((flags & cursorVariatorHeadFootFlags.doHeadZone) == cursorVariatorHeadFootFlags.doHeadZone)
            {
                _headZone = 1;
            }

            if ((flags & cursorVariatorHeadFootFlags.doHeadZoneBig) == cursorVariatorHeadFootFlags.doHeadZoneBig)
            {
                _headZone = 2;
            }

            if ((flags & cursorVariatorHeadFootFlags.doHeadExtenedZone) == cursorVariatorHeadFootFlags.doHeadExtenedZone)
            {
                _headZoneExtension = 1;
            }

            if ((flags & cursorVariatorHeadFootFlags.doLeftZone) == cursorVariatorHeadFootFlags.doLeftZone)
            {
                _leftZone = 1;
            }

            if ((flags & cursorVariatorHeadFootFlags.doRightZone) == cursorVariatorHeadFootFlags.doRightZone)
            {
                _rightZone = 1;
            }
        }

        /// <summary>
        /// Sets the color use.
        /// </summary>
        /// <param name="output">The output.</param>
        public void setColorUse(cursorVariatorState output)
        {
            if (output.isHeadZone || output.isFootZone)
            {
                output.useInvertedForeground = true;
                output.useLayoutPalette = true;
                output.useColorIndex = (Int32)colors.acePaletteVariationRole.header;
                return;
            }
            if (output.isHeadZoneExtended || output.isFootZoneExtended)
            {
                output.useInvertedForeground = true;
                output.useLayoutPalette = true;
                if (output.isOdd)
                {
                    output.useColorIndex = (Int32)colors.acePaletteVariationRole.odd;
                }
                else
                {
                    output.useColorIndex = (Int32)colors.acePaletteVariationRole.even;
                }
                return;
            }
            if (output.isLeftZone || output.isRightZone)
            {
                output.useLayoutPalette = true;
                output.useInvertedForeground = true;
                output.useColorIndex = (Int32)colors.acePaletteVariationRole.heading;
                return;
            }

            output.useLayoutPalette = false;
            output.useInvertedForeground = false;
            output.useColorIndex = (Int32)colors.acePaletteVariationRole.normal;

            if (output.isOdd)
            {
                output.useColorIndex = (Int32)colors.acePaletteVariationRole.odd;
            }
            else if (output.isEven)
            {
                output.useColorIndex = (Int32)colors.acePaletteVariationRole.even;
            }

            if (output.isMinor)
            {
                output.useColorIndex++;
            }
            if (output.isMajor)
            {
                output.useColorIndex++;
                output.useColorIndex++;
            }
        }

        private cursorVariatorState _lastState;

        /// <summary>
        /// Result of last <see cref="state(int, int)"/> call
        /// </summary>
        public cursorVariatorState lastState
        {
            get { return _lastState; }
            protected set { _lastState = value; }
        }

        /// <summary>
        /// Returns state of speficied position according to <c>area</c>, <c>headZone</c>, <c>footZone</c> and other parameters.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public cursorVariatorState state(Int32 x, Int32 y)
        {
            cursorVariatorState output = new zone.cursorVariatorState();
            output.x = x;
            output.y = y;

            if (area == null)
            {
                throw new NotSupportedException("area can't be null! cursorVariator");
            }

            output.isInside = area.isInside(x, y);
            output.isOutside = !area.isInsideOrEdge(x, y);

            Int32 dX = x - area.x;

            Int32 dY = y - area.y;

            output.isFirst = (dY == area.TopLeft.y);

            output.isLast = (dY == area.BottomRight.y);

            if (output.isInside)
            {
                output.isHeadZone = area.isNearToCorner(x, y, textCursorZoneCorner.Top, headZone);

                if (!output.isHeadZone) output.isHeadZoneExtended = area.isNearToCorner(x, y, textCursorZoneCorner.Top, headZone + headZoneExtension);

                output.isFootZone = area.isNearToCorner(x, y, textCursorZoneCorner.Bottom, footZone);

                if (!output.isFootZone) output.isFootZoneExtended = area.isNearToCorner(x, y, textCursorZoneCorner.Bottom, footZone + footZoneExtension);

                output.isLeftZone = area.isNearToCorner(x, y, textCursorZoneCorner.Left, leftZone);
                output.isRightZone = area.isNearToCorner(x, y, textCursorZoneCorner.Right, rightZone);

                if (doOdd) output.isOdd = (dY % 2 != 0);
                if (doOdd) output.isEven = !output.isOdd;

                if (minor > 0) output.isMinor = (dY % minor == 0);
                if (major > 0) output.isMajor = (dY % major == 0);
            }
            else
            {
            }

            lastState = output;
            setColorUse(output);

            return output;
        }
    }
}