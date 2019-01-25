using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataComplex.plot.elements
{
    public enum plotModelAxisScale
    {
        linear,
        logarithmic
    }

    public class plotModelCurve
    {

        public String curveTitle { get; set; }

    }


    public class plotModelAxis
    {

        public plotModelAxis()
        {

        }

        public String axisTitle { get; set; }



        public plotModelAxisScale scale { get; set; } = plotModelAxisScale.linear;

        public Double start { get; set; } = 0;

        public Double end { get; set; } = 0;
    }
}
