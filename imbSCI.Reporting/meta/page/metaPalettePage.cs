// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaPalettePage.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.page
{
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Core.style.color;
    using imbSCI.Data.enums.appends;
    using imbSCI.Reporting.script;
    using System.Drawing;

    public class metaPalettePage : metaPage
    {
        private string _baseColor;

        /// <summary> </summary>
        public string baseColor
        {
            get
            {
                return _baseColor;
            }
            set
            {
                _baseColor = value;
                OnPropertyChanged("baseColor");
            }
        }

        private aceColorPalette _palette;

        /// <summary> </summary>
        public aceColorPalette palette
        {
            get
            {
                return _palette;
            }
            set
            {
                _palette = value;
                OnPropertyChanged("palette");
            }
        }

        public override docScript compose(docScript script)
        {
            script = this.checkScript(script);
            int column = 0;
            string hexColor = palette.hexColor;
            Color altColor = ColorWorks.GetColor(hexColor); //.getColorFromHex();

            //Color altOne = altColor.getVariation(0.2F, 0.1F, 80);
            //Color altTwo = altColor.getVariation(0.2F, 0.1F, 160);
            //Color altThree = altColor.getVariation(0.2F, 0.1F, 240);

            //aceColorPalette paletteOne = new aceColorPalette(altOne.toHexColor());
            //aceColorPalette paletteTwo = new aceColorPalette(altTwo.toHexColor());
            //aceColorPalette paletteThree = new aceColorPalette(altThree.toHexColor());

            script.x_scopeIn(this);

            script = composeForPalette(script, altColor, 0.0F, 0.0F, 0, "--", true);
            script = composeForPalette(script, altColor, 0.0F, 0.0F, 0, "Main", false);
            script = composeForPalette(script, altColor, 0.1F, 0.1F, 0, "Brignter 01", false);
            script = composeForPalette(script, altColor, 0.2F, 0.2F, 0, "Brignter 02", false);
            script = composeForPalette(script, altColor, 0.4F, 0.4F, 0, "Brignter 03", false);
            script = composeForPalette(script, altColor, 0.1F, 0.1F, 120, "Primary 01", false);
            script = composeForPalette(script, altColor, 0.2F, 0.2F, 120, "Primary 02", false);
            script = composeForPalette(script, altColor, 0.4F, 0.3F, 120, "Primary 03", false);
            script = composeForPalette(script, altColor, 0.1F, 0.1F, 240, "Secondary 01", false);
            script = composeForPalette(script, altColor, 0.3F, 0.3F, 240, "Secondary 02", false);
            script = composeForPalette(script, altColor, 0.4F, 0.4F, 240, "Secondary 03", false);

            //script = composeForPalette(script, paletteOne, "Alt 1");
            //script = composeForPalette(script, paletteTwo, "Alt 2");
            //script = composeForPalette(script, paletteThree, "Alt 3");
            //script.appendLine(appendType.heading_2, "Base color");

            //script.appendLine(appendType.bold, palette.hexColor);

            //script.s_width(column, 5);

            //script.appendLine(appendType.regular, "Background variations");

            //for (int i = 0; i < palette.ccount; i++)
            //{
            //    script.appendLine(appendType.bold, palette.bgColors[i].toHexColor());
            //    script.s_settings(palette, i, acePaletteShotResEnum.background).isHorizontal = false;
            //}

            //script.add(appendType.regular, "Border variations", false);

            //for (int i = 0; i < palette.ccount; i++)
            //{
            //    script.appendLine(appendType.bold, palette.tpColors[i].toHexColor());
            //    script.s_settings(palette, i, acePaletteShotResEnum.border).isHorizontal = false;
            //}

            //script.add(appendType.regular, "Foreground variations", false);

            //for (int i = 0; i < palette.ccount; i++)
            //{
            //    script.appendLine(appendType.bold, palette.fgColors[i].toHexColor());
            //    script.s_settings(palette, i, acePaletteShotResEnum.foreground).isHorizontal = false;
            //}

            script.x_scopeOut(this);

            return script;
        }

        public docScript composeForPalette(docScript script, Color baseColor, float bright, float sat, int hue, string title, bool doLabels = false)
        {
            script.x_moveToCorner(textCursorZoneCorner.Top);

            script.x_move(textCursorZoneCorner.Right, 1, true);

            script.x_moveToCorner(textCursorZoneCorner.Left);

            if (doLabels)
            {
                script.s_width(0, 13);
            }
            else
            {
                script.s_width(0, 23);
            }

            Color altOne = baseColor; //.getVariation(bright, sat, hue);

            aceColorPalette pal = new aceColorPalette(altOne.ColorToHex()); //.toHexColor());

            if (doLabels)
            {
                script.AppendLine();
                script.AppendLine(appendType.bold, "Title");
                script.AppendLine();

                script.AppendLine();
                script.AppendLine(appendType.heading_2, "Base color");

                script.AppendLine();
                script.AppendLine(appendType.regular, "Bright", appendRole.tableColumnHead);

                script.AppendLine(appendType.regular, "Saturation", appendRole.tableColumnFoot);

                script.AppendLine(appendType.regular, "Hue", appendRole.tableBetween);
            }
            else
            {
                script.AppendLine(appendType.heading_2, "Palette");

                script.AppendLine(appendType.bold, title);
                script.AppendLine();

                script.AppendLine(appendType.heading_2, "Base color", appendRole.tableColumnHead);

                script.s_settings(altOne, Color.Black);
                script.AppendLine(appendType.bold, pal.hexColor);

                script.AppendLine(appendType.heading_2, "Change");
                script.AppendLine(appendType.regular, bright.ToString(), appendRole.tableColumnHead);

                script.AppendLine(appendType.regular, sat.ToString(), appendRole.tableColumnFoot);

                script.AppendLine(appendType.regular, hue.ToString(), appendRole.tableBetween);
            }

            if (doLabels)
            {
                script.AppendLine(appendType.regular, "bgColors");
            }
            else
            {
                script.AppendLine(appendType.regular, "Background variations");
            }

            for (int i = 0; i < pal.ccount; i++)
            {
                if (doLabels)
                {
                    script.AppendLine();
                    script.AppendLine(appendType.bold, i.ToString());
                }
                else
                {
                    script.AppendLine(appendType.bold, pal.bgColors[i].ColorToHex());
                    script.s_settings(pal, i, acePaletteShotResEnum.background).isHorizontal = false;
                }
            }

            if (doLabels)
            {
                script.AppendLine(appendType.regular, "tpColors");
            }
            else
            {
                script.add(appendType.regular, "Border variations", false);
            }

            for (int i = 0; i < pal.ccount; i++)
            {
                if (doLabels)
                {
                    script.AppendLine();
                    script.AppendLine(appendType.bold, i.ToString());
                }
                else
                {
                    script.AppendLine(appendType.bold, pal.tpColors[i].ColorToHex());
                    script.s_settings(pal, i, acePaletteShotResEnum.border).isHorizontal = false;
                }
            }

            if (doLabels)
            {
                script.AppendLine(appendType.regular, "fgColors");
            }
            else
            {
                script.add(appendType.regular, "Foreground variations", false);
            }

            for (int i = 0; i < pal.ccount; i++)
            {
                if (doLabels)
                {
                    script.AppendLine();
                    script.AppendLine(appendType.bold, i.ToString());
                }
                else
                {
                    script.AppendLine(appendType.bold, pal.fgColors[i].ColorToHex());
                    script.s_settings(pal, i, acePaletteShotResEnum.foreground).isHorizontal = false;
                }
            }
            return script;
        }

        public metaPalettePage(string hexBaseColor)
        {
            baseColor = hexBaseColor.ToUpper();
            name = hexBaseColor.Trim('#');
            basicBlocksFlags = metaPageCommonBlockFlags.none;
            settings.tabHeadColor = ColorWorks.GetColor(hexBaseColor); //.getColorFromHex();
            palette = new aceColorPalette(hexBaseColor);
        }
    }
}