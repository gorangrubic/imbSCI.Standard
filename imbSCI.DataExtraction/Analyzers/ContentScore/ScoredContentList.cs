using HtmlAgilityPack;
using imbSCI.Core.collection;
using imbSCI.Core.data;
using imbSCI.Core.data.providers;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.files;
using imbSCI.Core.math;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Extractors.Core;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.DataExtraction.Extractors
{
    public class ScoredContentList<TItem>:List<ScoredContent<TItem>> where TItem : class
    {

    }
}