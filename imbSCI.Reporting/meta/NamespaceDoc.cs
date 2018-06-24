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
namespace imbSCI.Reporting.meta
{
    using System.Runtime.CompilerServices;

    /// <summary>
    /// META structure is abstract DATA representation of report: content and relationship between parts of it.
    /// </summary>
    /// <remarks>
    /// <para>Logical hierarchy of a META structure</para>
    /// <list type="bullet">
    ///     <listheader>Structure</listheader>
    ///     <item>
    ///         <term>metaDocumentSet</term>
    ///         <description>The main container for single test report. It is related to one science test run, has unique test run stamp.</description>
    ///     </item>
    ///     <item>
    ///         <term>metaDocument</term>
    ///         <description>Describes an output unit (html, pdf, md, xlsx) - it containes pages that may result in separate subfolder (html, md) files or may be as part of it (psf, xlsx) depending on settings and output format.</description>
    ///     </item>
    ///     <item>
    ///         <term>metaDocumentPage</term>
    ///         <description>
    ///             <para>Containes subpages and/or content blocks</para>
    ///         Describes one output onit for particular data source. It may be separate file or part of <c>metaDocument</c>. In web crawler scenario: describes one particular part of the process</description>
    ///     </item>
    ///     <item>
    ///         <term>MetaContentNestedBase classes : IMetaContent</term>
    ///         <description>Content blocks that describe structure of content and hold particular information to be shown.</description>
    ///     </item>
    /// </list>
    /// </remarks>
    [CompilerGenerated]
    internal class NamespaceDoc
    {
    }
}