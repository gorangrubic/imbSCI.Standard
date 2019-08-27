using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.math.range.finder;
using imbSCI.Core.reporting.render;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace imbSCI.Core.math.range.frequency
{

    /// <summary>
    /// Utility for creation of {T} instance (<see cref="GetMergedInstance()"/>) with property values set to the most frequent values  learned from examples by calling <see cref="Learn(IEnumerable{T})"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InstanceMergerByFrequencies<T> where T : class, new()
    {
        public Dictionary<PropertyInfo, frequencyCounter<Object>> PropertyCounters = new Dictionary<PropertyInfo, frequencyCounter<object>>();

        public InstanceMergerByFrequencies()
        {
            DeployType();

        }


        public InstanceMergerByFrequencies(IEnumerable<T> instances)
        {
            DeployType();
            Learn(instances);
        }

        protected Boolean IsSuitable(Type propertyType)
        {
            if (propertyType.IsPrimitive) return true;
            if (propertyType == typeof(String)) return true;
            if (propertyType.IsEnum) return true;
            return false;
        }

        protected Boolean IsSuitable(PropertyInfo property)
        {
            if (property.GetIndexParameters().Length > 0) return false;
            if (!IsSuitable(property.PropertyType)) return false;
            return (property.CanRead && property.CanWrite);
        }

        protected void DeployType()
        {
            Type type = typeof(T);
            var props = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (var prop in props)
            {
                if (IsSuitable(prop))
                {

                    frequencyCounter<Object> propCounter = new frequencyCounter<object>();
                    PropertyCounters.Add(prop, propCounter);

                }
            }
        }

        public void Learn(IEnumerable<T> instances)
        {

            foreach (T instance in instances)
            {
                foreach (var pair in PropertyCounters)
                {
                    var propertyValue = pair.Key.GetValue(instance, null);
                    if (propertyValue != null)
                    {
                        pair.Value.Count(propertyValue);
                    }
                }
            }

        }

        public T GetMergedInstance()
        {
            T output = new T();

            foreach (var pair in PropertyCounters)
            {
                var topValue = pair.Value.GetItemsWithTopFrequency().FirstOrDefault();
                if (topValue != null)
                {
                    pair.Key.SetValue(output, topValue, null);
                }
            }

            return output;

        }
    }
}