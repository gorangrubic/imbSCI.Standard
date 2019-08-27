using imbSCI.Reporting.charts.core;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

using System.Xml.Serialization;
using imbSCI.Data;
using System.Data;
using imbSCI.Core.extensions.table;
using System.Linq;

namespace imbSCI.Reporting.charts.model
{
public class chartLabelFormat
    {
        public string dataColumnName { get; set; } = "";
        public String format { get; set; } = "";

    }
}