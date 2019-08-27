using imbSCI.DataExtraction.SourceData;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.MetaTables.Descriptors
{
    [Flags]
    public enum MetaTableFormatType
    {
        unknown = 0,

        /// <summary>
        /// Entries are in columns, properties are in rows
        /// </summary>
        vertical = 1,

        /// <summary>
        /// The horizontal: properties are in columns, entries are in rows
        /// </summary>
        horizontal = 2
    }
}