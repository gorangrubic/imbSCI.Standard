// --------------------------------------------------------------------------------------------------------------------
// <copyright file="styleContainerShot.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.style.shot
{
    using imbSCI.Core.extensions.enumworks;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.geometrics;
    using imbSCI.Core.reporting.lowLevelApi;
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data;
    using imbSCI.Data.enums.appends;
    using System;

    /// <summary>
    /// Style settings for container
    /// </summary>
    public class styleContainerShot : IStyleInstruction
    {
        public appendRole role { get; private set; }

        private appendType _type;

        /// <summary>
        ///
        /// </summary>
        public appendType type
        {
            get { return _type; }
            private set { _type = value; }
        }

        public styleContainerShot Clone()
        {
            styleContainerShot output = new styleContainerShot();

            output.aligment = aligment;
            output.doCallForMerge = doCallForMerge;
            output.doSizedownContent = doSizedownContent;
            output.doWrapText = doWrapText;
            output.minSize = minSize;
            output.numberFormat = numberFormat;
            output.role = role;
            output.sizeAndBorder = new styleFourSide();
            sizeAndBorder.CopySideSettingsTo(output.sizeAndBorder);

            return output;//.apply(sizeAndBorder.sget
        }

        public static String getCodeName(appendRole __role, appendType __type, styleTheme theme)
        {
            return __role.ToString() + "_" + __type.ToString() + "_" + theme.getCodeName();
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

            output = role.ToString().adds("-", type.ToString(), themeCodeName);
            codeName = output;
            return output;
        }

        /// <summary>
        /// Returns the zone spatial settings -- btw. top/bottom and left/right margin and padding are equalized: top=>bottom, left=>right
        /// </summary>
        /// <returns></returns>
        public cursorZoneSpatialSettings GetFormatSetup()
        {
            cursorZoneSpatialSettings output = new cursorZoneSpatialSettings(minSize.width, minSize.height, sizeAndBorder.left.margin, sizeAndBorder.top.margin, sizeAndBorder.left.padding, sizeAndBorder.top.padding);

            return output;
        }

        public styleContainerShot()
        {
        }

        public styleContainerShot(appendRole __role, appendType __type, styleTheme theme)
        {
            processRole(__role, __type, theme);
        }

        internal void processRole(appendRole __role, appendType __type, styleTheme theme)
        {
            role = __role;
            type = __type;

            Int32 sizeKWidth = 8;
            Int32 sizeKHeight = 1;

            themeCodeName = theme.getCodeName();

            var sizeEnum = imbStringMarkdownExtensions.getTextSizeEnum(__type);
            var size = theme.fontSize.headingSizes[sizeEnum.ToInt32()];

            fourSideSetting margin = theme.fontSize.headingMargins[sizeEnum.ToInt32()];
            fourSideSetting padding = theme.fontSize.headingPaddings[sizeEnum.ToInt32()];

            sizeAndBorder.apply(margin, styleFourSideParameter.margin);
            sizeAndBorder.apply(padding, styleFourSideParameter.padding);

            String roleName = role.ToString();

            aligment = textCursorZoneCorner.Left;

            if (roleName.StartsWith("merged"))
            {
                doWrapText = true;
                doCallForMerge = true;
            }

            if (roleName.StartsWith("section"))
            {
                doWrapText = true;
            }

            if (roleName.EndsWith("Head"))
            {
                aligment = textCursorZoneCorner.center;
                sizeAndBorder.bottom.type = styleBorderType.Thin;
            }

            if (roleName.EndsWith("Foot"))
            {
                aligment = textCursorZoneCorner.Left;
                sizeAndBorder.top.type = styleBorderType.Thin;
            }

            switch (role)
            {
                case appendRole.none:
                    break;

                case appendRole.mergedHead:

                    break;

                case appendRole.mergedContent:
                    break;

                case appendRole.mergedFoot:

                    break;

                case appendRole.sectionHead:

                    break;

                case appendRole.sectionContent:
                    break;

                case appendRole.sectionFoot:

                    break;

                case appendRole.majorHeading:
                    break;

                case appendRole.minorHeading:
                    break;

                case appendRole.paragraph:
                    break;

                case appendRole.remark:
                    break;

                case appendRole.tableHead:
                    sizeAndBorder.top.type = styleBorderType.Double;
                    doWrapText = true;
                    doCallForMerge = true;
                    break;

                case appendRole.tableColumnHead:
                    doWrapText = true;
                    break;

                case appendRole.tableColumnFoot:
                    sizeAndBorder.top.type = styleBorderType.Dotted;
                    break;

                case appendRole.tableCellValue:
                    break;

                case appendRole.tableCellAnnotation:
                    sizeAndBorder.top.type = styleBorderType.Dotted;
                    sizeAndBorder.bottom.type = styleBorderType.Dotted;
                    break;

                case appendRole.tableCellNovalue:

                    break;

                case appendRole.tableBetween:
                    sizeAndBorder.top.type = styleBorderType.Dotted;
                    sizeAndBorder.bottom.type = styleBorderType.Dotted;
                    break;

                case appendRole.tableFoot:
                    sizeAndBorder.bottom.type = styleBorderType.Double;
                    break;

                case appendRole.i_container:
                    break;

                case appendRole.i_margin:
                    sizeKHeight = 1;
                    sizeKWidth = 1;
                    break;

                case appendRole.i_line:
                    break;

                default:
                    break;
            }

            minSize = new styleSize(size * sizeKWidth, size * sizeKHeight);
            minSize.width += sizeAndBorder.left.padding + sizeAndBorder.right.padding;
            minSize.height += sizeAndBorder.top.padding + sizeAndBorder.bottom.padding;
        }

        private styleFourSide _sizeAndBorder = new styleFourSide();

        /// <summary>
        ///
        /// </summary>
        public styleFourSide sizeAndBorder
        {
            get { return _sizeAndBorder; }
            set { _sizeAndBorder = value; }
        }

        private Boolean _doCallForMerge;

        /// <summary>
        /// Should it call for multicell merge - in case it is applicable
        /// </summary>
        public Boolean doCallForMerge
        {
            get { return _doCallForMerge; }
            set { _doCallForMerge = value; }
        }

        #region --- minSize ------- minimum size

        private styleSize _minSize = new styleSize();

        /// <summary>
        /// minimum size
        /// </summary>
        public styleSize minSize
        {
            get
            {
                return _minSize;
            }
            set
            {
                _minSize = value;
            }
        }

        #endregion --- minSize ------- minimum size

        private Boolean _doWrapText = true;

        /// <summary>
        ///
        /// </summary>
        public Boolean doWrapText
        {
            get { return _doWrapText; }
            set { _doWrapText = value; }
        }

        private Boolean _doSizedownContent = false;

        /// <summary>
        ///
        /// </summary>
        public Boolean doSizedownContent
        {
            get { return _doSizedownContent; }
            set { _doSizedownContent = value; }
        }

        private String _numberFormat;

        /// <summary>
        ///
        /// </summary>
        public String numberFormat
        {
            get { return _numberFormat; }
            set { _numberFormat = value; }
        }

        private textCursorZoneCorner _aligment = new textCursorZoneCorner();

        /// <summary>
        ///
        /// </summary>
        public textCursorZoneCorner aligment
        {
            get { return _aligment; }
            set { _aligment = value; }
        }
    }
}