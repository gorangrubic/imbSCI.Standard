using HtmlAgilityPack;
using imbSCI.Core.files.folders;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Extractors.Data;
using imbSCI.DataExtraction.Extractors.Task;
using imbSCI.DataExtraction.MetaTables.Constructors;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.SourceData.Analysis;
using imbSCI.DataExtraction.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.Extractors.Core
{
    public abstract class HtmlExtractorBase : IHtmlExtractor
    {
        public folderNode folder { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether extractor should use the universal meta table constructor. <see cref="universalMetaTableConstructor"/>
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use universal constructors]; otherwise, <c>false</c>.
        /// </value>
        public Boolean UseUniversalConstructors { get; set; } = false;

        public HtmlNodeValueExtractionSettings valueExtraction { get; protected set; } = new HtmlNodeValueExtractionSettings();
        public CellContentAnalysis sourceContentAnalysis { get; protected set; } = new CellContentAnalysis();

        public void SetThrowException(Boolean DoThrow)
        {
            DoThrowException = DoThrow;
        }
        
        public Boolean DoThrowException { get; protected set; } = true;
        //public reportExpandedData ExtractorCustomizationSettings { get; set; } = new reportExpandedData();

        public virtual void DeployCustomizationSettings(reportExpandedData ExtractorCustomizationSettings)
        {
            ExtractorTools.SetSettingsFromData(this, ExtractorCustomizationSettings);
        }

        public ITextRender output { get; set; } = null;

        public void Log(String message)
        {
            if (output != null)
            {
                output.AppendLine(message);
            }
        }

        public void Deploy(ITextRender _output, HtmlNodeValueExtractionSettings _valueExtraction,CellContentAnalysis _sourceContentAnalysis, folderNode _folder)
        {
            output = _output;
            valueExtraction = _valueExtraction;
            sourceContentAnalysis = _sourceContentAnalysis;
            folder = _folder;
            universalMetaTableConstructor.Deploy(sourceContentAnalysis, valueExtraction);
        }

        protected UniversalMetaTableConstructor universalMetaTableConstructor { get; set; } = new UniversalMetaTableConstructor();

        /// <summary>
        /// Prepares the and construct.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="output">The output.</param>
        public void PrepareAndConstruct(TableExtractionTask task, List<TableExtractionChain> output)
        {
            //List<SourceTable> sourceTables = 

            //var sourceDict = output.ToDictionary(x => x.source);


            switch (task.multiNodePolicy)
            {
               
                case TaskMultiNodePolicy.AsSingleTableRows:
                case TaskMultiNodePolicy.AsSignleTableColumns:
                    var st = output.Select(x => x.source).ToList();
                    output.Clear();

                    SourceTable sti = st[0];
                    SourceTable final = sti;
                    for (int i = 1; i < st.Count; i++)
                    {
                        if (task.multiNodePolicy == TaskMultiNodePolicy.AsSingleTableRows)
                        {
                            final = final.MergeAsRows(st[i]);
                        }
                        else
                        {
                            final = final.MergeAsColumns(st[i]);
                        }
                    }

                    TableExtractionChain chain = new TableExtractionChain()
                    {
                        source = final,
                        name = task.resultTableNamePrefix
                    };

                    //sourceTables.Add(final);

                    
                //  if (firstMetaTable == null) firstMetaTable = ch.meta;

                    output.Add(chain);

                    break;
                 default:
                case TaskMultiNodePolicy.AsSeparatedTables:
//                    foreach (var ch in output)
//                    {
//                        sourceTables.Add(ch.source);
////                        ch.meta = Construct(ch.source, task);
//                       // if (firstMetaTable == null) firstMetaTable = ch.meta;
//                    }
                    break;
            }

            var mode = task.score.executionMode;

            for (int i2 = 0; i2 < output.Count; i2++)
            {
                var metaDescription = ConstructDescription(output[i2].source, task);
                if (mode != ExtractionTaskEngineMode.Training)
                {
                    var metaTable = Construct(output[i2].source, task);
                    if (metaTable != null)
                    {
                        metaTable.ExtraInfoEntries.Merge(task.ExtraInfoEntries);
                        metaTable.ExtraInfoEntries.Add(MetaTable.EXTRAINFOENTRYKEY_TASKNAME, task.name);
                        metaTable.ExtraInfoEntries.Add(MetaTable.EXTRAINFOENTRYKEY_EXTRACTORNAME, task.ExtractorName);

                    }
                    if (mode == ExtractionTaskEngineMode.Validation)
                    {
                        if (metaTable.Comment.isNullOrEmpty())
                        {
                            metaTable.Comment = "Constructed by " + GetType().Name;
                        }
                    }
                    AfterConstruction(metaTable, task);
                    output[i2].meta = metaTable;
                }
            }


        }

         public virtual  MetaTableDescription ConstructDescription(SourceTable sourceTable, TableExtractionTask task)
        {
            var tb = universalMetaTableConstructor.ConstructDescription(sourceTable, task, output);


            return tb;
        }

        public virtual MetaTable Construct(SourceTable sourceTable, TableExtractionTask task)
        {
            var tb = universalMetaTableConstructor.Construct(sourceTable, task, output);


            return tb;
        }

        public virtual void AfterConstruction(MetaTable metaTable, TableExtractionTask task)
        {
            universalMetaTableConstructor.AfterConstruction(metaTable, task, output);


            
        }

        public abstract SourceTable MakeSourceTable(HtmlNode tableNode);

        public abstract MetaTableDescription GetTableDescription();

        //public abstract MetaTableSchema GetTableSchema();


        public virtual void SetSourceTableCell(SourceTableCell cell, HtmlNode cellNode, HtmlDocument htmlDocument)
        {
            if (htmlDocument == null)
            {
                htmlDocument = cellNode.OwnerDocument;
            }

            if (cell == null) return;

            if (valueExtraction.StoreExtraData)
            {
                foreach (var att in cellNode.Attributes)
                {
                    cell.AttributeData.Add(att.Name, att.DeEntitizeValue);
                }
                cell.SourceCellTagName = cellNode.Name;
                cell.SourceCellXPath = cellNode.XPath;
                cell.SourceValueString = cellNode.InnerHtml;


            }

            if (valueExtraction.StoreSourceNode) cell.SourceNode = cellNode;

            cellNode.DescendantNodes();

            String v = cellNode.GetTextContent(); 

            //   v = HtmlDocument.HtmlEncode(cellNode.InnerText);
            //htmlDocument.Encoding
            String nv = valueExtraction.ProcessInput(v, htmlDocument.StreamEncoding, htmlDocument.Encoding);

            cell.Value = nv;
        }

        
    }
}