using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace imbSCI.Core.data.transfer
{
    public class CustomTypeConverterDictionary
    {
        List<ICustomTypeConverter> items { get; set; } = new List<ICustomTypeConverter>();

        public void AddConverter(ICustomTypeConverter converter)
        {
            items.Add(converter);
        }

        public ICustomTypeConverter GetConverter(Type type, Type inputType)
        {
            ICustomTypeConverter output = items.FirstOrDefault(x => x.TargetType == type && x.InputTypes.Contains(inputType));

            //if (items.ContainsKey(type)) return items[type];
            return output;
        }

        public Object ConvertValueFor(Object entryValue, PropertyInfo targetProperty)
        {
            Object v = null; // entry.GetOutputValue(metaProperty);  //metaProperty.GetValue(entry.properties[metaProperty.PropertyName]);

            var converter = GetConverter(targetProperty.PropertyType, entryValue.GetType());
            
            

            if (converter == null)
            {
                 v = entryValue.imbConvertValueSafe(targetProperty.PropertyType);
            } else
            {
                v = converter.Convert(entryValue);
            }
           
            return v;
        }

    }
}