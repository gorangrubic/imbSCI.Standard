using imbSCI.Data;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.MetaTables.Core
{
    [Serializable]
    public class MetaTablePropertyCollection : MetaTableContentItemCollectionBase<MetaTableProperty>
    {
        public Boolean IsInConflict()
        {
            List<String> propertyNames = new List<string>();
            foreach (var prop in this.items)
            {
                if (propertyNames.Contains(prop.PropertyName))
                {
                    return true;
                }
                else
                {
                    propertyNames.Add(prop.PropertyName);
                }
            }
            return false;
        }

        public MetaTableProperty Get(String propertyName, MetaTablePropertyAliasList alias = null)
        {
            MetaTableProperty output = this.items.FirstOrDefault(x => x.PropertyName.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
            if (output == null)
            {
                if (alias != null)
                {
                    MetaTablePropertyAliasEntry alias_entry = alias.Match(propertyName, false);
                    if (alias_entry != null)
                    {
                        foreach (String a in alias_entry.aliasPropertyNames)
                        {
                            output = Get(a, null);
                            if (output != null)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return output;
        }

        internal MetaTableProperty Import( MetaTableProperty item)
        {
            Int32 ec = items.Count(x => x.PropertyName == item.PropertyName);
            if (ec > 0)
            {
                item.PropertyName += ec.ToString();
            }
            Add(item);
            return item;
        }

        public MetaTableProperty Add(String propertyDisplayName, Int32 index = -1)
        {
            MetaTableProperty output = new MetaTableProperty()
            {
                DisplayName = propertyDisplayName
            };
            
            if (output.PropertyName.isNullOrEmpty())
            {
                output.PropertyName = index.ToString();
                output.DisplayName = output.PropertyName;
            } else
            {
                
            }
            if (index == -1)
            {
                index = items.Count;
            }
           
            Add(output);
            output.index = index;
            return output;
        }

        public MetaTablePropertyCollection(MetaTable table):base(table)
        {
        }

        //public MetaTablePropertyCollection(MetaTable parent) : base(parent)
        //{
        //}
    }
}