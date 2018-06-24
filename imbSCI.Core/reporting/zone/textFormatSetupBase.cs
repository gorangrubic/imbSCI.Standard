// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textFormatSetupBase.cs" company="imbVeles" >
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
    using imbSCI.Core.reporting.geometrics;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Block formatting base
    /// </summary>
    public abstract class textFormatSetupBase : selectRangeArea, INotifyPropertyChanged
    {
        /// <summary>
        /// Konstruktor za dvodimenziono podesavanje
        /// </summary>
        /// <param name="__width"></param>
        /// <param name="__height"></param>
        /// <param name="__leftRightMargin"></param>
        /// <param name="__topBottomMargin"></param>
        /// <param name="__leftRightPadding"></param>
        /// <param name="__topBottomPadding"></param>
        public textFormatSetupBase(Int32 __width, Int32 __height, Int32 __leftRightMargin, Int32 __topBottomMargin, Int32 __leftRightPadding, Int32 __topBottomPadding) : base(__width, __height)
        {
            //width = __width;
            //height = __height;
            margin = new fourSideSetting(__leftRightMargin, __topBottomMargin);
            padding = new fourSideSetting(__leftRightPadding, __topBottomPadding);
            autoHeight(true, 1);
        }

        public textFormatSetupBase() : base(85, 43)
        {
        }

        /// <summary>
        /// konstruktor za jednolinijsko podesavanje
        /// </summary>
        /// <param name="__width"></param>
        /// <param name="__leftRightMargin"></param>
        /// <param name="__leftRightPadding"></param>
        public textFormatSetupBase(Int32 __width, Int32 __leftRightMargin, Int32 __leftRightPadding) : base(__width, 1)
        {
            _width = __width;
            _margin = new fourSideSetting(__leftRightMargin, 0);
            _padding = new fourSideSetting(__leftRightPadding, 0);
            autoHeight(true, 1);
        }

        /// <summary>
        /// postavlja automatsku visinu
        /// </summary>
        /// <param name="ifLess"></param>
        /// <param name="contentHeight"></param>
        protected void autoHeight(Boolean ifLess, Int32 contentHeight = 1)
        {
            Int32 mHeight = margin.topAndBottom + padding.topAndBottom + contentHeight;
            if (!ifLess)
            {
                height = mHeight;
                return;
            }
            else
            {
                if (height < mHeight)
                {
                    height = mHeight;
                }
            }
        }

        /// <summary>
        /// Width - padding - margin: sirina u koju se upisuje sadrzaj
        /// </summary>
        public Int32 innerWidth
        {
            get
            {
                Int32 output = width - padding.leftAndRight - margin.leftAndRight;
                if (output < 1) output = 0;
                return output;
            }
        }

        /// <summary>
        /// Pozicija od koje pocinje upisivanje sadrzaja
        /// </summary>
        public Int32 innerLeftPosition
        {
            get
            {
                Int32 output = padding.left + margin.left;
                return output;
            }
        }

        /// <summary>
        /// Pozicija do koje se upisuje sadrzaj (width - padding.right, margin.right)
        /// </summary>
        public Int32 innerRightPosition
        {
            get
            {
                Int32 output = width - (padding.right + margin.right);
                return output;
            }
        }

        /// <summary>
        /// Pozicija za leve strane - marget.left
        /// </summary>
        public Int32 innerBoxedLeftPosition
        {
            get
            {
                Int32 output = margin.left;
                return output;
            }
        }

        /// <summary>
        /// Pozicija na desnoj strani - witdh - margin.right
        /// </summary>
        public Int32 innerBoxedRightPosition
        {
            get
            {
                Int32 output = width - (margin.right);
                return output;
            }
        }

        /// <summary>
        /// Sirina na kojoj se ispisuje pozadina (width-margin)
        /// </summary>
        public Int32 innerBoxedWidth
        {
            get
            {
                Int32 output = width - margin.leftAndRight;
                if (output < 0) output = 0;
                return output;
            }
        }

        /// <summary>
        /// Visina na kojoj se ispisuje sadrzaj> height - padding - margin
        /// </summary>
        public Int32 innerHeight
        {
            get
            {
                Int32 output = height - padding.topAndBottom - margin.topAndBottom;
                if (output < 0) output = 0;
                return output;
            }
        }

        /// <summary>
        /// Visina na kojoj se ispisuje pozadina> height - margin
        /// </summary>
        public Int32 innerBoxedHeight
        {
            get
            {
                Int32 output = height - margin.topAndBottom;
                if (output < 0) output = 0;
                return output;
            }
        }

        /// <summary>
        /// Prva vertikalna pozicija u kojoj moze da pise sadrzaj
        /// </summary>
        public Int32 innerTopPosition
        {
            get
            {
                Int32 output = margin.top + padding.top;
                return output;
            }
        }

        /// <summary>
        /// Krajnja vertikalna pozicija na kojoj moze da pise sadrzaj
        /// </summary>
        public Int32 innerBottomPosition
        {
            get
            {
                Int32 output = height - (padding.bottom + margin.bottom);
                return output;
            }
        }

        /// <summary>
        /// Prva vertikalna pozicija - bez paddinga
        /// </summary>
        public Int32 innerBoxedTopPosition
        {
            get
            {
                Int32 output = margin.top;
                return output;
            }
        }

        /// <summary>
        /// Krajnja vertikalna pozicija na kojoj moze da pise sadrzaj
        /// </summary>
        public Int32 innerBoxedBottomPosition
        {
            get
            {
                Int32 output = height - (margin.bottom);
                return output;
            }
        }

        #region -----------  margin  -------  [margina za liniju koja se ne boji pozadinskim sablonom]

        private fourSideSetting _margin = new fourSideSetting(0, 0); // = new fourSideSetting();

        /// <summary>
        /// margina za liniju koja se ne boji pozadinskim sablonom
        /// </summary>
        // [XmlIgnore]
        [Category("textLineContent")]
        [DisplayName("margin")]
        [Description("margina za liniju koja se ne boji pozadinskim sablonom")]
        public fourSideSetting margin
        {
            get
            {
                return _margin;
            }
            set
            {
                // Boolean chg = (_margin != value);
                _margin = value;
                OnPropertyChanged("margin");
                // if (chg) {}
            }
        }

        #endregion -----------  margin  -------  [margina za liniju koja se ne boji pozadinskim sablonom]

        #region --- padding ------- padding za koji se sadrzaj odvaja od ivice pozadne

        private fourSideSetting _padding = new fourSideSetting(0, 0);

        /// <summary>
        /// padding za koji se sadrzaj odvaja od ivice pozadne
        /// </summary>
        public fourSideSetting padding
        {
            get
            {
                return _padding;
            }
            set
            {
                _padding = value;
                OnPropertyChanged("padding");
            }
        }

        #endregion --- padding ------- padding za koji se sadrzaj odvaja od ivice pozadne

        /// <summary>
        /// Pozicija na kojoj se zavrsava sav sadrzaj ovog bloka: Y+margin+padding+innerHeight
        /// </summary>
        public int outerBottomPosition
        {
            get
            {
                Int32 output = 0;
                output += margin.top + innerBoxedHeight + margin.bottom;
                return output;
            }
        }

        /// <summary>
        /// Pozicija sa sve marginom> x+margin.left+padding.left+innerWidth+padding.right+margin.right
        /// </summary>
        public int outerRightPosition
        {
            get
            {
                Int32 output = 0;
                output += margin.leftAndRight + innerBoxedWidth;
                return output;
            }
        }

        /// <summary>
        /// Left position for specified zone.
        /// </summary>
        /// <param name="currentZone">Zone to test against</param>
        /// <returns>Requested position</returns>
        public Int32 left(textCursorZone currentZone)
        {
            switch (currentZone)
            {
                case textCursorZone.innerZone:
                    return innerLeftPosition;
                    break;

                case textCursorZone.innerBoxedZone:
                    return innerBoxedLeftPosition;
                    break;

                default:
                case textCursorZone.outterZone:
                    return 0;
                    break;
            }
        }

        /// <summary>
        /// Right position for specified zone.
        /// </summary>
        /// <param name="currentZone">Zone to test against</param>
        /// <returns>Requested position</returns>
        public Int32 right(textCursorZone currentZone)
        {
            switch (currentZone)
            {
                case textCursorZone.innerZone:
                    return innerRightPosition;
                    break;

                case textCursorZone.innerBoxedZone:
                    return innerBoxedRightPosition;
                    break;

                default:
                case textCursorZone.outterZone:
                    return outerRightPosition;
                    break;
            }
        }

        /// <summary>
        /// Top position for specified zone.
        /// </summary>
        /// <param name="currentZone">Zone to test against</param>
        /// <returns>Requested position</returns>
        public Int32 top(textCursorZone currentZone)
        {
            switch (currentZone)
            {
                case textCursorZone.innerZone:
                    return innerTopPosition;
                    break;

                case textCursorZone.innerBoxedZone:
                    return innerBoxedTopPosition;
                    break;

                default:
                case textCursorZone.outterZone:
                    return 0;
                    break;
            }
        }

        /// <summary>
        /// Bottom position for specified zone.
        /// </summary>
        /// <param name="currentZone">Zone to test against</param>
        /// <returns>Requested position</returns>
        public Int32 bottom(textCursorZone currentZone)
        {
            switch (currentZone)
            {
                case textCursorZone.innerZone:
                    return innerBottomPosition;
                    break;

                case textCursorZone.innerBoxedZone:
                    return innerBoxedBottomPosition;
                    break;

                default:
                case textCursorZone.outterZone:
                    return outerBottomPosition;
                    break;
            }
        }

        public Int32 getWidth(textCursorZone currentZone)
        {
            switch (currentZone)
            {
                case textCursorZone.innerZone:
                    return innerRightPosition - innerLeftPosition;
                    break;

                case textCursorZone.innerBoxedZone:
                    return innerBoxedRightPosition - innerBoxedLeftPosition;
                    break;

                default:
                case textCursorZone.outterZone:
                    return outerRightPosition;
                    break;
            }
        }

        /// <summary>
        /// Heights the specified current zone.
        /// </summary>
        /// <param name="currentZone">The current zone.</param>
        /// <returns></returns>
        public Int32 getHeight(textCursorZone currentZone)
        {
            switch (currentZone)
            {
                case textCursorZone.innerZone:
                    return innerBottomPosition - innerTopPosition;
                    break;

                case textCursorZone.innerBoxedZone:
                    return innerBoxedBottomPosition - innerBoxedTopPosition;
                    break;

                default:
                case textCursorZone.outterZone:
                    return outerBottomPosition;
                    break;
            }
        }
    }
}