using imbSCI.Core.math.range.frequency;
using System;
using System.Collections.Generic;
using System.Text;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.Extractors;
using imbSCI.DataExtraction.Analyzers;
using HtmlAgilityPack;
using System.Linq;
using imbSCI.Core.extensions.data;
using System.Xml.Serialization;
using imbSCI.Core.collection;
using imbSCI.DataExtraction.Extractors.Detectors;
using imbSCI.Core.math.range.finder;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.Extractors.Core;

namespace imbSCI.DataExtraction.Analyzers.Templates
{
public enum RecordTemplateRole
    {
        RowDataProvider,
        RowDataAndContextProvider,
        GeneralContextProvider
    }
}