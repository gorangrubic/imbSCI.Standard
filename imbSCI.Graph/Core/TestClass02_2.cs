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
public class TestClass02:TestMetrics
    {
        public TestClass02():base(250,600,5)
        {

        }

        public Double Test2Property01 { get; set; }
        public Double Test2Property02 { get; set; }
        public Double Test2Property03 { get; set; }
        public Int32 Test2Property04 { get; set; }
        public Int32 Test2Property05 { get; set; }


        public Decimal Test2Property14 { get; set; }
        public Decimal Test2Property15 { get; set; }

        public String PropertyOne { get; set; } = "";
    }
}