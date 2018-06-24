// // using BibtexLibrary;
using imbSCI.Core.attributes;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.reporting;
using imbSCI.Core.style.color;
using imbSCI.DataComplex.special;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace imbSCI.BibTex
{
    /// <summary>
    /// Strong typed object model, with data from/for <see cref="BibTexEntryBase"/>. Inherit this class to introduce support for additional BibTex tags.
    /// </summary>
    [imb(imbAttributeName.basicColor, ColorWorks.ColorLightGray)]
    [imb(imbAttributeName.reporting_categoryOrder, "Entry,Topic,Publication")]
    public class BibTexEntryModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BibTexEntryModel"/> class.
        /// </summary>
        public BibTexEntryModel()
        {
            EntryKey = imbStringGenerators.getRandomString(8);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BibTexEntryModel"/> class.
        /// </summary>
        /// <param name="entryKey">The entry key.</param>
        /// <param name="entryType">Type of the entry.</param>
        public BibTexEntryModel(String entryKey, String entryType)
        {
            EntryKey = entryKey;
            EntryType = entryType;
        }

        /// <summary>
        /// Constructs object instance with data from the entry. To enable loging, use <see cref="BibTexEntryModel()"/> and <see cref="SetFromEntry(BibTexEntryBase, ILogBuilder)"/> instead
        /// </summary>
        /// <param name="entry">The entry.</param>
        public BibTexEntryModel(BibTexEntryBase entry)
        {
            EntryKey = entry.Key;
            EntryType = entry.type;

            SetFromEntry(entry);
        }

        /// <summary>
        /// Gets BibTex source, using specified <c>translationTextTable</c> to convert UTF-8 strings into proper LaTeX symbols
        /// </summary>
        /// <param name="table">Table with pairs used to convert UTF-8 strings into proper LaTeX symbols</param>
        /// <param name="log">The log.</param>
        /// <returns>BibTex source code</returns>
        public String GetSource(translationTextTable table, ILogBuilder log = null)
        {
            return GetEntry(table, log).GetSource(table);
        }

        private static Object _propDictionary_lock = new object();

        private static Dictionary<String, PropertyInfo> _propDictionary;

        /// <summary>
        /// Property dictionary
        /// </summary>
        private static Dictionary<String, PropertyInfo> propDictionary
        {
            get
            {
                return _propDictionary;
            }
        }

        private void SetDictionary()
        {
            if (_propDictionary == null)
            {
                lock (_propDictionary_lock)
                {
                    if (_propDictionary == null)
                    {
                        _propDictionary = new Dictionary<String, PropertyInfo>();
                        System.Reflection.PropertyInfo[] prop = GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                        foreach (var pi in prop)
                        {
                            if (pi.PropertyType.IsValueType || pi.PropertyType.isTextOrNumber())
                            {
                                _propDictionary.Add(pi.Name, pi);
                            }
                        }

                        // add here if any additional initialization code is required
                    }
                }
            }
        }

        /// <summary>
        /// Sets object instance properties.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="log">The log - if null, log is off</param>
        public void SetFromEntry(BibTexEntryBase entry, ILogBuilder log = null)
        {
            SetDictionary();
            foreach (var tag in entry.Tags)
            {
                if (propDictionary.ContainsKey(tag.Key))
                {
                    this.imbSetPropertyConvertSafe(propDictionary[tag.Key], tag.Value.Value);
                }
                else
                {
                    if (log != null) log.log(entry.Key + "[" + tag.Key + "] - property not declared at [" + GetType().Name + "] - consider using your own class, derived from BibTexEntryModel");
                }
            }
        }

        /// <summary>
        /// Gets untyped <see cref="BibTexEntryBase"/> object, consumed for BibTex format export
        /// </summary>
        /// <param name="log">The log.</param>
        /// <returns>BibTex entry with data from this object instance</returns>
        public BibTexEntryBase GetEntry(translationTextTable process = null, ILogBuilder log = null)
        {
            SetDictionary();

            BibTexEntryBase entry = new BibTexEntryBase();
            entry.type = EntryType;
            entry.Key = EntryKey;

            foreach (var pair in propDictionary)
            {
                Object vl = this.imbGetPropertySafe(pair.Key, "");
                BibTexEntryTag tag = new BibTexEntryTag(pair.Key, vl.toStringSafe());

                entry.Tags.Add(tag.Key, tag);
            }

            entry.UpdateSource(process);

            return entry;
        }

        /// <summary> Type of BibTex entry </summary>
        [Category("Entry")]
        [DisplayName("Type")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Type of BibTex entry")] // [imb(imbAttributeName.reporting_escapeoff)]
        [imb(imbAttributeName.reporting_columnWidth, "10")]
        public String EntryType { get; set; } = "article";

        /// <summary> Mendeley Entry Key </summary>
        [Category("Entry")]
        [DisplayName("Key")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Mendeley Entry Key")] // [imb(imbAttributeName.reporting_escapeoff)]
        [imb(imbAttributeName.reporting_columnWidth, "10")]
        public String EntryKey { get; set; } = "";

        /// <summary> Entry title </summary>
        [Category("Topic")]
        [DisplayName("Title")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Entry title")] // [imb(imbAttributeName.reporting_escapeoff)]
        [imb(imbAttributeName.reporting_columnWidth, "100")]
        [imb(imbAttributeName.viewPriority, 200)]
        [imb(imbAttributeName.basicColor, ColorWorks.ColorCoolCyan)]
        public String title { get; set; } = default(String);

        [Category("Topic")]
        [DisplayName("Abstract")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("")] // [imb(imbAttributeName.reporting_escapeoff)]
        [imb(imbAttributeName.measure_letter, "a")]
        [imb(imbAttributeName.basicColor, ColorWorks.ColorCoolCyan)]
        public String @abstract { get; set; } = default(String);

        [Category("Topic")]
        [DisplayName("Keywords")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Defined keywords")] // [imb(imbAttributeName.reporting_escapeoff)]
        [imb(imbAttributeName.reporting_columnWidth, 50)]
        public String keywords { get; set; } = default(String);

        /// <summary> Authors </summary>
        [Category("Publication")]
        [DisplayName("Author")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Authors of the paper")] // [imb(imbAttributeName.reporting_escapeoff)]
        [imb(imbAttributeName.reporting_columnWidth, 50)]
        public String author { get; set; } = "";

        /// <summary> Name of publication </summary>
        [Category("Publication")]
        [DisplayName("Journal")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Name of publication")] // [imb(imbAttributeName.reporting_escapeoff)]
        [imb(imbAttributeName.reporting_columnWidth, "50")]
        public String journal { get; set; } = default(String);

        /// <summary> Number of </summary>
        [Category("Publication")]
        [DisplayName("Volume")]
        [imb(imbAttributeName.reporting_columnWidth, "10")]
        [Description("Volume of the issue")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Int32 volume { get; set; } = default(Int32);

        /// <summary> Number of </summary>
        [Category("Publication")]
        [DisplayName("Number")]
        [imb(imbAttributeName.reporting_columnWidth, "10")]
        [Description("Number of the issue")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Int32 number { get; set; } = default(Int32);

        /// <summary> Number of </summary>
        [Category("Publication")]
        [DisplayName("Pages")]
        [imb(imbAttributeName.reporting_columnWidth, "10")]
        [Description("Page range in the publication")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public String pages { get; set; } = "";

        [Category("Publication")]
        [DisplayName("Year")]
        [imb(imbAttributeName.reporting_columnWidth, "10")]
        [Description("Published")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Int32 year { get; set; } = default(Int32);

        /// <summary> Number of </summary>
        [Category("Publication")]
        [DisplayName("ISSN")]
        [imb(imbAttributeName.reporting_columnWidth, "20")]
        [Description("ISSN number")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public String issn { get; set; } = "";

        [Category("Document")]
        [DisplayName("DOI")]
        [imb(imbAttributeName.reporting_columnWidth, "40")]
        [Description("Digital Object Identified system")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public String doi { get; set; } = "";

        [Category("Document")]
        [DisplayName("Url")]
        [imb(imbAttributeName.reporting_columnWidth, "40")]
        [imb(imbAttributeName.reporting_escapeoff)]
        [Description("Url associated with the publication")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public String url { get; set; } = "";


    }
}