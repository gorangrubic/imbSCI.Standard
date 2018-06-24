// --------------------------------------------------------------------------------------------------------------------
// <copyright file="propertyMappingTools.cs" company="imbVeles" >
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
namespace imbSCI.Data.data.maps.mapping
{
    #region imbVeles using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion imbVeles using

    /// <summary>
    /// Tools to map relationship between two classes
    /// </summary>
    public static class propertyMappingTools
    {
        /// <summary>
        /// Vraca recnik sa vrednostima iz objekta.
        /// </summary>
        /// <remarks>
        /// Recnik moze biti indeksiran prema imenima propertija za ciljani objekat ili za izvorni objekat
        /// </remarks>
        /// <param name="source">Objekat iz koga se preuzimaju vrednosti</param>
        /// <param name="map">Mapa iz koje preuzima vrednosti</param>
        /// <param name="targetNames">Whom property names to use for the dictionary?</param>
        /// <returns>Recnik sa imenima propertija i vrednostima</returns>
        public static Dictionary<String, Object> getDictionaryFromMappedSource(this Object source, propertyMap map,
                                                                               propertyMapNameSource targetNames = propertyMapNameSource.useTargetNames)
        {
            Dictionary<String, Object> output = new Dictionary<String, Object>();
            if (source == null)
            {
                throw new ArgumentNullException("Can't map from null - dude!", nameof(source)); //new aceGeneralException("Mapping>> izvor je null");
                //return output;
            }

            Type iTI = source.GetType();

            foreach (propertyMapEntry pME in map)
            {
                if (pME.isActive)
                {
                    var pi = iTI.GetProperty(pME.sourceProperty);
                    if (pi != null)
                    {
                        Object val = pi.GetValue(source, null); //source.imbGetPropertySafe(pi, null);
                        String key = pME.targetProperty;
                        if (targetNames == propertyMapNameSource.useSourceNames)
                        {
                            key = pME.sourceProperty;
                        }
                        output.Add(key, val);
                    }
                }
            }

            return output;
        }

        public static List<Object> getValuesFromMappedSource(this object source, propertyMap map)
        {
            List<Object> output = new List<object>();
            Dictionary<String, Object> dictOutput = source.getDictionaryFromMappedSource(map);

            output.AddRange(dictOutput.Values.ToArray());
            return output;
        }

        /// <summary>
        /// Postavlja vednosti iz recnika u dati ciljani objekat - da bi bilo kompatibilno niz mora biti u istom rasporedu kao i u property mapu
        /// </summary>
        /// <param name="target">Objekat koji prima vednosti</param>
        /// <param name="values">Recnik vrednosti koje treba da primeni na ciljani objekat.</param>
        /// <param name="map">Mapa koju treba da koristi za kopiranje vednosti - ako se prosledi primenice podesavanja isActive i kopirace samo one vednosti koje su navedene i u mapi i u prosledjenom recniku</param>
        /// <returns>Broj propertija koji su postavljeni</returns>
        public static Int32 setValuesToMappedTarget(this object target, Dictionary<String, Object> values,
                                                    propertyMap map = null)
        {
            Int32 output = 0;
            if (target == null)
            {
                throw new ArgumentNullException("Can't map to null - dude!", nameof(target)); //new aceGeneralException("Mapping>> izvor je null");
                //return output;
            }

            Type iTI = target.GetType();
            String propName = "";

            if (map != null)
            {
                // prenos pomocu mape
                foreach (propertyMapEntry pME in map)
                {
                    propName = pME.targetProperty;

                    if ((pME.isActive && values.ContainsKey(propName)))
                    {
                        var pi = iTI.GetProperty(propName);
                        if (pi != null)
                        {
                            pi.SetValue(target, values[propName], null); //target.imbSetPropertySafe(pi, values[propName]);
                            output++;
                        }
                    }
                }
            }
            else
            {
                // prenos direktno
                foreach (KeyValuePair<String, Object> pME in values)
                {
                    propName = pME.Key;
                    var pi = iTI.GetProperty(propName);
                    if (pi != null)
                    {
                        //imbPropertyInfo iPI = iTI.allPropertiesByName[propName];
                        //target.imbSetPropertySafe(pi, pME.Value);
                        pi.SetValue(target, pME.Value, null);
                        output++;
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Primenjuje prosledjenu property mapu
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="map"></param>
        /// <returns>Broj propertija koji su postavljeni</returns>
        public static Int32 setObjectByMappedSource(this object target, object source, propertyMap map)
        {
            Dictionary<String, Object> dictOutput = source.getDictionaryFromMappedSource(map);
            return target.setValuesToMappedTarget(dictOutput, map);
        }
    }
}