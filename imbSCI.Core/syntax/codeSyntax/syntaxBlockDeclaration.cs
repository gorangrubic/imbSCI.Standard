// --------------------------------------------------------------------------------------------------------------------
// <copyright file="syntaxBlockDeclaration.cs" company="imbVeles" >
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
    /// <summary>
    /// Deklaracija jednog bloka
    /// </summary>
    public class syntaxBlockDeclaration : syntaxDeclarationItemBase, ISyntaxDeclarationElement
    {
        public syntaxBlockDeclaration()
        {
        }

        #region --- role ------- uloga koju ima ovaj blok

        private syntaxBlockRole _role = syntaxBlockRole.permanent;

        /// <summary>
        /// uloga koju ima ovaj blok
        /// </summary>
        public syntaxBlockRole role
        {
            get
            {
                return _role;
            }
            set
            {
                _role = value;
                OnPropertyChanged("role");
            }
        }

        #endregion --- role ------- uloga koju ima ovaj blok

        #region --- lines ------- lines defined inside block

        private syntaxBlockLineDeclarationCollection _lines = new syntaxBlockLineDeclarationCollection();

        /// <summary>
        /// lines defined inside block
        /// </summary>
        public syntaxBlockLineDeclarationCollection lines
        {
            get
            {
                return _lines;
            }
            set
            {
                _lines = value;
                OnPropertyChanged("lines");
            }
        }

        #endregion --- lines ------- lines defined inside block

        #region --- render ------- Režim renderovanja

        private syntaxBlockRenderMode _render = syntaxBlockRenderMode.complete;

        /// <summary>
        /// Režim renderovanja
        /// </summary>
        public syntaxBlockRenderMode render
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

        #endregion --- render ------- Režim renderovanja
    }
}