using imbSCI.DataExtraction.MetaEntities;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using imbSCI.Core.math.classificationMetrics;

namespace imbSCI.DataExtraction.TypeEntities
{
public enum TypePropertyItemConverterType
    {
        Undefined,
        DateTimeConverter,
        EnumerationConverter,
        NumericFactor
    }
}