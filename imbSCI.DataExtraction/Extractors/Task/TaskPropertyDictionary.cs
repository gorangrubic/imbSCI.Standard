using imbSCI.Core.files.folders;
using imbSCI.Data;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.Validation.TaskValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Extractors.Task
{
    [Serializable]
    public class TaskPropertyDictionary
    {
        public TaskPropertyDictionary()
        {

        }

        public String TaskName { get; set; } = "";

        public Boolean Any()
        {
            return Count() > 0;
        }
        
        public Int32 Count()
        {
            return items.Count;
        }

        public List<TaskPropertyEntry> items { get; set; } = new List<TaskPropertyEntry>();

        public void CollectProperties(IEnumerable<TaskPropertyValidation> input)
        {
            foreach (var i in input)
            {
                var p = OpenProperty(i.item);
            }
             

        }

        public void CollectProperties(IEnumerable<MetaTableProperty> input)
        {
            foreach (var i in input)
            {
                var p = OpenProperty(i);
            }


        }

        public virtual List<MetaTableProperty> CollectProperties(MetaTable metaTable)
        {
            List<MetaTableProperty> discovered = new List<MetaTableProperty>();

            if (!metaTable.IsValid) return discovered;

            foreach (MetaTableProperty mtProp in metaTable.properties.items)
            {
                var p = OpenProperty(mtProp);
                if (p.exampleValue.isNullOrEmpty())
                {
                    discovered.Add(mtProp);

                    foreach (MetaTableEntry entry in metaTable.entries.items)
                    {
                        p.exampleValue = entry.GetStoredValue(mtProp);
                    }
                }
            }
            return discovered;
        }

        public TaskPropertyEntry GetProperty(String propertyName)
        {
            var output = items.FirstOrDefault(x => x.propertyName == propertyName);
            return output;
        }


        public TaskPropertyEntry OpenProperty(MetaTableProperty property) 
        {
            var output = items.FirstOrDefault(x => x.propertyName == property.PropertyName);
            if (output == null)
            {
                output = new TaskPropertyEntry(property);
                items.Add(output);
            }
            return output;
        }

        public TaskPropertyEntry OpenProperty(String propertyName, String propertyType)
        {
            var output = items.FirstOrDefault(x => x.propertyName == propertyName);
            if (output == null)
            {
                output = new TaskPropertyEntry()
                {
                    propertyName = propertyName,
                    propertyType = propertyType,
                    exampleValue = ""
                };
                items.Add(output);
            }
            
            return output;
        }


    }
}