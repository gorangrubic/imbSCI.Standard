using imbSCI.Core.files.folders;
using imbSCI.Data;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.SourceData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace imbSCI.DataExtraction.Extractors.Task
{

    [Serializable]
    public class TaskPropertyEntry
    {
        public TaskPropertyEntry()
        {

        }

        public TaskPropertyEntry(MetaTableProperty meta)
        {
            Meta = meta;
            propertyName = meta.PropertyName;
            propertyType = meta.ValueTypeName;

        }

        public String propertyName { get; set; } = "";
        public String propertyType { get; set; } = "";
        public String exampleValue { get; set; } = "";

        /// <summary>
        /// Reference to meta table property instance
        /// </summary>
        /// <value>
        /// The meta.
        /// </value>
        public MetaTableProperty Meta { get; set; } = null;


    }
}