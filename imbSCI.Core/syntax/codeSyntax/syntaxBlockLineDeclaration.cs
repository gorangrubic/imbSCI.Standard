// --------------------------------------------------------------------------------------------------------------------
// <copyright file="syntaxBlockLineDeclaration.cs" company="imbVeles" >
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

    /// <summary>
    /// Declaration of one parameter, instruction, comment, empty line
    /// </summary>
    public class syntaxBlockLineDeclaration : syntaxDeclarationItemBase, ISyntaxDeclarationElement
    {
        internal syntaxBlockLineDeclaration()
        {
        }

        // public class  : syntaxDeclarationItemBase, ISyntaxDeclarationElement

        #region --- className ------- name of lineclass declaration

        private String _className = "";

        /// <summary>
        /// name of lineclass declaration
        /// </summary>
        public String className
        {
            get
            {
                return _className;
            }
            set
            {
                _className = value;
                OnPropertyChanged("className");
            }
        }

        #endregion --- className ------- name of lineclass declaration

        private syntaxTokenDeclarationCollection _tokens = new syntaxTokenDeclarationCollection();

        /// <summary>
        /// deklaracija tokena (parametara)
        /// </summary>
        public syntaxTokenDeclarationCollection tokens
        {
            get { return _tokens; }
        }

        #region --- render ------- Type of render template

        private syntaxBlockLineType _render = syntaxBlockLineType.normal;

        /// <summary>
        /// Type of render template
        /// </summary>
        public syntaxBlockLineType render
        {
            get
            {
                return _render;
            }
            set
            {
                _render = value;
                OnPropertyChanged("render");
            }
        }

        #endregion --- render ------- Type of render template
    }
}