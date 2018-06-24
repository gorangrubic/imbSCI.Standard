// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TFDFCounter.cs" company="imbVeles" >
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
using imbSCI.DataComplex.tables;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.DataComplex.tf_idf
{
    /// <summary>
    ///
    /// </summary>
    public class TFDFCounter
    {
        /// <summary>
        /// Gets the containers.
        /// </summary>
        /// <returns></returns>
        public List<TFDFContainer> GetContainers()
        {
            return items.Values.ToList();
        }

        /// <summary>
        /// Increases the absolute fequency, at the current <c>document</c>, of a term associated with <c>indexForm</c>. If this is the first time the term was counted, it creates internal container, having <c>item</c> optionaly set
        /// </summary>
        /// <param name="indexForm">The index form.</param>
        /// <param name="item">The item.</param>
        public void Add(String indexForm, Object item = null)
        {
            if (items.ContainsKey(indexForm))
            {
                items[indexForm].AddCount(DocumentID, item);
            }
            else
            {
                AddIfNew(indexForm, item);
            }
        }

        /// <summary>
        /// Sets the item for term.
        /// </summary>
        /// <param name="indexForm">The index form.</param>
        /// <param name="item">The item.</param>
        public void SetItemForTerm(String indexForm, Object item)
        {
            AddIfNew(indexForm, item);
        }

        /// <summary>
        /// Gets the document frequency.
        /// </summary>
        /// <param name="indexForm">The index form.</param>
        /// <returns></returns>
        public Int32 GetDocumentFrequency(String indexForm)
        {
            if (items.ContainsKey(indexForm))
            {
                return items[indexForm].documentFrequency;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the term frequency.
        /// </summary>
        /// <param name="indexForm">The index form.</param>
        /// <param name="documentID">The document identifier.</param>
        /// <returns></returns>
        public Int32 GetTermFrequency(String indexForm, Int32 documentID)
        {
            if (items.ContainsKey(indexForm))
            {
                return items[indexForm][documentID];
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the term total frequency.
        /// </summary>
        /// <param name="indexForm">The index form.</param>
        /// <returns></returns>
        public Int32 GetTermTotalFrequency(String indexForm)
        {
            if (items.ContainsKey(indexForm))
            {
                return items[indexForm].totalFrequency;
            }
            return 0;
        }

        /// <summary>
        /// Nexts the document.
        /// </summary>
        /// <returns></returns>
        public Int32 NextDocument()
        {
            DocumentID++;
            return DocumentID;
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <param name="indexForm">The index form.</param>
        /// <returns></returns>
        public TFDFContainer GetContainer(String indexForm)
        {
            if (items.ContainsKey(indexForm))
            {
                return items[indexForm];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all containers.
        /// </summary>
        /// <returns></returns>
        public List<TFDFContainer> GetAllContainers()
        {
            List<TFDFContainer> output = new List<TFDFContainer>();

            foreach (TFDFContainer c in items.Values)
            {
                output.Add(c);
            }

            output.Sort((x, y) => -x.totalFrequency.CompareTo(y.totalFrequency));

            return output;
        }

        //public void ExportTextReport<T>(StringBuilder sb) T where
        //{
        //    var l = GetAllContainers();

        //    l.Sort((x, y) => -x.totalFrequency.CompareTo(y.totalFrequency));

        //    StringBuilder sb = new StringBuilder();
        //    Int32 i = 1;
        //    foreach (TFDFContainer container in TFDF)
        //    {
        //        pipelineTaskTFDFContentSubject s = new pipelineTaskTFDFContentSubject(container);
        //        pipelineTask<pipelineTaskTFDFContentSubject> task = new pipelineTask<pipelineTaskTFDFContentSubject>(s);
        //        sb.AppendLine(String.Format("[" + i.ToString("D5") + "] {0,-40} : DF {1,-5} - TF {2,-5} ", s.tfdf.indexForm, s.tfdf.documentFrequency, s.tfdf.totalFrequency));
        //        i++;
        //        sTasks.Add(task);
        //    }
        //}

        /// <summary>
        /// Gets the table with containers.
        /// </summary>
        /// <returns></returns>
        public objectTable<TFDFContainer> GetTableWithContainers()
        {
            objectTable<TFDFContainer> output = new objectTable<TFDFContainer>(nameof(TFDFContainer.indexForm), nameof(TFDFContainer));

            foreach (TFDFContainer c in items.Values)
            {
                output.Add(c);
            }

            return output;
        }

        /// <summary>
        /// Adds if new.
        /// </summary>
        /// <param name="indexForm">The index form.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private TFDFContainer AddIfNew(String indexForm, Object item)
        {
            if (!items.ContainsKey(indexForm))
            {
                var tmp = new TFDFContainer(indexForm, item, DocumentID);
                items.TryAdd(indexForm, tmp);
                return tmp;
            }
            return items[indexForm];
        }

        public List<String> GetIndexForms()
        {
            List<String> output = new List<string>();
            foreach (var item in items)
            {
                output.Add(item.Value.indexForm);
            }
            return output;
        }

        /// <summary>
        /// Gets or sets the document identifier.
        /// </summary>
        /// <value>
        /// The document identifier.
        /// </value>
        public Int32 DocumentID { get; protected set; } = 0;

        /// <summary>
        /// The items
        /// </summary>
        private ConcurrentDictionary<String, TFDFContainer> items = new ConcurrentDictionary<string, TFDFContainer>();
    }
}