// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyLoaderTool.cs" company="imbVeles" >
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Core.extensions.typeworks
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.reporting;
    using imbSCI.Data;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Utility object that performs Assembly loading, resolves referenced assemblies and containes the result
    /// </summary>
    public class AssemblyLoaderTool
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyLoaderTool"/> class.
        /// </summary>
        public AssemblyLoaderTool()
        {
        }

        /// <summary>
        /// Static utility method - internally creates <see cref="AssemblyLoaderTool"/> and loads specified assemly file
        /// </summary>
        /// <param name="dllFile">The DLL file.</param>
        /// <param name="_log">The log.</param>
        /// <param name="loadReferencedAssemblies">if set to <c>true</c> [load referenced assemblies].</param>
        /// <param name="loadReflectionOnly">if set to <c>true</c> [load reflection only].</param>
        /// <returns></returns>
        public static Assembly LoadAssembly(String dllFile, ILogBuilder _log, Boolean loadReferencedAssemblies = false, Boolean loadReflectionOnly = true)
        {
            AssemblyLoaderTool output = new AssemblyLoaderTool(_log, loadReferencedAssemblies, loadReflectionOnly);

            output.LoadAssembly(dllFile);

            return output.assembly;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyLoaderTool"/> class and deploys general settings.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="loadReferencedAssemblies">if set to <c>true</c> [load referenced assemblies].</param>
        /// <param name="loadReflectionOnly">if set to <c>true</c> [load reflection only].</param>
        public AssemblyLoaderTool(ILogBuilder _log, Boolean loadReferencedAssemblies, Boolean loadReflectionOnly)
        {
            log = _log;
            LoadReferencedAssemblies = loadReferencedAssemblies;
            LoadReflectionOnly = loadReflectionOnly;
        }

        /// <summary>
        /// Creates new instance of <see cref="AssemblyLoaderTool"/> and copies general settings into new instance: <see cref="LoadReflectionOnly"/>, <see cref="LoadReferencedAssemblies"/>, <see cref="log"/>
        /// </summary>
        /// <returns></returns>
        public AssemblyLoaderTool GetSubLoader()
        {
            AssemblyLoaderTool output = new AssemblyLoaderTool();
            output.LoadReferencedAssemblies = LoadReferencedAssemblies;
            output.LoadReflectionOnly = LoadReflectionOnly;
            output.log = log;
            return output;
        }

        /// <summary>
        /// Gets or sets the log.
        /// </summary>
        /// <value>
        /// The log.
        /// </value>
        public ILogBuilder log { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [load referenced assemblies].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [load referenced assemblies]; otherwise, <c>false</c>.
        /// </value>
        public Boolean LoadReferencedAssemblies { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether [load reflection only].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [load reflection only]; otherwise, <c>false</c>.
        /// </value>
        public Boolean LoadReflectionOnly { get; set; } = true;

        /// <summary>
        /// Gets or sets the assembly.
        /// </summary>
        /// <value>
        /// The assembly.
        /// </value>
        public Assembly assembly { get; set; }

        /// <summary>
        /// Gets or sets the directory.
        /// </summary>
        /// <value>
        /// The directory.
        /// </value>
        public DirectoryInfo directory { get; set; }

        /// <summary>
        /// Reference to the file
        /// </summary>
        /// <value>
        /// The file information.
        /// </value>
        public FileInfo fileInfo { get; set; }

        /// <summary>
        /// Loads the assembly.
        /// </summary>
        /// <param name="dllFile">The DLL file.</param>
        /// <exception cref="ArgumentNullException">dllFile - Path can not be null nor empty</exception>
        /// <exception cref="ArgumentOutOfRangeException">File not found at specified path [" + dllFile + "] - dllFile</exception>
        public void LoadAssembly(String dllFile)
        {
            if (dllFile.isNullOrEmpty()) { throw new ArgumentNullException(nameof(dllFile), "Path can not be null nor empty"); }

            if (!File.Exists(dllFile)) { throw new ArgumentOutOfRangeException("File not found at specified path [" + dllFile + "]", nameof(dllFile)); }

            fileInfo = new FileInfo(dllFile);
            directory = fileInfo.Directory;

            if (LoadReflectionOnly)
            {
                AppDomain domain = AppDomain.CurrentDomain;

                domain.ReflectionOnlyAssemblyResolve += Domain_ReflectionOnlyAssemblyResolve;

                assembly = Assembly.ReflectionOnlyLoadFrom(fileInfo.FullName);

                domain.ReflectionOnlyAssemblyResolve -= Domain_ReflectionOnlyAssemblyResolve;
            }
            else
            {
                assembly = Assembly.LoadFile(fileInfo.FullName);
            }

            if (assembly != null)
            {
                if (log != null)
                {
                    log.log("Assembly [" + assembly.FullName + "] loaded from (" + fileInfo.FullName + ")");
                }
            }
        }

        /// <summary>
        /// Gets or sets the related assemblies.
        /// </summary>
        /// <value>
        /// The related assemblies.
        /// </value>
        public List<AssemblyLoaderTool> RelatedAssemblies { get; set; } = new List<AssemblyLoaderTool>();

        /// <summary>
        /// Handles the ReflectionOnlyAssemblyResolve event of the Domain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="ResolveEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        private Assembly Domain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            List<FileInfo> found = new List<FileInfo>();

            Assembly output = null;

            if (!LoadReferencedAssemblies) return null;

            String searchPattern = args.Name + "*.dll";

            found.AddRange(directory.GetFiles(searchPattern, SearchOption.TopDirectoryOnly).ToList());
            if (!found.Any())
            {
                found.AddRange(directory.GetFiles(searchPattern, SearchOption.AllDirectories).ToList());
            }

            if (!found.Any())
            {
                if (log != null) log.log("Assembly file search [" + searchPattern + "] failed (" + directory.FullName + ")");
            }
            else
            {
                var subTool = GetSubLoader();
                RelatedAssemblies.Add(subTool);

                foreach (var f in found)
                {
                    subTool.LoadAssembly(f.FullName);
                    break;
                }

                output = subTool.assembly;
            }

            return output;
        }
    }
}