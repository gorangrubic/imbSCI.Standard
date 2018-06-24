// --------------------------------------------------------------------------------------------------------------------
// <copyright file="reportOutputRepository.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.delivery
{
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.render.config;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="resourceDictionaryBase{T}.Object}" />
    public class reportOutputRepository : resourceDictionaryBase<reportOutputUnit>
    {
        public void setOutputUnit(builderSettings settings, object output, string logicalPath, reportElementLevel level)
        {
            var format = settings.forms[level].fileformat;
            string tmp_name = logicalPath.getPathVersion(-1, "\\", true);
            string tmp_path = Path.GetDirectoryName(logicalPath);
            string filename = settings.formats.getFilename(tmp_name, format);
            form = settings.forms[level].form;

            reportOutputUnit tmp = null;
            if (form == reportOutputForm.folder)
            {
                tmp_path = imbSciStringExtensions.add(tmp_path, tmp_name, "\\");
                tmp = new reportOutputUnit(tmp_path, tmp_name, format, form, output, level);
            }
            else if (form == reportOutputForm.file)
            {
                tmp_path = imbSciStringExtensions.add(tmp_path, filename, "\\");
                tmp = new reportOutputUnit(tmp_path, tmp_name, format, form, output, level);
            }
            else
            {
                //  tmp = new reportOutputUnit(tmp_path, tmp_name, format, form, output, level);
            }

            Add(logicalPath, tmp);
        }

        public void saveAll(IDocumentRender render)
        {
            foreach (KeyValuePair<string, reportOutputUnit> pair in this)
            {
                var format = pair.Value.fileformat;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string name { get; set; }

        public string setOutputRootPath(DirectoryInfo __root)
        {
            basePath = __root.FullName;
            return rebuild(name);
        }

        public string rebuild(string __name)
        {
            directoryPath = basePath;
            directoryPath = imbSciStringExtensions.add(directoryPath, name, "\\");
            return directoryPath;
        }

        protected override reportOutputUnit getDefault()
        {
            return new reportOutputUnit();
        }

        public reportOutputRepository(DirectoryInfo __root, string __name)
        {
            name = __name;

            setOutputRootPath(__root);
        }

        /// <summary>
        ///
        /// </summary>
        public reportOutputForm form { get; set; } = reportOutputForm.unknown;

        /// <summary>
        ///
        /// </summary>
        public string basePath { get; set; }

        #region --- directoryPath ------- path - directory part

        private string _directoryPath = "";

        /// <summary>
        /// path - directory part
        /// </summary>
        public string directoryPath
        {
            get
            {
                return _directoryPath;
            }
            set
            {
                _directoryPath = value;
                OnPropertyChanged("directoryPath");
            }
        }

        #endregion --- directoryPath ------- path - directory part
    }
}