using imbSCI.DataExtraction.MetaTables.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.MetaTables.Core
{
    [Serializable]
    public abstract class MetaTableContentItemCollectionBase<T> 
    {
        
        protected MetaTable Table { get; set; } = null;


        public List<T> items { get; set; } = new List<T>();

        public Int32 Count
        {
            get
            {
                return items.Count;
            }
        }

        public Boolean Any()
        {
            return (items.Count > 0);
        }

        protected void Add(T item)
        {
            if (item == null) return;
            items.Add(item);
        }

        public MetaTableContentItemCollectionBase(MetaTable _Table)
        {
            Table = _Table;
        }
        
    }
}