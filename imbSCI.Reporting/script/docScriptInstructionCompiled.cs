// --------------------------------------------------------------------------------------------------------------------
// <copyright file="docScriptInstructionCompiled.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.extensions;
    using imbSCI.Core.reporting.template;
    using imbSCI.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using d = docScriptArguments;

    /// <summary>
    /// Compiled version of docScriptInstruction with compilation debug information
    /// </summary>
    /// <seealso cref="docScriptInstruction" />
    public class docScriptInstructionCompiled : docScriptInstruction
    {
        #region --- missingData ------- List of placeholders that were missing data points suring compile process

        /// <summary>
        /// List of placeholders that were missing data points suring compile process
        /// </summary>
        public List<string> missingData { get; set; } = new List<string>();

        #endregion --- missingData ------- List of placeholders that were missing data points suring compile process

        #region --- keyListForNonStrings ------- list of properties that are not strings

        /// <summary>
        /// list of properties that are not strings
        /// </summary>
        public List<docScriptArguments> keyListForNonStrings { get; set; } = new List<d>();

        #endregion --- keyListForNonStrings ------- list of properties that are not strings

        #region --- keyListForStringCollections ------- list of properties with string collection

        /// <summary>
        /// list of properties with string collection
        /// </summary>
        public List<docScriptArguments> keyListForStringCollections { get; set; } = new List<d>();

        #endregion --- keyListForStringCollections ------- list of properties with string collection

        #region --- keyListForStrings ------- List of properties with String type value

        /// <summary>
        /// List of properties with String type value
        /// </summary>
        public List<docScriptArguments> keyListForStrings { get; set; } = new List<d>();

        #endregion --- keyListForStrings ------- List of properties with String type value

        #region --- keyListForFailedStrings ------- List of properties that had no required data to finish template compilation

        /// <summary>
        /// List of properties that had no required data to finish template compilation
        /// </summary>
        public List<docScriptArguments> keyListForFailedStrings { get; set; } = new List<d>();

        #endregion --- keyListForFailedStrings ------- List of properties that had no required data to finish template compilation

        /// <summary>
        /// Indicating whether compile did failed on any placeholder or string lines
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is compile failed; otherwise, <c>false</c>.
        /// </value>
        public bool isCompileFailed
        {
            get
            {
                if (missingData.Any()) return true;
                if (keyListForFailedStrings.Any()) return true;
                return false;
            }
        }

        /// <summary>
        /// Indicating if string arguments were detected
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is compilable; otherwise, <c>false</c>.
        /// </value>
        public bool isCompilable
        {
            get
            {
                if (keyListForStrings.Any()) return true;
                if (keyListForStringCollections.Any()) return true;
                return false;
            }
        }

        public docScriptInstructionCompiled(docScriptInstruction __source, docScriptFlags flags) : base(__source.type)
        {
            this.copyInto(__source);
            analyse(flags);
        }

        /// <summary>
        /// Populates keyLists
        /// </summary>
        /// <exception cref="System.ArgumentNullException">docScriptArgument can't have null value!</exception>
        internal void analyse(docScriptFlags flags)
        {
            foreach (docScriptArguments arg in Keys)
            {
                if (this[arg] == null)
                {
                    if (!flags.HasFlag(docScriptFlags.ignoreArgumentValueNull))
                    {
                        throw new ArgumentNullException(type.toStringSafe().add(arg.ToString(), "->"), "docScriptArgument can't have null value!");
                    }
                    else
                    {
                        // this[arg] = "";
                    }
                }
                if (this[arg] is string)
                {
                    keyListForStrings.Add(arg);
                }
                else if (this[arg] is IEnumerable<string>)
                {
                    keyListForStringCollections.Add(arg);
                }
                else
                {
                    keyListForNonStrings.Add(arg);
                }
            }
        }

        /// <summary>
        /// Compiles template arguments using dataSets
        /// </summary>
        /// <param name="dataSets">The data sets to apply</param>
        public void compileTemplate(params PropertyCollection[] dataSets)
        {
            if (dataSets == null)
            {
                return;
            }
            foreach (docScriptArguments arg in keyListForStringCollections)
            {
                IEnumerable<string> lines = this[arg] as IEnumerable<string>;
                List<string> newLines = new List<string>();
                foreach (string line in lines)
                {
                    newLines.Add(applyLine(line, arg, dataSets));
                }
                this[arg] = newLines;
            }
            foreach (docScriptArguments arg in keyListForStrings)
            {
                string line = this[arg] as string;
                this[arg] = applyLine(line, arg, dataSets);
            }
        }

        /// <summary>
        /// Applies the dataset on the line
        /// </summary>
        /// <param name="line">The line.</param>
        /// <param name="arg">The argument.</param>
        /// <param name="dataSets">The data sets.</param>
        /// <returns></returns>
        internal string applyLine(string line, docScriptArguments arg, PropertyCollection[] dataSets)
        {
            string cline = line.applyToContent(false, dataSets);
            if (cline.isTemplate())
            {
                keyListForFailedStrings.AddUnique(arg);
                reportTemplatePlaceholderCollection plcs = cline.getPlaceHolders();
                foreach (KeyValuePair<string, reportTemplatePlaceholder> plcp in plcs)
                {
                    missingData.AddUnique(plcp.Key);
                }
            }
            return cline;
        }
    }
}