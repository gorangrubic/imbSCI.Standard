using imbSCI.DataExtraction.MetaEntities;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using imbSCI.Core.math.classificationMetrics;

namespace imbSCI.DataExtraction.TypeEntities
{
  /// <summary>
    /// Type Property link definition
    /// </summary>
    [Serializable]
    public class TypePropertyMapDefinitionItem
    {
        public TypePropertyMapDefinitionItem() { }

        public Boolean IsUID { get; set; } = false;

        public String typePropertyName { get; set; } = "";

        public String metaPropertyName { get; set; } = "";

        public TypePropertyItemConverter converter { get; set; } 
    }
}