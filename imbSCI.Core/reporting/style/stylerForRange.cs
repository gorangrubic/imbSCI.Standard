// --------------------------------------------------------------------------------------------------------------------
// <copyright file="stylerForRange.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.style
{
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    /// <summary>
    /// Base of range stylers.
    /// </summary>
    /// <remarks>
    /// Range stylers are applied to <c>selectRangeArea</c>
    /// </remarks>
    /// <seealso cref="cursorVariator" />
    public class stylerForRange : cursorVariator
    {
        /// <summary>
        /// Setups for section.
        /// </summary>
        /// <param name="_theme">The theme.</param>
        /// <param name="area">The area.</param>
        /// <param name="loadPreset">if set to <c>true</c> [load preset].</param>
        public void setupForSection(styleTheme _theme, selectRangeArea area, Boolean loadPreset = false)
        {
            // _area = new selectRangeArea(cur.x, cur.y, cur.x + 1, data.Keys.Count + cur.y);
            if (loadPreset) loadPresetForSection();
            _headZone = area.expand(textCursorZoneCorner.Top, doInsertHeader);
            _leftZone = area.expand(textCursorZoneCorner.Left, doInsertRowId);
            _footZone = area.expand(textCursorZoneCorner.Bottom, doInsertFooter);
        }

        public void setupForList(IEnumerable<Object> items, cursor cur, String title, String footer, Boolean loadPreset = false)
        {
            _area = new selectRangeArea(cur.x, cur.y, cur.x + 1, items.count() + cur.y);
            if (loadPreset) loadPresetForSourceLines();

            if (!title.isNullOrEmpty()) _headZone = area.expand(textCursorZoneCorner.Top, doInsertHeader);
            _leftZone = area.expand(textCursorZoneCorner.Left, doInsertRowId);
            if (!footer.isNullOrEmpty()) _footZone = area.expand(textCursorZoneCorner.Bottom, doInsertFooter);
        }

        //public void setupForList(IEnumerable<Object> items, String title, String footer, Boolean loadPreset = false)
        //{
        //}

        /// <summary>
        /// Setups for variables.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="cur">The current.</param>
        /// <param name="loadPreset">if set to <c>true</c> [load preset].</param>
        public void setupForVariables(PropertyCollection data, cursor cur, String title, String footer, String between, Boolean loadPreset = false)
        {
            if (loadPreset) loadPresetForPairs();
            _area = new selectRangeArea(cur.x, cur.y, cur.x + 2, data.Keys.Count + cur.y);

            if (doBetweenPairs) area.expand(textCursorZoneCorner.Right, 1);

            _leftZone = 1;

            if (!title.isNullOrEmpty()) _headZone = area.expand(textCursorZoneCorner.Top, doInsertHeader);

            if (!footer.isNullOrEmpty()) _footZone = area.expand(textCursorZoneCorner.Bottom, doInsertFooter);

            //_footZoneExtension = area.expand(textCursorZoneCorner.Bottom, doInsertColumnFooter);
            if (!between.isNullOrEmpty()) _leftZone = area.expand(textCursorZoneCorner.Left, 1);
            _leftZone += area.expand(textCursorZoneCorner.Left, doInsertRowId);

            _rightZone = area.expand(textCursorZoneCorner.Right, doInsertMinorMajor);
        }

        /// <summary>
        /// Setups for DataTable
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="cur">The current.</param>
        /// <param name="loadPreset">if set to <c>true</c> [load preset].</param>
        public void setupForTable(DataTable table, cursor cur, Boolean loadPreset = false)
        {
            if (loadPreset) loadPresetForTable();
            _area = new selectRangeArea(cur.x, cur.y, table.Columns.Count + cur.x, table.Rows.Count + cur.y);

            _headZoneExtension = area.expand(textCursorZoneCorner.Top, doInsertColumnHeading);
            _headZone = area.expand(textCursorZoneCorner.Top, doInsertHeader);

            _footZone = area.expand(textCursorZoneCorner.Bottom, doInsertFooter);

            _footZoneExtension = area.expand(textCursorZoneCorner.Bottom, doInsertColumnFooter);
            _leftZone = area.expand(textCursorZoneCorner.Left, doInsertRowId);
            _rightZone = area.expand(textCursorZoneCorner.Right, doInsertMinorMajor);

            _area = area;
        }

        public void loadPresetFrom(PropertyCollection preset)
        {
            preset.setToObject(this, "style_");
        }

        /// <summary>
        /// Saves the current settings into collection
        /// </summary>
        /// <param name="preset">The preset.</param>
        public void savePresetTo(PropertyCollection preset)
        {
            preset.buildPropertyCollection<PropertyCollection>(false, true, "style_", preset);
        }

        /// <summary>
        ///
        /// </summary>
        public static Int32 ST = 1;

        /// <summary>
        /// Gets area that matches this position.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="completeArea">if set to <c>true</c> returns complete area not sliced part</param>
        /// <returns></returns>
        public selectRangeArea getArea(Int32 x, Int32 y, Boolean completeArea = false)
        {
            appendRole rol = getAppendRole(state(x, y));
            return getAreaOfRole(rol, x, y, completeArea);
        }

        /// <summary>
        /// Gets area for style application with merge on
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public selectRangeArea getAreaOfRole(appendRole role, Int32 x, Int32 y, Boolean completeArea = false)
        {
            selectRangeArea output = new selectRangeArea(x, y, ST, ST);

            if (completeArea)
            {
                switch (role)
                {
                    case appendRole.sectionHead:
                        role = appendRole.mergedHead;
                        break;

                    case appendRole.sectionContent:
                        role = appendRole.mergedContent;
                        break;

                    case appendRole.sectionFoot:
                        role = appendRole.mergedFoot;
                        break;

                    case appendRole.tableColumnHead:
                        output.reset(area.x, area.TopLeft.y + headZone, area.width, area.TopLeft.y + headZone + headZone);
                        return output;
                        break;

                    case appendRole.tableColumnFoot:

                        output.reset(area.x, area.BottomRight.y - footZone - footZoneExtension, area.width, area.BottomRight.y - footZone);
                        return output;
                        break;

                    case appendRole.tableCellValue:
                    case appendRole.tableCellAnnotation:
                    case appendRole.tableCellNovalue:
                    case appendRole.tableBetween:
                        role = appendRole.mergedContent;
                        break;
                }
            }

            switch (role)
            {
                case appendRole.none:
                    break;

                case appendRole.i_container:
                    return area;
                    break;

                case appendRole.mergedHead:
                    output.reset(area.TopLeft.x, area.TopLeft.y, area.width, headZone);
                    break;

                case appendRole.mergedContent:
                    output.reset(area.TopLeft.x + leftZone, area.BottomRight.y + headZone, area.width - leftZone - rightZone, area.height - footZone - headZone - headZoneExtension - footZoneExtension);
                    break;

                case appendRole.mergedFoot:
                    output.reset(area.TopLeft.x, area.BottomRight.y - footZone, area.width, footZone);
                    break;

                case appendRole.tableHead:
                    output.reset(area.TopLeft.x, area.TopLeft.y, area.width, headZone);
                    break;

                case appendRole.tableColumnHead:
                    output.reset(x, area.TopLeft.y + headZone, ST, area.TopLeft.y + headZone + headZone);
                    break;

                case appendRole.tableColumnFoot:

                    output.reset(x, area.BottomRight.y - footZone - footZoneExtension, ST, area.BottomRight.y - footZone);

                    break;

                case appendRole.tableCellValue:
                case appendRole.tableCellAnnotation:
                case appendRole.tableCellNovalue:
                case appendRole.tableBetween:
                    output.reset(x, y, ST, ST);
                    break;

                case appendRole.tableFoot:
                    output.reset(area.TopLeft.x, area.BottomRight.y - footZone, area.width, footZone);
                    break;

                case appendRole.sectionHead:
                    output.reset(x, area.TopLeft.y, ST, headZone);
                    break;

                case appendRole.sectionContent:
                    output.reset(x, y, ST, ST);
                    break;

                case appendRole.sectionFoot:
                    output.reset(x, area.BottomRight.y, ST, footZone);
                    break;

                default:
                    output.reset(x, y, ST, ST);
                    break;
                    //throw new ArgumentOutOfRangeException();
            }
            return output;
        }

        //public selectRangeArea getRowArea(Int32 y)
        //{
        //    cursorVariatorState currentState = state(area.x, y);
        //    appendRole role = getAppendRole(currentState);

        //    return output;
        //}

        /// <summary>
        /// Provides complete <see cref="styleShotSet"/> according to x,y coordinates and desired <see cref="aceCommonTypes.enums.appendType"/>.
        /// </summary>
        /// <param name="x">X (horizontal) position relative to <c>selectRangeArea</c> 0,0 point.</param>
        /// <param name="y">Y (vertical) position relative to <c>selectRangeArea</c> 0,0 point.</param>
        /// <param name="type">If <c>appendType.none</c> it will automatically find the best match</param>
        /// <returns></returns>
        public styleShotSet getStyleShot(Int32 x, Int32 y, appendType type = appendType.none)
        {
            cursorVariatorState currentState = state(x, y);

            appendRole role = getAppendRole(currentState);

            if (type == appendType.none)
            {
                throw new NotSupportedException("appendType is set to none!");
                //type = role.convertRoleToType();
            }

            acePaletteRole colorToUse = mainPaletteRole;
            if (currentState.useLayoutPalette) colorToUse = layoutPaletteRole;

            styleShotSet output = new styleShotSet(role, type, (acePaletteVariationRole)currentState.useColorIndex, colorToUse, currentState.useInvertedForeground, theme);

            return output;
        }

        #region --- isDisabled ------- when styler is disabled it resolves always into appendRole.normal

        private Boolean _isDisabled = false;

        /// <summary>
        /// when styler is disabled it resolves always into appendRole.normal
        /// </summary>
        public Boolean isDisabled
        {
            get
            {
                return _isDisabled;
            }
            set
            {
                _isDisabled = value;
                OnPropertyChanged("isDisabled");
            }
        }

        #endregion --- isDisabled ------- when styler is disabled it resolves always into appendRole.normal

        #region -----------  tableOptionFlags  -------  [Extra table options]

        private appendTableOptionFlags _tableOptionFlags = appendTableOptionFlags.footMearged | appendTableOptionFlags.topHeadMerged; // = new appendTableOptionFlags();

        /// <summary>
        /// Extra table options
        /// </summary>
        // [XmlIgnore]
        [Category("stylerForRangeBase")]
        [DisplayName("tableOptionFlags")]
        [Description("Extra table options")]
        public appendTableOptionFlags tableOptionFlags
        {
            get
            {
                return _tableOptionFlags;
            }
            set
            {
                // Boolean chg = (_tableOptionFlags != value);
                _tableOptionFlags = value;
                OnPropertyChanged("tableOptionFlags");
                // if (chg) {}
            }
        }

        #endregion -----------  tableOptionFlags  -------  [Extra table options]

        public void setOptiopFlags(appendTableOptionFlags __tableOptionFlags)
        {
            tableOptionFlags = __tableOptionFlags;
        }

        #region --- headFootFlags ------- flags for headFoot variationsy

        private cursorVariatorHeadFootFlags _headFootFlags
            = cursorVariatorHeadFootFlags.doHeadZone | cursorVariatorHeadFootFlags.doLeftZone |
            cursorVariatorHeadFootFlags.doFootZone | cursorVariatorHeadFootFlags.doRightZone |
            cursorVariatorHeadFootFlags.addTableNameHeader | cursorVariatorHeadFootFlags.addTableDescFooter | cursorVariatorHeadFootFlags.addColumnDescForFoot | cursorVariatorHeadFootFlags.addRowNumberOnMajor | cursorVariatorHeadFootFlags.addRowNumberOnMajor;

        /// <summary>
        /// flags for headFoot variationsy
        /// </summary>
        public cursorVariatorHeadFootFlags headFootFlags
        {
            get
            {
                return _headFootFlags;
            }
            set
            {
                _headFootFlags = value;
                OnPropertyChanged("headFootFlags");
            }
        }

        #endregion --- headFootFlags ------- flags for headFoot variationsy

        #region --- oddEvenFlags ------- flags for oddEven

        private cursorVariatorOddEvenFlags _oddEvenFlags = cursorVariatorOddEvenFlags.doMinorOn5 | cursorVariatorOddEvenFlags.doMajorOn5Minor | cursorVariatorOddEvenFlags.doOddEven;

        /// <summary>
        /// flags for oddEven
        /// </summary>
        public cursorVariatorOddEvenFlags oddEvenFlags
        {
            get
            {
                return _oddEvenFlags;
            }
            set
            {
                _oddEvenFlags = value;
                OnPropertyChanged("oddEvenFlags");
            }
        }

        #endregion --- oddEvenFlags ------- flags for oddEven

        public stylerForRange(styleTheme _theme, PropertyCollection data, cursor cur)
        {
            theme = _theme;
            setupForVariables(data, cur, data.getAndRemove("dsa_title", "").toStringSafe(), data.getAndRemove("dsa_footer", "").toStringSafe(), "", true);
        }

        public stylerForRange(styleTheme _theme, DataTable table, cursor cur)
        {
            theme = _theme;
            setupForTable(table, cur);
        }

        /// <summary>
        /// Unconfigured instance <see cref="stylerForRange"/> class.
        /// </summary>
        /// <param name="_theme">The theme.</param>
        public stylerForRange(styleTheme _theme) : base()
        {
            theme = _theme;
        }

        private stylerForRangePresetEnum _activePreset = stylerForRangePresetEnum.none;

        /// <summary>
        ///
        ///
        /// </summary>
        public stylerForRangePresetEnum activePreset
        {
            get { return _activePreset; }
            set { _activePreset = value; }
        }

        public void loadPresetForTable()
        {
            activePreset = stylerForRangePresetEnum.table;
            headFootFlags = cursorVariatorHeadFootFlags.doHeadZone | cursorVariatorHeadFootFlags.doFootZone | cursorVariatorHeadFootFlags.doFootExtendedZone
            | cursorVariatorHeadFootFlags.doHeadExtenedZone | cursorVariatorHeadFootFlags.addTableNameHeader | cursorVariatorHeadFootFlags.addTableDescFooter
            | cursorVariatorHeadFootFlags.addRowNumberOnMajor | cursorVariatorHeadFootFlags.addRowNumberOnMinor | cursorVariatorHeadFootFlags.doLeftZone;

            oddEvenFlags = cursorVariatorOddEvenFlags.doOddEven | cursorVariatorOddEvenFlags.doMinorOn5 | cursorVariatorOddEvenFlags.doMajorOn2Minor;

            tableOptionFlags = appendTableOptionFlags.footMearged | appendTableOptionFlags.topHeadMerged | appendTableOptionFlags.topHeadFullWidth
            | appendTableOptionFlags.footFullWidth | appendTableOptionFlags.footAlignmentCenter | appendTableOptionFlags.topHeadAlignmentCenter | appendTableOptionFlags.addRowNumberOnLeft;

            container = new styleFourSide();
        }

        public void loadPresetForPairs()
        {
            activePreset = stylerForRangePresetEnum.pairs;
            headFootFlags = cursorVariatorHeadFootFlags.doHeadZone | cursorVariatorHeadFootFlags.doFootZone | cursorVariatorHeadFootFlags.addTableNameHeader | cursorVariatorHeadFootFlags.addTableDescFooter;

            oddEvenFlags = cursorVariatorOddEvenFlags.doOddEven | cursorVariatorOddEvenFlags.doMinorOn5 | cursorVariatorOddEvenFlags.doMajorOn2Minor;

            tableOptionFlags = appendTableOptionFlags.footMearged | appendTableOptionFlags.topHeadMerged | appendTableOptionFlags.topHeadFullWidth
            | appendTableOptionFlags.footFullWidth | appendTableOptionFlags.footAlignmentCenter | appendTableOptionFlags.topHeadAlignmentCenter | appendTableOptionFlags.useBetween;

            container = new styleFourSide();
        }

        public void loadPresetForSection()
        {
            activePreset = stylerForRangePresetEnum.section;
            headFootFlags = cursorVariatorHeadFootFlags.doHeadZone | cursorVariatorHeadFootFlags.doFootZone | cursorVariatorHeadFootFlags.addTableNameHeader | cursorVariatorHeadFootFlags.addTableDescFooter;

            oddEvenFlags = cursorVariatorOddEvenFlags.none;

            tableOptionFlags = appendTableOptionFlags.footMearged | appendTableOptionFlags.topHeadMerged | appendTableOptionFlags.topHeadFullWidth
            | appendTableOptionFlags.footFullWidth | appendTableOptionFlags.footAlignmentCenter | appendTableOptionFlags.topHeadAlignmentCenter;

            container = new styleFourSide();
        }

        public void loadPresetForSourceLines()
        {
            activePreset = stylerForRangePresetEnum.sourcelines;
            headFootFlags = cursorVariatorHeadFootFlags.doHeadZone | cursorVariatorHeadFootFlags.doFootZone | cursorVariatorHeadFootFlags.addTableNameHeader
                | cursorVariatorHeadFootFlags.addTableDescFooter | cursorVariatorHeadFootFlags.addRowNumberOnMajor | cursorVariatorHeadFootFlags.addRowNumberOnMinor;

            oddEvenFlags = cursorVariatorOddEvenFlags.doOddEven | cursorVariatorOddEvenFlags.doMinorOn5 | cursorVariatorOddEvenFlags.doMajorOn2Minor;

            tableOptionFlags = appendTableOptionFlags.footMearged | appendTableOptionFlags.topHeadMerged | appendTableOptionFlags.topHeadFullWidth
            | appendTableOptionFlags.footFullWidth | appendTableOptionFlags.footAlignmentCenter | appendTableOptionFlags.topHeadAlignmentCenter | appendTableOptionFlags.addRowNumberOnLeft;

            container = new styleFourSide();
        }

        #region --- mainPaletteRole ------- Currently applied main palette role

        private acePaletteRole _mainPaletteRole = acePaletteRole.colorA;

        /// <summary>
        /// Currently applied main palette role
        /// </summary>
        public acePaletteRole mainPaletteRole
        {
            get
            {
                return _mainPaletteRole;
            }
            set
            {
                if (value != acePaletteRole.none)
                {
                    _mainPaletteRole = value;
                    OnPropertyChanged("mainPaletteRole");
                }
            }
        }

        #endregion --- mainPaletteRole ------- Currently applied main palette role

        #region --- layoutPaletteRole ------- Currently applied layout palette role

        private acePaletteRole _layoutPaletteRole = acePaletteRole.colorB;

        /// <summary>
        /// Currently applied layout palette role
        /// </summary>
        public acePaletteRole layoutPaletteRole
        {
            get
            {
                return _layoutPaletteRole;
            }
            set
            {
                if (value != acePaletteRole.none)
                {
                    _layoutPaletteRole = value;
                    OnPropertyChanged("layoutPaletteRole");
                }
            }
        }

        #endregion --- layoutPaletteRole ------- Currently applied layout palette role

        //  protected acePaletteRole layoutPaletteRole = acePaletteRole.colorA;

        protected styleTheme theme;

        protected styleFourSide container = new styleFourSide();

        public virtual appendType getAppendType(Int32 x, Int32 y)
        {
            appendType output = appendType.regular;

            throw new NotImplementedException();

            // output = getAppendRole(state(x, y)).convertRoleToType();
            return output;
        }

        /// <summary>
        /// Gets the append role recommandation according to x and y position
        /// </summary>
        /// <returns></returns>
        public virtual appendRole getAppendRole(cursorVariatorState state)
        {
            if (state.isHeadZone)
            {
                if (tableOptionFlags.HasFlag(appendTableOptionFlags.topHeadMerged))
                {
                    return appendRole.mergedHead;
                }
                else
                {
                    return appendRole.sectionHead;
                }
            }

            if (state.isHeadZoneExtended)
            {
                return appendRole.tableColumnHead;
            }

            if (state.isFootZone)
            {
                if (tableOptionFlags.HasFlag(appendTableOptionFlags.footMearged))
                {
                    return appendRole.mergedFoot;
                }
                else
                {
                    return appendRole.sectionHead;
                }
            }

            if (state.isLeftZone)
            {
                if (state.isHeadZone)
                {
                    return appendRole.tableCellNovalue;
                }

                if (state.isHeadZoneExtended)
                {
                    return appendRole.tableColumnHead;
                }
                return appendRole.tableColumnHead;
            }
            else if (state.isRightZone)
            {
                if (state.isHeadZone)
                {
                    return appendRole.tableCellNovalue;
                }

                if (state.isHeadZoneExtended)
                {
                    return appendRole.tableColumnHead;
                }

                if (state.isMajor)
                {
                    return appendRole.tableBetween;
                }

                if (state.isMinor)
                {
                    return appendRole.tableBetween;
                }

                return appendRole.none;
            }

            return appendRole.tableCellValue;
        }

        // ------------------------------------------- Boolean Do tests

        public Boolean doOddEven
        {
            get
            {
                return oddEvenFlags.HasFlag(cursorVariatorOddEvenFlags.doOddEven);
            }
        }

        public Boolean doInsertHeader
        {
            get
            {
                return headFootFlags.HasFlag(cursorVariatorHeadFootFlags.addTableNameHeader);
            }
        }

        public Boolean doInsertFooter
        {
            get
            {
                return headFootFlags.HasFlag(cursorVariatorHeadFootFlags.addTableDescFooter);
            }
        }

        public Boolean doInsertMinorMajor
        {
            get
            {
                return headFootFlags.HasFlag(cursorVariatorHeadFootFlags.doRightZone) || headFootFlags.HasFlag(cursorVariatorHeadFootFlags.addRowNumberOnMajor) || headFootFlags.HasFlag(cursorVariatorHeadFootFlags.addRowNumberOnMinor);
            }
        }

        public Boolean doInsertMinorRowOnRight
        {
            get
            {
                return headFootFlags.HasFlag(cursorVariatorHeadFootFlags.doRightZone) || headFootFlags.HasFlag(cursorVariatorHeadFootFlags.addRowNumberOnMinor);
            }
        }

        public Boolean doInsertMajorRowOnRight
        {
            get
            {
                return headFootFlags.HasFlag(cursorVariatorHeadFootFlags.doRightZone) || headFootFlags.HasFlag(cursorVariatorHeadFootFlags.addRowNumberOnMajor);
            }
        }

        public Boolean doInsertRowIdOnRight
        {
            get
            {
                return doInsertMinorRowOnRight || doInsertMajorRowOnRight;
            }
        }

        public Boolean doInsertRowId
        {
            get
            {
                return tableOptionFlags.HasFlag(appendTableOptionFlags.addRowNumberOnLeft);
            }
        }

        public Boolean doInsertColumnHeading
        {
            get
            {
                return headFootFlags.HasFlag(cursorVariatorHeadFootFlags.doHeadExtenedZone);
            }
        }

        public Boolean doInsertColumnFooter
        {
            get
            {
                return headFootFlags.HasFlag(cursorVariatorHeadFootFlags.addColumnDescForFoot) || headFootFlags.HasFlag(cursorVariatorHeadFootFlags.addColumnTypeForFoot);
            }
        }

        public Boolean doBetweenPairs
        {
            get
            {
                return tableOptionFlags.HasFlag(appendTableOptionFlags.useBetween);
            }
        }
    }
}