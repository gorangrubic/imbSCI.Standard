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
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.Core.syntax.generator
{
public class ClassGenerationContext
    {

        public PropertyAppendType propertyAppendType { get; set; } = PropertyAppendType.backingField;

        public PropertyAppendFlags propertyAppendFlags { get; set; } = PropertyAppendFlags.setAll;

        public String classDescription { get; set; } = "";

        public String className { get; set; } = "myClass";
        public String namespaceName { get; set; } = "myNamespace";

        public List<String> namespacesToUse { get; set; } = new List<string>();

    }
}