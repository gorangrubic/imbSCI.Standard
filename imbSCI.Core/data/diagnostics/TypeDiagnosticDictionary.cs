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
public class TypeDiagnosticDictionary
    {

        public Dictionary<Type, TypeDiagnosticsEntry> registry { get; set; } = new Dictionary<Type, TypeDiagnosticsEntry>();
        public Boolean Contains(Type type)
        {
            return registry.ContainsKey(type);
        }
        public TypeDiagnosticsEntry Get(Type type)
        {
            if (!registry.ContainsKey(type)) registry.Add(type, new TypeDiagnosticsEntry(type));
            return registry[type];
        }
    }
}