// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Property.cs" company="imbVeles" >
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
using imbSCI.Core.extensions.text;
using imbSCI.Core.math.classificationMetrics;
using System;
using System.Xml.Serialization;

namespace imbSCI.Graph.DGML.core
{
    /// <summary>
    ///
    /// </summary>
    public class Property:IElementWithProporties
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Property"/> class.
        /// </summary>
        public Property()
        {
        }

        public Property(String propertyName, String label, Type type)
        {
            Id = propertyName;
            Label = label;
            DataType = type.FullName;
        }

        /// <summary>
        /// Creates new property [Experimental]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="_label">The label.</param>
        /// <returns></returns>
        public Property MakeProperty<T>(T value, String _label)
        {
            var tmp = new Property();
            tmp.Id = _label.getValidFileName();
            tmp.Label = _label;
            tmp.DataType = typeof(T).FullName;
            tmp.Value = value;
            return tmp;
        }

        public String Id { get; set; } = "";

        public String Label { get; set; } = "";

        public String DataType { get; set; } = "";

        public Boolean IsReference { get; set; } = false;


        [XmlIgnore]
        public reportExpandedData Properties { get; protected set; } = new reportExpandedData();

        [XmlIgnore]
        public Object Value { get; set; }
    }
}