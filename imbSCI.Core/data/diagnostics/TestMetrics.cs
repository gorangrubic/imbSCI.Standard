using imbSCI.Core.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.files.folders;
using imbSCI.Core.math;
using imbSCI.Core.math.measurement;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.Core.data.diagnostics
{
  public abstract class TestMetrics:MetricsBase
    {
        protected TestMetrics(Int32 min=100, Int32 max=200, Int32 doubleDivision=10)
        {
            RandomizeData(min, max, doubleDivision);
        }

        public void RandomizeData(Int32 min=100, Int32 max=200, Int32 doubleDivision=10)
        {
            Random rnd = new Random();
            foreach (var pi_int in Integers) pi_int.SetValue(this, rnd.Next(min, max), null);
            

            foreach (var pi_dbl in Doubles) pi_dbl.SetValue(this, rnd.Next(min, max).GetRatio(doubleDivision), null);

            foreach (var pi_dcm in Decimals) pi_dcm.SetValue(this, Convert.ToDecimal(rnd.Next(min, max).GetRatio(doubleDivision)), null);
        }
    }
}