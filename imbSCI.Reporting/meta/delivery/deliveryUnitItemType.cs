// --------------------------------------------------------------------------------------------------------------------
// <copyright file="deliveryUnitItemType.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.delivery
{
    using System.Data;

    /// <summary>
    /// deliveryUnitItemType -- defines one file/content action
    /// </summary>
    public enum deliveryUnitItemType
    {
        none,

        /// <summary>
        /// Text template with <see cref="imbSCI.Data.enums.fields.templateFieldSubcontent"/> and (optionally) <see cref="imbSCI.Data.enums.fields.templateFieldBasic"/> and other placeholders.
        /// </summary>
        /// <remarks>
        /// <para>Subcontent is automatically extracted into <see cref="PropertyCollection"/></para>
        /// </remarks>
        contentTemplate,

        /// <summary>
        /// The content - direct output node
        /// </summary>
        content,

        /// <summary>
        /// String templates with <see cref="imbSCI.Data.enums.fields.templateFieldBasic"/> and other placeholders.
        /// </summary>
        dataTemplate,

        /// <summary>
        /// It is a file that should be included into deliveryInstance output
        /// </summary>
        supportFile,

        /// <summary>
        /// It is a complete folder that should be included into deli
        /// </summary>
        supportFolder,

        /// <summary>
        /// It is an external source of data to be included inside <see cref="imbSCI.Reporting.reporting.render.IRenderExecutionContext.data" /> collection for template application
        /// </summary>
        dataSource,

        /// <summary>
        /// The item is actually automatically generated output
        /// </summary>
        generatedOutput,
    }
}