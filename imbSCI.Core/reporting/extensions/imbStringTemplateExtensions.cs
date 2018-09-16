// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbStringTemplateExtensions.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.extensions
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.reporting.template;
    using System;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Extensions for easier application of <c>stringTemplate</c> mechanism
    /// </summary>
    public static class imbStringTemplateExtensions
    {
        /// <summary>
        /// Determines whether this String has template placeholders
        /// </summary>
        /// <param name="template">The template.</param>
        /// <returns>
        ///   <c>true</c> if the specified template is template; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean isTemplate(this String template)
        {
            //MatchCollection found = stringTemplateTools.regex_import.Matches(template);
            return stringTemplateTools.regex_import.IsMatch(template) || stringTemplateTools.regex_import_2nd.IsMatch(template);
        }

        /// <summary>
        /// Replaces all template placeholder fields if mached with <c>data</c>  PropertyCollection key values.
        /// </summary>
        /// <param name="template">Any string that has {{{}}} template tags within</param>
        /// <param name="dataset">Multiple PropertyCollection or IEnumerable collections with PropertyCollection instances</param>
        /// <param name="removeUnMatched">If TRUE it will remove all remaining/unmached tags in the last iteration / PropertyCollection supplied.</param>
        /// <returns>String with matched template tags replaced with content from <c>data</c> collection.</returns>
        /// <remarks>
        /// It uses internally <see cref="reportTemplatePlaceholderCollection"/> for template mechanism.
        /// You may provide any and multiple IEnumerable PropertyCollection collections as part of <c>dataset</c> params array.
        /// </remarks>
        /// <example>
        /// <code>
        ///     String templateString = "His is {{{title}}} {{{first_name}}} {{{last_name}}} working with {{{company_name}}}."
        ///     String output = templateString.applyToContent(true, contactBean, accountBean);
        /// </code>
        /// Where <c>contactBean</c> and <c>accountBean</c> contain data from a corporate CRM.
        /// </example>
        /// <example>
        /// <code>
        ///     String output = templateString.applyToContent(true, contactBean, allCrmBeans, accountBean);
        /// </code>
        /// Where <c>allCrmBeans</c> is IEnumerable with multiple PropertyCollection instances.
        /// </example>
        public static String applyToContent(this String template, Boolean removeUnMatched, params PropertyCollection[] dataset)
        {
            PropertyCollection[] data = dataset.getFlatArray<PropertyCollection>();
            String mContent = template;

            var dcl = data.Last();

            // plc.loadTemplateString(template);
            if (template.isTemplate())
            {
                reportTemplatePlaceholderCollection plc = new reportTemplatePlaceholderCollection(template);

                foreach (PropertyCollection dc in data)
                {
                    mContent = plc.applyToContent(dc, mContent, ((dcl == dc) && removeUnMatched));
                }
            }

            return mContent;
        }

        /// <summary>
        /// Creates placeholder collection
        /// </summary>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        public static reportTemplatePlaceholderCollection getPlaceHolders(this String template)
        {
            reportTemplatePlaceholderCollection plc = new reportTemplatePlaceholderCollection();
            plc.loadTemplateString(template);
            return plc;
        }

        /// <summary>
        /// Replaces all hard {{{data_fieldname}}} and soft {{data_fieldname}} fields if mached with <c>data</c>  PropertyCollection key values.
        /// </summary>
        /// <param name="template">Any string that has {{{}}} template tags within</param>
        /// <param name="data">Collection with <c>key</c> values matching template tags</param>
        /// <param name="removeUnMatched">If TRUE it will remove all remaining/unmached tags. Use this if collection is the last PropertyCollection to apply</param>
        /// <returns>String with matched template tags replaced with content from <c>data</c> collection</returns>
        public static String applyToContent(this String template, PropertyCollection data, Boolean removeUnMatched = false)
        {
            reportTemplatePlaceholderCollection plc = new reportTemplatePlaceholderCollection();
            plc.loadTemplateString(template);
            return plc.applyToContent(data, template, removeUnMatched);
        }
    }
}