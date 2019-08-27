using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions;
using imbSCI.DataExtraction.Extractors.Task;
using imbSCI.Core.data;
using imbSCI.DataExtraction.MetaEntities;

namespace imbSCI.DataExtraction.TypeEntities
{
  

   

    /// <summary>
    /// Instanced property map, ready  for data objects construction
    /// </summary>
    public class TypePropertyMap
    {
        /// <summary>
        /// If set - it will enable objects synchronization via UID value
        /// </summary>
        /// <value>
        /// The type uid property.
        /// </value>
        public PropertyInfo TypeUIDProperty { get; set; }


        public IDataMiningTypeProvider typeProvider { get; set; }

        public Object DefaultInstance { get; set; } = null;
        public Type type { get; set; }

        public void SetDefaultInstance()
        {
            if (DefaultInstance == null)
            {
                var constuctor = type.GetConstructor(new Type[] { });
                if (constuctor != null)
                {
                    DefaultInstance = constuctor.Invoke(new object[] { });
                }
                 var prop = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                if (DefaultInstance != null)
                {
                    foreach (PropertyInfo item in prop)
                    {
                        DefaultValues.Add(item, item.GetValue(DefaultInstance, new object[] { }));
                    }
                } else
                {
                    foreach (PropertyInfo item in prop)
                    {
                        DefaultValues.Add(item, item.PropertyType.GetDefaultValue());
                    }
                }
            }
        }

     //   MetaTablePropertyAliasList UIDMetaProperties = new MetaTablePropertyAliasList();


        //public MetaTableProperty GetUIDMetaProperty(MetaTablePropertyCollection metaProperties)
        //{
        //    MetaTableProperty output = null;
        //    foreach (var ae in UIDMetaProperties.items)
        //    {
        //        output = metaProperties.Get(ae.rootPropertyName, UIDMetaProperties);
        //        if (output != null) break;
        //    }
        //    return output;
        //}

        public TypePropertyMap(Type _type, MetaEntityClass schema, TypePropertyMapDefinition definition, IDataMiningTypeProvider typeProvider)
        {
            type = _type;
            typeProvider = typeProvider;

            var prop = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToDictionary(x => x.Name);

            List<MetaTableProperty> taskProperties = new List<MetaTableProperty>();

            foreach (TypePropertyMapDefinitionItem item in definition.items)
            {
                if (!prop.ContainsKey(item.typePropertyName))
                {
                    continue;
                }

                var schemaProperty = schema.FindProperty(item.metaPropertyName);

                if (schemaProperty == null)
                {
                    continue;
                }

                

                var pi = prop[item.typePropertyName];

                if (schema.namePropertyName == schemaProperty.PropertyName)
                {
                    TypeUIDProperty = pi;
                }

                propertyLinkABs.Add(schemaProperty, pi);
                propertyLinkBAs.Add(pi, schemaProperty);
            }
        }


        //public TypePropertyMap(Type _type, TaskPropertyDictionary schema, TypePropertyMapDefinition definition, IDataMiningTypeProvider typeProvider)
        //{
        //    type = _type;
        //    typeProvider = typeProvider;

        //    Dictionary<String, TypePropertyMapDefinitionItem> mappedProperties = new Dictionary<string, TypePropertyMapDefinitionItem>();
        //    Dictionary<MetaTablePropertyAliasEntry, PropertyInfo> reverseMap = new Dictionary<MetaTablePropertyAliasEntry, PropertyInfo>();

        //    MetaTablePropertyAliasList alist = new MetaTablePropertyAliasList();

        //    var prop = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            


        //    List<TaskPropertyEntry> taskProperties = new List<TaskPropertyEntry>();

        //    foreach (TypePropertyMapDefinitionItem item in definition.items)
        //    {
                
        //        var pi = prop.FirstOrDefault(x => x.Name.Equals(item.typePropertyName));

        //        //if (item.IsUID)
        //        //{
        //        //    TypeUIDProperty = prop.FirstOrDefault(x => x.Name.Equals(item.typePropertyName, StringComparison.InvariantCultureIgnoreCase));
        //        //    UIDMetaProperties.items.Add(item.metaPropertyNames);
        //        //}

        //        if (pi == null)
        //        {
        //            throw new Exception("Specified property name [" + item.typePropertyName + "] not found in type [" + type.Name + "]");
        //        }

        //        alist.items.Add(item.metaPropertyNames);

        //        taskProperties.AddRange(taskProperties.Where(x => item.metaPropertyNames.isMatch(x.propertyName)));

        //        mappedProperties.Add(item.typePropertyName, item);
        //        reverseMap.Add(item.metaPropertyNames, pi);
        //    }

        //    foreach (MetaTableProperty item in taskProperties.Select(x=>x.Meta))
        //    {
        //        MetaTablePropertyAliasEntry a = alist.Match(item.PropertyName);
        //        if (a != null)
        //        {
        //            var pi = reverseMap[a];
        //            propertyLinkABs.Add(item, pi);
        //            propertyLinkBAs.Add(pi,item);
        //        }
        //    }
        //    SetDefaultInstance();
        //}

        /// <summary>
        /// Sets values from <see cref="MetaEntity.Setters"/> items of <c>MetaEntity</c>
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="existing">The existing.</param>
        /// <param name="OverwriteExisting">if set to <c>true</c> [overwrite existing].</param>
        /// <returns></returns>
        public Object SetObjectByMetaEntitySetters(MetaEntity entity, Object existing = null, Boolean OverwriteExisting = true)
        {
            if (existing == null)
            {
                existing = type.getInstance(null);
            }

            foreach (KeyValuePair<MetaTableProperty, PropertyInfo> pair in propertyLinkABs)
            {
                MetaPropertySetter setter = entity.GetSetter(pair.Key.PropertyName);

                if (setter == null) continue;

                SetPropertyValue(pair.Key, pair.Value, setter.Value, existing, OverwriteExisting);

            }

            return existing;
        }


        public PropertyInfo GetPropertyInfo(MetaTableProperty entityProperty)
        {
            return propertyLinkABs[entityProperty];
        }

        protected void SetPropertyValue(MetaTableProperty metaProperty, PropertyInfo pi, Object valueToSet, Object existing = null, Boolean OverwriteExisting = true)
        {
            object[] o = new object[] { };

            Object e_v = null;
            Boolean doWrite = true;
            if (!OverwriteExisting)
            {
                e_v = pi.GetValue(existing, o);
                if (DefaultValues[pi] != e_v)
                {
                    doWrite = false;
                }
            }

            if (doWrite)
            {
                Object v = valueToSet;  //metaProperty.GetValue(entry.properties[metaProperty.PropertyName]);

                //String value = ;
                v = typeProvider.ConvertMetaEntryValue(v, pi);
                pi.SetValue(existing, v, null);
            }
        }

        /// <summary>
        /// Sets the object by entry from MetaTable
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="input">The input.</param>
        /// <param name="existing">The existing.</param>
        /// <param name="OverwriteExisting">if set to <c>true</c> [overwrite existing].</param>
        /// <returns></returns>
        public Object SetObjectByMetaEntry(MetaTableEntry entry,MetaTable input, Object existing = null, Boolean OverwriteExisting = true) 
        {
            if (existing == null)
            {
                existing = type.getInstance(null);
            }

            object[] o = new object[] { };

            foreach (KeyValuePair<MetaTableProperty, PropertyInfo> pair in propertyLinkABs)
            {
                MetaTableProperty metaProperty = input.properties.Get(pair.Key.PropertyName);

                if (metaProperty != null)
                {
                    Object v = entry.GetOutputValue(metaProperty);

                    SetPropertyValue(metaProperty, pair.Value, v, existing, OverwriteExisting);
                }
            }

            return existing;
        }

        /// <summary>
        /// Sets the object by meta entry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry">The entry.</param>
        /// <param name="existing">The existing.</param>
        /// <param name="OverwriteExisting">if set to <c>true</c> [overwrite existing].</param>
        /// <returns></returns>
        public T SetObjectByMetaEntry<T>(MetaTableEntry entry, T existing = null, Boolean OverwriteExisting = true) where T : class, new()
        {
            return SetObjectByMetaEntry(entry, existing as T, true);
        }


        public String GetObjectUID(Object input)
        {
            if (TypeUIDProperty == null) return "";
            String k = TypeUIDProperty.GetValue(input, null) as String;
            return k;
        }

        /// <summary>
        /// Maps input objects into UID based dictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public Dictionary<String, T> GetObjectByUID<T>(List<T> input)
        {
            Dictionary<String, T> output = new Dictionary<string, T>();
            foreach (T i in input)
            {
                String k = GetObjectUID(i); //TypeUIDProperty.GetValue(i, null) as String;
                output.Add(k, i);
            }
            return output;
        }
        public Dictionary<PropertyInfo, Object> DefaultValues { get; set; } = new Dictionary<PropertyInfo, object>();
        public Dictionary<MetaTableProperty,PropertyInfo> propertyLinkABs { get; set; } = new Dictionary<MetaTableProperty, PropertyInfo>();
        public Dictionary<PropertyInfo, MetaTableProperty> propertyLinkBAs { get; set; } = new Dictionary<PropertyInfo, MetaTableProperty>();

    }
}