// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGetToSetFromString.cs" company="imbVeles" >
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
// Project: imbSCI.Data
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Data.interfaces
{
    using imbSCI.Data.enums;
    using System;

    /// <summary>
    /// Data description classes having serialization capability over customizable declaration syntax
    /// </summary>
    public interface IGetToSetFromString
    {
        /// <summary>
        /// Makes string declaration of the param;
        /// </summary>
        /// <param name="declaration">Declaration format</param>
        String getToString(typedParamDeclarationType declaration = typedParamDeclarationType.nameDoubleDotType);

        /// <summary>
        /// Declares name and value type (class) from string declaration, formated as defined by the declaration
        /// </summary>
        /// <param name="input">String declaration of the param. Example: "numericParam:Int32;textMessage:String"</param>
        /// <param name="declaration">What format is used for string representation</param>
        void setFromString(String input, typedParamDeclarationType declaration = typedParamDeclarationType.nameDoubleDotType);
    }
}