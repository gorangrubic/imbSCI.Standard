// --------------------------------------------------------------------------------------------------------------------
// <copyright file="styleFourSide.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.style.core
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.geometrics;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Data;
    using imbSCI.Data.data;
    using imbSCI.Data.enums.appends;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;

    /// <summary>
    /// Complex settings four style
    /// </summary>
    /// <remarks>
    /// if BorderColor is none - it will follow Forecolor value
    /// </remarks>
    /// <seealso cref="aceCommonTypes.primitives.imbBindable" />
    public class styleFourSide : dataBindableBase
    {
        private appendType _type;

        /// <summary>
        ///
        /// </summary>
        public appendType type
        {
            get { return _type; }
            private set { _type = value; }
        }

        public static String getCodeName(appendType __type, styleTheme theme)
        {
            return imbSciStringExtensions.add(__type.ToString(), theme.getCodeName(), "-");
        }

        /// <summary>
        ///
        /// </summary>
        protected String themeCodeName
        {
            get { return _themeCodeName; }
            private set { _themeCodeName = value; }
        }

        private String _themeCodeName = "";
        private String codeName = "";

        /// <summary>
        /// Gets code name of the object. CodeName should be unique per each unique set of values of properties. In other words: if two instances of the same class have different CodeName that means values of their key properties are not same.
        /// </summary>
        /// <returns>
        /// Unique string to identify unique values
        /// </returns>
        public String getCodeName()
        {
            if (!imbSciStringExtensions.isNullOrEmptyString(codeName)) return codeName;
            String output = "";

            output = imbSciStringExtensions.add(type.ToString(), themeCodeName, "-");
            codeName = output;
            return output;
        }

        public styleFourSide()
        {
        }

        public styleFourSide(styleTheme _theme, appendType __type)
        {
            type = __type;
            theme = _theme;
            themeCodeName = _theme.getCodeName();
            processType(__type);
        }

        internal void processType(appendType __type)
        {
            type = __type;

            switch (type)
            {
                case appendType.c_table:
                    setup(0, 25, acePaletteRole.colorDefault, 1, styleBorderType.Thin, styleSideDirection.left, styleSideDirection.right, styleSideDirection.top);
                    setup(0, 25, acePaletteRole.colorDefault, 2, styleBorderType.Double, styleSideDirection.bottom);
                    break;

                case appendType.c_section:
                    setup(0, 25, acePaletteRole.colorDefault, 2, styleBorderType.Thin, styleSideDirection.left);
                    break;

                default:
                    break;
            }
        }

        public void setup(Int32 __padding, Int32 __margin, Color __borderColor, Int32 __thickness, styleBorderType __type, params styleSideDirection[] directions)
        {
            List<styleSideDirection> sides = directions.getFlatList<styleSideDirection>();
            if (sides.Count == 0)
            {
                sides.AddMultiple(styleSideDirection.top, styleSideDirection.right, styleSideDirection.left, styleSideDirection.bottom);
            }

            foreach (styleSideDirection side in sides)
            {
                this[side].borderColorStatic = __borderColor;
                this[side].padding = __padding;
                this[side].margin = __margin;
                this[side].thickness = __thickness;
                this[side].type = __type;
            }
        }

        /// <summary>
        /// Setups the specified padding.
        /// </summary>
        /// <param name="__padding">The padding. -1 to ignore</param>
        /// <param name="__margin">The margin. -1 to ignore</param>
        /// <param name="__borderColor">Color of the border. none to ignore</param>
        /// <param name="__thickness">The thickness. -1 to ignore</param>
        /// <param name="__type">The type. unknown to ignore</param>
        /// <param name="directions">The directions.</param>
        public void setup(Int32 __padding, Int32 __margin, acePaletteRole __borderColor, Int32 __thickness, styleBorderType __type, params styleSideDirection[] directions)
        {
            List<styleSideDirection> sides = directions.getFlatList<styleSideDirection>();
            if (sides.Count == 0)
            {
                sides.AddMultiple(styleSideDirection.top, styleSideDirection.right, styleSideDirection.left, styleSideDirection.bottom);
            }

            foreach (styleSideDirection side in sides)
            {
                this[side].borderColor = __borderColor;
                this[side].padding = __padding;
                this[side].margin = __margin;
                this[side].thickness = __thickness;
                this[side].type = __type;
            }
        }

        /// <summary>
        /// Applies the specified <c>fourSideSetting</c> into <c>target</c> property for all four sides
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="target">The target.</param>
        public void apply(fourSideSetting setting, styleFourSideParameter target)
        {
            this[styleSideDirection.bottom, target] = setting.bottom;
            this[styleSideDirection.top, target] = setting.top;
            this[styleSideDirection.left, target] = setting.left;
            this[styleSideDirection.right, target] = setting.right;
        }

        public void CopySideSettingsTo(styleFourSide target)
        {
            target[styleSideDirection.bottom] = bottom;
            target[styleSideDirection.top] = top;
            target[styleSideDirection.left] = left;
            target[styleSideDirection.right] = right;
        }

        public Object this[styleSideDirection side, styleFourSideParameter param]
        {
            get
            {
                styleSide sd = this[side];
                return sd[param];
            }
            set
            {
                styleSide sd = this[side];
                sd[param] = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="styleSide"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="styleSide"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public styleSide this[styleSideDirection key]
        {
            get
            {
                switch (key)
                {
                    case styleSideDirection.bottom:
                        return bottom;
                        break;

                    case styleSideDirection.left:
                        return left;
                        break;

                    case styleSideDirection.right:
                        return right;
                        break;

                    case styleSideDirection.top:
                        return top;
                        break;
                }
                return left;
            }
            set
            {
                switch (key)
                {
                    case styleSideDirection.bottom:
                        bottom = value;
                        break;

                    case styleSideDirection.left:
                        left = value;
                        break;

                    case styleSideDirection.right:
                        right = value;
                        break;

                    case styleSideDirection.top:
                        top = value;
                        break;
                }
            }
        }

        #region ----------- Boolean [ doVerticalSymetry ] -------  [it will keep synchronized top and bottom sides]

        private Boolean _doVerticalSymetry = false;

        /// <summary>
        /// it will keep synchronized top and bottom sides
        /// </summary>
        [Category("Switches")]
        [DisplayName("doVerticalSymetry")]
        [Description("it will keep synchronized top and bottom sides")]
        protected Boolean doVerticalSymetry
        {
            get { return _doVerticalSymetry; }
            set { _doVerticalSymetry = value; OnPropertyChanged("doVerticalSymetry"); }
        }

        #endregion ----------- Boolean [ doVerticalSymetry ] -------  [it will keep synchronized top and bottom sides]

        #region ----------- Boolean [ doHorizontalSymetry ] -------  [it will keep synchronized left and right]

        private Boolean _doHorizontalSymetry = false;

        /// <summary>
        /// it will keep synchronized left and right
        /// </summary>
        [Category("Switches")]
        [DisplayName("doHorizontalSymetry")]
        [Description("it will keep synchronized left and right")]
        protected Boolean doHorizontalSymetry
        {
            get { return _doHorizontalSymetry; }
            set { _doHorizontalSymetry = value; OnPropertyChanged("doHorizontalSymetry"); }
        }

        #endregion ----------- Boolean [ doHorizontalSymetry ] -------  [it will keep synchronized left and right]

        #region --- right ------- Bindable property

        private styleSide _right = new styleSide(styleSideDirection.right);

        /// <summary>
        /// Bindable property
        /// </summary>
        public styleSide right
        {
            get
            {
                if (doHorizontalSymetry) return _left;
                return _right;
            }
            set
            {
                if (doHorizontalSymetry)
                {
                    _left = value;
                    OnPropertyChanged("left");
                }
                else
                {
                    _right = value;
                }

                OnPropertyChanged("right");
            }
        }

        #endregion --- right ------- Bindable property

        #region --- left ------- Bindable property

        private styleSide _left = new styleSide(styleSideDirection.left);

        /// <summary>
        /// Bindable property
        /// </summary>
        public styleSide left
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;
                OnPropertyChanged("left");
            }
        }

        #endregion --- left ------- Bindable property

        #region --- bottom ------- Bindable property

        private styleSide _bottom = new styleSide(styleSideDirection.bottom);

        /// <summary>
        /// Bindable property
        /// </summary>
        public styleSide bottom
        {
            get
            {
                if (doHorizontalSymetry)
                {
                    return _top;
                }
                return _bottom;
            }
            set
            {
                if (doHorizontalSymetry)
                {
                    _top = value;
                    OnPropertyChanged("top");
                }
                else
                {
                    _bottom = value;
                }
                OnPropertyChanged("bottom");
            }
        }

        #endregion --- bottom ------- Bindable property

        #region --- top ------- Bindable property

        private styleSide _top = new styleSide(styleSideDirection.top);
        private styleTheme theme;

        /// <summary>
        /// Bindable property
        /// </summary>
        public styleSide top
        {
            get
            {
                return _top;
            }
            set
            {
                _top = value;
                OnPropertyChanged("top");
            }
        }

        #endregion --- top ------- Bindable property
    }
}