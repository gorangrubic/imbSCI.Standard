
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
public class StructureGraphInformationSet
    {
        public List<StructureGraphInformation> items { get; set; } = new List<StructureGraphInformation>();

        public List<StructureGraphInformation> changes { get; set; } = new List<StructureGraphInformation>();

        //public void Publish(folderNode folder, String name, aceAuthorNotation notation=null)
        //{
        //    DataTable dt = items.ReportToDataTable<StructureGraphInformation>(true);
        //    dt.SetTitle(name + " records");
        //    dt.GetReportAndSave(folder, notation);

        //    dt = changes.ReportToDataTable<StructureGraphInformation>(true);
        //    dt.SetTitle(name + " changes");
        //    dt.GetReportAndSave(folder, notation);

        //    builderForText output = new builderForText();
        //    foreach (var item in items)
        //    {
        //        item.Report(null, output);
        //    }
        //    output.ReportSave(folder, name + "_records", "Structure graph entries");

        //    output = new builderForText();
        //    foreach (var item in changes)
        //    {
        //        item.Report(null, output);
        //    }
        //    output.ReportSave(folder, name + "_changes", "Structure graph changes log");
        //}

        /// <summary>
        /// Adds the specified item and returns difference compared to the last added
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="information">The information.</param>
        /// <returns></returns>
        public StructureGraphInformation Add(String name, StructureGraphInformation information)
        {
            information.name = name;

            StructureGraphInformation last = items.LastOrDefault();
            
            items.Add(information);

            StructureGraphInformation change = information;

            if (last != null)
            {
                change = information.GetDifference(last);
                if (change.TotalCount != 0)
                {
                    changes.Add(change);
                }
            } 

            return change;
        }
    }
}