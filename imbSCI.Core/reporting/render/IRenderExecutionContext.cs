// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRenderExecutionContext.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.render
{
    using imbSCI.Core.collection;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.style;
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;

    //using imbReportingCore.interfaces;
    //using imbReportingCore.script;

    /// <summary>
    /// Execution context for rendering and reporting
    /// </summary>
    public interface IRenderExecutionContext
    {
        Object dUnit { get; }

        //   Dictionary<logOutputSpecial, Object> specialLogOuts { get; }

        void regFileOutput(String filepath, Enum idPath, String description, String title = "");

        void regFileOutput(String filepath, String idPath, String description, String title = "");

        PropertyCollectionDictionary dataDictionary { get; }

        //  Dictionary<logOutputSpecial, object> specialLogOuts { get; }

        FileInfo saveFileOutput(String output, String filepath, Enum idPath, String description, String title = "");

        FileInfo saveFileOutput(String output, String filepath, String idPath, String description, String title = "");

        /// <summary>
        /// Gets the file info
        /// </summary>
        /// <param name="basename">The basename.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        FileInfo getFileInfo(String basename, getWritableFileMode mode, reportOutputFormatName format);

        //  reportOutputRepository outputRepositorium { get; }

        void compileError(String message, object compiled, Exception ex = null);

        // void error(string msg, appendType atype = appendType.none, Exception ex = null);

        void executionError(String message, object ins, Exception ex = null);

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        String name { get; }

        /// <summary>
        /// Index position of the current instruction
        /// </summary>
        /// <value>
        /// 0 based instruction line number
        /// </value>
       // Int32 index { get; }
        /// <summary>
        /// Execution data storage
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        PropertyCollection data { get; }

        /// <summary>
        /// Active directory
        /// </summary>
        /// <value>
        /// The current directory.
        /// </value>
        DirectoryInfo directoryScope { get; set; }

        /// <summary>
        /// Directory that is root for current execution context
        /// </summary>
        /// <value>
        /// Directory reporting output root
        /// </value>
        DirectoryInfo directoryRoot { get; set; }

        /// <summary>
        /// Theme used for rendering
        /// </summary>
        /// <value>
        /// The style.
        /// </value>
        styleTheme theme { get; }

        /// <summary>
        /// Logs a custom message
        /// </summary>
        /// <param name="msg">The MSG.</param>
        void log(String msg);

        /// <summary>
        /// Reports an error with optional message and exception
        /// </summary>
        /// <param name="msg">Custom message about the error</param>
        /// <param name="atype">Type of Append operation that caused error</param>
        /// <param name="ex">Exception if happen</param>
        void error(String msg, appendType atype = appendType.none, Exception ex = null);

        /// <summary>
        /// Render instance used for output
        /// </summary>
        /// <value>
        /// The render.
        /// </value>
        ITextRender render { get; }

        /// <summary>
        /// Current scope in the meta model
        /// </summary>
        /// <value>
        /// The scope.
        /// </value>
        IMetaContentNested scope { get; }

        /// <summary>
        /// File output
        /// </summary>
        /// <value>
        /// The file information.
        /// </value>
        //FileInfo fileInfo { get; set; }

        /// <summary>
        /// Scheduled styling instructions -- used to process> current append or future append
        /// </summary>
        areaStyleInstructionStack styleStack { get; set; }

        /// <summary>
        /// Dictionary of selectRangeArea entries for each metaContent member
        /// </summary>
        selectRangeAreaDictionary metaContentRanges { get; }

        Dictionary<logOutputSpecial, object> specialLogOuts { get; set; }
        // reportOutputRepository outputRepositorium { get; set; }
    }
}