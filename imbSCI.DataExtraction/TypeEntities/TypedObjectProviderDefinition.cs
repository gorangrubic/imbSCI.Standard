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
    public class TypedObjectProviderDefinition
    {
        

        public String name { get; set; } = "";
        
        public TypedObjectProviderDefinition() { }

        public String ObjectTypeName { get; set; } = "";

        public String EntityClassNamePath { get; set; } = "";

        public Boolean IsPrimaryForEntityClass { get; set; } = false;

        public TypePropertyMapDefinition mapDefinition { get; set; } = new TypePropertyMapDefinition();
    }
}