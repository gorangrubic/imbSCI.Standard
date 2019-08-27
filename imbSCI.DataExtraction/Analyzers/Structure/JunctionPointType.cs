using imbSCI.Core.math.range.frequency;
using System;
using System.Collections.Generic;
using System.Text;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.Analyzers.Data;
using HtmlAgilityPack;
using imbSCI.Core.extensions.text;
using System.Linq;
using imbSCI.Core.extensions.data;
using System.Xml.Serialization;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting.render.builders;
using System.IO;

namespace imbSCI.DataExtraction.Analyzers.Structure
{
public enum JunctionPointType
    {
        Undefined,
        BranchToLeafs,
        DeepJunctionPoint
    }
}