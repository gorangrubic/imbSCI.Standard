using System;
using System.Linq;
using System.Collections.Generic;

namespace imbSCI.BibTex
{

    /// <summary>
    /// KeyValue entry, a property of a <see cref="BibTexEntryBase"/>
    /// </summary>
    public class BibTexEntryTag
    {
        /// <summary>
        /// Initializes a new blank instance of the <see cref="BibTexEntryTag"/> class.
        /// </summary>
        public BibTexEntryTag() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BibTexEntryTag"/> class.
        /// </summary>
        /// <param name="_key">The key.</param>
        /// <param name="_value">The value.</param>
        public BibTexEntryTag(String _key, String _value)
        {
            Key = _key;
            Value = _value;
        }

        /// <summary>
        /// Property name
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public String Key { get; set; }

        /// <summary>
        /// Clean, unicode version of the value, associated with the <see cref="Key"/>
        /// </summary>
        /// <value>
        /// The value - as written/read in the BibTex file
        /// </value>
        public String Value { get; set; }

        /// <summary>
        /// Source version of the value. Compared to the <see cref="Value"/>, it contains LaTeX symbol tags instead of Unicode equivalents
        /// </summary>
        /// <value>
        /// The source - content of the tag
        /// </value>
        public String source { get; set; }
    }

}