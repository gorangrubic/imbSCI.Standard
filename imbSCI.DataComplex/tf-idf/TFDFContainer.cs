// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TFDFContainer.cs" company="imbVeles" >
//
// Copyright (C) 2018 imbVeles
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// <summary>
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Xml.Serialization;

namespace imbSCI.DataComplex.tf_idf
{
    /// <summary>
    ///
    /// </summary>
    public class TFDFContainer
    {
        public TFDFContainer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TFDFContainer"/> class.
        /// </summary>
        /// <param name="_indexForm">The index form.</param>
        /// <param name="_item">The item.</param>
        /// <param name="_documentID">The document identifier.</param>
        public TFDFContainer(String _indexForm, Object _item, Int32 _documentID)
        {
            indexForm = _indexForm;
            items.Add(_item);

            //item = _item;
            onDocumentFrequency.TryAdd(_documentID, 1);
        }

        /// <summary>
        /// Adds the count.
        /// </summary>
        /// <param name="_documentID">The document identifier.</param>
        public void AddCount(Int32 _documentID, Object _item = null)
        {
            if (onDocumentFrequency.ContainsKey(_documentID))
            {
                onDocumentFrequency[_documentID]++;
                //var tmp = new TFDFContainer(indexForm, item, _documentID);
                //items.TryAdd(indexForm, tmp);
                //return tmp;
            }
            else
            {
                onDocumentFrequency.TryAdd(_documentID, 1);
            }

            items.Add(_item);

            totalFrequency++;
        }

        /// <summary>
        /// Gets the term frequency at the specified document identifier.
        /// </summary>
        /// <value>
        /// The <see cref="System.Int32" />.
        /// </value>
        /// <param name="documentID">The document identifier.</param>
        /// <returns></returns>
        public Int32 this[Int32 documentID]
        {
            get
            {
                if (onDocumentFrequency.ContainsKey(documentID))
                {
                    return onDocumentFrequency[documentID];
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets the index form.
        /// </summary>
        /// <value>
        /// The index form.
        /// </value>
        public String indexForm { get; protected set; } = "";

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        [XmlIgnore]
        public ConcurrentBag<Object> items { get; protected set; } = new ConcurrentBag<object>();

        /// <summary>
        /// Gets or sets the term frequency.
        /// </summary>
        /// <value>
        /// The term frequency.
        /// </value>
        public Int32 totalFrequency { get; protected set; } = 1;

        protected Int32 _documentFrequency = 0;

        /// <summary>
        /// Gets or sets the document frequency.
        /// </summary>
        /// <value>
        /// The document frequency.
        /// </value>
        public Int32 documentFrequency
        {
            get
            {
                return onDocumentFrequency.Count();
            }

            set
            {
                _documentFrequency = value;
            }
        }

        /// <summary>
        /// Gets or sets the on document frequency.
        /// </summary>
        /// <value>
        /// The on document frequency.
        /// </value>
        private ConcurrentDictionary<Int32, Int32> onDocumentFrequency { get; set; } = new ConcurrentDictionary<Int32, Int32>();
    }
}