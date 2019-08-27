using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace imbSCI.DataExtraction.TypeEntities
{
    [Serializable]
    public class TypePropertyMapDefinition
    {

        public TypePropertyMapDefinition() { }

       // public String TypeUIDPropertyName {get;set;} = "name";

        public List<TypePropertyMapDefinitionItem> items { get; set; } = new List<TypePropertyMapDefinitionItem>();


        public TypePropertyMapDefinitionItem GetOrAdd(String typePropertyName, String metaPropertyName)
        {
            TypePropertyMapDefinitionItem output = items.FirstOrDefault(x => x.metaPropertyName == metaPropertyName && x.typePropertyName == typePropertyName);
            if (output == null)
            {
                output = new TypePropertyMapDefinitionItem()
                {
                    typePropertyName = typePropertyName,
                    metaPropertyName = metaPropertyName
                };
                items.Add(output);
            }
            return output;
        }



    }
}