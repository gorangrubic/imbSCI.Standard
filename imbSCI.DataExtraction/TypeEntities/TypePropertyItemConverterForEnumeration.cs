using imbSCI.DataExtraction.MetaEntities;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.extensions.enumworks;
using System.Globalization;
using imbSCI.Core.extensions.text;
using imbSCI.Data;

namespace imbSCI.DataExtraction.TypeEntities
{
public class TypePropertyItemConverterForEnumeration : IPropertyItemConverter
    {
        public reportExpandedData replacements { get; set; } = new reportExpandedData();
        public Type enumType { get; set; }

        public object Convert(object input)
        {
            String input_string = input.toStringSafe();

            foreach (var pair in replacements)
            {
                input_string = input_string.Replace(pair.key, pair.value);
            }
            return imbEnumExtendBase.getEnumByName(enumType, input_string);
        }
    }
}