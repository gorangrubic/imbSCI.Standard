
using imbSCI.Core.math;
using imbSCI.Data.extensions.data;
using imbSCI.Data.extensions;
using imbSCI.Data;
using imbSCI.Graph.Converters;
using imbSCI.Core.extensions.data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using imbSCI.Core.files.folders;
using imbSCI.Data.collection.graph;
using imbSCI.Core.math.range.frequency;
using System.IO;
using imbSCI.Data.interfaces;
using imbSCI.Core.math.range.finder;
using imbSCI.Core.reporting.render;
using imbSCI.Core.data;
using System.Data;

using imbSCI.Core.reporting.render.builders;
using imbSCI.Core.extensions.table;

namespace imbSCI.Graph.Data
{
    [Serializable]
    public class StructureGraphInformation
    {

        public StructureGraphInformation GetDifference(StructureGraphInformation other)
        {
            StructureGraphInformation output = new StructureGraphInformation();
            output.name = "Difference between " + name + " and " + other.name;
            output.InputCount = InputCount - other.InputCount;
            output.TotalCount = TotalCount - other.TotalCount;
            output.LeafNodes = LeafNodes - other.LeafNodes;

            output.LevelRange = LevelRange.GetDifference(other.LevelRange);
            output.LeafLevelRange = LeafLevelRange.GetDifference(other.LeafLevelRange);
            output.ChildrenCountRange = ChildrenCountRange.GetDifference(other.ChildrenCountRange);

            return output;

        }

        public void Report(folderNode folder, ITextRender output)
        {
            if (output == null) output = new builderForText();

            this.ReportBase(output, true, name);

            if (folder != null)
            {
                output.ReportSave(folder, name, "Node graph structure information");
            }
        }

        /// <summary>
        /// Optional label for graph information
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String name { get; set; } = "";

        /// <summary>
        /// Number of input items, used for graph construction
        /// </summary>
        /// <value>
        /// The input count.
        /// </value>
        public Int32 InputCount { get; set; } = 0;

        /// <summary>
        /// Total number of nodes constructed
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
        public Int32 TotalCount { get; set; } = 0;

        public Int32 LeafNodes { get; set; } = 0;

        public rangeFinder LevelRange { get; set; } = new rangeFinder("Level");
        public rangeFinder LeafLevelRange { get; set; } = new rangeFinder("LeafLevel");
        public rangeFinder ChildrenCountRange { get; set; } = new rangeFinder("ChildrenCount");

        public void Populate(IObjectWithPathAndChildren source)
        {
            if (name.isNullOrEmpty()) name = source.name;

            var allChilren =  source.getAllChildren();

            foreach (var item in allChilren)
            {
                Int32 ChildrenCount = 0;
                foreach (var citem in item)
                {
                    ChildrenCount++;
                }

                ChildrenCountRange.Learn(ChildrenCount);

                if (ChildrenCount == 0)
                {
                    LeafNodes++;
                    LeafLevelRange.Learn(item.level);
                }
                LevelRange.Learn(item.level);
            }
            
            TotalCount = allChilren.Count;
        }

        public StructureGraphInformation()
        {

        }

    }
}