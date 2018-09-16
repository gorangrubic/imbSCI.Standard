// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ncLineSelectedCollection.cs" company="imbVeles" >
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
// Project: imbSCI.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Core.syntax.nc.line
{
    using imbSCI.Data;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Kolekcija selektovanih linija
    /// </summary>
    public class ncLineSelectedCollection : ncLineCollection
    {
        public ncLineSelectedCollection()
        {
        }

        /// <summary>
        /// Pasivni konstruktor - nista ne kalkulise samo definise
        /// </summary>
        /// <param name="__source"></param>
        /// <param name="__criteria"></param>
        /// <param name="selected"></param>
        public ncLineSelectedCollection(ncLineCollection __source, ncLineCriteria __criteria, IEnumerable<ncLine> selected)
        {
            source = __source;
            criteria = __criteria;
            foreach (ncLine ln in selected)
            {
                Add(ln);
            }
        }

        /// <summary>
        /// Vraca jednolinijski opis kolekcije
        /// </summary>
        /// <returns></returns>
        public String writeInlineDescription()
        {
            String output = "Selected ";
            output += String.Format("{0} of {1}", this.Count, source.Count);

            output = output.add("using [" + criteria.GetType().Name + "] total criteria: " + criteria.criteriaCount());

            return output;
        }

        #region --- source ------- Izvorna kolekcija

        private ncLineCollection _source;

        /// <summary>
        /// Izvorna kolekcija
        /// </summary>
        public ncLineCollection source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
                OnPropertyChanged("source");
            }
        }

        #endregion --- source ------- Izvorna kolekcija

        #region --- criteria ------- selektor ili prost kriterijum koji je napravio ovu kolekciju

        private ncLineCriteria _criteria;

        /// <summary>
        /// selektor ili prost kriterijum koji je napravio ovu kolekciju
        /// </summary>
        public ncLineCriteria criteria
        {
            get
            {
                return _criteria;
            }
            set
            {
                _criteria = value;
                OnPropertyChanged("criteria");
            }
        }

        #endregion --- criteria ------- selektor ili prost kriterijum koji je napravio ovu kolekciju
    }
}