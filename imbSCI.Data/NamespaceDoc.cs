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
// Project: imbSCI.Data
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Data
{
    /// <summary>
    /// The namespace provides the core layer of shared: enumerations, interfaces and extensions as well as a number of thread-safe collections and data structures.
    /// </summary>
    /// <remarks>
    /// <para>The main purpose of this library is to basic set of building blocks, shared by wide range of imbVeles projects.</para>
    /// <para>Here are thread-safe multidimensional collections and dictionaries consumed on many layers of the imbSCI and imbACE frameworks.</para>
    ///   <list type = "bullet" >
    ///     <listheader>
    ///         <term>Namespace overview</term>
    ///         <description>Brief description on key features of the imbSCI.Core namespace</description>
    ///     </listheader>
    ///     <item>
    ///         <term>Extensions for: <see cref="String"/>s, <see cref="Enum"/>s, directed <see cref="graph"/>s, and paths (<see cref="imbPathExtensions"/>)</term>
    ///         <description>Very elementary extension methods, performing the most frequent operations.</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="imbSCI.Data.data"/>: fundamentals</term>
    ///         <description>Bindable primitives, <see cref="Regex"/>-based string parsers, complex data-structure load/save mechanism and <see cref="Type"/> to <see cref="Type"/> property auto-mappings</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="imbSCI.Data.collection"/>: multidimensional and thread-safe</term>
    ///         <description>Generic and non-generic directed graph, layered stacks, 2D+ relational matrices, dictionaries...</description>
    ///     </item>
    ///     <item>
    ///         <term>Common <see cref="imbSCI.Data.enums"/> and <see cref="imbSCI.Data.interfaces"/></term>
    ///         <description>Mostly revolving around reporting, data annotation and core framework options</description>
    ///     </item>
    /// </list>
    /// </remarks>
    /// <example>
    ///     Several extensions, combined for textual representation of all flags in specified Enum <c>en</c>.
    ///     <code>
    ///     String output = "";
    ///     foreach (Enum eni in en.getEnumListFromFlags()) {
    ///         output = output.add(eni.toStringSafe().imbTitleCamelOperation(), ", ");
    ///     }
    ///     </code>
    /// </example>
    /// <seealso>
    /// <para>Few recommended namespace members to learn more about</para>
    /// <see cref="imbSCI.Data.imbSciEnumExtensions.getEnumListFromFlags(Enum)"/>
    /// <see cref="imbSCI.Data.collection.graph.graphWrapNode{TItem}"/>
    /// <see cref="imbSCI.Data.imbGraphExtensions.getDeepest(IEnumerable{interfaces.IObjectWithPathAndChildren}, int)"/>
    /// <see cref="imbSCI.Data.imbGraphExtensions.getFilterOut(IEnumerable{interfaces.IObjectWithPathAndChildren}, System.Text.RegularExpressions.Regex, System.Text.RegularExpressions.Regex)"/>
    /// <see cref="imbSCI.Data.extensions.data.imbPathExtensions.getPathTo(interfaces.IObjectWithPathAndChildSelector, interfaces.IObjectWithPathAndChildSelector)"/>
    /// <see cref="imbSCI.Data.data.changeBindableBase"/>
    /// <see cref="imbSCI.Data.data.maps.mapping.propertyMappingTools.getValuesFromMappedSource(object, data.maps.mapping.propertyMap)"/>
    /// <see cref="imbSCI.Data.collection.nested.aceEnumDictionarySet2D{TEnum, TD1Key, TD2Key, TValue}"/>
    /// </seealso>
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class NamespaceDoc
    {
    }

    /// <summary>
    /// The namespace provides the core layer of shared: enumerations, interfaces and extensions as well as a number of thread-safe collections and data structures.
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class NamespaceGroupDoc
    {
    }
}