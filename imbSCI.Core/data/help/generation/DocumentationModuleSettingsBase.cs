using imbSCI.Core.attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace imbSCI.Core.data.help.generation
{
public abstract class DocumentationModuleSettingsBase
    {

        /// <summary> If true it use this module for documentation generation </summary>
        [Category("Switch")]
        [DisplayName("UseModule")]
        [Description("If true it use this module for documentation generation")]
        // [imb(imbAttributeName.measure_important, dataPointImportance.important)]
        public Boolean UseModule { get; set; } = true;



    }
}