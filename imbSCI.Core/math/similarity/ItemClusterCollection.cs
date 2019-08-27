using imbSCI.Core.extensions.data;
using imbSCI.Core.math;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Core.math.similarity
{
    public class ItemClusterCollection<T>
    {
        public ItemClusterCollection()
        {

        }

        /// <summary>
        /// Makes a new cluster, without registrating it - you have to call: <see cref="AddCluster(ItemCluster<T>)"/> to keep it within collection
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public TCluster NewCluster<TCluster>(String name="") where TCluster:ItemCluster<T>, new()
        {
            TCluster output = new TCluster();
            output.name = "C" + Clusters.Count();

            return output;
        }

        public void AddCluster(ItemCluster<T> input)
        {
            Clusters.Add(input);
        }

        /// <summary>
        /// Number of clusters (without the null cluster)
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public Int32 Count
        {
            get
            {
                return Clusters.Count;
            }
        }

        public List<ItemCluster<T>> GetClusters(Boolean includeNullCluster)
        {
            var output = Clusters.ToList();
            if (includeNullCluster)
            {
                output.Add(NullCluster);
            }
            return output;
        }

        public ItemCluster<T> NullCluster { get; protected set; } = new ItemCluster<T>()
        {
            name = ItemCluster<T>.NAMEFOR_NOTCLUSTERED_ITEMS
        };

        protected List<ItemCluster<T>> Clusters { get; set; } = new List<ItemCluster<T>>();
    }
}