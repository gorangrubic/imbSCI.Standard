// using BibtexLibrary;
using imbSCI.Core.extensions.table;
using imbSCI.Core.reporting;
using imbSCI.DataComplex.special;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.BibTex
{
    /// <summary>
    /// Object model twin of <see cref="BibTexDataFile"/>, contains <see cref="BibTexEntryModel"/> instances
    /// </summary>
    /// <typeparam name="T">Custom, expanded class of the basic entry object model</typeparam>
    /// <seealso cref="System.Collections.Generic.List{T}" />
    public class BibTexCollection<T> : List<T> where T : BibTexEntryModel, new()
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String name { get; set; }

        /// <summary>
        /// Gets the entries, indexed by <see cref="BibTexEntryModel.EntryKey"/>
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, BibTexEntryModel> GetIndex()
        {
            Dictionary<String, BibTexEntryModel> output = new Dictionary<string, BibTexEntryModel>();

            foreach (BibTexEntryModel bib in this)
            {
                if (!output.ContainsKey(bib.EntryKey))
                {
                    output.Add(bib.EntryKey, bib);
                }
            }

            return output;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BibTexCollection{T}"/> class.
        /// </summary>
        /// <param name="_name">The name.</param>
        /// <param name="entries">The entries.</param>
        public BibTexCollection(String _name, IEnumerable<BibTexEntryBase> entries)
        {
            name = _name;
            Add(entries, false);
        }

        /// <summary>
        /// Generates BibTex source
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        public String GetSource(translationTextTable table, ILogBuilder log = null)
        {
            StringBuilder sb = new StringBuilder();

            foreach (BibTexEntryModel emodel in this)
            {
                sb.AppendLine(emodel.GetEntry(table, log).GetSource(table));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the data file object model
        /// </summary>
        /// <param name="filename">The filename, without extension</param>
        /// <param name="table">LaTeX translation table</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        public BibTexDataFile GetDataFile(String filename, translationTextTable table, ILogBuilder log = null)
        {
            BibTexDataFile file = new BibTexDataFile();
            file.name = filename;
            foreach (BibTexEntryModel emodel in this)
            {
                file.UntypedEntries.Add(emodel.GetEntry(table, log));
            }
            return file;
        }

        /// <summary>
        /// Adds the specified entries.
        /// </summary>
        /// <param name="entries">The entries.</param>
        /// <param name="ReplaceOnSameKey">if set to <c>true</c> [replace on same key].</param>
        public void Add(IEnumerable<BibTexEntryBase> entries, Boolean ReplaceOnSameKey = true)
        {
            var index = GetIndex();
            List<BibTexEntryModel> newItems = new List<BibTexEntryModel>();
            List<String> newKeys = new List<string>();

            foreach (BibTexEntryBase bib in entries)
            {
                if (!ReplaceOnSameKey)
                {
                    if (index.ContainsKey(bib.Key)) continue;
                    if (newKeys.Contains(bib.Key)) continue;
                }
                else
                {
                    if (newKeys.Contains(bib.Key))
                    {
                        var toRemove = this.Where(x => x.EntryKey == bib.Key).ToList();
                        toRemove.ForEach(x => this.Remove(x));
                    }

                    T entry = new T();
                    entry.SetFromEntry(bib);
                    newKeys.Add(entry.EntryKey);
                    Add(entry as T);
                }
            }
        }
    }
}