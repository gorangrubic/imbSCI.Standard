using imbSCI.Core.data;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.table;
using imbSCI.Core.files;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.reporting;
using imbSCI.Data;
using imbSCI.DataComplex.converters;
using imbSCI.DataComplex.extensions.data.operations;
using imbSCI.DataComplex.tables;
using System;
using System.Collections.Generic;
using System.Data;

namespace imbSCI.DataComplex.data
{
public class classificationReportDataComplexContext
    {
        public List<DataTable> overview_tables { get; set; } = new List<DataTable>();
        public Dictionary<String, List<DataTable>> comparative_tables { get; set; } = new Dictionary<string, List<DataTable>>();
        public Dictionary<String, List<DataTable>> cumulative_tables { get; set; } = new Dictionary<string, List<DataTable>>();


        public Dictionary<String, List<DataTable>> comparative_narrow_tables { get; set; } = new Dictionary<string, List<DataTable>>();

        public Dictionary<String, List<classificationReportSpace>> report_spaces { get; set; } = new Dictionary<string, List<classificationReportSpace>>();

    }
}