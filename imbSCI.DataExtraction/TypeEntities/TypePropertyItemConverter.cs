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
  public class TypePropertyItemConverter
    {

        public TypePropertyItemConverter()
        {

        }
        public TypePropertyItemConverterType type { get; set; } = TypePropertyItemConverterType.Undefined;

        public String Definition { get; set; } = "";

        public IPropertyItemConverter GetConverter(PropertyInfo propertyInfo)
        {
            switch (type)
            {
                case TypePropertyItemConverterType.DateTimeConverter:
                    TypePropertyItemConverterForDate dateConverter = new TypePropertyItemConverterForDate()
                    {
                        format = Definition
                    };
                    return dateConverter;
                    break;
                case TypePropertyItemConverterType.EnumerationConverter:
                    TypePropertyItemConverterForEnumeration enumConverter = new TypePropertyItemConverterForEnumeration()
                    {
                        enumType = propertyInfo.PropertyType
                    };
                    var pairs = Definition.SplitSmart(" ", "", true, true);
                    foreach (var p in pairs)
                    {
                        var pvls = p.SplitSmart(":", "", true, true);
                        enumConverter.replacements.Add(pvls[0], pvls[1], "");
                    }
                    return enumConverter;
                    break;
                case TypePropertyItemConverterType.NumericFactor:
                    TypePropertyItemConverterForNumericFactor factorConverter = new TypePropertyItemConverterForNumericFactor();

                    factorConverter.Factor = imbTypeExtensions.imbConvertValueSafeTyped<Decimal>(Definition);

                    return factorConverter;
                    
                    break;

                default:
                    return null;
                    break;
                    
            }
        }


    }
}