// --------------------------------------------------------------------------------------------------------------------
// <copyright file="styleTextFont.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Data;
    using imbSCI.Data.data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;

#if NETCORE

    //public class FontFamily
    //{
    //    public String Name { get; set; } = "";

    //    public FontFamily(String name)
    //    {
    //        Name = name;

    //    }
    //}

    //[Flags]
    //public enum FontStyle
    //{
    //    Regular,
    //    Italic,
    //    Underline,
    //    Bold,
    //    Strikeout

    //}

    //public class Font
    //{
    //    public String Name { get; set; }

    //    public FontFamily FontFamily { get; set;  }
    //}

    //public static class SystemFonts
    //{
    //    public static Font CaptionFont { get; internal set; }

    //    public static Font GetFontByName(this String fontName)
    //    {
    //        return new Font();
    //    }
    //}

#endif

    /// <summary>
    /// Font descriptor with some extra data
    /// </summary>
    /// <seealso cref="aceCommonTypes.primitives.imbBindable" />
    /// \ingroup_disabled report_ll_style
    public class styleTextFont : imbBindable
    {
        /// <summary>
        /// Single-font constructor
        /// </summary>
        /// <param name="font"></param>
        public styleTextFont(aceFont font)
        {
            Add(font);
        }

        /// <summary>
        /// Adds the specified font.
        /// </summary>
        /// <param name="font">The font.</param>
        internal void Add(aceFont font)
        {
            if (font == aceFont.none) return;

            fontName = font.ToString().imbTitleCamelOperation(true);
            var newFont = SystemFonts.GetFontByName(fontName);
            if (newFont != null)
            {
                if (drawingFont == null)
                {
                    drawingFont = newFont;
                    family = drawingFont.FontFamily;
                }
                systemFontList.Add(newFont);
            }

            if (_systemFont == null)
            {
                _systemFont = SystemFonts.CaptionFont;
            }
        }

        /// <summary>
        /// Constructor with multiple fonts load
        /// </summary>
        /// <param name="fontNames"></param>
        public styleTextFont(params aceFont[] fontNames)
        {
            foreach (aceFont font in fontNames)
            {
                Add(font);
            }
        }

        /// <summary>
        /// Returns font family line applicable for CSS and HTML
        /// </summary>
        /// <returns></returns>
        public String getFontFamilyLine()
        {
            String output = "";

            foreach (Font font in systemFontList)
            {
                output = imbSciStringExtensions.add(output, font.Name, ",");
            }
            if (imbSciStringExtensions.isNullOrEmptyString(output))
            {
                output = imbSciStringExtensions.add(output, fontName, ",");
            }
            return imbSciStringExtensions.add(output, ";");
        }

        #region --- family ------- Font Family name

        private FontFamily _family = new FontFamily("Tahoma");

        /// <summary>
        /// Font Family name
        /// </summary>
        public FontFamily family
        {
            get
            {
                return _family;
            }
            set
            {
                _family = value;
                OnPropertyChanged("family");
            }
        }

        #endregion --- family ------- Font Family name

        #region -----------  systemFontList  -------  [Complete list of system fonts]

        private List<Font> _systemFontList = new List<Font>(); // = new List<Font>();

                                                               /// <summary>
                                                               /// Complete list of system fonts
                                                               /// </summary>
        // [XmlIgnore]
        [Category("styleTextFont")]
        [DisplayName("systemFontList")]
        [Description("Complete list of system fonts")]
        public List<Font> systemFontList
        {
            get
            {
                return _systemFontList;
            }
            set
            {
                // Boolean chg = (_systemFontList != value);
                _systemFontList = value;
                OnPropertyChanged("systemFontList");
                // if (chg) {}
            }
        }

        #endregion -----------  systemFontList  -------  [Complete list of system fonts]

        #region -----------  systemFont  -------  [reference to System font]

        private Font _systemFont; // = new Font();

                                  /// <summary>
                                  /// reference to System font
                                  /// </summary>
        // [XmlIgnore]
        [Category("styleTextFont")]
        [DisplayName("systemFont")]
        [Description("reference to System font")]
        public Font drawingFont
        {
            get
            {
                return _systemFont;
            }
            set
            {
                // Boolean chg = (_systemFont != value);
                _systemFont = value;
                OnPropertyChanged("systemFont");
                // if (chg) {}
            }
        }

        #endregion -----------  systemFont  -------  [reference to System font]

        #region -----------  style  -------  [Font style]

        private FontStyle _style = FontStyle.Regular; // = new FontStyle();

                                                      /// <summary>
                                                      /// Font style
                                                      /// </summary>
        // [XmlIgnore]
        [Category("styleTextFont")]
        [DisplayName("style")]
        [Description("Font style")]
        public FontStyle style
        {
            get
            {
                return _style;
            }
            set
            {
                // Boolean chg = (_style != value);
                _style = value;
                OnPropertyChanged("style");
                // if (chg) {}
            }
        }

        #endregion -----------  style  -------  [Font style]

        #region -----------  fontName  -------  [font name - to be used even if systemFont fails]

        private String _fontName = ""; // = new String();

                                       /// <summary>
                                       /// font name - to be used even if systemFont fails
                                       /// </summary>
        // [XmlIgnore]
        [Category("styleTextFont")]
        [DisplayName("fontName")]
        [Description("font name - to be used even if systemFont fails")]
        public String fontName
        {
            get
            {
                return _fontName;
            }
            set
            {
                // Boolean chg = (_fontName != value);
                _fontName = value;
                OnPropertyChanged("fontName");
                // if (chg) {}
            }
        }

        #endregion -----------  fontName  -------  [font name - to be used even if systemFont fails]
    }
}