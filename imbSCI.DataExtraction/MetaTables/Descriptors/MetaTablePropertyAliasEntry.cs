using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.table;
using imbSCI.Core.math.range.frequency;
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

namespace imbSCI.DataExtraction.MetaTables.Descriptors
{
    [Serializable]
    public class MetaTablePropertyAliasEntry
    {
        public MetaTablePropertyAliasEntry()
        {
        }

        public MetaTablePropertyAliasEntry(IEnumerable<String> aliasList)
        {
            foreach (String a in aliasList)
            {
                if (rootPropertyName.isNullOrEmpty())
                {
                    rootPropertyName = a;
                }
                else
                {
                    aliasPropertyNames.Add(a);
                }
            }
        }

        public Boolean isMatch(String name)
        {
            if (rootPropertyName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            foreach (String a in aliasPropertyNames)
            {
                if (name.Equals(a, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public String rootPropertyName { get; set; } = "";

        /// <summary>
        /// Gets or sets the alias property names.
        /// </summary>
        /// <value>
        /// The alias property names.
        /// </value>
        public List<String> aliasPropertyNames { get; set; } = new List<string>();
    }
}