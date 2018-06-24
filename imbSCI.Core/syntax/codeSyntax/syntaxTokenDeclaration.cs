// --------------------------------------------------------------------------------------------------------------------
// <copyright file="syntaxTokenDeclaration.cs" company="imbVeles" >
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
    /// Deklaracija jednog tokena
    /// </summary>
    public class syntaxTokenDeclaration : syntaxDeclarationItemBase, ISyntaxDeclarationElement
    {
        public syntaxTokenDeclaration()
        {
        }

        private syntaxTokenType _type = syntaxTokenType.unknown;

        /// <summary>
        /// Vrsta tokena
        /// </summary>
        public syntaxTokenType type
        {
            get
            {
                return _type;
            }
        }

        #region --- typeName ------- Property type name

        private String _typeName = "String";

        /// <summary>
        /// Property type name
        /// </summary>
        public String typeName
        {
            get
            {
                return _typeName;
            }
            set
            {
                _typeName = value;
                OnPropertyChanged("typeName");
            }
        }

        #endregion --- typeName ------- Property type name

        #region --- valueFormat ------- Formatiranje vrednosti

        private String _valueFormat = "\"{0}\"";

        /// <summary>
        /// Formatiranje vrednosti
        /// </summary>
        public String valueFormat
        {
            get
            {
                return _valueFormat;
            }
            set
            {
                _valueFormat = value;
                OnPropertyChanged("valueFormat");
            }
        }

        #endregion --- valueFormat ------- Formatiranje vrednosti
    }
}