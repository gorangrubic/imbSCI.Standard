// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaSystemInfo.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.meta.blocks
{
    using imbSCI.Core.reporting.colors;
    using imbSCI.Data.enums.fields;
    using imbSCI.Reporting.script;

    public class metaSystemInfo : MetaContainerNestedBase
    {
        //public override IMetaContentNested SearchForChild(string needle)
        //{
        //    needle = CleanNeedle(needle);
        //    if (this.name == needle) return this;
        //    return null;
        //}

        /// <summary>
        /// Performs construction (or upgrade) of DOM with:
        /// </summary>
        /// <param name="resources"></param>
        /// <remarks>
        /// <para>This method is meant to be called just after constructor and before <c>compose</c> or other application method. </para>
        /// <para>It is not automatically called by constructor for easier prerequirements handling. </para>
        /// <para>Inside the method it is safe to access <c>parent</c>, <c>page</c>, <c>document</c> or any other automatic property.</para>
        /// <para>This method is meant to be called just once: it should remove any existing dynamically created nodes at beginning of execution - in purpose that any subsequent call produces the same result</para>
        /// </remarks>
        public override void construct(object[] resources)
        {
            colors = acePaletteRole.colorB;
            width = blockWidth.full;
        }

        public override docScript compose(docScript script)
        {
            if (script == null) script = new docScript(name);
            script.x_scopeIn(this);

            script.heading("System status", 3, false);

            script.AppendPair("Memory allocated", templateFieldBasic.sys_mem, ": ", true);
            script.AppendPair("Free Physical memory", templateFieldBasic.sys_memphysical, ": ", true);
            script.AppendPair("Free Virtual memory", templateFieldBasic.sys_memvirtual, ": ", true);
            script.AppendPair("Free space in paging file", templateFieldBasic.sys_pagefile, ": ", true);
            script.AppendPair("Threads", templateFieldBasic.sys_threads, ": ", true);
            script.AppendPair("Start time", templateFieldBasic.sys_start, ": ", true);
            script.AppendPair("Time since start", templateFieldBasic.sys_runtime, ": ", true);
            script.AppendPair("Start path", templateFieldBasic.sys_path, ": ", true);
            script.AppendPair("OS name", templateFieldBasic.sys_osname, ": ", true);
            script.AppendPair("OS version", templateFieldBasic.sys_osversion, ": ", true);
            script.AppendPair("Bits", templateFieldBasic.sys_cputype, ": ", true);

            // script.add(appendType.c_section, docScriptArguments.dsa_name, docScriptArguments.dsa_content, docScriptArguments.dsa_description, docScriptArguments.dsa_class_attribute, docScriptArguments.dsa_id_attribute).set(name, content, description, "header", "#" + name);

            //script.add(appendType.s_palette).arg(acePaletteRole.colorDefault);

            script.x_scopeOut(this);
            return script;
        }

        private string _description = "";

        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                // Boolean chg = (_description != value);
                _description = value;
                OnPropertyChanged("description");
                // if (chg) {}
            }
        }

        public metaSystemInfo()
        {
            name = "systeminfo";

            description = "";
        }
    }
}