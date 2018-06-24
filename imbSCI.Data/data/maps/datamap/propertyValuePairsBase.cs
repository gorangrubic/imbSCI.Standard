// --------------------------------------------------------------------------------------------------------------------
// <copyright file="propertyValuePairsBase.cs" company="imbVeles" >
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
    using imbSCI.Data.collection;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Osnovna kolekcija propertyValueParova -- kolekcija se ne oslanja na imbPropertyInfo, koristi obican String name
    /// </summary>
    ///
    public class propertyValuePairsBase<T> : aceDictionaryCollection<T>, IAceDictionaryCollection where T : class //imbCollectionMeta<Object>, IValuePairs //Dictionary<string, object>, IValuePairs
    {
        /// <summary>
        /// Updataes data from the object. If fields defined does just update. If fields count is 0 - search all propertiesi in object
        /// </summary>
        /// <param name="target"></param>
        public List<object> getValues(Object target, Boolean reset = false)
        {
            List<Object> output = new List<object>();
            if (!checkInput(target, "getValues(Object target)")) return output;
            if (reset) Clear();

            if (Count == 0)
            {
                //var ins =
                //    target.GetType().getPrgetAllPropertyInfoWithValue(
                //        x => !x.isIndexer && x.isReadWrite && x.isPrimitive, false);
                //if (ins.Any())
                //{
                //    ins.ForEach(x => Add(x.property.propertyRealName, x.propertyValue));
                //}
            }
            else
            {
                var iTI = target.GetType();

                foreach (String eMI in this.Keys)
                {
                    PropertyInfo iPI = iTI.GetProperty(eMI);
                    if (iPI != null)
                    {
                        Object vl = iPI.GetValue(target, null); // target.imbGetPropertySafe(iPI);
                        if (vl != null)
                        {
                            // this[eMI] = vl;
                            output.Add(vl);
                        }
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Kompaktni opis podataka
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public String getDataDescription(String format = "")
        {
            if (String.IsNullOrEmpty(format)) format = "{0}={1} ({2})"; //collectionDefinitions.valuePairFormat_standard;

            StringBuilder sb = new StringBuilder();
            foreach (String eMI in this.Keys)
            {
                try
                {
                    Object vl = this[eMI];
                    if (vl == null)
                    {
                        sb.Append(String.Format(format, eMI, "null", "null"));
                    }
                    else
                    {
                        sb.Append(String.Format(format, eMI, vl.ToString(), vl.GetType()));
                    }
                }
                catch (Exception ex)
                {
                    sb.Append("Error: " + ex.Message);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Kopira vrednosti iz sebe u target objekat. Izvrsava konverziju vrednosti ako je valueConversion ukljucen.
        /// </summary>
        /// <remarks>
        /// Dodeljuje vrednosti istoimenim propertijima. Parovi za koje nije pronasao propertije se preskacu.
        /// </remarks>
        /// <param name="target">Objekat u koji upisuje vrednosti</param>

        public void setValues(Object target, Boolean valueConversion = true)
        {
            if (!checkInput(target, "setValues(Object target)")) return;
            foreach (String eMI in this.Keys)
            {
                PropertyInfo iPI = target.GetType().GetProperty(eMI);
                if (iPI != null)
                {
                    Object vl = this[eMI];

                    //if (valueConversion)
                    //{
                    //    vl = vl.imbConvertValueSafe(iPI.PropertyType);
                    //}

                    if (vl != null)
                    {
                        iPI.SetValue(target, vl, null);
                        //target.imbSetPropertySafe(iPI, vl);
                    }
                }
            }
        }

        /// <summary>
        /// Vraca vrednosti iz reda a koje se nalaze pod imenom u propertyValue paru - funkcija mapiranja ustvari
        /// </summary>
        /// <param name="row"></param>
        /// <param name="reset"></param>
        /// <returns></returns>
        public List<Object> getValues(DataRow row, Boolean reset = false)
        {
            List<Object> output = new List<object>();
            foreach (String eMI in this.Keys)
            {
                output.Add(row[eMI]);
            }
            return output;
        }

        //        public Boolean isEqual(propertyValuePairs target)
        //        {
        //            if (target == null) return false;
        //            if (this.Count != target.Count) return false;
        //            if (target.Keys.Any(x => !this.ContainsKey(x))) return false;

        ////            if (target.Any<Object>(x=>x != this[x.Key])) return false;
        //            return this.isSame(target);
        //        }

        /// <summary>
        /// Proverava da li uneti objekat odgovara
        /// </summary>
        /// <param name="target"></param>
        /// <param name="methodCall"></param>
        /// <returns></returns>
        protected Boolean checkInput(Object target, String methodCall)
        {
            Boolean output = false;

            if (target == null)
            {
                //this.note(devNoteType.typology,
                //          methodCall + " source object is null: " + GetType().getTypology().typeSignature(),
                //          " a." + methodCall, devNoteBehaviour.silent);
                return output;
            }

            return true;
        }
    }
}