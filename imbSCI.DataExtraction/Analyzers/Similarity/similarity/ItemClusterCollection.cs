using imbSCI.Core.extensions.data;
using imbSCI.Core.math;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.DataExtraction.Analyzers.Similarity.similarity
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

        public virtual void AddCluster(ItemCluster<T> input)
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

        protected TCluster ConvertCluster<TCluster>(ItemCluster<T> c) where TCluster : ItemCluster<T>, new()
        {
            TCluster output = new TCluster();
            output.name = c.name;
            
            output.ClusterSeed = c.ClusterSeed;
            foreach (T item in c.items)
            {
                if (c.scoreDictionary.ContainsKey(item))
                {
                    output.Add(item, c.scoreDictionary[item]);
                } else
                {
                    //output.Add(item, )
                }
            }
            return output;
        }

        protected void RegisterItem<TCluster>(Dictionary<T, List<TCluster>> output, T item, TCluster cluster) where TCluster:ItemCluster<T>, new()
        {
            if (!output.ContainsKey(item)) output.Add(item, new List<TCluster>());
            if (!output[item].Contains(cluster)) output[item].Add(cluster);
        }

        /// <summary>
        /// Removes empty clusters
        /// </summary>
        /// <returns>Number of removed clusters</returns>
        public Int32 RemoveEmptyClusters()
        {
            List<Object> emptyClusters = new List<object>();

            foreach (var cluster in Clusters)
            {
                if (cluster.items.Count == 0)
                {
                    emptyClusters.Add(cluster);
                }
            }

            foreach (var ec in emptyClusters)
            {
                Clusters.Remove(ec as ItemCluster<T>);
            }

            return emptyClusters.Count;
        }

        /// <summary>
        /// Gets the item to cluster associations.
        /// </summary>
        /// <typeparam name="TCluster">The type of the cluster.</typeparam>
        /// <returns></returns>
        public Dictionary<T, List<TCluster>> GetItemToClusterAssociations<TCluster>() where TCluster:ItemCluster<T>, new()
        {
            Dictionary<T, List<TCluster>> output = new Dictionary<T, List<TCluster>>();

            var clusters = GetClusters<TCluster>(false);

            foreach (var cluster in clusters)
            {
                RegisterItem<TCluster>(output, cluster.ClusterSeed, cluster);

                foreach (var item in cluster.items)
                {
                    RegisterItem<TCluster>(output, item, cluster);
                }
            }

            return output;
        }

        /// <summary>
        /// Gets the clusters of specified type
        /// </summary>
        /// <typeparam name="TCluster">The type of the cluster.</typeparam>
        /// <param name="includeNullCluster">if set to <c>true</c> [include null cluster].</param>
        /// <returns></returns>
        public List<TCluster> GetClusters<TCluster>(Boolean includeNullCluster) where TCluster:ItemCluster<T>, new()
        {
            List<TCluster> output = new List<TCluster>();
            foreach (var c in Clusters)
            {
                if (c is TCluster cTc)
                {
                    output.Add(cTc);
                }
                
            }
            
            if (includeNullCluster)
            {
                output.Add(ConvertCluster<TCluster>(NullCluster));
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