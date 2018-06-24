// --------------------------------------------------------------------------------------------------------------------
// <copyright file="docScriptStandaloneContextWrapper.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.script
{
    using imbSCI.Core.collection;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.style;
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using imbSCI.Reporting.delivery;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;

    /// <summary>
    /// Wrapper with it's own directoryScope and directoryRoot
    /// </summary>
    /// <seealso cref="imbSCI.Core.reporting.render.IRenderExecutionContext" />
    public class docScriptStandaloneContextWrapper : IRenderExecutionContext
    {
        public docScriptStandaloneContextWrapper(IRenderExecutionContext __context)
        {
            context = __context;
        }

        public DirectoryInfo directoryScope
        {
            get
            {
                return context.directoryScope;
            }

            set
            {
                context.directoryScope = value;
            }
        }

        public DirectoryInfo directoryRoot
        {
            get
            {
                return context.directoryRoot;
            }

            set
            {
                context.directoryRoot = value;
            }
        }

        /// <summary> </summary>
        protected IRenderExecutionContext context { get; set; }

        public object dUnit
        {
            get
            {
                return context.dUnit;
            }
        }

        public Dictionary<logOutputSpecial, object> specialLogOuts
        {
            get
            {
                return context.specialLogOuts;
            }
        }

        public PropertyCollectionDictionary dataDictionary
        {
            get
            {
                return context.dataDictionary;
            }
        }

        public reportOutputRepository outputRepositorium
        {
            get
            {
                return null; // context.outputRepositorium;
            }
        }

        public string name
        {
            get
            {
                return context.name;
            }
        }

        public PropertyCollection data
        {
            get
            {
                return context.data;
            }
        }

        public styleTheme theme
        {
            get
            {
                return context.theme;
            }
        }

        public ITextRender render
        {
            get
            {
                return context.render;
            }
        }

        public IMetaContentNested scope
        {
            get
            {
                return context.scope;
            }
        }

        //public FileInfo fileInfo
        //{
        //    get
        //    {
        //        return context.fileInfo;
        //    }

        //    set
        //    {
        //        context.fileInfo = value;
        //    }
        //}

        public areaStyleInstructionStack styleStack
        {
            get
            {
                return context.styleStack;
            }

            set
            {
                context.styleStack = value;
            }
        }

        public selectRangeAreaDictionary metaContentRanges
        {
            get
            {
                return context.metaContentRanges;
            }
        }

        Dictionary<logOutputSpecial, object> IRenderExecutionContext.specialLogOuts { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void regFileOutput(string filepath, Enum idPath, string description, string title = "")
        {
            context.regFileOutput(filepath, idPath, description, title);
        }

        public void regFileOutput(string filepath, string idPath, string description, string title = "")
        {
            context.regFileOutput(filepath, idPath, description, title);
        }

        public FileInfo saveFileOutput(string output, string filepath, Enum idPath, string description, string title = "")
        {
            return context.saveFileOutput(output, filepath, idPath, description, title);
        }

        public FileInfo saveFileOutput(string output, string filepath, string idPath, string description, string title = "")
        {
            return context.saveFileOutput(output, filepath, idPath, description, title);
        }

        public FileInfo getFileInfo(string basename, getWritableFileMode mode, reportOutputFormatName format)
        {
            return context.getFileInfo(basename, mode, format);
        }

        public void compileError(string message, object compiled, Exception ex = null)
        {
            context.compileError(message, compiled, ex);
        }

        public void executionError(string message, object ins, Exception ex = null)
        {
            context.executionError(message, ins, ex);
        }

        public void log(string msg)
        {
            context.log(msg);
        }

        public void error(string msg, appendType atype = appendType.none, Exception ex = null)
        {
            context.error(msg, atype, ex);
        }
    }
}