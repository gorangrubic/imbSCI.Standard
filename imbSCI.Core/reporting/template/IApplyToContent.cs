// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplyToContent.cs" company="imbVeles" >
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
    #region imbVeles using

    using System;
    using System.Data;
    using System.Reflection;

    #endregion imbVeles using

    internal interface IApplyToContent
    {
        /// <summary>
        /// Rucno dodaje novi placeholder ili vraca postojeci ako vec postoji pod tim imenom
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="_pi">Postavlja pi</param>
        /// <returns>vraca novi ili postojeci placeholder</returns>
        reportTemplatePlaceholder addPlaceholder(String fieldName = "", PropertyInfo _pi = null);

        /// <summary>
        /// Applies the property collection
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mContent"></param>
        /// <param name="fieldPrefix"></param>
        /// <returns></returns>
        String applyToContent(PropertyCollection source, String mContent, Boolean autoRemove = false);

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mContent"></param>
        /// <param name="fieldPrefix"></param>
        /// <param name="autoRemove"></param>
        /// <returns></returns>
        String applyToContent(Object source, String mContent, String fieldPrefix = "main_", Boolean autoRemove = false);

        /// <summary>
        /// Applies values from DataTable -- using shema and all rows.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="mContent"></param>
        /// <param name="fieldPrefix">Prefix applied on placeholder key</param>
        /// <returns></returns>
        String applyToContent(DataTable dt, String mContent, Boolean autoRemove = false);

        /// <summary>
        /// removes all placeholder tags from the content
        /// </summary>
        /// <param name="mContent"></param>
        /// <returns></returns>
        String removeFromContent(String mContent);

        /// <summary>
        /// Ucitava string u kome se nalazi template -- dodaje pronadjene placeholdere u kolekciju
        /// </summary>
        /// <param name="formatString">String koji se obradjuje</param>
        /// <param name="makeReport">Da li da pravi izvestaj o importu</param>
        /// <returns>Broj novo dodatih placeholdera</returns>
        Int32 loadTemplateString(String formatString);
    }
}