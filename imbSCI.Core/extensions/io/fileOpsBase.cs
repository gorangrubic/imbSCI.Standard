// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileOpsBase.cs" company="imbVeles" >
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
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using System;
    using System.IO;

    public static class fileOpsBase
    {
        /// <summary>
        /// Gets the file extension.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static String getFileExtension(this appendLinkType type)
        {
            switch (type)
            {
                case appendLinkType.anchor:
                    break;

                case appendLinkType.image:
                    return ".png";
                    break;

                case appendLinkType.link:
                    return ".html";
                    break;

                case appendLinkType.reference:
                case appendLinkType.referenceImage:
                    return ".png";
                    break;

                case appendLinkType.referenceLink:
                    break;

                case appendLinkType.scriptLink:
                case appendLinkType.scriptPostLink:
                    return ".js";
                    break;

                case appendLinkType.styleLink:
                    return ".css";
                    break;

                case appendLinkType.unknown:
                    break;

                default:
                    return ".js";
                    break;
            }

            return "";
        }

        private static Int32 _standardBlockSize = 5000;

        /// <summary>
        ///
        /// </summary>
        public static Int32 standardBlockSize
        {
            get
            {
                return _standardBlockSize;
            }
            set
            {
                _standardBlockSize = value;
            }
        }

        private static DirectoryInfo _applicationFolder; //= new String();

        /// <summary>
        ///
        /// </summary>
        public static DirectoryInfo applicationFolder
        {
            get
            {
                if (_applicationFolder == null)
                {
                    applicationFolder = new DirectoryInfo(Directory.GetCurrentDirectory());
                }
                return _applicationFolder;
            }
            set
            {
                _applicationFolder = value;
            }
        }

        /// <summary>
        /// Pravi i/ili proverava ispravnost search filtera. Ako je null onda daje *.*; Ako dobije samo ekstenziju ili ekstenziju sa . priprema *.ext filter. Ako dobije filter koji ima filename i ext onda dodaje * ispred .
        /// </summary>
        /// <param name="filterOrExt"></param>
        /// <returns>Search filter koji se koristi u Files i Directory pretragama</returns>
        public static String getFileSearchFilter(this String filterOrExt)
        {
            if (String.IsNullOrEmpty(filterOrExt))
            {
                return "*.*";
            }

            String output = filterOrExt.Trim();

            if (!output.Contains("*."))
            {
                output = "*." + output;
            }
            else
            {
                if (output.StartsWith("*."))
                {
                }
                else if (output.StartsWith("."))
                {
                    output = "*." + output;
                }
                else if (output.Contains("*"))
                {
                    output = "." + output;
                }
                else if (output.Contains("."))
                {
                    output = output.Replace(".", "*.");
                }
            }

            return output;
        }

        /// <summary>
        /// Proverava ispravnost unete putanje, vraca apsolutnu formu (u odnosu na mesto izvrsavanja programa)
        /// </summary>
        /// <param name="path">Relativna ili apsolutna putanja koju treba proveriti</param>
        /// <returns></returns>
        public static String getResolvedPath(this String path, Boolean includeFilename = true)
        {
            String output = "";
            String filename = "";

            if (String.IsNullOrEmpty(path))
            {
                //var pt = aceCommonInput.aceCommonInputSystem.main.installedPath;
                path = Environment.CurrentDirectory;
            }

            path = path.Replace(Environment.NewLine, "");

            if (String.IsNullOrEmpty(path))
            {
                //var pt = aceCommonInput.aceCommonInputSystem.main.installedPath;
                path = Environment.CurrentDirectory;
            }

            if (Path.HasExtension(path))
            {
                if (includeFilename) filename = Path.GetFileName(path);
                output = Path.GetDirectoryName(path);
            }
            else
            {
                output = path;
            }

            output = imbSciStringExtensions.removeEndsWith(output, "\\");
            var di = getDirectory(output);
            output = di.FullName;

            if (!String.IsNullOrEmpty(filename))
            {
                output = imbSciStringExtensions.add(output, filename, "\\");
            }

            return output;
        }

        /// <summary>
        /// Vraca directory info za prosledjen path i generise putanju automatski ako je nema
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DirectoryInfo getDirectory(this String path, Boolean returnOnlyCreated = false)
        {
            if (path == "none")
            {
                path = "";
            }

            if (String.IsNullOrEmpty(path))
            {
                //var pt = aceCommonInput.aceCommonInputSystem.main.installedPath;
                path = Environment.CurrentDirectory;
            }

            if (!Path.IsPathRooted(path))
            {
                path = imbSciStringExtensions.add(Environment.CurrentDirectory, path, "\\");
            }

            Boolean _returnDirInfo = true;

            if (!Directory.Exists(path))
            {
                // logSystem.log("Creating folder: " + path, logType.Execution);
                Directory.CreateDirectory(path);
                if (returnOnlyCreated)
                {
                    _returnDirInfo = false;
                }
            }
            else
            {
            }

            if (_returnDirInfo)
            {
                return new DirectoryInfo(path);
            }

            return null;
        }

        //public static DirectoryInfo toDirectory(this outputFolder folder, Boolean insideInstalledPath = true)
        //{
        //    return getDirectory((string) folder.ToString(), insideInstalledPath);
        //}

        ///// <summary>
        ///// Vraca info za dati output folder - automatski generise folder ako treba
        ///// </summary>
        ///// <param name="folder"></param>
        ///// <returns></returns>
        //public static DirectoryInfo getDirectory(this outputFolder folder, Boolean insideInstalledPath = true)
        //{
        //    return getDirectory((string) folder.ToString(), insideInstalledPath);
        //}

        //internal static String getPathInsideDirectory(String filename, outputFolder folder = outputFolder.system,
        //                                              Boolean insideInstalledPath = true)
        //{
        //    String output = "";
        //    output = outputFolder.system.ToString() +"\\";
        //    return output + filename;
        //}

        /// <summary>
        /// Kopiranje fajlova koji odgovaraju upitu
        /// </summary>
        /// <param name="sourcePath">Putanja sa koje se kopira</param>
        /// <param name="clientPath">Putanja na koju se kopira - samo folder path, ne filename</param>
        /// <param name="subjectTitle">Naziv fajla koji se kopira</param>
        /// <returns>Diagnostic message</returns>
        public static String copyFile(this String sourcePath, String clientPath, String subjectTitle = "")
        {
            String output = "";
            String sourceFilename = subjectTitle;

            if (subjectTitle == "")
            {
                sourceFilename = Path.GetFileName(sourcePath);
            }

            sourcePath = imbSciStringExtensions.ensureEndsWith(Path.GetDirectoryName(sourcePath), "\\");
            clientPath = imbSciStringExtensions.ensureEndsWith(Path.GetDirectoryName(clientPath), "\\");

            String sourceFilePath = imbSciStringExtensions.ensureEndsWith(sourcePath, sourceFilename);
            String targetFilePath = imbSciStringExtensions.ensureEndsWith(clientPath, sourceFilename);

            var fi = new DirectoryInfo(clientPath);

            if (fi.Exists)
            {
                try
                {
                    File.Copy(sourceFilePath, targetFilePath);
                    //File.Copy(sourcePath, clientPath + sourceFilename, true);
                    output = "[" + subjectTitle + "] OK (" + clientPath + ")";
                    // if (!(subjectTitle=="")) //logSystem.log(output, imbFramework.imbConsole.logType.Done);
                }
                catch (Exception ex)
                {
                    output = "[" + subjectTitle + "] " + clientPath + " (source: " + sourcePath + ") " + Environment.NewLine + ex.Message;
                    // if (!(subjectTitle == "")) //logSystem.log(output, imbFramework.imbConsole.logType.CriticalWarning);
                }
            }
            else
            {
                output = "[" + subjectTitle + "] path not reached: " + clientPath;
                //if (!(subjectTitle == "")) //logSystem.log(output, imbFramework.imbConsole.logType.FatalError);
            }
            return output;
        }

        /// <summary>
        /// Zips the directory.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="zipFilename">The zip filename.</param>
        /// <param name="zipAsSubFolder">if set to <c>true</c> [zip as sub folder].</param>
        /// <param name="selectionPattern">The selection pattern.</param>
        /// <returns></returns>
        public static Object zipDirectory(this DirectoryInfo from, DirectoryInfo to, String zipFilename, Boolean zipAsSubFolder = false, String selectionPattern = "*.*")
        {
            String sourceFolder = @from.FullName;
            String destinationPath; // = to.FullName;
            zipFilename = Path.GetFileNameWithoutExtension(zipFilename);
            zipFilename = imbSciStringExtensions.ensureEndsWith(zipFilename, ".zip");

            destinationPath = Path.Combine(to.FullName, zipFilename);

            FileInfo zipFile = destinationPath.getWritableFile(getWritableFileMode.autoRenameExistingOnOtherDate);

            throw new NotImplementedException();

            //ZipFile zip = new ZipFile();

            //String pathPrefix = @from.FullName;
            //if (zipAsSubFolder) pathPrefix = @from.Parent.FullName;

            //String localPath = imbSciStringExtensions.removeStartsWith(@from.FullName, pathPrefix);
            //ZipEntry localDir = zip.AddDirectory(@from.FullName, localPath);

            //zip.Save(destinationPath);
            //return zip;
            return null;
        }

        /// <summary>
        /// Copies the directory.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="selectionPattern">The selection pattern.</param>
        /// <param name="copyAsSubfolder">if set to <c>true</c> [copy as subfolder].</param>
        /// <param name="copySubFolders">if set to <c>true</c> [copy sub folders].</param>
        /// <returns></returns>
        public static DirectoryInfo copyDirectory(this DirectoryInfo from, DirectoryInfo to, String selectionPattern = "*.*", Boolean copyAsSubfolder = false, Boolean copySubFolders = true)
        {
            String sourceFolder = @from.FullName;
            String destinationFolder; // = to.FullName;

            if (copyAsSubfolder)
            {
                destinationFolder = Path.Combine(to.FullName, @from.Name);
            }
            else
            {
                destinationFolder = to.FullName;
            }

            copyFiles(sourceFolder, destinationFolder, "copyDirectory", selectionPattern, copySubFolders);
            return to;
        }

        /// <summary>
        /// Kopira sve fajlove koji odgovaraju podesavanjima.
        /// </summary>
        /// <param name="sourceFolder">Odakle kopira</param>
        /// <param name="destinationFolder">Gde kopira</param>
        /// <param name="subjectTitle">Naslov koji koristi u Logovanju</param>
        /// <param name="selectionPattern">Uslovi selektovanja, npr> *.dll</param>
        /// <param name="copySubFolders">Ako je TRUE onda ce prekopirati i sve foldere kao i njihov sadrzaj koji odgovara upitu</param>
        public static void copyFiles(this String sourceFolder, String destinationFolder,
                                     String subjectTitle = "Copy folder content", String selectionPattern = "*.*",
                                     Boolean copySubFolders = false)
        {
            DirectoryInfo runTimeFolder = new DirectoryInfo(sourceFolder);

            if (copySubFolders)
            {
                DirectoryInfo[] directories = runTimeFolder.GetDirectories();

                foreach (DirectoryInfo includeDir in directories)
                {
                    DirectoryInfo destinationDirectory = new DirectoryInfo(destinationFolder);
                    destinationDirectory.CreateSubdirectory(includeDir.Name);
                    String targetPath = destinationFolder + includeDir.Name + "\\";

                    copyFiles(includeDir.FullName, targetPath, subjectTitle, selectionPattern, copySubFolders);
                }
            }

            FileInfo[] libraryFiles = runTimeFolder.GetFiles(selectionPattern);
            foreach (FileInfo includeFile in libraryFiles)
            {
                copyFile(includeFile.FullName, destinationFolder, subjectTitle);
            }
        }

        /// <summary>
        /// Deletes the files.
        /// </summary>
        /// <param name="targetFolder">The target folder.</param>
        /// <param name="selectionPattern">The selection pattern.</param>
        /// <param name="subFolders">if set to <c>true</c> [sub folders].</param>
        /// <param name="allowOutOfSandBoxTarget">if set to <c>true</c> [allow out of sand box target].</param>
        /// <exception cref="System.Exception">Sandbox Exception: can't delete files outside application folder [" + fileOpsBase.applicationFolder.FullName + "]. Requested delete path [" + targetFolder + "].</exception>
        public static void deleteFiles(this DirectoryInfo targetFolder, String selectionPattern = "*.*",
                                Boolean subFolders = false, Boolean allowOutOfSandBoxTarget = false)
        {
            if (!targetFolder.Exists) return;
            if (!targetFolder.FullName.StartsWith(fileOpsBase.applicationFolder.FullName) && !allowOutOfSandBoxTarget)
            {
                throw new NotSupportedException("Sandbox protection: can't delete files outside application folder [" + fileOpsBase.applicationFolder.FullName + "]. Requested delete path [" + targetFolder + "].");
                return;
            }

            if (subFolders)
            {
                var directories = targetFolder.GetDirectories();
                foreach (DirectoryInfo di in directories)
                {
                    di.deleteFiles(selectionPattern, subFolders, allowOutOfSandBoxTarget);
                }
            }

            FileInfo[] libraryFiles = targetFolder.GetFiles(selectionPattern);
            foreach (FileInfo deleteFile in libraryFiles)
            {
                deleteFile.Delete();
            }
        }

        /// <summary>
        /// Deletes the files matching the <c>selectionPattern</c>
        /// </summary>
        /// <param name="targetFolder">The target folder.</param>
        /// <param name="selectionPattern">The selection pattern.</param>
        /// <param name="subFolders">if set to <c>true</c> [sub folders].</param>
        /// <param name="allowOutOfSandBoxTarget">if set to <c>true</c> [allow out of sand box target].</param>
        /// <exception cref="System.Exception">Sandbox Exception: can't delete files outside application folder [" + fileOpsBase.applicationFolder.FullName + "]. Requested delete path [" + targetFolder + "].</exception>
        public static void deleteFiles(this String targetFolder, String selectionPattern = "*.*",
                                   Boolean subFolders = false, Boolean allowOutOfSandBoxTarget = false)
        {
            if (Path.IsPathRooted(targetFolder))
            {
                if (!targetFolder.StartsWith(fileOpsBase.applicationFolder.FullName) && !allowOutOfSandBoxTarget)
                {
                    throw new NotSupportedException("Sandbox Exception: can't delete files outside application folder [" + fileOpsBase.applicationFolder.FullName + "]. Requested delete path [" + targetFolder + "].");
                    return;
                }
            }

            DirectoryInfo runTimeFolder = new DirectoryInfo(targetFolder);
            runTimeFolder.deleteFiles(selectionPattern, subFolders, allowOutOfSandBoxTarget);
        }

        /// <summary>
        /// Deletes all inside target folder
        /// </summary>
        /// <param name="targetFolder">The source folder.</param>
        /// <param name="allowOutOfSandBoxTarget">if set to <c>true</c> [allow out of sand box target].</param>
        /// <exception cref="System.Exception">Sandbox Exception: can't delete files outside application folder [" + fileOpsBase.applicationFolder + "]. Requested delete path [" + targetFolder + "].</exception>
        public static void deleteAll(this String targetFolder, Boolean allowOutOfSandBoxTarget = false)
        {
            if (Path.IsPathRooted(targetFolder))
            {
                if (!targetFolder.StartsWith(fileOpsBase.applicationFolder.FullName) && !allowOutOfSandBoxTarget)
                {
                    throw new NotSupportedException("Sandbox Exception: can't delete files outside application folder [" + fileOpsBase.applicationFolder + "]. Requested delete path [" + targetFolder + "].");
                    return;
                }
            }

            DirectoryInfo runTimeFolder = new DirectoryInfo(targetFolder);
            runTimeFolder.deleteFiles("*.*", true, allowOutOfSandBoxTarget);

            if (runTimeFolder.Exists)
            {
                foreach (var di in runTimeFolder.GetDirectories())
                {
                    if (!di.hasFiles())
                    {
                        di.Delete(true);
                    }
                    else
                    {
                        throw new NotImplementedException("This shouldn't happen: directory still has files");
                        //aceLog.loger.AppendLine("Error: can't delete folder [" + di.FullName + "] - it has files inside");
                    }
                }
            }
        }

        /// <summary>
        /// Proverava fajl - vraca TRUE ako ispunjava uslov iz fileCheckType
        /// </summary>
        /// <param name="path">Putanja na kome se fajl mozda nalazi</param>
        /// <param name="checkType">tip provere</param>
        /// <returns></returns>
        public static Boolean fileCheck(this String path, fileCheckType checkType)
        {
            Boolean output = false;
            FileStream tmp;
            try
            {
                switch (checkType)
                {
                    case fileCheckType.exists:
                        output = File.Exists(path);
                        break;

                    case fileCheckType.read:
                        tmp = File.OpenRead(path);
                        output = true;
                        tmp.Close();
                        break;

                    case fileCheckType.write:
                        tmp = File.OpenWrite(path);
                        output = true;
                        tmp.Close();
                        break;
                }
            }
            catch //(Exception ex)
            {
                output = false;
            }
            return output;
        }

        internal static String lastFilePath = "";

        public static String getLastFilePath()
        {
            return lastFilePath;
        }
    }
}