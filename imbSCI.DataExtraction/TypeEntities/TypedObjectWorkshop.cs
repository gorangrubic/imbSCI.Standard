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
using System.Collections;

namespace imbSCI.DataExtraction.TypeEntities
{
    public class TypedObjectWorkshop
    {
        public List<TypedObjectProvider> ObjectProviders { get; set; } = new List<TypedObjectProvider>();

        public MetaEntityNamespaceCollection Namespaces { get; set; }
        public IDataMiningTypeProvider TypeProvider { get; set; }

        //  public Dictionary<MetaEntityClass, TypedObjectProvider> ObjectProviderByClassNamepath { get; set; } = new Dictionary<MetaEntityClass, TypedObjectProvider>();


        public TypedObjectProvider GetProviderForEntityClass(MetaEntityClass entityClass)
        {
            TypedObjectProvider output = null;

            foreach (var provider in ObjectProviders)
            {
                if (provider.entityClass == entityClass)
                {
                    if (output == null)
                    {
                        output = provider;
                    } else
                    {
                        if (provider.IsPrimary)
                        {
                            output = provider;
                        }
                    }
                }
            }

            return output;
        }

        public TypedObjectProvider GetProviderForType(Type entityType)
        {
            return ObjectProviders.FirstOrDefault(x => x.type == entityType);
        }

        public TypedObjectWorkshop(MetaEntityNamespaceCollection namespaces, TypedObjectWorkshopSettings workshopSettings, IDataMiningTypeProvider typeProvider)
        {
            Namespaces = namespaces;
            TypeProvider = typeProvider;

            foreach (TypedObjectProviderDefinition provider in workshopSettings.providers)
            {
                TypedObjectProvider provider_instance = new TypedObjectProvider(provider);

                MetaEntityClass entityClass = Namespaces.FindClass(provider.EntityClassNamePath);
                provider_instance.Deploy(entityClass, typeProvider);

                ObjectProviders.Add(provider_instance);
            }

            foreach (TypedObjectProvider provider in ObjectProviders)
            {
                foreach (TypePropertyMapDefinitionItem itemDefinition in provider.map.items)
                {
                    PropertyInfo pi = provider.type.GetProperty(itemDefinition.typePropertyName);

                    TypePropertyMapItem mapItem = new TypePropertyMapItem()
                    {
                        metaPropertyName = itemDefinition.metaPropertyName,
                        typePropertyName = itemDefinition.typePropertyName,
                        propertyInfo = pi
                    };

                    //var IListInterface = pi.PropertyType.GetInterface("IList");

                    //Type[] GenericArguments = pi.PropertyType.GetGenericArguments();
                    
                    //if (GenericArguments.Any())
                    //{

                    //}

                    TypedObjectProvider subentity_provider = GetProviderForType(pi.PropertyType);

                    mapItem.TypeProvider = subentity_provider;

                    IMetaEntityExpressionTarget metaTarget = provider.entityClass.SelectTargetByPath(itemDefinition.metaPropertyName);
                    if (metaTarget is MetaEntityClassProperty metaProperty)
                    {
                        mapItem.metaProperty = metaProperty;
                    }

                    if (itemDefinition.converter != null)
                    {
                        IPropertyItemConverter mapConverter = itemDefinition.converter.GetConverter(pi);
                        mapItem.converter = mapConverter;
                    }
                    
                    provider.items.Add(mapItem);
                }
            }
        }

        /// <summary>
        /// Objects from meta entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="output">The output.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public Object ObjectFromMetaEntity(MetaEntity entity, Object output, TypedObjectProvider provider)
        {
            if (provider==null) provider = GetProviderForEntityClass(entity.EntityClassDefinition);

            if (output == null) output = provider.type.getInstance(null);

            DeploySetters(output, entity, provider);

            foreach (TypePropertyMapItem item in provider.items)
            {
                if (item.metaPropertyName==".")
                {
                    Object typeTarget = item.propertyInfo.GetValue(output, null);
                    DeploySetters(typeTarget, entity, item.TypeProvider);
                } else
                {
                    if (item.metaProperty.type.HasFlag(MetaEntityClassPropertyType.value))
                    {
                        if (item.metaProperty.type.HasFlag(MetaEntityClassPropertyType.collection))
                        {

                        }
                        else
                        {

                        }
                    } else if (item.metaProperty.type.HasFlag(MetaEntityClassPropertyType.entity))
                    {
                        var subentity = entity.Items.FirstOrDefault(x => x.name == item.metaProperty.name); 

                        if (item.metaProperty.type.HasFlag(MetaEntityClassPropertyType.collection))
                        {
                            Object collection = item.propertyInfo.GetValue(output, null);

                            if (collection is IList IListCollection)
                            {
                                foreach (MetaEntity entityItem in subentity.Items)
                                {
                                    Object instanceItem = ObjectFromMetaEntity(entityItem, null, item.TypeProvider);

                                    IListCollection.Add(instanceItem);
                                }
                                
                            }

                        } else
                        {
                            Object subtyped_instance = item.propertyInfo.GetValue(output, null);

                            Object subinstance = ObjectFromMetaEntity(subentity, subtyped_instance, item.TypeProvider);
                            item.propertyInfo.SetValue(output, subinstance, new object[] { });
                        }
                    }
                }
            }

            return output;

        }

        public void DeploySetters(Object output, MetaEntity entity, TypedObjectProvider provider)
        {
            foreach (TypePropertyMapItem item in provider.items)
            {
               
                if (item.metaProperty == null)
                {

                }
                else
                {
                    MetaPropertySetter setter = entity.GetSetter(item.metaProperty.PropertyName);
                    if (setter != null)
                    {
                        
                        Object setterValue = setter.Value;
                        if (setterValue != null)
                        {
                            if (item.converter != null)
                            {
                                setterValue = item.converter.Convert(setterValue);
                            }
                            SetPropertyValue(item.metaProperty, item.propertyInfo, setterValue, output, true);
                        }
                    }
                }

            }
        }



            protected void SetPropertyValue(MetaTableProperty metaProperty, PropertyInfo pi, Object valueToSet, Object existing = null, Boolean OverwriteExisting = true)
        {
            object[] o = new object[] { };

            Object e_v = null;
            Boolean doWrite = true;
            //if (!OverwriteExisting)
            //{
            //    e_v = pi.GetValue(existing, o);
            //    if (DefaultValues[pi] != e_v)
            //    {
            //        doWrite = false;
            //    }
            //}

            if (doWrite)
            {
                Object v = valueToSet;  //metaProperty.GetValue(entry.properties[metaProperty.PropertyName]);

                //String value = ;
                v = TypeProvider.ConvertMetaEntryValue(v, pi);
                pi.SetValue(existing, v, null);
            }
        }

        /*
        public void ApplyMapItem(TypePropertyMapDefinitionItem item, Object target, MetaEntity entity)
        {
            PropertyInfo pi = target.GetType().GetProperty(item.typePropertyName);
            TypedObjectProvider subentity_provider = GetProviderForType(pi.PropertyType);
            IMetaEntityExpressionTarget metaTarget = entity.SelectTargetByPath(item.metaPropertyName);

            if (metaTarget is MetaPropertySetter metaSetter)
            {
                var entityProperty = entity.EntityClassDefinition.FindProperty(metaSetter.name);

                SetPropertyValue(entityProperty, pi, metaSetter.Value, target, true);
            } else if (metaTarget is MetaEntity metaEntity)
            {
                

            }



            if (pi.PropertyType.is)
        }


        public Object ObjectFromMetaEntity(MetaEntity entity, Object existing=null) {

            entity.CheckClassDefinition(Namespaces, "");

            var entityClass = entity.EntityClassDefinition;
            TypedObjectProvider provider_instance = ObjectProviderByClassNamepath[entity.EntityClassDefinition];

            existing = provider_instance.map.SetObjectByMetaEntitySetters(entity, existing, true);

            foreach (var subentity in entity.Items)
            {
                var entityProperty = entityClass.FindProperty(subentity.name);
                var subentityClass = Namespaces.FindClass(entityProperty.ValueTypeName);
                var typeProperty = provider_instance.map.GetPropertyInfo(entityProperty);

                if (entityProperty.type.HasFlag(MetaEntityClassPropertyType.collection))
                {
                    Object list = typeProperty.GetValue(existing, null);

                    if (list is IList listInstance)
                    {
                        List<Object> subentity_instances = new List<object>();

                        foreach (var subentity_instance in subentity.Items)
                        {
                            Object subinstance = ObjectFromMetaEntity(subentity_instance, null);
                            subentity_instances.Add(subinstance);
                            listInstance.Add(subinstance);
                        }

                    }

                } else
                {
                    Object subinstance = ObjectFromMetaEntity(subentity, null);

                    typeProperty.SetValue(existing, subinstance, new object[] { });
                }
                
            }
            

            return existing;
        }
        */
        /*
        public List<Object> UpdateObjectsFromTable(TableExtractionTaskResult result, List<Object> existingObjects=null)
        {
            var task = result.task;
            var metas = result.extracted.Select(x=>x.meta);

            foreach (var meta in metas)
            {
                existingObjects = UpdateObjectsFromTable(meta, task, existingObjects, true);
            }

            return existingObjects;
        }
        

        public List<Object> UpdateObjectsFromTable(MetaTable input, TableExtractionTask task,List<Object> existingObjects=null, Boolean OverwriteExisting=true)
        {
            var objectProvidersForTask = TaskVsObjectProvider.links.GetAllLinked(task);
            foreach (var oPT in objectProvidersForTask)
            {
               existingObjects = oPT.UpdateObjectsFromTable(input, existingObjects, OverwriteExisting);
            }

            return existingObjects;
        }
        */
        

     //   public ModelElementRelationDictionary<TableExtractionTask, TypedObjectProvider> TaskVsObjectProvider { get; set; } = new ModelElementRelationDictionary<TableExtractionTask, TypedObjectProvider>();
        
    }
}