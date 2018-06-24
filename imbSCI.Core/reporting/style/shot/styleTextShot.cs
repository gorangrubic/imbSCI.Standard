// --------------------------------------------------------------------------------------------------------------------
// <copyright file="styleTextShot.cs" company="imbVeles" >
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
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.lowLevelApi;
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Data;
    using imbSCI.Data.enums.appends;
    using System;
    using System.Drawing;

    /// <summary>
    /// Concrete instance of textual decoration styling
    /// </summary>
    public sealed class styleTextShot : IStyleInstruction
    {
        public static String getCodeName(appendRole __role, appendType __type, styleTheme theme)
        {
            return __role.ToString() + "_" + __type.ToString() + "_" + theme.getCodeName();
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
        ///
        /// </summary>
        protected String themeCodeName
        {
            get { return _themeCodeName; }
            private set { _themeCodeName = value; }
        }

        /// <summary>
        /// The prefered way of obtaining style information
        /// </summary>
        /// <param name="role">The role of particular append</param>
        /// <param name="__type">The type of particular append</param>
        /// <param name="theme">The theme to use fonts and size settings from</param>
        public styleTextShot(appendRole role, appendType __type, styleTheme theme)
        {
            processRole(role);
            processType(__type, theme);
            themeCodeName = theme.getCodeName();
        }

        /// <summary>
        /// Styling based only on <c>appendRole</c> information.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="theme">The theme.</param>
        public styleTextShot(appendRole role, styleTheme theme)
        {
            processRole(role);
            processType(type, theme);
            themeCodeName = theme.getCodeName();
        }

        /// <summary>
        /// Sets the styling info using <c>appendRole</c> information
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="theme">The theme.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        internal void processRole(appendRole __role)
        {
            //appendType type = appendType.heading;

            //acePaletteVariationRole varRole = acePaletteVariationRole.normal;
            Boolean setType = (type == appendType.none);
            role = __role;

            //colorRole = role.convertRoleToVariationRole();
            switch (role)
            {
                case appendRole.none:
                    break;

                case appendRole.mergedHead:
                    doCallForMerge = true;
                    doCallForInverse = true;
                    modification = styleTextModificationEnum.bigger;
                    //   colorRole = acePaletteVariationRole.header;
                    break;

                case appendRole.mergedContent:
                    modification = styleTextModificationEnum.normal;
                    doCallForMerge = true;
                    //   colorRole = acePaletteVariationRole.normal;
                    break;

                case appendRole.mergedFoot:
                    modification = styleTextModificationEnum.smaller;
                    doCallForMerge = true;
                    doCallForInverse = true;
                    //   colorRole = acePaletteVariationRole.heading;
                    break;

                case appendRole.sectionHead:
                    modification = styleTextModificationEnum.bigger;
                    if (setType) type = appendType.bold;
                    doCallForInverse = true;
                    //  colorRole = acePaletteVariationRole.header;
                    break;

                case appendRole.sectionContent:
                    modification = styleTextModificationEnum.normal;
                    //  colorRole = acePaletteVariationRole.normal;
                    if (setType) type = appendType.regular;
                    break;

                case appendRole.sectionFoot:
                    modification = styleTextModificationEnum.smaller;
                    if (setType) type = appendType.italic;
                    //  colorRole = acePaletteVariationRole.heading;
                    doCallForInverse = true;
                    break;

                case appendRole.majorHeading:
                    // colorRole = acePaletteVariationRole.heading;
                    if (setType) type = appendType.heading_3;
                    break;

                case appendRole.minorHeading:
                    // colorRole = acePaletteVariationRole.normal;
                    if (setType) type = appendType.heading_5;
                    break;

                case appendRole.paragraph:
                    // colorRole = acePaletteVariationRole.normal;
                    modification = styleTextModificationEnum.normal;
                    if (setType) type = appendType.paragraph;
                    break;

                case appendRole.remark:
                    // colorRole = acePaletteVariationRole.heading;
                    modification = styleTextModificationEnum.small;
                    break;

                case appendRole.tableHead:
                    modification = styleTextModificationEnum.bigger;
                    //colorRole = acePaletteVariationRole.header;
                    if (setType) type = appendType.bold;
                    doCallForInverse = true;
                    doCallForMerge = true;
                    break;

                case appendRole.tableColumnHead:
                    doCallForInverse = true;
                    //colorRole = acePaletteVariationRole.heading;
                    modification = styleTextModificationEnum.bigger;
                    if (setType) type = appendType.regular;

                    break;

                case appendRole.tableColumnFoot:
                    //colorRole = acePaletteVariationRole.heading;
                    modification = styleTextModificationEnum.smaller;
                    //if (setType) type = appendType.italic;
                    break;

                case appendRole.tableCellValue:

                    //colorRole = acePaletteVariationRole.even;
                    doCallForVariance = true;

                    if (setType) type = appendType.regular;
                    break;

                case appendRole.tableCellAnnotation:
                    //colorRole = acePaletteVariationRole.even;
                    doCallForVariance = true;
                    modification = styleTextModificationEnum.smaller;
                    if (setType) type = appendType.comment;
                    break;

                case appendRole.tableCellNovalue:
                    //colorRole = acePaletteVariationRole.even;
                    doCallForVariance = true;
                    if (setType) type = appendType.none;
                    break;

                case appendRole.tableBetween:
                    //colorRole = acePaletteVariationRole.even;
                    doCallForVariance = true;
                    modification = styleTextModificationEnum.smaller;
                    if (setType) type = appendType.regular;
                    break;

                case appendRole.tableFoot:
                    //colorRole = acePaletteVariationRole.heading;
                    modification = styleTextModificationEnum.smaller;
                    if (setType) type = appendType.italic;
                    doCallForInverse = true;
                    doCallForMerge = true;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Styling information based on <c>appendType</c> and <c>styleTheme</c>
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="theme">The theme.</param>
        public styleTextShot(appendType type, styleTheme theme)
        {
            processType(type, theme);
            themeCodeName = theme.getCodeName();
        }

        //public styleTextShot(appendType type, styleTheme theme)
        //{
        //    this.type = type;
        //    this.theme = theme;
        //}

        /// <summary>
        /// Processes the <c>appendType</c> and <c>styleTheme</c> to define all styling information
        /// </summary>
        /// <param name="__type">The type.</param>
        /// <param name="theme">The theme.</param>
        internal void processType(appendType __type, styleTheme theme)
        {
            type = __type;

            sizeEnum = imbStringMarkdownExtensions.getTextSizeEnum(type);

            fontStyle = new FontStyle();

            if (theme.fontSize.isThisHeading(sizeForFont))
            {
                font = theme.fontForHeadings.drawingFont;
            }
            else
            {
                font = theme.fontForText.drawingFont;
            }
            sizeForFont = theme.fontSize.headingSizes[sizeEnum.ToInt32()];

            doCallForInverse = theme.fontSize.isThisMajorHeading(sizeEnum);

            switch (type)
            {
                case appendType.squareQuote:
                    fontStyle = FontStyle.Italic | FontStyle.Underline;
                    break;

                case appendType.blockquote:
                    fontStyle = FontStyle.Italic | FontStyle.Bold;
                    break;

                case appendType.marked:
                    fontStyle = FontStyle.Underline;
                    break;

                case appendType.quotation:
                case appendType.italic:
                    fontStyle = FontStyle.Italic;
                    break;

                case appendType.bold:
                    fontStyle = FontStyle.Bold;

                    break;

                case appendType.monospace:
                    fontStyle = FontStyle.Regular;

                    //target.Font.SetFromFont(System.Drawing.SystemFonts.DefaultFont);
                    break;

                case appendType.math:
                case appendType.source:
                case appendType.sourceCS:
                case appendType.sourceJS:
                case appendType.sourcePY:
                case appendType.sourceXML:

                    break;

                case appendType.striketrough:
                    fontStyle = FontStyle.Strikeout;
                    break;

                case appendType.comment:
                    modification = styleTextModificationEnum.smaller;
                    break;
            }
        }

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

        private styleTextModificationEnum _modification = styleTextModificationEnum.none;

        /// <summary>
        /// How font size should be modified before applied?
        /// </summary>
        public styleTextModificationEnum modification
        {
            get { return _modification; }
            set { _modification = value; }
        }

        private acePaletteVariationRole _colorRole = acePaletteVariationRole.normal;

        /// <summary>
        /// What color role should be applied here
        /// </summary>
        public acePaletteVariationRole colorRole
        {
            get { return _colorRole; }
            set { _colorRole = value; }
        }

        private styleTextRotationEnum _rotation = styleTextRotationEnum.rotate0;

        /// <summary>
        ///
        /// </summary>
        public styleTextRotationEnum rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        private styleTextSizeEnum _sizeEnum;

        /// <summary>
        ///
        /// </summary>
        public styleTextSizeEnum sizeEnum
        {
            get { return _sizeEnum; }
            set { _sizeEnum = value; }
        }

        private Int32 _sizeFont;

        /// <summary>
        /// Size of font to be set
        /// </summary>
        public Int32 sizeForFont
        {
            get { return _sizeFont; }
            set { _sizeFont = value; }
        }

        private FontStyle _fontStyle;

        /// <summary>
        /// What font style it should set
        /// </summary>
        public FontStyle fontStyle
        {
            get { return _fontStyle; }
            set { _fontStyle = value; }
        }

        private Boolean _doCallForInverse;

        /// <summary>
        /// This ShotSet will ask engine to apply inversed colors
        /// </summary>
        public Boolean doCallForInverse
        {
            get { return _doCallForInverse; }
            set { _doCallForInverse = value; }
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

        private Boolean _doCallForVariance;

        /// <summary>
        /// Should it call for OddEven roles to be applied?
        /// </summary>
        public Boolean doCallForVariance
        {
            get { return _doCallForVariance; }
            set { _doCallForVariance = value; }
        }

        private Font _font;
        private styleTheme theme;

        /// <summary>
        ///
        /// </summary>
        public Font font
        {
            get { return _font; }
            set { _font = value; }
        }
    }
}