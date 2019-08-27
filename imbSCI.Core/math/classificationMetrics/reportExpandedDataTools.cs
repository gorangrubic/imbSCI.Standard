// --------------------------------------------------------------------------------------------------------------------
// <copyright file="classificationReport.cs" company="imbVeles" >
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
using imbSCI.Core.attributes;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Serialization;


namespace imbSCI.Core.math.classificationMetrics
{
    public static class reportExpandedDataTools
    {

        public static void SaveObjectPairs(this reportExpandedData settings, String filename_prefix, folderNode folder)
        {
            var objectPairs = settings.GetObjectPairs();

            foreach (var pair in objectPairs)
            {
                String filename = filename_prefix.add(pair.key, "_") + ".xml";
                String path = folder.pathFor(filename, Data.enums.getWritableFileMode.overwrite, "Object XML stored in " + filename_prefix);
                File.WriteAllText(path, pair.value);
            }

        }

        public static void AddObjectEntry(this reportExpandedData settings, String key, Object objectToInsert, String Description = "")
        {
            String vs = objectSerialization.ObjectToXML(objectToInsert, objectToInsert.GetType());
            settings.Add(key, vs, Description);
        }

        public static T GetObjectEntry<T>(this reportExpandedData settings, String key) where T : class, new()
        {

            var vs = settings.FirstOrDefault(x => x.key == key);

            if (vs == null) return null;

            var v = vs.value;
            //objectSerialization.ObjectToXML(objectToInsert, objectToInsert.GetType());
            //settings.Add(key, v, Description);
            return objectSerialization.ObjectFromXML<T>(v);
        }


        /// <summary>
        /// Sets the settings from data.
        /// </summary>
        /// <param name="ObjectToSet">The ObjectToSet.</param>
        /// <param name="settings">The settings.</param>
        public static void SetSettingsFromData(this reportExpandedData settings, Object ObjectToSet)
        {
            var properties = ObjectToSet.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            var dict = settings.GetDictionary();

            foreach (PropertyInfo pi in properties)
            {
                if (pi.GetIndexParameters().Length > 0) continue;

                

                if (dict.ContainsKey(pi.Name))
                {
                    Object v = null;
                    if (pi.PropertyType.IsClass)
                    {

                        var xml = dict[pi.Name].value;
                        if (xml.Contains(pi.PropertyType.Name))
                        {
                            xml = new Regex("\\<\\?xml.*\\?>").Replace(xml, "");
                            xml = xml.Trim(Environment.NewLine.ToCharArray());
                            
                            v = objectSerialization.ObjectOfTypeFromXML(xml, pi.PropertyType);
                        }

                    }
                    else
                    {
                        v = dict[pi.Name].value.imbConvertValueSafe(pi.PropertyType);
                    }


                    pi.SetValue(ObjectToSet, v, null);
                }
            }
        }

        public static void GetSettingsFromObjectToSet(this reportExpandedData settings, Object ObjectToSet)
        {
            // reportExpandedData settings = new reportExpandedData();

            var properties = ObjectToSet.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);

            foreach (PropertyInfo pi in properties)
            {
                if (pi.GetIndexParameters().Length > 0) continue;
                var v = pi.GetValue(ObjectToSet, new object[] { });
                String vs = "";

                if (pi.PropertyType.IsClass)
                {
                    vs = objectSerialization.ObjectToXML(v, pi.PropertyType);

                }
                else
                {
                    vs = v.ToString(); //= dict[pi.Name].value.imbConvertValueSafe(pi.PropertyType);
                }


                settings.Add(pi.Name, vs, "");

            }

            //  return settings;
        }


    }
}