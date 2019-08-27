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
using imbSCI.Core.data;

using imbSCI.DataExtraction.Extractors.Task;
using imbSCI.DataExtraction.MetaEntities;

namespace imbSCI.DataExtraction.TypeEntities
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithName" />
    public class TypedObjectProvider:IObjectWithName // where T:class, new()
    {
        public String name { get; set; } 

        public Type type { get; set; }

        public MetaEntityClass entityClass { get; set; }

        public TypePropertyMapDefinition map { get; set; }

        public Boolean IsPrimary { get; set; } = false;

        public List<TypePropertyMapItem> items { get; set; } = new List<TypePropertyMapItem>();

        protected TypedObjectProviderDefinition definition { get; set; }

        public TypedObjectProvider(TypedObjectProviderDefinition _definition)
        {
            definition = _definition;
            name = definition.name;
            map = definition.mapDefinition;
            IsPrimary = definition.IsPrimaryForEntityClass;
        }

        public Object GetInstance()
        {
            return type.getInstance(null);
        }

        public void Deploy(MetaEntityClass _entityClass, IDataMiningTypeProvider typeProvider)
        {
            entityClass = _entityClass;
            type = typeProvider.GetTypeByName(definition.ObjectTypeName);
            
           // map = new TypePropertyMap(type, entityClass, definition.mapDefinition, typeProvider);
        }


        //public void Deploy(TableExtractionTask task, IDataMiningTypeProvider typeProvider)
        //{

        //    type = typeProvider.GetTypeByName(definition.ObjectTypeName);

        //    map = new TypePropertyMap(type,task.PropertyDictionary, definition.mapDefinition, typeProvider);
        //}




        /*
        /// <summary>
        /// Updates objects by data from meta table. If map defines no UID property (<see cref="TypePropertyMapDefinition.TypeUIDPropertyName"/>), new objects are created for each entry
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <param name="existingObjects">The existing objects.</param>
        /// <param name="OverwriteExisting">if set to <c>true</c> [overwrite existing].</param>
        /// <returns></returns>
        public List<Object> UpdateObjectsFromTable(MetaTable input,List<Object> existingObjects=null, Boolean OverwriteExisting=true)
        {
            if (existingObjects == null) existingObjects = new List<Object>();

            if (map.TypeUIDProperty == null)
            {
                foreach (var entry in input.entries.items)
                {
                    Object existing = map.SetObjectByMetaEntry(entry, input, null, OverwriteExisting);

                    existingObjects.AddUnique(existing);
                }
            }
            else
            {
                var UIDMetaProperty = map.GetUIDMetaProperty(input.properties);
                var existingByUID = map.GetObjectByUID(existingObjects);
                
                foreach (var entry in input.entries.items)
                {
                    Object existing = null;
                    String mUID = entry.GetStoredValue(UIDMetaProperty);

                    if (existingByUID.ContainsKey(mUID))
                    {
                        existing = existingByUID[mUID];
                    }
                    existing = map.SetObjectByMetaEntry(entry, input, existing, OverwriteExisting);
                    existingObjects.AddUnique(existing);
                }
            }
            return existingObjects;
        }
        */
      
    }

}
