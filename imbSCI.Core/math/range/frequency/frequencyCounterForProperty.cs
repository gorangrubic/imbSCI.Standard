using imbSCI.Core.math.range.finder;
using imbSCI.Core.reporting.render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.Core.math.range.frequency
{
public class frequencyCounterForProperty<T, TP>:frequencyCounter<TP>
    {
        public Func<T, TP> PropertySelector { get; protected set; }

        public Dictionary<TP, List<T>> Instances { get; set; } = new Dictionary<TP, List<T>>();

        public List<T> GetInstances(List<TP> propertyValues)
        {
            List<T> output = new List<T>();
            foreach (TP propVal in propertyValues)
            {
                output.AddRange(Instances[propVal]);
            }
            return output;
        }



        public void CountInstance(T input)
        {
            TP val = PropertySelector(input);

            if (!Instances.ContainsKey(val))
            {
                Instances.Add(val, new List<T>());
            }

            Instances[val].Add(input);

            base.Count(val);
        }

        public frequencyCounterForProperty(Func<T, TP> _propertySelector)
        {
            PropertySelector = _propertySelector;
        }
    }
}