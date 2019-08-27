using imbSCI.Core.extensions.data;
using imbSCI.Core.math;
using imbSCI.Core.math.range.finder;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Core.math.similarity
{
    public class ItemCluster<T>
    {
        public ItemCluster()
        {

        }

        public void Add(T item, Double score)
        {
            items.Add(item);
            scoreDictionary.Add(item, score);
            range.Learn(score);
            if (ClusterSeed == null)
            {
                ClusterSeed = item;
            }
        }

        public T ClusterSeed { get; set; }

        public rangeFinder range { get; set; } = new rangeFinder();

        public Dictionary<T, Double> scoreDictionary { get; set; } = new Dictionary<T, double>();

        public const String NAMEFOR_NOTCLUSTERED_ITEMS = "NotClustered";

        public String name { get; set; } = "";

        public List<T> items { get; set; } = new List<T>();

    }
}