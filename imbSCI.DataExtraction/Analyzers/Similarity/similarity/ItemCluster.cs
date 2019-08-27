using imbSCI.Core.extensions.data;
using imbSCI.Core.files.folders;
using imbSCI.Core.math;
using imbSCI.Core.math.range.finder;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace imbSCI.DataExtraction.Analyzers.Similarity.similarity
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
            //if (ClusterSeed == null)
            //{
            //    ClusterSeed = item;
            //}
        }

        /// <summary>
        /// Removes the specified item from cluster
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(T item)
        {
            items.Remove(item);
            if (scoreDictionary.ContainsKey(item)) scoreDictionary.Remove(item);
            range = null;
        }

        public T ClusterSeed { get; set; }

        public rangeFinder range
        {
            get {
                if (_range == null)
                {
                    if (scoreDictionary != null)
                    {
                        _range = new rangeFinder();
                        foreach (var pair in scoreDictionary)
                        {
                            _range.Learn(pair.Value);
                        }
                    }
                }
                return _range;
            }
            set { _range = value; }
        }

        public Dictionary<T, Double> scoreDictionary { get; set; } = new Dictionary<T, double>();

        public const String NAMEFOR_NOTCLUSTERED_ITEMS = "NotClustered";
        private rangeFinder _range = new rangeFinder();

        public String name { get; set; } = "";

        public List<T> items { get; set; } = new List<T>();

    }
}