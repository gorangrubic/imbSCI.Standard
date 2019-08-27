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
using imbSCI.Data.collection.nested;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using imbSCI.DataComplex.tables;
using imbSCI.Core.data;
using imbSCI.DataExtraction.Extractors.Task;

namespace imbSCI.DataExtraction.Validation.TaskValidation
{
    [Serializable]
    public class TaskValidationResultCollection
    {
        public TaskValidationResultCollection()
        {
            
        }

        public TaskValidationResultCollection(IEnumerable<TableExtractionTask> tasks)
        {
            foreach (var task in tasks)
            {
                var taskResult = new TaskValidationResult(task);
                Results.Add(task, taskResult);
             
            }
        }

        
        public aceEnumListSet<ValidationOutcome, TaskValidationResult> ResultsByOutcome { get; set; } = new aceEnumListSet<ValidationOutcome, TaskValidationResult>();

        public DataTable GetReportDataTable()
        {
            DataTable table = new DataTable("task_validation_overview");
            table.SetDescription("Task validation results overview");
            
            var task_column = table.Columns.Add("Task").SetWidth(25).SetHeading("Task name");
                        
            var column_outcome = table.Columns.Add("Outcome").SetGroup("Result").SetWidth(15);
            var column_outputType = table.Columns.Add("Output").SetGroup("Result").SetWidth(40);
            var column_message = table.Columns.Add("Comment").SetGroup("Result").SetWidth(60);

            var extractor_column = table.Columns.Add("Extractor").SetWidth(25).SetHeading("Extractor type");
            




            var column_resultEntityMin = table.Columns.Add("resultEntityMin").SetGroup("MetaTable Results").SetHeading("Entity").SetLetter("Min").SetWidth(8);

            var column_resultEntityMax = table.Columns.Add("resultEntityMax").SetGroup("MetaTable Results").SetHeading("Entity").SetLetter("Max").SetWidth(8);


            var column_resultPropertyMin = table.Columns.Add("resultPropertyMin").SetGroup("MetaTable Results").SetHeading("Property").SetLetter("Min").SetWidth(8);


            var column_resultPropertyMax = table.Columns.Add("resultPropertyMax").SetGroup("MetaTable Results").SetHeading("Property").SetLetter("Max").SetWidth(8);

          




            var column_SourceCreated = table.Columns.Add("SourceCreated").SetWidth(10).SetHeading("Sources").SetDesc("Source tables created");


            var column_MetaCreated = table.Columns.Add("MetaCreated").SetWidth(10).SetHeading("MetaTables").SetDesc("Meta tables created"); ;


            var column_Properties = table.Columns.Add("Properties").SetWidth(10).SetGroup("Property validation");

            foreach (var rpair in ResultsByOutcome.Keys)
            {
                table.Columns.Add("column_propval_" + rpair.ToString()).SetWidth(5).SetGroup("Property validation").SetHeading(rpair.ToString()).SetDesc("Number of properties with validation outcome set to [" + rpair.ToString() + "]");
            }



            var column_ScoreFactor_MetaTables = table.Columns.Add("ScoreFactor_MetaTables", typeof(Double)).SetWidth(10).SetFormat("F3").SetHeading("MetaTables").SetGroup("Score factors").SetDesc("Rate of MetaTable successful creation");


            var column_ScoreFactor_PropertyValidationRate = table.Columns.Add("ScoreFactor_PropertyValidationRate", typeof(Double)).SetWidth(10).SetFormat("F3").SetHeading("Rate").SetLetter("Property").SetGroup("Score factors").SetDesc("Property validation success rate"); 


            var column_ScoreFactor_PropertyNameDiversity = table.Columns.Add("ScoreFactor_PropertyNameDiversity", typeof(Double)).SetWidth(10).SetFormat("F3").SetHeading("Diversity").SetLetter("Property").SetGroup("Score factors").SetDesc("Property name diversity factor");


            var column_ScoreFactor_PropertyCountRange = table.Columns.Add("ScoreFactor_PropertyCountRange", typeof(Double)).SetWidth(10).SetFormat("F3").SetHeading("Stability").SetLetter("Property").SetGroup("Score factors").SetDesc("Property count stability factor");


            var column_Score = table.Columns.Add("Score", typeof(Double)).SetWidth(10).SetFormat("F5").SetHeading("Score").SetGroup("Score factors").SetDesc("Overall score - product of score factors");


          




            var column_Executed = table.Columns.Add("Training").SetGroup("Test count").SetWidth(10);
            var column_Executed_validation = table.Columns.Add("Validation").SetGroup("Test count").SetWidth(10);

                        var column_index_entryID = table.Columns.Add("index_entryID").SetWidth(5).SetHeading("e_id").SetGroup("MetaTable Description");
            var column_index_propertyID = table.Columns.Add("index_propertyID").SetWidth(5).SetHeading("p_id").SetGroup("MetaTable Description");
            var column_valueZoneY = table.Columns.Add("valueZoneY").SetWidth(5).SetHeading("vz_y").SetGroup("MetaTable Description");
            var column_valueZoneX = table.Columns.Add("valueZoneX").SetWidth(5).SetHeading("vz_x").SetGroup("MetaTable Description");
            var column_format = table.Columns.Add("format").SetWidth(10).SetHeading("Format").SetGroup("MetaTable Description");

            submethod(ResultsByOutcome[ValidationOutcome.Validated]);
            submethod(ResultsByOutcome[ValidationOutcome.Invalid]);
            submethod(ResultsByOutcome[ValidationOutcome.Modified]);
            submethod(ResultsByOutcome[ValidationOutcome.undefined]);
            



            void submethod(IEnumerable<TaskValidationResult> plist)
            {
                foreach (TaskValidationResult p in plist)
                {
                    var dr = table.NewRow();

                    var prop_results = p.PropertyValidation.GetResults();
                    foreach (var rpair in prop_results)
                    {
                        dr["column_propval_" + rpair.Key.ToString()] = rpair.Value.Count;
                    }

                    
                    dr[task_column] = p.task.name; // source.home;

                    var source = p.task;

                    dr[extractor_column] = source.ExtractorName;

                    if (source.tableDescription != null)
                    {
                        dr[column_index_entryID] = source.tableDescription.index_entryID;
                        dr[column_index_propertyID] = source.tableDescription.index_propertyID;
                        dr[column_valueZoneX] = source.tableDescription.sourceDescription.valueZone.x;
                        dr[column_valueZoneY] = source.tableDescription.sourceDescription.valueZone.y;
                        dr[column_format] = source.tableDescription.format;
                    }

                    dr[column_Executed] = p.executionCalls.TotalFrequency;
                    dr[column_Executed_validation] = p.executionCalls.GetFrequencyForItem(ExtractionTaskEngineMode.Validation);

                    dr[column_MetaCreated] = p.ValidMetaTables; //  s//ource.score.TaskRuns.Count(x => x.MetaTableCreated); //.MetaTableCreated;

                    dr[column_Properties] = p.PropertyValidation.items.Count; //. .MetaTableEntries;

                    dr[column_SourceCreated] = source.score.TaskRuns.Count(x => x.SourceTableCreated);
                    dr[column_outputType] = p.OutputType.ToString();

                    dr[column_outcome] = p.Outcome.ToString();
                    dr[column_message] = p.Message;


                    dr[column_Score] = p.Score;



                    dr[column_ScoreFactor_PropertyCountRange] = p.ScoreFactor_PropertyCountRange;



                    dr[column_ScoreFactor_PropertyNameDiversity] = p.ScoreFactor_PropertyNameDiversity;



                    dr[column_ScoreFactor_PropertyValidationRate] = p.ScoreFactor_PropertyValidationRate;



                    dr[column_ScoreFactor_MetaTables] = p.ScoreFactor_MetaTables;


                    if (p.EntryCount.IsLearned)
                    {

                        dr[column_resultEntityMin] = p.EntryCount.Minimum;



                        dr[column_resultEntityMax] = p.EntryCount.Maximum;
                    }

                    if (p.PropertyCount.IsLearned)
                    {
                        dr[column_resultPropertyMin] = p.PropertyCount.Minimum;



                        dr[column_resultPropertyMax] = p.PropertyCount.Maximum;
                    }

                    table.Rows.Add(dr);
                }
            }




            return table;
            
        }

        public DataTable propertyReport_invalid = null;
        public DataTable propertyReport_valid = null;
        public DataTable propertyReport_other = null;

        public void Publish(folderNode folder,TableExtractionTaskReporting TaskReporting, aceAuthorNotation notation)
        {

            if (TaskReporting == null)
            {
                TaskReporting = new TableExtractionTaskReporting()
                {
                    baseOutputFolder = folder,
                    currentOutputFolder = folder,
                    
                };
            }

            List<DataTable> allReportTables = new List<DataTable>();
            List<DataTable> trainingReportTables = new List<DataTable>();

            List<DataTable> validationReportTables = new List<DataTable>();

            //List<DataTable> trainingReportTables = new List<DataTable>();

            // DataSet complete_report = new DataSet();

            
            TaskValidationResult firstResult = null;
            foreach (KeyValuePair<TableExtractionTask, TaskValidationResult> pair in Results)
            {
                if (firstResult == null) firstResult = pair.Value;
                pair.Value.Publish(folder, notation);

               propertyReport_invalid= pair.Value.PropertyValidation.GetReportTable(pair.Value, propertyReport_invalid, new List<ValidationOutcome>() { ValidationOutcome.Invalid });
                propertyReport_valid = pair.Value.PropertyValidation.GetReportTable(pair.Value, propertyReport_valid, new List<ValidationOutcome>() { ValidationOutcome.Validated });
               propertyReport_other = pair.Value.PropertyValidation.GetReportTable(pair.Value, propertyReport_other, new List<ValidationOutcome>() { ValidationOutcome.Modified, ValidationOutcome.undefined });

               

                trainingReportTables.Add(pair.Value.ReportTable_Training);
                validationReportTables.Add(pair.Value.ReportTable_PropertyValudation);
              //  allReportTables.AddRange(pair.Value.ReportTables);
            }

            allReportTables.Add(propertyReport_invalid);
            allReportTables.Add(propertyReport_valid);
            allReportTables.Add(propertyReport_other);

            DataTable training_report = TaskReporting.GetTaskScore(Results.Keys).GetReportAndSave(TaskReporting.currentOutputFolder, notation);
            training_report.SetTitle("Training run results");
            training_report.GetReportAndSave(folder, notation);
            //allReportTables.Add(training_report);

            var overview_table = GetReportDataTable();
            //allReportTables.Add(overview_table);

            propertyReport_invalid.SetTitle("Invalid");
            propertyReport_valid.SetTitle("Valid");
            propertyReport_other.SetTitle("Other");

            allReportTables.GetReportAndSave(folder, notation, "property_reports");
            trainingReportTables.GetReportAndSave(folder, notation, "training_reports");
            validationReportTables.GetReportAndSave(folder, notation, "validation_reports");
            overview_table.GetReportAndSave(folder, notation);
      
            //complete_report.GetReportAndSave(folder, notation, "complete_evaluation");
        }

        /// <summary>
        /// Finalizes the validation, clears task score data
        /// </summary>
        public void FinalizeValidation()
        {
            TableExtractionTaskReporting reporting = new TableExtractionTaskReporting();

            foreach (var pair in Results)
            {
                if (pair.Value.Outcome == ValidationOutcome.undefined)
                {
                    ValidationOutcome result = pair.Value.Compute();
                    pair.Value.ReportTable_Training = reporting.GetTaskExecutionScoreReport(pair.Key);
                }

                //pair.Key.PropertyDictionary.CollectProperties(pair.Value.PropertyValidation.GetResults()[ValidationOutcome.Validated]);
               // pair.Key.score = new TableExtractionTaskScore();
            }

            
            ResultsByOutcome = new aceEnumListSet<ValidationOutcome, TaskValidationResult>();
            foreach (var pair in Results)
            {
                

                ResultsByOutcome[pair.Value.Outcome].Add(pair.Value);

                
                //pair.Key.PropertyDictionary.CollectProperties(pair.Value.PropertyValidation.GetResults()[ValidationOutcome.Validated]);
                // pair.Key.score = new TableExtractionTaskScore();
            }
        }

        public Dictionary<TableExtractionTask, TaskValidationResult> Results { get; set; } = new Dictionary<TableExtractionTask, TaskValidationResult>();

    }
}