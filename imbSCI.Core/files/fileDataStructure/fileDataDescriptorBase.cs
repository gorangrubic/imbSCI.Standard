// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileDataDescriptorBase.cs" company="imbVeles" >
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
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.reporting;
using imbSCI.Data;
using imbSCI.Data.enums;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;

namespace imbSCI.Core.files.fileDataStructure
{
    using imbSCI.Core.data.transfer;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Common base class for <see cref="fileDataPropertyDescriptor"/> and <see cref="fileDataStructureDescriptor"/>
    /// </summary>
    /// <seealso cref="IObjectWithNameAndDescription" />
    public abstract class fileDataDescriptorBase : IObjectWithNameAndDescription
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String name { get; set; } = "";

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public String description { get; set; } = "";

        /// <summary>
        /// Gets or sets the name or property path.
        /// </summary>
        /// <value>
        /// The name or property path.
        /// </value>
        public String nameOrPropertyPath { get; protected set; } = "";

        /// <summary>
        /// Gets or sets the filename mode.
        /// </summary>
        /// <value>
        /// The filename mode.
        /// </value>
        public fileDataFilenameMode filenameMode { get; protected set; } = fileDataFilenameMode.propertyValue;

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>
        /// The mode.
        /// </value>
        public fileDataPropertyMode formatMode { get; set; } = fileDataPropertyMode.autoTextOrXml;

        /// <summary>
        /// Resolved file name
        /// </summary>
        /// <value>
        /// The filename.
        /// </value>
        public String filename { get; protected set; }

        /// <summary>
        /// Descriptive information on the memberInfo
        /// </summary>
        /// <value>
        /// The s mi.
        /// </value>
        public settingsMemberInfoEntry memberMetaInfo { get; protected set; }

        public Type type { get; protected set; }

        //public Boolean typeIsDataStructure { get; protected set; }

        public fileDataPropertyOptions options { get; protected set; } = fileDataPropertyOptions.none;

        public abstract String GetFilepath(String path = "", IFileDataStructure instance = null, Boolean appendPathInOutput = false);

        /// <summary>
        /// Saves the data file.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="fullpath">The fullpath.</param>
        /// <param name="output">The output.</param>
        /// <param name="preventThrow">if set to <c>true</c> it will prevent throwing exception.</param>
        /// <exception cref="System.NotImplementedException">JSON not implemented yet
        /// or
        /// binary file not implemented yet</exception>
        protected void SaveDataFile(Object instance, String fullpath, ILogBuilder output = null, Boolean preventThrow = false)
        {
            fullpath = fullpath.getWritableFile(getWritableFileMode.overwrite).FullName;

            try
            {
                if (options.HasFlag(fileDataPropertyOptions.SupportLoadSave))
                {
                    ISupportLoadSave loadSave = instance as ISupportLoadSave;
                    loadSave.SaveAs(fullpath, getWritableFileMode.overwrite);
                }
                else
                {
                    switch (formatMode)
                    {
                        case fileDataPropertyMode.XML:

                            objectSerialization.saveObjectToXML(instance, fullpath);

                            break;

                        case fileDataPropertyMode.JSON:
                            throw new NotImplementedException("JSON not implemented yet");
                            break;

                        case fileDataPropertyMode.binary:
                            throw new NotImplementedException("binary file not implemented yet");
                            break;

                        case fileDataPropertyMode.text:
                            String text = "";// openBase.openFileToString(filepath, false);

                            if (type == typeof(List<String>))
                            {
                                List<String> list = (List<String>)instance;
                                text = list.toCsvInLine(ListStringSeparator);
                            }
                            else if (type == typeof(String))
                            {
                                text = instance as String;
                            }
                            else
                            {
                                throw new InvalidDataException("Format type [" + formatMode + "] not supported for type [" + type.Name + "] " + "Format not supported - SaveDataFile");
                            }

                            text.saveStringToFile(fullpath, getWritableFileMode.overwrite);

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                //if (output == null && preventThrow) output = aceLog.loger;

                fileDataStructureExtensions.FileDataStructureError("Error saving [" + fullpath + "] of type: " + instance.GetType().Name, null, output, ex, instance as IFileDataStructure);
                if (!preventThrow) throw;
            }
        }

        //public String ListStringSeparator = ";" + Environment.NewLine;

        private String _ListStringSeparator = Environment.NewLine;

        /// <summary>
        ///
        /// </summary>
        public String ListStringSeparator
        {
            get { return _ListStringSeparator; }
            set { _ListStringSeparator = value; }
        }

        /// <summary>
        /// Loads the data file.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <param name="output">The output.</param>
        /// <param name="itemType">Type of the item.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">
        /// JSON not implemented yet
        /// or
        /// binary file not implemented yet
        /// </exception>
        /// <exception cref="InvalidDataException">Format type [" + formatMode + "] not supported for type [" + type.Name + "]" + "Format not supported - LoadDataFile</exception>
        /// <exception cref="System.NotImplementedException">JSON not implemented yet
        /// or
        /// binary file not implemented yet</exception>
        protected Object LoadDataFile(String filepath, ILogBuilder output = null, Type itemType = null)
        {
            Object instance = null;
            if (itemType == null) itemType = type;
            if (File.Exists(filepath))
            {
                if (options.HasFlag(fileDataPropertyOptions.SupportLoadSave))
                {
                    ISupportLoadSave loadSave = itemType.getInstance() as ISupportLoadSave;
                    loadSave.name = name;
                    loadSave.LoadFrom(filepath);
                }
                else
                {
                    switch (formatMode)
                    {
                        default:
                        case fileDataPropertyMode.XML:
                            instance = objectSerialization.loadObjectFromXML(filepath, itemType);
                            break;

                        case fileDataPropertyMode.JSON:
                            throw new NotImplementedException("JSON not implemented yet");
                            break;

                        case fileDataPropertyMode.binary:
                            throw new NotImplementedException("binary file not implemented yet");
                            break;

                        case fileDataPropertyMode.text:
                            String text = openBase.openFileToString(filepath, false);

                            if (type == typeof(List<String>))
                            {
                                instance = imbSciStringExtensions.SplitSmart(text, ListStringSeparator, "", false, true);
                            }
                            else if (type == typeof(String))
                            {
                                instance = text;
                            }
                            else
                            {
                                throw new InvalidDataException("Format type [" + formatMode + "] not supported for type [" + type.Name + "]" + "Format not supported - LoadDataFile");
                            }

                            break;
                    }
                }
            }
            else
            {
                var constructor = type.GetConstructor(new Type[] { });
                if (constructor != null)
                {
                    instance = Activator.CreateInstance(itemType, new Type[] { });
                    if (output != null)
                    {
                        // output.log("File [" + filepath + "] not found - but new instance of (" + itemType.Name + ") created");
                    }
                }
                else
                {
                    if (output != null)
                    {
                        //output.log("Loading [" + filepath + "] failed as file not found (" + itemType.Name + ")");
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// Deploys the descriptor base.
        /// </summary>
        /// <param name="memberInfo">The member information.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="instance">The instance.</param>
        protected void deployDescriptorBase(MemberInfo memberInfo, fileDataBaseAttribute attribute, Object instance = null)
        {
            filenameMode = attribute.filenameMode;
            nameOrPropertyPath = attribute.nameOrPropertyPath;
            options = attribute.options;
            memberMetaInfo = new settingsMemberInfoEntry(memberInfo);

            name = memberMetaInfo.displayName.or(memberInfo.Name);
            description = memberMetaInfo.description;
            formatMode = attribute.formatMode;

            if (memberInfo is PropertyInfo)
            {
                PropertyInfo pi = memberInfo as PropertyInfo;
                type = pi.PropertyType;

                if (type.GetInterface(nameof(ISupportLoadSave)) != null)
                {
                    options |= fileDataPropertyOptions.SupportLoadSave;

                    if (formatMode == fileDataPropertyMode.autoTextOrXml) formatMode = fileDataPropertyMode.XML;
                }

                if (formatMode == fileDataPropertyMode.autoTextOrXml)
                {
                    if (type == typeof(String))
                    {
                        formatMode = fileDataPropertyMode.text;
                    }
                    else if (type == typeof(List<String>))
                    {
                        if (options.HasFlag(fileDataPropertyOptions.itemAsFile))
                        {
                            formatMode = fileDataPropertyMode.text;
                        }
                        else
                        {
                            formatMode = fileDataPropertyMode.XML;
                        }
                    }
                    else
                    {
                        formatMode = fileDataPropertyMode.XML;
                    }
                }
            }
            else if (memberInfo is Type)
            {
                type = memberInfo as Type;

                if (formatMode == fileDataPropertyMode.autoTextOrXml)
                {
                    formatMode = fileDataPropertyMode.XML;
                }
            }

            switch (filenameMode)
            {
                case fileDataFilenameMode.customName:
                    filename = attribute.nameOrPropertyPath;
                    break;

                case fileDataFilenameMode.memberInfoName:
                    filename = memberInfo.Name.getCleanFilepath("");
                    break;

                case fileDataFilenameMode.propertyValue:
                    if (instance != null)
                    {
                        PropertyExpression pexp = new PropertyExpression(instance, attribute.nameOrPropertyPath);
                        filename = pexp.getValue().toStringSafe(memberInfo.Name).getCleanFilepath("");
                    }
                    else
                    {
                        filename = attribute.nameOrPropertyPath.getCleanFilepath("");
                    }
                    break;
            }
        }
    }
}