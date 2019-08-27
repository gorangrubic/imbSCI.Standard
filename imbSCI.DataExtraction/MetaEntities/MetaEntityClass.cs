using imbSCI.Core.collection;
using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.MetaEntities
{

    public class MetaEntityClass: MetaEntityTargetWithPathBase, IMetaEntityExpressionTargetWithPath
    {

        public String namePropertyName { get; set; } = "";

        public MetaEntityClassProperty AddProperty(MetaTableProperty property, Boolean throwException = true)
        {
            MetaEntityClassProperty newProperty = new MetaEntityClassProperty()
            {
                type = MetaEntityClassPropertyType.value,
                PropertyName = property.PropertyName,
                DisplayName = property.DisplayName

            };

            newProperty.Learn(property);

            if (AddProperty(newProperty, throwException))
            {
                return newProperty;
            } else
            {
                return null;
            }
        }

        public List<MetaEntityClassProperty> AddProperties(IEnumerable<MetaTableProperty> table_properties, Boolean throwException = true)
        {
            List<MetaEntityClassProperty> newProperties = new List<MetaEntityClassProperty>();

            foreach (MetaTableProperty prop in table_properties)
            {
                var newProp = AddProperty(prop, throwException);
                if (newProp != null)
                {
                    newProperties.Add(newProp);
                }
            }

            return newProperties;
        }

        public MetaEntityClassProperty AddValueProperty(String propertyName, Type valueType)
        {
            var ilistInterface = valueType.GetInterface(nameof(IList));
            var type = MetaEntityClassPropertyType.value;
            String propertyValueType = valueType.Name;

            if (ilistInterface != null)
            {
                type |= MetaEntityClassPropertyType.collection;
                propertyValueType = valueType.GetGenericArguments().FirstOrDefault().Name;
            }
            MetaEntityClassProperty newProperty = new MetaEntityClassProperty()
            {
                type = type,
                PropertyTypeName = propertyValueType,
                name = propertyName
            };

            AddProperty(newProperty);
            

            return newProperty;
        }

        public MetaEntityClassProperty AddEntityProperty(String propertyName, String EntityClassName, Boolean isCollection=false)
        {
            var type = MetaEntityClassPropertyType.entity;
            if (isCollection) type |= MetaEntityClassPropertyType.collection;

            MetaEntityClassProperty newProperty = new MetaEntityClassProperty()
            {
                type = type,
                PropertyTypeName = EntityClassName,
                name = propertyName
            };

            AddProperty(newProperty);


            return newProperty;

        }

        public Boolean AddProperty(MetaEntityClassProperty newProperty, Boolean throwException=true)
        {
            if (newProperty.name.isNullOrEmpty())
            {
                throw new ArgumentOutOfRangeException(nameof(newProperty), $"Property name is null or empty");
            }
            if (!Properties.Any(x => x.name == newProperty.name))
            {
                newProperty.Parent = this;
                Properties.Add(newProperty);
                return true;
            }
            else
            {
                if (throwException) throw new ArgumentOutOfRangeException(nameof(newProperty), $"Property {newProperty.name} already declared");
            }
            return false;
        }

        public List<MetaEntityClassProperty> Properties { get; set; } = new List<MetaEntityClassProperty>();

        [XmlIgnore]
        public override List<IMetaEntityExpressionTarget> items {
            get {

                return Properties.ToList<IMetaEntityExpressionTarget>();
            }
        }

        public MetaEntityClassProperty FindProperty(MetaTablePropertyAliasEntry propertyName)
        {
            MetaEntityClassProperty property = Properties.FirstOrDefault(x => x.name.Equals(propertyName.rootPropertyName, StringComparison.InvariantCultureIgnoreCase));

            if (property == null)
            {
                foreach (String variation in propertyName.aliasPropertyNames)
                {
                    property = Properties.FirstOrDefault(x => x.name.Equals(variation, StringComparison.InvariantCultureIgnoreCase));
                    if (property != null) break;
                }
            }

            return property;
        }


        public MetaEntityClassProperty FindProperty(String propertyName)
        {
            MetaEntityClassProperty property = Properties.FirstOrDefault(x => x.name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));

            return property;
        }

        //public String name { get; set; } = "main";

        //public IMetaEntityExpressionTarget Parent { get; set; }

        public MetaEntityClass()
        {

        }

        public DataTable CreateDataTableForEntities(MetaEntityClassPropertyType typesToInclude = MetaEntityClassPropertyType.value | MetaEntityClassPropertyType.valueCollection)
        {
            DataTable output = new DataTable();

            output.Columns.Add(nameof(MetaEntity.name), typeof(String)).SetHeading("Name");
            output.Columns.Add(nameof(MetaEntity.EntityClassName), typeof(String)).SetHeading("Class");

            CreateDataTable(output, typesToInclude);

            return output;
        }

        public void CreateDataTableRow(DataTable output, MetaEntity entity, MetaEntityClassPropertyType typesToInclude = MetaEntityClassPropertyType.value | MetaEntityClassPropertyType.valueCollection)
        {
            var types = typesToInclude.getEnumListFromFlags<MetaEntityClassPropertyType>();
            var dr = output.NewRow();

            if (output.Columns.Contains(nameof(MetaEntity.name))) dr[nameof(MetaEntity.name)] = entity.name;
            if (output.Columns.Contains(nameof(MetaEntity.EntityClassName))) dr[nameof(MetaEntity.EntityClassName)] = entity.EntityClassName;


            foreach (var property in Properties)
            {
                if (types.Contains(property.type))
                {
                    String column_name = property.GetSelectExpression();
                    
                    if (output.Columns.Contains(column_name))
                    {
                        var setter = entity.Setters.FirstOrDefault(x => x.name.Equals(property.name));
                        if (setter != null)
                        {
                            
                            if (property.type.HasFlag(MetaEntityClassPropertyType.collection))
                            {
                                MetaPropertyInstruction instruction = new MetaPropertyInstruction(setter, property);
                                dr[column_name] = instruction.value;
                            } else
                            {
                                if (setter.Value != null)
                                {
                                    dr[column_name] = setter.Value;
                                }
                            }
                            
                        }
                        
                    }
                }
            }

            output.Rows.Add(dr);
        }

        public DataTable CreateDataTable(DataTable output, MetaEntityClassPropertyType typesToInclude = MetaEntityClassPropertyType.value | MetaEntityClassPropertyType.valueCollection)
        {

            if (output == null) output = new DataTable();

            var types = typesToInclude.getEnumListFromFlags<MetaEntityClassPropertyType>();

            MetaEntityNamespace Namespace = Parent as MetaEntityNamespace;

            String namespacePath = this.GetSelectExpression();

            foreach (var property in Properties)
            {
                if (types.Contains(property.type))
                {
                    String column_name = property.GetSelectExpression();
                    Type propertyType = property.GetValueType();

                    if (!output.Columns.Contains(column_name))
                    {
                        if (property.type.HasFlag(MetaEntityClassPropertyType.collection))
                        {
                            propertyType = typeof(String);
                        } else
                        {

                        }
                        output.Columns.Add(column_name, propertyType).SetHeading(property.name).SetGroup(namespacePath);
                    }
                }
            }

            return output;
        }

        public MetaEntity CreateEntity(MetaEntityNamespace classCollection, String entityName="")
        {
            MetaEntity output = new MetaEntity();
            output.EntityClassDefinition = this;
            output.EntityClassName = name;
            output.name = entityName;

            MetaEntityItemSelection itemSelector = new MetaEntityItemSelection();


            foreach (var property in Properties)
            {
                if (property.type.HasFlag(MetaEntityClassPropertyType.value))
                {
                    if (property.type.HasFlag(MetaEntityClassPropertyType.collection))
                    {
                        output.Setters.Add(new MetaPropertySetter() { Parent = output, name = property.name, Value= new List<Object>() });
                    } else
                    {
                        output.Setters.Add(new MetaPropertySetter() { Parent = output, name = property.name });
                    }
                    

                } else if (property.type.HasFlag(MetaEntityClassPropertyType.entity))
                {
                   // itemSelector.SetSelection(classCollection.SelectTarget(property.PropertyTypeName));

                    var entityClass = classCollection.FindClass(property.PropertyTypeName);  // itemSelector.SelectedClass;

                    var subEntity = entityClass.CreateEntity(classCollection, property.name);
                    subEntity.Parent = output;

                    output.Items.Add(subEntity);

                    //classCollection.CreateEntity(property.name));
                }

            }

            return output;
        }

        public override IMetaEntityExpressionTarget SelectTarget(string expressionPathNode)
        {
            if (MetaEntityTools.IsMultinodeExpressionPath(expressionPathNode)) return Parent.SelectTargetByPath(expressionPathNode);

            IMetaEntityExpressionTarget head = this;

            MetaEntityClassProperty property = this.Properties.FirstOrDefault(x => x.name == expressionPathNode);

            if (property == null)
            {
                property = new MetaEntityClassProperty()
                {
                    name = expressionPathNode,
                    type = MetaEntityClassPropertyType.none
                };

                AddProperty(property, false);
            }

            return property;

        }
    }
}