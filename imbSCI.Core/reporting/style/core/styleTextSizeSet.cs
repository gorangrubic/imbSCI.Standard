// --------------------------------------------------------------------------------------------------------------------
// <copyright file="styleTextSizeSet.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.enumworks;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.geometrics;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Data;
    using imbSCI.Data.data;
    using imbSCI.Data.enums.reporting;
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Calc. struct class providing automatically calculated font sizes
    /// </summary>
    /// <remarks>
    /// Automatically sets font sizes for> H1....H6, text and remarks
    /// </remarks>
    /// \ingroup_disabled report_ll_style
    public class styleTextSizeSet : imbBindable, IAppendDataFields
    {
        /// <summary>
        /// Appends its data points into new or existing property collection
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>Updated or newly created property collection</returns>
        public PropertyCollection AppendDataFields(PropertyCollection data = null)
        {
            if (data == null) data = new PropertyCollection();
            this.buildPropertyCollection(false, false, "style", data);

            builderForStyle styler = new builderForStyle();

            for (var i = 0; i < 6; i++)
            {
                styler.Clear();
                styler.Append(imbSciStringExtensions.add(headingSizes[i].ToString(), "px", ""), HtmlTextWriterStyle.FontSize);

                styler.AppendLine(headingPaddings[i].ToString("padding", styleToStringFormat.multilineCss));

                styler.AppendLine(headingMargins[i].ToString("margin", styleToStringFormat.multilineCss));

                data["style_h" + (i + 1).ToString()] = styler.ToString();
            }

            styler.Clear();
            styler.Append(imbSciStringExtensions.add(textSize.ToString(), "px", ""), HtmlTextWriterStyle.FontSize);
            data[templateFieldStyleInserts.style_p] = styler.ToString();

            styler.Clear();
            styler.Append(imbSciStringExtensions.add(smallText.ToString(), "px", ""), HtmlTextWriterStyle.FontSize);
            data[templateFieldStyleInserts.style_q] = styler.ToString();
            styler.Clear();

            return data;
        }

        #region --- headingSizes ------- List of heading sizes

        private List<int> _headingSizes = new List<int>();

        /// <summary>
        /// List of heading sizes
        /// </summary>
        public List<int> headingSizes
        {
            get
            {
                return _headingSizes;
            }
            private set
            {
                _headingSizes = value;
                OnPropertyChanged("headingSizes");
            }
        }

        #endregion --- headingSizes ------- List of heading sizes

        #region --- headingMargins ------- Bindable property

        private List<fourSideSetting> _headingMargins = new List<fourSideSetting>();

        /// <summary>
        /// Bindable property
        /// </summary>
        public List<fourSideSetting> headingMargins
        {
            get
            {
                return _headingMargins;
            }
            private set
            {
                _headingMargins = value;
                OnPropertyChanged("headingMargins");
            }
        }

        #endregion --- headingMargins ------- Bindable property

        #region --- headingPaddings ------- padding

        private List<fourSideSetting> _headingPaddings = new List<fourSideSetting>();

        /// <summary>
        /// padding
        /// </summary>
        public List<fourSideSetting> headingPaddings
        {
            get
            {
                return _headingPaddings;
            }
            private set
            {
                _headingPaddings = value;
                OnPropertyChanged("headingPaddings");
            }
        }

        #endregion --- headingPaddings ------- padding

        #region --- smallText ------- Small text size

        private Int32 _smallText;

        /// <summary>
        /// Small text size
        /// </summary>
        public Int32 smallText
        {
            get
            {
                return _smallText;
            }
            private set
            {
                _smallText = value;
                OnPropertyChanged("smallText");
            }
        }

        #endregion --- smallText ------- Small text size

        #region --- textSize ------- size for main text

        private Int32 _textSize;

        /// <summary>
        /// size for main text
        /// </summary>
        public Int32 textSize
        {
            get
            {
                return _textSize;
            }
            private set
            {
                _textSize = value;
                OnPropertyChanged("textSize");
            }
        }

        #endregion --- textSize ------- size for main text

        internal static Int32 factor = 8;

        /// <summary>
        /// The factor extra -- from <c>factor</c> it reserves given number of sizes definitions
        /// </summary>
        internal static Int32 factorExtra = 2;

        /// <summary>
        /// CodeName should enable identification of unique text size configuration. In other words: if CodeName differs it means that two object of this type do not have the same property values.
        /// </summary>
        /// <returns></returns>
        public String getCodeName()
        {
            if (!imbSciStringExtensions.isNullOrEmptyString(codeName)) return codeName;
            String output = "";

            output = output.adds("-", headingSizes[0].toStringSafe(), headingSizes[hFactor].toStringSafe(), headingMargins[0].ToString(), headingPaddings[0].ToString());

            codeName = output;
            return output;
        }

        private String codeName = "";

        /// <summary>
        /// Automatically recalculats
        /// </summary>
        /// <param name="sizeForH1">Size of the biggest text heading</param>
        /// <param name="sizeForText">Size of normal test (i.e. paragraph)</param>
        /// <param name="H1Margin">Margin of the biggest text heading</param>
        /// <param name="H1Padding">Padding of H1</param>
        public styleTextSizeSet(Int32 sizeForH1, Int32 sizeForText, fourSideSetting H1Margin, fourSideSetting H1Padding)
        {
            deploy(sizeForH1, sizeForText, H1Margin, H1Padding);
        }

        /// <summary>
        /// Determines whether if <c>i</c> points to heading or regular text
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns>
        ///   <c>true</c> if [is this heading] [the specified i]; otherwise, <c>false</c>.
        /// </returns>
        internal Boolean isThisHeading(Int32 i)
        {
            return (i < hFactor);
        }

        internal Boolean isThisHeading(styleTextSizeEnum i)
        {
            return (i.ToInt32() < hFactor);
        }

        internal Boolean isThisMajorHeading(styleTextSizeEnum i)
        {
            return (i.ToInt32() < (hFactor / 3));
        }

        internal Boolean isThisMinorHeading(styleTextSizeEnum i)
        {
            return (i.ToInt32() < (2 * (hFactor / 3)));
        }

        internal Int32 hFactor = 0;
        internal Decimal step;

        /// <summary>
        /// Applies size modification
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="mod">The mod.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        internal Int32 getSizeMod(Int32 input, styleTextModificationEnum mod)
        {
            Int32 output = input;
            switch (mod)
            {
                case styleTextModificationEnum.none:
                    break;

                case styleTextModificationEnum.normal:
                    break;

                case styleTextModificationEnum.small:
                    output = input - Convert.ToInt32(2 * step);
                    break;

                case styleTextModificationEnum.smaller:
                    output = input - Convert.ToInt32(1 * step);
                    break;

                case styleTextModificationEnum.bigger:
                    output = input + Convert.ToInt32(1 * step);
                    break;

                case styleTextModificationEnum.big:
                    output = input + Convert.ToInt32(2 * step);
                    break;

                case styleTextModificationEnum.subscript:
                case styleTextModificationEnum.superscript:
                    // output = input / 4;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return output;
        }

        /// <summary>
        /// Automatically recalculates sizes
        /// </summary>
        /// <param name="sizeForH1">Size of the biggest text heading</param>
        /// <param name="sizeForText">Size of normal test (i.e. paragraph)</param>
        /// <param name="H1Margin">Margin of the biggest text heading</param>
        /// <param name="H1Padding">Padding of H1</param>
        internal void deploy(Int32 sizeForH1, Int32 sizeForText, fourSideSetting H1Margin, fourSideSetting H1Padding)
        {
            Int32 dif = (sizeForH1 - sizeForText);
            hFactor = factor - factorExtra;
            step = Math.Floor((Decimal)dif / hFactor);
            Decimal stepMargin = Math.Floor((Decimal)H1Margin.topAndBottom / hFactor);
            Decimal stepPadding = Math.Floor((Decimal)H1Padding.topAndBottom / hFactor);

            headingSizes.Add(sizeForH1);
            headingMargins.Add(H1Margin);
            headingPaddings.Add(H1Padding);

            for (var i = 0; i < (factor); i++)
            {
                headingSizes.Add(sizeForH1 - (Int32)(i * step));
                headingMargins.Add(H1Margin.getResized(-(Int32)(stepMargin * i), 0));
                headingPaddings.Add(H1Padding.getResized(-(Int32)(stepPadding * i), 0));
            }

            textSize = headingSizes[(int)styleTextSizeEnum.regular];
            smallText = headingSizes[(int)styleTextSizeEnum.smaller];
        }
    }
}