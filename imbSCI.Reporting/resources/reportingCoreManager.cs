// --------------------------------------------------------------------------------------------------------------------
// <copyright file="reportingCoreManager.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.resources
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Data;
    using imbSCI.Reporting.meta.theme;
    using System;

    /// <summary>
    /// Manager of reporting resources
    /// </summary>
    public static class reportingCoreManager
    {
        /// <summary>
        /// Should it produce verbose log output on Meta report model notifications
        /// </summary>
        public static bool doVerboseLog { get; } = false;

        /// <summary>
        /// Loads <c>BuildAction</c> Content files from two reserved folders.
        /// </summary>
        /// <remarks>
        /// Targeted file must be set: BuildAction = Content, Copy Local = Always
        /// </remarks>
        /// <param name="rdir">Folder with the file</param>
        /// <param name="foldername">If there is subfolder - name it</param>
        /// <param name="filename">Full filename, including extension</param>
        /// <returns></returns>
        public static string loadReportResourceFile(this reportResourceFolders rdir, string filename, string foldername = "")
        {
            string pathFormat = "/{0}/{2}/{1}";
            if (foldername.isNullOrEmpty())
            {
                pathFormat = "{0}{2}/{1}";
            }
            string path = string.Format(pathFormat, rdir.ToString(), filename, foldername);

            Uri uri = new Uri(path, UriKind.Relative);
            //StreamResourceInfo sri = Application.GetResourceStream(uri);
            //StreamReader sReader = new StreamReader(sri.Stream);

            string output = "";
            //  output = sReader.ReadToEnd();

            throw new NotImplementedException();

            return output;
        }

        /// <summary>
        /// Prepares this instance.
        /// </summary>
        public static void prepare()
        {
            metaDocumentThemeManager.prepare();
        }
    }
}