using imbSCI.Core.data;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.reporting.format;
using imbSCI.Core.reporting.render.converters;
using imbSCI.Core.reporting.render.core;
using imbSCI.Core.reporting.zone;
using imbSCI.Data;
using imbSCI.Data.enums;
using imbSCI.Data.enums.appends;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace imbSCI.Core.reporting.render.builders
{
[Flags]
    public enum PropertyAppendFlags
    {
        none = 0,
        setDefaultValue = 1,

        setXmlDocumentation = 2,

        setComponentModelAttributes = 4,

        setXmlSerializationAttributes = 8,

        setSCIReportingDefinitions = 16,

        setAll = setDefaultValue | setXmlDocumentation | setComponentModelAttributes | setXmlSerializationAttributes | setSCIReportingDefinitions
    }
}