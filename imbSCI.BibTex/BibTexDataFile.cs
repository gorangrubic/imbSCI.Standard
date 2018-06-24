// using BibtexLibrary;
using imbSCI.Core.extensions.table;
using imbSCI.Core.reporting;
using imbSCI.Core.style.color;
using imbSCI.Core.style.preset;
using imbSCI.DataComplex.special;

//using imbSCI.DataComplex.tables;
//using imbSCI.DataComplex.tables.extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.BibTex
{
    /// <summary>
    /// BibTex data with load and export methods
    /// </summary>
    public class BibTexDataFile
    {
        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        /// <value>
        /// The source path.
        /// </value>
        public String sourcePath { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BibTexDataFile"/> class.
        /// </summary>
        public BibTexDataFile()
        {
            Deploy();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BibTexDataFile"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public BibTexDataFile(String path)
        {
            Deploy();
            Load(path, null);
        }

        private void Deploy()
        {
            AddField(typeColumn);
            AddField(keyColumn);
        }

        /// <summary>
        /// All fields used by the entries
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public List<String> fields { get; protected set; } = new List<string>();

        /// <summary>
        /// Converts all <see cref="BibTexEntryBase"/> from the file, into dictionary. In case of repeated <see cref="BibTexEntryModel.EntryKey"/>, only the first is part of returned dictionary.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        public BibTexCollection<T> ConvertToModel<T>(ILogBuilder log = null) where T : BibTexEntryModel, new()
        {
            BibTexCollection<T> output = new BibTexCollection<T>(name, UntypedEntries);

            return output;
        }

        /// <summary>
        /// Builds data table - from BibTex entries
        /// </summary>
        /// <param name="tagsFilter">List of tag names to include as columns. Include all found, if null</param>
        /// <returns></returns>
        public DataTable ConvertToDataTable(List<String> tagsFilter = null, propertyAnnotationPreset preset = null, ILogBuilder log = null)
        {
            DataTable output = new DataTable();
            output.SetTitle(name);

            List<String> fieldsToUse = new List<string>();
            if (tagsFilter == null)
            {
                fieldsToUse.AddRange(fields);
            }
            else
            {
                foreach (String field in fields)
                {
                    if (tagsFilter.Contains(field)) fieldsToUse.Add(field);
                }
            }

            foreach (String field in fieldsToUse)
            {
                DataColumn dc = output.Columns.Add(field);

              //  dc.SetDefaultBackground(ColorWorks.ColorLightGray);
            }

            if (preset != null)
            {
                output.SetAdditionalInfoEntry("Format template", preset.name);
                preset.DeployTo(output, false, log);
            }

            foreach (BibTexEntryBase ent in UntypedEntries)
            {
                var row = output.NewRow();

                foreach (String field in fieldsToUse)
                {
                    if (field == typeColumn)
                    {
                        row[typeColumn] = ent.type;
                    }
                    else if (field == keyColumn)
                    {
                        row[keyColumn] = ent.Key;
                    }
                    else
                    {
                        row[field] = ent.Get(field);
                    }
                }

                output.Rows.Add(row);
            }

            output.SetDescription("BibTex bibliography entries, converted to table using imbSCI.Tools library");

            output.SetAdditionalInfoEntry("Source file", sourcePath);
            output.SetAdditionalInfoEntry("Conversion date", DateTime.Now.ToShortDateString());

            return output;
        }

        /// <summary>
        /// Gets BibTex source code
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        public String GetSource(translationTextTable table, ILogBuilder log = null)
        {
            StringBuilder sb = new StringBuilder();

            foreach (BibTexEntryBase entry in UntypedEntries)
            {
                sb.AppendLine(entry.GetSource(table));
            }

            return sb.ToString();
        }


        /// <summary>
        /// Adds the field.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        private void AddField(String tokenKey)
        {
            if (!fields.Contains(tokenKey))
            {
                fields.Add(tokenKey);
            }
        }

        public const String typeColumn = "EntryType";
        public const String keyColumn = "EntryKey";

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public BibTexLoadSettings settings { get; set; } = new BibTexLoadSettings();

        /// <summary>
        /// Regex select BibTexEntryStart : ^@{1}([\w]+)\{([\w\d]+),
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_isBibTexEntryStart = new Regex(@"^@{1}([\w]+)\{([\w\d]+),", RegexOptions.Compiled | RegexOptions.Multiline);

        /// <summary>
        /// Test if input matches ^@{1}([\w]+)\{([\w\d]+),
        /// </summary>
        /// <param name="input">String to test</param>
        /// <returns>IsMatch against _select_isBibTexEntryStart</returns>
        public static Boolean isBibTexEntryStart(String input)
        {
            if (String.IsNullOrEmpty(input)) return false;
            return _select_isBibTexEntryStart.IsMatch(input);
        }

        /// <summary>
        /// Regex select SplitEntries : ^@
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_SplitEntries = new Regex(@"^@", RegexOptions.Compiled | RegexOptions.Multiline);

        /// <summary>
        /// Regex select keyAndTypeSelectionName : ^([\w]*)\{([\w\d]*)
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_keyAndTypeSelection = new Regex(@"^([\w]*)\{([\w\d]*)", RegexOptions.Compiled);

        /// <summary>
        /// Match Evaluation for keyAndTypeSelectionName : _select_keyAndTypeSelectionName
        /// </summary>
        /// <param name="m">Match with value to process</param>
        /// <returns>For m.value "something" returns "SOMETHING"</returns>
        private static String _replace_keyAndTypeSelectionName(Match m)
        {
            String output = m.Value.Replace(".", "");
            output = output.Replace(" ", "");

            return output.ToUpper();
        }

        /// <summary>
        /// List of BibTexEntryBase instances, loaded from the source file
        /// </summary>
        public List<BibTexEntryBase> UntypedEntries { get; protected set; } = new List<BibTexEntryBase>();

        /// <summary>
        /// Loads Bibtex file from <c>path</c>
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="log">The log.</param>
        public void Load(String path, ILogBuilder log = null)
        {
            sourcePath = path;
            name = Path.GetFileNameWithoutExtension(sourcePath);
            String source = File.ReadAllText(path);
            LoadSource(source, log);
        }

        /// <summary>
        /// Saves data from the instance into BibTex file
        /// </summary>
        /// <param name="path">The path, to save the data into</param>
        /// <param name="processorTable">LaTeX entities translation table</param>
        /// <param name="log">The log.</param>
        public void Save(String path, translationTextTable processorTable = null, ILogBuilder log = null)
        {
            String source = GetSource(processorTable, log);
            File.WriteAllText(path, source);
        }

        /// <summary>
        /// Loads Bibtex entries from the source code
        /// </summary>
        /// <param name="source">The BibTex string source code</param>
        /// <param name="log">The log.</param>
        public void LoadSource(String source, ILogBuilder log = null)
        {
            BibTexSourceProcessor processor = new BibTexSourceProcessor();
            translationTextTable processorTable = processor.latex;

            var sourceParts = _select_SplitEntries.Split(source);
            foreach (String entrySource in sourceParts)
            {
                Match mch = _select_keyAndTypeSelection.Match(entrySource);
                if (mch.Success)
                {
                    var lastType = mch.Groups[1].Value;
                    var lastKey = mch.Groups[2].Value;
                    var bEB = new BibTexEntryBase(entrySource, lastType, lastKey, processorTable);

                    UntypedEntries.Add(bEB);
                }
            }

            foreach (var bED in UntypedEntries)
            {
                foreach (BibTexEntryTag bEDt in bED.Tags.Values)
                {
                    if (!fields.Contains(bEDt.Key))
                    {
                        fields.Add(bEDt.Key);
                    }
                }
            }

            if (log != null) log.log("BibTex segments [" + UntypedEntries.Count + "] ready for parsing.");
        }

        /// <summary>
        /// Gets the bib tex code.
        /// </summary>
        /// <param name="processed">if set to <c>true</c> [processed].</param>
        /// <param name="processorTable">The processor table.</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        public String GetBibTexCode(Boolean processed = false, translationTextTable processorTable = null, ILogBuilder log = null)
        {
            StringBuilder sb = new StringBuilder();

            if (processorTable == null) processed = true;
            if (processed == true) processorTable = null;

            foreach (var entry in UntypedEntries)
            {
                entry.UpdateSource(processorTable);
                sb.AppendLine(entry.source);
            }

            return sb.ToString();
        }
    }
}