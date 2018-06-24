// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataProviderTools.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.data
{
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Core.reporting.style.setups;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using imbSCI.Data.enums.fields;
    using imbSCI.Reporting.script;
    using System;
    using System.Data;
    using System.IO;

    /// <summary>
    /// Set of static extensions used for data packaging and delovery to metaContent model
    /// </summary>
    public static class dataProviderTools
    {
        /// <summary>
        /// Sets the title, description and/or bottom line
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="bottomLine">The bottom line.</param>
        /// <returns>New or existing data collection with data inside</returns>
        public static PropertyCollection setTitleDescAndBottomLine(this PropertyCollection data, string title = "", string description = "", string bottomLine = "")
        {
            if (data == null) data = new PropertyCollection();
            data.Add(templateFieldBasic.meta_softwareName, title);
            data.addStringToMultikeys(title, true, templateFieldBasic.meta_softwareName, docScriptArguments.dsa_title, docScriptArguments.dsa_name, templateFieldMetaBlock.block_name);
            data.addStringToMultikeys(description, true, templateFieldBasic.meta_subtitle, docScriptArguments.dsa_description, templateFieldMetaBlock.block_desc);
            data.addStringToMultikeys(bottomLine, true, templateFieldBasic.meta_desc, templateFieldMetaBlock.block_bottomline);
            return data;
        }

        /// <summary>
        /// Sets the colors.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="mainColor">Color of the main.</param>
        /// <param name="layoutColor">Color of the layout.</param>
        /// <returns></returns>
        public static PropertyCollection setColors(this PropertyCollection data, acePaletteRole mainColor, acePaletteRole layoutColor)
        {
            if (data == null) data = new PropertyCollection();

            data.addObjectToMultikeys(mainColor, true, templateFieldStyle.style_defaultColorRole, docScriptArguments.dsa_paletteRole);
            data.addObjectToMultikeys(layoutColor, true, templateFieldStyle.style_layoutColorRole);

            return data;
        }

        /// <summary>
        /// Sets the sizing.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="doMerge">if set to <c>true</c> [do merge].</param>
        /// <param name="varRole">The variable role.</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public static PropertyCollection setSizing(this PropertyCollection data, int width, int height, bool doMerge, acePaletteVariationRole varRole, appendRole role)
        {
            if (data == null) data = new PropertyCollection();

            data.addObjectToMultikeys(width, true, templateFieldStyle.style_width, docScriptArguments.dsa_w);
            data.addObjectToMultikeys(height, true, templateFieldStyle.style_height, docScriptArguments.dsa_h);
            data.addObjectToMultikeys(doMerge, true, docScriptArguments.dsa_doMerge);
            data.addObjectToMultikeys(varRole, true, docScriptArguments.dsa_variationRole);
            data.addObjectToMultikeys(role, true, templateFieldStyle.style_appendRole);

            return data;
        }

        /// <summary>
        /// Sets the variator flags.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="headFoot">The head foot.</param>
        /// <param name="tableOps">The table ops.</param>
        /// <param name="oddEven">The odd even.</param>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        public static PropertyCollection setVariatorFlags(this PropertyCollection data, cursorVariatorHeadFootFlags headFoot, appendTableOptionFlags tableOps, cursorVariatorOddEvenFlags oddEven, styleFourSide container)
        {
            if (data == null) data = new PropertyCollection();

            data.addObjectToMultikeys(headFoot, true, templateFieldStyle.style_headFootFlags);
            data.addObjectToMultikeys(tableOps, true, templateFieldStyle.style_appendTableOptionFlags);
            data.addObjectToMultikeys(oddEven, true, templateFieldStyle.style_oddEvenFlags);
            data.addObjectToMultikeys(container, true, templateFieldStyle.style_containerStyle);

            return data;
        }

        /// <summary>
        /// Sets the system status.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="log">The log.</param>
        /// <param name="completeRefresh">if set to <c>true</c> [complete refresh].</param>
        /// <returns></returns>
        public static PropertyCollection setSystemStatus(this PropertyCollection data, string log, bool completeRefresh = false)
        {
            if (data == null) data = new PropertyCollection();

            // data.addObjectToMultikeys(log, false, templateFieldBasic.sys_log);

            data.addObjectToMultikeys(DateTime.Now.ToLongTimeString(), false, templateFieldBasic.sys_time);

            // data.addObjectToMultikeys(imbSystemInfo.getProcessRunTimeSpan(), false, templateFieldBasic.sys_runtime);

            if (completeRefresh)
            {
                data.addObjectToMultikeys(DateTime.Now.ToLongDateString(), false, templateFieldBasic.sys_date);

                data.addObjectToMultikeys(AppDomain.CurrentDomain.FriendlyName, false, templateFieldBasic.sys_app);
                data.addObjectToMultikeys(Directory.GetCurrentDirectory(), false, templateFieldBasic.sys_path);
                string start = System.Diagnostics.Process.GetCurrentProcess().StartTime.ToShortDateString() + " " + System.Diagnostics.Process.GetCurrentProcess().StartTime.ToShortTimeString();
                data.addStringToMultikeys(start, false, templateFieldBasic.sys_start);
                data.addObjectToMultikeys(imbStringGenerators.getRandomString(16), false, templateFieldBasic.sys_uid);
            }
            return data;
        }
    }
}