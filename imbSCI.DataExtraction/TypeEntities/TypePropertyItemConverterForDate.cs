using imbSCI.DataExtraction.MetaEntities;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.extensions.enumworks;
using imbSCI.Core.extensions.typeworks;
using System.Globalization;
using imbSCI.Core.extensions.text;
using imbSCI.Data;

namespace imbSCI.DataExtraction.TypeEntities
{
    public class TypePropertyItemConverterForDate : IPropertyItemConverter
    {
        public String format { get; set; }
        public object Convert(object input)
        {
            DateTime result = DateTime.MinValue;
            CultureInfo ci = CultureInfo.CurrentCulture;
            String inputString = input.toStringSafe();
            
            DateTime.TryParseExact(inputString, format, ci, DateTimeStyles.None, out result);
            return result;
        }
    }
}