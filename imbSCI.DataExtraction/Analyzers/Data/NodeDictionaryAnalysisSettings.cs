using HtmlAgilityPack;
using imbSCI.Core.extensions.data;
using imbSCI.Core.math;
using imbSCI.Core.math.range.frequency;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.extensions;
using imbSCI.Data.extensions.data;
using imbSCI.DataExtraction.Extractors.Detectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Analyzers.Data
{
    public class NodeDictionaryAnalysisSettings:LeafNodeDictionary
    {

        public ChunkDetectorCollection Detectors { get; set; } = new ChunkDetectorCollection();

        public DataPointMapper DataPointMapSettings { get; set; } = new DataPointMapper();

        public List<String> TagsToIgnore { get; set; } = new List<string>();

        public List<String> XPathTagsToRemove { get; set; } = new List<string>();

        public String leafSelectionXPath { get; set; } = ".//*[not(*)]";

         public Int32 JunctionSizeMin { get; set; } = 4;

        public Int32 stepsToRetreatFromLeaf { get; set; } = 2;

        public Int32 MinLevel { get; set; } = 3;

        public NodeDictionaryAnalysisSettings()
        {

        }
    }
}