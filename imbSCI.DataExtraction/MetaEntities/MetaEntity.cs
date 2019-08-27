using imbSCI.Core.collection;
using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.reporting.render;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.MetaTables.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.MetaEntities
{
    public class MetaEntity: MetaEntityTargetWithPathBase 
    {

        public String EntityClassName { get; set; } = "";

        [XmlIgnore]
        public override List<IMetaEntityExpressionTarget> items
        {
            get
            {
                return new List<IMetaEntityExpressionTarget>();
            }
        }
       

        internal MetaEntityClass EntityClassDefinition { get; set; } = null;

        public List<MetaEntity> Items { get; set; } = new List<MetaEntity>();

        public MetaEntity()
        {

        }

        public List<MetaPropertyInstruction> ConvertToInstructions(MetaEntityNamespaceCollection namespaces, Boolean includeItems=true)
        {
            List<MetaPropertyInstruction> output = new List<MetaPropertyInstruction>();

            CheckClassDefinition(namespaces, EntityClassName);

            output.Add(new MetaPropertyInstruction(nameof(name), name));
            output.Add(new MetaPropertyInstruction(nameof(EntityClassName), EntityClassName));

            foreach (var setter in Setters)
            {
                var setterProperty = EntityClassDefinition.FindProperty(setter.name);

                output.Add(new MetaPropertyInstruction(setter, setterProperty));
            }

            if (includeItems)
            {
                foreach (var item in Items)
                {
                    var itemProperty = EntityClassDefinition.FindProperty(item.name);
                    if (itemProperty.type.HasFlag(MetaEntityClassPropertyType.collection))
                    {
                        List<MetaPropertyInstruction> itemInstructions = new List<MetaPropertyInstruction>();
                        foreach (var subitem in item.Items)
                        {
                            var subinstructions = subitem.ConvertToInstructions(namespaces);
                            if (subinstructions.Any())
                            {
                                itemInstructions.Add(new MetaPropertyInstruction(subitem.name, subinstructions));
                            }
                        }
                        if (itemInstructions.Any())
                        {
                            output.Add(new MetaPropertyInstruction(itemProperty.name, itemInstructions));
                        }
                    }
                    else
                    {
                        var subinstructions = item.ConvertToInstructions(namespaces);
                        output.Add(new MetaPropertyInstruction(itemProperty.name, subinstructions));
                    }
                }
            }

            return output;

        }

       
        public void WriteJSON(ITextRender output, MetaEntityNamespaceCollection namespaces)
        {
            var instructions = ConvertToInstructions(namespaces);
            MetaPropertyInstruction rootInstruction = new MetaPropertyInstruction("", instructions);
            rootInstruction.WriteToOutput(output, true);
            
            
        }

        public DataSet GetDataSet(MetaEntityNamespaceCollection namespaces)
        {
            DataTable valueTable = GetVerticalDataTable(namespaces);
           

            DataSet output = new DataSet();
            output.Tables.Add(valueTable);

            foreach (var item in Items)
            {
                var itemProperty = EntityClassDefinition.FindProperty(item.name);
                if (itemProperty.type.HasFlag(MetaEntityClassPropertyType.collection))
                {
                    MetaEntityClass itemClass = namespaces.FindClass(itemProperty.PropertyTypeName);

                    DataTable collectionTable = itemClass.CreateDataTableForEntities(MetaEntityClassPropertyType.valueCollection);
                    collectionTable.SetTitle(itemProperty.name);

                    foreach (var subitem in item.Items)
                    {
                        itemClass.CreateDataTableRow(collectionTable, subitem, MetaEntityClassPropertyType.valueCollection);
                    }
                    output.Tables.Add(collectionTable);
                }
                else
                {
                    var itemVerticalTable = item.GetVerticalDataTable(namespaces, itemProperty.name);

                    output.Tables.Add(itemVerticalTable);
                }
            }

            return output;

        }

        public DataTable GetVerticalDataTable(MetaEntityNamespaceCollection namespaces, String tableName="")
        {
            var instructions = ConvertToInstructions(namespaces, false);

            DataTable table = new DataTable();
            table.SetTitle(tableName.or(name, EntityClassName));


            var column_name = table.Columns.Add("name");


            var column_value = table.Columns.Add("value");


            var column_contentType = table.Columns.Add("contentType");


            foreach (var inst in instructions)
            {
                var dr = table.NewRow();

                dr[column_value] = inst.value;

                dr[column_name] = inst.name;

                if (inst.property != null)
                {
                    dr[column_contentType] = inst.property.ContentType.ToString();
                }

                table.Rows.Add(dr);
            }

            return table;


        }
      

        public String GetJSON(MetaEntityNamespaceCollection namespaces)
        {
            builderForText output = new builderForText();
            WriteJSON(output, namespaces);
            return output.GetContent().Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
        }

        //public Dictionary<String, MetaEntity> ChildEntities { get; set; } = new Dictionary<String, MetaEntity>();

       // public Dictionary<String, MetaEntityCollection> EntityCollections { get; set; } = new Dictionary<String, MetaEntityCollection>();

        public void CheckClassDefinition(MetaEntityNamespaceCollection namespaces, String rootclassname)
        {
            if (EntityClassName.isNullOrEmpty())
            {
                EntityClassName = rootclassname;
            }

            if (EntityClassDefinition == null)
            {
                EntityClassDefinition = namespaces.SelectTarget(EntityClassName) as MetaEntityClass;
            }
        }

        public List<MetaPropertySetter> Setters { get; set; } = new List<MetaPropertySetter>();

        

        public void AcceptEntryProperties(MetaTableEntry entry)
        {
            foreach (var ep in EntityClassDefinition.Properties)
            {
                if (ep.type.HasFlag(MetaEntityClassPropertyType.value))
                {
                    if (entry.HasValueFor(ep))
                    {
                        SetSetter(ep.PropertyName, entry.GetOutputValue(ep));
                    }
                }
            }
        }

        public void Finalize(MetaEntityNamespaceCollection namespaces)
        {
            CheckClassDefinition(namespaces, "");

            if (!EntityClassDefinition.namePropertyName.isNullOrEmpty())
            {
                if (name.isNullOrEmpty())
                {
                    var nameSetter = GetSetter(EntityClassDefinition.namePropertyName);
                    if (nameSetter != null)
                    {
                        name = nameSetter.Value.toStringSafe("");
                    }
                }
            }

            foreach (var item in Items)
            {
                item.Finalize(namespaces);
            }
        }


        public Boolean IsAnySetterSet()
        {
            var valueProperties = EntityClassDefinition.Properties.Where(x => x.type == MetaEntityClassPropertyType.value).ToList();
            if (valueProperties.Any())
            {
                foreach(var vp in valueProperties)
                {
                    var setter = GetSetter(vp.PropertyName);
                    if (setter!=null)
                    {
                        if (!setter.Value.isNullOrEmpty()) return true;
                    }
                }
            } else
            {
                return true;
            }

            return false;
        }

        public MetaPropertySetter GetNameSetter()
        {
            return GetSetter(EntityClassDefinition.namePropertyName);
        }

        /// <summary>
        /// Gets the setter.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Found setter or null if no setter found</returns>
        public MetaPropertySetter GetSetter(String propertyName)
        {
            var setter = Setters.FirstOrDefault(x => x.name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));

            return setter;
        }

        public void SetSetter(String propertyName, Object value)
        {
            MetaEntityClassProperty property = this.EntityClassDefinition.FindProperty(propertyName); //.Properties.FirstOrDefault(x => x.name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));

            var setter = Setters.FirstOrDefault(x => x.name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));

            if (property.type.HasFlag(MetaEntityClassPropertyType.collection))
            {
                if (setter.Value is IList valueList)
                {
                    valueList.Add(value);
                }
            } else
            {
                setter.Value = value;
            }
        }

        public override IMetaEntityExpressionTarget SelectTarget(string expressionPathNode)
        {
            IMetaEntityExpressionTarget head = this;

            if (expressionPathNode.isNullOrEmpty()) return head;

            MetaEntityClassProperty property = this.EntityClassDefinition.Properties.FirstOrDefault(x => x.name.Equals(expressionPathNode));

            if (property.type.HasFlag(MetaEntityClassPropertyType.value))
            {
                head = Setters.FirstOrDefault(x => x.name == expressionPathNode);
            }
            else if (property.type.HasFlag(MetaEntityClassPropertyType.entity))
            {
                head = Items.FirstOrDefault(x => x.name == expressionPathNode);

                //if (property.type.HasFlag(MetaEntityClassPropertyType.collection))
                //{
                //    return property;
                //}
                //else
                //{
                   
                //}


            };

            return head;

        }

    }
}
