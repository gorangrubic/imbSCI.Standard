// --------------------------------------------------------------------------------------------------------------------
// <copyright file="reportLinkCollection.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.links
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting;
    using imbSCI.Data;
    using imbSCI.Reporting.links.groups;
    using imbSCI.Reporting.links.reportRegistry;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Report link collection
    /// </summary>
    public class reportLinkCollection : IEnumerable<reportLink>
    {
        public int Count()
        {
            return items.Count;
        }

        public reportLinkCollection()
        {
            AddGroup("main", "");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="reportLinkCollection"/> class. Creates main group with specified name and secription
        /// </summary>
        /// <param name="mainGroupName">Name of the main group.</param>
        /// <param name="mainGroupDesc">The main group desc.</param>
        public reportLinkCollection(string mainGroupName, string mainGroupDesc)
        {
            AddGroup(mainGroupName, mainGroupDesc);
            title = mainGroupName;
            description = mainGroupDesc;
        }

        public reportLinkCollection(reportInPackageGroup __group)
        {
            if (__group == null)
            {
                title = "Main";
            }
            else
            {
                title = __group.name;
                description = __group.description;
            }

            currentGroup = __group;
            groups.Add(__group);
        }

        /// <summary>
        ///
        /// </summary>
        protected List<reportLink> items { get; set; } = new List<reportLink>();

        #region --- title ------- naziv kolekcije linkova

        /// <summary>
        /// naziv kolekcije linkova
        /// </summary>
        public string title { get; set; } = "";

        #endregion --- title ------- naziv kolekcije linkova

        #region --- description ------- opis kolekcije linkova

        /// <summary>
        /// opis kolekcije linkova
        /// </summary>
        public string description { get; set; } = "";

        #endregion --- description ------- opis kolekcije linkova

        #region --- priority ------- Prioritet

        /// <summary>
        /// Prioritet
        /// </summary>
        public int priority { get; set; } = 100;

        #endregion --- priority ------- Prioritet

        /// <summary>
        /// Adds the group.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public reportInPackageGroup AddGroup(string title, string description, int __priority = -1)
        {
            reportInPackageGroup group = new reportInPackageGroup();
            group.name = title;
            group.description = description;
            if (__priority == -1)
            {
                if (groups.Any())
                {
                    group.priority = groups.Last().priority + 5;
                }
            }
            else
            {
                group.priority = __priority;
            }
            groups.Add(group);

            currentGroup = group;
            return group;
        }

        /// <summary> </summary>
        public bool preventDuplicateURLs { get; protected set; } = false;

        /// <summary>
        /// Adds new link and associates it with the current group
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public reportLink AddLink(string title, string description, string url)
        {
            if (preventDuplicateURLs)
            {
                if (items.Any(x => x.linkPath == url))
                {
                    return items.First(x => x.linkPath == url);
                }
            }
            reportLink link = new reportLink(title, description, url);
            link.state = reportLinkState.pathIsUrl;
            link.group = currentGroup;
            link.priority = link.priority + items.Count;
            items.Add(link);
            return link;
        }

        public reportLink AddLinkToElement(string title, string description, string elementPath)
        {
            if (preventDuplicateURLs)
            {
                if (items.Any(x => x.linkPath == elementPath))
                {
                    return items.First(x => x.linkPath == elementPath);
                }
            }
            reportLink link = new reportLink(title, description, elementPath);
            link.state = reportLinkState.pathIsMetaModelPath;
            link.group = currentGroup;
            link.priority = link.priority + items.Count;
            items.Add(link);
            return link;
        }

        public reportLink AddLinkAsQuery(string __title, string __description, reportingRegistryQuery __query)
        {
            reportLink link = new reportLink(__title, __description, __query);
            link.state = reportLinkState.registryQuery;
            link.group = currentGroup;
            link.priority = link.priority + items.Count;

            items.Add(link);
            return link;
        }

        /// <summary>
        /// Linking to element instance, it will be resolved by <see cref="imbSCI.Reporting.meta.metaTools"/>
        /// </summary>
        /// <param name="toElement">To element.</param>
        /// <returns></returns>
        public reportLink AddLinkToElement(IMetaContentNested toElement, string titleOverride = "", string descriptionOverride = "", ILogBuilder logger = null)
        {
            if (toElement == null)
            {
                if (logger != null) logger.log("Can't add link to [null] element! reportLinkCollection n:[" + title + "] g:[" + currentGroup.name + "]");
                //if (reportingCoreManager.doVerboseLog)
                return null;
            }

            string _title = titleOverride.or(toElement.title);
            string _description = descriptionOverride.or(toElement.description);

            reportLink link = new reportLink(_title, _description, toElement.path);
            link.element = toElement;
            link.state = reportLinkState.elementInstance;
            link.group = currentGroup;
            link.priority = link.priority + items.Count;
            items.Add(link);
            return link;
        }

        public reportLink AddLink(reportLink link)
        {
            link.group = currentGroup;
            link.priority = link.priority + (link.priority - (100 + items.Count));
            items.Add(link);
            return link;
        }

        public void AddLinks(IEnumerable<reportLink> links)
        {
            foreach (var ln in links)
            {
                AddLink(ln.linkTitle, ln.linkDescription, ln.linkPath);
            }
        }

        public void sortup()
        {
            //            groups.Sort((x, y) => x.priority.CompareTo(y.priority));
            items.Sort((x, y) => x.effectivePriority.CompareTo(y.effectivePriority)); //.group.priority * y.priority));
        }

        /// <summary>
        /// The grup that is currently opened 2017c
        /// </summary>
        protected reportInPackageGroup currentGroup { get; set; }

        public reportInPackageGroup GetMainGroup()
        {
            return groups.First();
        }

        public reportInPackageGroup GetGroupOfFirstLink()
        {
            reportLink lnk = items.FirstOrDefault();
            if (lnk == null) return null;
            return lnk.group;
        }

        /// <summary>
        ///
        /// </summary>
        protected List<reportInPackageGroup> groups { get; set; } = new List<reportInPackageGroup>();

        public static string TemplateBootstrapNavbar
        {
            get
            {
                if (templateBootstrapNavbar.isNullOrEmpty())
                {
                    templateBootstrapNavbar = "<li><a href=\"{0}\">{1}</a></li>" + Environment.NewLine;
                }
                return templateBootstrapNavbar;
            }

            set
            {
                templateBootstrapNavbar = value;
            }
        }

        public static string TemplateBootstrapNavbarGroup
        {
            get
            {
                if (templateBootstrapNavbarGroup.isNullOrEmpty())
                {
                    templateBootstrapNavbarGroup = "<li role=\"separator\" class=\"divider\"></li>" + Environment.NewLine;
                    templateBootstrapNavbarGroup += "<li><a href=\"{0}\">{1}</a></li>" + Environment.NewLine;
                    templateBootstrapNavbarGroup += "<li role=\"separator\" class=\"divider\"></li>" + Environment.NewLine;
                }
                return templateBootstrapNavbarGroup;
            }

            set
            {
                templateBootstrapNavbarGroup = value;
            }
        }

        #region --- templateBootstrapAllowedExtensions ------- Extensions that are allowed

        private static List<string> _templateBootstrapAllowedExtensions;

        /// <summary>
        /// Extensions that are allowed
        /// </summary>
        public static List<string> templateBootstrapAllowedExtensions
        {
            get
            {
                if (_templateBootstrapAllowedExtensions == null)
                {
                    _templateBootstrapAllowedExtensions = new List<string>();
                    _templateBootstrapAllowedExtensions.Add(".html");
                    _templateBootstrapAllowedExtensions.Add(".htm");
                }
                return _templateBootstrapAllowedExtensions;
            }
        }

        #endregion --- templateBootstrapAllowedExtensions ------- Extensions that are allowed

        ///// <summary>
        ///// Creates new reportLink using supplied reportInPackage instance
        ///// </summary>
        ///// <param name="rp">reportInPackage instance to create link for</param>
        ///// <returns>Newly created and automatically added reportLink</returns>
        //public reportLink AddForReportInPackage(reportInPackage rp)
        //{
        //    reportLink link = new reportLink(rp.linkTitle, rp.linkDescription, rp.linkPath);

        //    link.group = rp.group;

        //    items.Add(link);

        //    return link;
        //}

        private static string templateBootstrapNavbar = "";
        private static string templateBootstrapNavbarGroup = "";

        public string makeHtmlInsert(string template = "", string groupTemplate = "", List<string> extensionsAllowed = null, bool convertToRelative = true)
        {
            if (extensionsAllowed == null) extensionsAllowed = templateBootstrapAllowedExtensions;
            if (template.isNullOrEmpty())
            {
                template = TemplateBootstrapNavbar;
            }

            if (groupTemplate.isNullOrEmpty())
            {
                groupTemplate = TemplateBootstrapNavbarGroup;
            }

            if (convertToRelative)
            {
                foreach (reportLink link in items)
                {
                    link.convertToRelative();
                }
            }

            string html = "";
            currentGroup = groups.First();

            foreach (reportLink link in items)
            {
                bool allow = true;

                if (Path.HasExtension(link.linkPath))
                {
                    string ext = Path.GetExtension(link.linkPath);
                    allow = extensionsAllowed.Contains(ext);
                }

                if (allow)
                {
                    // link.linkPath
                    if (link.group != currentGroup)
                    {
                        html += string.Format(groupTemplate, link.group.name);

                        currentGroup = link.group;
                    }
                    html += string.Format(template, link.linkPath, link.linkTitle);
                }
            }

            return html;
        }

        //public reportHtmlDocument makeHtml(reportHtmlDocument htmlReport = null)
        //{
        //    return makeHtml(htmlReport, false);
        //}

        ///// <summary>
        ///// Generates HTML for complete link collection
        ///// </summary>
        ///// <param name="htmlReport">HTML content report to render into</param>
        ///// <param name="excludeShema">Removes URL shema prefix from link target</param>
        ///// <returns>reportHtmlDocument that was set in input or creates new if htmlReport was null</returns>
        //public reportHtmlDocument makeHtml(reportHtmlDocument htmlReport, bool excludeShema)
        //{
        //    if (htmlReport == null) htmlReport = new reportHtmlDocument();

        //    if (items.Count == 0) return htmlReport;

        //    htmlReport.open("div", htmlClassForReport.linkBlock, title);

        //    if (!title.isNullOrEmpty())
        //    {
        //        htmlReport.open("h3");
        //        htmlReport.Append(title);
        //        htmlReport.close();
        //    }
        //    if (!description.isNullOrEmpty())
        //    {
        //        htmlReport.open("p");
        //        htmlReport.Append(description);
        //        htmlReport.close();
        //    }

        //    htmlReport.open("ul");

        //    foreach (reportLink link in items)
        //    {
        //        link.convertToRelative(htmlReport);

        //        htmlReport.open("il");

        //        string linkSrc = link.linkPath.removeUrlShema();
        //        linkSrc = linkSrc.removeStartsWith(htmlReport.paths.directory.FullName);
        //        linkSrc = imbStringPathTools.localizePath(htmlReport.paths.directory.FullName, linkSrc);
        //        linkSrc = linkSrc.removeStartsWith("\\");
        //        //linkSrc.removeStartsWith(htmlReport.paths.directory.FullName)
        //        string urlShema = urlShema = @"file:///";
        //        if (excludeShema)
        //        {
        //            urlShema = "";
        //        }
        //        htmlReport.AppendLink(linkSrc, link.linkTitle, htmlClassForReport.none, "", htmlTagName.label, urlShema);
        //        if (!link.linkDescription.isNullOrEmpty())
        //        {
        //            htmlReport.Append(link.linkDescription, htmlTagName.p);
        //        }
        //        htmlReport.close();
        //    }

        //    htmlReport.close();

        //    htmlReport.close();

        //    return htmlReport;
        //}

        public IEnumerator<reportLink> GetEnumerator()
        {
            return ((IEnumerable<reportLink>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<reportLink>)items).GetEnumerator();
        }
    }
}