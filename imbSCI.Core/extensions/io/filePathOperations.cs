// --------------------------------------------------------------------------------------------------------------------
// <copyright file="filePathOperations.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.files;
    using imbSCI.Core.reporting;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using System;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    /// <summary>
    /// Set of extensions processing file path strings and performing file operations
    /// </summary>
    public static class filePathOperations
    {
        /// <summary>
        /// Gets the image format by extension.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">File extension from [" + filepath + "] not recognized - filepath</exception>
        public static ImageFormat GetImageFormatByExtension(this String filepath)
        {
            String ext = Path.GetExtension(filepath).Trim(".".ToArray()).ToLower();

            switch (ext)
            {
                case "jpg":
                case "jpeg":
                    return ImageFormat.Jpeg;
                    break;

                case "png":
                    return ImageFormat.Png;
                    break;

                case "bmp":
                    return ImageFormat.Bmp;

                    break;

                default:

                    foreach (var pi in typeof(ImageFormat).GetProperties(BindingFlags.Static | BindingFlags.Public))
                    {
                        if (pi.Name.Equals(ext, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return (ImageFormat)pi.GetValue(null, null);
                        }
                    }

                    break;
            }

            throw new ArgumentException("File extension from [" + filepath + "] not recognized", "filepath");

            return null;
        }

        public static Regex regex_propertyNameSelect = new Regex(@"\.?([^\.\[]+)\[{1}");

        /// <summary>
        /// Izdvaja ime propertija u member path-u. --- brise sve simbole koji cine elemente patha
        /// </summary>
        /// <remars>
        /// Primer 01: Za environment.serialization.typeName[key]   - vraca typeName
        /// Primer 02: Za environment.serialization.typeName        - vraca typeName
        /// </remars>
        /// <param name="propertyFullName"></param>
        /// <returns></returns>
        public static String getCleanPropertyName(this string propertyFullName)
        {
            if (imbSciStringExtensions.isNullOrEmptyString(propertyFullName)) return "";
            if (regex_propertyNameSelect.IsMatch(propertyFullName))
            {
                propertyFullName = regex_propertyNameSelect.Match(propertyFullName).Value;
            }
            else
            {
                propertyFullName = propertyFullName.getPathVersion(-1, ".");
            }

            propertyFullName = propertyFullName.Trim(".-:".ToArray());
            return propertyFullName;
        }

        /// <summary>
        /// Inserts timestamp as filename sufix
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static String addTimeStamp(this String filename)
        {
            String filenameOnly = Path.GetFileNameWithoutExtension(filename);
            String fileExtension = Path.GetExtension(filename);
            String basePath = Path.GetDirectoryName(filename);

            String timestamp = String.Format("dd-MM-yyyy-HH-mm-ss", DateTime.Now);

            String output = "";
            output = output.add(basePath, "");
            output = output.add(filenameOnly, "\\");
            output = output.add(timestamp, "_");
            output = output.add(fileExtension, ".");
            filename = output;
            return output;
        }

        /// <summary>
        /// 2017: inserts supplied sufix before .extension and after filenameWithoutExtension. It will put \"_\" between original filename and sufix
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="sufix"></param>
        /// <returns></returns>
        public static String addFileNameSufix(this String filepath, String sufix)
        {
            String filenameOnly = Path.GetFileNameWithoutExtension(filepath);
            String fileExtension = Path.GetExtension(filepath);
            String basePath = Path.GetDirectoryName(filepath);

            String output = "";
            output = output.add(basePath, "");
            output = output.add(filenameOnly, "\\");
            output = output.add(sufix, "_");
            output = output.add(fileExtension, ".");
            //filepath = output;
            return output;
        }

        public const Int32 AUTORENAME_FILE_COUNT_LIMIT = 500;

        /// <summary>
        /// Path unique sufix automatic counter. It will add to filename numeric counter of priorly existed files. Also it may add preTestPrefix on filename at beginning of the procedure.
        /// </summary>
        /// <param name="filepath">Putanja ka fajlu koji zeli da snimi</param>
        /// <param name="addPreTestPrefix">Adds this sufix before testing on existing file</param>
        /// <returns>Putanju dopunjenu sa _n sufixom tako da je fajl unikatan</returns>
        public static String addUniqueSufix(this String filepath, String preTestPrefix = "")
        {
            if (String.IsNullOrEmpty(filepath))
            {
                filepath = "";
            }

            String filenameOnly = Path.GetFileNameWithoutExtension(filepath);
            String fileExtension = Path.GetExtension(filepath);
            String basePath = Path.GetDirectoryName(filepath);

            basePath = basePath.getPathOrDefault();

            if (!imbSciStringExtensions.isNullOrEmptyString(preTestPrefix)) filenameOnly = filenameOnly.add(preTestPrefix, "_");

            List<string> bcks = new List<string>();

            bcks.AddRange(Directory.EnumerateFiles(basePath, filenameOnly + "*" + fileExtension));

            String timestamp = "";
            if (bcks.Count > 0)
            {
                timestamp = "_" + bcks.Count().ToString();
            }

            String output = "";
            output = output.add(basePath, "");
            output = output.add(filenameOnly, "\\");
            output = output.add(timestamp, "_");
            output = output.add(fileExtension, ".");
            filepath = output;

            Int32 l = 0;
            while (File.Exists(output))
            {
                l++;
                output = output.addUniqueSufix("_double_");
                if (l > AUTORENAME_FILE_COUNT_LIMIT)
                {
                    throw new ArgumentOutOfRangeException(nameof(filepath), "Number of with the same name, at this path broken [" + l + "] limit :: " + output);
                    return output;
                }
            }

            return output;
        }

        private static Object PATH_FORBIDEN_Lock = new object();

        private static List<String> _PATH_FORBIDEN;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static List<String> PATH_FORBIDEN
        {
            get
            {
                if (_PATH_FORBIDEN == null)
                {
                    lock (PATH_REPLACE_Lock)
                    {
                        if (_PATH_FORBIDEN == null)
                        {
                            _PATH_FORBIDEN = new List<String>();
                            var charsToRemove = Path.GetInvalidPathChars();
                            for (int i = 0; i < charsToRemove.Length; i++)
                            {
                                Char ch = charsToRemove[i];
                                switch (ch)
                                {
                                    case ':':
                                    case '\\':

                                        break;

                                    default:
                                        _PATH_FORBIDEN.Add(ch.ToString());
                                        break;
                                }
                            }
                        }
                    }
                }
                return _PATH_FORBIDEN;
            }
        }

        private static Object PATH_REPLACE_Lock = new Object();

        private static Dictionary<String, String> _PATH_REPLACE;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static Dictionary<String, String> PATH_REPLACE
        {
            get
            {
                if (_PATH_REPLACE == null)
                {
                    lock (PATH_REPLACE_Lock)
                    {
                        if (_PATH_REPLACE == null)
                        {
                            _PATH_REPLACE = new Dictionary<String, String>();
                            _PATH_REPLACE.Add("~", "_");
                            _PATH_REPLACE.Add("@", "");
                            _PATH_REPLACE.Add("%", "");
                            _PATH_REPLACE.Add("$", "");
                            _PATH_REPLACE.Add("?", "");
                            _PATH_REPLACE.Add("=", "");
                            _PATH_REPLACE.Add("\"", "");
                            _PATH_REPLACE.Add("'", "");
                            _PATH_REPLACE.Add("`", "");
                            _PATH_REPLACE.Add("#", "");
                            _PATH_REPLACE.Add("*", "");
                            //                    _PATH_REPLACE.Add(":", "");
                            _PATH_REPLACE.Add(";", "");
                            _PATH_REPLACE.Add("    ", " ");
                            _PATH_REPLACE.Add("   ", " ");
                            _PATH_REPLACE.Add("  ", " ");
                            _PATH_REPLACE.Add(" ", "_");
                        }
                    }
                }
                return _PATH_REPLACE;
            }
        }

        private static Object FILENAME_FORBIDEN_Lock = new Object();

        private static List<String> _FILENAME_FORBIDEN;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static List<String> FILENAME_FORBIDEN
        {
            get
            {
                if (_FILENAME_FORBIDEN == null)
                {
                    lock (FILENAME_FORBIDEN_Lock)
                    {
                        if (_FILENAME_FORBIDEN == null)
                        {
                            _FILENAME_FORBIDEN = new List<String>();
                            var charsToRemove = Path.GetInvalidFileNameChars();
                            for (int i = 0; i < charsToRemove.Length; i++)
                            {
                                Char ch = charsToRemove[i];

                                switch (ch)
                                {
                                    //case '\\':

                                    //    break;
                                    default:
                                        _FILENAME_FORBIDEN.Add(ch.ToString());
                                        break;
                                }
                            }
                        }
                    }
                }
                return _FILENAME_FORBIDEN;
            }
        }

        private static Object FILENAMEREPLACE_CreationLock = new Object();

        private static Dictionary<String, String> _FILENAME_REPLACE;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static Dictionary<String, String> FILENAME_REPLACE
        {
            get
            {
                if (_FILENAME_REPLACE == null)
                {
                    lock (FILENAMEREPLACE_CreationLock)
                    {
                        if (_FILENAME_REPLACE == null)
                        {
                            _FILENAME_REPLACE = new Dictionary<String, String>();
                            _FILENAME_REPLACE.Add("~", "_");
                            _FILENAME_REPLACE.Add("@", "");
                            _FILENAME_REPLACE.Add("%", "");
                            _FILENAME_REPLACE.Add("$", "");
                            _FILENAME_REPLACE.Add("?", "");
                            _FILENAME_REPLACE.Add("=", "");
                            _FILENAME_REPLACE.Add("\"", "");
                            _FILENAME_REPLACE.Add("'", "");
                            _FILENAME_REPLACE.Add("`", "");
                            _FILENAME_REPLACE.Add("#", "");
                            _FILENAME_REPLACE.Add("*", "");
                            _FILENAME_REPLACE.Add(":", "");
                            _FILENAME_REPLACE.Add(";", "");
                            _FILENAME_REPLACE.Add("    ", " ");
                            _FILENAME_REPLACE.Add("   ", " ");
                            _FILENAME_REPLACE.Add("  ", " ");
                            _FILENAME_REPLACE.Add(" ", "_");
                        }
                    }
                }
                return _FILENAME_REPLACE;
            }
        }

        /// <summary>
        /// Removes all substrings from the input string
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="chars">The chars.</param>
        /// <returns></returns>
        public static String removeStrings(this String input, IEnumerable<String> chars)
        {
            String output = input;

            foreach (String c in chars)
            {
                output = output.Replace(c, "");
            }
            return output;
        }

        public static String getCleanFilepath(this String fullpath, String extension = "")
        {
            String output = fullpath;

            //for (int i = 0; i < PATH_REPLACE.Count(); i++)
            //{
            //}
            foreach (var chp in PATH_REPLACE) // <--- ovde ubaciti for petlju
            {
                output = output.Replace(chp.Key, chp.Value);
            }

            output = output.removeStrings(PATH_FORBIDEN); //.Remove(PATH_FORBIDEN.ToArray());

            if (!String.IsNullOrEmpty(extension))
            {
                output = output.add(extension, ".");
            }
            return output;
        }

        /// <summary>
        /// 2017c> Makes proper filename our of any string -- better than> <see cref="getCleanFilePath(string)"/> (Izbacuje iz imena fajla sve nepravilne karaktere)
        /// </summary>
        /// <param name="name">filename without extension</param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static String getFilename(this String name, String extension = "")
        {
            if (imbSciStringExtensions.isNullOrEmptyString(name)) name = "noname_" + imbStringGenerators.getRandomString(8);
            String output = name.ToLower();

            output = output.removeStrings(FILENAME_FORBIDEN);

            foreach (var chp in FILENAME_REPLACE)
            {
                output = output.Replace(chp.Key, chp.Value);
            }

            if (!String.IsNullOrEmpty(extension))
            {
                output = output.add(extension, ".");
            }
            return output;
        }

        /// <summary>
        /// 2017: Removes forbiden characters from file path and double \\\\ but leaves underline
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static String getCleanFilePath(this String path)
        {
            if (imbSciStringExtensions.isNullOrEmptyString(path)) path = "_null_";
            String output = path.getCleanFilepath();
            return output;
        }

        /// <summary>
        /// Opens the file to string list
        /// </summary>
        /// <param name="_file">File to open</param>
        /// <param name="skipEmptyLines">if set to <c>true</c> [skip empty lines].</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>List of lines from loaded string</returns>
        public static List<String> openFileToList(this FileInfo _file, Boolean skipEmptyLines, Encoding encoding = null)
        {
            return _file.FullName.openFileToList(skipEmptyLines, encoding);
        }

        /// <summary>
        /// Opens the file to string list
        /// </summary>
        /// <param name="_path">The path.</param>
        /// <param name="skipEmptyLines">if set to <c>true</c> [skip empty lines].</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>List of lines from loaded string</returns>
        public static List<String> openFileToList(this String _path, Boolean skipEmptyLines, Encoding encoding = null)
        {
            List<string> output = new List<string>();
            String line = "";
            StreamReader tmpStream;
            int eLine = 0;

            tmpStream = new StreamReader(_path);

            while ((line = tmpStream.ReadLine()) != null)
            {
                if (skipEmptyLines && String.IsNullOrWhiteSpace(line))
                {
                    eLine++;
                }
                else
                {
                    output.Add(line);
                }
            }

            tmpStream.Close();
            tmpStream.Dispose();

            return output;
        }

        private static Object saveStringToFileLock = new object();

        /// <summary>
        /// Saves complete string into path
        /// </summary>
        /// <param name="data"></param>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static FileInfo saveStringToFile(this String data, String path, getWritableFileMode mode = getWritableFileMode.overwrite, Encoding encoding = null)
        {
            lock (saveStringToFileLock)
            {
                if (encoding == null) encoding = UnicodeEncoding.Unicode;

                FileInfo fi = path.getWritableFile(mode);

                Directory.CreateDirectory(fi.DirectoryName);

                File.WriteAllText(fi.FullName, data, encoding);

                return fi;
            }
        }

        /// <summary>
        /// Miliseconds to wait before retry with failed operation
        /// </summary>
        public static Int32 RETRY_DELAY = 5000;

        public static String ApplicationHomePath { get; set; } = "";

        //public static String CheckApplicationHomePath()
        //{
        //    if (ApplicationHomePath == "")
        //    {
        //        AppDomain.CurrentDomain.BaseDirectory
        //    }

        //}

        /// <summary>
        /// Gets writable file based on selected mode. By default it will do overwrite. Autorename calls <see cref="addUniqueSufix(string, string)" /> extension that counts existing files and sets proper number.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        /// <exception cref="imbFileException">getWritableFile [" + mode.ToString() + "] failed when directory should be created from [" + dir.toStringSafe() + "]. "
        ///                         + ex.Message - null - null</exception>
        public static FileInfo getWritableFile(this String path, getWritableFileMode mode = getWritableFileMode.overwrite, ILogBuilder logger = null)
        {
            String output = path;
            String existing = path;

            if (path.Length > 200)
            {
                if (logger != null) logger.log("Path length critical: " + path.Length);
            }

            String dir = Path.GetDirectoryName(path);
            if (!imbSciStringExtensions.isNullOrEmpty(dir))
            {
                try
                {
                    var di = Directory.CreateDirectory(dir);
                }
                catch (Exception ex)
                {
                    throw new imbFileException("getWritableFile [" + mode.ToString() + "] failed when directory should be created from [" + dir.toStringSafe() + "]. "
                        + ex.Message, ex, null, path, null);
                }
            }

            Int32 retryCount = 5;

            Boolean jobDone = true;

            while (retryCount > 0)
            {
                jobDone = true;

                if (File.Exists(output))
                {
                    switch (mode)
                    {
                        case getWritableFileMode.appendFile:
                            break;

                        case getWritableFileMode.newOrExisting:
                            // do nothing

                            break;

                        case getWritableFileMode.existing:
                            break;

                        case getWritableFileMode.overwrite:

                            try
                            {
                                File.Delete(output);
                            }
                            catch (Exception ex)
                            {
                                if (logger != null) logger.log("The file [" + path + "] is blocked by another application. " + ex.Message);
                                jobDone = false;
                            }
                            break;

                        case getWritableFileMode.autoRenameExistingOnOtherDate:
                            FileInfo exfi = new FileInfo(output);
                            if (exfi.CreationTime.Date != DateTime.Now.Date)
                            {
                                existing = existing.addUniqueSufix("_" + exfi.CreationTime.ToString("MM-dd"));
                                File.Copy(output, existing);
                                try
                                {
                                    File.Delete(output);
                                }
                                catch (Exception ex)
                                {
                                    existing = output.addUniqueSufix("_" + DateTime.Now.ToString("MM-dd"));

                                    if (logger != null) logger.log("The log file was used by another process. Saving to: " + existing);
                                }
                            }
                            else
                            {
                            }
                            break;

                        case getWritableFileMode.autoRenameThis:
                            output = output.addUniqueSufix();
                            break;

                        case getWritableFileMode.autoRenameExistingToNextPage:
                            output = output.addUniqueSufix("_page_");
                            break;

                        case getWritableFileMode.autoRenameExistingToBack:
                            existing = existing.addUniqueSufix("backup");
                            File.Copy(output, existing);
                            break;

                        case getWritableFileMode.autoRenameExistingToOld:
                            existing = existing.addUniqueSufix("old");
                            File.Copy(output, existing);
                            break;

                        case getWritableFileMode.autoRenameExistingToOldOnce:

                            existing = existing.add("_old");
                            if (File.Exists(existing))
                            {
                                File.Delete(existing);
                            }
                            File.Copy(output, existing);
                            break;
                    }
                }
                if (jobDone) retryCount = 0;
                if (retryCount > 0)
                {
                    if (logger != null) logger.log(retryCount.ToString() + " retry [" + mode + "] file [" + existing + "]");
                    Thread.Sleep(RETRY_DELAY);
                    retryCount--;
                }
            }
            FileInfo fi = new FileInfo(output);
            return fi;
        }

        /// <summary>
        /// 2017: Returns DirectoryInfo - found or created for path made from> current directory, folderEnum and subfolderPath
        /// </summary>
        /// <param name="folderEnum"></param>
        /// <param name="subfolderPath"></param>
        /// <returns></returns>
        public static DirectoryInfo getAbsoluteDirectory(this Enum folderEnum, String subfolderPath = "")
        {
            String path = folderEnum.getAbsoluteFilePath(subfolderPath);

            DirectoryInfo di = Directory.CreateDirectory(path);

            return di;
        }

        /// <summary>
        /// 2017: Returns the absolute path - starting with the current directory, adding folderEnum and subfolderPath - if any supplied.
        /// </summary>
        /// <param name="folderEnum"></param>
        /// <param name="subfolderPath"></param>
        /// <returns>Absolute path</returns>
        public static String getAbsoluteFilePath(this Enum folderEnum, String subfolderPath = "")
        {
            String rootPath = Directory.GetCurrentDirectory();
            String output = rootPath;

            if (folderEnum != null)
            {
                if (folderEnum.ToString() != "none")
                {
                    output = output.add(folderEnum.ToString(), "\\");
                }
            }
            if (!imbSciStringExtensions.isNullOrEmptyString(subfolderPath))
            {
                if (Path.IsPathRooted(subfolderPath))
                {
                    if (subfolderPath.ToLower().StartsWith(rootPath.ToLower()))
                    {
                        subfolderPath = imbSciStringExtensions.removeStartsWith(subfolderPath, rootPath);
                    }
                }
                output = output.add(subfolderPath, "\\");
            }
            output = output.getCleanFilePath();
            return output;
        }

        /// <summary>
        /// Ako je path prazan onda vraca podrazumevani path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="defPath"></param>
        /// <returns></returns>
        public static String getPathOrDefault(this String path)
        {
            path = path.Trim();
            if (String.IsNullOrEmpty(path))
            {
                return Directory.GetCurrentDirectory();
            }
            return path;
        }

        /// <summary>
        /// Pronalazi fajlove koji odgovaraju pretrazi
        /// </summary>
        /// <param name="searchExtension">ekstenzija koju treba da pretra�i</param>
        /// <param name="pathToSearch"></param>
        /// <param name="includeSubDirectories"></param>
        /// <returns></returns>
        public static List<string> findFiles(String searchExtension = "job", string pathToSearch = "", Boolean includeSubDirectories = false)
        {
            List<string> output = new List<string>();
            if (String.IsNullOrEmpty(searchExtension)) return output;
            pathToSearch = pathToSearch.getPathOrDefault();
            if (!Directory.Exists(pathToSearch))
            {
                return output;
            }
            String query = "*".add(searchExtension, ".");

            query = query.Replace("..", ".");

            if (includeSubDirectories)
            {
                output.AddRange(Directory.EnumerateFiles(pathToSearch, query, SearchOption.AllDirectories));
            }
            else
            {
                output.AddRange(Directory.EnumerateFiles(pathToSearch, query, SearchOption.TopDirectoryOnly));
            }

            return output;
        }

        public static String Add(this String _path, String _pathSufix)
        {
            String output = "";
            /*String path = Path.GetFullPath(_path);
            String pathSufix = Path.GetFullPath(_pathSufix);
            */
            output = Path.Combine(_path, _pathSufix);
            // if (pathSufix.StartsWith(@"\")) pathSufix = pathSufix.Substring(0);
            //if (!path.EndsWith(@"\")) path = path + @"\";
            //  output = path + pathSufix;

            //if (pathSufix.StartsWith("\\")) pathSufix = pathSufix.Substring(0);

            /*
            if (!path.EndsWith("\\") && pathSufix.StartsWith("\\"))
            {
                path = path + "\\";
            }*/

            return output;
        }
    }
}