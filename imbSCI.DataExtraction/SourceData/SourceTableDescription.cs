using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.math.range.finder;
using imbSCI.Core.reporting.zone;
using imbSCI.Data.primitives;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData.Analysis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.SourceData
{
    [Serializable]
    public class SourceTableDescription
    {
        public void Transpose()
        {
            matrix.Transpose();
            coordinateXY transposedZone = new coordinateXY()
            {
                x = valueZone.y,
                y = valueZone.x
            };
            valueZone = transposedZone;
            
        }

        public SourceTableDescription()
        {
        }

       
        [XmlIgnore]
        public SourceTableSliceTest firstRowTest { get; set; } //= MakeSliceTest(sourceTable, SourceTableSliceType.row);

        [XmlIgnore]
        public SourceTableSliceTest firstColumnTest { get; set; } // = MakeSliceTest(sourceTable, SourceTableSliceType.column);


        [XmlIgnore]
        public ContentStatMatrix matrix { get; set; }

        public coordinateXY valueZone { get; set; } = new coordinateXY();

        public coordinateXY sourceSize { get; set; } = new coordinateXY();
    }
}