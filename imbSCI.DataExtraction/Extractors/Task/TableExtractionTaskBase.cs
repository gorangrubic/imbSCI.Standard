using imbSCI.Core.extensions.data;
using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Extractors.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.NodeQuery;
using imbSCI.DataExtraction.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Extractors.Task
{

    ///// <summary>
    ///// Data Extraction task, resulting in <see cref="MetaTable"/> and <see cref="DataTable"/> output
    ///// </summary>
    ///// <seealso cref="imbSCI.Data.interfaces.IObjectWithName" />
    //[Serializable]
    //public class TableExtractionTask : TableExtractionTaskBase
    //{

    //    /// <summary>
    //    /// Constructor for XML serialization
    //    /// </summary>
    //    public TableExtractionTask()
    //    {
    //    }





    //    //  public List<>
    //}



    [Serializable]
    public class TableExtractionTask : IObjectWithName
    {

        public Boolean HasChildTasks
        {
            get
            {
                return ChildTasks.Any();
            }
        }

        public List<TableExtractionTask> ChildTasks { get; set; } = new List<TableExtractionTask>();

        public TableExtractionTask()
        {
        }
            public TableExtractionTask(IEnumerable<TableExtractionTask> childTasks)
            {
                TableExtractionTask first = childTasks.First();
                ChildTasks.AddRange(childTasks);

                ExtractorName = first.ExtractorName;
                Comment = "Merged extraction task [" + String.Concat(childTasks.Select(x => x.name)) + "]";

                ExtraInfoEntries.Add("pIndex", first.ExtraInfoEntries.GetStringValue("pIndex", "_"));

                var taskIndexList = childTasks.Select(x => ExtraInfoEntries.GetStringValue("taskIndex", "")).ToList();

                String pIndex = ExtraInfoEntries.GetStringValue("pIndex", "_");
            
                String tIndex = taskIndexList.toCsvInLine("-");

                ExtraInfoEntries.Add("taskIndex", tIndex);


                name = pIndex + first.ExtractorName + tIndex;
            }


        //  public String XPathSelector { get; set; } = "";

        public IHtmlExtractor GetExtractor()
        {
            IHtmlExtractor extractor = ExtractorTools.HtmlExtractorProvider.GetInstance(ExtractorName);
            extractor.DeployCustomizationSettings(ExtractorCustomizationSettings);
            return extractor;
        }

        public void Save(String filepath)
        {
            objectSerialization.saveObjectToXML(this, filepath);
        }

        public static TableExtractionTask Load(String filepath)
        {
            return objectSerialization.loadObjectFromXML<TableExtractionTask>(filepath);
        }


        public String Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public TaskPropertyDictionary PropertyDictionary { get; set; } = new TaskPropertyDictionary();

        [XmlIgnore]
        public TableExtractionTaskScore score { get; set; } = new TableExtractionTaskScore();

        /// <summary>
        /// Saves file with task execution information
        /// </summary>
        /// <param name="folder">The folder.</param>
        public void WriteScoreReporter(folderNode folder)
        {
            if (score == null) return;
            if (score.executionMode == ExtractionTaskEngineMode.Application) return;
            if (score.CurrentEntry().reporter != null)
            {
                var pr = folder.pathFor($"task{name}_{score.TaskRuns.Count}_{score.executionMode}_reporter.txt", imbSCI.Data.enums.getWritableFileMode.overwrite, "");
                String prContent = score.CurrentEntry().reporter.GetContent();
                if (!prContent.isNullOrEmpty())
                {
                    File.WriteAllText(pr, prContent);
                }
            }

        }

        private String _name = "";
        private String _resultTableNamePrefix = "";
        private String _comment = "";
        private MetaTableDescription _tableDescription;

        /// <summary>
        /// How the task should be performed when more than one HtmlNode was selected by <see cref="Query"/>
        /// </summary>
        /// <value>
        /// The multi node policy.
        /// </value>
        public TaskMultiNodePolicy multiNodePolicy { get; set; } = TaskMultiNodePolicy.AsSingleTableRows;

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>
        /// The query.
        /// </value>
        public NodeQueryDefinition Query { get; set; } = new NodeQueryDefinition();

        public reportExpandedData ExtractorCustomizationSettings { get; set; } = new reportExpandedData();


        public reportExpandedData ExtraInfoEntries { get; set; } = new reportExpandedData();

        public void Learn(ContentChunk chunk)
        {
            if (chunk.multiNodePolicy != TaskMultiNodePolicy.undefined)
            {
                multiNodePolicy = chunk.multiNodePolicy;
            }
            //if (!chunk.name.isNullOrEmpty()) ExtraInfoEntries.Add("ChunkName", chunk.name);
            if (!chunk.title.isNullOrEmpty()) ExtraInfoEntries.Add(nameof(ContentChunk.title), chunk.title);
            if (!chunk.description.isNullOrEmpty()) ExtraInfoEntries.Add(nameof(ContentChunk.description), chunk.description);
            //ExtraInfoEntries.AddObjectEntry("chunk", chunk, "Serialized chunk");
        }


        /// public MetaTableSchema tableSchema { get; set; }

        public MetaTableDescription tableDescription
        {
            get
            {
                return _tableDescription;
            }
            set
            {
                if (_tableDescription != null && value == null)
                {

                }
                _tableDescription = value;
            }
        }
        


        /// <summary>
        /// Name prefix - to be used for output table name
        /// </summary>
        /// <value>
        /// The result table name prefix.
        /// </value>
        public String resultTableNamePrefix
        {
            get
            {
                if (_resultTableNamePrefix.isNullOrEmpty())
                {
                    return name;
                }
                return _resultTableNamePrefix;
            }
            set
            {
                if (value == name) return;
                _resultTableNamePrefix = value;
            }
        }

        public String ExtractorName { get; set; } = nameof(TableTagExtractor);


        public String name
        {
            get
            {
                if (_name.isNullOrEmpty())
                {
                    return "UnnamedTask_" + ExtractorName;
                }
                return _name;
            }
            set
            {
                if (value == "UnnamedTask_" + ExtractorName)
                {
                    return;
                }
                _name = value;
            }
        }
    }
}
