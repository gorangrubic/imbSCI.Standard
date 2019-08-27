using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.MetaTables.Core
{
    [Serializable]
    public class MetaTableEntryCollection : MetaTableContentItemCollectionBase<MetaTableEntry>
    {
        public Dictionary<String, MetaTableEntry> GetEntryDictionary(String UIDPropertyName)
        {
            Dictionary<String, MetaTableEntry> output = new Dictionary<string, MetaTableEntry>();

            foreach (var e in this.items)
            {
                var id = e.GetStoredValue(UIDPropertyName);
                output.Add(id, e);
            }

            return output;
        }

        public void ExpandEntries(MetaTablePropertyCollection properties)
        {
            foreach (var e in this.items)
            {
                foreach (var p in properties.items)
                {
                    if (!e.properties.ContainsKey(p.PropertyName))
                    {
                        e.properties.Add(p.PropertyName, p.GetDefaultData());
                    }
                }
            }
        }

        public List<String> GetAllValuesForProperty(MetaTableProperty property)
        {
            List<String> output = new List<string>();

            if (property == null) return output;
            if (!Any()) return output;

            foreach (MetaTableEntry entry in items)
            {
                if (entry != null)
                {
                    output.Add(entry.GetStoredValue(property));
                }
            }

            return output;
        }

        /*
        public MetaTableEntry CreateEntry(String uid)
        {
            MetaTableEntry output = new MetaTableEntry()
            {
                ID = uid
            };

            foreach (MetaTableProperty property in properties)
            {
                output.properties.Add(property.PropertyName, property.GetDefaultData());
            }

            return output;
        }*/

        public MetaTableEntry CreateEntry(MetaTableEntry sourceEntry, Boolean Add=false)
        {
            MetaTableEntry output = new MetaTableEntry();
            output.StoreAllValues(sourceEntry);

            if (Add) items.Add(output);

            return output;
        }

        public MetaTableEntry CreateEntry(List<SourceTableCell> entrySourceData, Boolean Add = false)
        {
            return createEntry<SourceTableCell>(entrySourceData, Add);
        }

        protected MetaTableEntry createEntry<T>(List<T> entrySourceData, Boolean Add = false) 
        {
            MetaTableEntry output = new MetaTableEntry();

            Type dataType = typeof(T);

            foreach (MetaTableProperty property in Table.properties.items)
            {
                String valueToStore = "";
                if (property.index < 0)
                {
                    valueToStore = items.Count.ToString();
                    output.StoreValue(property.PropertyName, valueToStore, null);
                } else if (property.index < entrySourceData.Count)
                {
                    switch (dataType.Name)
                    {
                        case nameof(String):
                            Object v = entrySourceData[property.index];
                            valueToStore = (String) v;
                            output.StoreValue(property.PropertyName, valueToStore, null);
                            break;
                        case nameof(SourceTableCell):
                            SourceTableCell c = entrySourceData[property.index] as SourceTableCell;
                            valueToStore = c.Value;
                            output.StoreValue(property.PropertyName, valueToStore, c);
                            break;
                        default:
                            throw new NotImplementedException();
                            break;
                    }
                } else
                {
                    valueToStore = property.GetDefaultData();
                    output.StoreValue(property.PropertyName, valueToStore, null);
                }

               
            }

            if (Add) items.Add(output);

            return output;
        }


        public MetaTableEntry CreateEntry(List<String> entrySourceData, Boolean Add=false)
        {
            return createEntry<String>(entrySourceData, Add);

            //MetaTableEntry output = new MetaTableEntry();

            //foreach (MetaTableProperty property in Table.properties.items)
            //{
            //    String valueToStore = "";
            //    if (property.index < 0)
            //    {
            //        valueToStore = items.Count.ToString();
            //    } else if (property.index < entrySourceData.Count)
            //    {
            //        valueToStore = entrySourceData[property.index];
            //    } else
            //    {
            //        valueToStore = property.GetDefaultData();
            //    }

            //    output.StoreValue(property.PropertyName, valueToStore);
            //}

            //if (Add) items.Add(output);

            
        }


        public MetaTableEntryCollection(MetaTable _Table):base(_Table)
        {
        }

    }
}