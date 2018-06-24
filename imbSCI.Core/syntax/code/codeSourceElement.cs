// --------------------------------------------------------------------------------------------------------------------
// <copyright file="codeSourceElement.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.code
{
    using imbSCI.Core.syntax.codeSyntax;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Gradivni element koda
    /// </summary>
    public class codeSourceElement
    {
        private Dictionary<Int32, codeSourceElement> _subElements = new Dictionary<int, codeSourceElement>(); // = "";

        /// <summary>
        /// Dictionary with sub elements - contained in this source
        /// </summary>
        public Dictionary<Int32, codeSourceElement> subElements
        {
            get { return _subElements; }
        }

        internal List<String> _tokens = new List<string>(); // = "";

        /// <summary>
        /// Lista tokena
        /// </summary>
        public List<String> tokens
        {
            get { return _tokens; }
        }

        internal codeSourceElementType _type = codeSourceElementType.unknown;// = "";

        /// <summary>
        /// type of code source element
        /// </summary>
        public codeSourceElementType type
        {
            get { return _type; }
        }

        internal String _name; // = "";

        /// <summary>
        /// Bindable property
        /// </summary>
        public String name
        {
            get { return _name; }
        }

        internal ISyntaxDeclarationElement _declaration; // = "";

        /// <summary>
        /// Syntax declaration for this element
        /// </summary>
        public ISyntaxDeclarationElement declaration
        {
            get { return _declaration; }
        }

        internal syntaxLineClassDeclaration _lineClass; // = "";

        /// <summary>
        /// Detected line class
        /// </summary>
        public syntaxLineClassDeclaration lineClass
        {
            get { return _lineClass; }
        }

        internal String _source; // = "";

        /// <summary>
        /// Content of the source element
        /// </summary>
        public String source
        {
            get { return _source; }
        }
    }
}