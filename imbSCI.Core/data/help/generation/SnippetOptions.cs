using imbSCI.Core.attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace imbSCI.Core.data.help.generation
{
    public class SnippetOptions
    {

        /// <summary> If <c>true</c> it will link Snippet documentation with members mentioned in snippet code </summary>
        [Category("Flag")]
        [DisplayName("LinkOnMemberDocumentation")]
        [Description("If <c>true</c> it will link Snippet documentation with members mentioned in snippet code")]
        [imb(imbAttributeName.measure_letter, "T")]
        [imb(imbAttributeName.basicColor, "#0066FF")] // color of Excel column header or GUI block...
        [imb(imbAttributeName.reporting_columnWidth, 10)] // width of Excel column ...
        public Boolean LinkOnMemberDocumentation { get; set; } = false;


        /// <summary> If <c>true</c> it will generate cheat sheet page/document for snippets </summary>
        [Category("Flag")]
        [DisplayName("CheatSheet")]
        [Description("If <c>true</c> it will generate cheat sheet page/document for snippets")]
        [imb(imbAttributeName.measure_letter, "T")]
        [imb(imbAttributeName.basicColor, "#0066FF")] // color of Excel column header or GUI block...
        [imb(imbAttributeName.reporting_columnWidth, 10)] // width of Excel column ...
        public Boolean CheatSheet { get; set; } = false;


    }
}