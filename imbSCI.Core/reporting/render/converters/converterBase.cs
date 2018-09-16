// --------------------------------------------------------------------------------------------------------------------
// <copyright file="converterBase.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.render.converters
{
    using imbSCI.Core.reporting.template;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public abstract class converterBase
    {
        protected stringTemplateConverterCollection _templates;
        public abstract stringTemplateConverterCollection templates { get; }

        //public abstract String GetButton(String caption, String btn_url, Enum btn_color, Enum btn_size);
        // public abstract String GetContent(Enum template, params Object[] resources);
        //public abstract String GetContainerOpen(Enum component, params Object[] resources); //, Enum btn_color=null, Enum btn_size=null, String caption="", String description="");
        //public abstract String GetContainerClose(Enum component, params Object[] resources); // Enum btn_color = null, Enum btn_size = null, String caption = "", String description = "");

        /// <summary>
        /// Makes Bootstrap button.
        /// </summary>
        /// <param name="btn_caption">The BTN caption.</param>
        /// <param name="btn_url">The BTN URL.</param>
        /// <param name="btn_color">Color of the BTN.</param>
        /// <param name="btn_size">Size of the BTN.</param>
        /// <returns>Bootstrap HTML</returns>
        public String GetButton(String btn_caption, String btn_url, String btn_color, String btn_size)
        {
            PropertyCollection data = makeData(btn_color, btn_size, btn_caption, btn_url);

            return templates.Convert(bootstrap_component.button, data);
        }

        public abstract String converterInsert { get; }

        protected PropertyCollection makeData(Object btn_color, Object btn_size, String btn_caption, String btn_url = "")
        {
            PropertyCollection data = new PropertyCollection();
            data.Add(nameof(btn_color), btn_color);
            data.Add(nameof(btn_size), btn_size);
            data.Add(nameof(btn_caption), btn_caption);
            data.Add(nameof(btn_url), btn_url);
            data.Add("c", converterInsert);
            return data;
        }

        protected List<String> _containerEnumValues;
        protected List<String> _templateEnumValues;

        private Type _containerEnumType;

        /// <summary> </summary>
        public abstract Type containerEnumType { get; }

        private Type _templateEnumType;

        /// <summary> </summary>
        public abstract Type templateEnumType { get; }

        public Boolean HasContainerByName(String containerName)
        {
            return containerEnumValues.Contains(containerName);
        }

        public Boolean HasTemplateByName(String containerName)
        {
            return templateEnumValues.Contains(containerName);
        }

        public string GetContainerOpen(String component, String color, String size, String btn_caption = "", String btn_url = "")
        {
            if (HasContainerByName(component))
            {
                Enum compEnum = Enum.Parse(containerEnumType, component) as Enum;
                //  PropertyCollection data = new PropertyCollection();

                PropertyCollection data = makeData(color, size, btn_caption, btn_url);

                return templates.GetContainerOpen(compEnum, data);
            }
            else
            {
                //throw new aceGeneralException("")
            }
            return "";
        }

        public string GetContainerClose(String component)
        {
            if (HasContainerByName(component))
            {
                Enum compEnum = Enum.Parse(containerEnumType, component) as Enum;
                return templates.GetContainerOpen(compEnum);
            }
            else
            {
            }
            return "";
        }

        public string GetContainerClose(Enum component)
        {
            // PropertyCollection data =  resources.getAllEnums().toPropertyCollection();
            return templates.GetContainerClose(component);
        }

        public string GetContainerOpen(Enum component, Enum btn_color, Enum btn_size, String btn_caption = "", String btn_url = "")
        {
            PropertyCollection data = makeData(btn_color, btn_size, btn_caption, btn_url);
            return templates.GetContainerOpen(component, data);
        }

        //public string GetContent(String component, String color, String size)
        //{
        //    if (HasTemplateByName(component))
        //    {
        //        Enum compEnum = Enum.Parse(templateEnumType, component) as Enum;
        //        PropertyCollection data = new PropertyCollection();

        //        data.Add("btn_color",color);
        //        data.Add("btn_size", size);

        //        return templates.Convert(compEnum, data);
        //    }
        //    return "";
        //}

        public string GetContent(Enum template, Enum btn_color, Enum btn_size, String btn_caption = "", String btn_url = "")
        {
            PropertyCollection data = makeData(btn_color, btn_size, btn_caption, btn_url);

            return templates.Convert(template, data);
        }

        public abstract List<String> containerEnumValues { get; }
        public abstract List<String> templateEnumValues { get; }
    }
}