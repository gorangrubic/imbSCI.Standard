using HtmlAgilityPack;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.DataExtraction.Analyzers.Similarity.similarity;
using imbSCI.Data;
using imbSCI.Data.extensions;
using imbSCI.Data.extensions.data;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.Extractors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Analyzers.Data
{
    public class LeafNodeDictionaryEntryNGram:nGramForObjects<LeafNodeDictionaryEntry>, IEquatable<LeafNodeDictionaryEntryNGram>
    {
        public LeafNodeDictionaryEntryNGram() { }
        public LeafNodeDictionaryEntryNGram(List<LeafNodeDictionaryEntry> entries):base(entries)
        {
        
        }

        /// <summary>
        /// Equals by <see cref="LeafNodeDictionaryEntry.ContentHash"/>
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(LeafNodeDictionaryEntryNGram other)
        {
            if (Count != other.Count) return false;

            for (int i = 0; i < Math.Min(Count,other.Count); i++)
            {
                if (!this[i].ContentHash.Equals(other[i].ContentHash))
                {
                    return false;
                }
            }
            return true;
        }
    }
}