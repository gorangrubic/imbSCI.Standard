// --------------------------------------------------------------------------------------------------------------------
// <copyright file="styleTheme.cs" company="imbVeles" >
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
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.geometrics;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Core.reporting.style.shot;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data;
    using imbSCI.Data.data;
    using imbSCI.Data.enums.reporting;
    using imbSCI.Data.interfaces;
    using System;
    using System.ComponentModel;
    using System.Data;

    /// <summary>
    /// Low-level object with all the most important styling features of report page or other output
    /// </summary>
    public sealed class styleTheme : imbBindable, IAppendDataFields, IGetCodeName
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="styleTheme"/> class.
        /// </summary>
        /// <param name="h1Size">Size of the h1.</param>
        /// <param name="pSize">Size of the p.</param>
        /// <param name="margin">The margin.</param>
        /// <param name="padding">The padding.</param>
        /// <param name="pageFontName">Name of the page font.</param>
        /// <param name="headingFontName">Name of the heading font.</param>
        public styleTheme(aceBaseColorSetEnum colorSet, Int32 h1Size, Int32 pSize, fourSideSetting margin, fourSideSetting padding,
                        aceFont pageFontName, aceFont headingFontName = aceFont.none)
        {
            palletes = new acePaletteProvider(colorSet.getSetOfColors());

            textShotProvider = new styleTextShotProvider(this);
            styleContainerProvider = new styleContainerShotProvider(this);
            borderProvider = new styleBorderProvider(this);
            styler = new stylerForRange(this);

            zoneMain = new cursorZone(80, 2, 2);
            cMain = new cursor(zoneMain, textCursorMode.scroll, textCursorZone.innerZone, "themeCursor");

            body = new stylePage();
            //body.maxSize

            fontForText = new styleTextFont(pageFontName);
            fontForHeadings = new styleTextFont(headingFontName);
            fontSize = new styleTextSizeSet(h1Size, pSize, margin, padding);
        }

        public styleTheme()
        {
        }

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

            foreach (var palPair in palletes)
            {
                output = imbSciStringExtensions.add(output, palPair.Value.hexColor, "-");
            }

            output = "|".adds("-", fontForHeadings.fontName, fontForText.fontName, fontSize.getCodeName(), "|");
            codeName = output;
            return output;
        }

        private String codeName = "";

        /// <summary>
        /// Appends its data points into new or existing property collection
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>Updated or newly created property collection</returns>
        public PropertyCollection AppendDataFields(PropertyCollection data = null)
        {
            if (data == null) data = new PropertyCollection();

            builderForStyle styler = new builderForStyle();
            if (body.maxSize.height > -1)
            {
                styler.AppendLine("max-height: " + imbSciStringExtensions.ensureEndsWith(body.maxSize.height.ToString().a("px"), ";"));
            }
            styler.AppendLine("max-width: " + imbSciStringExtensions.ensureEndsWith(body.maxSize.width.ToString().a("px"), ";"));
            styler.AppendLine(body.margins.ToString("padding", styleToStringFormat.multilineCss));

            String fontFamily = fontForText.getFontFamilyLine();

            styler.AppendLine("font-family: \"" + fontFamily + "\";");

            data[templateFieldStyleInserts.style_body] = styler.ContentToString(true);

            fontFamily = fontForHeadings.getFontFamilyLine();
            styler.AppendLine("font-family: \"" + fontFamily + "\";");

            data[templateFieldStyleInserts.style_h] = styler.ContentToString(true);

            fontSize.AppendDataFields(data);

            data[templateFieldStyleInserts.style_article] = "";
            data[templateFieldStyleInserts.style_section] = "";
            data[templateFieldStyleInserts.style_appendix] = "";
            data[templateFieldStyleInserts.style_lists] = "";
            data[templateFieldStyleInserts.style_header] = "";
            data[templateFieldStyleInserts.style_nav] = "";
            data[templateFieldStyleInserts.style_items] = "";
            data[templateFieldStyleInserts.style_colora_css] = palletes[acePaletteRole.colorA].getCSS(acePaletteRole.colorA.ToString());
            data[templateFieldStyleInserts.style_colorb_css] = palletes[acePaletteRole.colorB].getCSS(acePaletteRole.colorB.ToString());
            data[templateFieldStyleInserts.style_colorc_css] = palletes[acePaletteRole.colorC].getCSS(acePaletteRole.colorC.ToString());

            //palletes[acePaletteRole.colorA].AppendDataFields(data);

            //styler.AppendLine("float: left;");
            //styler.AppendLine("height: 32px;");
            //styler.AppendLine("");
            //styler.AppendLine("border-bottom-style: Solid;");
            //styler.AppendLine("border-bottom-width: 3px;");

            data[templateFieldStyleInserts.style_menu] = styler.ContentToString(true);
            //this.buildPropertyCollection(false, false, "target", data);

            // data[target.target_name] = name;
            // data[target.target_description] = description;
            // data[target.target_id] = id;
            // data[target.target_url] = url;
            return data;
        }

        #region --- styler ------- variator for automatic styling

        private stylerForRange _styler;

        /// <summary>
        /// variator for automatic styling
        /// </summary>
        public stylerForRange styler
        {
            get
            {
                return _styler;
            }
            set
            {
                _styler = value;
                OnPropertyChanged("styler");
            }
        }

        #endregion --- styler ------- variator for automatic styling

        private IStyleInstruction _activeStyleInstruction;

        /// <summary>
        /// Style instruction to be applied
        /// </summary>
        public IStyleInstruction activeStyleInstruction
        {
            get { return _activeStyleInstruction; }
            set { _activeStyleInstruction = value; }
        }

        #region ----------- Boolean [ doAutoStyleByType ] -------  [if TRUE <c>IRender</c> will on appending provide styleShotSet automatically for <c>activeStyleInstruction</c> property]

        private Boolean _doAutoStyleByType = false;

        /// <summary>
        /// if TRUE it will provide styleShotSet automatically for <c>activeStyleInstruction</c> property
        /// </summary>
        [Category("Switches")]
        [DisplayName("doAutoStyleByType")]
        [Description("if TRUE it will provide styleShotSet automatically for <c>activeStyleInstruction</c> property")]
        public Boolean doAutoStyleByType
        {
            get { return _doAutoStyleByType; }
            set { _doAutoStyleByType = value; OnPropertyChanged("doAutoStyleByType"); }
        }

        #endregion ----------- Boolean [ doAutoStyleByType ] -------  [if TRUE <c>IRender</c> will on appending provide styleShotSet automatically for <c>activeStyleInstruction</c> property]

        #region --- c ------- cursor

        private cursor _c;

        /// <summary>
        /// The main cursor shared between executor, renderer etc
        /// </summary>
        public cursor cMain
        {
            get
            {
                return _c;
            }
            set
            {
                _c = value;
                OnPropertyChanged("c");
            }
        }

        #endregion --- c ------- cursor

        #region --- cursorZone ------- zone of text format

        private cursorZone _zone;

        /// <summary>
        /// the main zone of text format
        /// </summary>
        public cursorZone zoneMain
        {
            get
            {
                return _zone;
            }
            set
            {
                _zone = value;
                OnPropertyChanged("zone");
            }
        }

        #endregion --- cursorZone ------- zone of text format

        #region -----------  palletes  -------  [collection of palletes]

        private acePaletteProvider _palletes; // = new acePaletteCollection();

        /// <summary>
        /// collection of palletes
        /// </summary>
        // [XmlIgnore]
        [Category("styleTheme")]
        [DisplayName("palletes")]
        [Description("collection of palletes")]
        public acePaletteProvider palletes
        {
            get
            {
                return _palletes;
            }
            private set
            {
                // Boolean chg = (_palletes != value);
                _palletes = value;
                OnPropertyChanged("palletes");
                // if (chg) {}
            }
        }

        #endregion -----------  palletes  -------  [collection of palletes]

        #region -----------  textShotProvider  -------  [Provider for text shots]

        private styleTextShotProvider _textShotProvider;

        /// <summary>
        /// Provider for text shots
        /// </summary>
        // [XmlIgnore]
        [Category("styleTheme")]
        [DisplayName("textShotProvider")]
        [Description("Provider for text shots")]
        public styleTextShotProvider textShotProvider
        {
            get
            {
                return _textShotProvider;
            }
            private set
            {
                // Boolean chg = (_textShotProvider != value);
                _textShotProvider = value;
                OnPropertyChanged("textShotProvider");
                // if (chg) {}
            }
        }

        #endregion -----------  textShotProvider  -------  [Provider for text shots]

        private styleContainerShotProvider _styleContainerProvider;

        /// <summary>
        /// Provider for text shots
        /// </summary>
        // [XmlIgnore]
        [Category("styleTheme")]
        [DisplayName("textShotProvider")]
        [Description("Provider for text shots")]
        public styleContainerShotProvider styleContainerProvider
        {
            get
            {
                return _styleContainerProvider;
            }
            private set
            {
                // Boolean chg = (_textShotProvider != value);
                _styleContainerProvider = value;
                OnPropertyChanged("styleContainerProvider");
                // if (chg) {}
            }
        }

        #region -----------  borderProvider  -------  [provider for container borders]

        private styleBorderProvider _borderProvider; // = new styleBorderProvider();

        /// <summary>
        /// provider for container borders
        /// </summary>
        // [XmlIgnore]
        [Category("styleTheme")]
        [DisplayName("borderProvider")]
        [Description("provider for container borders")]
        public styleBorderProvider borderProvider
        {
            get
            {
                return _borderProvider;
            }
            set
            {
                // Boolean chg = (_borderProvider != value);
                _borderProvider = value;
                OnPropertyChanged("borderProvider");
                // if (chg) {}
            }
        }

        #endregion -----------  borderProvider  -------  [provider for container borders]

        #region -----------  body  -------  [Style definition for body]

        private stylePage _body = new stylePage();

        /// <summary>
        /// Style definition for body
        /// </summary>
        // [XmlIgnore]
        [Category("style")]
        [DisplayName("body")]
        [Description("Style definition for body")]
        public stylePage body
        {
            get
            {
                return _body;
            }
            private set
            {
                // Boolean chg = (_body != value);
                _body = value;
                OnPropertyChanged("body");
                // if (chg) {}
            }
        }

        #endregion -----------  body  -------  [Style definition for body]

        #region -----------  fontSize  -------  [complete setup for heading and normal tags]

        private styleTextSizeSet _fontSize; // = new styleTextSizeSet();

        /// <summary>
        /// complete setup for heading and normal tags
        /// </summary>
        // [XmlIgnore]
        [Category("style")]
        [DisplayName("fontSize")]
        [Description("complete setup for heading and normal tags")]
        public styleTextSizeSet fontSize
        {
            get
            {
                return _fontSize;
            }
            private set
            {
                // Boolean chg = (_fontSize != value);
                _fontSize = value;
                OnPropertyChanged("fontSize");
                // if (chg) {}
            }
        }

        #endregion -----------  fontSize  -------  [complete setup for heading and normal tags]

        #region -----------  fontForText  -------  [font used for text]

        private styleTextFont _fontForText; // = aceFont.Helvetica; // = new aceFont();

        /// <summary>
        /// font used for text
        /// </summary>
        // [XmlIgnore]
        [Category("style")]
        [DisplayName("fontForText")]
        [Description("font used for text")]
        public styleTextFont fontForText
        {
            get
            {
                return _fontForText;
            }
            private set
            {
                // Boolean chg = (_fontForText != value);
                _fontForText = value;
                OnPropertyChanged("fontForText");
                // if (chg) {}
            }
        }

        #endregion -----------  fontForText  -------  [font used for text]

        #region -----------  fontForHeadings  -------  [Font for heading tags]

        private styleTextFont _fontForHeadings; //= styleTextFont.Impact;// = new aceFont();

        /// <summary>
        /// Font for heading tags
        /// </summary>
        // [XmlIgnore]
        [Category("style")]
        [DisplayName("fontForHeadings")]
        [Description("Font for heading tags")]
        public styleTextFont fontForHeadings
        {
            get
            {
                return _fontForHeadings;
            }
            private set
            {
                // Boolean chg = (_fontForHeadings != value);
                _fontForHeadings = value;
                OnPropertyChanged("fontForHeadings");
                // if (chg) {}
            }
        }

        #endregion -----------  fontForHeadings  -------  [Font for heading tags]
    }
}