// --------------------------------------------------------------------------------------------------------------------
// <copyright file="folderNode.cs" company="imbVeles" >
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
using imbSCI.Core;
using imbSCI.Core.attributes;
using imbSCI.Core.collection;
using imbSCI.Core.data;
using imbSCI.Core.enums;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.interfaces;
using imbSCI.Core.reporting;
using imbSCI.Data;
using imbSCI.Data.data;
using imbSCI.Data.enums;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Core.files.folders
{
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Data.collection.graph;
    using System.Collections;
    using System.Data;
    using System.IO;
    using System.Text.RegularExpressions;

    //[Flags]
    //public enum folderNodeQueryResponse
    //{
    //    none=0,

    //}

    /// <summary>
    /// Subfolder in the <see cref="folderStructure"/>
    /// </summary>
    /// <seealso cref="imbBindable" />
    /// <seealso cref="imbACE.Core.files.folders.IFolderNode" />
    /// <seealso cref="System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{System.String, imbACE.Core.files.folders.folderNode}}" />
    public class folderNode : imbBindable, IFolderNode, IEnumerable<KeyValuePair<string, folderNode>>, IEnumerable<folderNode>
    {

        public static folderStructure GetFolderNodeForPath(String path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            folderStructure node = new folderStructure(di.FullName, di.Name.imbTitleCamelOperation(true), "Folder node generated from DirectoryInfo object: " + di.FullName);

            return node;
        }

        /// <summary>
        /// Returns index with found xml files
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        public SerializedXMLFileDictionary ScanForResources(String searchString = "*.xml")
        {
            return SerializedXMLFileDictionary.ScanFolder(this, searchString);
        }

        /// <summary>
        /// Refers to the current directory of application
        /// </summary>
        public folderNode()
        {
            DirectoryInfo info = new DirectoryInfo(Directory.GetCurrentDirectory());
            folderNode par = info.Parent;

            parent = par;

            _name = info.Name;
            caption = _name.imbTitleCamelOperation(true);
            /*
            if (_description.isNullOrEmpty())
            {
                description = "Application root directory";
            }
            else
            {
                description = _description;
            }*/

            par.Add(this);
        }

        private void parentAutoSet()
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(Directory.GetCurrentDirectory());
                folderNode par = info.Parent;

                parent = par;

                _name = info.Name;
                caption = _name.imbTitleCamelOperation(true);
                if (_description.isNullOrEmpty())
                {
                    description = "Application root directory";
                }
                else
                {
                    description = _description;
                }

                par.Add(this);
            }
            catch (Exception ex)
            {
                throw new imbFileException("folderNode construction failed - parentAutoSet()", ex, this, null, null);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="folderNode"/> class.
        /// </summary>
        /// <param name="__name">The name.</param>
        /// <param name="__caption">The caption.</param>
        /// <param name="__description">The description.</param>
        public folderNode(string __name, string __caption, string __description)
        {
            _name = __name;
            caption = __caption;
            description = __description;

            //DirectoryInfo info = new DirectoryInfo(Directory.GetCurrentDirectory());
            //folderNode par = info.Parent;
            //par.Add(this);
        }

        /// <summary>
        /// Creates new folder node as subdirectory
        /// </summary>
        /// <param name="nameEnum">The name enum.</param>
        /// <param name="__caption">The caption.</param>
        /// <param name="__description">The description.</param>
        /// <returns></returns>
        public folderNode Add(Enum nameEnum, string __caption, string __description)
        {
            return Add(nameEnum.toString(), __caption, __description);
        }

        /// <summary>
        /// Nests the specified folder node into this instance
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public folderNode Add(folderNode node)
        {
            node.parent = this;
            if (items.ContainsKey(node.name))
            {
                items[node.name] = node;
            }
            else
            {
                items.Add(node.name, node);
            }

            return node;
        }

        public string forTreeview
        {
            get
            {
                return name;
            }
        }

        public folderNode Add(folderStructure subStructure)
        {
            folderNode node = new folderNode(subStructure.name, subStructure.caption, subStructure.description);
            return Add(node);
        }

        public static implicit operator DirectoryInfo(folderNode node)
        {
            return new DirectoryInfo(node.path);
        }

        public static implicit operator folderNode(DirectoryInfo di)
        {
            folderStructure node = new folderStructure(di.FullName, di.Name.imbTitleCamelOperation(true), "Folder node generated from DirectoryInfo object: " + di.FullName);

            return node;
        }

        public const string CAPTION_FOR_TUNNELFOLDER = "--";

        private object addFolderLock = new object();

        /// <summary>
        /// Adds new node or nodes to correspond to specified path or name. <c>pathOrName</c> can be path like: path1\\path2\\path3
        /// </summary>
        /// <remarks>
        /// If directory under specified path already exists, it will update its <see cref="caption"/> and <see cref="description"/> if these are empty, and return the existing node.
        /// </remarks>
        /// <param name="pathOrName">Name of the path or.</param>
        /// <param name="__caption">The caption - display name of the folder</param>
        /// <param name="__description">The description - description about the folder</param>
        /// <returns>Newly created or existing directory node</returns>
        public folderNode Add(string pathOrName, string __caption, string __description)
        {
            List<string> pathParts = imbSciStringExtensions.SplitSmart(pathOrName, System.IO.Path.DirectorySeparatorChar.ToString());
            folderNode head = this;
            if (pathParts.Count() > 1)
            {
                foreach (string part in pathParts)
                {
                    head = head.Add(part, CAPTION_FOR_TUNNELFOLDER, CAPTION_FOR_TUNNELFOLDER);
                }
                return head;
            }
            else
            {
                lock (addFolderLock)
                {
                    if (!items.ContainsKey(pathOrName))
                    {
                        folderNode node = new folderNode(pathOrName, __caption, __description);
                        node.parent = this;
                        items.Add(pathOrName, node);
                        return node;
                    }
                    else
                    {
                        folderNode node = items[pathOrName];
                        if (node.caption.isNullOrEmpty() || node.caption == CAPTION_FOR_TUNNELFOLDER) node.caption = __caption;
                        if (node.description.isNullOrEmpty() || node.description == CAPTION_FOR_TUNNELFOLDER) node.description = __description;
                        return node;
                    }
                }
            }
        }

        /// <summary>
        /// Scans the File System for unregistered sub directories at this folder, and returns newly registered ones.
        /// </summary>
        /// <param name="searchPattern">The search pattern - to match subdires names.</param>
        /// <param name="forceScan">if set to <c>true</c> it will force scan of readme file, that will pick up folder description and registered files.</param>
        /// <param name="attachSubdirectories">if set to <c>true</c> it will run the procedure recursevly over complete directory tree.</param>
        /// <param name="logger">The logger - to send debug information.</param>
        /// <returns>List of all previously unregistered subfolders</returns>
        public List<folderNode> AttachSubfolders(String searchPattern = "*", Boolean forceScan = true, Boolean attachSubdirectories = true, ILogBuilder logger = null)
        {
            DirectoryInfo di = this;
            var foundDirectories = di.GetDirectories(searchPattern, SearchOption.TopDirectoryOnly);

            List<folderNode> output = new List<folderNode>();

            foreach (DirectoryInfo d in foundDirectories)
            {
                if (!Contains(d.Name))
                {
                    output.Add(Attach(d, "", "", forceScan, false, logger));
                }
            }

            if (attachSubdirectories)
            {
                List<folderNode> toscan = output.ToList();

                foreach (folderNode t in toscan)
                {
                    output.AddRange(t.AttachSubfolders(searchPattern, forceScan, attachSubdirectories, logger));
                }
            }

            return output;
        }

        /// <summary>
        /// Attaches sub directory, sets <see cref="caption"/> and <see cref="description"/>. If these are not specified, it will scan the directory for readme file. <see cref="ScanReadMe(ILogBuilder)"/>
        /// </summary>
        /// <param name="directory">The subdirectory name.</param>
        /// <param name="__caption">The caption.</param>
        /// <param name="__description">The description.</param>
        /// <param name="forceScan">if set to <c>true</c> it will force readme file scan <see cref="ScanReadMe(ILogBuilder)"/>.</param>
        /// <param name="autoCreate">if set to <c>true</c> it will create the directory, if not existing already.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>Reference on newly attached directory, or null if it wasn't found nor created</returns>
        public folderNode Attach(String directory, String __caption = "", String __description = "", Boolean forceScan = false, Boolean autoCreate = true, ILogBuilder logger = null)
        {
            var di = new DirectoryInfo(path + Path.DirectorySeparatorChar + directory);
            return Attach(di, __caption, __description, forceScan, autoCreate, logger);
        }

        /// <summary>
        /// Attaches sub directory, sets <see cref="caption"/> and <see cref="description"/>. If these are not specified, it will scan the directory for readme file. <see cref="ScanReadMe(ILogBuilder)"/>
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="__caption">The caption.</param>
        /// <param name="__description">The description.</param>
        /// <param name="forceScan">if set to <c>true</c> it will force readme file scan <see cref="ScanReadMe(ILogBuilder)"/>.</param>
        /// <param name="autoCreate">if set to <c>true</c> it will create the directory, if not existing already.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>Reference on newly attached directory, or null if it wasn't found nor created</returns>
        public folderNode Attach(DirectoryInfo directory, String __caption = "", String __description = "", Boolean forceScan = false, Boolean autoCreate = true, ILogBuilder logger = null)
        {
            if (!directory.Exists)
            {
                if (autoCreate)
                {
                    directory.Create();
                }
                else
                {
                    if (logger != null) logger.log("Directory [" + directory.FullName + "] does not exist.");
                    return null;
                }
            }
            if (__caption.isNullOrEmpty())
            {
                __caption = directory.Name.imbTitleCamelOperation(true);
            }
            Boolean callForScan = forceScan;
            if (__description.isNullOrEmpty())
            {
                callForScan = true;
            }

            folderNode output = Add(directory.Name, __caption, __description);

            if (callForScan) output.ScanReadMe(logger);
            return output;
        }

        //public void AttachSubdirs(ILogBuilder logger)
        //{
        //    Directory.GetDirectories(
        //}

        /// <summary>
        /// Searches for readme file (<see cref="directory_readme_filename"/>) in this folder and extracts: <see cref="caption"/>, <see cref="description"/> and list of files
        /// </summary>
        /// <param name="logger">The logger.</param>
        public void ScanReadMe(ILogBuilder logger = null)
        {
            var p = path + Path.DirectorySeparatorChar + directory_readme_filename;
            if (!File.Exists(p))
            {
                if (logger != null) logger.log("Readme file not found at [" + p + "]");
                return;
            }

            var lines = File.ReadAllLines(p);
            String line = lines.FirstOrDefault(x => x.StartsWith("####"));
            if (line.isNullOrEmpty())
            {
                if (logger != null) logger.log("Readme file [" + p + "] format not recognized");
                return;
            }

            caption = line.removeStartsWith("#### ");

            var descriptionLines = lines.Where(x => x.StartsWith(" > ")).ToList();

            if (descriptionLines.Count > 1)
            {
                description = descriptionLines[1];
                description = description.removeStartsWith(" > ");
            }

            Int32 lc = 0;
            for (int i = 10; i < lines.Length; i++)
            {
                String ln = lines[i];
                if (ln.isNullOrEmpty())
                {
                }
                else if (ln.Contains("-----------------------"))
                {
                    lc++;
                    if (lc > 1) break;
                }
                else
                {
                    folderNodeFileDescriptor fInfo = folderNodeFileDescriptorTools.GetFileDescription(ln);
                    if (fInfo != null)
                    {
                        RegisterFile(fInfo.filename, fInfo.description);
                        //   AdditionalFileEntries.Add(fInfo.filename, this.GetFileDescription(fInfo.filename, fInfo.description));
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<string, folderNode> items { get; set; } = new Dictionary<string, folderNode>();

        /// <summary>
        /// Determines whether it has a subdirectory under specified key. Important: it only accounts for already registered filesystem entries. Check: <see cref="Attach(DirectoryInfo, string, string, bool, bool, ILogBuilder)"/>
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string key) => items.ContainsKey(key);

        /// <summary>
        /// description/comment - objasnjenje
        /// </summary>
        private string _description = "";

        /// <summary>
        /// description/comment - objasnjenje
        /// </summary>
        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        /// <summary>
        /// Display title
        /// </summary>
        public string caption { get; set; }

        /// <summary>
        /// file system ime direktorijuma
        /// </summary>
        private string _name = "";

        /// <summary>
        /// file system ime direktorijuma
        /// </summary>
        public virtual string name
        {
            get
            {
                return _name;
            }
            set { }
        }

        public virtual string path
        {
            get
            {
                if (name.isNullOrEmpty())
                {
                    throw new InvalidOperationException("Name not set for folderNode - Can't create path when name not set");
                }

                if (parent != null)
                {
                    return parent.path.add(name, Path.DirectorySeparatorChar);
                }
                else
                {
                    return name;
                }
            }
        }

        public const string NOTFOUND_RETURN = "--FILE NOT FOUND--";

        /// <summary>
        /// Finds the file - and returns relative path
        /// </summary>
        /// <param name="found">if set to <c>true</c> [found].</param>
        /// <param name="partOrPattern">The part or pattern.</param>
        /// <param name="options">The options.</param>
        /// <param name="returnRelative">if set to <c>true</c> [return relative].</param>
        /// <returns></returns>
        public string findFile(out bool found, string partOrPattern, SearchOption options = SearchOption.TopDirectoryOnly, bool returnRelative = false)
        {
            DirectoryInfo di = Directory.CreateDirectory(path);
            var files = di.GetFiles(partOrPattern, options);
            if (files.Any())
            {
                found = true;
                if (returnRelative)
                {
                    return imbSciStringExtensions.removeStartsWith(files.First().FullName, path).TrimStart('\\');
                }
                else
                {
                    return files.First().FullName;
                }
            }
            found = false;
            return "";
        }

        /// <summary>
        /// Finds the file - and returns relative path. If not found returns emptry string
        /// </summary>
        /// <param name="partOrPattern">The part or pattern.</param>
        /// <param name="options">The options.</param>
        /// <param name="returnRelative">if set to <c>true</c> [return relative].</param>
        /// <returns></returns>
        public string findFile(string partOrPattern, SearchOption options = SearchOption.TopDirectoryOnly, bool returnRelative = false)
        {
            bool found = true;
            string output = findFile(out found, partOrPattern, options, returnRelative);
            if (found) return output;
            return "";
        }

        /// <summary>
        /// Finds the files - returns relative paths
        /// </summary>
        /// <param name="partOrPattern">The part or pattern.</param>
        /// <param name="options">The options.</param>
        /// <param name="returnRelative">if set to <c>true</c> [return relative].</param>
        /// <returns></returns>
        public List<string> findFiles(string partOrPattern = "*.*", SearchOption options = SearchOption.TopDirectoryOnly, bool returnRelative = false)
        {
            DirectoryInfo di = Directory.CreateDirectory(path);
            var __files = di.GetFiles(partOrPattern, options);
            List<string> output = new List<string>();

            foreach (FileInfo fi in __files)
            {
                if (returnRelative)
                {
                    output.Add(imbSciStringExtensions.removeStartsWith(fi.FullName, path).TrimStart('\\'));
                }
                else
                {
                    output.Add(fi.FullName);
                }
            }

            return output;
        }

        /// <summary>
        /// Tries to locate files specified in inputNames if no files specified, than applies search pattern
        /// </summary>
        /// <param name="inputNames">Comma separated list of filenames, leave empty if search pattern should be used</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <returns></returns>
        public List<String> GetOrFindFiles(String inputNames, String searchPattern = "", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            List<string> filepaths = new List<string>();

            List<String> filenames = new List<String>();

            if (!inputNames.isNullOrEmpty())
            {
                filenames = inputNames.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            if (!filenames.Any())
            {
                filenames.Add(searchPattern);
            }

            var fnm = findFiles(filenames, searchOption);

            filepaths.AddRange(fnm);

            return filepaths;
        }

        /// <summary>
        /// Collects all files for all patterns
        /// </summary>
        /// <param name="partOrPatterns">The part or patterns.</param>
        /// <param name="options">The options.</param>
        /// <param name="returnRelative">if set to <c>true</c> [return relative].</param>
        /// <returns></returns>
        public List<string> findFiles(IEnumerable<string> partOrPatterns, SearchOption options = SearchOption.TopDirectoryOnly)
        {
            List<string> output = new List<string>();
            if (partOrPatterns == null) partOrPatterns = new List<String>();

            if (!partOrPatterns.Any())
            {
                var pl = new List<String>();
                pl.Add("*.*");
                partOrPatterns = pl;
            }

            foreach (String pattern in partOrPatterns)
            {
                output.AddRange(findFiles(pattern, options), true);
            }

            return output;
        }

        /// <summary>
        /// Detects subtree matches in the filesystem
        /// </summary>
        /// <param name="filePatterns">The file patterns.</param>
        /// <param name="min">The minimum.</param>
        /// <returns></returns>
        public graphNodeSetCollection findNodeTreeMatch(List<String> filePatterns, Int32 min = -1)
        {
            List<String> paths = new List<string>();

            paths = findFiles(filePatterns, SearchOption.AllDirectories);

            graphNode output = paths.BuildGraphFromPaths<graphNode>();

            List<IObjectWithPathAndChildren> leafList = output.getAllLeafs();

            List<graphNode> leafNodes = new List<graphNode>();
            foreach (var leaf in leafList)
            {
                if (leaf != null)
                {
                    leafNodes.Add((graphNode)leaf);
                }
            }

            return leafNodes.GetFirstNodeWithLeafs<graphNode>(filePatterns, min);
        }

        /// <summary>
        /// Finds the directories - returns relative paths
        /// </summary>
        /// <param name="partOrPattern">The part or pattern.</param>
        /// <param name="options">The options.</param>
        /// <param name="returnRelative">if set to <c>true</c> [return relative].</param>
        /// <returns></returns>
        public List<string> findDirectories(string partOrPattern = "*", SearchOption options = SearchOption.TopDirectoryOnly, bool returnRelative = false)
        {
            DirectoryInfo di = Directory.CreateDirectory(path);
            var files = di.GetDirectories(partOrPattern, options);
            List<string> output = new List<string>();
            foreach (DirectoryInfo fi in files)
            {
                if (returnRelative)
                {
                    output.Add(imbSciStringExtensions.removeStartsWith(fi.FullName, path).TrimStart('\\'));
                }
                else
                {
                    output.Add(files.First().FullName);
                }
            }

            return output;
        }

        /// <summary>
        /// Creates the directory, child node and finds unique name if required
        /// </summary>
        /// <param name="nameOrPath">The proposal.</param>
        /// <param name="desc">The desc.</param>
        /// <param name="findUniqueName">if set to <c>true</c> [find unique name].</param>
        /// <returns></returns>
        public folderNode createDirectory(string nameOrPath, string desc = "", bool findUniqueName = true)
        {
            string new_name = nameOrPath;

            if (findUniqueName) new_name = findUniqueDirectoryName(nameOrPath);

            folderNode new_node = Add(new_name, nameOrPath, desc);

            DirectoryInfo di = Directory.CreateDirectory(new_node.path);
            return new_node;
        }

        //public FileInfo findUniqueFileName(String proposal)
        //{
        //    String p = pathFor(proposal);
        //    return p.getWritableFile(enums.getWritableFileMode.autoRenameThis);
        //}

        /// <summary>
        /// Finds a unique directory name, for new directory to be created
        /// </summary>
        /// <param name="proposal">The proposal.</param>
        /// <returns></returns>
        public string findUniqueDirectoryName(string proposal)
        {
            string pt = imbSciStringExtensions.add(path, proposal, "\\");
            int c = 0;
            string output = proposal;
            while (Directory.Exists(pt))
            {
                c++;
                if (c > 1000) break;
                output = proposal + c.ToString("D5");
                pt = imbSciStringExtensions.add(path, output, "\\");
            }
            //DirectoryInfo di = Directory.CreateDirectory(path);
            //String[] dirs = Directory.GetDirectories(path);
            //proposal = proposal.makeUniqueName(dirs.Contains);
            return output;
        }

        /// <summary>
        /// Returns properly compiled path, without file registration. This is alternative to <see cref="pathFor(string, getWritableFileMode, string, bool)"/>
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>Just makes path</returns>
        public string pathMake(String filename)
        {
            filename = filename.getCleanFilepath("");
            //filename = filename.getCleanFileName(false);
            if (Path.IsPathRooted(filename))
            {
                filename = Path.GetFileName(filename);
            }
            else if (filename.StartsWith(path))
            {
                filename = imbSciStringExtensions.removeStartsWith(filename, path);
            }

            string output = imbSciStringExtensions.add(path, filename, Path.DirectorySeparatorChar);
            output = output.Replace("\\\\", Path.DirectorySeparatorChar.ToString());
            output = output.Replace("\\\\", Path.DirectorySeparatorChar.ToString());

            output = output.getWritableFile(getWritableFileMode.none).FullName;
            return output;
        }

        /// <summary>
        /// Saves the <c>content</c> string as text file. Returns path with filename specified. Optionally, sets <c>fileDescription</c> for directory readme generator
        /// </summary>
        /// <remarks>
        /// This method calls: <see cref="pathFor(string, getWritableFileMode, string, bool)"/> and then uses <see cref="File.WriteAllText(string, string)"/> to save the file.
        /// </remarks>
        /// <param name="content">The textual content to be saved</param>
        /// <param name="filename">The filename, if has no extension it will set .txt</param>
        /// <param name="mode">The mode.</param>
        /// <param name="fileDescription">The file description - if not specified, it will try to improvize :)</param>
        /// <param name="updateExistingDesc">if set to <c>true</c> it will force update if any existing file description was found. <see cref="RegisterFile(string, string, bool)" /></param>
        /// <returns>
        /// The path
        /// </returns>
        public String SaveText(String content, String filename, getWritableFileMode mode = getWritableFileMode.none, String fileDescription = "", Boolean updateExistingDesc = false)
        {
            if (!Path.HasExtension(filename))
            {
                filename = filename + ".txt";
            }

            String p = pathFor(filename, mode, fileDescription, updateExistingDesc);
            File.WriteAllText(p, content);

            return p;
        }

        /// <summary>
        /// Returns path with filename specified. Optionally, sets <c>fileDescription</c> for directory readme generator
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="fileDescription">The file description - if not specified, it will try to improvize :)</param>
        /// <param name="updateExisting">if set to <c>true</c> it will force update if any existing file description was found. <see cref="RegisterFile(string, string, bool)"/></param>
        /// <returns>The path</returns>
        public string pathFor(string filename, getWritableFileMode mode = getWritableFileMode.none, String fileDescription = "", Boolean updateExisting = false)
        {
            filename = filename.getCleanFilepath("");
            //filename = filename.getCleanFileName(false);
            if (Path.IsPathRooted(filename))
            {
                filename = Path.GetFileName(filename);
            }
            else if (filename.StartsWith(path))
            {
                filename = imbSciStringExtensions.removeStartsWith(filename, path);
            }

            string output = imbSciStringExtensions.add(path, filename, Path.DirectorySeparatorChar);
            output = output.Replace("\\\\", Path.DirectorySeparatorChar.ToString());
            output = output.Replace("\\\\", Path.DirectorySeparatorChar.ToString());

            if (mode != getWritableFileMode.none)
            {
                output = output.getWritableFile(mode).FullName;
            }

            RegisterFile(filename, fileDescription, updateExisting);

            return output;
        }

        /// <summary>
        /// Registers the file, with description provided. Later, this description is used for <see cref="generateReadmeFiles(aceAuthorNotation, string, int)"/> />
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="fileDescription">The file description.</param>
        /// <param name="updateExisting">if set to <c>true</c> it will force update if any existing file description was found.</param>
        public void RegisterFile(String filename, String fileDescription, Boolean updateExisting = false)
        {
            lock (addFileDescriptionLock)
            {
                if (updateExisting)
                {
                    if (AdditionalFileEntries.ContainsKey(filename)) AdditionalFileEntries.Remove(filename);
                }
                if (!AdditionalFileEntries.ContainsKey(filename))
                {
                    AdditionalFileEntries.Add(filename, this.GetFileDescription(filename, fileDescription));
                }
                else
                {
                    if (!fileDescription.isNullOrEmpty()) AdditionalFileEntries[filename] = this.GetFileDescription(filename, fileDescription);
                }
            }
        }

        private Object addFileDescriptionLock = new Object();

        /// <summary>
        /// Deletes all files, matching the <c>selectionPattern</c> in the folder and all sub folders if <c>subfolders</c> is <c>true</c>
        /// </summary>
        /// <param name="selectionPattern">The selection pattern.</param>
        /// <param name="subFolders">if set to <c>true</c> [sub folders].</param>
        public void deleteFiles(string selectionPattern = "*.*", bool subFolders = true)
        {
            fileOpsBase.deleteFiles(path, selectionPattern, subFolders);
        }

        /// <summary>
        /// Reference to the registered parent node (if any)
        /// </summary>
        public virtual IFolderNode parent { get; protected set; }

        /// <summary>
        /// Depth level, from the root <see cref="folderNode"/> object in the hierarchy
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        public int level
        {
            get
            {
                if (parent == null)
                {
                    return 1;
                }
                else
                {
                    return parent.level + 1;
                }
            }
        }

        /// <summary>
        /// Gets the root node in the structure
        /// </summary>
        /// <value>
        /// The root.
        /// </value>
        public IFolderNode root
        {
            get
            {
                if (parent == null)
                {
                    return this;
                }
                else
                {
                    return parent.root;
                }
            }
        }

        IFolderNode IFolderNode.root
        {
            get
            {
                return root;
            }
        }

        string IObjectWithTreeView.forTreeview
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        object IObjectWithRoot.root
        {
            get
            {
                return root;
            }
        }

        object IObjectWithParent.parent
        {
            get
            {
                return parent;
            }
        }

        /// <summary>
        /// Gets the <see cref="folderNode"/> with the specified key. If not found it will create new sub folder with <c>key</c> name
        /// </summary>
        /// <value>
        /// The <see cref="folderNode"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public folderNode this[Enum key]
        {
            get
            {
                if (!items.ContainsKey(key.ToString()))
                {
                    Directory.CreateDirectory(imbSciStringExtensions.add(path, key.ToString(), "\\"));
                    Add(key, key.ToString().imbTitleCamelOperation(true), "");
                }
                return ((IDictionary<string, folderNode>)items)[key.toString()];
            }

            //set
            //{
            //    ((IDictionary<string, folderNode>)items)[key.toString()] = value;
            //}
        }

        /// <summary>
        /// Gets the <see cref="folderNode"/> with the specified key. If not found it will create new sub folder with <c>key</c> name
        /// </summary>
        /// <value>
        /// The <see cref="folderNode"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public folderNode this[string key]
        {
            get
            {
                if (!items.ContainsKey(key))
                {
                    Directory.CreateDirectory(imbSciStringExtensions.add(path, key.ToString(), "\\"));
                    return Add(key, key.imbTitleCamelOperation(true), "");
                }
                return ((IDictionary<string, folderNode>)items)[key];
            }

            //set
            //{
            //    ((IDictionary<string, folderNode>)items)[key] = value;
            //}
        }

        /// <summary>
        /// Generates the folder readme.
        /// </summary>
        /// <param name="notation">The notation.</param>
        /// <param name="builder">The builder.</param>
        /// <param name="directoryStructureDepthLimit">The directory structure depth limit - i.e. until what subdirectory depth is described in the readme file.</param>
        /// <returns></returns>
        internal string generateFolderReadme(aceAuthorNotation notation, ITextRender builder = null, Int32 directoryStructureDepthLimit = 3)
        {
            if (builder == null) builder = new builderForMarkdown();
            String prefix = "";

            String mypath = path;
            if (parent != null)
            {
                mypath = mypath.removeStartsWith(parent.path);
                prefix = path;
            }

            builder.AppendHeading("Directory information", 2);

            builder.AppendHeading(caption, 3);
            builder.AppendLine(" > " + mypath);
            builder.AppendLine(" > " + description);

            builder.AppendHorizontalLine();

            foreach (String st in AdditionalDescriptionLines)
            {
                builder.AppendLine(st);
            }

            if (AdditionalDescriptionLines.Any())
            {
                builder.AppendHorizontalLine();
            }

            if (AdditionalFileEntries.Any())
            {
                builder.AppendHeading("Files in this directory:", 2);
                String format = "D" + AdditionalFileEntries.Count().ToString().Length.ToString();
                Int32 flc = 1;

                List<String> sortedKeys = AdditionalFileEntries.Keys.ToList();
                sortedKeys.Sort(String.CompareOrdinal);

                foreach (String key in sortedKeys)
                {
                    builder.AppendLine(flc.ToString(format) + " : " + AdditionalFileEntries[key].description);
                    flc++;
                }
            }

            //builder.AppendHeading("Folder treeview", 2);

            //builder.Append(this.tree)

            builder.AppendHorizontalLine();

            builder.AppendHeading("Subdirectories of: " + prefix, 2);

            var folderNodes = this.getAllChildrenInType<folderNode>(null, false, true, 0, directoryStructureDepthLimit);

            foreach (var fold in folderNodes)
            {
                Int32 levelDistance = fold.level - level;

                String insert = " -- ".Repeat(levelDistance);

                if (levelDistance > directoryStructureDepthLimit)
                {
                    if (fold.count() > 0)
                    {
                        builder.AppendCite(insert + "> directory " + fold.caption + " with [" + fold.count() + "] sub directories ...");
                    }
                }
                else
                {
                    builder.AppendLine(String.Format("{0,-60} : {1,-100}", insert + "> " + fold.caption, fold.path.removeStartsWith(prefix)));
                    if (!fold.description.isNullOrEmpty()) builder.AppendLine(insert + "| " + fold.description);
                }
                //builder.AppendLine();
                //  builder.prevTabLevel();
            }

            //  AdditionalFileEntries.Sort(String.CompareOrdinal);

            if (notation != null)
            {
                builder.AppendHorizontalLine();
                notation.GetDescription(builder);
            }

            builder.AppendLine();
            builder.AppendHorizontalLine();

            builder.AppendLine(imbSciStringExtensions.add(imbSciStringExtensions.add("File generated: ", DateTime.Now.ToLongDateString(), " "), DateTime.Now.ToLongTimeString()));

            return builder.ContentToString(true);
        }

        private Object GenerateReadmeLock = new Object();

        /// <summary>
        /// These are additional description lines that will be inserted in readme file generated by <see cref="generateReadmeFiles(aceAuthorNotation, string)" />
        /// </summary>
        /// <value>
        /// The additional description lines.
        /// </value>
        public List<String> AdditionalDescriptionLines { get; protected set; } = new List<string>();

        /// <summary>
        /// Gets or sets the additional file entries.
        /// </summary>
        /// <value>
        /// The additional file entries.
        /// </value>
        public Dictionary<String, folderNodeFileDescriptor> AdditionalFileEntries { get; protected set; } = new Dictionary<String, folderNodeFileDescriptor>();

        /// <summary>
        /// Default directory readme filename, used for <see cref="generateReadmeFiles(aceAuthorNotation, string)"/>
        /// </summary>
        public static String directory_readme_filename = "directory_readme.txt";

        /// <summary>
        /// Generates the readme files for complete folder tree
        /// </summary>
        /// <param name="notation">The notation data object</param>
        /// <param name="readmeFileName">Overrides the default readme file name, defined by <see cref="directory_readme_filename" />.</param>
        /// <param name="directoryStructureDepthLimit">The directory structure depth limit.</param>
        public void generateReadmeFiles(aceAuthorNotation notation, String readmeFileName = "", Int32 directoryStructureDepthLimit = 3)
        {
            if (readmeFileName.isNullOrEmpty())
            {
                readmeFileName = directory_readme_filename;
            }
            lock (GenerateReadmeLock)
            {
                String readme_filename = readmeFileName;
                String mpath = this.pathFor(readme_filename); //imbSciStringExtensions.add(path, readme_filename, "\\");
                generateFolderReadme(notation).saveStringToFile(mpath);
                var folderNodes = this.getAllChildrenInType<folderNode>();
                foreach (folderNode ni in folderNodes)
                {
                    String path = ni.pathFor(readme_filename);

                    FileInfo fi = path.getWritableFile(getWritableFileMode.overwrite);

                    String content = ni.generateFolderReadme(notation, null, directoryStructureDepthLimit);
                    content.saveStringToFile(fi.FullName, getWritableFileMode.overwrite);
                    saveBase.saveToFile(fi.FullName, content);
                }
            }
        }

        /// <summary>
        /// Gets the number of registered subfolders
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public Int32 Count
        {
            get
            {
                return items.Count;
            }
        }

        /// <summary>
        /// Gets the number of all registered subfolders in full subtree depth
        /// </summary>
        /// <returns>Number of all registered subfolders - including the ones from child nodes</returns>
        public Int32 CountAll()
        {
            Int32 c = Count;
            foreach (folderNode fl in items.Values)
            {
                c += fl.CountAll();
            }
            return c;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<string, folderNode>> GetEnumerator()
        {
            return ((IDictionary<string, folderNode>)items).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, folderNode>)items).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<folderNode> IEnumerable<folderNode>.GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }

        /// <summary>
        /// Unregisteres an subfolder from the node. It will not delete actual directory from the file system
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key)
        {
            items.Remove(key);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<IObjectWithPathAndChildren> IEnumerable<IObjectWithPathAndChildren>.GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void IObjectWithPathAndChildren.Remove(string key) => Remove(key);
    }
}