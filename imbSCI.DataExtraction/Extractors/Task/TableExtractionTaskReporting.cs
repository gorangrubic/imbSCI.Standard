using imbSCI.Core.extensions.table;
using imbSCI.Core.files.folders;
using imbSCI.Core.math;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.math.range.frequency;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.NodeQuery;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Extractors.Task
{
    public class TableExtractionTaskReporting
    {
        public TableExtractionTaskReporting()
        {

        }

        public folderNode SetCurrentOutputFolder(ExtractionTaskEngineMode engineMode, String contextName)
        {
           currentOutputFolder = baseOutputFolder.Add($"tasks_{engineMode}_" + contextName, contextName, $"{engineMode} results for tasks");
            return currentOutputFolder;
        }

        [XmlIgnore]
        public folderNode baseOutputFolder { get; set; }

        [XmlIgnore]
        public folderNode currentOutputFolder { get; set; }

        public DataTable GetTaskExecutionScoreReport(TableExtractionTask task)
        {
            DataTable table = new DataTable(task.name + "exe");

            

            var column_sourceFilepath = table.Columns.Add("sourceFilepath").SetWidth(60).SetHeading("Source filepath");
            var column_sourceTable_width = table.Columns.Add("sourceTable_width").SetWidth(5).SetHeading("st_w").SetDesc("Source table width");
            var column_sourceTable_height = table.Columns.Add("sourceTable_height").SetWidth(5).SetHeading("st_h");
            var column_metaTable_entries = table.Columns.Add("metaTable_entries").SetWidth(5).SetHeading("mt_e");
            var column_metaTable_properties = table.Columns.Add("metaTable_properties").SetWidth(5).SetHeading("mt_p");
            var column_index_entryID = table.Columns.Add("index_entryID").SetWidth(5).SetHeading("e_id");
            var column_index_propertyID = table.Columns.Add("index_propertyID").SetWidth(5).SetHeading("p_id");

            var column_valueZoneY = table.Columns.Add("valueZoneY").SetWidth(5).SetHeading("vz_y");
            var column_valueZoneX = table.Columns.Add("valueZoneX").SetWidth(5).SetHeading("vz_x");

            var column_format = table.Columns.Add("format").SetWidth(20).SetHeading("Format");

            foreach (TableExtractionTaskScoreEntry source in task.score.TaskRuns)
            {
              //  if (!source.IsReportEnabled) continue;

                var dr = table.NewRow();

                FileInfo fi = new FileInfo(source.source.filepath);

                String fp = fi.Directory.Name.add(fi.Name, Path.DirectorySeparatorChar);
                
                dr[column_sourceFilepath] = fp;


                if (source.metaTable != null)
                {
                    dr[column_metaTable_properties] = source.metaTable.Sum(x=>x.properties.Count); //.metaTable_properties;
                    dr[column_metaTable_entries] = source.metaTable.Sum(x => x.entries.Count); // .metaTable_entries;
                }

                if (source.sourceTable != null)
                {
                    dr[column_sourceTable_height] = source.sourceTable.Average(x=>x.Height); //.sourceTable_height;

                    dr[column_sourceTable_width] = source.sourceTable.Average(x => x.Width); //.sourceTable_width;
                }


                if (source.metaTableDescription != null)
                {

                    dr[column_index_entryID] = source.metaTableDescription.index_entryID;

                    dr[column_index_propertyID] = source.metaTableDescription.index_propertyID;

                    dr[column_valueZoneX] = source.metaTableDescription.sourceDescription.valueZone.x;

                    dr[column_valueZoneY] = source.metaTableDescription.sourceDescription.valueZone.y;

                    dr[column_format] = source.metaTableDescription.format;

                }

                //dr[column_Executed] = source.score.ExecutionCount;

                //dr[column_MetaCreated] = source.score.TaskRuns.Count(x => x.MetaTableCreated); //.MetaTableCreated;

                //dr[column_Properties] = source.PropertyDictionary.items.Count; //. .MetaTableEntries;

                //dr[column_SourceCreated] = source.score.TaskRuns.Count(x => x.SourceTableCreated);

                table.Rows.Add(dr);
   
            }

            table.AddExtra("Task: " + task.name);
            if (!task.Comment.isNullOrEmpty()) table.AddExtra("Comment: " + task.Comment);
            table.AddExtra("Extractor: " + task.ExtractorName);
            

            return table;

        }

        public DataTable GetTaskScore(IEnumerable<TableExtractionTask> tasks, String tablenameSufix="")
        {
            DataTable table = new DataTable("TaskRunResults" + tablenameSufix);

            var task_column = table.Columns.Add("Task").SetWidth(20).SetHeading("Task name");
            var extractor_column = table.Columns.Add("Extractor").SetWidth(20).SetHeading("Extractor type");

            var column_index_entryID = table.Columns.Add("index_entryID").SetWidth(5).SetHeading("e_id");
            var column_index_propertyID = table.Columns.Add("index_propertyID").SetWidth(5).SetHeading("p_id");

            var column_valueZoneY = table.Columns.Add("valueZoneY").SetWidth(5).SetHeading("vz_y");
            var column_valueZoneX = table.Columns.Add("valueZoneX").SetWidth(5).SetHeading("vz_x");


            var column_format = table.Columns.Add("format").SetWidth(10).SetHeading("Format");
           
            var column_Executed = table.Columns.Add("Executed").SetWidth(10);

            var column_SourceCreated = table.Columns.Add("SourceCreated").SetWidth(10);


            var column_MetaCreated = table.Columns.Add("MetaCreated").SetWidth(10);


            var column_Properties = table.Columns.Add("Properties").SetWidth(10);


            foreach (TableExtractionTask t in tasks)
            {

                var dr = table.NewRow();

                dr[task_column] = t.name; // source.home;

                var source = t;

                dr[extractor_column] = source.ExtractorName;

                if (source.tableDescription != null)
                {

                    dr[column_index_entryID] = source.tableDescription.index_entryID;

                    dr[column_index_propertyID] = source.tableDescription.index_propertyID;

                    dr[column_valueZoneX] = source.tableDescription.sourceDescription.valueZone.x;

                    dr[column_valueZoneY] = source.tableDescription.sourceDescription.valueZone.y;

                    dr[column_format] = source.tableDescription.format;

                }

                dr[column_Executed] = source.score.ExecutionCount;

                dr[column_MetaCreated] = source.score.TaskRuns.Count(x => x.MetaTableCreated); //.MetaTableCreated;

                dr[column_Properties] = source.PropertyDictionary.items.Count; //. .MetaTableEntries;

                dr[column_SourceCreated] = source.score.TaskRuns.Count(x => x.SourceTableCreated);

                table.Rows.Add(dr);
   
            }

            return table;

        }

        
    }
}