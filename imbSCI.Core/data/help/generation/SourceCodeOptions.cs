using imbSCI.Core.attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace imbSCI.Core.data.help.generation
{
public class SourceCodeOptions
    {

        /// <summary> If <c>true</c> it will generate missing NamespaceDoc and NamespaceGroupDoc classes, with content generated during Documentation generation  </summary>
        [Category("Flag")]
        [DisplayName("SaveNamespaceDoc")]
        [Description("If <c>true</c> it will generate missing NamespaceDoc and NamespaceGroupDoc classes, with content generated during Documentation generation ")]
        [imb(imbAttributeName.measure_letter, "T")]
        [imb(imbAttributeName.basicColor, "#0066FF")] // color of Excel column header or GUI block...
        [imb(imbAttributeName.reporting_columnWidth, 10)] // width of Excel column ...
        public Boolean SaveNamespaceDoc { get; set; } = false;


        /// <summary> If <c>true</c> it will extract source code examples from internal test units (found in documentation sources) </summary>
        [Category("Flag")]
        [DisplayName("ExampleFromTestUnits")]
        [Description("If <c>true</c> it will extract source code examples from internal test units (found in documentation sources)")]
        [imb(imbAttributeName.measure_letter, "T")]
        [imb(imbAttributeName.basicColor, "#0066FF")] // color of Excel column header or GUI block...
        [imb(imbAttributeName.reporting_columnWidth, 10)] // width of Excel column ...
        public Boolean ExampleFromTestUnits { get; set; } = false;


        /// <summary> If <c>true</c> it will extract examples from the source projects, where possibile </summary>
        [Category("Flag")]
        [DisplayName("ExampleFromInternalUse")]
        [Description("If <c>true</c> it will extract examples from the source projects, where possibile")]
        [imb(imbAttributeName.measure_letter, "T")]
        [imb(imbAttributeName.basicColor, "#0066FF")] // color of Excel column header or GUI block...
        [imb(imbAttributeName.reporting_columnWidth, 10)] // width of Excel column ...
        public Boolean ExampleFromInternalUse { get; set; } = false;



    }
}