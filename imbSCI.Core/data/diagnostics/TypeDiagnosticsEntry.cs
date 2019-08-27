using imbSCI.Core.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.files;
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
    public class TypeDiagnosticsEntry
    {

        public void Set(Type _type)
        {
            type = _type;
        }

        public List<PropertyInfo> RelevantProperties { get; set; } = new List<PropertyInfo>();

        public static Boolean HasXmlIgnore(PropertyInfo pi)
        {
            var attributes = pi.GetCustomAttributes(true);

            if (!attributes.Any()) return false;

            var xmlIgnore  = attributes.FirstOrDefault(x => x.GetType() == typeof(XmlIgnoreAttribute));

            return xmlIgnore != null;
        }

        public void TestOnBinary()
        {
            var constructors = type.GetConstructors();

            var parameterLess = constructors.FirstOrDefault(x => !x.GetParameters().Any());

            if (parameterLess == null)
            {
                ReportProblem("Public parameterless constructor not found");
            }

            if (XmlSerializationOk)
            {
                var instance = type.getInstance(null);

                try
                {
                    byte[] bytes = objectSerialization.SerializeBinary(instance);
                }
                catch (Exception ex)
                {
                    while (ex != null)
                    {
                        Message.AppendLine("Binary serialization fails");
                        ReportProblem(ex);
                        ex = ex.InnerException;
                    }
                }
            }


            IsTested = true;
        }

        public void Test()
        {
            var constructors = type.GetConstructors();

            var parameterLess = constructors.FirstOrDefault(x => !x.GetParameters().Any());

            if (parameterLess==null)
            {
                ReportProblem("Public parameterless constructor not found");  
            }

            if (XmlSerializationOk)
            {
                var instance = type.getInstance(null);

                try
                {
                    String xml = objectSerialization.ObjectToXML(instance);
                }
                catch (Exception ex)
                {
                    Message.AppendLine("XML serialization fails");
                    while (ex != null)
                    {
                        ReportProblem(ex);
                        ex = ex.InnerException;
                    }
                }


                try
                {
                    byte[] bytes = objectSerialization.SerializeBinary(instance);
                } catch (Exception ex)
                {
                    while (ex != null)
                    {
                        Message.AppendLine("Binary serialization fails");
                        ReportProblem(ex);
                        ex = ex.InnerException;
                    }
                }
            }
            

            IsTested = true;
        }


        public TypeDiagnosticsEntry(Type _type)
        {
            Set(_type);

            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public );

            foreach (PropertyInfo pi in props)
            {
                if (HasXmlIgnore(pi)) continue;

                if (!pi.CanWrite)
                {
                    ReportProblem("Read-only property", pi);
                }
                if (pi.PropertyType.IsValueType)
                {

                } if (pi.PropertyType.IsInterface)
                {
                    ReportProblem("Interface cannot be XML serialized", pi);
                } if (pi.PropertyType.IsClass)
                {
                    if (pi.PropertyType.isTextOrNumber())
                    {

                    } else
                    {
                        ClassTypeProperties.Add(pi);
                    }
                }

            }
        }

        public void ReportProblem(Exception ex)
        {
            IsTested = true;
            XmlSerializationOk = false;
            
          
            TestExceptions.Add(ex);

            Message.AppendLine("XmlSerialization failed: " + ex.Message);
        }


        public void ReportProblem(String message)
        {
            IsTested = true;
            XmlSerializationOk = false;
            Message.AppendLine(message);
        }

        public void ReportProblem(String message, PropertyInfo pi)
        {
            IsTested = true;
            XmlSerializationOk = false;
            message = "Property [" + pi.PropertyType.GetCleanTypeName() + " : " + pi.Name + "] : " + message;
            Message.AppendLine(message);
            ProblematicProperties.Add(pi);
        }

        public Type type { get; set; }

        public Boolean IsTested { get; set; } = false;

        public Boolean XmlSerializationOk { get; set; } = true;

        public List<PropertyInfo> ProblematicProperties { get; set; } = new List<PropertyInfo>();
        public List<PropertyInfo> ClassTypeProperties { get; set; } = new List<PropertyInfo>();

        public StringBuilder Message { get; set; } = new StringBuilder();

        public List<Exception> TestExceptions { get; set; } = new List<Exception>();

    }
}