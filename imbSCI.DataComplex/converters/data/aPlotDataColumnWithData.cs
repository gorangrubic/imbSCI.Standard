using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataComplex.converters.data
{
public class aPlotDataColumnWithData
    {

        public aPlotDataColumn aPlotColumn { get; set; }

        public DataColumn column { get; set; }

        public List<Double> values { get; set; }

        public aPlotDataColumnWithData()
        {

        }

    }
}