using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.extensions.data;
using imbSCI.Data.interfaces;

namespace imbSCI.DataExtraction.TypeEntities
{
    public class ModelElementRelationEntry
    {
        public ModelElementRelationEntry() { }
        public String ElementA { get; set; } = "";
        public String ElementB { get; set; } = "";
    }
}