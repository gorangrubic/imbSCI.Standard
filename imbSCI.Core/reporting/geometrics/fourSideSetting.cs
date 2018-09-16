// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fourSideSetting.cs" company="imbVeles" >
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
    using imbSCI.Data;
    using imbSCI.Data.enums.reporting;
    using System;
    using System.ComponentModel;
    using System.Text;

    /// <summary>
    /// Struktura koja sadrzi> top, bottom, left i right podesavanje
    /// </summary>
    public class fourSideSetting : INotifyPropertyChanged, ICloneable
    {
        public static implicit operator fourSideSetting(Int32 a)
        {
            return new fourSideSetting(a, a);
        }

        public override string ToString()
        {
            return String.Format("{0}px {1}px {2}px {3}px", top, right, bottom, left);
        }

        /// <summary>
        /// Make CSS ready string
        /// </summary>
        /// <param name="name">property name to be used as prefix in front of -top, -right...</param>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString(String name, styleToStringFormat format)
        {
            switch (format)
            {
                default:
                case styleToStringFormat.inlineCss:
                    return imbSciStringExtensions.add(name, imbSciStringExtensions.ensureEndsWith(ToString(), ";"), ": ");
                    break;

                case styleToStringFormat.multilineCss:
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(imbSciStringExtensions.add(imbSciStringExtensions.add(imbSciStringExtensions.add(name, "top: ", "-"), top.ToString(), " "), "px", "") + ";");
                    sb.AppendLine(imbSciStringExtensions.add(imbSciStringExtensions.add(imbSciStringExtensions.add(name, "right: ", "-"), right.ToString(), " "), "px", "") + ";");
                    sb.AppendLine(imbSciStringExtensions.add(imbSciStringExtensions.add(imbSciStringExtensions.add(name, "bottom: ", "-"), bottom.ToString(), " "), "px", "") + ";");
                    sb.AppendLine(imbSciStringExtensions.add(imbSciStringExtensions.add(imbSciStringExtensions.add(name, "left: ", "-"), left.ToString(), " "), "px", "") + ";");
                    return sb.ToString();
                    break;
            }

            return "";
        }

        public void snapAllTo(Int32 target = 0)
        {
            if (top < target) top = target;
            if (bottom < target) bottom = target;
            if (left < target) left = target;
            if (right < target) right = target;
        }

        /// <summary>
        /// Changes all value for __change and upplies minimum limit on all four
        /// </summary>
        /// <param name="__change"></param>
        /// <param name="limit">Minimum value for all sides</param>
        /// <returns></returns>
        public fourSideSetting getResized(Int32 __change, Int32 limit = 0)
        {
            var nleft = left + __change;
            var nright = right + __change;
            var ntop = top + __change;
            var nbottom = bottom + __change;
            fourSideSetting output = new fourSideSetting(nleft, ntop, nright, nbottom);
            output.snapAllTo(limit);
            return output;
        }

        public fourSideSetting getResized(Int32 __left, Int32 __top, Int32 __right, Int32 __bottom, Int32 limit = 0)
        {
            var nleft = left + __left;
            var nright = right + __right;
            var ntop = top + __top;
            var nbottom = bottom + __bottom;
            fourSideSetting output = new fourSideSetting(nleft, ntop, nright, nbottom);
            output.snapAllTo(limit);
            return output;
        }

        public fourSideSetting(Int32 __left, Int32 __top, Int32 __right, Int32 __bottom)
        {
            left = __left;
            top = __top;
            right = __right;
            bottom = __bottom;
        }

        /// <summary>
        /// Equal value on all four sides <see cref="fourSideSetting"/> class.
        /// </summary>
        /// <param name="input">The input.</param>
        public fourSideSetting(Int32 input)
        {
            top = input;
            bottom = input;
            left = input;
            right = input;
        }

        public fourSideSetting(Int32 __leftAndRight, Int32 __topAndBottom)
        {
            top = __topAndBottom;
            bottom = __topAndBottom;
            left = __leftAndRight;
            right = __leftAndRight;
        }

        #region -----------  top  -------  [count / ratio]

        private Int32 _top = 0; // = new Int32();

        /// <summary>
        /// count / ratio
        /// </summary>
        // [XmlIgnore]
        [Category("Counters")]
        [DisplayName("top")]
        [Description("count / ratio")]
        public Int32 top
        {
            get
            {
                if (_top < 0)
                {
                }
                return _top;
            }
            set
            {
                _top = value;
                OnPropertyChanged("top");
            }
        }

        #endregion -----------  top  -------  [count / ratio]

        #region -----------  left  -------  [count / ratio]

        private Int32 _left = 0; // = new Int32();

        /// <summary>
        /// count / ratio
        /// </summary>
        // [XmlIgnore]
        [Category("Counters")]
        [DisplayName("left")]
        [Description("count / ratio")]
        public Int32 left
        {
            get
            {
                if (_left < 0)
                {
                }
                return _left;
            }
            set
            {
                _left = value;
                OnPropertyChanged("left");
            }
        }

        #endregion -----------  left  -------  [count / ratio]

        #region -----------  right  -------  [count / ratio]

        private Int32 _right = 0; // = new Int32();

        /// <summary>
        /// count / ratio
        /// </summary>
        // [XmlIgnore]
        [Category("Counters")]
        [DisplayName("right")]
        [Description("count / ratio")]
        public Int32 right
        {
            get
            {
                return _right;
            }
            set
            {
                _right = value;
                OnPropertyChanged("right");
            }
        }

        #endregion -----------  right  -------  [count / ratio]

        #region -----------  bottom  -------  [count / ratio]

        private Int32 _bottom = 0; // = new Int32();

        /// <summary>
        /// count / ratio
        /// </summary>
        // [XmlIgnore]
        [Category("Counters")]
        [DisplayName("bottom")]
        [Description("count / ratio")]
        public Int32 bottom
        {
            get
            {
                return _bottom;
            }
            set
            {
                _bottom = value;
                OnPropertyChanged("bottom");
            }
        }

        #endregion -----------  bottom  -------  [count / ratio]

        public Int32 leftAndRight
        {
            get { return left + right; }
        }

        public Int32 topAndBottom
        {
            get { return top + bottom; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public object Clone()
        {
            return new fourSideSetting(left, top, right, bottom);
        }

        public void Learn(object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "Source is null. Can-t learn from null source!");
            }
            fourSideSetting s = source as fourSideSetting;

            left = s.left;
            right = s.right;
            top = s.top;
            bottom = s.bottom;
        }
    }
}