using imbSCI.Core.extensions.table;
using imbSCI.Core.math;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.math.range.finder;
using imbSCI.Core.math.range.frequency;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.Analyzers.Data;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.NodeQuery;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Validation.TaskValidation
{
    [Serializable]
    public abstract class ValidationResultBase
    {
        public virtual ValidationOutcome SetOutcome(ValidationOutcome outcome, String message)
        {
            Message = message;
            Outcome = outcome;
            return outcome;
        }

        public String Message { get; set; } = "";
        public ValidationOutcome Outcome { get; set; } = ValidationOutcome.undefined;

        [NonSerialized]
        public builderForText reporter  = new builderForText();
    }
}