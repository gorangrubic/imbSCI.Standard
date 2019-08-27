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
//    d3.format(".0%")(0.123);  // rounded percentage, "12%"
//d3.format("($.2f")(-3.5); // localized fixed-point currency, "(£3.50)"
//d3.format("+20")(42);     // space-filled and signed, "                 +42"
//d3.format(".^20")(42);    // dot-filled and centered, ".........42........."
//d3.format(".2s")(42e6);   // SI-prefix with two significant digits, "42M"
//d3.format("#x")(48879);   // prefixed lowercase hexadecimal, "0xbeef"
//d3.format(",.2r")(4223);  // grouped thousands with two significant digits, "4,200"

    public class chartAxisTick
    {
        public chartAxisTick()
        {

        }
        public String format { get; set; } = "";

        public Boolean isFormatJS { get; set; } = false;

        public Boolean localtime { get; set; } = false;


        public String ToJS()
        {
            String output = " tick: {" + Environment.NewLine;
            if (isFormatJS)
            {
                output += " format: " + format + ", " + Environment.NewLine;
            } else
            {
                output += "format: '" + format + "', " + Environment.NewLine;
            }
            output += "}";

            return output;
        }
    }
}