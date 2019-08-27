using imbSCI.Core.attributes;
using imbSCI.Core.data;
using imbSCI.Core.enums;
using imbSCI.Core.extensions.data;
using imbSCI.Core.files.fileDataStructure;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.Core.syntax.generator
{
    public class ClassGenerator
    {

        public void GenerateUsings(StringBuilder sb, ClassGenerationContext context)
        {
            foreach (String ns in context.namespacesToUse)
            {
                sb.AppendLine("using " + ns + ";");
            }
        }


        public String GenerateClassCode<T>(List<settingsPropertyEntry> properties, ClassGenerationContext context) where T : BuilderForCode, new()
        {
            T builder = new T();

            ClassGenerationExtensions.GetUsingForTypes(properties.Select(x => x.type), context.namespacesToUse);

            ClassGenerationExtensions.GetUsingForFlags(context.propertyAppendFlags, true, context.namespacesToUse);

            context.namespacesToUse.RemoveDuplicates();


            builder.AppendUsing(context.namespacesToUse);

            builder.OpenNamespace(context.namespaceName);

            builder.OpenClass(context.className, "public", context.classDescription);


            builder.AppendLine();

            builder.OpenMethod(new CodeMethodBlockInfo()
            {
                methodAccess = "public",
                methodName = context.className,
                returnType = "",
                parameters = new List<KeyValuePair<Type, string>>(),
                methodDescription = "Default constructor"
            });

            builder.CloseMethod();

            builder.AppendLine();

            foreach (var spe in properties)
            {
                builder.AppendLine();
                builder.AppendProperty(spe, context.propertyAppendType, context.propertyAppendFlags);
            }




            builder.CloseClass();




            builder.CloseNamespace();

            return builder.GetContent();


        }
    }
}
