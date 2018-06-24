// --------------------------------------------------------------------------------------------------------------------
// <copyright file="reportIncludeFile.cs" company="imbVeles" >
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

    using imbSCI.Core.attributes;
    using imbSCI.Core.reporting;
    using imbSCI.Data.enums.reporting;
    using System.ComponentModel;
    using System.IO;

    #endregion imbVeles using

    /// <summary>
    /// Tip include fajla
    /// </summary>
    [imb(imbAttributeName.collectionPrimaryKey, "name")]
    public class reportIncludeFile : imbReportingBindable
    {
        public reportIncludeFile()
        {
        }

        public reportIncludeFile(string _sourcePath, reportIncludeFileType __fileType = reportIncludeFileType.cssStyle,
                                 bool __doLocalCopy = true)
        {
            sourceFilePath = _sourcePath;
            name = Path.GetFileNameWithoutExtension(_sourcePath);
            extension = Path.GetExtension(_sourcePath);
            filetype = __fileType;
            doLocalCopy = __doLocalCopy;
            filename = name + extension;

            localOutputPath = filename;
        }

        #region --- extension ------- ekstenzija

        private string _extension;

        /// <summary>
        /// ekstenzija
        /// </summary>
        public string extension
        {
            get { return _extension; }
            set
            {
                _extension = value;
                OnPropertyChanged("extension");
            }
        }

        #endregion --- extension ------- ekstenzija

        #region -----------  name  -------  [file name izlaznog fajla - ekstenzija ostaje ista kao ulazna]

        private string _name; // = new String();

        /// <summary>
        /// file name izlaznog fajla - ekstenzija ostaje ista kao ulazna
        /// </summary>
        // [XmlIgnore]
        [Category("reportIncludeFile")]
        [DisplayName("name")]
        [Description("file name izlaznog fajla (bez ekstenzije) - ekstenzija ostaje ista kao ulazna")]
        public string name
        {
            get { return _name; }
            set
            {
                // Boolean chg = (_name != value);
                _name = value;
                OnPropertyChanged("name");
                // if (chg) {}
            }
        }

        #endregion -----------  name  -------  [file name izlaznog fajla - ekstenzija ostaje ista kao ulazna]

        #region --- filename ------- ime fajla zajedno sa ekstenzijom

        private string _filename;

        /// <summary>
        /// ime fajla zajedno sa ekstenzijom
        /// </summary>
        public string filename
        {
            get { return _filename; }
            set
            {
                _filename = value;
                OnPropertyChanged("filename");
            }
        }

        #endregion --- filename ------- ime fajla zajedno sa ekstenzijom

        #region --- localOutputPath ------- lokalna putanja u izlaznom folderu, sve sa filename.extension na kraju

        private string _localOutputPath;

        /// <summary>
        /// lokalna putanja u izlaznom folderu, sve sa filename.extension na kraju
        /// </summary>
        public string localOutputPath
        {
            get { return _localOutputPath; }
            set
            {
                _localOutputPath = value;
                OnPropertyChanged("localOutputPath");
            }
        }

        #endregion --- localOutputPath ------- lokalna putanja u izlaznom folderu, sve sa filename.extension na kraju

        #region -----------  sourceFilePath  -------  [putanja ka fajlu koji treba da se ubaci pored izvestaja]

        private string _sourceFilePath; // = new String();

        /// <summary>
        /// putanja ka fajlu koji treba da se ubaci pored izvestaja
        /// </summary>
        // [XmlIgnore]
        [Category("reportIncludeFile")]
        [DisplayName("sourceFilePath")]
        [Description("putanja ka fajlu koji treba da se ubaci pored izvestaja")]
        public string sourceFilePath
        {
            get { return _sourceFilePath; }
            set
            {
                // Boolean chg = (_sourceFilePath != value);
                _sourceFilePath = value;
                OnPropertyChanged("sourceFilePath");
                // if (chg) {}
            }
        }

        #endregion -----------  sourceFilePath  -------  [putanja ka fajlu koji treba da se ubaci pored izvestaja]

        #region -----------  filetype  -------  [Tip fajla koji treba da se ubaci]

        private reportIncludeFileType _filetype; // = new reportIncludeFileType();

        /// <summary>
        /// Tip fajla koji treba da se ubaci
        /// </summary>
        // [XmlIgnore]
        [Category("reportIncludeFile")]
        [DisplayName("filetype")]
        [Description("Tip fajla koji treba da se ubaci")]
        public reportIncludeFileType filetype
        {
            get { return _filetype; }
            set
            {
                // Boolean chg = (_filetype != value);
                _filetype = value;
                OnPropertyChanged("filetype");
                // if (chg) {}
            }
        }

        #endregion -----------  filetype  -------  [Tip fajla koji treba da se ubaci]

        #region -----------  doLocalCopy  -------  [Da li treba da napravi lokalnu kopiju fajla koji se ubacuje]

        private bool _doLocalCopy; // = new Boolean();

        /// <summary>
        /// Da li treba da napravi lokalnu kopiju fajla koji se ubacuje
        /// </summary>
        // [XmlIgnore]
        [Category("reportIncludeFile")]
        [DisplayName("doLocalCopy")]
        [Description("Da li treba da napravi lokalnu kopiju fajla koji se ubacuje")]
        public bool doLocalCopy
        {
            get { return _doLocalCopy; }
            set
            {
                // Boolean chg = (_doLocalCopy != value);
                _doLocalCopy = value;
                OnPropertyChanged("doLocalCopy");
                // if (chg) {}
            }
        }

        #endregion -----------  doLocalCopy  -------  [Da li treba da napravi lokalnu kopiju fajla koji se ubacuje]
    }
}