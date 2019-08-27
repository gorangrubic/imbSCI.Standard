//using imbSCI.Core.reporting.render.builders;
//using imbSCI.DataExtraction.MetaTables.Constructors;
//using imbSCI.DataExtraction.MetaTables.Descriptors;
//using imbSCI.DataExtraction.SourceData;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace imbSCI.DataExtraction.MetaTables.Constructors
//{
//    [Serializable]
//    public class TableDescriptionBuilder : MetaTableConstructorBase
//    {




//        public MetaTableDescription CompileDescription()
//        {
//            SourceTableDescriptionAggregation aggregatedDescriptions = task.score.LastEntry().aggregatedDescriptions;


//            List<SourceTableDescription> sourceDescriptions = task.score.TaskRuns.Where(x => x.executionMode == ExtractionTaskEngineMode.Training).Where(x => x.IsSuccess).Select(x => x.metaTableDescription.sourceDescription).ToList();
//            aggregatedDescriptions = new SourceTableDescriptionAggregation(sourceDescriptions);

//            aggregatedDescriptions.Report(folder, reporter);
//            //task.score.CurrentEntry().aggregatedDescriptions = aggregatedDescriptions;
//            //var run = task.score.CurrentEntry();
//            //var run_i = task.score.TaskRuns.IndexOf(run);
//            metaDescription = new imbSCI.DataExtraction.MetaTables.Descriptors.MetaTableDescription(new SourceTableDescription(), imbSCI.DataExtraction.MetaTables.Descriptors.MetaTableFormatType.unknown);
//            metaDescription.Comment = "Created by validation task run [" + run_i + "]";

//            SourceTableSliceTestAggregation SelectedAsPropertyUID = null;
//            SourceTableSliceTestAggregation SelectedAsEntryUID = null;

//            if (aggregatedDescriptions.rowTestAggregation.IsPreferedAsPropertyUID)
//            {
//                SelectedAsPropertyUID = aggregatedDescriptions.rowTestAggregation;
//            }
//            else if (aggregatedDescriptions.columnTestAggregation.IsPreferedAsPropertyUID)
//            {
//                SelectedAsPropertyUID = aggregatedDescriptions.columnTestAggregation;
//            }
//            else if (aggregatedDescriptions.rowTestAggregation.IsSuitableAsUID)
//            {
//                SelectedAsPropertyUID = aggregatedDescriptions.rowTestAggregation;
//            }
//            else if (aggregatedDescriptions.columnTestAggregation.IsSuitableAsUID)
//            {
//                SelectedAsPropertyUID = aggregatedDescriptions.columnTestAggregation;
//            }
//            else if (aggregatedDescriptions.rowTestAggregation.IsAcceptableAsPropertyUID)
//            {
//                SelectedAsPropertyUID = aggregatedDescriptions.rowTestAggregation;
//            }
//            else if (aggregatedDescriptions.columnTestAggregation.IsAcceptableAsPropertyUID)
//            {
//                SelectedAsPropertyUID = aggregatedDescriptions.columnTestAggregation;
//            }

//            if (SelectedAsPropertyUID == null)
//            {
//                metaDescription.index_propertyID = -1;
//            }
//            else
//            {
//                if (aggregatedDescriptions.rowTestAggregation == SelectedAsPropertyUID)
//                {
//                    SelectedAsEntryUID = aggregatedDescriptions.columnTestAggregation;
//                }
//                else if (aggregatedDescriptions.columnTestAggregation == SelectedAsPropertyUID)
//                {
//                    SelectedAsEntryUID = aggregatedDescriptions.rowTestAggregation;
//                }
//            }

//            if (SelectedAsEntryUID != null)
//            {

//                if (SelectedAsEntryUID.IsDistinctValue)
//                {

//                }
//                else
//                {
//                    SelectedAsEntryUID = null;
//                    metaDescription.index_entryID = -1;
//                }
//            }


//            if (SelectedAsEntryUID != null)
//            {
//                metaDescription.entrySource = SelectedAsEntryUID.format;
//                metaDescription.index_entryID = 0;
//            }

//            if (SelectedAsPropertyUID != null)
//            {
//                metaDescription.propertySource = SelectedAsPropertyUID.format;
//                metaDescription.index_propertyID = 0;
//            }


//            //if (metaDescription.propertySource == SourceTableSliceType.undefined)
//            //{
//            //    if (aggregatedDescriptions.sourceWidth.Range <= aggregatedDescriptions.sourceHeight.Range)
//            //    {
//            //        metaDescription.propertySource = SourceTableSliceType.column;
//            //    } else if (aggregatedDescriptions.sourceWidth.Range > aggregatedDescriptions.sourceHeight.Range)
//            //    {
//            //        metaDescription.propertySource = SourceTableSliceType.row; 
//            //    }
//            //}

//            if (metaDescription.entrySource == SourceTableSliceType.undefined)
//            {
//                switch (metaDescription.propertySource)
//                {
//                    default:

//                        break;
//                    case SourceTableSliceType.column:
//                        metaDescription.entrySource = SourceTableSliceType.row;
//                        break;
//                    case SourceTableSliceType.row:
//                        metaDescription.entrySource = SourceTableSliceType.column;
//                        break;
//                }
//            }

//            switch (metaDescription.propertySource)
//            {
//                default:

//                    break;
//                case SourceTableSliceType.column:
//                    metaDescription.sourceDescription.valueZone.y = 0;// metaDescription.index_propertyID + 1;
//                    metaDescription.sourceDescription.valueZone.x = 0;//metaDescription.index_entryID + 1;
//                                                                      // metaDescription.IsTransposed = true;
//                                                                      //   metaDescription.entrySource = SourceTableSliceType.row;
//                    metaDescription.format = MetaTableFormatType.vertical;
//                    break;
//                case SourceTableSliceType.row:
//                    //  metaDescription.entrySource = SourceTableSliceType.column;
//                    metaDescription.format = MetaTableFormatType.horizontal;
//                    metaDescription.sourceDescription.valueZone.x = 0; // metaDescription.index_propertyID + 1;
//                    metaDescription.sourceDescription.valueZone.y = 0; // metaDescription.index_entryID + 1;
//                    break;
//            }


//            task.tableDescription = metaDescription;

//            metaDescription.Report(folder, reporter);

//            task.score.CurrentEntry().aggregatedDescriptions = aggregatedDescriptions;
//            task.score.CurrentEntry().metaTableDescription = task.tableDescription;
//        //}

//        //task.score.CurrentEntry().aggregatedDescriptions = aggregatedDescriptions;

//        }
        

//    }
//}
