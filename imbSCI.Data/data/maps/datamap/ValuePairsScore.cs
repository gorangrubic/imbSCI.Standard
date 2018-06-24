// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValuePairsScore.cs" company="imbVeles" >
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
// Project: imbSCI.Data
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Data.data.maps.datamap
{
    #region imbVeles using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion imbVeles using

    /// <summary>
    /// 2013c: LowLevel resurs
    /// </summary>
    public class ValuePairsScore : imbBindable
    {
        #region --- score ------- Bindable property

        private Dictionary<String, Int32> _score = new Dictionary<string, int>();

        /// <summary>
        /// Bindable property
        /// </summary>
        public Dictionary<String, Int32> score
        {
            get { return _score; }
            set
            {
                _score = value;
                OnPropertyChanged("score");
            }
        }

        #endregion --- score ------- Bindable property

        public void clear()
        {
            _score = new Dictionary<string, int>();
        }

        public Dictionary<string, int> sort()
        {
            throw new NotImplementedException();
            //   return score.SortByValue() as Dictionary<String, Int32>;
        }

        /// <summary>
        /// Vraća ključ koji ima najviše poena
        /// </summary>
        /// <returns></returns>
        public String getTopScored()
        {
            if (score.Count == 0) return "";

            String output = score.First().Key;

            foreach (String k in score.Keys)
            {
                if (score[k] > score[output]) output = k;
            }

            return output;
        }

        /// <summary>
        /// Dodaje novi string key u listu ili povecava broj bodova za postojeci
        /// </summary>
        /// <param name="stringKey"></param>
        public void Add(String stringKey)
        {
            if (!score.ContainsKey(stringKey))
            {
                score.Add(stringKey, 0);
            }
            score[stringKey]++;
        }

        /// <summary>
        /// Dodaje pojene za prosleđene parove
        /// </summary>
        /// <param name="ivp"></param>
        public void Add(IValuePairs ivp)
        {
            foreach (String k in ivp.Keys)
            {
                if (!score.ContainsKey(k))
                {
                    score.Add(k, 0);
                }
                score[k]++;
            }
        }
    }
}