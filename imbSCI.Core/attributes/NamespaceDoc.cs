// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NamespaceDoc.cs" company="imbVeles" >
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
namespace imbSCI.Core.attributes
{
    using System.Runtime.CompilerServices;

    /// <summary>
    /// <para>Extensive Scientific Data annotation-purpose tools.</para>
    /// </summary>
    /// <remarks>
    /// <para>The <see cref="imbAttribute"/> is used to declare meta information for data agregation, reporting, user-help content creation and similar.</para>
    /// <list>
    /// 	<listheader>
    ///			<term>Key points</term>
    ///		</listheader>
    ///		<item>
    ///			<term>Declare meta annotation for your classes</term>
    ///			<description>Use <see cref="imbAttribute"/> with <see cref="imbAttributeName"/> te control table/column format and legend-content of a property or whole class</description>
    ///		</item>
    ///		<item>
    ///			<term>Aggregate data</term>
    ///			<description>Use <see cref="imbSCI.Core.extensions.table"/> and <see cref="imbSCI.Core.extensions.data"/> to aggregate data</description>
    ///		</item>
    ///		<item>
    ///			<term>Generate spreadsheet reports</term>
    ///			<description>Use <see cref="imbSCI.DataComplex.tables"/> to easly generate feature rich Excel, CSV... reports from object collections</description>
    ///		</item>
    ///		<item>
    ///			<term>Generate text reports</term>
    ///			<description>Use <see cref="imbSCI.Core.data.settingsEntriesForObject"/> to generate textual description of your (setup, or data report summaries) objects</description>
    ///		</item>
    /// </list>
    /// <example>
    /// .NET Framework's basic attributes are used when possible: <see cref="DisplayNameAttribute"/>, <see cref="DescriptionAttribute"/>,
    /// <see cref="CategoryAttribute"/>
    /// <code>
    /// [Category("Switch")]
    /// [DisplayName("doSomething")]
    /// [Description("If true it will do something")]
    /// public Boolean doSomething { get; set; } = true;
    /// </code>
    /// But for more specifics, you can use <see cref="imbAttribute"/> (<c>_imbSCI*</c> snippets)
    /// <code>
    /// [imb(imbAttributeName.measure_letter, "V")] // --- letter we use in our sci article for some parameter
    /// [imb(imbAttributeName.measure_setUnit, "sec")] // ----- unit of measure, associated with the property
    /// [imb(imbAttributeName.reporting_columnWidth, 50)] // ----- width of column in the report spreadsheet
    /// [imb(imbAttributeName.measure_important)] // ----- tells that this column/property is important, resulting in application of highlighted style in the report
    /// [imb(imbAttributeName.reporting_valueformat, "#.0")] // --- specifies number format to be applied for cell value, in the report
    /// </code>
    /// </example>
    /// </remarks>
    /// <seealso cref="imbAttribute" />
    /// <seealso cref="imbAttributeName" />
    [CompilerGenerated]
    internal class NamespaceDoc
    {
    }
}