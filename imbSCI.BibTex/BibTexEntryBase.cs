using imbSCI.Core.extensions.text;
using imbSCI.DataComplex.special;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.BibTex
{
    /// <summary>
    /// BibTex untyped entry
    /// </summary>
    public class BibTexEntryBase
    {
        /// <summary>
        /// Regex select SelectTags : ^([\w]*) = \{(.*)\},?
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_isSelectTags = new Regex(@"^([\w]*) = (.*),?", RegexOptions.Compiled | RegexOptions.Multiline);

        /// <summary>
        /// Test if input matches ^([\w]*) = \{(.*)\},?
        /// </summary>
        /// <param name="input">String to test</param>
        /// <returns>IsMatch against _select_isSelectTags</returns>
        public static Boolean isSelectTags(String input)
        {
            if (String.IsNullOrEmpty(input)) return false;
            return _select_isSelectTags.IsMatch(input);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BibTexEntryBase"/> class.
        /// </summary>
        public BibTexEntryBase()
        {
            Key = imbStringGenerators.getRandomString(8);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BibTexEntryBase"/> class.
        /// </summary>
        /// <param name="_source">The source.</param>
        /// <param name="_type">The type.</param>
        /// <param name="_key">The key.</param>
        /// <param name="processor">The processor.</param>
        public BibTexEntryBase(String _source, String _type, String _key, translationTextTable processor = null)
        {
            Deploy(_source, _type, _key, processor);
        }

        private List<Char> sourceTrim = new List<char>() { '{', '}', '"', ',' };

        /// <summary>
        /// Deploys the specified source.
        /// </summary>
        /// <param name="_source">The source.</param>
        /// <param name="_type">The type.</param>
        /// <param name="_key">The key.</param>
        /// <param name="processor">The processor.</param>
        public void Deploy(String _source, String _type, String _key, translationTextTable processor = null)
        {
            type = _type;
            Key = _key;
            source = _source;

            foreach (Match mch in _select_isSelectTags.Matches(source))
            {
                String source = mch.Groups[2].Value.Trim(sourceTrim.ToArray());

                BibTexEntryTag tmp = new BibTexEntryTag(mch.Groups[1].Value, source);
                tmp.source = source;
                AddTag(tmp.Key, tmp);
            }

            ProcessSource(processor);
        }

        /// <summary>
        /// Adds the tag.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="tag">The tag.</param>
        public void AddTag(String key, BibTexEntryTag tag)
        {
            if (!Tags.ContainsKey(Key))
            {
                Tags.Add(key, tag);
            }
        }

        public const String tagFormatOther = "{0} = {{1}}";

        public const String tagFormatTitle = "{0} = {{{1}}}";

        public const String tagKey_Title = "title";

        /// <summary>
        /// Updates the source.
        /// </summary>
        /// <param name="table">The table.</param>
        public void UpdateSource(translationTextTable table = null)
        {
            List<BibTexEntryTag> _tags = new List<BibTexEntryTag>();

            foreach (var pair in Tags)
            {
                if (table != null) pair.Value.source = table.translate(pair.Value.Value, true);
                _tags.Add(pair.Value);
            }

            Tags.Clear();
            foreach (var t in _tags)
            {
                AddTag(t.Key, t);
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("@" + type + "{" + Key + ",");

            Int32 c = 0;
            foreach (var pair in Tags)
            {
                String format = tagFormatOther;

                if (pair.Key == tagKey_Title) format = tagFormatTitle;

                String line = "";

                if (table != null)
                {
                    line = String.Format(format, pair.Key, pair.Value.source);
                }
                else
                {
                    line = String.Format(format, pair.Key, pair.Value.Value);
                }

                sb.AppendLine(line);

                c++;
                if (c < Tags.Count) sb.Append(",");
            }
            sb.AppendLine("}");

            source = sb.ToString();
        }

        /// <summary>
        /// Calls <see cref="UpdateSource(translationTextTable)"/> and returns reconstructed BibTex source (<see cref="source"/>)
        /// </summary>
        /// <param name="table">The LaTex entity translation table</param>
        /// <returns>BibTex source</returns>
        public String GetSource(translationTextTable table)
        {
            UpdateSource(table);
            return source;
        }


        /// <summary>
        /// Processes the source.
        /// </summary>
        /// <param name="table">The table.</param>
        public void ProcessSource(translationTextTable table = null)
        {
            if (table == null) return;
            foreach (var pair in Tags)
            {
                pair.Value.Value = table.translate(pair.Value.source);
            }
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public String source { get; set; } = "";

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string type { get; set; } = "article";

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; } = "";

        /// <summary>
        /// Tegs tag value for specified <c>_key</c>, if not defined, returns empty string
        /// </summary>
        /// <param name="_key">The key.</param>
        /// <returns></returns>
        public String Get(String _key)
        {
            if (Tags.ContainsKey(_key)) return Tags[_key].Value;
            return "";
        }

        /// <summary>
        /// The tags
        /// </summary>
        private Dictionary<string, BibTexEntryTag> _tags = new Dictionary<string, BibTexEntryTag>();

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public Dictionary<string, BibTexEntryTag> Tags
        {
            get { return _tags; }
            set { _tags = value; }
        }
    }
}