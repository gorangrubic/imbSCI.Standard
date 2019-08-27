using imbSCI.Core.collection;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.MetaTables.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.MetaEntities
{
/*
    public class MetaEntityCollection
    {
        public String EntityClassName { get; set; } = "";
        internal MetaEntityClass EntityClassDefinition { get; set; } = null;

        public List<MetaEntity> items { get; set; } = new List<MetaEntity>();

        public MetaEntityCollection()
        {

        }
    }*/

    public static class MetaEntityTools
    {

        /// <summary>
        /// Collects all classes from properties and their 
        /// </summary>
        /// <param name="rootClass">The root class.</param>
        /// <param name="namespaces">The namespaces.</param>
        /// <param name="includeSelf">if set to <c>true</c> [include self].</param>
        /// <returns></returns>
        public static List<MetaEntityClass> CollectRelevantClasses(this MetaEntityClass rootClass, MetaEntityNamespaceCollection namespaces, Boolean includeSelf = true)
        {
            List<MetaEntityClass> output = new List<MetaEntityClass>();

            if (includeSelf) output.Add(rootClass);

            List<MetaEntityClass> iteration = new List<MetaEntityClass>() { rootClass };

            while (iteration.Any())
            {
                List<MetaEntityClass> next_iteration = new List<MetaEntityClass>();
                foreach (var cf in iteration)
                {
                    foreach (MetaEntityClassProperty property in cf.Properties)
                    {
                        if (property.type.HasFlag(MetaEntityClassPropertyType.entity))
                        {
                            var entityClass = namespaces.FindClass(property.ValueTypeName);
                            if (!output.Any(x=>x.GetNamepath() == entityClass.GetNamepath()))
                            {
                                next_iteration.Add(entityClass);
                                output.Add(entityClass);
                            }
                        }
                    }
                }

                iteration = next_iteration;
            }

            return output;
           
        }

        public static Boolean IsMultinodeExpressionPath(String expressionPathOrNode)
        {
            if (expressionPathOrNode.Contains("."))
            {
                return true;
            }

            return false;
        }

        public static String GetSelectExpression(this IMetaEntityExpressionTargetWithPath entity, IMetaEntityExpressionTarget fromParent=null)
        {
            String output = "";
            if (entity.Parent != null) {

                if (fromParent != entity.Parent)
                {
                    
                    if (entity.Parent is IMetaEntityExpressionTargetWithPath parentWithPath)
                    {
                        if (entity.Parent != entity) output = parentWithPath.GetSelectExpression(fromParent);
                    }
                } else
                {

                }
            }
            output = output.add(entity.name, ".");

            return output;
            
        }


        public static IMetaEntityExpressionTarget SelectTargetByPath(this IMetaEntityExpressionTarget entity, String expression)
        {
            List<String> parts = expression.SplitSmart(".");

            IMetaEntityExpressionTarget head = entity as IMetaEntityExpressionTarget;

            IMetaEntityExpressionTarget lastHead = null;

            foreach (String part in parts)
            {
                if (head != null)
                {
                    if (lastHead != head)
                    {
                        if (head.Parent == null)
                        {
                            if (lastHead != null)
                            {
                                head.Parent = lastHead;
                            }
                        }
                       
                    }
                    lastHead = head;

                    head = head.SelectTarget(part);
                }

                //if (head is MetaEntity entityHead)
                //{

                //    MetaEntityClassProperty property = entityHead.EntityClassDefinition.Properties.FirstOrDefault(x => x.name == part);

                //    if (property.type.HasFlag(MetaEntityClassPropertyType.value))
                //    {
                //        head = entity.Setters.FirstOrDefault(x => x.name == part);
                //    } else if (property.type.HasFlag(MetaEntityClassPropertyType.entity))
                //    {
                //        head = entity.items.FirstOrDefault(x => x.name == part);
                //    };


                //} else if (head is MetaPropertySetter entityProperty) {
                //    return head;
                //}

            }

            return head;
            
        }
    }
}