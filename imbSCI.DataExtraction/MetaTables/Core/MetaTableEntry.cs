using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.MetaTables.Core
{
    [Serializable]
    public class MetaTableEntry
    {
      //  public String ID { get; set; }

        internal Dictionary<String, String> properties { get; set; } = new Dictionary<string, string>();

        internal Dictionary<String, SourceTableCell> sourceCellLinks { get; set; } = new Dictionary<string, SourceTableCell>();

        public void StoreAllValues(MetaTableEntry source)
        {
            foreach (var pair in source.properties)
            {
                StoreValue(pair.Key, pair.Value);
            }
            
        }

        public void StoreValue(String PropertyName, String value, SourceTableCell sourceCell = null)
        {
            if (sourceCell != null)
            {
                if (!sourceCellLinks.ContainsKey(PropertyName))
                {
                   sourceCellLinks.Add(PropertyName, sourceCell);
                }
            }

            if (!properties.ContainsKey(PropertyName)) properties.Add(PropertyName, "");
            properties[PropertyName] = value;
        }

        public Boolean OverwriteStoredValue(String PropertyName, String value, SourceTableCell sourceCell = null)
        {
            if (sourceCell != null)
            {
                if (sourceCellLinks.ContainsKey(PropertyName))
                {
                    return false;
                    
                }
                else
                {
                    sourceCellLinks.Add(PropertyName, sourceCell);
                }
            }

            if (!properties.ContainsKey(PropertyName)) return false;
            properties[PropertyName] = value;
            return true;
        }

        

        public String GetStoredValue(String PropertyName)
        {
            if (!properties.ContainsKey(PropertyName)) return "";  //property.GetDefaultData();
            return properties[PropertyName];
        }

        public Boolean HasLinkedCell(MetaTableProperty property)
        {
            if (!sourceCellLinks.ContainsKey(property.PropertyName)) return false;
            if (sourceCellLinks[property.PropertyName] == null) return false;
            return true;
        }


        public SourceTableCell GetLinkedCell(MetaTableProperty property)
        {
             if (!HasLinkedCell(property)) return null;
            return sourceCellLinks[property.PropertyName];
        }
     
        public String GetStoredValue(MetaTableProperty property)
        {
            if (!properties.ContainsKey(property.PropertyName)) return property.GetDefaultData();
            return properties[property.PropertyName];
        }

        public Boolean HasValueFor(String propertyName)
        {
            return properties.ContainsKey(propertyName);
        }

        public Boolean HasValueFor(MetaTableProperty property)
        {
            return properties.ContainsKey(property.PropertyName);
        }

        public Object GetOutputValue(MetaTableProperty property)
        {
            if (!properties.ContainsKey(property.PropertyName)) return property.GetDefaultData();
            return property.GetValue(properties[property.PropertyName]);
        }

        public MetaTableEntry()
        {
        }
    }
}