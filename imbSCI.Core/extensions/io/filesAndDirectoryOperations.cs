// --------------------------------------------------------------------------------------------------------------------
// <copyright file="filesAndDirectoryOperations.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.data;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;

    public static class filesAndDirectoryOperations
    {
        /// <summary>
        /// Determines whether there is any file in the directory
        /// </summary>
        /// <param name="di">The di.</param>
        /// <returns>
        ///   <c>true</c> if the specified di has files; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean hasFiles(this DirectoryInfo di)
        {
            foreach (FileInfo fi in di.EnumerateFiles())
            {
                return true;
            }

            foreach (var fi in di.EnumerateDirectories())
            {
                return true;
            }
            return false;
        }

        public static String getWebPathBackslashFormat(this String start)
        {
            String output = start.Replace("\\", "/");
            output = output.Replace("//", "/");
            return output;
        }

        /// <summary>
        /// Gets the relative path from <c>start</c> to <c>parent</c>
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Iteration limit triggered c=[" + c.ToString() + "] for start [" + start.FullName + "] is not subdir of parent [" + parent.FullName + "]</exception>
        /// <exception cref="ArgumentOutOfRangeException">parent - start [" + start.FullName + "] is not subdir of parent [" + parent.FullName + "]</exception>
        public static String getRelativePathToParent(this DirectoryInfo start, DirectoryInfo parent)
        {
            String output = "";

            if (start.FullName.Contains(parent.FullName))
            {
                Int32 c = 0;
                DirectoryInfo head = start;

                String difPath = imbSciStringExtensions.removeStartsWith(start.FullName, parent.FullName);
                var elements = difPath.Split("\\".ToArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (String el in elements)
                {
                    output = output.add("..", "\\");
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("parent", "start [" + start.FullName + "] is not subdir of parent [" + parent.FullName + "]");
                //output = "(error)";
            }
            output = imbSciStringExtensions.ensureEndsWith(output, "\\");
            return output;
        }

        /// <summary>
        /// Does the directory operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="targetPath">The target path - relative or not</param>
        /// <param name="from">From.</param>
        /// <param name="returnFrom">if set to <c>true</c> [return from].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static DirectoryInfo doDirectoryOperation(this directoryOperation operation, String targetPath, DirectoryInfo from = null, Boolean returnFrom = false)
        {
            if (from == null) from = new DirectoryInfo(Directory.GetCurrentDirectory());
            String fromPath = from.FullName; //.add
            String targetDirPath = "";

            DirectoryInfo di; // targetPath = Path.Combine(fromPath, targetPath);
            if (!Path.IsPathRooted(targetPath))
            {
                targetPath = Path.Combine(fromPath, targetPath);
            }

            targetDirPath = Path.GetDirectoryName(targetPath);
            DirectoryInfo targetDir = null;

            String flName = Path.GetFileName(targetDirPath);
            if (flName.isNullOrEmpty()) flName = from.Name;
            //targetDir = new DirectoryInfo(targetDirPath);

            switch (operation)
            {
                case directoryOperation.none:
                    return from;
                    break;

                case directoryOperation.unknown:
                    return from;
                    break;

                case directoryOperation.selectOrBuild:
                    targetPath = Path.Combine(fromPath, targetPath);
                    targetDir = Directory.CreateDirectory(targetPath);
                    returnFrom = true;
                    break;

                case directoryOperation.compress:
                    targetDir = new DirectoryInfo(targetDirPath);
                    var zip = from.zipDirectory(targetDir, flName, false);

                    break;

                case directoryOperation.delete:
                    if (Directory.Exists(targetDirPath))
                    {
                        targetDir = new DirectoryInfo(targetDirPath);
                        targetDir.Delete(true);
                    }

                    returnFrom = true;
                    break;

                case directoryOperation.selectIfExists:
                    if (Directory.Exists(targetDirPath))
                    {
                        targetDir = new DirectoryInfo(targetDirPath);
                    }
                    else
                    {
                        returnFrom = true;
                    }
                    break;

                case directoryOperation.selectParent:
                    return from.Parent;
                    break;

                case directoryOperation.selectRoot:
                    return from.Root;
                    break;

                case directoryOperation.selectRuntimeRoot:
                    return new DirectoryInfo(Directory.GetCurrentDirectory());
                    break;

                case directoryOperation.copy:
                    targetDir = new DirectoryInfo(targetDirPath);
                    from.copyDirectory(targetDir);
                    break;

                case directoryOperation.rename:
                    targetDir = new DirectoryInfo(targetDirPath);
                    from.copyDirectory(targetDir);
                    from.Delete(true);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (returnFrom)
            {
                return from;
            }
            else
            {
                //if (targetDir == null) targetDir = new DirectoryInfo(targetDirPath);
                return targetDir;
            }
        }

        /// <summary>
        /// Gets the size of all files within the <c>directory</c>, including subfolders. Optionally applies <c>searchFilter</c> for results
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="searchFilter">The search filter.</param>
        /// <returns></returns>
        public static long getDirectorySize(this string directory, String searchFilter = "*.*")
        {
            return new DirectoryInfo(directory).GetFiles(searchFilter, SearchOption.AllDirectories).Sum(file => file.Length);
        }

        /// <summary>
        /// Feeds directory data info <see cref="PropertyCollection"/>
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static PropertyCollection getDirectoryInfo(this DirectoryInfo directory, PropertyCollection output = null)
        {
            if (output == null)
            {
                output = new PropertyCollection();
            }

            output["dir_files"] = 0;
            output["dir_subdirs"] = 0;
            output["dir_size"] = 0;

            if (directory == null)
            {
                return output;
            }

            long files = 0;
            long subs = 0;
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = directory.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
                files++;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = directory.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += getDirectorySize(di);
                subs++;
            }

            output["dir_files"] = files;
            output["dir_subdirs"] = subs;
            output["dir_size"] = size;

            return output;
        }

        /// <summary>
        /// Returns byte count of the directory, by enumerating all files within
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static long getDirectorySize(this DirectoryInfo directory)
        {
            if (directory == null) return 0;

            long size = 0;
            // Add file sizes.
            FileInfo[] fis = directory.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = directory.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += getDirectorySize(di);
            }
            return size;
        }
    }
}