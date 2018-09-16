// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileDataStructureDescriptor.cs" company="imbVeles" >
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
using imbSCI.Core.data;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.reporting;
using imbSCI.Data;
using imbSCI.Data.enums;
using System;
using System.Collections.Generic;

namespace imbSCI.Core.files.fileDataStructure
{
    using imbSCI.Core.files.folders;
    using System.IO;
    using System.Reflection;
    using imbStringPathTools = imbSCI.Core.extensions.text.imbStringPathTools;

    /// <summary>
    /// Descriptor for class implementing <see cref="IFileDataStructure"/> interface. Metadata are defined with <see cref="fileStructureAttribute"/>
    /// </summary>
    /// <seealso cref="fileDataDescriptorBase" />
    public class fileDataStructureDescriptor : fileDataDescriptorBase
    {
        internal SortedList<Int32, fileDataPropertyDescriptor> fileDataProperties = new SortedList<int, fileDataPropertyDescriptor>();

        public fileStructureMode mode { get; set; } = fileStructureMode.subdirectory;

        /// <summary>
        /// Loads the data structure.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="parentFolder">The parent folder.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="output">The output.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">Can't have File Data Structure loaded if no file structure mode specified</exception>
        internal IFileDataStructure LoadDataStructure(String path, folderNode parentFolder = null, IFileDataStructure instance = null, ILogBuilder output = null)
        {
            if (parentFolder == null) parentFolder = new folderNode();

            switch (mode)
            {
                case fileStructureMode.subdirectory:
                    if (path.Contains("."))
                    {
                        path = Path.GetDirectoryName(path);
                        if (path.isNullOrEmptyString())
                        {
                            fileDataStructureExtensions.FileDataStructureError("This is subdirectory data structure, do not use directory names with dot [" + path + "] " + "Path contains dot", parentFolder, output, null, instance);
                            //throw new ArgumentException("This is subdirectory data structure, do not use directory names with dot [" + path + "] " + "Path contains dot", nameof(path));
                        }
                    }

                    var folder = parentFolder.createDirectory(path, "", false);
                    filename = type.Name.getCleanPropertyName().add(formatMode.GetExtension(), ".");

                    if (instance != null)
                    {
                        instance.folder = folder;
                    }
                    parentFolder = folder;
                    break;

                case fileStructureMode.none:
                    filename = GetFilepath(path, instance, false);
                    fileDataStructureExtensions.FileDataStructureError("Can't have File Data Structure loaded if no file structure mode specified. " + path, parentFolder, output, null, instance);
                    //throw new NotImplementedException("Can't have File Data Structure loaded if no file structure mode specified");
                    break;
            }

            String filepath = parentFolder.pathFor(filename);

            if (File.Exists(filepath))
            {
                instance = LoadDataFile(filepath, output) as IFileDataStructure;
            }
            else
            {
                if (instance == null)
                {
                    fileDataStructureExtensions.FileDataStructureError("File not found[" + filepath + "] no instance created", parentFolder, output, null, instance);
                }
                else
                {
                    fileDataStructureExtensions.FileDataStructureError("File not found[" + filepath + "] default instance created", parentFolder, output, null, instance);
                    //  Console.WriteLine("File not found [" + filepath + "] default instance created");
                }
            }

            instance.folder = parentFolder;

            foreach (var pair in fileDataProperties)
            {
                fileDataPropertyDescriptor pDesc = pair.Value;
                try
                {
                    pDesc.LoadDataAndSet(instance, parentFolder, output);
                }
                catch (Exception ex)
                {
                    fileDataStructureExtensions.FileDataStructureError("Loading property [" + pDesc.name + "] for [" + instance.name + "] failed. " + ex.Message, parentFolder, output, ex, instance);
                }
            }

            if (instance != null)
            {
                parentFolder.description = instance.description;
                parentFolder.caption = instance.name;
            }

            try
            {
                instance.OnLoaded();
            }
            catch (Exception ex)
            {
                fileDataStructureExtensions.FileDataStructureError("instance [" + instance.name + "] OnLoaded() failed. " + ex.Message + " : " + ex.StackTrace, parentFolder, output, ex, instance);
                //Console.WriteLine("instance [" + instance.name + "] OnLoaded() failed. " + ex.Message + " : " + ex.StackTrace);
            }

            return instance;
        }

        /// <summary>
        /// Deletes the data structure file, where filepath is determined using the <see cref="fileDataStructureDescriptor"/> of <see cref="IFileDataStructure"/> instance specified.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="output">The output.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">Can't have File Data Structure loaded if no file structure mode specified</exception>
        internal Boolean DeleteDataStructure(IFileDataStructure instance, ILogBuilder output = null)
        {
            String filename = ""; // GetFilepath("", instance, false);

            switch (mode)
            {
                case fileStructureMode.subdirectory:

                    if (!Directory.Exists(instance.folder.path)) return false;

                    DirectoryInfo di = new DirectoryInfo(instance.folder.path);

                    di.Delete(true);

                    break;

                case fileStructureMode.none:
                    fileDataStructureExtensions.FileDataStructureError("Can't have File Data Structure loaded if no file structure mode specified", instance?.folder, output, null, instance);
                    //throw new NotImplementedException("Can't have File Data Structure loaded if no file structure mode specified");
                    break;
            }

            return true;
        }

        private String GetFilenameAndSetInstanceFolder(IFileDataStructure instance, folderNode parentFolder = null, ILogBuilder output = null)
        {
            String filename = ""; // GetFilepath("", instance, false);
            try
            {
                switch (mode)
                {
                    case fileStructureMode.subdirectory:
                        //parentFolder = Directory.CreateDirectory(parentFolder.path);
                        if (instance.folder == null)
                        {
                            instance.folder = parentFolder.Add(instance.name, instance.name, "Directory for [" + instance.GetType().Name + "]. " + instance.description);
                        }
                        else
                        {
                            if (instance.folder.name != instance.name)
                            {
                                instance.folder = parentFolder.Add(instance.name, instance.name, "Directory for [" + instance.GetType().Name + "]. " + instance.description);
                            }
                        }
                        filename = type.Name.getCleanPropertyName().add(formatMode.GetExtension(), ".");

                        break;

                    case fileStructureMode.none:
                        fileDataStructureExtensions.FileDataStructureError("Can't have File Data Structure loaded if no file structure mode specified", parentFolder, output, null, instance);

                        //throw new NotImplementedException("Can't have File Data Structure loaded if no file structure mode specified");
                        break;
                }
            }
            catch (Exception ex)
            {
                fileDataStructureExtensions.FileDataStructureError("SaveDataStructure failed at designating folder and filename: " + ex.Message, parentFolder, output, ex, instance);
            }
            return filename;
        }

        /// <summary>
        /// Saves the data structure: its properties marked with <see cref="fileDataAttribute"/> attribute and it self
        /// </summary>
        /// <param name="instance">The instance that has to be saved</param>
        /// <param name="parentFolder">The parent folder in which this instance will be saved - if not specified the application current folder is used</param>
        /// <param name="output">Logger</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">Can't have File Data Structure loaded if no file structure mode specified</exception>
        internal String SaveDataStructure(IFileDataStructure instance, folderNode parentFolder = null, ILogBuilder output = null)
        {
            if (parentFolder == null)
            {
                parentFolder = new folderNode();
            }

            String desc = instance.description;
            if (desc.isNullOrEmpty()) desc = description;

            String filename = GetFilenameAndSetInstanceFolder(instance, parentFolder, output);

            instance.OnBeforeSave();

            foreach (var pair in fileDataProperties)
            {
                fileDataPropertyDescriptor pDesc = pair.Value;
                pDesc.SaveData(instance, instance.folder, output);
            }

            String filepath = instance.folder.pathFor(filename, getWritableFileMode.overwrite, desc, true);

            SaveDataFile(instance, filepath, output);

            return filepath;
        }

        public fileDataStructureDescriptor()
        {
        }

        public fileDataStructureDescriptor(IFileDataStructure dataStructure)
        {
            deployDescriptor(dataStructure);
        }

        internal void deployDescriptor(IFileDataStructure dataStructure)
        {
            Type type = dataStructure.GetType();
            deployDescriptor(type, dataStructure);
        }

        internal void deployDescriptor(Type type, IFileDataStructure dataStructure)
        {
            //Type type = dataStructure.GetType();

            fileStructureAttribute attribute = Attribute.GetCustomAttribute(type, typeof(fileStructureAttribute)) as fileStructureAttribute;
            if (attribute != null)
            {
                deployDescriptorBase(type, attribute, dataStructure);
            }

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);

            foreach (var prop in properties)
            {
                fileDataAttribute propAttribute = Attribute.GetCustomAttribute(prop, typeof(fileDataAttribute)) as fileDataAttribute;
                if (propAttribute != null)
                {
                    fileDataPropertyDescriptor propDescriptor = new fileDataPropertyDescriptor(prop, propAttribute, dataStructure);
                    Int32 p = propDescriptor.memberMetaInfo.priority;
                    while (fileDataProperties.ContainsKey(p))
                    {
                        p++;
                    }

                    fileDataProperties.Add(p, propDescriptor);
                }
            }
        }

        public override string GetFilepath(string path = "", IFileDataStructure instance = null, bool appendPathInOutput = false)
        {
            String filepath = "";
            switch (filenameMode)
            {
                case fileDataFilenameMode.memberInfoName:
                case fileDataFilenameMode.customName:
                    filepath = filename;
                    break;

                case fileDataFilenameMode.propertyValue:

                    filepath = Path.GetFileNameWithoutExtension(path);
                    path = imbStringPathTools.getPathVersion(path, 1, Path.DirectorySeparatorChar);
                    if (instance != null)
                    {
                        PropertyExpression pexp = new PropertyExpression(instance, nameOrPropertyPath);
                        pexp.setValue(filename);
                    }
                    break;

                default:

                    fileDataStructureExtensions.FileDataStructureError("File Data Structure can't have filename by property value. " + path, instance?.folder, null, null, instance);
                    break;
            }

            if (options.HasFlag(fileDataPropertyOptions.itemAsFile))
            {
                filepath = filepath + "_*";
            }

            //path = path.ensureEndsWith(filepath);

            if (appendPathInOutput) path = path.add(filepath, Path.DirectorySeparatorChar);

            return path;
        }
    }
}