// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cursor.cs" company="imbVeles" >
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
    using System;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Universal 2D cursor for text and data navigation
    /// </summary>
    /// \ingroup_disabled report_ll_zone
    public class cursor : selectRange, INotifyPropertyChanged
    {
        public String ToString()
        {
            return "X:" + x.ToString("D4") + " Y:" + y.ToString("D4") + " in [" + frame.x.ToString("D4") + ":" + frame.y.ToString("D4") + "/" + frame.width.ToString("D4") + "x" + frame.height.ToString("D4") + "]";
        }

        private String _name = "c";

        /// <summary>
        /// sole purpose of name is for easier diagnostics
        /// </summary>
        public String name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        /// <summary>
        /// Active frame
        /// </summary>
        /// <value>
        /// Active frame
        /// </value>
        //protected cursorZone _frame = null;
        protected cursorZone frame
        {
            get
            {
                if (tempFrame == null) useTempFrame = false;
                if (useTempFrame)
                {
                    return tempFrame;
                }
                return mainFrame;
            }
            set
            {
                if (useTempFrame)
                {
                    tempFrame = value;
                }
                mainFrame = value;
            }
        }

        /// <summary>
        /// Returns TRUE if currently the cursor uses temporary frame not the main
        /// </summary>
        // [XmlIgnore]
        [Category("cursor")]
        [DisplayName("isTempFrameActive")]
        [Description("Returns TRUE if currently the cursor uses temporary frame not the main")]
        public Boolean isTempFrameActive
        {
            get
            {
                return useTempFrame;
            }
        }

        /// <summary>
        /// The use temporary frame
        /// </summary>
        protected Boolean useTempFrame = false;

        /// <summary>
        /// The main frame
        /// </summary>
        protected cursorZone mainFrame = null;

        /// <summary>
        /// The temporary frame -- when set
        /// </summary>
        protected cursorZone tempFrame = null;

        /// <summary>
        /// The mode
        /// </summary>
        protected textCursorMode mode = textCursorMode.fixedZone;

        #region --- currentZone -------

        private textCursorZone _currentZone = textCursorZone.innerZone;

        /// <summary>
        /// Gets or sets the current zone.
        /// </summary>
        /// <value>
        /// The current zone.
        /// </value>
        protected textCursorZone currentZone
        {
            get
            {
                return _currentZone;
            }
            set
            {
                if (mainZone == textCursorZone.unknownZone)
                {
                    if (_currentZone != textCursorZone.unknownZone)
                    {
                        //if (value != currentZone)
                        mainZone = _currentZone;
                    }
                    else
                    {
                    }
                }
                _currentZone = value;
                OnPropertyChanged("currentZone");
            }
        }

        #endregion --- currentZone -------

        #region --- valueReadZone ------- snimljena pozicija na ekranu odakle treba kasnije da se iscitava unos sa tastature

        private selectZone _valueReadZone;

        /// <summary>
        /// snimljena pozicija na ekranu odakle treba kasnije da se iscitava unos sa tastature
        /// </summary>
        /// <value>
        /// The value read zone.
        /// </value>
        public selectZone valueReadZone
        {
            get
            {
                if (!_valueReadZone.isDefined) _valueReadZone = autoValueReadZone();
                return _valueReadZone;
            }
            set
            {
                _valueReadZone = value;
                OnPropertyChanged("valueReadZone");
            }
        }

        #endregion --- valueReadZone ------- snimljena pozicija na ekranu odakle treba kasnije da se iscitava unos sa tastature

        /// <summary>
        /// Automatski pravi value read zonu na mestu gde je stao kursor
        /// </summary>
        /// <returns></returns>
        protected selectZone autoValueReadZone()
        {
            var sr = selectToCorner(textCursorZoneCorner.Right);
            selectZone output = new selectZone(x, y, sr.x, 1);
            return output;
        }

        #region ----------- Boolean [ doPivotTranslation ] -------  [If TRUE cursor will rotate orientation so X is vertical and Y is horisontal]

        private Boolean _doPivotTranslation = false;

        /// <summary>
        /// If TRUE cursor will rotate orientation so X is vertical and Y is horisontal
        /// </summary>
        /// <value>
        ///   <c>true</c> if [do pivot translation]; otherwise, <c>false</c>.
        /// </value>
        [Category("Switches")]
        [DisplayName("doPivotTranslation")]
        [Description("If TRUE cursor will rotate orientation so X is vertical and Y is horisontal")]
        public Boolean doPivotTranslation
        {
            get { return _doPivotTranslation; }
            set { _doPivotTranslation = value; OnPropertyChanged("doPivotTranslation"); }
        }

        #endregion ----------- Boolean [ doPivotTranslation ] -------  [If TRUE cursor will rotate orientation so X is vertical and Y is horisontal]

        // -------------- PENCIL ------------- //

        #region -----------  pencil  -------  [Defines relative zone affected by background brushing, value writing, merge call, value delete or other spatial operation]

        private selectRangeArea _pencil = new selectRangeArea(0, 0, 1, 1);

        /// <summary>
        /// Defines zone affected by background brushing, value writing, merge call, value delete or other spatial operation
        /// </summary>
        // [XmlIgnore]
        [Category("cursor")]
        [DisplayName("pencil")]
        [Description("Defines zone affected by background brushing, value writing, merge call, value delete or other spatial operation")]
        protected selectRangeArea pencil
        {
            get
            {
                if (doUseDefaultPencil) return pencilDefault;

                return _pencil;
            }
            set
            {
                // Boolean chg = (_pencil != value);
                _pencil = value;
                OnPropertyChanged("pencil");
                // if (chg) {}
            }
        }

        #endregion -----------  pencil  -------  [Defines relative zone affected by background brushing, value writing, merge call, value delete or other spatial operation]

        /// <summary>
        /// Gets the pencil projected on current cursor position
        /// </summary>
        /// <value>
        /// Result is trimmed accprding to the current zone within the frame
        /// </value>
        public selectRangeArea pencilAbsolute
        {
            get
            {
                if (doPencilWidthToZone && !doUseDefaultPencil)
                {
                    var __tmp = selectToCorner(textCursorZoneCorner.Right);
                    pencil.resize(__tmp.x, -1);
                }
                return projectArea(pencil);
            }
        }

        /// <summary>
        /// Gets the pencil according to <see cref="pencilType"/>
        /// </summary>
        /// <param name="ptype">The ptype.</param>
        /// <returns>area covered by pencil</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public selectRangeArea getPencil(pencilType ptype, Int32 thickness = 0)
        {
            selectRangeArea output = new selectRangeArea(x, y, x, y);
            switch (ptype)
            {
                case pencilType.point:
                    return output;
                    break;

                case pencilType.active:
                    return pencilAbsolute;
                    break;

                case pencilType.useDefault:
                    return projectArea(pencilDefault);
                    break;

                case pencilType.zoneHorizontal:
                    output.reset(frame.left(currentZone), y, frame.getWidth(currentZone), thickness);
                    break;

                case pencilType.zoneVertical:
                    output.reset(x, frame.top(currentZone), thickness, frame.getHeight(currentZone));
                    break;

                case pencilType.zoneArea:
                    output.reset(frame.left(currentZone), frame.top(currentZone), frame.getWidth(currentZone), frame.getHeight(currentZone));
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return output;
        }

        /// <summary>
        /// Projects an area onto current cursor position and returns its crosection with allowed zone frame  area if it colides with current frame zone
        /// </summary>
        /// <param name="area">The area.</param>
        /// <returns>area or part of area within allowed zone frame</returns>
        public selectRangeArea projectArea(selectRangeArea area)
        {
            selectRangeArea output = new selectRangeArea(x, y, x + area.width, y + area.height);

            return frame.selectRangeArea(currentZone).getCrossection(output);
        }

        protected selectRangeArea pencilDefault = new selectRangeArea(0, 0, 1, 1);

        /// <summary>
        /// Default pencil is seen when get called on <c>pencilAbsolute</c>
        /// </summary>
        protected Boolean doUseDefaultPencil = false;

        #region --- doPencilWidthToZone ------- if TRUE it will keep pencil width locked to current zone

        private Boolean _doPencilWidthToZone = false;

        /// <summary>
        /// if TRUE it will keep pencil width locked to current zone
        /// </summary>
        protected Boolean doPencilWidthToZone
        {
            get
            {
                return _doPencilWidthToZone;
            }
            set
            {
                _doPencilWidthToZone = value;
                OnPropertyChanged("doPencilWidthToZone");
            }
        }

        #endregion --- doPencilWidthToZone ------- if TRUE it will keep pencil width locked to current zone

        /// <summary>
        /// Sets pencil options
        /// </summary>
        /// <param name="useDefaultPencil">if set to <c>true</c> pencil will be ignored and default pencil will be returned on get pencilAbsolute.</param>
        /// <param name="pencilWidthToZone">if set to <c>true</c> pencil width will automatically follow width of current zone</param>
        public void setPencil(Boolean useDefaultPencil, Boolean pencilWidthToZone)
        {
            doUseDefaultPencil = useDefaultPencil;
            doPencilWidthToZone = pencilWidthToZone;
        }

        // ------------

        /// <summary>
        /// Pravi selectZone na osnovu sadrzaja trenutne linije. Trazi [ i ] kao granicnike zone. Ako ih ima vise, upotrebice prvi par.
        /// </summary>
        /// <param name="zoneHeight">Height of the zone.</param>
        /// <param name="zoneWidth">Width of the zone.</param>
        /// <param name="option">The option.</param>
        /// <returns>
        /// selectZone objekat koji treba upotrebiti kao inputReadZone ili za nesto drugo
        /// </returns>
        /// <remarks>
        /// Ako u trenutnoj liniji:
        /// - ima vise [ ] parova: upotrebice prvi
        /// - je sve prazno ili popunjeno razmacima: upotrebice celu liniju kao zonu
        /// - ako je od x-a do desnog inner x-a prazno
        /// </remarks>
        public selectZone getSelectZone(Int32 zoneHeight = -1, Int32 zoneWidth = -1, params selectZoneOption[] option)
        {
            var optionList = option.ToList();
            selectZone output = new selectZone();
            output.y = y;

            if (zoneHeight == -1)
            {
                zoneHeight = 1;
                output.height = zoneHeight;
            }

            selectZoneOption takeOption = option.takeFirstFromList(selectZoneOption.takeDefinedWidth,
                                                      selectZoneOption.takeFromPositionToRightEnd,
                                                      selectZoneOption.takeBracetDefinedArea,
                                                      selectZoneOption.takeCompleteLine);

            if (takeOption == selectZoneOption.takeDefinedWidth)
            {
                if (zoneWidth == -1)
                {
                    takeOption = selectZoneOption.takeFromPositionToRightEnd;
                }
            }

            if (takeOption == selectZoneOption.takeBracetDefinedArea)
            {
                //if (target is ISupportsCursorWriteAndSelect)
                //{
                //    ISupportsCursorWriteAndSelect target2 = target as ISupportsCursorWriteAndSelect;
                //    String lastLine = target2.select(this, -1, true);
                //    Int32 si = lastLine.IndexOf("[");

                //    if (si == -1)
                //    {
                //        prevLine();
                //        lastLine = target2.select(this, -1, true);
                //        if (lastLine.IndexOf("[") == -1)
                //        {
                //            nextLine();
                //        } else
                //        {
                //            si = lastLine.IndexOf("[");
                //        }
                //    }

                //    Int32 ln = lastLine.IndexOf("]") - si;
                //    if (ln < 0)
                //    {
                //        takeOption = selectZoneOption.takeFromPositionToRightEnd;
                //    } else
                //    {
                //        output.y = y;
                //        output.x = si;
                //        output.weight = ln;
                //    }
                //}
            }

            switch (takeOption)
            {
                case selectZoneOption.takeFromPositionToRightEnd:
                    output.x = x;
                    output.width = selectToCorner(textCursorZoneCorner.Right).x;
                    break;

                case selectZoneOption.takeCompleteLine:
                    output.x = frame.innerLeftPosition;
                    output.width = frame.innerWidth;
                    break;
            }

            var moveOption = option.takeFirstFromList(selectZoneOption.none,
                                                      selectZoneOption.moveCursorToBottomEndOfZone,
                                                      selectZoneOption.moveCursorToBeginningOfZone);

            switch (moveOption)
            {
                case selectZoneOption.moveCursorToBeginningOfZone:
                    moveLineTo(output.y);
                    moveXTo(output.x);
                    break;

                case selectZoneOption.moveCursorToBottomEndOfZone:
                    moveLineTo(output.y + output.height);
                    break;
            }

            return output;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="cursor"/> class.
        /// </summary>
        /// <param name="__target">The target.</param>
        /// <param name="__mode">The mode.</param>
        /// <param name="__zone">The zone.</param>
        public cursor(cursorZone __target, textCursorMode __mode, textCursorZone __zone, String __name = "c")
        {
            name = __name;
            frame = __target;
            mode = __mode;
            currentZone = __zone;
        }

        /// <summary>
        /// Prebacuje kursor u datu zonu
        /// </summary>
        /// <param name="__zone">The zone.</param>
        /// <param name="__corner">The corner.</param>
        public void switchToZone(textCursorZone __zone, textCursorZoneCorner __corner = textCursorZoneCorner.default_corner)
        {
            if (__corner == textCursorZoneCorner.default_corner) __corner = default_direction;
            currentZone = __zone;
            moveToCorner(__corner);
        }

        /// <summary>
        /// Vraca kursor na njegovu glavnu zonu
        /// </summary>
        public void switchToMainZone()
        {
            if (mainZone == textCursorZone.unknownZone)
            {
                Exception ex =
                    new NotSupportedException("The " + nameof(currentZone) + " zone left undefined");
                //"Main zone je ostao nepoznat - nikada nije doslo do dodeljivanja nove vrednosti currentZone propertiju!");
                throw ex;
            }
            currentZone = mainZone;
        }

        /// <summary>
        /// zona koja je "home" za ovaj kursor - odnosno, zona za koju je ovaj kursor primarno namenjen i u koju se vraca nakon privremenih premestanja u druge zone metodom switchToZone()
        /// </summary>
        protected textCursorZone mainZone = textCursorZone.innerZone;

        /// <summary>
        /// Postavlja child objekat na poziciju kursora
        /// </summary>
        /// <param name="child"></param>
        public void placeChild(ISupportsBasicCursor child)
        {
            child.margin.left = x;
            child.margin.top = y;

            Int32 dMLeft = x - child.innerBoxedLeftPosition;
            Int32 dMTop = y - child.innerBoxedTopPosition;

            child.width = frame.width; // Math.Min(child.width + dMLeft, target.width - x);
            child.height = frame.height; // Math.Min(child.height + dMTop, target.height - y);
        }

        /// <summary>
        /// Sets the temporary frame by using subzone preset
        /// </summary>
        /// <param name="sub">The sub.</param>
        /// <param name="startPosition">The start position.</param>
        /// <returns></returns>
        public cursorZone setTempFrame(cursorSubzoneFrame sub, textCursorZoneCorner startPosition = textCursorZoneCorner.none)
        {
            tempFrame = new cursorZone(this.frame, sub);
            useTempFrame = true;
            moveToCorner(startPosition);
            return tempFrame;
        }

        /// <summary>
        /// Sets the temporary zone by adjusting
        /// </summary>
        /// <param name="rel_width">From current position to <c>rel_width</c>.</param>
        /// <param name="rel_height">Height of the relative<c>rel_height</c></param>
        /// <param name="startPosition">The start position.</param>
        /// <param name="leftRightPadding">The left right padding.</param>
        /// <param name="topBottomPadding">The top bottom padding.</param>
        public cursorZone setTempFrame(Int32 rel_width, Int32 rel_height, textCursorZoneCorner startPosition = textCursorZoneCorner.none, Int32 leftRightPadding = 0, Int32 topBottomPadding = 0)
        {
            tempFrame = new cursorZone(x + rel_width, y + rel_height, x, y, leftRightPadding, topBottomPadding);
            useTempFrame = true;
            moveToCorner(startPosition);
            return tempFrame;
        }

        /// <summary>
        /// Cancels temporal frame if tempFrame was used
        /// </summary>
        /// <param name="startPosition">The start position.</param>
        public void setToParentFrame(textCursorZoneCorner startPosition = textCursorZoneCorner.none)
        {
            if (frame.parent != null)
            {
                frame = frame.parent;
            }
            moveToCorner(startPosition);
        }

        public cursorZone setToSubFrame(cursorZoneRole role, Int32 index = 0, textCursorZoneCorner __corner = textCursorZoneCorner.default_corner)
        {
            cursorZone subframe = frame.subzones[role, index];
            if (subframe != null)
            {
                tempFrame = subframe;
            }
            return subframe;
        }

        public void moveTo(int __x, int __y)
        {
            moveXTo(__x);
            moveLineTo(__y);
        }

        /// <summary>
        /// Cancels temporal frame if tempFrame was used
        /// </summary>
        /// <param name="startPosition">The start position.</param>
        public void backToMainFrame(textCursorZoneCorner startPosition = textCursorZoneCorner.none, Boolean exceptionIfWasInMain = false)
        {
            if ((!isTempFrameActive) && exceptionIfWasInMain) throw new NotSupportedException(nameof(backToMainFrame) + " backToMainZone called but cursor is already in the main frame!");
            tempFrame = null;
            useTempFrame = false;
            moveToCorner(startPosition);
        }

        public selectRangeArea selectZonePoint()
        {
            selectRangeArea output = new selectRangeArea(x, y, x, y);
            return output;
        }

        /// <summary>
        /// Selects the zone area in.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="steps">The steps.</param>
        /// <returns></returns>
        public selectRangeArea selectZoneAreaIn(textCursorZoneCorner direction, Int32 steps = 1)
        {
            Int32 __x = x;
            Int32 __y = y;
            for (int i = 0; i < steps; i++)
            {
                moveInDirection(direction, i);
            }
            selectRangeArea output = new selectRangeArea(__x, __y, x, y);
            moveTo(__x, __y);
            return output;
        }

        /// <summary>
        /// Selects the current zone RangeArea accorting to marigin, pading and ZoneMode
        /// </summary>
        /// <returns>Area struct with coordinates and dimensions</returns>
        public selectRangeArea selectZoneArea()
        {
            Int32 __x = x;
            Int32 __y = y;
            selectRange cursorStart = new selectRange(x, y);
            moveToCorner(textCursorZoneCorner.UpLeft);
            selectRange zoneTopLeft = new selectRange(x, y);
            moveToCorner(textCursorZoneCorner.DownRight);
            x = __x;
            y = __y;
            selectRangeArea output = new selectRangeArea(zoneTopLeft.x, zoneTopLeft.y, x, y);
            return output;
        }

        /// <summary>
        /// Vraca dimenzije trenutne zone
        /// </summary>
        /// <returns></returns>
        public selectRange selectZone()
        {
            Int32 __x = x;
            Int32 __y = y;
            moveToCorner(textCursorZoneCorner.UpLeft);
            var res = selectToCorner(textCursorZoneCorner.DownRight);
            x = __x;
            y = __y;

            return res;
        }

        /// <summary>
        /// Vraca rastojanje izmedju trenutne pozicije kursora i datog kraja/coska
        /// </summary>
        /// <param name="__corner">The corner.</param>
        /// <returns></returns>
        public selectRangeArea selectAreaToCorner(textCursorZoneCorner __corner = textCursorZoneCorner.default_corner)
        {
            if (__corner == textCursorZoneCorner.default_corner) __corner = default_direction;
            Int32 __x = x;
            Int32 __y = y;

            moveToCorner(__corner);
            Int32 _dX = x - __x;
            Int32 _dY = y - __y;
            selectRangeArea res = new selectRangeArea(__x, __y, _dX, _dY);

            x = __x;
            y = __y;

            return res;
        }

        /// <summary>
        /// Vraca rastojanje izmedju trenutne pozicije kursora i datog kraja/coska
        /// </summary>
        /// <param name="__corner">The corner.</param>
        /// <returns></returns>
        public selectRange selectToCorner(textCursorZoneCorner __corner = textCursorZoneCorner.default_corner)
        {
            if (__corner == textCursorZoneCorner.default_corner) __corner = default_direction;
            Int32 __x = x;
            Int32 __y = y;
            moveToCorner(__corner);
            Int32 _dX = x - __x;
            Int32 _dY = y - __y;
            selectRange res = new selectRange(_dX, _dY);

            x = __x;
            y = __y;

            return res;
        }

        /// <summary>
        /// Pomera kursor u dati ugao
        /// </summary>
        /// <param name="__corner">The corner.</param>
        public void moveToCorner(textCursorZoneCorner __corner = textCursorZoneCorner.default_corner)
        {
            if (__corner == textCursorZoneCorner.default_corner) __corner = default_direction;
            switch (__corner)
            {
                case textCursorZoneCorner.Left:
                    x = 0;
                    break;

                case textCursorZoneCorner.Right:
                    x = frame.width;
                    break;

                case textCursorZoneCorner.Top:
                    y = 0;
                    break;

                case textCursorZoneCorner.Bottom:
                    y = frame.height;
                    break;

                case textCursorZoneCorner.UpLeft:
                    y = 0;
                    x = 0;
                    break;

                case textCursorZoneCorner.UpRight:
                    y = 0;
                    x = frame.width;
                    break;

                case textCursorZoneCorner.DownLeft:
                    y = frame.height;
                    x = 0;
                    break;

                case textCursorZoneCorner.DownRight:
                    y = frame.height;
                    x = frame.width;
                    break;
            }
            checkPositions();
        }

        #region -----------  default_direction  -------  [Default movement direction to be applied when .none is sent]

        private textCursorZoneCorner _default_direction = textCursorZoneCorner.default_corner; // = new textCursorZoneCorner();

        /// <summary>
        /// Default movement direction to be applied when .none is sent
        /// </summary>
        /// <value>
        /// The default direction.
        /// </value>
        // [XmlIgnore]
        [Category("cursor")]
        [DisplayName("default_direction")]
        [Description("Default movement direction to be applied when .none is sent")]
        protected textCursorZoneCorner default_direction
        {
            get
            {
                return _default_direction;
            }
            set
            {
                // Boolean chg = (_default_direction != value);
                _default_direction = value;
                OnPropertyChanged("default_direction");
                // if (chg) {}
            }
        }

        #endregion -----------  default_direction  -------  [Default movement direction to be applied when .none is sent]

        /// <summary>
        /// Sets the default direction.
        /// </summary>
        /// <param name="newDefault">The new default.</param>
        public void setDefaultDirection(textCursorZoneCorner newDefault)
        {
            default_direction = newDefault;
        }

        /// <summary>
        /// Pomera kursor u dati ugao
        /// </summary>
        /// <param name="__corner">The corner.</param>
        public void moveInDirection(textCursorZoneCorner __corner = textCursorZoneCorner.default_corner, Int32 step = 1)
        {
            if (__corner == textCursorZoneCorner.default_corner) __corner = default_direction;
            switch (__corner)
            {
                case textCursorZoneCorner.Left:
                    prev(step);
                    break;

                case textCursorZoneCorner.Right:
                    next(step);
                    break;

                case textCursorZoneCorner.Top:
                    prevLine(step);
                    break;

                case textCursorZoneCorner.Bottom:
                    nextLine(step);
                    break;

                case textCursorZoneCorner.UpLeft:
                    moveInDirection(textCursorZoneCorner.Top, step);
                    moveInDirection(textCursorZoneCorner.Left, step);
                    break;

                case textCursorZoneCorner.UpRight:
                    moveInDirection(textCursorZoneCorner.Top, step);
                    moveInDirection(textCursorZoneCorner.Right, step);

                    break;

                case textCursorZoneCorner.DownLeft:
                    moveInDirection(textCursorZoneCorner.Bottom, step);
                    moveInDirection(textCursorZoneCorner.Left, step);
                    break;

                case textCursorZoneCorner.DownRight:
                    moveInDirection(textCursorZoneCorner.Bottom, step);
                    moveInDirection(textCursorZoneCorner.Right, step);
                    break;
            }
            checkPositions();
        }

        /// <summary>
        /// Sets margin at current position using direction parameter
        /// </summary>
        /// <param name="direction">What border of margin to set at this position</param>
        public void setMarginHere(textCursorZoneCorner direction = textCursorZoneCorner.default_corner)
        {
            if (direction == textCursorZoneCorner.default_corner) direction = default_direction;
            switch (direction)
            {
                case textCursorZoneCorner.Left:
                    frame.margin.left = x;
                    break;

                case textCursorZoneCorner.Right:
                    frame.margin.right = frame.outerRightPosition - x;
                    break;

                case textCursorZoneCorner.Top:
                    frame.margin.top = y;
                    break;

                case textCursorZoneCorner.Bottom:
                    frame.margin.top = frame.outerBottomPosition - y;
                    break;

                case textCursorZoneCorner.UpLeft:
                    setMarginHere(textCursorZoneCorner.Top);
                    setMarginHere(textCursorZoneCorner.Left);
                    break;

                case textCursorZoneCorner.UpRight:
                    setMarginHere(textCursorZoneCorner.Top);
                    setMarginHere(textCursorZoneCorner.Right);

                    break;

                case textCursorZoneCorner.DownLeft:
                    setMarginHere(textCursorZoneCorner.Bottom);
                    setMarginHere(textCursorZoneCorner.Left);
                    break;

                case textCursorZoneCorner.DownRight:
                    setMarginHere(textCursorZoneCorner.Bottom);
                    setMarginHere(textCursorZoneCorner.Right);
                    break;
            }
        }

        /// <summary>
        /// proverava poziciju i primenjuje ogranicenje
        /// </summary>
        /// <returns>TRUE if correction was made</returns>
        internal Boolean checkPositions()
        {
            Boolean correctionMade = false;
            switch (currentZone)
            {
                case textCursorZone.innerZone:
                    if (y > frame.innerBottomPosition)
                    {
                        if (mode != textCursorMode.scroll)
                        {
                            y = frame.innerBottomPosition;
                            correctionMade = true;
                        }
                    }
                    if (y < frame.innerTopPosition)
                    {
                        y = frame.innerTopPosition;
                        correctionMade = true;
                    }
                    if (mode != textCursorMode.scroll)
                    {
                        if (x > frame.innerRightPosition)
                        {
                            x = frame.innerRightPosition;
                            correctionMade = true;
                        }
                    }
                    if (x < frame.innerLeftPosition)
                    {
                        x = frame.innerLeftPosition;
                        correctionMade = true;
                    }
                    break;

                case textCursorZone.innerBoxedZone:
                    if (y > frame.innerBoxedBottomPosition)
                    {
                        if (mode != textCursorMode.scroll)
                        {
                            y = frame.innerBoxedBottomPosition;
                            correctionMade = true;
                        }
                    }
                    if (y < frame.innerBoxedTopPosition)
                    {
                        y = frame.innerBoxedTopPosition;
                        correctionMade = true;
                    }
                    if (mode != textCursorMode.scroll)
                    {
                        if (x > frame.innerBoxedRightPosition)
                        {
                            x = frame.innerBoxedRightPosition;
                            correctionMade = true;
                        }
                    }
                    if (x < frame.innerBoxedLeftPosition)
                    {
                        x = frame.innerBoxedLeftPosition;
                        correctionMade = true;
                    }
                    break;

                case textCursorZone.outterZone:
                    if (y > frame.height)
                    {
                        if (mode != textCursorMode.scroll)
                        {
                            y = frame.height;
                            correctionMade = true;
                        }
                    }
                    if (y < 0)
                    {
                        y = 0;
                        correctionMade = true;
                    }

                    if (mode != textCursorMode.scroll)
                    {
                        if (x > frame.width)
                        {
                            x = frame.width;
                            correctionMade = true;
                        }
                    }
                    if (x < 0)
                    {
                        x = 0;
                        correctionMade = true;
                    }
                    break;
            }
            return correctionMade;
        }

        public void toTopLeftCorner()
        {
            x = -1;
            y = -1;
            checkPositions();
        }

        /// <summary>
        /// Moves for specified X and Y. It calls checkPositions() afterwards.
        /// </summary>
        /// <param name="xMovement">The x movement. Above 0 means in Right direction, below 0 means in Left direction</param>
        /// <param name="yMovement">The y movement. Above 0 means Downward, below 0 means Upward</param>
        /// <returns></returns>
        public Boolean moveFor(Int32 xMovement, Int32 yMovement = 0)
        {
            x += xMovement;
            y += yMovement;
            return checkPositions();
        }

        public void next(Boolean breakLine)
        {
            if (breakLine)
            {
                enter();
            }
            else
            {
                next(1);
            }
        }

        /// <summary>
        /// Pomera kursor na lokalnu liniju
        /// </summary>
        /// <param name="zoneLineNumber">The zone line number.</param>
        /// <returns></returns>
        public Boolean moveLineTo(Int32 zoneLineNumber)
        {
            Int32 tY = zoneLineNumber;
            switch (currentZone)
            {
                case textCursorZone.innerZone:
                    tY += frame.innerTopPosition;
                    break;

                case textCursorZone.innerBoxedZone:
                    tY += frame.innerBoxedTopPosition;
                    break;

                case textCursorZone.outterZone:
                    tY += 0;
                    break;
            }

            y = tY;
            return checkPositions();
        }

        public Boolean moveXTo(Int32 zoneXNumber)
        {
            Int32 tX = zoneXNumber;
            switch (currentZone)
            {
                case textCursorZone.innerZone:
                    tX += frame.innerLeftPosition;
                    break;

                case textCursorZone.innerBoxedZone:
                    tX += frame.innerBoxedLeftPosition;
                    break;

                case textCursorZone.outterZone:
                    tX += 0;
                    break;
            }

            x = tX;
            return checkPositions();
        }

        /// <summary>
        /// Moves downwards and places X on left border
        /// </summary>
        /// <returns></returns>
        public Boolean enter()
        {
            moveToCorner(textCursorZoneCorner.Left);
            return nextLine();
        }

        /// <summary>
        /// Moves downwards by step without affecting Y position
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public Boolean nextLine(Int32 step = 1)
        {
            y = y + step;
            return checkPositions();
        }

        /// <summary>
        /// Previouses the line.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <returns></returns>
        public Boolean prevLine(Int32 step = -1)
        {
            y = y + step;
            return checkPositions();
        }

        /// <summary>
        /// Nexts the specified step.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <returns></returns>
        public Boolean next(Int32 step = 1)
        {
            x = x + step;
            return checkPositions();
        }

        /// <summary>
        /// Move X by step on left *negative*
        /// </summary>
        /// <param name="step">Step of movement</param>
        /// <returns></returns>
        public Boolean prev(Int32 step = 1)
        {
            x = x - step;
            return checkPositions();
        }

        //public Boolean tabNext()
        //{
        //    if (x < tabPosition(printHorizontal.left))
        //    {
        //        x = tabPosition(printHorizontal.left);
        //    }
        //    else if (x < tabPosition(printHorizontal.middle))
        //    {
        //        x = tabPosition(printHorizontal.middle);
        //    } else if (x < tabPosition(printHorizontal.right))
        //    {
        //        x = tabPosition(printHorizontal.right);
        //    }
        //    return checkPositions();
        //}

        //public Int32 tabPosition(printHorizontal field)
        //{
        //    ISupportsBasicCursor target_e = target as ISupportsBasicCursor;
        //    switch (field)
        //    {
        //        case printHorizontal.left:
        //            return target.innerLeftPosition;

        //            break;
        //        case printHorizontal.middle:
        //            if (target_e != null)
        //            {
        //                return target_e.leftFieldWidth + target.innerLeftPosition;
        //            }
        //            break;
        //        case printHorizontal.right:
        //            if (target_e != null)
        //            {
        //                return target.innerRightPosition - target_e.rightFieldWidth;
        //            }
        //            break;
        //    }

        //    return 0;
        //}

        //public Boolean tab(printHorizontal field)
        //{
        //    x = tabPosition(field);
        //    return checkPositions();
        //}

        #region --- x ------- X pozicija kursora

        private Int32 _x = 0;

        /// <summary>
        /// X pozicija kursora
        /// </summary>
        public override Int32 x
        {
            get
            {
                if (doPivotTranslation)
                {
                    return _y;
                }
                return _x;
            }
            set
            {
                if (doPivotTranslation)
                {
                    _y = value;
                    OnPropertyChanged("x");
                    return;
                }
                _x = value;
                OnPropertyChanged("x");
            }
        }

        #endregion --- x ------- X pozicija kursora

        #region --- y ------- pozicija po liniji

        private Int32 _y;

        /// <summary>
        /// pozicija po liniji
        /// </summary>
        public Int32 y
        {
            get
            {
                if (doPivotTranslation)
                {
                    return _x;
                }
                return _y;
            }
            set
            {
                if (doPivotTranslation)
                {
                    _x = value;
                }
                else
                {
                    _y = value;
                }

                OnPropertyChanged("y");
            }
        }

        #endregion --- y ------- pozicija po liniji

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}