// --------------------------------------------------------------------------------------------------------------------
// <copyright file="docScriptCompiled.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.render;
    using imbSCI.Data.data;
    using imbSCI.Data.enums.appends;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using d = docScriptArguments;

#pragma warning disable CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    /// <summary>
    /// Compiled resion of the script
    /// </summary>
    /// <seealso cref="imbSCI.Cores.primitives.imbBindable" />
    public class docScriptCompiled : imbBindable
#pragma warning restore CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    {
        /// <summary>
        /// Adds the specified ins.
        /// </summary>
        /// <param name="ins">The ins.</param>
        /// <returns></returns>
        public docScriptInstructionCompiled compile(docScriptInstruction ins, IRenderExecutionContext log, PropertyCollectionDictionary __indata, PropertyCollection __extraData, docScriptFlags flags)
        {
            docScriptInstructionCompiled instructionCompiled = new docScriptInstructionCompiled(ins, flags);

            try
            {
                PropertyCollectionDictionary indata = __indata;
                PropertyCollection extraData = __extraData;

                if (ins.type == appendType.x_scopeIn)
                {
                    currentDataPath = ins.getProperString(d.dsa_path);
                }
                else if (ins.type == appendType.x_scopeOut)
                {
                    currentDataPath = currentDataPath.getPathVersion(1, "\\", true);
                }

                PropertyCollection dataItem = null;
                if (indata != null)
                {
                    var tmp = indata[currentDataPath];
#pragma warning disable CS0184 // The given expression is never of the provided ('DictionaryEntry') type
                    if (tmp is DictionaryEntry)
#pragma warning restore CS0184 // The given expression is never of the provided ('DictionaryEntry') type
                    {
                        //DictionaryEntry entry = (DictionaryEntry)tmp;
                        //dataItem = entry.Value as PropertyCollection;
                    }
                    else
                    {
                        dataItem = tmp as PropertyCollection;
                    }
                }

                /// Compiling data into template
                instructionCompiled.compileTemplate(dataItem, extraData);

                if (instructionCompiled.isCompileFailed)
                {
                    string msg = scriptInstructions.Count().ToString("D4") + " " + ins.type.ToString() + " failed on: " + instructionCompiled.missingData.toCsvInLine();
                    if (flags.HasFlag(docScriptFlags.ignoreCompilationFails))
                    {
                        log.log(msg);
                    }
                    else
                    {
                        log.compileError(msg, instructionCompiled);
                    }

                    if (flags.HasFlag(docScriptFlags.allowFailedInstructions))
                    {
                        scriptInstructions.Add(instructionCompiled);
                    }
                    scriptInstructionsFailed.Add(instructionCompiled);
                }
                else
                {
                    scriptInstructions.Add(instructionCompiled);

                    if (instructionCompiled.isCompilable)
                    {
                        // log.log("--- compiled Strings(" + instructionCompiled.keyListForStrings.Count + ") and StringCollections(" + instructionCompiled.keyListForStringCollections.Count + ")");
                    }
                }

                missingData.AddRange(instructionCompiled.missingData);
            }
            catch (Exception ex)
            {
            }
            return instructionCompiled;
        }

        /// <summary>
        /// The script instructions
        /// </summary>
        protected List<docScriptInstructionCompiled> scriptInstructions = new List<docScriptInstructionCompiled>();

        protected List<docScriptInstructionCompiled> scriptInstructionsFailed = new List<docScriptInstructionCompiled>();

        #region --- missingData ------- List of placeholders that were missing data points suring compile process

        /// <summary>
        /// List of placeholders that were missing data points suring compile process
        /// </summary>
        public List<string> missingData { get; set; } = new List<string>();

        #endregion --- missingData ------- List of placeholders that were missing data points suring compile process

        /// <summary>
        /// Initializes a new instance of the <see cref="docScriptCompiled"/> class.
        /// </summary>
        /// <param name="script">The script.</param>
        public docScriptCompiled(docScript script)
        {
            name = script.name;
        }

        #region --- currentDataPath ------- current data path

        private string _currentDataPath = "";

        /// <summary>
        /// current data path
        /// </summary>
        protected string currentDataPath
        {
            get
            {
                return _currentDataPath;
            }
            set
            {
                _currentDataPath = value;
                OnPropertyChanged("currentDataPath");
            }
        }

        #endregion --- currentDataPath ------- current data path

        public int Count() => scriptInstructions.Count();

        public int CountFailed() => scriptInstructionsFailed.Count();

        public IEnumerator<docScriptInstructionCompiled> GetEnumerator() => scriptInstructions.GetEnumerator();

        /// <summary>
        /// Name of source documentSet or other kind of source
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string name { get; set; }

        /// <summary>
        /// Gets the filename.
        /// </summary>
        /// <value>
        /// The filename.
        /// </value>
        public string filename
        {
            get { return name.getCleanFilePath(); }
        }
    }
}