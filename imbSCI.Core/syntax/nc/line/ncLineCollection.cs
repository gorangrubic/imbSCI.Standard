// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ncLineCollection.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Data.collection;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Kolekcija ncLinija
    /// </summary>
    public class ncLineCollection : aceCollection<ncLine>
    {
        public String writeItemList(Boolean includeLineBeforeAndAfter, Boolean includePrefixAndSufixDots)
        {
            String output = "";
            foreach (ncLine ln in this)
            {
                output = output.log("Line:" + this.writeIndexOutOf(ln));
                output = output.log(this.writeNcLineDescription(ln, includeLineBeforeAndAfter, includePrefixAndSufixDots));
                output = output.newLine();
            }
            return output;
        }

        /// <summary>
        /// Vraca liniju koda sa kontekstualnim informacijama: broj linije, sta je bilo pre toga, sta je bilo posle
        /// </summary>
        /// <param name="line"></param>
        /// <param name="includeLineBeforeAndAfter"></param>
        /// <param name="includePrefixAndSufixDots"></param>
        /// <returns></returns>
        public String writeNcLineDescription(ncLine line, Boolean includeLineBeforeAndAfter, Boolean includePrefixAndSufixDots)
        {
            String codeLineFormat = "[{0,3}] {1}";
            String codeLineEtcSufixPrefix = "...";

            String output = "";
            if (line.index > 2)
            {
                if (includePrefixAndSufixDots)
                {
                    output.log(String.Format(codeLineFormat, line.index - 2, "..."));
                }
            }

            if (includeLineBeforeAndAfter)
            {
                ncLine prevLine = getRelativeLine(line, -1);

                if (prevLine != null)
                {
                    output.log(String.Format(codeLineFormat, prevLine, prevLine.ToString()));
                }
            }

            output.log(String.Format(codeLineFormat, line.index, line.ToString()));

            if (includeLineBeforeAndAfter)
            {
                ncLine nextLine = getRelativeLine(line, 1);

                if (nextLine != null)
                {
                    output.log(String.Format(codeLineFormat, nextLine, nextLine.ToString()));
                }
            }

            ncLine lineAfterNext = getRelativeLine(line, 2);
            if (lineAfterNext != null)
            {
                if (includePrefixAndSufixDots)
                {
                    output.log(String.Format(codeLineFormat, lineAfterNext.index, codeLineEtcSufixPrefix));
                }
            }

            return output;
        }

        /// <summary>
        /// Pronalazi liniju koja je na poziciji indexStep u odnosu na relativeTo liniju
        /// </summary>
        /// <param name="relativeTo">Linija od koje pocinje pretragu</param>
        /// <param name="indexStep">1 = next line, -1 = prev line, -5 = 5 lines before, 5 = 5 lines after, 0 = this line</param>
        /// <returns></returns>
        public ncLine getRelativeLine(ncLine relativeTo, Int32 indexStep = 1)
        {
            if (relativeTo == null) return null;
            if (indexStep == 0) return relativeTo;
            if (!Contains(relativeTo))
            {
                return null;
            }
            return getLineAt(relativeTo.index, false);
        }

        /// <summary>
        /// Vraca liz ninija - nije vazno da li je fromIndex veci ili manji - automatski ce ih poredjati
        /// </summary>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        /// <param name="includingLast"></param>
        /// <returns></returns>
        public List<ncLine> getLines(Int32 fromIndex, Int32 toIndex, Boolean includingLast)
        {
            List<ncLine> output = new List<ncLine>();

            Int32 from = Math.Min(fromIndex, toIndex);
            Int32 to = Math.Max(fromIndex, toIndex);
            if (includingLast) to++;

            for (Int32 a = from; a < to; a++)
            {
                ncLine tmp = getLineAt(a, false);
                if (tmp != null)
                {
                    output.Add(tmp);
                }
            }
            return output;
        }

        /// <summary>
        /// Vraca liniju na indexu - vraca null ako je kolekcija prazna, ili ako je indeks van opsega a returnLast=False
        /// </summary>
        /// <param name="index"></param>
        /// <param name="returnLast">Ako je TRUE onda vraca poslednji clan kolekcija u pravcu gde je indeks izasao iz opsega</param>
        /// <returns>Null ili ncLine na trazenoj liniji</returns>
        public ncLine getLineAt(Int32 index, Boolean returnLast = true)
        {
            if (Count == 0) return null;
            if (index < 0)
            {
                if (returnLast)
                {
                    return this[0];
                }
                else
                {
                    return null;
                }
            }
            if (index >= Count)
            {
                if (returnLast)
                {
                    return this[Count - 1];
                }
                else
                {
                    return null;
                }
            }
            return this[index];
        }

        /// <summary>
        /// Pravi kolekciju selektovanih linija na osnovu prosledjenog kriterijuma koji moze biti selektor ili obican kriterijum
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public ncLineSelectedCollection getSelection(ncLineCriteria criteria)
        {
            List<ncLine> lns = new List<ncLine>();// selectLines(criteria);

            if (criteria.GetType() == typeof(ncLineCriteria))
            {
                lns = selectLines(criteria);
            }
            if (criteria.GetType() == typeof(ncLineRelativeCriteria))
            {
                lns = filterSelectedLines(criteria as ncLineRelativeCriteria);
            }
            if (criteria.GetType() == typeof(ncLineSelector))
            {
                lns = selectLinesBySelector(criteria as ncLineSelector);
            }
            return new ncLineSelectedCollection(this, criteria, lns);
        }

        /// <summary>
        /// Vraca sve linije koje ispunjavaju prost linijski kriterijum
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<ncLine> selectLines(ncLineCriteria criteria, IEnumerable<ncLine> input = null)
        {
            if (input == null) input = this;

            List<ncLine> output = new List<ncLine>();
            if (!String.IsNullOrEmpty(criteria.customCommand))
            {
                criteria.customCommand = criteria.customCommand.ToUpper();
            }

            foreach (ncLine line in input)
            {
                Boolean selectOk = criteria.testLineCriteria(line); // testLineCriteria(line, criteria);
                if (selectOk)
                {
                    output.Add(line);
                }
            }

            return output;
        }

        /// <summary>
        /// Testira lineRelativeCriteria uslov
        /// </summary>
        /// <param name="line"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public Boolean testLineRelativeCriteria(ncLine line, ncLineRelativeCriteria criteria)
        {
            Boolean selectOk = false;
            switch (criteria.relativeType)
            {
                case ncLineRelativeCriteriaType.disabled:
                    selectOk = true;
                    break;

                case ncLineRelativeCriteriaType.onExactPosition:
                    if (criteria.relativePosition == 0)
                    {
                        selectOk = true;
                    }
                    else
                    {
                        ncLine relLine = getLineAt(line.index + criteria.relativePosition, false);
                        if (relLine == null)
                        {
                            selectOk = false;
                        }
                        else
                        {
                            selectOk = criteria.testLineCriteria(relLine, false);//testLineCriteria(relLine, criteria);
                        }
                    }
                    break;

                case ncLineRelativeCriteriaType.anywhereWithin:
                    List<ncLine> relLines = getLines(line.index, line.index + criteria.relativePosition, true);
                    foreach (ncLine ln in relLines)
                    {
                        if (criteria.testLineCriteria(ln, false))
                        {
                            selectOk = true;
                            break;
                        }
                    }
                    break;
            }
            return selectOk;
        }

        /// <summary>
        /// Filtrira listu linija na osnovu relativnog kriterijuma
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<ncLine> filterSelectedLines(ncLineRelativeCriteria criteria, IEnumerable<ncLine> input = null)
        {
            if (input == null) input = this;
            List<ncLine> output = new List<ncLine>();

            foreach (ncLine line in input)
            {
                Boolean selectOk = testLineRelativeCriteria(line, criteria);
                if (selectOk)
                {
                    output.Add(line);
                }
            }

            return output;
        }

        /// <summary>
        /// Selektuje linije na optimizocan nacin - prvo mainCriteria, pa onda redom relCriteria dok prvi ne pukne
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public List<ncLine> selectLinesBySelector(ncLineSelector selector)
        {
            List<ncLine> input = selectLines(selector.mainCriteria);
            Boolean selectOk = false;

            List<ncLine> output = new List<ncLine>();

            foreach (ncLine line in input)
            {
                selectOk = true;
                foreach (ncLineRelativeCriteria relCriteria in selector)
                {
                    selectOk = testLineRelativeCriteria(line, relCriteria);
                    if (selectOk == false) break;
                }
                if (selectOk)
                {
                    output.Add(line);
                }
            }

            return output;
        }
    }
}