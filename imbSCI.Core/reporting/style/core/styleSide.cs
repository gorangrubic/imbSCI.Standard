// --------------------------------------------------------------------------------------------------------------------
// <copyright file="styleSide.cs" company="imbVeles" >
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
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Data.data;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    /// <summary>
    /// One side styling> border, pad/margin, color of the border
    /// </summary>
    public class styleSide : imbBindable
    {
        public styleSide(styleSideDirection __direction)
        {
            direction = __direction;
        }

        #region -----------  direction  -------  [direction of this side]

        private styleSideDirection _direction; // = new styleSideDirection();

        /// <summary>
        /// direction of this side
        /// </summary>
        // [XmlIgnore]
        [Category("styleSide")]
        [DisplayName("direction")]
        [Description("direction of this side")]
        public styleSideDirection direction
        {
            get
            {
                return _direction;
            }
            set
            {
                // Boolean chg = (_direction != value);
                _direction = value;
                OnPropertyChanged("direction");
                // if (chg) {}
            }
        }

        #endregion -----------  direction  -------  [direction of this side]

        /// <summary>
        /// Setups the specified color role.
        /// </summary>
        /// <param name="__colorRole">The color role. Unknown for no changes</param>
        /// <param name="__thickness">The thickness. -1 for no changes</param>
        /// <param name="__padding">The padding.-1 for no changes</param>
        /// <param name="__margin">The margin.-1 for no changes</param>
        /// <param name="__type">The type. Unknown for no changes</param>
        public void setup(acePaletteRole __colorRole = acePaletteRole.none, Int32 __thickness = -1, Int32 __padding = -1, Int32 __margin = -1, styleBorderType __type = styleBorderType.unknown)
        {
            if (__colorRole != acePaletteRole.none) borderColor = __colorRole;
            if (__thickness > -1) thickness = __thickness;
            if (__padding > -1) padding = __padding;
            //padding = __padding;
            if (__margin > -1) margin = __margin;
            //margin = __margin;
            if (__type != styleBorderType.unknown) type = __type;
            type = __type;
        }

        public object this[styleFourSideParameter param]
        {
            get
            {
                switch (param)
                {
                    case styleFourSideParameter.borderColor:
                        return borderColor;
                        break;

                    case styleFourSideParameter.padding:
                        return padding;
                        break;

                    case styleFourSideParameter.margin:
                        return margin;
                        break;

                    case styleFourSideParameter.thickness:
                        return thickness;
                        break;

                    case styleFourSideParameter.type:
                        return type;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (param)
                {
                    case styleFourSideParameter.borderColor:
                        borderColor = (acePaletteRole)value;
                        break;

                    case styleFourSideParameter.padding:
                        padding = (Int32)value;
                        break;

                    case styleFourSideParameter.margin:
                        margin = (Int32)value;
                        break;

                    case styleFourSideParameter.thickness:
                        thickness = (Int32)value;
                        break;

                    case styleFourSideParameter.type:
                        type = (styleBorderType)value;

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #region -----------  type  -------  [type of border on this side]

        private styleBorderType _type = styleBorderType.None; // = new styleBorderType();

        /// <summary>
        /// type of border on this side
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        // [XmlIgnore]
        [Category("styleBorderSide")]
        [DisplayName("type")]
        [Description("type of border on this side")]
        public styleBorderType type
        {
            get
            {
                return _type;
            }
            set
            {
                // Boolean chg = (_type != value);
                if (value == styleBorderType.unknown) return;
                _type = value;
                OnPropertyChanged("type");
                // if (chg) {}
            }
        }

        #endregion -----------  type  -------  [type of border on this side]

        public Color borderColorStatic { get; set; } = Color.Black;

        #region -----------  borderColor  -------  [Color of the border]

        private acePaletteRole _borderColor = acePaletteRole.none; // = new Color();

        /// <summary>
        /// Color of the border
        /// </summary>
        // [XmlIgnore]
        [Category("styleBorderSide")]
        [DisplayName("borderColor")]
        [Description("Color of the border")]
        public acePaletteRole borderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                // Boolean chg = (_borderColor != value);
                if (value == acePaletteRole.none) return;
                _borderColor = value;
                OnPropertyChanged("borderColor");
                // if (chg) {}
            }
        }

        #endregion -----------  borderColor  -------  [Color of the border]

        #region -----------  thickness  -------  [thickness of border]

        private Int32 _thickness = 0; // = new Int32();

        /// <summary>
        /// thickness of border
        /// </summary>
        // [XmlIgnore]
        [Category("styleBorderSide")]
        [DisplayName("thickness")]
        [Description("thickness of border")]
        public Int32 thickness
        {
            get
            {
                return _thickness;
            }
            set
            {
                // Boolean chg = (_thickness != value);
                if (value < 0) return;
                _thickness = value;
                OnPropertyChanged("thickness");
                // if (chg) {}
            }
        }

        #endregion -----------  thickness  -------  [thickness of border]

        #region -----------  margin  -------  [ammount of margin]

        private Int32 _margin; // = new Int32();

        /// <summary>
        /// ammount of margin
        /// </summary>
        // [XmlIgnore]
        [Category("styleBorderSide")]
        [DisplayName("margin")]
        [Description("ammount of margin")]
        public Int32 margin
        {
            get
            {
                return _margin;
            }
            set
            {
                // Boolean chg = (_margin != value);
                if (value < 0) return;
                _margin = value;
                OnPropertyChanged("margin");
                // if (chg) {}
            }
        }

        #endregion -----------  margin  -------  [ammount of margin]

        #region -----------  padding  -------  [amount of padding]

        private Int32 _padding; // = new Int32();

        /// <summary>
        /// amount of padding
        /// </summary>
        // [XmlIgnore]
        [Category("styleBorderSide")]
        [DisplayName("padding")]
        [Description("amount of padding")]
        public Int32 padding
        {
            get
            {
                return _padding;
            }
            set
            {
                // Boolean chg = (_padding != value);
                if (value < 0) return;
                _padding = value;
                // if (chg) {}
            }
        }

        #endregion -----------  padding  -------  [amount of padding]
    }
}