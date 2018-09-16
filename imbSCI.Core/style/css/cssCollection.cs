// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cssCollection.cs" company="imbVeles" >
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
using imbSCI.Core.files.folders;
using imbSCI.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.Core.style.css
{
    /// <summary>
    /// Represents collection of <see cref="cssEntryDefinition"/>
    /// </summary>
    public class cssCollection : IEnumerable<KeyValuePair<String, cssEntryDefinition>>
    {
        /// <summary>
        /// Policy on managing entries that already exist
        /// </summary>
        public enum cssEntryPolicy
        {
            /// <summary>
            /// Replace completly existing entry
            /// </summary>
            replace,

            /// <summary>
            /// The set if not existing
            /// </summary>
            setIfNotExisting,

            /// <summary>
            /// Merge with existing entry: adding new attributes and replaces the overlapping attributes
            /// </summary>
            merge
        }

        /// <summary>
        /// The regex entryselector: group[1] is name, group[2] contains the attribute pairs
        /// </summary>
        public static Regex REGEX_ENTRYSELECTOR = new Regex("([\\.#]?[\\s\\,\\w\\d\\-_]*)\\s*\\{([\\[\\]\\#\\(\\)\\n\\r\\w\\d\\.\\'\\`\\,\\%\\\"\\*\\:\\-\\;\\s\\!\\?/\\+\\@]*)\\}");

        /// <summary>
        /// The format entryformat
        /// </summary>
        public static String FORMAT_ENTRYFORMAT = "{0} {\r\n{1}\r\n}";

        /// <summary>
        /// Gets or sets the <see cref="cssEntryDefinition"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="cssEntryDefinition"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public cssEntryDefinition this[String key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Set(value);
            }
        }

        /// <summary>
        /// Gets css entry definition for item with <c>name</c>. Name may have class or id prefix: . or #. If it has multiple names, separated by comma: it will select the first
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public cssEntryDefinition Get(String name)
        {
            if (name.isNullOrEmpty()) return null;

            if (name.Contains(","))
            {
                name = name.SplitSmart(",", "", true, true).First();
            }

            if (!items.ContainsKey(name))
            {
                return null;
            }

            return items[name];
        }

        /// <summary>
        /// Sets the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="policy">The policy.</param>
        public void Set(cssEntryDefinition entry, cssEntryPolicy policy = cssEntryPolicy.replace)
        {
            if (items.ContainsKey(entry.name))
            {
                switch (policy)
                {
                    case cssEntryPolicy.replace:
                        items.Remove(entry.name);
                        items.Add(entry.name, entry);
                        break;

                    case cssEntryPolicy.setIfNotExisting:
                        break;

                    case cssEntryPolicy.merge:
                        items[entry.name].Merge(entry);
                        break;
                }
            }
            else
            {
                items.Add(entry.name, entry);
            }
        }

        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="value">The value.</param>
        public void Set(String name, String attributeName, String value)
        {
            cssEntryDefinition entry = Get(name);
            if (entry == null)
            {
                entry = new cssEntryDefinition();
                entry.name = name;
            }

            entry.Set(attributeName, value);
        }

        /// <summary>
        /// Loads the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        public Boolean Load(String path, cssEntryPolicy policy = cssEntryPolicy.replace)
        {
            if (File.Exists(path))
            {
                String s = File.ReadAllText(path);
                FromString(s, policy);
                return true;
            }
            return false;
        }

        public Boolean Load(folderNode folder, String filename, cssEntryPolicy policy = cssEntryPolicy.replace)
        {
            String p = folder.findFile("conceptual.css", SearchOption.TopDirectoryOnly);
            return Load(p, policy);
        }

        public static cssCollection LoadFile(String path, cssEntryPolicy policy = cssEntryPolicy.replace)
        {
            cssCollection output = new cssCollection();
            output.Load(path, policy);
            return output;
        }

        public static cssCollection LoadFile(folderNode folder, String filename, cssEntryPolicy policy = cssEntryPolicy.replace)
        {
            cssCollection output = new cssCollection();

            String p = folder.findFile(filename, SearchOption.TopDirectoryOnly);
            output.Load(p, policy);
            return output;
        }

        /// <summary>
        /// Phrases CSS string source code
        /// </summary>
        /// <param name="cssCode">The CSS code.</param>
        /// <param name="policy">The policy.</param>
        public void FromString(String cssCode, cssEntryPolicy policy = cssEntryPolicy.replace)
        {
            var ms = REGEX_ENTRYSELECTOR.Matches(cssCode);

            foreach (Match m in ms)
            {
                foreach (String n in m.Groups[1].Value.SplitSmart(",", "", true, true))
                {
                    cssEntryDefinition entry = new cssEntryDefinition();
                    entry.name = n;
                    entry.fromString(m.Groups[2].Value, cssEntryDefinition.syntaxFormat.cssFileFormat);
                    Set(entry, policy);
                }
            }
        }

        /// <summary>
        /// To the string.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public String ToString(cssEntryDefinition.syntaxFormat format = cssEntryDefinition.syntaxFormat.cssFileFormat)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<String, cssEntryDefinition> entry in items)
            {
                sb.AppendLine(entry.Value.ToString(format));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Saves the css collection to specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Save(String path)
        {
            var s = ToString(cssEntryDefinition.syntaxFormat.cssFileFormat);

            File.WriteAllText(path, s);
        }

        /// <summary>
        /// Saves the specified folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="description">The description.</param>
        public String Save(folderNode folder, String filename, String description = "")
        {
            filename = filename.ensureEndsWith(".css");

            var path = folder.pathFor(filename, Data.enums.getWritableFileMode.newOrExisting, description, true);

            Save(path);

            return path;
        }

        public IEnumerator<KeyValuePair<string, cssEntryDefinition>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, cssEntryDefinition>>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, cssEntryDefinition>>)items).GetEnumerator();
        }

        /// <summary>
        /// Collection of preset definitions
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public Dictionary<String, cssEntryDefinition> items { get; protected set; } = new Dictionary<string, cssEntryDefinition>();
    }
}