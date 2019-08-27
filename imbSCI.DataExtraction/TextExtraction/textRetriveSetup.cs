// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textRetriveSetup.cs" company="imbVeles" >
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
// Project: imbNLP.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataExtraction.TextExtraction
{
    #region imbVELES USING

    using imbSCI.Data.data;
    using System.ComponentModel;

    #endregion imbVELES USING

    /// <summary>
    /// Podešavanja vezana za vađenje teksta iz XmlNode-a, odnosno iz HTML-a
    /// </summary>
    public class textRetriveSetup : imbBindable
    {
        #region -----------  insertNewLine  -------  [Da li da ubaci dodatni newLine posle svakog taga]

        private bool _insertNewLine; // = new Boolean();

        /// <summary>
        /// Da li da ubaci dodatni newLine posle svakog taga
        /// </summary>
        // [XmlIgnore]
        [Category("textRetriveSetup")]
        [DisplayName("insertNewLine")]
        [Description("Da li da ubaci dodatni newLine posle svakog taga")]
        public bool insertNewLine
        {
            get { return _insertNewLine; }
            set
            {
                _insertNewLine = value;
                OnPropertyChanged("insertNewLine");
            }
        }

        #endregion -----------  insertNewLine  -------  [Da li da ubaci dodatni newLine posle svakog taga]

        #region ----- STRUKTURNI TAGOVI

        #region -----------  div  -------  [Rezim obrade za div]

        private textRetrive_structure _divExtractMode = textRetrive_structure.newLine;

        /// <summary>
        /// Rezim obrade za
        /// </summary>
        [Category("Extraction")]
        [DisplayName("divExtractMode")]
        [Description("Rezim obrade za div")]
        public textRetrive_structure divExtractMode
        {
            get { return _divExtractMode; }
            set
            {
                _divExtractMode = value;
                OnPropertyChanged("divExtractMode");
            }
        }

        #endregion -----------  div  -------  [Rezim obrade za div]

        #region -----------  span  -------  [Rezim obrade za span]

        private textRetrive_structure _spanExtractMode = textRetrive_structure.spaceInline;

        /// <summary>
        /// Rezim obrade za
        /// </summary>
        [Category("Extraction")]
        [DisplayName("spanExtractMode")]
        [Description("Rezim obrade za span")]
        public textRetrive_structure spanExtractMode
        {
            get { return _spanExtractMode; }
            set
            {
                _spanExtractMode = value;
                OnPropertyChanged("spanExtractMode");
            }
        }

        #endregion -----------  span  -------  [Rezim obrade za span]

        #region -----------  a  -------  [Rezim obrade za a]

        private textRetrive_structure _aExtractMode = textRetrive_structure.spaceInline;

        /// <summary>
        /// Rezim obrade za
        /// </summary>
        [Category("Extraction")]
        [DisplayName("aExtractMode")]
        [Description("Rezim obrade za a")]
        public textRetrive_structure aExtractMode
        {
            get { return _aExtractMode; }
            set
            {
                _aExtractMode = value;
                OnPropertyChanged("aExtractMode");
            }
        }

        #endregion -----------  a  -------  [Rezim obrade za a]

        #region -----------  li  -------  [Rezim obrade za li]

        private textRetrive_structure _liExtractMode = textRetrive_structure.normal;

        /// <summary>
        /// Rezim obrade za
        /// </summary>
        [Category("Extraction")]
        [DisplayName("li Extract Mode")]
        [Description("Rezim obrade za li")]
        public textRetrive_structure liExtractMode
        {
            get { return _liExtractMode; }
            set
            {
                _liExtractMode = value;
                OnPropertyChanged("liExtractMode");
            }
        }

        #endregion -----------  li  -------  [Rezim obrade za li]

        #region -----------  lu  -------  [Rezim obrade za lu]

        private textRetrive_structure _luExtractMode = textRetrive_structure.newLine;

        /// <summary>
        /// Rezim obrade za
        /// </summary>
        [Category("Extraction")]
        [DisplayName("lu Extract Mode")]
        [Description("Rezim obrade za lu")]
        public textRetrive_structure luExtractMode
        {
            get { return _luExtractMode; }
            set
            {
                _luExtractMode = value;
                OnPropertyChanged("luExtractMode");
            }
        }

        #endregion -----------  lu  -------  [Rezim obrade za lu]

        #region -----------  td  -------  [Rezim obrade za td]

        private textRetrive_structure _tdExtractMode = textRetrive_structure.spaceInline;

        /// <summary>
        /// Rezim obrade za
        /// </summary>
        [Category("Extraction")]
        [DisplayName("td Extract Mode")]
        [Description("Rezim obrade za td")]
        public textRetrive_structure tdExtractMode
        {
            get { return _tdExtractMode; }
            set
            {
                _tdExtractMode = value;
                OnPropertyChanged("tdExtractMode");
            }
        }

        #endregion -----------  td  -------  [Rezim obrade za td]

        #region -----------  tr  -------  [Rezim obrade za tr]

        private textRetrive_structure _trExtractMode = textRetrive_structure.newLine;

        /// <summary>
        /// Rezim obrade za
        /// </summary>
        [Category("Extraction")]
        [DisplayName("tr Extract Mode")]
        [Description("Rezim obrade za tr")]
        public textRetrive_structure trExtractMode
        {
            get { return _trExtractMode; }
            set
            {
                _trExtractMode = value;
                OnPropertyChanged("trExtractMode");
            }
        }

        #endregion -----------  tr  -------  [Rezim obrade za tr]

        #endregion ----- STRUKTURNI TAGOVI

        #region -----------  p  -------  [Rezim obrade za p]

        private textRetrive_structure _pExtractMode = textRetrive_structure.normal;

        /// <summary>
        /// Rezim obrade za
        /// </summary>
        [Category("Extraction")]
        [DisplayName("p Extract Mode")]
        [Description("Rezim obrade za p")]
        public textRetrive_structure pExtractMode
        {
            get { return _pExtractMode; }
            set
            {
                _pExtractMode = value;
                OnPropertyChanged("pExtractMode");
            }
        }

        #endregion -----------  p  -------  [Rezim obrade za p]

        #region -----------  heading  -------  [Rezim obrade za heading]

        private textRetrive_structure _headingExtractMode = textRetrive_structure.normal;

        /// <summary>
        /// Rezim obrade za
        /// </summary>
        [Category("Extraction")]
        [DisplayName("heading Extract Mode")]
        [Description("Rezim obrade za heading")]
        public textRetrive_structure headingExtractMode
        {
            get { return _headingExtractMode; }
            set
            {
                _headingExtractMode = value;
                OnPropertyChanged("headingExtractMode");
            }
        }

        #endregion -----------  heading  -------  [Rezim obrade za heading]

        #region -----------  comment  -------  [Rezim obrade za comment]

        private textRetrive_structure _commentExtractMode = textRetrive_structure.ignore;

        /// <summary>
        /// Rezim obrade za
        /// </summary>
        [Category("Extraction")]
        [DisplayName("comment Extract Mode")]
        [Description("Rezim obrade za comment")]
        public textRetrive_structure commentExtractMode
        {
            get { return _commentExtractMode; }
            set
            {
                _commentExtractMode = value;
                OnPropertyChanged("commentExtractMode");
            }
        }

        #endregion -----------  comment  -------  [Rezim obrade za comment]

        #region -----------  table  -------  [Kako se ponasa kada naleti na TABLE tag - obrada table]

        private textRetrive_table _tableExtractMode = textRetrive_table.ignore;

        /// <summary>
        /// Kako se ponasa kada naleti na TABLE tag - obrada
        /// </summary>
        [Category("Extraction")]
        [DisplayName("table Extract Mode")]
        [Description("Kako se ponasa kada naleti na TABLE tag - obrada table")]
        public textRetrive_table tableExtractMode
        {
            get { return _tableExtractMode; }
            set
            {
                _tableExtractMode = value;
                OnPropertyChanged("tableExtractMode");
            }
        }

        #endregion -----------  table  -------  [Kako se ponasa kada naleti na TABLE tag - obrada table]

        #region -----------  metaSpace  -------  [Rezim obrade za metaSpace]

        private textRetrive_structure _metaSpaceExtractMode = textRetrive_structure.normal;

        /// <summary>
        /// Rezim obrade za
        /// </summary>
        [Category("Extraction")]
        [DisplayName("metaSpace Extract Mode")]
        [Description("Rezim obrade za metaSpace")]
        public textRetrive_structure metaSpaceExtractMode
        {
            get { return _metaSpaceExtractMode; }
            set
            {
                _metaSpaceExtractMode = value;
                OnPropertyChanged("metaSpaceExtractMode");
            }
        }

        #endregion -----------  metaSpace  -------  [Rezim obrade za metaSpace]

        #region -----------  meta  -------  [Rezim obrade za meta]

        private textRetrive_meta _metaExtractMode = textRetrive_meta.onlyDescriptionAndKeywords;

        /// <summary>
        /// Rezim obrade za
        /// </summary>
        [Category("Extraction")]
        [DisplayName("meta Extract Mode")]
        [Description("Rezim obrade za meta")]
        public textRetrive_meta metaExtractMode
        {
            get { return _metaExtractMode; }
            set
            {
                _metaExtractMode = value;
                OnPropertyChanged("metaExtractMode");
            }
        }

        #endregion -----------  meta  -------  [Rezim obrade za meta]

        #region -----------  links  -------  [Koje vrednosti iz linka prikazuje u extraktu :: Rezim obrade za links]

        private textRetrive_links _linksExtractMode = textRetrive_links.title;

        /// <summary>
        /// Koje vrednosti iz linka prikazuje u extraktu :: Rezim obrade za
        /// </summary>
        [Category("Extraction")]
        [DisplayName("links Extract Mode")]
        [Description("Koje vrednosti iz linka prikazuje u extraktu :: Rezim obrade za links")]
        public textRetrive_links linksExtractMode
        {
            get { return _linksExtractMode; }
            set
            {
                _linksExtractMode = value;
                OnPropertyChanged("linksExtractMode");
            }
        }

        #endregion -----------  links  -------  [Koje vrednosti iz linka prikazuje u extraktu :: Rezim obrade za links]

        #region -----------  inlineSpace  -------  [Koliko da odmakne kada je inline opcija]

        private string _inlineSpace = "    "; // = new String();

        /// <summary>
        /// Koliko da odmakne kada je inline opcija
        /// </summary>
        // [XmlIgnore]
        [Category("Export")]
        [DisplayName("inlineSpace")]
        [Description("Koliko da odmakne kada je inline opcija")]
        public string inlineSpace
        {
            get { return _inlineSpace; }
            set
            {
                _inlineSpace = value;
                OnPropertyChanged("inlineSpace");
            }
        }

        #endregion -----------  inlineSpace  -------  [Koliko da odmakne kada je inline opcija]

        #region -----------  doExportScripts  -------  [Da li text export sadrži SCRIPT tagove?]

        private bool _doExportScripts = false; // = new Boolean();

        /// <summary>
        /// Da li text export sadrži SCRIPT tagove?
        /// </summary>
        // [XmlIgnore]
        [Category("Export")]
        [DisplayName("doExportScripts")]
        [Description("Da li text export sadrži SCRIPT tagove?")]
        public bool doExportScripts
        {
            get { return _doExportScripts; }
            set
            {
                _doExportScripts = value;
                OnPropertyChanged("doExportScripts");
            }
        }

        #endregion -----------  doExportScripts  -------  [Da li text export sadrži SCRIPT tagove?]

        #region -----------  doExportStyles  -------  [Da li da izvozi definicije stilova]

        private bool _doExportStyles = false; // = new Boolean();

        /// <summary>
        /// Da li da izvozi definicije stilova
        /// </summary>
        // [XmlIgnore]
        [Category("textRetriveSetup")]
        [DisplayName("doExportStyles")]
        [Description("Da li da izvozi definicije stilova")]
        public bool doExportStyles
        {
            get { return _doExportStyles; }
            set
            {
                // Boolean chg = (_doExportStyles != value);
                _doExportStyles = value;
                OnPropertyChanged("doExportStyles");
                // if (chg) {}
            }
        }

        #endregion -----------  doExportStyles  -------  [Da li da izvozi definicije stilova]

        #region -----------  doExportTitle  -------  [Da li na pocetku da prikaze TITLE]

        private bool _doExportTitle = true; // = new Boolean();

        /// <summary>
        /// Da li na pocetku da prikaze TITLE
        /// </summary>
        // [XmlIgnore]
        [Category("Export")]
        [DisplayName("doExportTitle")]
        [Description("Da li na pocetku da prikaze TITLE")]
        public bool doExportTitle
        {
            get { return _doExportTitle; }
            set
            {
                _doExportTitle = value;
                OnPropertyChanged("doExportTitle");
            }
        }

        #endregion -----------  doExportTitle  -------  [Da li na pocetku da prikaze TITLE]

        #region -----------  doExportComments  -------  [Da li izvozi komentare]

        private bool _doExportComments = false; // = new Boolean();

        /// <summary>
        /// Da li izvozi komentare
        /// </summary>
        // [XmlIgnore]
        [Category("Export")]
        [DisplayName("doExportComments")]
        [Description("Da li izvozi komentare")]
        public bool doExportComments
        {
            get { return _doExportComments; }
            set
            {
                // Boolean chg = (_doExportComments != value);
                _doExportComments = value;
                OnPropertyChanged("doExportComments");
                // if (chg) {}
            }
        }

        #endregion -----------  doExportComments  -------  [Da li izvozi komentare]

        #region -----------  doCompressNewLines  -------  [Da li da kompresuje nove linije? svako pojavljivanje 3 nl za redom pretvara u 2nl]

        private bool _doCompressNewLines = true; // = new Boolean();

        /// <summary>
        /// Da li da kompresuje nove linije? svako pojavljivanje 3 nl za redom pretvara u 2nl
        /// </summary>
        // [XmlIgnore]
        [Category("textRetriveSetup")]
        [DisplayName("doCompressNewLines")]
        [Description("Da li da kompresuje nove linije? svako pojavljivanje 3 nl za redom pretvara u 2nl")]
        public bool doCompressNewLines
        {
            get { return _doCompressNewLines; }
            set
            {
                // Boolean chg = (_doCompressNewLines != value);
                _doCompressNewLines = value;
                OnPropertyChanged("doCompressNewLines");
                // if (chg) {}
            }
        }

        #endregion -----------  doCompressNewLines  -------  [Da li da kompresuje nove linije? svako pojavljivanje 3 nl za redom pretvara u 2nl]

        #region -----------  doRetrieveChildren  -------  [Da li vraca text i za child nodove]

        private bool _doRetrieveChildren = false; // = new Boolean();

        /// <summary>
        /// Da li vraca text i za child nodove
        /// </summary>
        // [XmlIgnore]
        [Category("textRetriveSetup")]
        [DisplayName("doRetrieveChildren")]
        [Description("Da li vraca text i za child nodove")]
        public bool doRetrieveChildren
        {
            get
            {
                return _doRetrieveChildren;
            }
            set
            {
                // Boolean chg = (_doRetrieveChildren != value);
                _doRetrieveChildren = value;
                OnPropertyChanged("doRetrieveChildren");
                // if (chg) {}
            }
        }

        #endregion -----------  doRetrieveChildren  -------  [Da li vraca text i za child nodove]

        #region -----------  doHtmlCleanUp  -------  [Da li da ocisti HTML nakon izvlacenja sadrzaja?]

        private bool _doHtmlCleanUp = true; // = new Boolean();

        /// <summary>
        /// Da li da ocisti HTML nakon izvlacenja sadrzaja?
        /// </summary>
        // [XmlIgnore]
        [Category("textRetriveSetup")]
        [DisplayName("doHtmlCleanUp")]
        [Description("Da li da ocisti HTML nakon izvlacenja sadrzaja?")]
        public bool doHtmlCleanUp
        {
            get
            {
                return _doHtmlCleanUp;
            }
            set
            {
                // Boolean chg = (_doHtmlCleanUp != value);
                _doHtmlCleanUp = value;
                OnPropertyChanged("doHtmlCleanUp");
                // if (chg) {}
            }
        }

        #endregion -----------  doHtmlCleanUp  -------  [Da li da ocisti HTML nakon izvlacenja sadrzaja?]

        #region -----------  doCyrToLatTransliteration  -------  [Da li da izvrsi transliteraciju iz Cyr u Lat]

        private bool _doCyrToLatTransliteration = false; // = new Boolean();

        /// <summary>
        /// Da li da izvrsi transliteraciju iz Cyr u Lat
        /// </summary>
        // [XmlIgnore]
        [Category("textRetriveSetup")]
        [DisplayName("doCyrToLatTransliteration")]
        [Description("Da li da izvrsi transliteraciju iz Cyr u Lat")]
        public bool doCyrToLatTransliteration
        {
            get
            {
                return _doCyrToLatTransliteration;
            }
            set
            {
                // Boolean chg = (_doCyrToLatTransliteration != value);
                _doCyrToLatTransliteration = value;
                OnPropertyChanged("doCyrToLatTransliteration");
                // if (chg) {}
            }
        }

        #endregion -----------  doCyrToLatTransliteration  -------  [Da li da izvrsi transliteraciju iz Cyr u Lat]
    }
}