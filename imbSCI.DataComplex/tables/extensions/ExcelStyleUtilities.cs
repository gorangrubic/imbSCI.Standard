using imbSCI.Core.extensions.enumworks;
using imbSCI.Core.extensions.table.style;
using imbSCI.Core.reporting.style.core;
using imbSCI.Core.reporting.style.enums;
using imbSCI.Core.reporting.style.shot;
using imbSCI.Core.reporting.zone;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;

namespace imbSCI.DataComplex.tables.extensions
{
    public static class ExcelStyleUtilities
    {

        public static ExcelVerticalAlignment GetVerticalAlignment(this textCursorZoneCorner aligment)
        {

            switch (aligment)
            {
                default:
                case textCursorZoneCorner.Top:
                case textCursorZoneCorner.UpLeft:
                case textCursorZoneCorner.UpRight:
                    return ExcelVerticalAlignment.Top;
                    break;

                case textCursorZoneCorner.Bottom:
                case textCursorZoneCorner.DownRight:
                case textCursorZoneCorner.DownLeft:

                    return ExcelVerticalAlignment.Bottom;
                    break;

                case textCursorZoneCorner.Left:
                case textCursorZoneCorner.Right:
                case textCursorZoneCorner.center:
                case textCursorZoneCorner.default_corner:
                case textCursorZoneCorner.none:
                    return ExcelVerticalAlignment.Center;
                    break;


            }


        }

        public static ExcelHorizontalAlignment GetHorizontalAlignment(this textCursorZoneCorner aligment)
        {

            switch (aligment)
            {
                default:
                case textCursorZoneCorner.Top:
                case textCursorZoneCorner.Bottom:
                case textCursorZoneCorner.center:
                case textCursorZoneCorner.default_corner:
                case textCursorZoneCorner.none:
                    return ExcelHorizontalAlignment.Center;
                    break;
                case textCursorZoneCorner.UpRight:
                case textCursorZoneCorner.DownRight:
                case textCursorZoneCorner.Right:
                    return ExcelHorizontalAlignment.Right;
                    break;

                case textCursorZoneCorner.DownLeft:
                case textCursorZoneCorner.Left:
                case textCursorZoneCorner.UpLeft:
                    return ExcelHorizontalAlignment.Left;
                    break;

            }

            /*

            if (dc.GetValueType().isNumber())
            {
                ws.Cells[ex_row.Row, dc.Ordinal + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            else if (dc.GetValueType().IsEnum)
            {
                ws.Cells[ex_row.Row, dc.Ordinal + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            else if (dc.GetValueType().isBoolean())
            {
                ws.Cells[ex_row.Row, dc.Ordinal + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            else
            {
                ws.Cells[ex_row.Row, dc.Ordinal + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            }*/
        }


        /// <summary>
        /// Sets the style.
        /// </summary>
        /// <param name="ExcelStyle">The excel style.</param>
        /// <param name="styleEntry">The style entry.</param>
        public static void SetStyle(this ExcelStyle Style, dataTableStyleEntry styleEntry, Boolean isEven = false)
        {
            Style.Font.SetStyle(styleEntry.Text);
            //Style.TextRotation = styleEntry.Text.ro2

            if (isEven)
            {
                Style.Fill.SetStyle(styleEntry.Background);
            }
            else
            {
                Style.Fill.SetStyle(styleEntry.BackgroundAlt);
            }
            Style.SetStyle(styleEntry.Cell);
        }

        /// <summary>
        /// Sets the style.
        /// </summary>
        /// <param name="Fill">The fill.</param>
        /// <param name="styleEntry">The style entry.</param>
        public static void SetStyle(this ExcelFill Fill, styleSurfaceColor styleEntry)
        {
            if (styleEntry != null)
            {
                Fill.PatternType = (ExcelFillStyle)styleEntry.FillType;
                Fill.BackgroundColor.SetColor(styleEntry.Color);
                Fill.BackgroundColor.Tint = new decimal(styleEntry.Tint);
            }
        }

        public static void SetStyle(this ExcelRow row, dataTableStyleEntry style, Boolean isEven = false)
        {
            if (style != null)
            {
                if (style?.Cell?.minSize?.height == null)
                {
                    return;
                }
                row.Height = style.Cell.minSize.height;
                row.StyleName = style.key.ToString();
                row.Style.SetStyle(style, isEven);
            }
        }

        public static void SetStyle(this ExcelStyle Style, styleFourSide side)
        {
            if (side != null)
            {
                Style.SetStyle(side.top);
                Style.SetStyle(side.bottom);
                Style.SetStyle(side.left);
                Style.SetStyle(side.right);
            }
        }

        /// <summary>
        /// Sets the style.
        /// </summary>
        /// <param name="Style">The style.</param>
        /// <param name="side">The side.</param>
        public static void SetStyle(this ExcelStyle Style, styleSide side)
        {
            ExcelBorderItem bri = null;
            switch (side.direction)
            {
                case styleSideDirection.bottom:
                    bri = Style.Border.Bottom;
                    break;

                case styleSideDirection.left:
                    bri = Style.Border.Left;
                    break;

                case styleSideDirection.right:
                    bri = Style.Border.Right;
                    break;

                case styleSideDirection.top:
                    bri = Style.Border.Top;
                    break;
            }

            if (side.type != styleBorderType.unknown)
            {
                bri.Style = (ExcelBorderStyle)side.type.ToInt32();
                if (bri.Style != ExcelBorderStyle.None) bri.Color.SetColor(side.borderColorStatic);
            }
        }

        /// <summary>
        /// Sets the style.
        /// </summary>
        /// <param name="Style">The style.</param>
        /// <param name="styleEntry">The style entry.</param>
        public static void SetStyle(this ExcelStyle Style, styleContainerShot styleEntry)
        {
            Style.WrapText = styleEntry.doWrapText;
            Style.SetStyle(styleEntry.sizeAndBorder);

            switch (styleEntry.aligment)
            {
                case Core.reporting.zone.textCursorZoneCorner.Bottom:
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    break;

                case textCursorZoneCorner.center:
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    break;

                case textCursorZoneCorner.default_corner:
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    break;

                case textCursorZoneCorner.DownLeft:
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    break;

                case textCursorZoneCorner.DownRight:
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    break;

                case textCursorZoneCorner.Left:
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    break;

                case textCursorZoneCorner.none:
                    break;

                case textCursorZoneCorner.Right:
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    break;

                case textCursorZoneCorner.Top:
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    break;

                case textCursorZoneCorner.UpLeft:
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    break;

                case textCursorZoneCorner.UpRight:
                    Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                    Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    break;

                default:
                    break;
            }

            Style.ShrinkToFit = styleEntry.doSizedownContent;
        }

        public static void SetStyle(this ExcelFont Font, styleTextFontSingle styleEntry)
        {
            Font.Name = styleEntry.FontName.ToString();
            Font.Color.SetColor(styleEntry.Color);

            Font.Bold = styleEntry.Style.HasFlag(styleTextTypeEnum.bold);
            Font.Italic = styleEntry.Style.HasFlag(styleTextTypeEnum.italic);
            Font.Strike = styleEntry.Style.HasFlag(styleTextTypeEnum.striketrough);
            Font.UnderLine = styleEntry.Style.HasFlag(styleTextTypeEnum.underline);
            Font.Size = styleEntry.FontSize;
        }

    }
}