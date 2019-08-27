using HtmlAgilityPack;
using imbSCI.Core.data;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.text;
using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Core.math;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.math.range.finder;
using imbSCI.Core.math.range.frequency;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using imbSCI.DataComplex.tables;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Extractors.Task;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.NodeQuery;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Validation.TaskValidation
{
    [Serializable]
    public class TaskValidationResult:ValidationResultBase
    {
        public frequencyCounter<String> metaPropertyNameCounter { get; set; } = new frequencyCounter<string>();
        public frequencyCounter<ExtractionTaskEngineMode> executionCalls { get; set; } = new frequencyCounter<ExtractionTaskEngineMode>();

        public rangeFinder PropertyCount { get; set; } = new rangeFinder("Properties");
        public rangeFinder EntryCount { get; set; } = new rangeFinder("Entries");

        public TaskPropertyValidationDictionary PropertyValidation { get; set; } = new TaskPropertyValidationDictionary();

        public Int32 ValidMetaTables { get; set; } = 0;


        public List<DataTable> ReportTables { get; set; } = new List<DataTable>();

        public DataTable ReportTable_Training { get; set; }
        public DataTable ReportTable_PropertyValudation { get; set; }


        public Int32 ValidationRunCount { get; set; } = 0;

        //public List<TaskPropertyValidation> ValidPropertyList { get; set; } = new List<TaskPropertyValidation>();
        //public List<TaskPropertyValidation> InValidPropertyList { get; set; } = new List<TaskPropertyValidation>();

       // public TaskPropertyDictionary PropertyDictionary { get; set; } = new TaskPropertyDictionary();

        public void Publish(folderNode folder, aceAuthorNotation notation)
        {
            objectSerialization.saveObjectToXML(task, folder.pathFor(task.name + "_declaration.xml", imbSCI.Data.enums.getWritableFileMode.overwrite, "Declaration of task [" + task.name + "]"));

            ReportTable_PropertyValudation = PropertyValidation.GetReportTable(this);
            var pvr = PropertyValidation.GetResults();
            foreach (var pv in pvr[ValidationOutcome.Invalid]) {
                reporter.AppendLine(pv.item.PropertyName + " : " + pv.item.DisplayName);
                reporter.AppendLine(pv.Outcome +  " : " + pv.Message);
                reporter.nextTabLevel();

                reporter.AppendParagraph(pv.reporter.GetContent());
                reporter.prevTabLevel();
            }
            reporter.ReportSave(folder, task.name + "_validation_log.txt");


            task.ExtractorCustomizationSettings.ReportSave(folder, task.name + "_settings", "Custom settings for extractor [" + task.ExtractorName + "] stored in task [" + task.name + "]");

            //foreach (var t in ReportTables)
            //{
            //    t.GetReportAndSave(folder, notation, task.name);
            //}

            
        }

        public Double ScoreFactor_MetaTables = 0;
        public Double ScoreFactor_PropertyValidationRate = 0;
        public Double ScoreFactor_PropertyNameDiversity = 0;
        public Double ScoreFactor_PropertyCountRange = 0;
        public Double Score = 0;

        public Double GetScore()
        {
            ScoreFactor_MetaTables = ValidMetaTables.GetRatio(ValidationRunCount, 0, 0);
            ScoreFactor_PropertyCountRange = 1.GetRatio(PropertyCount.Range);
            ScoreFactor_PropertyValidationRate = Math.Pow(PropertyValidation.GetScore(), 3);
            ScoreFactor_PropertyNameDiversity = Math.Pow(PropertyValidation.GetPropertyNameDiversityScore(), 3);

            Double output = ScoreFactor_MetaTables * ScoreFactor_PropertyCountRange * ScoreFactor_PropertyValidationRate * ScoreFactor_PropertyNameDiversity;
            Score = output;

            return output;
        }

        public ValidationOutcome Compute()
        {
            List<MetaTable> MetaTables = new List<MetaTable>();
            List<TableExtractionTaskScoreEntry> ValidationRuns = new List<TableExtractionTaskScoreEntry>();

           

            foreach (TableExtractionTaskScoreEntry run in task.score.TaskRuns)
            {
                executionCalls.Count(run.executionMode);
                switch (run.executionMode)
                {
                    default:
                        break;
                    case ExtractionTaskEngineMode.Validation:
                        ValidationRunCount++;
                        ValidationRuns.Add(run);
                        if (run.MetaTableCreated)
                        {
                            MetaTables.AddRange(run.metaTable.Where(x => x.IsValid));
                        } else
                        {

                        }
                        break;
                }
            }

            if (!MetaTables.Any())
            {
                return SetOutcome(ValidationOutcome.Invalid, "Failed to produce any meta table");
            }

            ValidMetaTables = MetaTables.Count;

            foreach (MetaTable table in MetaTables)
            {
                PropertyCount.Learn(table.properties.Count);
                EntryCount.Learn(table.entries.Count);
                PropertyValidation.CollectProperties(table);

                var unresolved = PropertyValidation.GetUnresolved();

                foreach (MetaTableEntry entry in table.entries.items)
                {
                    foreach (MetaTableProperty property in table.properties.items)
                    {
                        metaPropertyNameCounter.Count(property.PropertyName);

                        if (unresolved.ContainsKey(property.PropertyName))
                        {
                            var prop_validation = unresolved[property.PropertyName];

                            if (property.PropertyName.isStartWithNumber())
                            {
                                prop_validation.SetOutcome(ValidationOutcome.Invalid, "Property name [" + property.PropertyName + "] starts with number");
                                continue;
                            }

                            prop_validation.ValueCounter.Count(entry.GetStoredValue(property));

                            if (entry.HasLinkedCell(property))
                            {
                                var sourceCell = entry.GetLinkedCell(property);
                                if (sourceCell.SourceNode != null)
                                {
                                    if (prop_validation.XPath.isNullOrEmpty())
                                    {
                                        prop_validation.XPath = sourceCell.SourceNode.XPath;
                                    }
                                    if (sourceCell.SourceNode.HasChildNodes)
                                    {
                                        List<HtmlNode> descendant = sourceCell.SourceNode.DescendantNodes().ToList();


                                        if (descendant.Count(x=>!x.Name.StartsWith("#")) > 1)
                                        {
                                            String signature = descendant.Select(x => x.Name).toCsvInLine();

                                            prop_validation.reporter.AppendLine(sourceCell.SourceCellXPath);
                                            prop_validation.reporter.AppendLine(sourceCell.SourceNode.OuterHtml);

                                            prop_validation.SetOutcome(ValidationOutcome.Invalid, "Source cell contains [" + descendant.Count + "] descendant nodes: " + signature);
                                            continue;
                                        } else
                                        {

                                        }
                                    }
                                } else
                                {
                                    prop_validation.LinkedNodes++;
                                }
                            }

                            
                        }
                    }
                }

            }

            OutputType = TaskOutputType.data;
            if (EntryCount.Range == 0)
            {
                OutputType |= TaskOutputType.fixedEntityCount;
                if (EntryCount.Maximum == 1)
                {
                    OutputType |= TaskOutputType.singleEntity;
                } 
            } else
            {
                OutputType |= TaskOutputType.variableEntityCount;
            }

            if (PropertyCount.Range == 0)
            {
                OutputType |= TaskOutputType.fixedPropertyCount;
            } else
            {
                OutputType |= TaskOutputType.variablePropertyCount;
            }

            if (OutputType.HasFlag(TaskOutputType.unstableEntityAndPropertyCounts))
            {
                SetOutcome(ValidationOutcome.Invalid, "Extraction result is unstable");   
            }

            var prop_unresolved = PropertyValidation.GetUnresolved().Values.ToList();

            foreach (var prop_validation in prop_unresolved) {

                prop_validation.Frequency = metaPropertyNameCounter.GetFrequencyForItem(prop_validation.item.PropertyName);
                prop_validation.DistinctValues = prop_validation.ValueCounter.DistinctCount();
                prop_validation.SpamPropertyMeasure = 1 - prop_validation.Frequency.GetRatio(metaPropertyNameCounter.GetTopFrequency());

                /*
                if (prop_validation.DistinctValues == 1)
                {
                    prop_validation.SetOutcome(ValidationOutcome.Invalid, "Property had only one distinct value");
                    continue;
                }*/

                prop_validation.SetOutcome(ValidationOutcome.Validated, "");
            }



            var prop_resulrs = PropertyValidation.GetResults();

            

            if (prop_resulrs[ValidationOutcome.Invalid].Any())
            {
                 SetOutcome(ValidationOutcome.Invalid, "[" + prop_resulrs[ValidationOutcome.Invalid].Count + "] invalid properties detected");
                
            }

            task.PropertyDictionary.CollectProperties(prop_resulrs[ValidationOutcome.Validated]);


            Outcome = ValidationOutcome.Validated;

            return Outcome;
        }

        public TaskOutputType OutputType { get; set; } = TaskOutputType.unknown;

        public override ValidationOutcome SetOutcome(ValidationOutcome outcome, string message)
        {
            GetScore();
            return base.SetOutcome(outcome, message);
        }

        public TableExtractionTask task { get; set; }

        public TaskValidationResult(TableExtractionTask _task)
        {
            task = _task;

           // task.score = new TableExtractionTaskScore();
           // task.score.ValidationResult = this;
        }

    }
}