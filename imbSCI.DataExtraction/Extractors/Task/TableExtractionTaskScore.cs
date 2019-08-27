using imbSCI.Core.extensions.table;
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
using imbSCI.DataExtraction.Validation.TaskValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Extractors.Task
{
    /// <summary>
    /// Non-production analytics of extraction task results
    /// </summary>
    [Serializable]
    public class TableExtractionTaskScore
    {

        public ExtractionTaskEngineMode executionMode { get; set; } = ExtractionTaskEngineMode.Undefined;


       // public TaskValidationResult ValidationResult { get; set; }

        /// <summary>
        ///Entry before the current
        /// </summary>
        /// <returns></returns>
        public TableExtractionTaskScoreEntry LastEntry()
        {
            if (TaskRuns.Count > 1)
            {
                return TaskRuns[TaskRuns.Count - 2];
            }
            return null;
        }

        public TableExtractionTaskScoreEntry CurrentEntry()
        {
            if (!TaskRuns.Any())
            {
                TaskRuns.Add(new TableExtractionTaskScoreEntry()
                {
                
                });
            }
            return TaskRuns.Last();
        }

        public TableExtractionTaskScoreEntry CreateNewEntry(HtmlSourceAndUrl source,  ExtractionTaskEngineMode _executionMode)
        {
            executionMode = _executionMode;
            
            TaskRuns.Add(new TableExtractionTaskScoreEntry()
            {
                executionMode = _executionMode,
                source = source
            });
            return TaskRuns.Last();
        }

        public List<TableExtractionTaskScoreEntry> TaskRuns = new List<TableExtractionTaskScoreEntry>();
        /*
        public List<MetaTableDescription> TableDescriptions = new List<MetaTableDescription>();
        public List<SourceTable> SourceTables = new List<SourceTable>();
        public List<SourceWithUrl> SourceList = new List<SourceTable>();
        */
        private Int32 _executionCount = 0;
        private Int32 _lastRunResult = 0;

        public Int32 ExecutionCount
        {
            get { return _executionCount; }
            set {
            
                _executionCount = value;
            }
        }


       

        public TableExtractionTaskScore()
        {

        }
    }
}