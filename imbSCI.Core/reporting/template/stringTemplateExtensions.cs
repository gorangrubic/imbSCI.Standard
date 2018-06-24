// --------------------------------------------------------------------------------------------------------------------
// <copyright file="stringTemplateExtensions.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.template
{
    using imbSCI.Core.extensions.data;

    #region imbVeles using

    using System;
    using System.Collections.Generic;

    #endregion imbVeles using

    /// <summary>
    /// Operacije sa stringTemplateExtensions
    /// </summary>
    public static class stringTemplateExtensions
    {
        public static String WILLCARD_NEWLINE = "~nl";

        #region --- willcards ------- collection of template willcards

        private static Dictionary<String, String> _willcards;

        /// <summary>
        /// collection of template willcards
        /// </summary>
        public static Dictionary<String, String> willcards
        {
            get
            {
                if (_willcards == null)
                {
                    _willcards = new Dictionary<String, String>();
                    _willcards.Add("~nl", "\r\n");
                }
                return _willcards;
            }
        }

        #endregion --- willcards ------- collection of template willcards

        /// <summary>
        /// Deploys all template willcards
        /// </summary>
        /// <param name="templateString"></param>
        /// <returns></returns>
        public static String deployTemplateWillcards(this String templateString)
        {
            String output = templateString;

            output = output.Replace(willcards);

            return output;
        }
    }
}