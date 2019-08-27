using imbSCI.Core.collection;
using imbSCI.Core.data;
using imbSCI.Core.files.folders;
using imbSCI.Core.math.range.frequency;
using imbSCI.Core.reporting.render;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.SourceData.Analysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.MetaTables.Descriptors
{
    public class MetaTablePropertyCollectionAggregation
    {
        
        public static List<MetaTableProperty> MergePropertyDefinitionsFromTables(IEnumerable<MetaTable> tables)
        {
            MetaTablePropertyCollectionAggregation tool = new MetaTablePropertyCollectionAggregation();
            return tool.ProcessTables(tables);
        }

        //Dictionary<String, RefinedPropertyStats> PropertyStats { get; set; } = new Dictionary<string, RefinedPropertyStats>();
        //Dictionary<String, MetaTableProperty> PropertyDrafts { get; set; } = new Dictionary<string, MetaTableProperty>();
        ListDictionary<String, MetaTableProperty> PropertySets { get; set; } = new ListDictionary<string, MetaTableProperty>();

        public List<MetaTableProperty> ProcessTables(IEnumerable<MetaTable> tables)
        {
            List<MetaTableProperty> output = new List<MetaTableProperty>();

            foreach (MetaTable t in tables)
            {
                Process(t);
            }

            foreach (var pair in PropertySets)
            {
                InstanceMergerByFrequencies<MetaTableProperty> merger = new InstanceMergerByFrequencies<MetaTableProperty>(pair.Value);
                MetaTableProperty property = merger.GetMergedInstance();
                output.Add(property);
            }

            return output;
        }

        protected void Process(MetaTable table)
        {
            foreach (var property in table.properties.items)
            {
               // var propData = table.entries.GetAllValuesForProperty(property);

                PropertySets[property.PropertyName].Add(property);

                /*
                if (propData.Count == 0)
                {

                }
                else
                {

                    if (!PropertyStats.ContainsKey(property.PropertyName))
                    {
                        PropertyStats.Add(property.PropertyName, new RefinedPropertyStats());
                        PropertyDrafts.Add(property.PropertyName, property);
                    }

                    RefinedPropertyStats propertyStats = PropertyStats[property.PropertyName];

                    foreach (var data in propData)
                    {
                        CellContentInfo info = analyser.DetermineContentType(data);

                        propertyStats.Assign(info);
                    }


                    
                }*/
            }
        }

    }
}