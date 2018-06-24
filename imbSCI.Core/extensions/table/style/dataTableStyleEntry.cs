// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataTableStyleEntry.cs" company="imbVeles" >
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
using System;

namespace imbSCI.Core.extensions.table.style
{
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Core.reporting.style.shot;
    using System.Drawing;

    public class dataTableStyleEntry
    {
        public dataTableStyleEntry()
        {
        }

        /// <summary>
        /// Clones only the backgrounds: <see cref="Background"/> and <see cref="BackgroundAlt"/> while the rest of data is just referenced
        /// </summary>
        /// <returns></returns>
        public dataTableStyleEntry CloneBackground()
        {
            dataTableStyleEntry output = new dataTableStyleEntry();

            output.Background = Background.Clone();
            output.BackgroundAlt = BackgroundAlt.Clone();
            output.Cell = Cell;
            output.Text = Text;
            return output;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public dataTableStyleEntry Clone()
        {
            dataTableStyleEntry output = new dataTableStyleEntry();

            output.Background = Background.Clone();
            output.BackgroundAlt = BackgroundAlt.Clone();
            output.Cell = Cell.Clone();
            output.Text = Text.Clone();
            return output;
        }

        public void DeployStyle(Object enumType)
        {
            key = enumType;

            if (enumType is DataRowInReportTypeEnum)
            {
                DataRowInReportTypeEnum enumType_DataRowInReportTypeEnum = (DataRowInReportTypeEnum)enumType;
                DeployDefault(enumType_DataRowInReportTypeEnum);
            }
        }

        public void DeployDefault(DataRowInReportTypeEnum enumType)
        {
            key = enumType;
            Background.FillType = reporting.style.enums.styleFillType.Solid;
            Text.FontName = reporting.style.enums.aceFont.Calibri;
            Cell.aligment = reporting.zone.textCursorZoneCorner.Left;
            Text.FontSize = 10;
            Cell.doWrapText = true;

            BackgroundAlt.Color = Background.Color;
            switch (enumType)
            {
                case DataRowInReportTypeEnum.columnCaption:
                    Background.Color = System.Drawing.Color.DarkGray;
                    Background.Tint = 0.6;
                    BackgroundAlt.Color = System.Drawing.Color.DarkGray;
                    BackgroundAlt.Tint = 0.7;
                    Text.Style = reporting.style.enums.styleTextTypeEnum.bold;
                    Cell.aligment = reporting.zone.textCursorZoneCorner.center;
                    Cell.minSize.height = 30;
                    break;

                case DataRowInReportTypeEnum.dataHighlightA:
                    Text.Style = reporting.style.enums.styleTextTypeEnum.bold;
                    Cell.sizeAndBorder.setup(3, 0, Color.OrangeRed, 1, reporting.style.enums.styleBorderType.Dotted, styleSideDirection.top, styleSideDirection.bottom);
                    Background.Color = Color.OrangeRed;
                    Background.Tint = 0.5;
                    BackgroundAlt.Color = Color.Orange;
                    BackgroundAlt.Tint = 0.5;
                    break;

                case DataRowInReportTypeEnum.dataHighlightB:
                    Text.Style = reporting.style.enums.styleTextTypeEnum.bold;
                    Cell.sizeAndBorder.setup(3, 0, Color.CadetBlue, 1, reporting.style.enums.styleBorderType.Dotted, styleSideDirection.top, styleSideDirection.bottom);
                    Background.Color = Color.CadetBlue;
                    Background.Tint = 0.5;
                    BackgroundAlt.Color = Color.DarkSeaGreen;
                    BackgroundAlt.Tint = 0.5;
                    break;

                case DataRowInReportTypeEnum.dataHighlightC:
                    Text.Style = reporting.style.enums.styleTextTypeEnum.bold;
                    Cell.sizeAndBorder.setup(3, 0, Color.SteelBlue, 1, reporting.style.enums.styleBorderType.Dotted, styleSideDirection.top, styleSideDirection.bottom);
                    Background.Color = Color.SteelBlue;
                    Background.Tint = 0.5;
                    BackgroundAlt.Color = Color.SteelBlue;
                    BackgroundAlt.Tint = 0.3;
                    break;

                case DataRowInReportTypeEnum.removedLight:
                    Text.Style = reporting.style.enums.styleTextTypeEnum.regular | styleTextTypeEnum.striketrough;
                    //Cell.sizeAndBorder.setup(0, 0, Color.Gray, 0, reporting.style.enums.styleBorderType.None);
                    Text.Color = Color.LightGray;
                    Background.Color = Color.WhiteSmoke;
                    BackgroundAlt.Color = Color.Snow;
                    Background.Tint = 0.8;
                    BackgroundAlt.Tint = 0.8;
                    break;

                case DataRowInReportTypeEnum.removedStrong:
                    Text.Style = reporting.style.enums.styleTextTypeEnum.regular | styleTextTypeEnum.striketrough;
                    //Cell.sizeAndBorder.setup(0, 0, Color.Gray, 0, reporting.style.enums.styleBorderType.None);
                    Text.Color = Color.OrangeRed;
                    Background.Color = Color.WhiteSmoke;
                    BackgroundAlt.Color = Color.Snow;
                    Background.Tint = 0.8;
                    BackgroundAlt.Tint = 0.8;
                    break;

                case DataRowInReportTypeEnum.columnDescription:
                    Background.Color = Color.LightSteelBlue;
                    BackgroundAlt.Color = Color.LightSteelBlue;
                    Cell.aligment = reporting.zone.textCursorZoneCorner.center;
                    Background.Tint = 0.4;
                    BackgroundAlt.Tint = 0.5;
                    Text.FontSize = 9;
                    Cell.minSize.height = 18;
                    break;

                case DataRowInReportTypeEnum.columnFooterInfo:
                    Cell.minSize.height = 28;
                    break;

                case DataRowInReportTypeEnum.columnInformation:
                    Background.Color = Color.LightSlateGray;
                    BackgroundAlt.Color = Color.SlateGray;
                    Background.Tint = 0.9;
                    BackgroundAlt.Tint = 0.8;
                    Text.FontSize = 9;
                    Cell.minSize.height = 28;
                    Cell.aligment = reporting.zone.textCursorZoneCorner.center;
                    break;

                case DataRowInReportTypeEnum.data:
                    Background.Color = Color.WhiteSmoke;
                    BackgroundAlt.Color = Color.Snow;
                    Background.Tint = 0.2;
                    BackgroundAlt.Tint = 0.2;
                    Text.FontSize = 9;
                    Cell.minSize.height = 15;
                    break;

                case DataRowInReportTypeEnum.dataAggregate:
                    break;

                case DataRowInReportTypeEnum.info:
                    break;

                case DataRowInReportTypeEnum.mergedCategoryHeader:
                    Background.Color = Color.Gray;
                    Background.Tint = 0.7;
                    BackgroundAlt.Color = Color.Gray;
                    BackgroundAlt.Tint = 0.9;
                    Cell.minSize.height = 25;
                    Cell.aligment = reporting.zone.textCursorZoneCorner.center;
                    break;

                case DataRowInReportTypeEnum.mergedFooterInfo:

                    Background.Color = Color.LightSlateGray;
                    BackgroundAlt.Color = Color.SlateGray;
                    Background.Tint = 0.8;
                    BackgroundAlt.Tint = 0.7;
                    Text.FontSize = 9;
                    Cell.minSize.height = 25;
                    break;

                case DataRowInReportTypeEnum.mergedHeaderInfo:
                    Background.Color = Color.LightSlateGray;
                    BackgroundAlt.Color = Color.SlateGray;
                    Background.Tint = 0.2;

                    BackgroundAlt.Tint = 0.2;
                    Text.FontSize = 9;
                    Cell.minSize.height = 24;
                    break;

                case DataRowInReportTypeEnum.mergedHeaderTitle:
                    Background.Color = Color.LightBlue;
                    Background.Tint = 0.2;
                    BackgroundAlt.Color = Color.LightBlue;
                    BackgroundAlt.Tint = 0.3;
                    Text.Style = reporting.style.enums.styleTextTypeEnum.bold;
                    Text.FontSize = 9;
                    Cell.minSize.height = 30;
                    break;

                case DataRowInReportTypeEnum.mergedHorizontally:
                    Cell.minSize.height = 12;
                    Text.FontSize = 8;
                    Background.Color = Color.LightGray;
                    BackgroundAlt.Color = Color.LightGray;
                    Background.Tint = 0.9;
                    BackgroundAlt.Tint = 0.8;
                    Cell.aligment = reporting.zone.textCursorZoneCorner.Left;
                    break;

                case DataRowInReportTypeEnum.none:
                    break;

                default:
                    Text.Style = reporting.style.enums.styleTextTypeEnum.regular;
                    //Cell.sizeAndBorder.setup(0, 0, Color.Gray, 0, reporting.style.enums.styleBorderType.None);
                    // Text.Color = Color.OrangeRed;
                    Background.Color = Color.WhiteSmoke;
                    BackgroundAlt.Color = Color.Snow;
                    Background.Tint = 0.4;
                    BackgroundAlt.Tint = 0.2;
                    break;
            }

            switch (enumType)
            {
                case DataRowInReportTypeEnum.group01:
                    Background.Color = Color.LightBlue;
                    BackgroundAlt.Color = Color.LightBlue;
                    break;

                case DataRowInReportTypeEnum.group02:
                    Background.Color = Color.Bisque;
                    BackgroundAlt.Color = Color.Bisque;
                    break;

                case DataRowInReportTypeEnum.group03:
                    Background.Color = Color.LightCyan;
                    BackgroundAlt.Color = Color.LightCyan;
                    break;

                case DataRowInReportTypeEnum.group04:
                    Background.Color = Color.LightGoldenrodYellow;
                    BackgroundAlt.Color = Color.LightGoldenrodYellow;
                    break;

                case DataRowInReportTypeEnum.group05:
                    Background.Color = Color.LightPink;
                    BackgroundAlt.Color = Color.LightPink;
                    break;
            }
        }

        public Object key { get; set; }

        public styleSurfaceColor Background { get; set; } = new styleSurfaceColor();

        public styleSurfaceColor BackgroundAlt { get; set; } = new styleSurfaceColor();

        public styleTextFontSingle Text { get; set; } = new styleTextFontSingle();

        public styleContainerShot Cell { get; set; } = new styleContainerShot();
    }
}