// --------------------------------------------------------------------------------------------------------------------
// <copyright file="stringTemplate.cs" company="imbVeles" >
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
    using imbSCI.Data;

    #region imbVeles using

    using System;
    using System.Collections.Generic;
    using System.Data;

    #endregion imbVeles using

    /// <summary>
    /// Klasa koja opisuje jedan template
    /// </summary>
    /// \ingroup_disabled report_ll_templates
    public class stringTemplate : stringTemplateDeclaration
    {
        /// <summary>
        /// Applies to content.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="mContent">Content of the m.</param>
        /// <returns></returns>
        public String applyToContent(DataRow row, String mContent = null)
        {
            if (imbSciStringExtensions.isNullOrEmptyString(mContent)) mContent = template;

            return placeholders.applyToContent(row, mContent);
        }

        /// <summary>
        /// Applies data from a property collection to its content
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="mContent">Content of the m.</param>
        /// <returns></returns>
        public string applyToContent(PropertyCollection source, String mContent = null)
        {
            if (imbSciStringExtensions.isNullOrEmptyString(mContent)) mContent = template;

            return placeholders.applyToContent(source, mContent);
        }

        public string applyToContent(Dictionary<String,String> source, String mContent = null)
        {
            if (imbSciStringExtensions.isNullOrEmptyString(mContent)) mContent = template;

            return placeholders.applyToContent(source, mContent);
        }

        /// <summary>
        /// Applies to content.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="mContent">Content of the m.</param>
        /// <returns></returns>
        public string applyToContent(DataTable dt, String mContent = null)
        {
            if (imbSciStringExtensions.isNullOrEmptyString(mContent)) mContent = template;
            return placeholders.applyToContent(dt, template);
        }

        /// <summary>
        /// Removes from content.
        /// </summary>
        /// <param name="mContent">Content of the m.</param>
        /// <returns></returns>
        public string removeFromContent(String mContent = null)
        {
            if (imbSciStringExtensions.isNullOrEmptyString(mContent)) mContent = template;
            return placeholders.removeFromContent(template);
        }

        /// <summary>
        /// Loads the template string.
        /// </summary>
        /// <param name="formatString">The format string.</param>
        /// <returns></returns>
        public int loadTemplateString(string formatString)
        {
            template = formatString;
            return placeholders.loadTemplateString(formatString);
        }

        /// <summary>
        /// Constructor that runs the template code evaluation instantly
        /// </summary>
        /// <param name="templateCode"></param>
        public stringTemplate(string templateCode)
        {
            template = templateCode;
            placeholders.loadTemplateString(template);
        }

        #region --- placeholders ------- kolekcija parametara / plejs holdera

        private reportTemplatePlaceholderCollection _placeholders = new reportTemplatePlaceholderCollection();

        /// <summary>
        /// kolekcija parametara / plejs holdera
        /// </summary>
        public reportTemplatePlaceholderCollection placeholders
        {
            get
            {
                return _placeholders;
            }
            set
            {
                _placeholders = value;
                OnPropertyChanged("placeholders");
            }
        }

        #endregion --- placeholders ------- kolekcija parametara / plejs holdera
    }
}