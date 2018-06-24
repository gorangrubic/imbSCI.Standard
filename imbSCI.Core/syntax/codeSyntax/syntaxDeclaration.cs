// --------------------------------------------------------------------------------------------------------------------
// <copyright file="syntaxDeclaration.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.codeSyntax
{
    using imbSCI.Core.reporting.template;
    using System;
    using System.ComponentModel;
    using System.Text;
    using System.Xml.Serialization;

    /// <summary>
    /// Deserijalizovan opis sintakse: header, templates,
    /// </summary>
    /// <remarks>
    /// Tokom primene softvera (production) koristi se samo za čitanje već definisanih deklaracija.
    /// Tokom razvoja softvera (development) koristi se za in-code pravljenje prototipova
    /// </remarks>
    [XmlInclude(typeof(Encoding))]
    public class syntaxDeclaration : syntaxDeclarationItemBase, ISyntaxDeclarationElement
    {
        public syntaxDeclaration()
        {
        }

        #region --- header ------- syntax meta data header

        private syntaxHeaderDeclaration _header = new syntaxHeaderDeclaration();

        /// <summary>
        /// syntax meta data header
        /// </summary>
        public syntaxHeaderDeclaration header
        {
            get
            {
                return _header;
            }
            set
            {
                _header = value;
                OnPropertyChanged("header");
            }
        }

        #endregion --- header ------- syntax meta data header

        #region --- types ------- List of special types

        private syntaxTypeNameCollection _types = new syntaxTypeNameCollection();

        /// <summary>
        /// List of special types
        /// </summary>
        public syntaxTypeNameCollection types
        {
            get
            {
                return _types;
            }
            set
            {
                _types = value;
                OnPropertyChanged("types");
            }
        }

        #endregion --- types ------- List of special types

        #region --- structure ------- rendering structure

        private syntaxRenderStructure _structure = new syntaxRenderStructure();

        /// <summary>
        /// rendering structure
        /// </summary>
        public syntaxRenderStructure structure
        {
            get
            {
                return _structure;
            }
            set
            {
                _structure = value;
                OnPropertyChanged("structure");
            }
        }

        #endregion --- structure ------- rendering structure

        //#region --- regexBlocks ------- Regex used to select block name and content
        //private String _regexBlocks = "";
        ///// <summary>
        ///// Regex used to select block name and content
        ///// </summary>
        //public String regexBlocks
        //{
        //    get
        //    {
        //        return _regexBlocks;
        //    }
        //    set
        //    {
        //        _regexBlocks = value;
        //        OnPropertyChanged("regexBlocks");
        //    }
        //}
        //#endregion

        //private Regex __regexBlocks;

        //public Regex getBlockRegex()
        //{
        //    if (__regexBlocks == null) {
        //        __regexBlocks = new Regex(regexBlocks);
        //    }
        //    return __regexBlocks;
        //}

        //#region --- regexProperty ------- Regex used to select property name and value
        //private String _regexProperty = "";
        ///// <summary>
        ///// Regex used to select property name and value
        ///// </summary>
        //public String regexProperty
        //{
        //    get
        //    {
        //        return _regexProperty;
        //    }
        //    set
        //    {
        //        _regexProperty = value;
        //        OnPropertyChanged("regexProperty");
        //    }
        //}
        //#endregion

        //#region --- regexLine ------- Regex used to select line content: name and properties
        //private String _regexLine = "";
        ///// <summary>
        ///// Regex used to select line content: name and properties
        ///// </summary>
        //public String regexLine
        //{
        //    get
        //    {
        //        return _regexLine;
        //    }
        //    set
        //    {
        //        _regexLine = value;
        //        OnPropertyChanged("regexLine");
        //    }
        //}
        //#endregion

        //#region --- regexComment ------- Regex used to test and extract comment line
        //private String _regexComment = "";
        ///// <summary>
        ///// Regex used to test and extract comment line
        ///// </summary>
        //public String regexComment
        //{
        //    get
        //    {
        //        return _regexComment;
        //    }
        //    set
        //    {
        //        _regexComment = value;
        //        OnPropertyChanged("regexComment");
        //    }
        //}
        //#endregion

        #region ----------- Boolean [ doSkipUndeclaredBlocks ] -------  [if TRUE: during source code processing it will ignore undeclared blocks]

        private Boolean _doSkipUndeclaredBlocks = true;

        /// <summary>
        /// if TRUE: during source code processing it will ignore undeclared blocks
        /// </summary>
        [Category("Switches")]
        [DisplayName("doSkipUndeclaredBlocks")]
        [Description("if TRUE: during source code processing it will ignore undeclared blocks")]
        public Boolean doSkipUndeclaredBlocks
        {
            get { return _doSkipUndeclaredBlocks; }
            set { _doSkipUndeclaredBlocks = value; OnPropertyChanged("doSkipUndeclaredBlocks"); }
        }

        #endregion ----------- Boolean [ doSkipUndeclaredBlocks ] -------  [if TRUE: during source code processing it will ignore undeclared blocks]

        #region ----------- Boolean [ doSkipUndeclaredLines ] -------  [if TRUE: it will ignore undeclared property lines / instructions]

        private Boolean _doSkipUndeclaredLines = true;

        /// <summary>
        /// if TRUE: it will ignore undeclared property lines / instructions
        /// </summary>
        [Category("Switches")]
        [DisplayName("doSkipUndeclaredLines")]
        [Description("if TRUE: it will ignore undeclared property lines / instructions")]
        public Boolean doSkipUndeclaredLines
        {
            get { return _doSkipUndeclaredLines; }
            set { _doSkipUndeclaredLines = value; OnPropertyChanged("doSkipUndeclaredLines"); }
        }

        #endregion ----------- Boolean [ doSkipUndeclaredLines ] -------  [if TRUE: it will ignore undeclared property lines / instructions]

        #region --- fileTemplate ------- struktura kompletnog fajla

        private stringTemplateDeclaration _fileTemplate = new stringTemplateDeclaration();

        /// <summary>
        /// struktura kompletnog fajla
        /// </summary>
        public stringTemplateDeclaration fileTemplate
        {
            get
            {
                return _fileTemplate;
            }
            set
            {
                _fileTemplate = value;
                OnPropertyChanged("fileTemplate");
            }
        }

        #endregion --- fileTemplate ------- struktura kompletnog fajla

        #region --- blockTemplate ------- struktura jednog bloka

        private stringTemplateDeclaration _blockTemplate = new stringTemplateDeclaration();

        /// <summary>
        /// struktura jednog bloka
        /// </summary>
        public stringTemplateDeclaration blockTemplate
        {
            get
            {
                return _blockTemplate;
            }
            set
            {
                _blockTemplate = value;
                OnPropertyChanged("blockTemplate");
            }
        }

        #endregion --- blockTemplate ------- struktura jednog bloka

        #region --- lineDefaultTemplate ------- podrazumevan template za liniju

        private stringTemplateDeclaration _lineDefaultTemplate = new stringTemplateDeclaration();

        /// <summary>
        /// podrazumevan template za liniju
        /// </summary>
        public stringTemplateDeclaration lineDefaultTemplate
        {
            get
            {
                return _lineDefaultTemplate;
            }
            set
            {
                _lineDefaultTemplate = value;
                OnPropertyChanged("lineDefaultTemplate");
            }
        }

        #endregion --- lineDefaultTemplate ------- podrazumevan template za liniju

        #region --- commentDefaultTemplate ------- podrazumevan template za komentar

        private stringTemplateDeclaration _commentDefaultTemplate = new stringTemplateDeclaration();

        /// <summary>
        /// podrazumevan template za komentar
        /// </summary>
        public stringTemplateDeclaration commentDefaultTemplate
        {
            get
            {
                return _commentDefaultTemplate;
            }
            set
            {
                _commentDefaultTemplate = value;
                OnPropertyChanged("commentDefaultTemplate");
            }
        }

        #endregion --- commentDefaultTemplate ------- podrazumevan template za komentar

        #region --- blocks ------- syntax blocks declarations

        private syntaxBlockDeclarationCollection _blocks = new syntaxBlockDeclarationCollection();

        /// <summary>
        /// syntax blocks declarations
        /// </summary>
        public syntaxBlockDeclarationCollection blocks
        {
            get
            {
                return _blocks;
            }
            set
            {
                _blocks = value;
                OnPropertyChanged("blocks");
            }
        }

        #endregion --- blocks ------- syntax blocks declarations

        #region --- blockClass ------- class for block selection

        private syntaxLineClassDeclaration _blockClass;

        /// <summary>
        /// class for block selection
        /// </summary>
        public syntaxLineClassDeclaration blockClass
        {
            get
            {
                return _blockClass;
            }
            set
            {
                _blockClass = value;
                OnPropertyChanged("blockClass");
            }
        }

        #endregion --- blockClass ------- class for block selection

        #region --- lineClasses ------- Line classes declaration

        private syntaxLineClassDeclarationCollection _lineClasses = new syntaxLineClassDeclarationCollection();

        /// <summary>
        /// Line classes declaration
        /// </summary>
        public syntaxLineClassDeclarationCollection lineClasses
        {
            get
            {
                return _lineClasses;
            }
            set
            {
                _lineClasses = value;
                OnPropertyChanged("lineClasses");
            }
        }

        #endregion --- lineClasses ------- Line classes declaration
    }
}