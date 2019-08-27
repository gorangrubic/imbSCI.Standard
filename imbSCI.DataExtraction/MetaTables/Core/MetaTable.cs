using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.table;
using imbSCI.Core.files;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.math.range.frequency;
using imbSCI.DataComplex.extensions.data.operations;
using imbSCI.DataComplex.extensions.data.schema;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.SourceData.Analysis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.MetaTables.Core
{
    [Serializable]
    public class MetaTable
    {
        private MetaTableDescription _description;
        private MetaTablePropertyCollection _properties;

        public const String EXTRAINFOENTRYKEY_TASKNAME = "Task";
        public const String EXTRAINFOENTRYKEY_EXTRACTORNAME = "Extractor";

        public reportExpandedData ExtraInfoEntries { get; set; } = new reportExpandedData();

        public String Comment { get; set; } = "";

        public Int32 GetSize()
        {
            return properties.Count * entries.Count;
        }


        public void Save(String filepath)
        {
            objectSerialization.saveObjectToXML(this, filepath);
        }

        public static MetaTable Load(String filepath)
        {
            MetaTable output = objectSerialization.loadObjectFromXML<MetaTable>(filepath);
            output.Init();
            return output;

        }

        protected void Init()
        {
            entries = new MetaTableEntryCollection(this);
            properties = new MetaTablePropertyCollection(this);
        }

        public MetaTable()
        {
            Init();
        }

        public MetaTable(MetaTableDescription _description)
        {
            description = _description;
            Init();
        }


        protected List<String> GetPropertyNames( List<List<string>> data, Int32 index)
        {
            if (index >= 0)
            {
                List<String> output = new List<string>();
                for (int i2 = 0; i2 < data[index].Count; i2++)
                {
                    if (data[index][i2].isNullOrEmpty())
                    {
                        output.Add("P" + i2.ToString());
                    } else
                    {
                        output.Add(data[index][i2]);
                    }
                }
                return output;
            } else
            {
                List<String> output = new List<string>();
                var width = data.FirstOrDefault();
                if (width != null)
                {
                    for (int i = 0; i < width.Count; i++)
                    {
                        output.Add("P" + i.ToString());
                    }
                }
                return output;
            }
        }


        public void SetSchema(SourceTable source)
        {
            
            if (description.format == MetaTableFormatType.horizontal)
            {
                var rows = source.GetContentByRows();
                
                if (rows.Count <= description.index_propertyID)
                {
                    return;
                }
                var propertyList = GetPropertyNames(rows, description.index_propertyID);

                for (int i = 0;
                    //description.sourceDescription.valueZone.x; 
                    i < propertyList.Count; i++) //description.sourceDescription.valueZone.width + description.sourceDescription.valueZone.x; i++)
                {
                    var property = properties.Add(propertyList[i],i);
                }
            }
            else if (description.format == MetaTableFormatType.vertical)
            {
                List<List<string>> columns = source.GetContentByColumns();
                

                if (columns.Count <= description.index_propertyID)
                {
                    return;
                }

                var propertyList = GetPropertyNames(columns, description.index_propertyID);

                for (int i = 0;
                    // description.sourceDescription.valueZone.y;
                    i < propertyList.Count; i++) // description.sourceDescription.valueZone.height + description.sourceDescription.valueZone.y; i++)
                {
                    
                    var property = properties.Add(propertyList[i], i);
                }
            }

            if (description.index_entryID < 0)
            {
                var property = properties.Add(description.entryIDPropertyName, description.index_entryID);
                
            }
        }

        [XmlIgnore]
        protected SourceTable sourceTable { get; set; } = null;

        public void SetEntriesAndLinkToSource(SourceTable source)
        {
            Int32 Skip = description.EntrySkipCount;
            List<List<SourceTableCell>> data = source.GetContentCells(description.format == MetaTableFormatType.vertical);
            sourceTable = source;

            for (int i = Skip; i < data.Count; i++)
            {
                if (i != description.index_propertyID) entries.CreateEntry(data[i], true);
            }

        }

        /// <summary>
        /// Transforms the scraped content (<see cref="SourceTable"/>) into entries
        /// </summary>
        /// <param name="source">The source.</param>
        public void SetEntries(SourceTable source)
        {
            List<List<string>> data = null;
            Int32 Skip = description.EntrySkipCount;

            if (description.format == MetaTableFormatType.horizontal)
            {
                data = source.GetContentByRows();    //if (i2 >= description.sourceDescription.valueZone.y)
            }
            else if (description.format == MetaTableFormatType.vertical)
            {
                data = source.GetContentByColumns(); //for (int i = description.sourceDescription.valueZone.x; i < description.sourceDescription.valueZone.width + description.sourceDescription.valueZone.x; i++)
            }

            for (int i = Skip; i < data.Count; i++)
            {
                if (i != description.index_propertyID) entries.CreateEntry(data[i], true);
            }
        }

        public List<String> GetAllValuesForProperty(String propertyName)
        {
            var property = properties.Get(propertyName);  //properties.FirstOrDefault(x => x.PropertyName == propertyName || x.DisplayName == propertyName);

            return entries.GetAllValuesForProperty(property);
        }


        public Int32 ApplySchema(IEnumerable<MetaTableProperty> schemaProperties)
        {
            Int32 c = 0;
            foreach (MetaTableProperty sp in schemaProperties)
            {
                MetaTableProperty p = properties.Get(sp.PropertyName);
                if (p != null)
                {
                    p.Learn(sp);
                    c++;
                }
            }

            return c;
        }
      
        /// <summary>
        /// Allows <see cref="schema"/> upgrade after additional analysis of the stored <see cref="entries"/>. This should be called after <see cref="SetEntries(SourceTable)"/>. 
        /// </summary>
        /// <param name="analyser">Content analyser to be used</param>
        public void RefineSchema(CellContentAnalysis analyser)
        {
            
          //  DataTable output = new DataTable("schemaTable");

            foreach (var property in properties.items)
            {
                var propData = entries.GetAllValuesForProperty(property);

                if (propData.Count == 0)
                {

                }
                else
                {
                    RefinedPropertyStats propertyStats = new RefinedPropertyStats();

                    foreach (var data in propData)
                    {
                        CellContentInfo info = analyser.DetermineContentType(data);

                        propertyStats.Assign(info);
                    }

                    propertyStats.Compute();

                    propertyStats.Deploy(property);

                  //  GetColumn(property, output, propertyStats);
                }
            }

        }

      

        public DataColumn GetColumn(MetaTableProperty property, DataTable output, RefinedPropertyStats propertyStats = null)
        {
            if (!output.Columns.Contains(property.PropertyName))
            {
                DataColumn cl = output.Columns.Add(property.PropertyName);

                cl.SetHeading(property.DisplayName);
                cl.SetValueType(property.GetValueType());

                if (!property.ValueFormat.isNullOrEmpty())
                {
                    cl.SetFormat(property.ValueFormat);
                }
                cl.SetLetter(property.PropertyName);

                if (propertyStats != null)
                {
                    cl.SetWidth(Math.Min(propertyStats.max_width, 30));
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("Content: " + property.ContentType.ToString());
                //sb.Append("Property name: " + property.PropertyName);
                if (!property.ValueTypeName.isNullOrEmpty())
                {
                    sb.Append(" ValueType: " + property.ValueTypeName);
                }

                cl.SetDesc(sb.ToString());

                return cl;
            } else
            {
                return null;
            }
        }

        [XmlIgnore]
        public Boolean IsValid
        {
            get
            {
                if (properties.Count == 0) return false;
                if (entries.Count == 0) return false;
                if (properties.items.Any(x => x == null)) return false;
                if (entries.items.Any(x => x == null)) return false;

                return true;
            }
        }


        public DataTable GetDataTable(String tableName)
        {
            DataTable output = new DataTable(tableName);

            if (description != null)
            {

                output.SetAdditionalInfoEntry("Orientation", description.format.ToString(), "Orientation of source data table");
                output.SetAdditionalInfoEntry("Entry UID", description.entryIDPropertyName, "Name of entry UID property");

                output.SetAdditionalInfoEntry("ZoneX", description.sourceDescription.valueZone.x, "");
                output.SetAdditionalInfoEntry("ZoneY", description.sourceDescription.valueZone.y, "");

                output.SetAdditionalInfoEntry("PropertyID", description.index_propertyID, "");
                output.SetAdditionalInfoEntry("EntryID", description.index_entryID, "");
                output.SetAdditionalInfoEntry("Transposed", description.IsTransposed, "");
                
            }

            foreach (var pair in ExtraInfoEntries)
            {
                output.SetAdditionalInfoEntry(pair.key, pair.value, pair.description);
                if (pair.key == nameof(ContentChunk.description)) output.SetDescription(pair.value);
            }

            foreach (MetaTableProperty property in properties.items)
            {
                var cl = GetColumn(property, output, null);
            }


            output.SetAdditionalInfoEntry("Properties", properties.items.Count, "Number of properties");
            output.SetAdditionalInfoEntry("Entries", entries.items, "Number of entries");

            foreach (MetaTableEntry entry in entries.items)
            {
                if (entry == null) continue;
                var dr = output.NewRow();

                foreach (var property in properties.items)
                {
                    if (output.Columns.Contains(property.PropertyName))
                    {
                        dr[property.PropertyName] = entry.GetOutputValue(property);                        
                    }
                }

                output.Rows.Add(dr);
            }

            return output;
        }

        [XmlIgnore]
        public MetaTableDescription description
        {
            get
            {
                return _description;
            }
            protected set
            {
                _description = value;
            }
        }

        [XmlIgnore]
        public MetaTableEntryCollection entries { get; protected set; }
        

        [XmlIgnore]
        public MetaTablePropertyCollection properties
        {
            get
            {

                return _properties;
            }

            protected set
            {

                _properties = value;
            }
        }
    }
}