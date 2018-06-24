// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValuePairsExtension.cs" company="imbVeles" >
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
    using System.Collections;

    #endregion imbVeles using

    /// <summary>
    /// EKSTENZIJE za IVALUE PAIRS kolekcije
    /// </summary>
    public static class IValuePairsExtension
    {
        /// <summary>
        /// Vraca IValuePair sa vrednostima koje je uzeo iz IDictionary dataSource-a -- ne sme da bude ekstenzija jer ce puci XmlSerijalizacija
        /// </summary>
        /// <typeparam name="TVP"></typeparam>
        /// <param name="selectedFields"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static TVP getValues<TVP>(this IValuePairs selectedFields, IDictionary dataSource)
            where TVP : class, IValuePairs, new()
        {
            TVP output = new TVP();
            foreach (String eMI in selectedFields.Keys)
            {
                if (dataSource.Contains(eMI as Object))
                {
                    output.Add(eMI, dataSource[eMI as Object]);
                }
            }
            return output;
        }

        private static IValuePairs _getCrossSection(IValuePairs output, IValuePairs source, IValuePairs second,
                                                    crossSectionType cst)
        {
            foreach (String eMI in source.Keys)
            {
                if (second.ContainsKey(eMI))
                {
                    if (second[eMI] == source[eMI])
                    {
                        switch (cst)
                        {
                            case crossSectionType.differentValues:

                                break;

                            case crossSectionType.sameValues:
                                output.Add(eMI, second[eMI]);
                                break;
                        }
                    }
                    else
                    {
                        switch (cst)
                        {
                            case crossSectionType.differentValues:
                                output.Add(eMI, second[eMI]);
                                break;

                            case crossSectionType.sameValues:
                                break;
                        }
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Vraca presek koji ce biti dostavljen u odabranom tipu kolekcije
        /// </summary>
        /// <typeparam name="TVP"></typeparam>
        /// <param name="source"></param>
        /// <param name="second"></param>
        /// <param name="cst"></param>
        /// <returns></returns>
        public static TVP getCrossSection<TVP>(this IValuePairs source, IValuePairs second, crossSectionType cst)
            where TVP : class, IValuePairs, new()
        {
            TVP output = new TVP();
            return _getCrossSection(output, source, second, cst) as TVP;
        }

        /// <summary>
        /// Vraca presek u kolekciji koja ce biti istog tipa kao i source parametar
        /// </summary>
        /// <param name="source">kolekcija sa cijim se imenima poredi</param>
        /// <param name="second"></param>
        /// <param name="cst"></param>
        /// <returns></returns>
        public static IValuePairs getCrossSection(this IValuePairs source, IValuePairs second, crossSectionType cst)
        {
            IValuePairs output = Activator.CreateInstance(source.GetType(), null) as IValuePairs;
            //IValuePairs output = source.GetType().getInstance() as IValuePairs;
            return _getCrossSection(output, source, second, cst);
        }
    }
}