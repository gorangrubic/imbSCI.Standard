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
public class TypePropertyItemConverterForNumericFactor : IPropertyItemConverter
    {
        public Decimal Factor { get; set; } = 1;
        public object Convert(object input)
        {

            Decimal input_dec = imbTypeExtensions.imbConvertValueSafeTyped<Decimal>(input);

            input_dec = input_dec * Factor;

            return input_dec;
            
        }
    }
}