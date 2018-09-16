// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cursorZoneExtensions.cs" company="imbVeles" >
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

    public static class cursorZoneExtensions
    {
        /// <summary>
        /// Gets the preset spatial settings based on preset
        /// </summary>
        /// <param name="preset">The preset.</param>
        /// <returns></returns>
        public static cursorZoneSpatialSettings getPresetSpatialSettings(this cursorZoneSpatialPreset preset)
        {
            var output = new cursorZoneSpatialSettings();
            output.setPresetSpatialSettings(preset);
            return output;
        }

        /// <summary>
        /// Deplozs spatial settings from named preset
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="preset">The preset.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static cursorZoneSpatialSettings setPresetSpatialSettings(this cursorZoneSpatialSettings output, cursorZoneSpatialPreset preset)
        {
            if (output == null) output = new cursorZoneSpatialSettings();

            switch (preset)
            {
                case cursorZoneSpatialPreset.sheetNormal:
                    output.width = 12;
                    output.height = 240;
                    output.spatialUnit = 120;
                    output.spatialUnitRatioYPerX = 0.2F;
                    output.spatialUnitMarginRatio = 0.1F;
                    output.spatialUnitPaddingRatio = 0.2F;
                    output.tabPerCellUnit = 1;
                    output.padding = new fourSideSetting(2);
                    output.margin = new fourSideSetting(1);
                    break;

                case cursorZoneSpatialPreset.sheetSquareCell:
                    output.width = 120;
                    output.height = 120;
                    output.spatialUnit = 25;
                    output.spatialUnitRatioYPerX = 1.0F;
                    output.tabPerCellUnit = 4;
                    break;

                case cursorZoneSpatialPreset.textPage:
                    output.width = 120;
                    output.height = 120;
                    output.spatialUnit = 10;
                    output.spatialUnitRatioYPerX = 1.6F;
                    output.tabPerCellUnit = 8;
                    break;

                case cursorZoneSpatialPreset.console:
                    output.width = 85;
                    output.height = 43;
                    output.spatialUnit = 5;
                    output.spatialUnitRatioYPerX = 1.6F;
                    output.tabPerCellUnit = 4;
                    break;

                case cursorZoneSpatialPreset.wideConsole:
                    output.width = 160;
                    output.height = 78;
                    output.spatialUnit = 5;
                    output.spatialUnitRatioYPerX = 1.6F;
                    output.tabPerCellUnit = 4;
                    break;

                case cursorZoneSpatialPreset.longTextLog:
                    output.width = 85;
                    output.height = 1200;
                    output.spatialUnit = 5;
                    output.spatialUnitRatioYPerX = 1.6F;
                    output.tabPerCellUnit = 4;
                    break;

                case cursorZoneSpatialPreset.a4OnFont10pt:
                    output.width = 90;
                    output.height = 62;
                    output.spatialUnit = 10;
                    output.spatialUnitRatioYPerX = 1.6F;
                    output.tabPerCellUnit = 8;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return output;
        }

        /// <summary>
        /// Builds the zone.
        /// </summary>
        /// <param name="layoutPreset">The layout preset.</param>
        /// <param name="spatialPreset">The spatial preset.</param>
        /// <param name="target">The target.</param>
        /// <returns>Resulting zone</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">If Layout presed never implemented</exception>
        public static cursorZone setZoneStructure(this cursorZone target, cursorZoneLayoutPreset layoutPreset, Boolean removeExistingSubzones = true)
        {
            if (target == null) target = new cursorZone();

            //target.subzones

            cursorZoneBuilder builder = new cursorZoneBuilder(target);
            target.margin = new fourSideSetting(0);
            target.padding = new fourSideSetting(1);

            switch (layoutPreset)
            {
                case cursorZoneLayoutPreset.none:
                    break;

                case cursorZoneLayoutPreset.oneFullPage:
                    break;

                case cursorZoneLayoutPreset.fourColumn:
                    builder.buildColumns(4, 1, 1, 1, 1);

                    break;

                case cursorZoneLayoutPreset.oneMajorTwoMinorColumn:
                    builder.buildColumns(3, 3, 1, 1);

                    break;

                case cursorZoneLayoutPreset.oneMinorTwoMajorColumn:
                    builder.buildColumns(3, 1, 2, 2);

                    break;

                case cursorZoneLayoutPreset.twoColumn:
                    builder.buildColumns(2, 1, 1, 1, 1);
                    break;

                case cursorZoneLayoutPreset.headFourColumn:
                    builder.buildColumns(4, 1, 1, 1, 1);
                    break;

                case cursorZoneLayoutPreset.headTwoColumn:
                    builder.buildColumns(2, 1, 1, 1, 1);
                    break;
                //case cursorZoneLayoutPreset.headFullPage:
                //    break;
                case cursorZoneLayoutPreset.headFourColumnFooter:
                    builder.buildColumns(4, 1, 1, 1, 1);
                    break;

                case cursorZoneLayoutPreset.headTwoColumnFooter:
                    builder.buildColumns(2, 1, 1, 1, 1);
                    break;
                //case cursorZoneLayoutPreset.headFullPageFooter:

                //    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return target;
        }
    }
}