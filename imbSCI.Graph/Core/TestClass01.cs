using imbSCI.Core.data.diagnostics;
using imbSCI.Core.files.folders;
using imbSCI.Data.collection.graph;
using imbSCI.Data.interfaces;
using imbSCI.Graph.Converters;
using imbSCI.Graph.DGML;
using imbSCI.Core.extensions.typeworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

namespace imbSCI.Graph.Core
{
public class TestClass01:TestMetrics
    {
        public TestClass01()
        {

        }

        public Double TestProperty01 { get; set; }
        public Double TestProperty02 { get; set; }
        public Double TestProperty03 { get; set; }
        public Int32 TestProperty04 { get; set; }
        public Int32 TestProperty05 { get; set; }


        public Decimal TestProperty14 { get; set; }
        public Decimal TestProperty15 { get; set; }

        public String PropertyOne { get; set; } = "";
    }
}