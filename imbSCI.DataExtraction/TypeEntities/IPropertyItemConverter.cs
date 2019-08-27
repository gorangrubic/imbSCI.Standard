using imbSCI.DataExtraction.MetaEntities;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.extensions.enumworks;
using System.Globalization;
using imbSCI.Core.extensions.text;
using imbSCI.Data;

namespace imbSCI.DataExtraction.TypeEntities
{
public interface IPropertyItemConverter
    {
        Object Convert(Object input);
    }
}