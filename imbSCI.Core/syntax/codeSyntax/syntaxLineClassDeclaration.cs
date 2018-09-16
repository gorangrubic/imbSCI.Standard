// --------------------------------------------------------------------------------------------------------------------
// <copyright file="syntaxLineClassDeclaration.cs" company="imbVeles" >
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
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Declaration of a line class.
    /// </summary>
    public class syntaxLineClassDeclaration : syntaxDeclarationItemBase, ISyntaxDeclarationElement
    {
        #region --- nameToken ------- Index of token with name

        private Int32 _nameToken = 0;

        /// <summary>
        /// Index of token with name
        /// </summary>
        public Int32 nameToken
        {
            get
            {
                return _nameToken;
            }
            set
            {
                _nameToken = value;
                OnPropertyChanged("nameToken");
            }
        }

        #endregion --- nameToken ------- Index of token with name

        private Regex regQuery = null;

        /// <summary>
        /// Gets REGEX for this line class
        /// </summary>
        /// <returns></returns>
        public Regex getRegex()
        {
            if (regQuery == null)
            {
                regQuery = new Regex(tokenQuery);
            }
            return regQuery;
        }

        #region --- lineType ------- type of line

        private syntaxBlockLineType _lineType = syntaxBlockLineType.normal;

        /// <summary>
        /// type of line
        /// </summary>
        public syntaxBlockLineType lineType
        {
            get
            {
                return _lineType;
            }
            set
            {
                _lineType = value;
                OnPropertyChanged("lineType");
            }
        }

        #endregion --- lineType ------- type of line

        public syntaxLineClassDeclaration()
        {
        }

        public syntaxLineClassDeclaration(String __className, String __template, String __tokenQuery)
        {
            name = __className;
            template = __template;
            tokenQuery = __tokenQuery;
        }

        #region --- template ------- template for output genetarion

        private String _template;

        /// <summary>
        /// template for output genetarion. Supports> ~nl for system specific new line sub string
        /// </summary>
        public String template
        {
            get
            {
                return _template;
            }
            set
            {
                _template = value;
                OnPropertyChanged("template");
            }
        }

        #endregion --- template ------- template for output genetarion

        #region --- tokenQuery ------- REGEX query that selects tokens

        private String _tokenQuery = "";

        /// <summary>
        /// REGEX query that selects tokens
        /// </summary>
        public String tokenQuery
        {
            get
            {
                return _tokenQuery;
            }
            set
            {
                _tokenQuery = value;
                OnPropertyChanged("tokenQuery");
            }
        }

        #endregion --- tokenQuery ------- REGEX query that selects tokens
    }
}