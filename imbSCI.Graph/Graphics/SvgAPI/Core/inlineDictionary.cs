using imbSCI.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.Graph.Graphics.SvgAPI.Core
{
    /// <summary>
    /// Dictionary maintaining style inline data/parameters - used by SVG, mxGraph, HTML and other XML based outputs
    /// </summary>
    public class inlineDictionary : IEnumerable<KeyValuePair<String, String>>
    {
        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(String key)
        {
            if (parameters.ContainsKey(key)) parameters.Remove(key);
        }

        /// <summary>
        /// Universal get and set indexer
        /// </summary>
        /// <value>
        /// The <see cref="String"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public String this[String key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Set(key, value);
            }
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Set(String key, String value)
        {
            if (value.isNullOrEmpty())
            {
                if (parameters.ContainsKey(key)) parameters.Remove(key);
                return;
            }
            if (!parameters.ContainsKey(key))
            {
                parameters.Add(key, value);
            }
            else
            {
                parameters[key] = value;
            }
        }

        /// <summary>
        /// Appends value at specified key.(or adds new entry)
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="separator">The separator - added between existing and new content.</param>
        public void Append(String key, String value, String separator = " ")
        {
            String tmp = "";
            if (parameters.ContainsKey(key))
            {
                tmp = parameters[key];
            }
            tmp = tmp.add(value, separator);
            Set(key, tmp);
        }

        public Boolean ContainsKey(String key)
        {
            return parameters.ContainsKey(key);
        }

        /// <summary>
        /// Universal get
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public String Get(String key)
        {
            if (parameters.ContainsKey(key))
            {
                return parameters[key];
            }
            return "";
        }

        public Boolean Any
        {
            get
            {
                return parameters.Any();
            }
        }

        /// <summary>
        /// Format set as default for this dictionary
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        public styleFormat mainFormat { get; set; } = styleFormat.htmlStyleFormat;

        /// <summary>
        /// In-line string format
        /// </summary>
        public enum styleFormat
        {
            notSet,

            /// <summary>
            /// The HTML style format: [property]:[value];
            /// </summary>
            htmlStyleFormat,

            /// <summary>
            /// The mx graph style format: [property]=[value];
            /// </summary>
            mxGraphStyleFormat,

            xmlAttribute
        }

        /// <summary>
        /// Gets the key value splitter.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static String GetKeyValueSplitter(styleFormat format)
        {
            switch (format)
            {
                default:
                case styleFormat.htmlStyleFormat:
                    return ":";
                    break;

                case styleFormat.mxGraphStyleFormat:
                    return "=";
                    break;

                case styleFormat.xmlAttribute:
                    return "";
                    break;
            }
        }

        /// <summary>
        /// Gets the entry splitter.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static String GetEntrySplitter(styleFormat format)
        {
            switch (format)
            {
                default:
                case styleFormat.htmlStyleFormat:
                case styleFormat.mxGraphStyleFormat:
                    return ";";
                    break;

                case styleFormat.xmlAttribute:
                    return "";
                    break;
            }
        }

        public static Regex REGEX_HTMLSTYLE = new Regex("\\b([\\w]+):([#\\d\\w\\s\\%\\.\\,\\-\\(\\)\\?\\!\\=]*);");

        public static Regex REGEX_XMGRAPH = new Regex("\\b([\\w]+)=([#\\d\\w\\s\\%\\.\\,\\-\\:\\(\\)\\?\\!]*);");

        public static Regex REGEX_XmlAttribute = new Regex("\\b([\\w]+)=\\\"([\\d\\w\\s\\%\\.\\,\\-\\:\\;\\(\\)\\?\\!\\=]*)\\\"\\s?");

        /// <summary>
        /// Gets the pair regex.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static Regex GetPairRegex(styleFormat format)
        {
            switch (format)
            {
                default:
                case styleFormat.xmlAttribute:
                    return REGEX_XmlAttribute;
                    break;

                case styleFormat.htmlStyleFormat:
                    return REGEX_HTMLSTYLE;
                    break;

                case styleFormat.mxGraphStyleFormat:
                    return REGEX_XMGRAPH;
                    break;
            }
        }

        /// <summary>
        /// Gets the pair format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static String GetPairFormat(styleFormat format)
        {
            switch (format)
            {
                default:
                case styleFormat.htmlStyleFormat:
                    return "{0}:{1};";
                case styleFormat.mxGraphStyleFormat:
                    return "{0}={1};";
                    break;

                case styleFormat.xmlAttribute:
                    return "{0}=\"{1}\" ";
                    break;
            }
        }

        /// <summary>
        /// Dictionary for parameters given as inline string
        /// </summary>
        public inlineDictionary()
        {
        }

        public inlineDictionary(styleFormat format)
        {
            mainFormat = format;
        }

        public const Boolean USE_REGEX_FROMSTRING = true;

        /// <summary>
        /// Populates parameters from inline string values, given in <see cref="styleFormat"/>
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="format">The format.</param>
        public void fromString(String input, styleFormat format = styleFormat.notSet)
        {
            if (format == styleFormat.notSet) format = mainFormat;

            if (USE_REGEX_FROMSTRING)
            {
                var rx = GetPairRegex(format);
                MatchCollection mch = rx.Matches(input);

                foreach (Match m in mch)
                {
                    if (m.Success)
                    {
                        Set(m.Groups[1].Value, m.Groups[2].Value);
                    }
                }
            }
            else
            {
                var entries = input.SplitSmart(GetEntrySplitter(format), "", true, true);
                foreach (var n in entries)
                {
                    var pair = n.SplitSmart(GetKeyValueSplitter(format), "", true, true);
                    if (parameters.ContainsKey(pair[0]))
                    {
                        parameters[pair[0]] = pair[1];
                    }
                    else
                    {
                        parameters.Add(pair[0], pair[1]);
                    }
                }
            }
        }

        /// <summary>
        /// Parameters to inline representation
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public String ToString(styleFormat format = styleFormat.notSet)
        {
            if (format == styleFormat.notSet) format = mainFormat;
            StringBuilder sb = new StringBuilder();
            String formatLine = GetPairFormat(format);

            foreach (var pair in parameters)
            {
                sb.AppendFormat(formatLine, pair.Key, pair.Value);
            }
            return sb.ToString();
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, string>>)parameters).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, string>>)parameters).GetEnumerator();
        }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        protected Dictionary<String, String> parameters { get; set; } = new Dictionary<string, string>();
    }
}