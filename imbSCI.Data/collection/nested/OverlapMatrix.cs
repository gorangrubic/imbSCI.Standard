using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Data.collection.nested
{
    // using aceCommonTypes.extensions;
    //   using aceCommonTypes.extensions.enumworks;



    /// <summary>
    /// Matrix of overlapping elements
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="imbSCI.Data.collection.nested.aceDictionary2D{System.Int32, System.Int32, System.Collections.Generic.List{T}}" />
    public class OverlapMatrix<T> : aceDictionary2D<Int32, Int32, List<T>> where T : IEquatable<T>
    {
        /// <summary>
        /// Deploys the specified collections.
        /// </summary>
        /// <param name="collections">The collections.</param>
        public void Deploy(IEnumerable<List<T>> collections)
        {
            var col = collections.ToList();

            Dictionary<String, List<T>> computed = new Dictionary<string, List<T>>();
            for (int x = 0; x < col.Count; x++)
            {
                for (int y = 0; y < col.Count; y++)
                {
                    if (x == y)
                    {
                        this[x, y] = col[x].ToList();
                    }
                    else
                    {
                        String k = x.ToString() + ":" + y.ToString();

                        if (computed.ContainsKey(k))
                        {
                            this[x, y] = computed[k];
                        }
                        else
                        {
                            var overlap_xy = col[x].GetCrossSection(col[y]);

                            if (!computed.ContainsKey(k)) computed.Add(k, overlap_xy);

                            k = y.ToString() + ":" + x.ToString();
                            if (!computed.ContainsKey(k)) computed.Add(k, overlap_xy);

                            /*
                            var overlap_yx = new List<T>();

                           

                            for (int xi = 0; xi < col[x].Count; xi++)
                            {
                                for (int yi = 0; yi < col[y].Count; yi++)
                                {
                                    if (col[x][xi].Equals(col[y][yi]))
                                    {
                                        if (!overlap_xy.Contains(col[x][xi])) overlap_xy.Add(col[x][xi]);
                                        if (!overlap_yx.Contains(col[y][yi])) overlap_yx.Add(col[y][yi]);
                                    }
                                }
                            }
                            */



                            this[x, y] = overlap_xy;

                        }
                    }
                }
            }


        }


        public OverlapMatrix(IEnumerable<List<T>> collections)
        {
            Deploy(collections);
        }
    }
}