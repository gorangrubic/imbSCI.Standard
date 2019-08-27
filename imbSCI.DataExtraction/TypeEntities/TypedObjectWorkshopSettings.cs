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
    public class TypedObjectWorkshopSettings
    {

        public TypedObjectWorkshopSettings() { }

        public List<TypedObjectProviderDefinition> providers { get; set; } = new List<TypedObjectProviderDefinition>();

        public TypedObjectProviderDefinition GetOrAdd(String typename, String namepath)
        {
            TypedObjectProviderDefinition output = null;

            output = providers.FirstOrDefault(x => x.ObjectTypeName == typename && x.EntityClassNamePath == namepath);

            if (output == null)
            {
                output = new TypedObjectProviderDefinition()
                {
                    ObjectTypeName = typename,
                    EntityClassNamePath = namepath,
                    name = typename + namepath
                };
                providers.Add(output);
            }
            return output;
        }

        //public List<ModelElementRelationEntry> TaskObjectProviderLinks { get; set; } = new List<ModelElementRelationEntry>();

        
    }
}