using imbSCI.Core.data;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting.render;
using imbSCI.Core.extensions.enumworks;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.SourceData.Analysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.MetaTables.Descriptors
{

    public enum MetaTableInterpretation
    {
        unknown,

        /// <summary>
        /// The single entity: table rows contain property identifier and property value. Primary column identifies property, the other sets the value
        /// </summary>
        singleEntity,
        /// <summary>
        /// The multi entities: each table row contains data on separate entity, columns represent properties of the entities
        /// </summary>
        multiEntities,

        /// <summary>
        /// The triplets: property names are hidden, table describes data triplets: [0,y] -> [x,y] -> [x,0]
        /// </summary>
        triplets

    }


    [Serializable]
    public class MetaTableDescription
    {
        public String Comment { get; set; } = "";

        private Int32 _index_propertyID = 0;
        private Int32 _index_entryID = 0;

        public String GetSignature()
        {
            return $"{sourceDescription.valueZone.x}:{sourceDescription.valueZone.y}:{index_entryID}:{index_propertyID}:{format}:{Interpretation}";
        }

        public void SetFromSignature(String signature)
        {
            var elements = signature.SplitSmart(":", "", true, true);

            sourceDescription.valueZone.x = Convert.ToInt32(elements[0]);
            sourceDescription.valueZone.y = Convert.ToInt32(elements[1]);

            index_entryID = Convert.ToInt32(elements[2]);
            index_propertyID = Convert.ToInt32(elements[3]);

            format = (MetaTableFormatType) imbTypeEnumExtensions.imbToEnumeration(elements[4], typeof(MetaTableFormatType));

            switch (format)
            {
                case MetaTableFormatType.horizontal:
                    propertySource = SourceTableSliceType.column;
                    entrySource = SourceTableSliceType.row;
                    break;
                case MetaTableFormatType.vertical:
                    propertySource = SourceTableSliceType.row;
                    entrySource = SourceTableSliceType.column;
                    break;
            }

            Interpretation = (MetaTableInterpretation)imbTypeEnumExtensions.imbToEnumeration(elements[5], typeof(MetaTableInterpretation));
        }

        public MetaTableDescription()
        {
        }

        public MetaTableDescription(SourceTableDescription sourceDesc, MetaTableFormatType orientation)
        {
            sourceDescription = sourceDesc;
            format = orientation;
        }

        public Int32 index_entryID
        {
            get { return _index_entryID; }
            set { _index_entryID = value; }
        }
        public Int32 index_propertyID
        {
            get {
                
                return _index_propertyID;
            }
            set {
                _index_propertyID = value;
            }
        }

        public void Report(folderNode folder, ITextRender output)
        {
            if (output == null) return ;
            this.ReportBase(output, false, "MetaTableDescription");

            
            sourceDescription.ReportBase(output, false, "Source description"); //.Report(folder, output);

        }

        public Boolean IsDeclared
        {
            get
            {
                if (propertySource == SourceTableSliceType.undefined) return false;
                if (entrySource == SourceTableSliceType.undefined) return false;
                if (Interpretation == MetaTableInterpretation.unknown) return false;

                return true;
            }
            
        }

        public SourceTableSliceType propertySource { get; set; } = SourceTableSliceType.undefined;
        public SourceTableSliceType entrySource { get; set; } = SourceTableSliceType.undefined;



        public MetaTableInterpretation Interpretation { get; set; } = MetaTableInterpretation.unknown;

        public String MetaEntitySetterExpression = "";
        public String MetaEntityClassName = "";




        public Int32 EntrySkipCount
        {
            get
            {
                if (sourceDescription == null) return 0;
                if (sourceDescription.valueZone == null) return 0;
                if (format == MetaTableFormatType.unknown) return 0;
                switch (format)
                {
                    case MetaTableFormatType.horizontal:
                        return sourceDescription.valueZone.y; 
                        break;
                    case MetaTableFormatType.vertical:
                        return sourceDescription.valueZone.x; 
                        break;
                    default:
                        break;
                }

                return 0;
            }
        }



        public SourceTableDescription sourceDescription { get; set; } = new SourceTableDescription();



        public Boolean IsTransposed { get; set; } = false;



        public MetaTableFormatType format { get; set; } = MetaTableFormatType.vertical;

        public void ToggleFormat()
        {
            if (format == MetaTableFormatType.vertical)
            {
                format = MetaTableFormatType.horizontal;
                IsTransposed = true;
            } else
            {
                format = MetaTableFormatType.vertical;
                IsTransposed = true;
            }
            sourceDescription.Transpose();
        }

        public String entryIDPropertyName { get; set; } = "ID"; 
    }
}