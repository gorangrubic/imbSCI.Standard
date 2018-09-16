// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cursorZone.cs" company="imbVeles" >
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
    using imbSCI.Data.interfaces;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Zone within cursor is allowed to move. Has 3 ranges: inner, boxed and outter
    /// </summary>
    /// \ingroup_disabled report_ll_zone
    public class cursorZone : cursorZoneSpatialSettings, ISupportsBasicCursor, IGetCodeName
    {
        public cursorPositionResponse GetPosition(cursor lineCursor, Int32 tolerance = 1)
        {
            if (isNearToCorner(lineCursor.x, lineCursor.y, textCursorZoneCorner.Top, tolerance))
            {
                return cursorPositionResponse.atBeginning;
            }
            else if (isNearToCorner(lineCursor.x, lineCursor.y, textCursorZoneCorner.Bottom, tolerance))
            {
                return cursorPositionResponse.atEnd;
            }

            return cursorPositionResponse.somewhereWithin;
        }

        /// <summary>
        /// Gets code name of the object. CodeName should be unique per each unique set of values of properties. In other words: if two instances of the same class have different CodeName that means values of their key properties are not same.
        /// </summary>
        /// <returns>
        /// Unique string to identify unique values
        /// </returns>
        public string getCodeName()
        {
            String output = "";

            return output;
        }

        #region -----------  Zone structure

        private cursorZoneCollection _subzones = new cursorZoneCollection();

        /// <summary>
        /// Zones within this zone. Master zone is automatically set by constructor
        /// </summary>
        public cursorZoneCollection subzones
        {
            get { return _subzones; }
            set { _subzones = value; }
        }

        private cursorZone _parent; // = new cursorZone();

        /// <summary>
        /// Reference to the parent zone. if null this is a master zone
        /// </summary>
        // [XmlIgnore]
        [Category("cursorZone")]
        [DisplayName("parent")]
        [Description("Reference to the parent zone. if null this is a master zone")]
        public cursorZone parent
        {
            get
            {
                return _parent;
            }
            set
            {
                // Boolean chg = (_parent != value);
                _parent = value;
                OnPropertyChanged("parent");
                // if (chg) {}
            }
        }

        #endregion -----------  Zone structure

        /// <summary>
        /// Selects the range area of space allocated within this zone, respecting what sub area type is specified
        /// </summary>
        /// <param name="subAreaType">Type of the sub area.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public selectRangeArea selectRangeArea(textCursorZone subAreaType)
        {
            switch (subAreaType)
            {
                case textCursorZone.unknownZone:
                case textCursorZone.innerZone:
                    return new zone.selectRangeArea(innerLeftPosition, innerTopPosition, innerRightPosition, innerBottomPosition);
                    break;

                case textCursorZone.innerBoxedZone:
                    return new zone.selectRangeArea(innerBoxedLeftPosition, innerBoxedTopPosition, innerBoxedRightPosition, innerBoxedBottomPosition);
                    break;

                case textCursorZone.outterZone:
                    return new zone.selectRangeArea(0, 0, outerRightPosition, outerBottomPosition);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new zone.selectRangeArea(0, 0, outerRightPosition, outerBottomPosition);
        }

        #region -----------  spatialUnit  -------  [Describes how big is one 'character' / position space of cursor]

        private selectRange _spatialUnit = new selectRange(1, 1);

        /// <summary>
        /// Describes how big is one 'character' / position space of cursor - 2D
        /// </summary>
        // [XmlIgnore]
        [Category("cursorZone")]
        [DisplayName("spatialUnit")]
        [Description("Describes how big is one 'character' / position space of cursor")]
        public selectRange spatialUnit
        {
            get
            {
                return _spatialUnit;
            }
            set
            {
                // Boolean chg = (_spatialUnit != value);
                _spatialUnit = value;
                OnPropertyChanged("spatialUnit");
                // if (chg) {}
            }
        }

        #endregion -----------  spatialUnit  -------  [Describes how big is one 'character' / position space of cursor]

        /// <summary>
        /// Zone without any space within. Dimensions must be set later
        /// </summary>
        public cursorZone() : base()
        {
            subzones.Add(cursorZoneRole.master, this);
        }

        /// <summary>
        /// Subzone frame by preset <see cref="cursorZone"/>
        /// </summary>
        /// <param name="sourceZone">The source zone.</param>
        /// <param name="sz">Subzone preset</param>
        public cursorZone(cursorZone sourceZone, cursorSubzoneFrame sz) : base(sourceZone.width, sourceZone.margin.leftAndRight, sourceZone.padding.leftAndRight)
        {
            margin.Learn(sourceZone.margin);
            padding.Learn(sourceZone.padding);

            height = sourceZone.height;
            switch (sz)
            {
                case cursorSubzoneFrame.fullFrame:
                    width = sourceZone.width;
                    break;

                case cursorSubzoneFrame.h1:
                    width = sourceZone.width / 2;
                    break;

                case cursorSubzoneFrame.h2:
                    margin.left = sourceZone.width / 2;
                    width = sourceZone.width - margin.left;
                    break;

                case cursorSubzoneFrame.q1:
                    width = sourceZone.width / 4;
                    break;

                case cursorSubzoneFrame.q2:
                    width = sourceZone.width / 4;
                    margin.left = width;
                    break;

                case cursorSubzoneFrame.q3:
                    width = sourceZone.width / 4;
                    margin.left = width * 2;
                    break;

                case cursorSubzoneFrame.q4:
                    width = sourceZone.width / 4;
                    margin.left = sourceZone.width - width;
                    break;
            }
        }

        /// <summary>
        /// Konstruktor za dvodimenziono podesavanje
        /// </summary>
        /// <param name="__width"></param>
        /// <param name="__height"></param>
        /// <param name="__leftRightMargin"></param>
        /// <param name="__topBottomMargin"></param>
        /// <param name="__leftRightPadding"></param>
        /// <param name="__topBottomPadding"></param>
        public cursorZone(int __width, int __height, int __leftRightMargin, int __topBottomMargin, int __leftRightPadding, int __topBottomPadding) : base(__width, __height, __leftRightMargin, __topBottomMargin, __leftRightPadding, __topBottomPadding)
        {
            subzones.Add(cursorZoneRole.master, this);
        }

        /// <summary>
        /// konstruktor za jednolinijsko podesavanje
        /// </summary>
        /// <param name="__width"></param>
        /// <param name="__leftRightMargin"></param>
        /// <param name="__leftRightPadding"></param>
        public cursorZone(int __width, int __leftRightMargin, int __leftRightPadding) : base(__width, __leftRightMargin, __leftRightPadding)
        {
            subzones.Add(cursorZoneRole.master, this);
        }
    }
}