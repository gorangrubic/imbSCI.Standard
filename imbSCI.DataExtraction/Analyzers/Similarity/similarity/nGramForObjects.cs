using imbSCI.Core.extensions.data;
using imbSCI.Core.math;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.DataExtraction.Analyzers.Similarity.similarity
{
    /// <summary>
    /// n-gram structure when sets of objects are compared for similarity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.List{T}" />
    /// <seealso cref="System.IEquatable{imbSCI.Core.math.similarity.nGramForObjects{T}}" />
    public class nGramForObjects<T>:List<T>, IEquatable<nGramForObjects<T>>
    {
        public nGramForObjects() { }

        public nGramForObjects(IEnumerable<T> entries)
        {
            AddRange(entries);
        }

        /// <summary>
        /// Equals by <see cref="LeafNodeDictionaryEntry.ContentHash"/>
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public virtual bool Equals(nGramForObjects<T> other)
        {
            if (Count != other.Count) return false;

            for (int i = 0; i < Math.Min(Count,other.Count); i++)
            {
                if (!this[i].Equals(other[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}