// --------------------------------------------------------------------------------------------------------------------
// <copyright file="translationTextTable.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataComplex.special
{
    using imbSCI.Core.files.unit;
    using imbSCI.Core.reporting;
    using imbSCI.Data;
    using imbSCI.DataComplex.exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Text simple replace-based translation table - dictionary
    /// </summary>
    /// <seealso cref="aceCommonTypes.collection.special.translationTable{System.String, System.String}" />
    [Serializable]
    public class translationTextTable : translationTable<string, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="translationTextTable"/> with conversion method specified, enabling auto conversion
        /// </summary>
        /// <param name="__conversionMethod">The conversion method.</param>
        public translationTextTable(autoTranslationMethod __conversionMethod)
        {
            conversionMethod = __conversionMethod;
        }

        public translationTextTable()
        {
        }

        protected autoTranslationMethod conversionMethod { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has automatic conversion enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is automatic conversion enabled; otherwise, <c>false</c>.
        /// </value>
        public bool isAutoConversionEnabled
        {
            get
            {
                return (conversionMethod != null);
            }
        }

        /// <summary>
        /// Checks if this entry is inside the translation table, if it is then where: as value or as key, returns: <see cref="translationTextTableEntryEnum.unknownEntry"/> if it wasn't found at all.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns></returns>
        public translationTextTableEntryEnum checkForEntry(string entry)
        {
            if (byKeys.ContainsKey(entry))
            {
                return translationTextTableEntryEnum.keyEntry;
            }

            if (byValues.ContainsKey(entry))
            {
                return translationTextTableEntryEnum.valueEntry;
            }

            return translationTextTableEntryEnum.unknownEntry;
        }

        /// <summary>
        /// The semarator that is used between Key and Value in the string pair format
        /// </summary>
        public const char ENTRYSEPARATOR = '|';

        protected fileunit sourceFile { get; set; }

        public bool isFileAttached
        {
            get
            {
                return (sourceFile != null);
            }
        }

        /// <summary>
        /// Clears all entries, optionally clears the associated file (via <see cref="Load(string)"/> or <see cref="Save(string)"/> methods)
        /// </summary>
        /// <param name="clearFile">if set to <c>true</c> [clear file].</param>
        public void Clear(bool clearFile = false)
        {
            byKeys.Clear();
            byValues.Clear();
            if (clearFile)
            {
                if (sourceFile != null)
                {
                    sourceFile.contentLines.Clear();
                    sourceFile.Save();
                }
            }
        }

        /// <summary>
        /// Loads the table from the specified filepath
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        public void Load(string filepath, ILogBuilder loger)
        {
            sourceFile = new fileunit(filepath, true);
            int cl = sourceFile.contentLines.Count;
            loger.log("Lexicon terms coding twins definitions: " + cl);

            int lind = 0;
            int lmax = cl / 20;
            lind = lmax;
            LoadCount = 0;
            foreach (string ln in sourceFile.contentLines)
            {
                lind--;

                SetEntryFromString(ln);
                LoadCount++;
                if (lind <= 0)
                {
                    lind = lmax;
                    loger.log("Coding twins loaded: " + LoadCount);
                }
            }

            loger.log("Coding twins completly loaded: " + LoadCount);
        }

        /// <summary>
        /// Appends the file with the entries of the table
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <exception cref="aceCommonTypes.core.exceptions.dataException">The source file was never defined - cant use save without filepath - null - Save() failed, no filepath</exception>
        public void Append(string filepath = null)
        {
            if (sourceFile == null)
            {
                if (imbSciStringExtensions.isNullOrEmptyString(filepath)) throw new dataException("The source file was never defined - cant use save without filepath", null, this, "Save() failed, no filepath");

                sourceFile = new fileunit(filepath, true);
            }

            List<string> entryList = GetEntriesAsStringPairs();
            sourceFile.AppendUnique(entryList);
            sourceFile.Save();
        }

        /// <summary>
        /// Saves the table to the specified filepath, overwriting the file content
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <exception cref="aceCommonTypes.core.exceptions.dataException">The source file was never defined - cant use save without filepath - null - Save() failed, no filepath</exception>
        public void Save(string filepath = null)
        {
            if (sourceFile == null)
            {
                if (imbSciStringExtensions.isNullOrEmptyString(filepath))
                {
                    throw new dataException("The source file was never defined - cant use save without filepath", null, this, "Save() failed, no filepath");
                }

                sourceFile = new fileunit(filepath, true);
            }

            List<string> entryList = GetEntriesAsStringPairs();
            sourceFile.contentLines.Clear();
            sourceFile.Append(entryList, true);
        }

        /// <summary>
        /// Gets the entries as string pairs in format: <c>rečnik|dictionary</c> where | is <see cref="ENTRYSEPARATOR"/>
        /// </summary>
        /// <returns></returns>
        public List<string> GetEntriesAsStringPairs()
        {
            List<string> output = new List<string>();
            foreach (string key in byKeys.Keys)
            {
                output.Add(GetEntryAsStringPair(key));
            }
            return output;
        }

        /// <summary>
        /// Gets the entries as string pairs, separated by <see cref="Environment.NewLine"/>
        /// </summary>
        /// <returns></returns>
        public string GetEntriesAsString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in byKeys.Keys)
            {
                sb.AppendLine(GetEntryAsStringPair(key));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the entry as string pair like: <c>rečnik|dictionary</c> either the <c>key</c> found as Key or Value in the table. Returns empty string if not found and autoconversion wasn't enabled
        /// </summary>
        /// <param name="key">The key or value to get pair for</param>
        /// <returns>Gets the entry as string pair like: <c>rečnik|dictionary</c> either the <c>key</c> found as Key or Value in the table. Returns empty string if not found and autoconversion wasn't enabled</returns>
        public string GetEntryAsStringPair(string key)
        {
            translationTextTableEntryEnum entry = checkForEntry(key);
            if (entry == translationTextTableEntryEnum.keyEntry)
            {
                return key + ENTRYSEPARATOR + byKeys[key];
            }
            else if (entry == translationTextTableEntryEnum.valueEntry)
            {
                return byKeys[key] + ENTRYSEPARATOR + key;
            }
            else if (entry == translationTextTableEntryEnum.unknownEntry)
            {
                if (isAutoConversionEnabled)
                {
                    return key + ENTRYSEPARATOR + conversionMethod(key);
                }
            }
            return "";
        }

        /// <summary>
        ///
        /// </summary>
        public int LoadCount { get; set; }

        /// <summary>
        /// Sets the entries from string containing pairs like: <c>pasoš|pasos</c> separated by <see cref="Environment.NewLine"/>
        /// </summary>
        /// <param name="entryPairs">The entry pairs.</param>
        /// <returns>Number of new entries set</returns>
        public int SetEntriesFromString(string entryPairs)
        {
            int c = 0;
            List<string> input = new List<string>();
            input.AddRange(imbSciStringExtensions.SplitSmart(entryPairs, Environment.NewLine, "", true));
            foreach (string en in input)
            {
                var env = SetEntryFromString(en);
                if (env == translationTextTableEntryEnum.newEntry) c++;
            }
            return c;
        }

        /// <summary>
        /// Sets the entry from string pair like: <c>štetočina|stetocina</c> if <c>štetočina</c> not already defined. It will recognize automatically if its not single line but multiple lines
        /// </summary>
        /// <param name="entryLine">The entry line.</param>
        /// <returns><see cref="translationTextTableEntryEnum.newEntry"/> if new entry was created, <see cref="translationTextTableEntryEnum.none"/> if there was problem in the format, other if the entry was found</returns>
        public translationTextTableEntryEnum SetEntryFromString(string entryLine)
        {
            if (imbSciStringExtensions.isNullOrEmptyString(entryLine)) return translationTextTableEntryEnum.none;

            if (entryLine.Contains(Environment.NewLine))
            {
                int c = SetEntriesFromString(entryLine);
                if (c > 0) return translationTextTableEntryEnum.newEntry;
            }

            string[] stp = entryLine.Split(new char[] { ENTRYSEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

            if (stp.Count() < 2) return translationTextTableEntryEnum.none;

            string k = stp[0].Trim();
            string v = stp[1].Trim();

            translationTextTableEntryEnum entry = checkForEntry(k);
            if ((entry == translationTextTableEntryEnum.unknownEntry || entry == translationTextTableEntryEnum.valueEntry))
            {
                Add(k, v);
                entry = translationTextTableEntryEnum.newEntry;
            }
            else
            {
            }
            return entry;
        }

        /// <summary>
        /// Gets the <see cref="String"/> with the specified key - if not found as key then it searches the values and returns key, otherwise returns null
        /// </summary>
        /// <value>
        /// The <see cref="String"/>.
        /// </value>
        /// <param name="key">The key, either it is Key or Value in the dictionary</param>
        /// <returns>null if not found, value or key </returns>
        private string this[string key]
        {
            get
            {
                if (byKeys.ContainsKey(key))
                {
                    return byKeys[key];
                }
                if (byValues.ContainsKey(key))
                {
                    return byValues[key];
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the word pair either the specified <c>key</c> was found as Key or Value, otherwise returns the <c>key</c> or makes auto conversion if <see cref="isAutoConversionEnabled" /> i.e. <see cref="conversionMethod" /> provided with <see cref="translationTextTable" /> constructor
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="oentry">Provides insight how word was found/converted</param>
        /// <returns></returns>
        public string GetWord(string key, out translationTextTableEntryEnum oentry)
        {
            translationTextTableEntryEnum entry = checkForEntry(key);
            oentry = entry;

            if (entry == translationTextTableEntryEnum.keyEntry)
            {
                return byKeys[key];
            }
            else if (entry == translationTextTableEntryEnum.valueEntry)
            {
                return byValues[key];
            }
            else if (entry == translationTextTableEntryEnum.unknownEntry)
            {
                if (isAutoConversionEnabled)
                {
                    /*
                    String v = conversionMethod(key);
                    String k = key;
                    Add(k, v);
                    */
                    SetWord(key);
                    return byKeys[key];
                }
            }
            return key;
        }

        private object SetWordLock = new object();

        /// <summary>
        /// Sets the word using autoconversion method, if enabled
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public translationTextTableEntryEnum SetWord(string key)
        {
            lock (SetWordLock)
            {
                if (!isAutoConversionEnabled)
                {
                    throw new dataException("Can't set word with SetWord(" + key + ") method because the conversion method was not specified");
                }
                translationTextTableEntryEnum entry = checkForEntry(key);
                if (entry == translationTextTableEntryEnum.unknownEntry || entry == translationTextTableEntryEnum.valueEntry)
                {
                    if (isAutoConversionEnabled)
                    {
                        string k = key;
                        string v = conversionMethod(key);
                        if (k != v)
                        {
                            Add(key, conversionMethod(key));
                            return translationTextTableEntryEnum.newEntry;
                        }
                        else
                        {
                            return translationTextTableEntryEnum.none;
                        }
                    }
                }
            }
            return translationTextTableEntryEnum.none;
        }

        /// <summary>
        /// Translates the specified input replacing <c>key</c> with <c>value</c>, uses the complete table
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="inverse">if set to <c>true</c> [inverse].</param>
        /// <returns></returns>
        public string translate(string input, bool inverse = false)
        {
            if (imbSciStringExtensions.isNullOrEmptyString(input)) return "";

            var disc = byKeys;
            if (inverse) disc = byValues;

            foreach (var pair in disc)
            {
                input = input.Replace(pair.Key, pair.Value);
            }

            return input;
        }
    }
}