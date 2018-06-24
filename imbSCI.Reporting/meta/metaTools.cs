// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaTools.cs" company="imbVeles" >
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
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.extensions;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.render.converters;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.fields;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex.extensions;
    using imbSCI.DataComplex.extensions.data;
    using imbSCI.DataComplex.extensions.data.operations;
    using imbSCI.DataComplex.extensions.data.schema;
    using imbSCI.DataComplex.extensions.text;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.exceptions;
    using imbSCI.Reporting.interfaces;
    using imbSCI.Reporting.links;
    using imbSCI.Reporting.links.groups;
    using imbSCI.Reporting.meta.blocks;
    using imbSCI.Reporting.meta.core;
    using imbSCI.Reporting.meta.delivery;
    using imbSCI.Reporting.meta.document;
    using imbSCI.Reporting.meta.documentSet;
    using imbSCI.Reporting.meta.page;
    using imbSCI.Reporting.meta.presets.links;
    using imbSCI.Reporting.script;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    ///
    /// </summary>
    /// \ingroup_disabled docElementCore
    public static class metaTools
    {
        /// <summary>
        /// Tests <c>test</c> vs <c>currentScope</c> relationship
        /// </summary>
        /// <param name="currentScope">The current scope.</param>
        /// <param name="test">The test.</param>
        /// <param name="testScope">The test scope.</param>
        /// <returns></returns>
        /// <remarks>
        /// <para>Target enums supported:</para>
        /// Same element
        /// <see cref = "metaModelTargetEnum.scope" />
        /// <c>test</c> is direct child of <c>currentScope</c>
        /// <see cref="metaModelTargetEnum.scopeChild"/>
        /// <c>test</c> is child at any level of <c>currentScope</c>
        /// <see cref="metaModelTargetEnum.scopeEachChild"/>
        /// <c>test</c> is parent at any level of <c>currentScope</c>
        /// <see cref="metaModelTargetEnum.scopeParent"/>
        /// <c>test</c> is not null
        /// <see cref="metaModelTargetEnum.scopeRelativePath"/>
        ///
        /// For any other <see cref="metaModelTargetEnum"/> value it will return <c>false</c>, except for <see cref="metaModelTargetEnum.none"/> returns <c>true</c>
        /// </remarks>
        /// <seealso cref="metaModelTargetEnum.scope"/>
        /// <seealso cref="metaModelTargetEnum.scopeChild"/>
        /// <seealso cref="metaModelTargetEnum.scopeEachChild"/>
        /// <seealso cref="metaModelTargetEnum.scopeParent"/>
        /// <seealso cref="metaModelTargetEnum.scopeRelativePath"/>
        public static Boolean testScope(this IObjectWithPathAndChildSelector currentScope, IObjectWithPathAndChildSelector test, metaModelTargetEnum testScope)
        {
            if (test == null) return false;
            switch (testScope)
            {
                default:
                    return false;
                    break;

                case metaModelTargetEnum.none:
                    return true;
                    break;

                case metaModelTargetEnum.scope:
                    return (currentScope == test);
                    break;

                case metaModelTargetEnum.scopeChild:
                    return (test.parent == currentScope);
                    break;

                case metaModelTargetEnum.scopeEachChild:
                    return test.path.StartsWith(currentScope.path);
                    break;

                case metaModelTargetEnum.scopeParent:
                    return currentScope.path.StartsWith(test.path);
                    break;

                case metaModelTargetEnum.scopeRelativePath:
                    return (test != null);
                    break;
            }
            return false;
        }

        /// <summary>
        /// Renders the set as drop downs.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="converter">The converter.</param>
        /// <returns></returns>
        public static string RenderSetAsDropDowns(this reportLinkCollectionSet menu, converterBase converter)
        {
            StringBuilder output = new StringBuilder();
            foreach (var pair in menu)
            {
                if (pair.Value.Count() > 0)
                {
                    output.AppendLine(pair.Value.RenderAsDropdown(converter));
                }
            }
            string outstr = output.ToString();
            return outstr;
        }

        /// <summary>
        /// Renders as dropdown.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="converter">The converter.</param>
        /// <returns></returns>
        public static string RenderAsDropdown(this reportLinkCollection menu, converterBase converter)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine(converter.GetContainerOpen(bootstrap_containers.dropdown_complete, bootstrap_color.basic, bootstrap_size.lg, menu.title, "#"));
            //output.AppendLine(converter.GetContent(bootstrap_component.dropdown_menuheadlink, bootstrap_color.basic, bootstrap_size.lg, menu.title));
            var Group = menu.GetGroupOfFirstLink();

            if (Group == null)
            {
                return "";
            }

            string currentGroup = Group.name;

            foreach (reportLink link in menu)
            {
                if (link.state != reportLinkState.pathIsUrl)
                {
                    throw new aceReportException("Link was not compiled!! compile links before rendering", aceReportExceptionType.executeScriptError);
                }
                if (link.group.name != currentGroup)
                {
                    output.AppendLine(converter.GetContent(bootstrap_component.dropdown_menuheader, bootstrap_color.basic, bootstrap_size.lg, currentGroup.imbTitleCamelOperation(true, false))); //. //.ToTitleCase(), "#"));
                    currentGroup = link.group.name;
                }
                output.AppendLine(converter.GetContent(bootstrap_component.dropdown_menuitem, bootstrap_color.basic, bootstrap_size.lg, link.linkTitle, link.linkPath));
            }

            output.AppendLine(converter.GetContainerClose(bootstrap_containers.dropdown_complete));
            string outstr = output.ToString();
            return outstr;
        }

        /// <summary>
        /// Renders as dropdown.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="converter">The converter.</param>
        /// <returns></returns>
        public static string RenderAsToolbar(this reportLinkCollection menu, converterBase converter, bootstrap_size size = bootstrap_size.unknown)
        {
            if (size == bootstrap_size.unknown)
            {
                if (menu is reportLinkToolbar)
                {
                    reportLinkToolbar menu_reportLinkToolbar = (reportLinkToolbar)menu;
                    size = menu_reportLinkToolbar.size;
                }
                else
                {
                    size = bootstrap_size.md;
                }
            }

            var Group = menu.GetGroupOfFirstLink();

            if (Group == null)
            {
                return "";
            }

            string currentGroup = Group.name;

            StringBuilder output = new StringBuilder();
            output.AppendLine(converter.GetContainerOpen(bootstrap_containers.buttontoolbar, bootstrap_color.primary, size, menu.title, "#"));

            output.AppendLine(converter.GetContainerOpen(bootstrap_containers.buttongroup, bootstrap_color.primary, size, currentGroup.imbTitleCamelOperation(), "#"));

            foreach (reportLink link in menu)
            {
                if (link.state != reportLinkState.pathIsUrl)
                {
                    throw new aceReportException("Link was not compiled!! compile links before rendering", aceReportExceptionType.executeScriptError);
                }
                if (link.group.name != currentGroup)
                {
                    output.AppendLine(converter.GetContainerClose(bootstrap_containers.buttongroup));
                    currentGroup = link.group.name;
                    output.AppendLine(converter.GetContainerOpen(bootstrap_containers.buttongroup, bootstrap_color.primary, size, currentGroup.imbTitleCamelOperation(), "#"));
                }
                output.AppendLine(converter.GetContent(bootstrap_component.button, link.importance.ConvertToBootstrapColor(), size, link.linkTitle, link.linkPath));
            }

            output.AppendLine(converter.GetContainerClose(bootstrap_containers.buttongroup));

            output.AppendLine(converter.GetContainerClose(bootstrap_containers.buttontoolbar));
            string outstr = output.ToString();
            return outstr;
        }

        /// <summary>
        /// Converts the color of to bootstrap.
        /// </summary>
        /// <param name="importance">The importance.</param>
        /// <returns></returns>
        public static bootstrap_color ConvertToBootstrapColor(this dataPointImportance importance)
        {
            switch (importance)
            {
                case dataPointImportance.alarm:
                    return bootstrap_color.warning;
                    break;

                case dataPointImportance.important:
                    return bootstrap_color.danger;
                    break;

                case dataPointImportance.normal:
                    return bootstrap_color.primary;
                    break;

                case dataPointImportance.none:
                    return bootstrap_color.sucess;
                    break;
            }
            return bootstrap_color.danger;
        }

        /// <summary>
        /// The link in template
        /// </summary>
        public static Regex linkInTemplate = new Regex(@"\$\$\$([\w\\\D]+)\$\$\$");

        /// <summary>
        /// Creates link in proper format for template
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static string GetLink(this IMetaContentNested element)
        {
            return "$$$" + element.elementLevel.ToString() + ":" + element.path + "$$$";
        }

        /// <summary>
        /// The elementpath path
        /// </summary>
        public static Regex ELEMENTPATH_PATH = new Regex(@":([\w\\\d\s\-\+\.,\(\)\[\]\$\@\!\&\^\%\#\=]+)");

        /// <summary>
        /// The elementpath element
        /// </summary>
        public static Regex ELEMENTPATH_ELEMENT = new Regex(@"([\w\\\d\s\-\+\.,\(\)\[\]\$\@\!\&\^\%\#\=]+)\:");

        /// <summary>
        /// The elementpath simplepath
        /// </summary>
        public static Regex ELEMENTPATH_SIMPLEPATH = new Regex(@"([\w\\\d\s\-\+\.,\(\)\[\]\$\@\!\&\^\%\#\=]+)");

        /// <summary>
        /// Converts the specified level string.
        /// </summary>
        /// <param name="levelString">The level string.</param>
        /// <returns></returns>
        public static reportElementLevel GetConvertedToLevel(this string levelString)
        {
            switch (levelString)
            {
                case nameof(reportElementLevel.delivery):
                    return reportElementLevel.delivery;
                    break;

                case nameof(reportElementLevel.documentSet):
                    return reportElementLevel.documentSet;
                    break;

                case nameof(reportElementLevel.servicepage):
                    return reportElementLevel.servicepage;
                    break;

                case nameof(reportElementLevel.document):
                    return reportElementLevel.document;
                    break;

                case nameof(reportElementLevel.page):
                    return reportElementLevel.page;
                    break;

                case nameof(reportElementLevel.block):
                    return reportElementLevel.block;
                    break;

                case nameof(reportElementLevel.none):
                    return reportElementLevel.none;
                    break;

                default:
                    return reportElementLevel.none;
                    break;
            }
        }

        /// <summary>
        /// Gets the report element level.
        /// </summary>
        /// <param name="elementPath">The element path.</param>
        /// <returns></returns>
        public static reportElementLevel GetReportElementPathAndLevel(this string elementPath, out string needle)
        {
            reportElementLevel level = reportElementLevel.none;
            bool reg = ELEMENTPATH_SIMPLEPATH.IsMatch(elementPath);

            var e_path = ELEMENTPATH_PATH.Match(elementPath);
            var e_element = ELEMENTPATH_ELEMENT.Match(elementPath);

            needle = e_path.Groups[1].Value.Trim(':');
            level = e_element.Groups[1].Value.Trim(':').GetConvertedToLevel();

            return level;
        }

        /// <summary>
        /// Compiles the links in the template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <param name="levels">The levels.</param>
        /// <returns></returns>
        /// <exception cref="aceReportException">null</exception>
        public static string CompileLinksInTemplate(this string template, deliveryInstance context, reportOutputFormatName format, List<reportElementLevel> levels)
        {
            var mc = linkInTemplate.Matches(template);
            try
            {
                foreach (Match m in mc)
                {
                    string elementPath = m.Groups[1].Value;
                    string elementMatch = m.Groups[0].Value;

                    metaDocumentRootSet root = context.scope.root as metaDocumentRootSet;

                    IMetaContentNested target = root.regPathGet(elementPath); //// as IMetaContentNested; //.resolve(imbSCI.Cores.reporting.style.enums.metaModelTargetEnum.scopeRelativePath, elementPath, null).First();

                    //IMetaContentNested target = context.scope.resolve(imbSCI.Cores.reporting.style.enums.metaModelTargetEnum.scopeRelativePath, elementPath, null).First();

                    string path = target.CompileLinkForElemenet(context, format, levels);
                    template = template.Replace(elementMatch, path);
                }
            }
            catch (Exception ex)
            {
                string msg = "CompileLinksInTemplate failed for [" + context.scope.path + "] : " + ex.Message;
                throw new aceReportException(context, msg, aceReportExceptionType.compileScriptError, null);
            }
            return template;
        }

        /// <summary>
        /// Compiles the links and template elements in the data table, creates duplicate for output, original is not changed
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <param name="levels">The levels.</param>
        /// <returns></returns>
        public static DataTable CompileTable(this DataTable table, deliveryInstance context, reportOutputFormatName format, List<reportElementLevel> levels)
        {
            DataTable output = table.GetClonedShema<DataTable>();
            int rc = table.Rows.Count;
            // IEnumerable<DataRow> rows = table.AsEnumerable().Take(rowsLimit);

            int c = 0;
            for (int i = 0; i < rc; i++)
            {
                DataRow nr = output.NewRow();

                foreach (DataColumn dc in output.Columns)
                {
                    object val = table.Rows[i][dc.ColumnName];
                    if (dc.HasLinks(false))
                    {
                        if (val is string)
                        {
                            if (!val.toStringSafe().Contains("$$$"))
                            {
                                nr[dc] = val;
                            }
                            else
                            {
                                nr[dc] = val.toStringSafe().CompileLinksInTemplate(context, format, levels);
                            }
                        }
                        else
                        {
                            nr[dc] = val;
                        }
                    }
                    else if (dc.HasTemplate(false))
                    {
                        nr[dc] = val.toStringSafe().applyToContent(false, context.data); //.CompileLinksInTemplate(context, format, levels);
                    }
                    else
                    {
                        nr[dc] = val;
                    }
                }

                output.Rows.Add(nr);
            }

            return output;
        }

        /// <summary>
        /// Compiles the link for elemenet
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <param name="levels">The levels.</param>
        /// <returns></returns>
        public static string CompileLinkForElemenet(this IMetaContentNested element, deliveryInstance context, reportOutputFormatName format, List<reportElementLevel> levels)
        {
            string path = "";
            var elLevel = element.getElementLevel();
            switch (elLevel)
            {
                case reportElementLevel.servicepage:
                case reportElementLevel.page:
                    path = element.GetOutputPath(context, format, levels, false);
                    break;

                case reportElementLevel.document:
                    metaDocument doc = element as metaDocument;
                    path = doc.index.GetOutputPath(context, format, levels, false);
                    break;

                case reportElementLevel.documentSet:
                    metaDocumentSet docs = element as metaDocumentSet;
                    path = docs.index.GetOutputPath(context, format, levels, false);
                    break;
            }

            path = path.getWebPathBackslashFormat().applyToContent(false, context.data);
            return path;
        }

        /// <summary>
        /// Creates collection of links with correct url paths
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <param name="levels">The levels.</param>
        /// <returns></returns>
        /// <exception cref="imbSCI.Reporting.exceptions.aceReportException">CompileLinkCollection - found link with undefined state</exception>
        public static reportLinkCollection CompileLinkCollection(this reportLinkCollection menu, deliveryInstance context, reportOutputFormatName format, List<reportElementLevel> levels)
        {
            if (menu == null)
            {
                return null;
                //throw new aceReportException(context,"reportLinkCollection sent to metaTools.CompileLinkCollection() was null", aceReportExceptionType.compileScriptError);
            }
            reportLinkCollection output = new reportLinkCollection(menu.GetMainGroup().name, menu.GetMainGroup().description);
            reportInPackageGroup group = menu.GetMainGroup();
            output.title = menu.title;
            output.description = menu.description;

            foreach (reportLink link in menu)
            {
                if (link.group != group)
                {
                    var g = output.AddGroup(link.group.name, link.group.description);
                    g.priority = link.group.priority;
                    group = g;
                }

                reportLink compiledLink = new reportLink(link);

                bool accept = true;

                switch (link.state)
                {
                    case reportLinkState.pathIsMetaModelPath:
                        compiledLink.element = context.scope.resolve(metaModelTargetEnum.scopeRelativePath, link.linkPath, null).First();
                        compiledLink.state = reportLinkState.pathIsUrl;
                        break;

                    case reportLinkState.registryQuery:
                        compiledLink.element = ((IHasReportRegistry)context.scope.root).reportRegistry.GetReport(link.registryQuery);
                        if (compiledLink.element == null)
                        {
                            compiledLink.state = reportLinkState.undefined;
                        }
                        else
                        {
                            compiledLink.state = reportLinkState.pathIsUrl;
                        }

                        break;

                    case reportLinkState.elementInstance:
                        compiledLink.state = reportLinkState.pathIsUrl;
                        break;

                    case reportLinkState.pathIsUrl:
                        // cool
                        break;

                    case reportLinkState.undefined:
                        accept = false;
                        throw new aceReportException(context, "CompileLinkCollection - found link with undefined state", aceReportExceptionType.compileScriptError).add(link.linkTitle).add(link.linkDescription);
                        break;
                }
                if (compiledLink.linkPath.isNullOrEmpty() && compiledLink.state == reportLinkState.pathIsUrl)
                {
                    compiledLink.linkPath = link.element.CompileLinkForElemenet(context, format, levels); //path;
                }
                if (compiledLink.state != reportLinkState.pathIsUrl)
                {
                    accept = false;
                }
                if (accept) output.AddLink(compiledLink);
            }

            output.sortup();

            return output;
        }

        /// <summary>
        /// Gets the link collection.
        /// </summary>
        /// <param name="scoped">The scoped.</param>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static reportLinkCollection GetLinkCollection(this metaDocument scoped, deliveryInstance context, reportOutputFormatName format, List<reportElementLevel> levels, bool makeAppApsolut = true)
        {
            reportLinkCollection menu = new reportLinkCollection();
            menu.title = scoped.documentTitle.or(scoped.name);
            menu.description = scoped.description;
            //String parent = context.GetDirectoryPath(scoped, levels);
            foreach (metaPage pg in scoped.pages)
            {
                string path = GetOutputPath(pg, context, format, levels, makeAppApsolut);
                menu.AddLink(pg.pageTitle.or(pg.name), pg.pageDescription, path.getWebPathBackslashFormat().Trim('/'));
            }
            return menu;
        }

        /// <summary>
        /// Gets the service page link collection.
        /// </summary>
        /// <param name="scoped">The scoped.</param>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static reportLinkCollection GetServicePageLinkCollection(this metaDocumentSet scoped, deliveryInstance context, reportOutputFormatName format, List<reportElementLevel> levels, bool makeAppApsolut = true)
        {
            reportLinkCollection menu = new reportLinkCollection();
            menu.title = scoped.documentSetTitle.or(scoped.name);
            menu.description = scoped.documentSetDescription;
            //String parent = context.GetDirectoryPath(scoped, levels);
            foreach (metaPage pg in scoped.pages)
            {
                string path = GetOutputPath(pg, context, format, levels, makeAppApsolut);
                menu.AddLink(pg.pageTitle.or(pg.name), pg.pageDescription, path.getWebPathBackslashFormat().Trim('/')); //
                                                                                                                        // menu.AddLink(pg.pageTitle.or(pg.name), pg.pageDescription, path.removeStartsWith(parent).getWebPathBackslashFormat().Trim('/'));
            }
            return menu;
        }

        /// <summary>
        /// Gets the service page link collection.
        /// </summary>
        /// <param name="scoped">The scoped.</param>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static reportLinkCollection GetDocumentSetsLinkCollection(this metaDocumentSet scoped, deliveryInstance context, reportOutputFormatName format, List<reportElementLevel> levels, bool makeAppApsolut = true)
        {
            reportLinkCollection menu = new reportLinkCollection();
            menu.title = scoped.documentSetTitle.or(scoped.name);
            menu.description = scoped.documentSetDescription;
            // String parent = context.GetDirectoryPath(scoped, levels);
            foreach (metaDocumentSet pg in scoped.documentSets)
            {
                string path = pg.GetIndexPath(context, format, levels, makeAppApsolut);
                menu.AddLink(pg.documentSetTitle.or(pg.name), pg.documentSetDescription, path.getWebPathBackslashFormat().Trim('/'));
            }
            return menu;
        }

        /// <summary>
        /// Gets the link collection
        /// </summary>
        /// <param name="scoped">The scoped.</param>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <param name="levels">The levels.</param>
        /// <param name="makeAppApsolut">if set to <c>true</c> [make application apsolut].</param>
        /// <returns></returns>
        public static reportLinkCollectionSet GetLinkCollection(this IMetaContentNested scoped, deliveryInstance context, reportOutputFormatName format, List<reportElementLevel> levels, bool makeAppApsolut = true)
        {
            reportLinkCollectionSet menu = new reportLinkCollectionSet();
            reportElementLevel level = scoped.getElementLevel();

            IMetaContentNested linkFrom = scoped;
            switch (level)
            {
                case reportElementLevel.document:

                //metaDocument document = (metaDocument)linkFrom;
                //menu.Add(document.documentTitle, document.GetLinkCollection(context, format));
                //break;
                case reportElementLevel.page:
                    linkFrom = scoped.document;

                    if (linkFrom != null)
                    {
                        metaDocument document2 = (metaDocument)linkFrom;
                        menu.Add(document2.documentTitle, document2.GetLinkCollection(context, format, levels, makeAppApsolut));
                    }
                    else
                    {
                        if (scoped.parent != null)
                        {
                            return GetLinkCollection(scoped.parent, context, format, levels, makeAppApsolut);
                        }
                    }

                    break;

                case reportElementLevel.documentSet:

                    metaDocumentSet documentSet = (metaDocumentSet)linkFrom;

                    menu.AddInGroup(documentSet.documentSetTitle, documentSet.GetServicePageLinkCollection(context, format, levels, makeAppApsolut));

                    menu.AddInGroup(documentSet.documentSetTitle, documentSet.GetDocumentSetsLinkCollection(context, format, levels, makeAppApsolut));
                    menu.currentItem.GetMainGroup().name = "Report sections";

                    foreach (metaDocument docum in linkFrom)
                    {
                        menu.Add(docum.documentTitle, docum.GetLinkCollection(context, format, levels, makeAppApsolut));
                    }
                    break;
            }

            return menu;
        }

        /// <summary>
        /// Gets the element level.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static reportElementLevel GetElementLevel(this IMetaContentNested element)
        {
            return element.GetType().GetElementLevel();
        }

        /// <summary>
        /// Gets the element level.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static reportElementLevel GetElementLevel(this Type type)
        {
            var tlist = type.GetBaseTypeList(true, true, typeof(MetaContentNestedBase));

            if (tlist.Contains(typeof(metaPage)))
            {
                return reportElementLevel.page;
            }
            else if (tlist.Contains(typeof(metaDocument)))
            {
                return reportElementLevel.document;
            }
            else if (tlist.Contains(typeof(metaDocumentSet)))
            {
                return reportElementLevel.documentSet;
            }
            else if (tlist.Contains(typeof(MetaContainerNestedBase)))
            {
                return reportElementLevel.block;
            }
            else if (tlist.Contains(typeof(deliveryInstance)))
            {
                return reportElementLevel.delivery;
            }
            else
            {
                return reportElementLevel.none;
            }
            return reportElementLevel.none;
        }

        public static string GetIndexPath(this IMetaContentNested scoped, deliveryInstance context, reportOutputFormatName format, List<reportElementLevel> levels, bool makeAppApsolut = true)
        {
            string filename = ""; // context.data.getProperString("", templateFieldBasic.path_file);

            filename = context.GetDirectoryPath(scoped, levels, makeAppApsolut).add("index", "\\").add(format.getDefaultExtension(), ".");
            //   Console.WriteLine("index: [" + filename + "]");
            return filename;
        }

        public static string GetOutputPath(this IMetaContentNested scoped, deliveryInstance context, reportOutputFormatName format, List<reportElementLevel> levels, bool makeAppApsolut = true)
        {
            string filename = ""; // context.data.getProperString("", templateFieldBasic.path_file);
            if (scoped == null)
            {
            }
            string filename_withExtension = scoped.name.add(format.getDefaultExtension(), ".");

            filename = context.GetDirectoryPath(scoped, levels, makeAppApsolut).add(filename_withExtension, "\\");
            //   Console.WriteLine("file: [" + filename + "]");
            return filename;
        }

        /// <summary>
        /// Gets the directory path.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="scoped">The scoped.</param>
        /// <param name="levels">What levels of element are able to create new folder</param>
        /// <param name="makeAppApsolut">if set TRUE it will make path including absolute path to application runtime </param>
        /// <returns></returns>
        public static string GetDirectoryPath(this IRenderExecutionContext context, IMetaContentNested scoped, List<reportElementLevel> levels, bool makeAppApsolut = true)
        {
            string parentPath = "";
            //if (scoped.parent != null)
            //{
            //    IMetaContent con = (IMetaContent)scoped.parent;
            //    parentPath = con.path;
            //}
            //else
            //{
            //
            //}
            List<IMetaContentNested> parents = new List<IMetaContentNested>();
            IMetaContentNested head = (IMetaContentNested)scoped;

            string pDir = "";
            do
            {
                if (levels.Contains(head.elementLevel))
                {
                    pDir = head.name.add(pDir, "\\");
                    parents.Add(head);
                }
                else
                {
                }
                head = head.parent;
            } while (head != null);

            string name = parentPath; //scoped.name.getFilename();

            name = parentPath.add(name, "\\");

            string dir = "";

            if (makeAppApsolut)
            {
                dir = context.directoryRoot.FullName.Replace(Directory.GetCurrentDirectory(), "").removeStartsWith("\\");
                dir = dir.add(pDir, "\\");
            }
            else
            {
                dir = "".t(templateFieldBasic.root_relpath);
                dir = dir + pDir;
            }

            //dir = dir.add(pDir, "\\");

            string output = dir.getCleanFilepath();
            //Console.WriteLine("dir: [" + dir + "]");
            return output; //as String;
        }

        /// <summary>
        /// Runs <c>construct</c> on all subitems from primary collection
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="resources"></param>
        public static void subConstruct(this IMetaContentNested parent, object[] resources)
        {
            List<object> reslist = resources.getFlatList<object>();

            foreach (IMetaComposeAndConstruct cont in parent)
            {
                cont.construct(resources);
            }
        }

        /// <summary>
        /// Run <c>compose(script)</c> on all subitems from primary collection
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        internal static docScript subCompose(this IMetaContentNested parent, docScript script)
        {
            parent.sortChildren();
            bool skip = true;

            foreach (IMetaComposeAndConstruct cont in parent)
            {
                skip = false;
                if (script.flags.HasFlag(docScriptFlags.ignoreNavigation))
                {
                    if (cont is INavigation)
                    {
                        skip = true;
                    }
                }
                if (!skip)
                {
                    docScript subScript = new docScript(script.context);
                    var subscript = cont.compose(subScript);
                    script.insertSub(subscript);
                }
                else
                {
                }
            }
            return script;
        }

        /// <summary>
        /// Checks if the script is the script is initiated - if not it will create new one with name of parent
        /// </summary>
        /// <param name="creator">The creator.</param>
        /// <param name="script">The script.</param>
        /// <returns>Existing or newly created script</returns>
        public static docScript checkScript(this IMetaContentNested creator, docScript script)
        {
            if (script == null) script = new docScript(creator.name);
            return script;
        }

        public static metaLink setTool(this metaLink link, externalTool tool, displayOption display)
        {
            link.tool = tool;
            link.display = display;
            return link;
        }
    }
}