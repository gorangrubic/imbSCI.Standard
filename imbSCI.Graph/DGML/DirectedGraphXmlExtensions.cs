// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectedGraphExtensions.cs" company="imbVeles" >
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
using imbSCI.Core.data.descriptors;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using imbSCI.Graph.DGML.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace imbSCI.Graph.DGML
{
public static class DirectedGraphXmlExtensions
    {
        public static void WriteElementAttributes(this IElementWithProporties graphElement, XmlWriter writer, TypeXmlAttributes xmlInfo)
        {
            foreach (PropertyInfoXmlAttributes property in xmlInfo)
            {
                if (!property.Ignore)
                {
                    
                    if (property.IsAttribute)
                    {
                        String v = property.Property.GetValue(graphElement, null).toStringSafe();
                        if (!v.isNullOrEmpty()) {
                            writer.WriteAttributeString(property.ElementName, v);
                        }
                    }
                }
            }

            foreach (var prop in graphElement.Properties)
            {
                if (!prop.value.isNullOrEmpty())
                {
                     writer.WriteAttributeString(prop.key, prop.value);
                }
            }
        }

        public static void WriteXml(this IElementWithProporties graphElement, XmlWriter writer, TypeXmlAttributes xmlInfo)
        {

            writer.WriteStartElement(xmlInfo.Root.ElementName);

            WriteElementAttributes(graphElement, writer, xmlInfo);


            writer.WriteEndElement();
            
        }

        public static void ReadElementAttributes(this IElementWithProporties graphElement,XmlReader reader, TypeXmlAttributes xmlInfo)
        {
            reader.MoveToFirstAttribute();

            do
            {
                String attName = reader.Name;
                var p_info = xmlInfo.Get(attName);
                Object att_value = null;

                if (reader.ReadAttributeValue())
                {
                    att_value = reader.Value;
                }
                if (att_value != null)
                {
                    if (p_info == null)
                    {
                        graphElement.Properties.Add(attName, att_value.toStringSafe());
                    }
                    else
                    {
                        Object att_value_typed = imbTypeExtensions.imbConvertValueSafe(att_value, p_info.Property.PropertyType);
                        p_info.Property.SetValue(graphElement, att_value_typed, null);
                    }
                }

            } while (reader.MoveToNextAttribute());
            reader.MoveToElement();
        }

        public static void ReadXml(this IElementWithProporties graphElement,XmlReader reader, TypeXmlAttributes xmlInfo)
        {
            //reader.ReadStartElement(xmlInfo.Root.ElementName);

            

            ReadElementAttributes(graphElement, reader, xmlInfo);

            //reader.ReadEndElement();
        }


        public static String GetIDFromLabel(this String labelString)
        {
            return labelString.getFilename();
        }
    }
}