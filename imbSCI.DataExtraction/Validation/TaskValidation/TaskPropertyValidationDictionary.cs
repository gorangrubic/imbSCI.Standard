using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.text;
using imbSCI.Core.files.folders;
using imbSCI.Core.math;
using imbSCI.Core.math.range;
using imbSCI.Core.math.similarity;

using imbSCI.Data;
using imbSCI.DataExtraction.Extractors.Task;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.SourceData.Analysis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace imbSCI.DataExtraction.Validation.TaskValidation
{
    [Serializable]
    public class TaskPropertyValidationDictionary:TaskPropertyDictionary
    {
        
        public DataTable GetReportTable(TaskValidationResult task, DataTable existingTable=null, List<ValidationOutcome> outcomes=null)
        {
            var table = new DataTable(task.task.name + "_prop_validation");

            table.SetDescription("Property validation results for task [" + task.task.name + "]");


            var column_task = table.Columns.Add("Task").SetWidth(25);

           
            var column_propertyName = table.Columns.Add("Property").SetGroup("Declaration").SetWidth(20);

            var column_propertyType = table.Columns.Add("Type").SetGroup("Declaration").SetWidth(10);

            var column_contentType = table.Columns.Add("Content").SetGroup("Declaration").SetWidth(10);

            var column_valueformat = table.Columns.Add("Format").SetGroup("Declaration").SetWidth(10);

            var column_frequency = table.Columns.Add("Frequency").SetWidth(10);

            var column_outcome = table.Columns.Add("Outcome").SetGroup("Result").SetWidth(10);

            var column_message = table.Columns.Add("Comment").SetGroup("Result").SetWidth(50);
            




            
            var column_linkedNode = table.Columns.Add("Node").SetWidth(10).SetGroup("Values");






            var column_DistinctValues = table.Columns.Add("Distinct").SetGroup("Values").SetWidth(10);
            var column_examplevalues1 = table.Columns.Add("Example1").SetGroup("Values").SetWidth(25).SetHeading("Example").SetDesc("Example value found at the property");
            var column_examplevalues2 = table.Columns.Add("Example2").SetGroup("Values").SetWidth(25).SetHeading("Example").SetDesc("Example value found at the property");
            var column_examplevalues3 = table.Columns.Add("Example3").SetGroup("Values").SetWidth(25).SetHeading("Example").SetDesc("Example value found at the property");
            var column_examplevalues4 = table.Columns.Add("Example4").SetGroup("Values").SetWidth(25).SetHeading("Example").SetDesc("Example value found at the property");
            var column_examplevalues5 = table.Columns.Add("Example5").SetGroup("Values").SetWidth(25).SetHeading("Example").SetDesc("Example value found at the property");

            var column_SpamPropertyMeasure = table.Columns.Add("SpamPropertyMeasure").SetGroup("Values").SetHeading("Spam").SetDesc("Measure describing probability that this property is a spam property").SetFormat("F3").SetWidth(10);


            var column_allcontenttypes = table.Columns.Add("All types").SetWidth(100);






            var column_xpath = table.Columns.Add("XPath").SetWidth(100);

            



            var results = GetResults();
            foreach (var rpair in results)
            {
                table.SetAdditionalInfoEntry(rpair.Key.ToString(), rpair.Value.Count, "Number of properties having outcome [" + rpair.Key.ToString() + "]");
            }


            DataTable targetTable = table;
            if (existingTable!=null)
            {
                targetTable = existingTable;
            }

            if (outcomes == null)
            {
                outcomes = new List<ValidationOutcome>() { ValidationOutcome.Validated, ValidationOutcome.Modified, ValidationOutcome.Invalid, ValidationOutcome.undefined };
            }

            foreach (var oc in outcomes)
            {
                submethod(results[oc], targetTable);
            }

           
            void submethod(IEnumerable<TaskPropertyValidation> plist, DataTable _targetTable=null)
            {
                foreach (TaskPropertyValidation p in plist)
                {
                    var dr = _targetTable.NewRow();

                    dr[column_task.ColumnName] = task.task.name;
                    dr[column_SpamPropertyMeasure.ColumnName] = p.SpamPropertyMeasure;

                    List<string> d_vals = p.ValueCounter.GetDistinctItems().trimToLimit(25, true, "...", false, true);

                    dr[column_DistinctValues.ColumnName] = p.DistinctValues;

                    dr[column_examplevalues1.ColumnName] = d_vals.ElementAtOrDefault(0);
                    dr[column_examplevalues2.ColumnName] = d_vals.ElementAtOrDefault(1);
                    dr[column_examplevalues3.ColumnName] = d_vals.ElementAtOrDefault(2);
                    dr[column_examplevalues4.ColumnName] = d_vals.ElementAtOrDefault(3);
                    dr[column_examplevalues5.ColumnName] = d_vals.ElementAtOrDefault(4);
                               
                    dr[column_propertyType.ColumnName] = p.item.ValueTypeName; // source.propertyType;

                    dr[column_frequency.ColumnName] = p.Frequency; //.GetRatio(task.score.metaPropertyNameCounter.DistinctCount());

                    dr[column_SpamPropertyMeasure.ColumnName] = p.SpamPropertyMeasure; //f.GetRatio(validation_calls); //source.persistance;

                    dr[column_propertyName.ColumnName] = p.item.PropertyName;// source.propertyName;

                    dr[column_message.ColumnName] = p.Message; // source.message;

                    dr[column_outcome.ColumnName] = p.Outcome;
                    dr[column_linkedNode.ColumnName] = p.LinkedNodes; // source.linkedNode;

                    dr[column_contentType.ColumnName] = p.item.ContentType.ToString(); // source.contentType;
                    dr[column_valueformat.ColumnName] = p.item.ValueFormat; // source.valueformat;
                    
                    dr[column_allcontenttypes.ColumnName] = p.item.AllContentTypes.getEnumListFromFlags<CellContentType>().toCsvInLine();
                    dr[column_xpath.ColumnName] = p.XPath; //.xpath;
                    _targetTable.Rows.Add(dr);
                }
            }

            return targetTable;
        }

        public Double GetPropertyNameDiversityScore()
        {
            List<String> nGrams = new List<string>();
            foreach (var pair in validations)
            {
                nGrams.AddRange(setAnalysisTools<Char>.getStringNGramSet(pair.Value.item.PropertyName));
            }

            var distinct = nGrams.Distinct().ToList();

            Double propertyNameDiversityFactor = (1 - 1.GetRatio(distinct.Count + 1));
            return propertyNameDiversityFactor;
        }


        public Double GetScore()
        {

            

            Int32 part = 0;

            Int32 whole = validations.Count;
            foreach (var pair in validations)
            {
                switch (pair.Value.Outcome)
                {
                    case ValidationOutcome.Invalid:
                        break;
                    case ValidationOutcome.Modified:
                    case ValidationOutcome.Validated:
            
                        part++;
                        break;
                    case ValidationOutcome.undefined:
                        break;
                }
            }

             //distinct.Count.GetRatio(nGrams.Count);

            Double score = part.GetRatio(whole, 0, 0);
            return score;
        }


        public Dictionary<ValidationOutcome, List<TaskPropertyValidation>> GetResults()
        {
            Dictionary<ValidationOutcome, List<TaskPropertyValidation>> output = new Dictionary<ValidationOutcome, List<TaskPropertyValidation>>();

            output.Add(ValidationOutcome.Invalid, new List<TaskPropertyValidation>());
            output.Add(ValidationOutcome.Modified, new List<TaskPropertyValidation>());
            output.Add(ValidationOutcome.undefined, new List<TaskPropertyValidation>());
            output.Add(ValidationOutcome.Validated, new List<TaskPropertyValidation>());

            foreach (var pair in validations)
            {
                if (!output.ContainsKey(pair.Value.Outcome)) output.Add(pair.Value.Outcome, new List<TaskPropertyValidation>());
                output[pair.Value.Outcome].Add(pair.Value);
            }


            return output;
        }

        public Dictionary<String, TaskPropertyValidation> validations { get; set; } = new Dictionary<string, TaskPropertyValidation>();

        public Dictionary<String, TaskPropertyValidation> GetUnresolved()
        {
            var l = validations.Values.ToList();
            Dictionary<String, TaskPropertyValidation> output = new Dictionary<string, TaskPropertyValidation>();

            var list = l.Where(x => x.Outcome == ValidationOutcome.undefined).ToList();
            foreach (var li in list)
            {
                output.Add(li.item.PropertyName, li);
            }
            return output;
        }


        public override List<MetaTableProperty> CollectProperties(MetaTable metaTable)
        {
            List<MetaTableProperty> discovered = base.CollectProperties(metaTable);
            foreach (var p in discovered)
            {
                if (!validations.ContainsKey(p.PropertyName)) validations.Add(p.PropertyName, new TaskPropertyValidation(p));
            }

            return discovered;
        }
    }
}