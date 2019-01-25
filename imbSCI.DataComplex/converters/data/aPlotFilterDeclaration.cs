using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataComplex.converters.data
{
public class aPlotFilterDeclaration : aPlotDataElementBase
    {
        public aPlotDataFilters filter_name { get; set; } = aPlotDataFilters.String2DoubleFilter;


        public aPlotFilterDeclaration() { }
        public aPlotFilterDeclaration(aPlotDataFilters _filter_name)
        {

            filter_name = _filter_name;
        }

        public static aPlotFilterDeclaration GetDoubleInputFilter()
        {
            var flt = new aPlotFilterDeclaration(aPlotDataFilters.String2DoubleFilter);
            flt.name = "InputFilter";
            return flt;
        }

        public static aPlotFilterDeclaration GetDoubleOutputFilter()
        {
            var flt = new aPlotFilterDeclaration(aPlotDataFilters.Double2StringFilter);
            flt.name = "OutputFilter";
            return flt;
        }
    }
}