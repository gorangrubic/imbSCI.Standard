// --------------------------------------------------------------------------------------------------------------------
// <copyright file="exeAppendContentExtensions.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.script.exeAppenders
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Data;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// ToolKit that supports <c>exeAppend</c> library
    /// </summary>
    public static class exeAppendContentExtensions
    {
        /// <summary>
        /// Gets the search pattern.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        public static List<string> getExtensionList(this exeAppendTemplateOptions flags)
        {
            List<string> output = new List<string>();
            List<exeAppendTemplateOptions> flgs = flags.getEnumListFromFlags<exeAppendTemplateOptions>();
            foreach (exeAppendTemplateOptions tl in flgs)
            {
                if (tl.ToString().EndsWith("Template"))
                {
                    string include = tl.ToString().removeEndsWith("Template");
                    output.Add(include);
                }
            }

            return output;
        }

        /// <summary>
        /// Gets the search pattern.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        public static string getSearchPattern(this exeAppendTemplateOptions flags)
        {
            string output = "";
            List<exeAppendTemplateOptions> flgs = flags.getEnumListFromFlags<exeAppendTemplateOptions>();
            foreach (exeAppendTemplateOptions tl in flgs)
            {
                if (tl.ToString().EndsWith("Template"))
                {
                    string include = tl.ToString().removeEndsWith("Template").add("*", ".");
                    output = output.add(include, "|");
                }
            }

            return output;
        }

        /// <summary>
        /// Directories the copy.
        /// </summary>
        /// <param name="sourceDirName">Name of the source dir.</param>
        /// <param name="destDirName">Name of the dest dir.</param>
        /// <param name="copySubDirs">if set to <c>true</c> [copy sub dirs].</param>
        /// <exception cref="DirectoryNotFoundException">Source directory does not exist or could not be found: "
        ///                     + sourceDirName</exception>
        public static void DirectoryCopy(this string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    subdir.FullName.DirectoryCopy(temppath, copySubDirs);
                }
            }
        }
    }
}