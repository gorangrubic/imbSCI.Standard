using imbSCI.Core.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.files.folders;
using imbSCI.Core.math;
using imbSCI.Core.math.measurement;
using imbSCI.Core.reporting.render;
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
    public class TypeDiagnostics
    {
        public TypeDiagnostics(folderNode outputfolder, Type type)
        {

            CollectTypes(type);

            builderForText debug = new builderForText();
            TestTypes(debug);

            debug.ReportSave(outputfolder, "typediagnostic_" + type.Name + ".txt", "Results of type diagnostics");

            debug = new builderForText();
            TestTypesForBinarySerialization(debug);
            debug.ReportSave(outputfolder, "typediagnostic_" + type.Name + "_binary.txt", "Results of type diagnostics");

        }


        public TypeDiagnosticDictionary RelatedTypes { get; set; } = new TypeDiagnosticDictionary();

        public void TestTypesForBinarySerialization(ITextRender output)
        {
            foreach (var pair in RelatedTypes.registry)
            {
                pair.Value.TestOnBinary();

            }


            foreach (var pair in RelatedTypes.registry)
            {
                output.AppendLine("Type: " + pair.Key.GetCleanTypeName());
                output.nextTabLevel();
                var entryRoot = RelatedTypes.Get(pair.Key);

                foreach (var pi in pair.Value.ClassTypeProperties)
                {

                    var entry = RelatedTypes.Get(pi.PropertyType);
                    if (!entry.XmlSerializationOk)
                    {
                        output.AppendLine("Property: " + pi.PropertyType.GetCleanTypeName() + " " + pi.Name);
                        output.AppendLine(entry.Message.ToString());
                    }

                }


                if (!entryRoot.XmlSerializationOk)
                {
                    output.AppendLine("Problems: ");
                    output.AppendLine(entryRoot.Message.ToString());
                }

                output.prevTabLevel();
            }
        }

        public void TestTypes(ITextRender output)
        {
            foreach (var pair in RelatedTypes.registry)
            {
                pair.Value.Test();

            }


            foreach (var pair in RelatedTypes.registry)
            {
                output.AppendLine("Type: " + pair.Key.GetCleanTypeName());
                output.nextTabLevel();
                var entryRoot = RelatedTypes.Get(pair.Key);
                
                foreach (var pi in pair.Value.ClassTypeProperties)
                {
                    
                    var entry = RelatedTypes.Get(pi.PropertyType);
                    if (!entry.XmlSerializationOk)
                    {
                        output.AppendLine("Property: " + pi.PropertyType.GetCleanTypeName() + " " + pi.Name);
                        output.AppendLine(entry.Message.ToString());
                    }

                }
                

                if (!entryRoot.XmlSerializationOk)
                {
                    output.AppendLine("Problems: ");
                    output.AppendLine(entryRoot.Message.ToString());
                }

                output.prevTabLevel();
            }




        }

        public void CollectTypes(Type type)
        {
            List<Type> iteration = new List<Type>() { type };

            while (iteration.Any())
            {
                List<Type> next_iteration = new List<Type>();

                foreach (var t in iteration)
                {
                    if (!RelatedTypes.Contains(t))
                    {
                        TypeDiagnosticsEntry entry = RelatedTypes.Get(t);

                        foreach (var pi in entry.ClassTypeProperties)
                        {
                            if (!RelatedTypes.Contains(pi.PropertyType))
                            {
                                if (!next_iteration.Contains(pi.PropertyType)) next_iteration.Add(pi.PropertyType);
                            }
                        }
                    }
                }

                iteration = next_iteration;
            }
        }

        public void DebugXmlSerialization(Type type)
        {
            CollectTypes(type);
        }

    }
}