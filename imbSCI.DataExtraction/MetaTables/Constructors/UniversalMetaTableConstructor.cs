using imbSCI.Core.reporting.render;
using imbSCI.Core.extensions.data;
using imbSCI.Data;
using imbSCI.DataExtraction.Extractors.Core;
using imbSCI.DataExtraction.Extractors.Data;
using imbSCI.DataExtraction.Extractors.Task;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.SourceData.Analysis;
using System;
using System.Collections.Generic;
using System.Text;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using System.Linq;
using imbSCI.Core.reporting.render.builders;
using System.IO;

namespace imbSCI.DataExtraction.MetaTables.Constructors
{
    /// <summary>
    /// Standard MetaTable constructor, used by <see cref="IHtmlExtractor"/>s by default
    /// </summary>
    /// <seealso cref="imbSCI.DataExtraction.MetaTables.Constructors.MetaTableConstructorBase" />
    [Serializable]
    public class UniversalMetaTableConstructor:MetaTableConstructorBase
    {

        public UniversalMetaTableConstructor()
        {

        }

        /// <summary>
        /// Performs a test on 
        /// </summary>
        /// <param name="sourceTable">The source table.</param>
        /// <param name="type">The type.</param>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public SourceTableSliceTest MakeSliceTest(SourceTable sourceTable, SourceTableSliceType type, Int32 i=0)
        {
            SourceTableSliceTest output = new SourceTableSliceTest()
            {
                format = type
            };

            switch (type)
            {
                case SourceTableSliceType.row:
                    output.Values = sourceTable.GetRow(i);
                    break;
                default:
                case SourceTableSliceType.column:
                    output.Values = sourceTable.GetColumn(i);
                    break;
            }

            CellContentType contentType = CellContentType.unknown;
            List<String> DistinctValues = new List<string>();
            foreach (var v in output.Values)
            {
                CellContentInfo t = sourceContentAnalysis.DetermineContentType(v);
                if (contentType == CellContentType.unknown)
                {
                    contentType = t.type;
                } else if (contentType != t.type)
                {
                    if (!t.type.HasFlag(contentType))
                    {
                        output.IsUniformFormat = false;
                    }
                }

                if (v.IsNullOrEmpty())
                {
                    output.IsNoEmptyValue = false;
                }
                if (!DistinctValues.Contains(v)) DistinctValues.Add(v);
            }
            if (DistinctValues.Count < output.Values.Count)
            {
                output.IsDistinctValue = false;
            }
            return output;
        }


        public MetaTableDescription ConstructDescription(SourceTable sourceTable, TableExtractionTask task, ITextRender logger)
        {
            //if (sourceTable[0, 0].Value.isNullOrEmpty())
            //{
            //    sourceTable[0, 0].Value = "ID";
            //}

            MetaTableDescription metaDescription = null;


            builderForText reporter = task.score.CurrentEntry().reporter;


            switch (task.score.executionMode)
            {
                case ExtractionTaskEngineMode.Training:

                    SourceTableDescription sourceDesc = sourceContentAnalysis.GetDescription(sourceTable);


                    metaDescription = new imbSCI.DataExtraction.MetaTables.Descriptors.MetaTableDescription(sourceDesc, imbSCI.DataExtraction.MetaTables.Descriptors.MetaTableFormatType.vertical);
                   // task.tableDescription = metaDescription;
                    

                    //task.score.CurrentEntry().metaTableDescription = metaDescription;

                    break;
                case ExtractionTaskEngineMode.Validation:
                default:
                case ExtractionTaskEngineMode.Application:

                    
                    if (task.tableDescription == null)
                    {
                        throw new Exception("Task [" + task.name + "] has no table description set.");
                    }
                   

                    break;
            }

            if (folder != null)
            {
                String sp = folder.pathFor("UMTC_Construct_" + task.name + "_" + task.score.executionMode.toString() + ".txt");
                File.WriteAllText(sp, reporter.GetContent());
            }

            return metaDescription;
        }


        public void AfterConstruction(MetaTable metaTable, TableExtractionTask task, ITextRender logger)
        {
            switch (task.score.executionMode)
            {
                default:
                    break;
                case ExtractionTaskEngineMode.Validation:


                    task.score.CurrentEntry().metaTable.Add(metaTable);
                   
                    break;
          
                case ExtractionTaskEngineMode.Application:

                    task.score.CurrentEntry().metaTable.Add(metaTable);

                    //if (task.tableDescription == null)
                    //{
                    //    var sourceDesc = sourceContentAnalysis.GetDescription(sourceTable);
                    //    task.tableDescription = new imbSCI.DataExtraction.MetaTables.Descriptors.MetaTableDescription(sourceDesc, imbSCI.DataExtraction.MetaTables.Descriptors.MetaTableFormatType.vertical);
                    //}


                    break;
            }
        }

        /// <summary>
        /// Constructs MetaTable from the source data
        /// </summary>
        /// <param name="sourceTable">The source table.</param>
        /// <param name="task">The task.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public MetaTable Construct(SourceTable sourceTable, TableExtractionTask task, ITextRender logger)
        {
            MetaTable metaTable = null;

            switch (task.score.executionMode)
            {
                case ExtractionTaskEngineMode.Application:
                    metaTable = new MetaTable(task.tableDescription);
                    metaTable.SetSchema(sourceTable);
                    metaTable.ApplySchema(task.PropertyDictionary.items.Select(x=>x.Meta));
                    metaTable.SetEntries(sourceTable);

                     metaTable.Comment = "Constructed by " + GetType().Name;
                    break;
                case ExtractionTaskEngineMode.Validation:
                    metaTable = new MetaTable(task.tableDescription);
                    metaTable.SetSchema(sourceTable);
                    metaTable.SetEntriesAndLinkToSource(sourceTable);  //.SetEntries(sourceTable);

                    metaTable.RefineSchema(sourceContentAnalysis);

                    metaTable.Comment = "Constructed by " + GetType().Name;
                    break;
                default:
                    break;
            }

            
            return metaTable;

        }

        


    }
}