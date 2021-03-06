// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyList.cs" company="imbVeles" >
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
// Project: imbSCI.Graph
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using imbSCI.Core.data;
using imbSCI.Graph.DGML.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace imbSCI.Graph.DGML.collections
{
    public class PropertyList : List<Property>
    {
        public PropertyList()
        {
        }

        [XmlIgnore]
        public List<settingsEntriesForObject> SEOs { get; protected set; } = new List<settingsEntriesForObject>();


        //public void RegisterProperties(settingsEntriesForObject seo)
        //{
        //    if (SEOs.Contains(seo)) return 
        //}

        public Property RegisterProperty(settingsPropertyEntry sme)
        {
            String UID = sme.memberInfo.DeclaringType.Name + "." + sme.memberInfo.Name;

            Property p = this.FirstOrDefault(x => x.Id == UID);
            if (p != null) return p;
           
            p = new Property(UID, sme.displayName, sme.pi.PropertyType);

            Add(p);

            return p;
        }

        public Property RegisterProperty(PropertyInfo pi)
        {
            String UID = pi.DeclaringType.Name + "." + pi.Name;

            Property p = this.FirstOrDefault(x => x.Id == UID);
            if (p != null) return p;

             p = new Property(UID, pi.Name, pi.PropertyType);

            Add(p);

            return p;
        }

        public Property RegisterProperty(String UID, String Label, String DataType)
        {
            Property p = this.FirstOrDefault(x => x.Id == UID);
            if (p != null) return p;
            p = new Property()
            {
                Id = UID,
                Label = Label
            };
            p.DataType = DataType;

            Add(p);

            return p;
        }

        ///// <summary>
        ///// Adds the property.
        ///// </summary>
        ///// <param name="propertyName">Name of the property.</param>
        ///// <param name="label">The label.</param>
        ///// <param name="type">The type.</param>
        ///// <returns></returns>
        //public Property AddProperty(String propertyName, String label, Type type)
        //{
        //    if (!this.Any(x => x.Id == propertyName))
        //    {
        //        Property p = new Property(propertyName, label, type);
        //        Add(p);
        //        return p;
        //    }
        //    return this.FirstOrDefault(x => x.Id == propertyName);
        //}
    }
}