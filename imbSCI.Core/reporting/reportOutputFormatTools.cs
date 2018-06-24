// --------------------------------------------------------------------------------------------------------------------
// <copyright file="reportOutputFormatTools.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting
{
    #region imbVeles using

    using imbSCI.Core.reporting.format;
    using imbSCI.Data.collection.nested;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.reporting;
    using System;
    using System.IO;

    #endregion imbVeles using

    public static class reportOutputFormatTools
    {
        public static reportIncludeFileType getIncludeFileTypeByExtension(this String filename)
        {
            String ext = Path.GetExtension(filename);
            ext = ext.ToLower();
            ext = ext.Trim('.');
            switch (ext)
            {
                case "css":
                    return reportIncludeFileType.cssStyle;
                    break;

                default:
                    return reportIncludeFileType.unknown;
                    break;
            }
        }

        public static externalTool getDefaultApplication(this reportOutputFormatName outputFormat)
        {
            switch (outputFormat)
            {
                case reportOutputFormatName.htmlReport:
                    return externalTool.firefox;
                    break;

                default:
                    return externalTool.notepadpp;
                    break;
            }
        }

        /// <summary>
        /// proverava da li je prosledjen tag defaultTag -- tj. ako jeste vraca podrazumevani tag prema datoj ulozi
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static htmlTagName checkForDefaultTag(this htmlTagName tag,
                                                     reportOutputRoles role = reportOutputRoles.appendLine)
        {
            if (tag == htmlTagName.defaultTag)
            {
                Object tg = defaultTags[reportOutputFormatName.htmlReport][role];
                if (tg is htmlTagName)
                {
                    tag = (htmlTagName)tg;
                }
            }
            return tag;
        }

        #region --- defaultTags ------- static and autoinitiated object

        private static imbCollectionNestedEnum<reportOutputFormatName, reportOutputRoles, Object> _defaultTags;

        /// <summary>
        /// Kolekcija podrazumevanih tagova za dat oblik izvestavanja
        /// </summary>
        private static imbCollectionNestedEnum<reportOutputFormatName, reportOutputRoles, Object> defaultTags
        {
            get
            {
                if (_defaultTags == null)
                {
                    _defaultTags = new imbCollectionNestedEnum<reportOutputFormatName, reportOutputRoles, Object>();

                    _defaultTags[reportOutputFormatName.htmlReport][reportOutputRoles.appendLine] = htmlTagName.p;
                    _defaultTags[reportOutputFormatName.htmlReport][reportOutputRoles.appendLink] = htmlTagName.a;
                    _defaultTags[reportOutputFormatName.htmlReport][reportOutputRoles.appendInline] = htmlTagName.p;
                    _defaultTags[reportOutputFormatName.htmlReport][reportOutputRoles.appendPair] = htmlTagName.span;
                    _defaultTags[reportOutputFormatName.htmlReport][reportOutputRoles.appendPairContainer] = htmlTagName.p;
                    _defaultTags[reportOutputFormatName.htmlReport][reportOutputRoles.appendPairItem] = htmlTagName.span;
                    _defaultTags[reportOutputFormatName.htmlReport][reportOutputRoles.appendPairValue] = htmlTagName.span;
                    _defaultTags[reportOutputFormatName.htmlReport][reportOutputRoles.container] = htmlTagName.div;
                }
                return _defaultTags;
            }
        }

        #endregion --- defaultTags ------- static and autoinitiated object

        /*

        #region --- defaultIncludeExtensions ------- lista ekstenzija prema rip

        private static Dictionary<reportIncludeFileType, String> _defaultIncludeExtensions =
            new Dictionary<reportIncludeFileType, string>();
        /// <summary>
        /// lista ekstenzija prema rip
        /// </summary>
        public static Dictionary<reportIncludeFileType, String> defaultIncludeExtensions
        {
            get
            {
                if (_defaultIncludeExtensions == null)
                {
                    _defaultIncludeExtensions = new Dictionary<reportIncludeFileType, String>();
                    _defaultIncludeExtensions.Add(reportIncludeFileType.cssStyle, ".css"
                }
                return _defaultIncludeExtensions;
            }
        }

        #endregion --- defaultIncludeExtensions ------- lista ekstenzija prema rip

        */
    }
}