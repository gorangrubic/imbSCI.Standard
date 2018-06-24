// --------------------------------------------------------------------------------------------------------------------
// <copyright file="reportIncludeFileCollection.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.includes
{
    #region imbVeles using

    using imbSCI.Core.extensions.io;
    using imbSCI.Core.reporting;
    using imbSCI.Data;
    using imbSCI.Data.collection;
    using imbSCI.Data.enums.reporting;
    using System.IO;
    using System.Text;

    #endregion imbVeles using

    /// <summary>
    /// imbCollectionMeta namenska kolekcija za  reportIncludeFile
    /// </summary>
    public class reportIncludeFileCollection : aceDictionaryCollection<reportIncludeFile>
    {
        public reportIncludeFileCollection() : base()
        {
        }

        /// <summary>
        /// Dodaje novi fajl
        /// </summary>
        /// <param name="__path"></param>
        /// <param name="__type"></param>
        public void AddFile(string __path, reportIncludeFileType __type = reportIncludeFileType.unknown)
        {
            if (__type == reportIncludeFileType.unknown)
            {
                __type = __path.getIncludeFileTypeByExtension();
            }

            ///            Directory.GetCurrentDirectory()

            string dirName = Path.GetDirectoryName(__path);
            //if (imbSciStringExtensions.isNullOrEmpty(dirName))
            //{
            //    __path = outputFolder.resources.ToString();
            //    DirectoryInfo di = new DirectoryInfo(__path);
            //    __path = di.FullName;
            //}

            //if (!Path.IsPathRooted(__path))
            //{
            //  //  __path = Path.Combine(imbCoreManager.runtimePath, __path);
            //}
            bool __lc = false;

            switch (__type)
            {
                case reportIncludeFileType.cssStyle:
                    __lc = true;
                    break;
            }

            reportIncludeFile rf = new reportIncludeFile(__path, __type, __lc);
            Add(rf);
        }

        /// <summary>
        /// Kopira sve fajlove iz kolekcije na datu putanju
        /// </summary>
        /// <param name="outputPath"></param>
        public void prepareIncludes(string outputPath)
        {
            string dirPath = Path.GetDirectoryName(outputPath);
            dirPath = dirPath.ensureEndsWith("\\");
            //DirectoryInfo directory = new DirectoryInfo(outputPath);

            foreach (reportIncludeFile inc in Values)
            {
                if (inc.doLocalCopy)
                {
                    fileOpsBase.copyFile(inc.sourceFilePath, dirPath, inc.filename);
                }

                switch (inc.filetype)
                {
                    case reportIncludeFileType.cssStyle:
                        //fileOpsBase.copyFile(inc.sourceFilePath, dirPath, inc.filename);
                        break;

                    case reportIncludeFileType.emailAttachmentStatic:
                        fileOpsBase.copyFile(inc.sourceFilePath, dirPath, inc.filename);
                        break;
                }

                //logSystem.log(
                //    "Report included file: copy[" + inc.doLocalCopy + "] source[" + inc.sourceFilePath + "] ",
                //    logType.Notification);
            }
        }

        public void addHTMLMetaTags(StringBuilder isb)
        {
            foreach (reportIncludeFile inc in Values)
            {
                switch (inc.filetype)
                {
                    case reportIncludeFileType.cssStyle:
                        string _ins = "<link href=\"" + inc.localOutputPath +
                                      "\" rel=\"stylesheet\" type=\"text/css\"/>";

                        isb.AppendLine(_ins);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}