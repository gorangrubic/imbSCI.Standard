using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.table;
using imbSCI.Core.math.range.frequency;
using imbSCI.Data;
using imbSCI.DataComplex.extensions.data.operations;
using imbSCI.DataComplex.extensions.data.schema;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.SourceData.Analysis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.DataExtraction.MetaTables.Descriptors
{
[Serializable]
    public class MetaTablePropertyAliasList
    {
        public static Regex StringInputFormat { get; set; } = new Regex(@"([a-zA-Z_0123456789\-]+)");

        /// <summary>
        /// Adds the entries specified in input string. Returns created entries
        /// </summary>
        /// <param name="multiLineAliasList">The multi line alias list.</param>
        /// <returns></returns>
        public List<MetaTablePropertyAliasEntry> AddEntries(String multiLineAliasList)
        {
            List<MetaTablePropertyAliasEntry> output = new List<MetaTablePropertyAliasEntry>();
            List<String> lines = new List<string>();
            lines.AddRange(multiLineAliasList.SplitSmart(Environment.NewLine));

            foreach (String ln in lines)
            {
                var e = AddEntry(ln);
                if (e != null)
                {
                    output.Add(e);
                }
            }

            return output;
        }

        /// <summary>
        /// Adds entry from string input. Returns newly created entry or null if no entry created
        /// </summary>
        /// <param name="commaSeparatedAlias">The comma separated alias.</param>
        /// <returns></returns>
        public MetaTablePropertyAliasEntry AddEntry(String commaSeparatedAlias)
        {
            List<String> al = new List<string>();
            foreach (Match m in StringInputFormat.Matches(commaSeparatedAlias))
            {
                if (!m.Value.isNullOrEmpty())
                {
                    al.Add(m.Value);
                }
            }
            if (al.Any())
            {
                MetaTablePropertyAliasEntry output = new MetaTablePropertyAliasEntry(al);
                items.Add(output);
                return output;
            }
            else
            {
                return null;
            }
        }
        
         public List<MetaTablePropertyAliasEntry> MatchQuery(String query, Boolean asPlaceholders = false)
        {
            if (!asPlaceholders)
            {
                List<MetaTablePropertyAliasEntry> output = items.Where(x => query.Contains(x.rootPropertyName)).ToList();

                return output;
            } else
            {
                List<MetaTablePropertyAliasEntry> output = items.Where(x => query.Contains("{{" + x.rootPropertyName + "}}")).ToList();

                return output;
            }
        }

        public MetaTablePropertyAliasEntry Match(String propertyName, Boolean createPseudoEntry = false)
        {
            MetaTablePropertyAliasEntry output = items.FirstOrDefault(x => x.isMatch(propertyName));

            if (output == null)
            {
                if (createPseudoEntry)
                {
                    output = new MetaTablePropertyAliasEntry(new String[] { propertyName });
                    items.Add(output);
                }
            }

            return output;
        }

        public MetaTablePropertyAliasList()
        {
        }

        public List<MetaTablePropertyAliasEntry> items { get; set; } = new List<MetaTablePropertyAliasEntry>();
    }
}