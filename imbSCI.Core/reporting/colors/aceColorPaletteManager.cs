// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceColorPaletteManager.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.colors
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.enumworks;
    using imbSCI.Data.data;

    #region imbVeles using

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    //using Xamarin.Forms;

    //using System.Windows.Media;

    //using aceCommonTypes.extensions;

    #endregion imbVeles using

#pragma warning disable CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    /// <summary>
    /// Color and palette cache and constructor
    /// </summary>
    /// \ingroup_disabled report_ll
    /// <seealso cref="aceCommonTypes.primitives.imbBindable" />
    public class aceColorPaletteManager : imbBindable
#pragma warning restore CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    {
        // private static aceColorPaletteForTypes _paletteLibrary = new aceColorPaletteForTypes();

        public static Int32 palletsRetrieved = 0;
        public static Int32 palletsLoaded = 0;
        public static Int32 palletsCreated = 0;
        private static String paletteLib_path = "collection.palette";
        private static String colorLib_path = "colorLib.palette";

        private static Int32 _colorsLoaded;

        /// <summary>
        /// Koliko je novih boja kreirano
        /// </summary>
        public static Int32 colorsLoaded
        {
            get { return _colorsLoaded; }
            set
            {
                _colorsLoaded = value;
                //OnPropertyChanged("colorsCreated");
            }
        }

        #region --- colorsCreated ------- Koliko je novih boja kreirano

        private static Int32 _colorsCreated;

        /// <summary>
        /// Koliko je novih boja kreirano
        /// </summary>
        public static Int32 colorsCreated
        {
            get { return _colorsCreated; }
            set
            {
                _colorsCreated = value;
                //OnPropertyChanged("colorsCreated");
            }
        }

        #endregion --- colorsCreated ------- Koliko je novih boja kreirano

        #region --- colorsRetrieved ------- Koliko je puta boja vracena

        private static Int32 _colorsRetrieved;

        /// <summary>
        /// Koliko je puta boja vracena
        /// </summary>
        public static Int32 colorsRetrieved
        {
            get { return _colorsRetrieved; }
            set
            {
                _colorsRetrieved = value;
                //OnPropertyChanged("colorsRetrieved");
            }
        }

        #endregion --- colorsRetrieved ------- Koliko je puta boja vracena

        #region --- paletteLibrary ------- Biblioteka paleta
        private static aceColorPaletteForTypes _paletteLibrary;
        /// <summary>
        /// Biblioteka paleta
        /// </summary>
        public static aceColorPaletteForTypes paletteLibrary
        {
            get
            {
                if (_paletteLibrary == null)
                {
                    _paletteLibrary = new aceColorPaletteForTypes();
                }
                return _paletteLibrary;
            }
        }
        #endregion

        #region -----------  library  -------  [Biblioteka svih boja ]

        private static aceColorLibrary _library = new aceColorLibrary();

        /// <summary>
        /// Biblioteka svih boja
        /// </summary>
        // [XmlIgnore]
        [Category("aceColorPaletteManager")]
        [DisplayName("library")]
        [Description("Biblioteka svih boja ")]
        public static aceColorLibrary library
        {
            get { return _library; }
            set
            {
                // Boolean chg = (_library != value);
                _library = value;
                // OnPropertyChanged("library");
                // if (chg) {}
            }
        }

        #endregion -----------  library  -------  [Biblioteka svih boja ]

        ///// <summary>
        ///// Vraca kesiran Color objekat prema zadanom baseColorHex i promenama
        ///// </summary>
        ///// <param name="baseColorHex"></param>
        ///// <param name="valueDelta"></param>
        ///// <param name="saturationDelta"></param>
        ///// <param name="hueDelta"></param>
        ///// <returns></returns>
        //public static Color getColor(String baseColorHex, float valueDelta = 0, float saturationDelta = 0,
        //                             Int32 hueDelta = 0, Boolean isForeColor = false, Boolean useBlackWhite = true)
        //{
        //    String path = baseColorHex;

        //    if (!path.Contains("_"))
        //    {
        //        path = makePath(baseColorHex, valueDelta, saturationDelta, hueDelta, isForeColor);
        //    }

        //    if (!library.collection.ContainsKey(path)) //.ContainsKey(path))
        //    {
        //        Color newColor;
        //        aceColorEntry newEntry = null;

        //        if (path.Contains("_foreColor"))
        //        {
        //            newColor = aceColorConverts.getSafeForeground(getColor(path.Replace("_foreColor", "")), useBlackWhite);
        //            newEntry = new aceColorEntry(baseColorHex, valueDelta, saturationDelta, hueDelta, isForeColor, newColor);
        //        }
        //        else
        //        {
        //            if (path.Contains("_vd"))
        //            {
        //                newColor = aceColorConverts.getBrighter(valueDelta, getColor(baseColorHex, 0, 0, 0, false, true),
        //                                                     saturationDelta, hueDelta);
        //                newEntry = new aceColorEntry(baseColorHex, valueDelta, saturationDelta, hueDelta, isForeColor,
        //                                          newColor);
        //            }
        //            else
        //            {
        //                newColor = aceColorConverts.fromHexColor(baseColorHex);
        //                newEntry = new aceColorEntry(baseColorHex, valueDelta, saturationDelta, hueDelta, isForeColor,
        //                                          newColor);
        //            }
        //        }
        //        colorsCreated++;

        //        library.collection.Add(newEntry.path, newEntry); //.Add(path, newColor);
        //    }
        //    else
        //    {
        //        colorsRetrieved++;
        //    }

        //    return (Color)library.collection[path].color;
        //}

        internal static String makePath(String baseColorHex, float valueDelta = 0, float saturationDelta = 0,
                                        Int32 hueDelta = 0, Boolean isForeColor = false)
        {
            String path = "";
            if ((valueDelta == 0) && (saturationDelta == 0) && (hueDelta == 0))
            {
                path = baseColorHex;
            }
            else
            {
                path = baseColorHex + "_vd" + valueDelta.ToString() + "_sd" + saturationDelta.ToString() + "_hd" +
                       hueDelta;
            }

            if (isForeColor) path = path + "_foreColor";

            return path;
        }

        public static aceColorPalette getDefaultPalette()
        {
            
            return paletteLibrary.defaultPalette;
        }

        /// <summary>
        /// Ovo da koriste Manager staticke klase kada treba da oboje klase koji nisu iz mojih biblioteka
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="basicColor"></param>
        /// <param name="_iconKey">Preporucena ikonica</param>
        public static aceColorPalette getPalette(String palleteName, String basicColor = "", String _iconKey = "")
        {
            //throw new NotImplementedException();
            if (!paletteLibrary.collection.ContainsKey(palleteName))
            {
                if (String.IsNullOrEmpty(basicColor)) return getDefaultPalette();
                List<acePaletteVariationRole> enumValues = Enum.GetValues(typeof(acePaletteVariationRole)).getFlatList<acePaletteVariationRole>();

                aceColorPalette cp = new aceColorPalette(basicColor, 0.1F, 0, 0.05F, 0.05F, acePaletteRole.none.ToInt32() + 1, true, palleteName);
                cp.iconKey = _iconKey;
                paletteLibrary.collection.Add(palleteName, cp);
                palletsCreated++;
            }
            palletsRetrieved++;
            return paletteLibrary.collection[palleteName];
            //return null;
        }

        //public static aceColorPalette getPaletteInfoBase(imbInfoBase sourceInfo)
        //{
        //    //String codeName = getPaletteForType()
        //    //return getPalette(codeName, sourceInfo.baseColor, sourceInfo.menuIcon);
        //}
    }
}