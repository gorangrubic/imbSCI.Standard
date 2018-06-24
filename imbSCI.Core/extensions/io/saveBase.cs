// --------------------------------------------------------------------------------------------------------------------
// <copyright file="saveBase.cs" company="imbVeles" >
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
namespace imbSCI.Core.extensions.io
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Basic set of file save extensions and tools
    /// </summary>
    public static class saveBase
    {
        /// <summary>
        /// Appends new content on the end of the file
        /// </summary>
        /// <param name="xmlLines">The XML lines or what ever String content part</param>
        /// <param name="_path">Filepath to save onto</param>
        /// <returns>TRUE on success</returns>
        public static Boolean saveToEndOfFile(this IEnumerable<String> xmlLines, String _path)
        {
            return saveAtEndOfFile(_path, xmlLines);
        }

        /// <summary>
        /// Saves IEnumerable collection of strings onto <c>_path</c>
        /// </summary>
        /// <param name="xmlLines">XML or other kind of String content to be saved on the path.</param>
        /// <param name="_path">Filepath to save onto</param>
        /// <returns>TRUE on success</returns>
        public static Boolean saveContentOnFilePath(this IEnumerable<String> xmlLines, String _path)
        {
            return saveToFile(_path, xmlLines);
        }

        /// <summary>
        /// Snima tekstualni fajl na zadatoj putanji
        /// </summary>
        /// <param name="_path">gde da snimi fajl</param>
        /// <param name="xmlLines">Sadrzaj koji treba da snimi</param>
        /// <returns></returns>
        public static Boolean saveToFile(String _path, IEnumerable<String> xmlLines)
        {
            Boolean output = true;
            StreamWriter tmpStream;

            _path = _path.Replace(Environment.NewLine, "");

            if (File.Exists(_path))
            {
                File.Delete(_path);
            }

            tmpStream = File.CreateText(_path);
            fileOpsBase.lastFilePath = _path;

            foreach (String iLine in xmlLines)
            {
                tmpStream.WriteLine(iLine);
            }

            tmpStream.Close();
            tmpStream.Dispose();
            output = true;
            // imbCoreManager.lastCalledPath = _path;

            //  logSystem.log("File saved: " + _path + " (ln:" + xmlLines.Count() + ")", logType.Notification);
            return output;
        }

        /// <summary>
        /// Snima na kraj Text fajla ---
        /// </summary>
        /// <param name="_path"></param>
        /// <param name="xmlLines"></param>
        /// <returns></returns>
        public static Boolean saveAtEndOfFile(this String _path, IEnumerable<String> xmlLines)
        {
            Boolean output = true;
            _path = _path.getResolvedPath(true);
            StreamWriter tmpStream = File.AppendText(_path);

            fileOpsBase.lastFilePath = _path;
            //aceCommonInputSystem.main.lastFilePath = _path;

            foreach (String iLine in xmlLines)
            {
                tmpStream.WriteLine(iLine);
            }

            tmpStream.Close();
            tmpStream.Dispose();
            output = true;
            // imbCoreManager.lastCalledPath = _path;

            //  logSystem.log("File saved: " + _path + " (ln:" + xmlLines.Count() + ")", logType.Notification);
            return output;
        }

        //public static Boolean saveToFile(String _path, String _data)
        //{
        //    List<String> xmlLines = new List<string>();
        //    //if (_path.Contains(Environment.NewLine) || (_data.ContainsAll(":", "\\", ".")))
        //    //{
        //    //    xmlLines.Add(_path);
        //    //    _path = _data;
        //    //}
        //    //else
        //    //{
        //    //    xmlLines.Add(_data);
        //    //}

        //    return saveToFile(_path, xmlLines);
        //}

        /// <summary>
        /// Jednostavna saveToFile funkcija -- TRUE ako je uspelo snimanje
        /// </summary>
        /// <param name="_path">Putanja</param>
        /// <param name="_data">Podatak u string formatu</param>
        /// <returns>da li je uspeo</returns>
        public static Boolean saveToFile(this String _path, String _data)
        {
            List<String> xmlLines = new List<string>();
            xmlLines.Add(_data);
            //if (_path.Contains(Environment.NewLine) || (_data.ContainsAll(":","\\", ".")))
            //{
            //    xmlLines.Add(_path);
            //    _path = _data;
            //} else
            //{
            //    xmlLines.Add(_data);
            //}

            return saveToFile(_path, xmlLines);
        }

        //private static saveBinaryLock

        /// <summary>
        /// Učitava fajl i vraća niz binarnih vrednosti
        /// </summary>
        /// <param name="_path"></param>
        /// <returns></returns>
        public static Boolean saveBinary(this String _path, Byte[] _output)
        {
            if (File.Exists(_path)) File.Delete(_path);
            try
            {
                File.WriteAllBytes(_path, _output);
                return true;
            }
            catch (Exception ex)
            {
                //logSystem.log(ex.Message, logType.Warning);
                return false;
            }
        }
    }
}