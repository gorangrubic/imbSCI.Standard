using imbSCI.Core.attributes;
using imbSCI.Core.data;
using imbSCI.Core.enums;
using imbSCI.Core.extensions;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.table;
using imbSCI.Core.files.fileDataStructure;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Data;
using imbSCI.Data.extensions.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.Core.syntax.generator
{
    public static class ClassGenerationExtensions
    {




        public static String GenerateClassForTable(this DataTable input, ClassGenerationContext context)
        {
            List<settingsPropertyEntry> properties = new List<settingsPropertyEntry>();

            ClassGenerator generator = new ClassGenerator();

            DataTable schemaTable = input.GetClonedShema<DataTable>();

            foreach (DataColumn dc in schemaTable.Columns)
            {
                properties.Add(dc.GetSPE());

            }

            String code = generator.GenerateClassCode<BuilderForCode>(properties, context);

            return code;
            //settingsEntriesForObject seo = new settingsEntriesForObject()

        }

        public static List<String> GetUsingForTypes(IEnumerable<Type> properties, List<String> output = null)
        {

            if (output == null) output = new List<string>();

            foreach (Type pi in properties)
            {
                String ns = pi.Namespace;
                if (!output.Contains(ns))
                {
                    output.Add(ns);
                }

                var gts = pi.GetGenericArguments();
                if (gts != null)
                {
                    foreach (Type gt in gts)
                    {
                        ns = gt.Namespace;
                        if (!output.Contains(ns))
                        {
                            output.Add(ns);
                        }
                    }
                }

            }



            return output;
        }


        /// <summary>
        /// Gets the using for properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="output">The output.</param>
        /// <returns></returns>
        public static List<String> GetUsingForProperties(IEnumerable<PropertyInfo> properties, List<String> output = null)
        {

            if (output == null) output = new List<string>();

            foreach (PropertyInfo pi in properties)
            {
                String ns = pi.PropertyType.Namespace;
                if (!output.Contains(ns))
                {
                    output.Add(ns);
                }

                var gts = pi.PropertyType.GetGenericArguments();
                if (gts != null)
                {
                    foreach (Type gt in gts)
                    {
                        ns = gt.Namespace;
                        if (!output.Contains(ns))
                        {
                            output.Add(ns);
                        }
                    }
                }

            }



            return output;
        }


        /// <summary>
        /// Gets the using for flags.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="addBasic">if set to <c>true</c> [add basic].</param>
        /// <param name="output">The output.</param>
        /// <returns></returns>
        public static List<String> GetUsingForFlags(PropertyAppendFlags flags, Boolean addBasic = true, List<String> output = null)
        {
            if (output == null) output = new List<string>();

            if (addBasic)
            {
                output.Add("System");
                output.Add("System.Data");
                output.Add("System.Collection.Generic");
                output.Add("System.Text");
                output.Add("System");
            }

            var fl = flags.getEnumListFromFlags();
            foreach (PropertyAppendFlags f in fl)
            {
                switch (f)
                {
                    case PropertyAppendFlags.setComponentModelAttributes:
                        output.Add("System.ComponentModel");
                        break;
                    case PropertyAppendFlags.setXmlSerializationAttributes:
                        output.Add("System.Xml.Serialization");
                        break;
                    case PropertyAppendFlags.setSCIReportingDefinitions:
                        output.Add("imbSCI.Core.attributes");
                        output.Add("imbSCI.Core");
                        output.Add("imbSCI.Data");
                        output.Add("imbSCI.Core.enums");
                        output.Add("imbSCI.Core.reporting.zone");


                        break;
                    case PropertyAppendFlags.setXmlDocumentation:
                        break;
                    case PropertyAppendFlags.setDefaultValue:
                        break;
                }
            }


            return output;
        }
    }
}