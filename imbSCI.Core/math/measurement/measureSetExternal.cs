// --------------------------------------------------------------------------------------------------------------------
// <copyright file="measureSetExternal.cs" company="imbVeles" >
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
namespace imbSCI.Core.math.measurement
{
    using System;

    /// <summary>
    /// MeasureSet for external application
    /// </summary>
    public class measureSetExternal : measureSetBase
    {
        public measureSetExternal(Type target, String name, String desc) : base(target, name, desc)
        {
        }

        ///// <summary>
        ///// Exports this instance.
        ///// </summary>
        ///// <returns></returns>
        //public PropertyCollectionExtendedList export(PropertyCollectionExtendedList output=null)
        //{
        //    if (output == null) output = new PropertyCollectionExtendedList();

        //    output.name = name;
        //    output.description = description;

        //    List<measureDisplayGroup> groups = this.displayGroups.export();

        //    foreach (measureDisplayGroup gr in groups)
        //    {
        //        PropertyCollectionExtended pce = new PropertyCollectionExtended();
        //        pce.name = gr.name;
        //        pce.description = gr.description;
        //        foreach (KeyValuePair<Int32, IMeasure> mes in gr)
        //        {
        //            IMeasure me = mes.Value;
        //            pce.Add(me.name, me);
        //        }
        //        output.Add(pce, pce.name, false);
        //    }

        //    return output;
        //}
    }
}