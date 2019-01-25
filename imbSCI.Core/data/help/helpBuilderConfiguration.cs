using imbSCI.Core.attributes;
using imbSCI.Core.extensions.data;
using imbSCI.Core.files.folders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace imbSCI.Core.data.help
{


    /*
       


   */

    /// <summary>
    /// Configuration for helpBuilder
    /// </summary>
    public class helpBuilderConfiguration
    {


        /// <summary> General blog search URL prefix - called when search blog for this entry link is generated </summary>
        [Category("Label")]
        [DisplayName("GeneralBlogSearchURL")]
        [Description("General blog search URL prefix - called when search blog for this entry link is generated")]
        [imb(imbAttributeName.reporting_escapeoff)] // allows URLs, when reported as HTML
        public String GeneralBlogSearchURL { get; set; } = "http://blog.veles.rs/?s={0}&submit=Search";


        /// <summary> If true it will try to load meta data from XML file </summary>
        [Category("Switch")]
        [DisplayName("doLoadXMLMeta")]
        [Description("If true it will try to load meta data from XML file")]
        // [imb(imbAttributeName.measure_important, dataPointImportance.important)]
        public Boolean doLoadXMLMeta { get; set; } = true;



        /// <summary> Path to export help content, relative to application execution path </summary>
        [Category("Label")]
        [DisplayName("outputPath")]
        [Description("Path to export help content, relative to application execution path")]
        [imb(imbAttributeName.reporting_escapeoff)] // allows URLs, when reported as HTML
        public String outputPath { get; set; } = "help";


        public string resourcesPath { get; set; } = "resources\\help";





    }

}