using imbSCI.Core.extensions.data;
using imbSCI.Core.math;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Core.math.similarity
{

    public class SimilarityBasedClusterizationMethodBase<T>
    {



    }
    public interface ISimilarityResultPair
    {
        
    }

    /// <summary>
    /// Base structure for similarity computation pair
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimilarityResultPair<T> where T:class
    {
        public Boolean IsRelatedTo(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                if (IsRelatedTo(item)) return true;
            }
            return false;
        }

        public Boolean IsRelatedTo(T node)
        {
            if (itemA == node) return true;
            if (itemB == node) return true;
            return false;
        }

        public T itemA { get; set; }
        public T itemB { get; set; }

        public virtual Double similarity { get; set; }

        public SimilarityResultPair() { }

        public SimilarityResultPair(T _itemA, T _itemB, Double _score)
        {
            itemA = _itemA;
            itemB = _itemB;
            similarity = _score;
        }

    }
}