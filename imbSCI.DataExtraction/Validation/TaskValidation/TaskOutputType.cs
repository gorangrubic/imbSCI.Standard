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
[Flags]
    public enum TaskOutputType
    {
        broken=0,
        data = 1,
        fixedPropertyCount=2,
        fixedEntityCount=4,
        singleEntity=8,
        variableEntityCount=16,
        variablePropertyCount=32,
        unknown = 64,
        unstableEntityAndPropertyCounts = variablePropertyCount | variableEntityCount,
        
    }
}