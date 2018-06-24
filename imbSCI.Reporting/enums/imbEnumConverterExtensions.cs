// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbEnumConverterExtensions.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.enums
{
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using System;

    /// <summary>
    /// Extensions to convert different enum types from one to another, including string
    /// </summary>
    public static class imbEnumConverterExtensions
    {
        /// <summary>
        /// Returns TRUE if the <c>policy</c> instructs throwing exception
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        public static Boolean doThrow(this onErrorPolicy policy)
        {
            if (policy == onErrorPolicy.onErrorThrowException) return true;
            return false;
        }

        /// <summary>
        /// Gets the file extension.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static String getFileExtension(this appendLinkType type)
        {
            switch (type)
            {
                case appendLinkType.anchor:
                    break;

                case appendLinkType.image:
                    return ".png";
                    break;

                case appendLinkType.link:
                    return ".html";
                    break;

                case appendLinkType.reference:
                case appendLinkType.referenceImage:
                    return ".png";
                    break;

                case appendLinkType.referenceLink:
                    break;

                case appendLinkType.scriptLink:
                case appendLinkType.scriptPostLink:
                    return ".js";
                    break;

                case appendLinkType.styleLink:
                    return ".css";
                    break;

                case appendLinkType.unknown:
                    break;

                default:
                    return ".js";
                    break;
            }

            return "";
        }

        /// <summary>
        /// Returns appendTypeKind according to appendType
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Proper kind of append</returns>
        public static appendTypeKind getAppendTypeKind(this appendType type)
        {
            switch (type)
            {
                case appendType.none:
                case appendType.bypass:
                    return appendTypeKind.none;
                    break;

                case appendType.c_data:
                case appendType.c_pair:
                case appendType.c_table:
                case appendType.c_link:
                case appendType.c_section:
                case appendType.footnote:
                case appendType.section:
                case appendType.c_open:
                case appendType.c_close:
                case appendType.c_line:
                    return appendTypeKind.complex;
                    break;

                case appendType.source:
                case appendType.sourceCS:
                case appendType.sourceJS:
                case appendType.sourcePY:
                case appendType.sourceXML:
                case appendType.squareQuote:
                case appendType.striketrough:
                case appendType.subscript:
                case appendType.superscript:
                case appendType.comment:
                case appendType.blockquote:
                case appendType.bold:
                case appendType.heading:
                case appendType.heading_1:
                case appendType.heading_2:
                case appendType.heading_3:
                case appendType.heading_4:
                case appendType.heading_5:
                case appendType.heading_6:
                case appendType.italic:
                case appendType.marked:
                case appendType.math:
                case appendType.monospace:
                case appendType.paragraph:
                case appendType.quotation:
                case appendType.regular:
                    return appendTypeKind.simple;
                    break;

                case appendType.i_chart:
                case appendType.i_external:
                case appendType.i_load:
                case appendType.i_page:
                case appendType.i_document:
                case appendType.i_meta:
                case appendType.i_function:
                case appendType.i_log:
                case appendType.x_directory:
                case appendType.x_save:
                case appendType.x_scopeIn:
                case appendType.x_scopeOut:
                case appendType.x_openTool:
                case appendType.x_export:
                case appendType.x_move:
                case appendType.x_data:
                case appendType.i_dataSource:
                case appendType.i_dataInDocument:
                case appendType.s_style:
                    return appendTypeKind.special;
                    break;

                case appendType.s_alternate:
                case appendType.s_normal:
                case appendType.s_palette:
                case appendType.s_width:
                case appendType.s_variation:

                case appendType.s_zone:
                    return appendTypeKind.style;
                    break;

                    break;

                    break;

                default:

                    String name = type.ToString();

                    if (name.StartsWith("i_")) return appendTypeKind.special;
                    if (name.StartsWith("x_")) return appendTypeKind.special;

                    if (name.StartsWith("s_")) return appendTypeKind.style;
                    if (name.StartsWith("c_")) return appendTypeKind.complex;

                    return appendTypeKind.other;
                    break;
            }
            return appendTypeKind.other;
        }

        public static acePaletteVariationRole convertRoleToVariationRole(this appendRole role)
        {
            //appendType type = appendType.heading;

            //acePaletteVariationRole varRole = acePaletteVariationRole.normal;

            acePaletteVariationRole colorRole = acePaletteVariationRole.normal;

            switch (role)
            {
                case appendRole.none:
                    break;

                case appendRole.mergedHead:

                    colorRole = acePaletteVariationRole.header;
                    break;

                case appendRole.mergedContent:

                    colorRole = acePaletteVariationRole.normal;
                    break;

                case appendRole.mergedFoot:

                    colorRole = acePaletteVariationRole.heading;
                    break;

                case appendRole.sectionHead:

                    colorRole = acePaletteVariationRole.header;
                    break;

                case appendRole.sectionContent:

                    colorRole = acePaletteVariationRole.normal;

                    break;

                case appendRole.sectionFoot:

                    colorRole = acePaletteVariationRole.heading;

                    break;

                case appendRole.majorHeading:
                    colorRole = acePaletteVariationRole.heading;

                    break;

                case appendRole.minorHeading:
                    colorRole = acePaletteVariationRole.normal;

                    break;

                case appendRole.paragraph:
                    colorRole = acePaletteVariationRole.normal;

                    break;

                case appendRole.remark:
                    colorRole = acePaletteVariationRole.heading;

                    break;

                case appendRole.tableHead:

                    colorRole = acePaletteVariationRole.header;

                    break;

                case appendRole.tableColumnHead:

                    colorRole = acePaletteVariationRole.heading;

                    break;

                case appendRole.tableColumnFoot:
                    colorRole = acePaletteVariationRole.heading;

                    break;

                case appendRole.tableCellValue:

                    colorRole = acePaletteVariationRole.even;

                    break;

                case appendRole.tableCellAnnotation:
                    colorRole = acePaletteVariationRole.even;

                    break;

                case appendRole.tableCellNovalue:
                    colorRole = acePaletteVariationRole.even;

                    break;

                case appendRole.tableBetween:
                    colorRole = acePaletteVariationRole.even;

                    break;

                case appendRole.tableFoot:
                    colorRole = acePaletteVariationRole.heading;

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return colorRole;
        }

        /// <summary>
        /// Converts <c>role</c> into <c>appendType</c>
        /// </summary>
        /// <param name="__role">The role.</param>
        /// <param name="__overrideResult">If not <c>none</c> it will override any conversion and return this as result</param>
        /// <param name="__defaultType">The default append Type to return if no rule is applicable.</param>
        /// <returns></returns>
        public static appendType convertRoleToType(this appendRole __role, appendType __overrideResult = appendType.none, appendType __defaultType = appendType.regular)
        {
            //appendType type = appendType.heading;

            //acePaletteVariationRole varRole = acePaletteVariationRole.normal;
            Boolean setType = (__overrideResult == appendType.none);

            appendType type = __defaultType;
            if (!setType)
            {
                type = __overrideResult;
                return type;
            }

            switch (__role)
            {
                case appendRole.none:
                    // type = appendType.regular;
                    break;

                case appendRole.mergedHead:
                    type = appendType.bold;
                    break;

                case appendRole.mergedContent:
                    type = appendType.regular;
                    break;

                case appendRole.mergedFoot:
                    type = appendType.italic;
                    break;

                case appendRole.sectionHead:

                    type = appendType.bold;

                    break;

                case appendRole.sectionContent:
                    type = appendType.regular;
                    break;

                case appendRole.sectionFoot:

                    type = appendType.italic;

                    break;

                case appendRole.majorHeading:
                    type = appendType.heading_1;
                    break;

                case appendRole.minorHeading:
                    type = appendType.heading_3;

                    break;

                case appendRole.paragraph:
                    type = appendType.paragraph;
                    break;

                case appendRole.remark:
                    type = appendType.italic;
                    break;

                case appendRole.tableHead:
                    type = appendType.bold;
                    break;

                case appendRole.tableColumnHead:
                    type = appendType.regular;
                    break;

                case appendRole.tableColumnFoot:
                    type = appendType.italic;
                    break;

                case appendRole.tableCellValue:

                    type = appendType.regular;
                    break;

                case appendRole.tableCellAnnotation:
                    type = appendType.comment;
                    break;

                case appendRole.tableCellNovalue:
                    // type = appendType.none;
                    break;

                case appendRole.tableBetween:
                    type = appendType.regular;
                    break;

                case appendRole.tableFoot:
                    type = appendType.italic;

                    break;

                default:
                    //type = appendType.regular;
                    //throw new ArgumentOutOfRangeException();
                    break;
            }
            return type;
        }

        public static styleTextTypeEnum getSubset(this appendType type)
        {
            styleTextTypeEnum output = styleTextTypeEnum.none;

            switch (type)
            {
                default:
                case appendType.bold:
                case appendType.monospace:
                case appendType.striketrough:
                case appendType.squareQuote:
                case appendType.blockquote:
                    if (Enum.TryParse<styleTextTypeEnum>(output.ToString(), out output))
                    {
                        return output;
                    }
                    break;

                case appendType.marked:
                    return styleTextTypeEnum.marked;
                    break;

                case appendType.quotation:
                case appendType.italic:
                    return styleTextTypeEnum.italic;
                    break;

                case appendType.math:
                case appendType.source:
                case appendType.sourceCS:
                case appendType.sourceJS:
                case appendType.sourcePY:
                case appendType.sourceXML:
                    return styleTextTypeEnum.source;
                    break;
            }
            return output;
        }

        /// <summary>
        /// Alias of <see cref="getFilenameExtension(reportOutputFormatName)"/>
        /// </summary>
        /// <param name="outputFormat"></param>
        /// <returns></returns>
        public static String getDefaultExtension(this reportOutputFormatName outputFormat)
        {
            return outputFormat.getFilenameExtension();
        }

        /// <summary>
        /// Gets the filename extension for <c>format</c>. String is without starting dot.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static String getFilenameExtension(this reportOutputFormatName format)
        {
            String extension = "bin";

            switch (format)
            {
                case reportOutputFormatName.sheetPDF:
                case reportOutputFormatName.docPDF:
                    extension = "pdf";
                    break;

                case reportOutputFormatName.sheetCsv:
                case reportOutputFormatName.csv:
                    extension = "csv";
                    break;

                case reportOutputFormatName.sheetExcel:
                case reportOutputFormatName.Excel:

                    extension = "xlsx";

                    break;

                case reportOutputFormatName.folderReadme:
                case reportOutputFormatName.textMdFile:
                case reportOutputFormatName.markdown:
                    extension = "md";

                    break;

                case reportOutputFormatName.textLog:
                    extension = "log";
                    break;

                case reportOutputFormatName.emailPlainText:
                case reportOutputFormatName.unknown:
                case reportOutputFormatName.textFile:
                    extension = "txt";
                    break;

                case reportOutputFormatName.sheetXML:
                case reportOutputFormatName.textXml:
                case reportOutputFormatName.serXml:
                case reportOutputFormatName.xml:
                    extension = "xml";
                    break;

                case reportOutputFormatName.sheetHtml:
                case reportOutputFormatName.textHtml:
                case reportOutputFormatName.emailHTML:
                case reportOutputFormatName.docHTML:
                case reportOutputFormatName.htmlViaMD:
                case reportOutputFormatName.htmlReport:
                    extension = "html";
                    break;

                case reportOutputFormatName.docRTF:
                    extension = "rtf";
                    break;

                case reportOutputFormatName.docJPG:
                    extension = "jpg";
                    break;

                case reportOutputFormatName.docTIFF:
                    extension = "tiff";
                    break;

                case reportOutputFormatName.docPNG:
                    extension = "png";
                    break;

                case reportOutputFormatName.rdf:
                    extension = "rdf";
                    break;

                case reportOutputFormatName.owl:
                    extension = "owl";
                    break;

                case reportOutputFormatName.json:
                    extension = "json";
                    break;

                case reportOutputFormatName.Writter:
                    extension = "odt";
                    break;

                case reportOutputFormatName.Calc:
                    extension = "ods";
                    break;

                case reportOutputFormatName.textCss:
                    extension = "css";
                    break;

                case reportOutputFormatName.docXAML:
                    extension = "xaml";
                    break;

                case reportOutputFormatName.Word:
                    extension = "docx";
                    break;

                case reportOutputFormatName.none:
                    extension = "";
                    break;

                default:

                    extension = format.ToString();
                    if (extension.StartsWith("text"))
                    {
                        extension = "txt";
                    }
                    else if (extension.EndsWith("log"))
                    {
                        extension = "log";
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("format", "Extension not defined yet -> aceCommonTypes.extensions.imbEnumConverterExtensions.getFilenameExtension(this reportOutputFormatName format)");
                    }

                    break;
            }

            return extension;
        }
    }
}