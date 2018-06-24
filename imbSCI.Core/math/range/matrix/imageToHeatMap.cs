// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imageToHeatMap.cs" company="imbVeles" >
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

//using Accord.Imaging.Filters;
using imbSCI.Core.extensions.io;
using imbSCI.Core.reporting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace imbSCI.Core.math.range.matrix
{
    public static class proceduralHeatMapGeneration { }

    /// <summary>
    /// Static methods for <see cref="HeatMapModel"/> creation from images
    /// </summary>
    public static class imageToHeatMap
    {
        /// <summary>
        /// Gets average intesity of sample patch located at x/y
        /// </summary>
        /// <param name="grayImage">The gray image.</param>
        /// <param name="x">horizobtal index of the field</param>
        /// <param name="y">vertical index of the field</param>
        /// <param name="bs">Sample patch size (side of the sampling square, in pixels)</param>
        /// <returns>Mean brightness value for the patch at x,y</returns>
        public static Double GetIntesity(this Bitmap grayImage, Int32 x, Int32 y, Int32 bs)
        {
            List<Double> vl = new List<double>();

            Int32 startX = x * bs;
            Int32 startY = y * bs;

            for (int i = 0; i < bs; i++)
            {
                for (int j = 0; j < bs; j++)
                {
                    Double px = grayImage.GetPixel(i + startX, j + startY).GetBrightness();

                    vl.Add(px);
                }
            }

            return vl.Average();
        }

        /// <summary>
        /// Creates a heat map model from image file. Supported formats: JPEG, PNG, TIFF etc. See: <see cref="Image.FromFile(string, bool)"/>
        /// </summary>
        /// <param name="imageFile">Path of the image file.</param>
        /// <param name="widthOfMap">The width of map - number of horizontal fields, vertical are computed in respect to image dimensional proportion </param>
        /// <param name="loger">The loger.</param>
        /// <returns>Heat map model, fields have mean brightness value</returns>
        public static HeatMapModel CreateFromImage(String imageFile, Int32 widthOfMap = 1000, ILogBuilder loger = null)
        {
            Bitmap grayImage = null;

            var imgFile = imageFile.getWritableFile(Data.enums.getWritableFileMode.existing, loger);
            if (!imgFile.Exists)
            {
                loger.log("Loading image file failed (" + imageFile + ") for heatmap model mapping");
            }
            else
            {
                try
                {
                    Bitmap sbmp = new Bitmap(Image.FromFile(imgFile.FullName));
                    grayImage = sbmp;
                    //  grayImage = Grayscale.CommonAlgorithms.Y.Apply(sbmp);
                }
                catch (Exception ex)
                {
                    loger.log("Error in loading file (" + imageFile + "): " + ex.Message);
                }
            }

            Int32 heightOfMap = Convert.ToInt32(grayImage.Size.Height.GetRatio(grayImage.Size.Width) * widthOfMap);
            HeatMapModel map = HeatMapModel.Create(widthOfMap, heightOfMap, "D3");

            map.properties.addAndDescribeKey("grayImage", grayImage, false, false);
            map.properties.addAndDescribeKey("source", imageFile, false, false);

            map.AllocateSize(widthOfMap, heightOfMap);

            Int32 bs = Convert.ToInt32(grayImage.Size.Width / widthOfMap);

            map.properties.addAndDescribeKey("fieldSize", bs, false, false);

            for (Int32 y = 0; y < heightOfMap; y++)
            {
                for (Int32 x = 0; x < widthOfMap; x++)
                {
                    map[x, y] = grayImage.GetIntesity(x, y, bs);
                }
            }

            return map;
        }
    }
}