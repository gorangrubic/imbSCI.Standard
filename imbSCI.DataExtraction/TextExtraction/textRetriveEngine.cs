// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textRetriveEngine.cs" company="imbVeles" >
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
// Project: imbNLP.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataExtraction.TextExtraction
{
    #region imbVELES USING

    //using imbNLP.Core.contentStructure.tokenizator;
    //using imbNLP.Transliteration;
    using imbSCI.Core.reporting;
    using imbSCI.DataComplex.extensions.text;
    using imbSCI.DataExtraction.Xml;
    using System;
    using System.Text;
    using System.Xml;
    using System.Xml.XPath;

    // using imbCore.xml.html;

    #endregion imbVELES USING

    /// <summary>
    /// Mehanizam za preuzimanje cistog teksta iz HTML-a
    /// </summary>
    public static class textRetriveEngine
    {
        public static string deploy(string value, textRetrive_structure mode, textRetriveSetup settings)
        {
            StringBuilder output = new StringBuilder();
            value = value.Trim();
            if (string.IsNullOrEmpty(value)) return "";
            switch (mode)
            {
                case textRetrive_structure.ignore:
                    break;

                case textRetrive_structure.newLine:
                    output.Append(Environment.NewLine);
                    output.Append(value);
                    output.Append(Environment.NewLine);
                    output.Append(Environment.NewLine);
                    break;

                case textRetrive_structure.normal:
                    output.Append(value);
                    output.Append(Environment.NewLine);
                    break;

                case textRetrive_structure.spaceInline:
                    output.Append(value + settings.inlineSpace);
                    break;
            }
            return output.ToString();
        }

        /// <summary>
        /// Proverava da li je prosledjeni node u saglasju sa podesavanjima
        /// </summary>
        /// <param name="source"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static bool checkNode(this XPathNavigator source, textRetriveSetup settings)
        {
            switch (source.NodeType)
            {
                case XPathNodeType.Element:
                    string nn = source.Name.ToLower();
                    switch (nn)
                    {
                        case "script":
                            return settings.doExportScripts;
                            break;

                        case "title":
                            return settings.doExportTitle;
                            break;

                        case "style":
                            return settings.doExportStyles;

                            break;

                        default:
                            return true;
                            break;
                    }
                    break;

                case XPathNodeType.Comment:
                    return settings.doExportComments;
                    break;

                case XPathNodeType.Whitespace:
                case XPathNodeType.SignificantWhitespace:
                    return false;
                    break;
            }
            return false;
        }

        public static textRetriveSetup checkSettings(this textRetriveSetup settings)
        {
            if (settings == null)
            {
                var trs = new textRetriveSetup();

                //var tRecord = resources.getFirstOfType<modelSpiderTestRecord>
                // ILogBuilder pRecordLog = resources.getFirstOfType<ILogBuilder>(false, false, false);
                // crawledPage cpage = resources.getOfType<crawledPage>();

                trs.doExportScripts = false;
                trs.doExportComments = false;
                trs.doExportStyles = false;
                trs.doRetrieveChildren = false;
                trs.doHtmlCleanUp = true;
                trs.doCyrToLatTransliteration = true;
                return trs;
            }
            return settings;
        }

        public static string retriveText(this IXPathNavigable source, textRetriveSetup settings = null)
        {
            return source.CreateNavigator().retriveText(settings);
        }

        /// <summary>
        /// 2014c> novi mehanizam za tekstualnu reprezentaciju ucitanog dokumenta
        /// </summary>
        /// <param name="source"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string retriveText(this XPathNavigator source, textRetriveSetup settings = null)
        {
            StringBuilder output = new StringBuilder();
            if (source == null)
            {
                return "";
            }
            settings = settings.checkSettings();

            XPathNodeIterator itr = source.SelectDescendants(XPathNodeType.Text, true);
            while (itr.MoveNext())
            {
                switch (itr.Current.NodeType)
                {
                    case XPathNodeType.Text:
                        string inner = itr.Current.Value;
                        if (!string.IsNullOrEmpty(inner))
                        {
                            var subNav = itr.Current.CreateNavigator();

                            if (subNav.MoveToParent())
                            {
                                if (subNav.checkNode(settings))
                                    output.AppendLine(deploySpacing(inner, subNav, settings));
                            }
                            else
                            {
                                if (subNav.checkNode(settings)) output.AppendLine(inner);
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            string out2 = output.ToString();

            if (settings.doCompressNewLines)
            {
                string nnnl = Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                string nnl = Environment.NewLine + Environment.NewLine + Environment.NewLine;
                //out2 = tokenization.blankLineSelector.Replace(out2, nnl);
                while (out2.Contains(nnnl))
                {
                    out2 = out2.Replace(nnnl, nnl);
                }
            }
            return out2;
        }

        /// <summary>
        /// Primenjuje podesavanja spejsinga - simulacija HTML strukture
        /// </summary>
        /// <param name="insert"></param>
        /// <param name="parentTag"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static string deploySpacing(string insert, XPathNavigator parentTag, textRetriveSetup settings)
        {
            string tag = parentTag.Name.ToLower();
            if (htmlDefinitions.HTMLTags_blockStructureTags.Contains(tag))
                return deploy(insert, settings.spanExtractMode, settings);
            if (htmlDefinitions.HTMLTags_headingTags.Contains(tag))
                return deploy(insert, settings.headingExtractMode, settings);
            if (htmlDefinitions.HTMLTags_tableItemTags.Contains(tag))
                return deploy(insert, settings.tdExtractMode, settings);
            return insert;
        }

        /// <summary>
        /// 2013A> GLAVNI mehaniza za preuzimanje TXT reprezentacije XmlNoda *podr�ani i multilevel
        /// </summary>
        /// <param name="source"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string retriveText(this XmlNode source, textRetriveSetup settings)
        {
            if (source == null)
            {
                return "";
            }

            StringBuilder output = new StringBuilder();
            string tmp = "";
            string tag = source.Name.ToLower();
            string tmpVal = "";
            switch (tag)
            {
                case "meta":
                    switch (settings.metaExtractMode)
                    {
                        case textRetrive_meta.full:

                            tmp = imbXmlCommonTools.getNodeDataSourceMulti(source, "@property =\"", true);
                            tmp += imbXmlCommonTools.getNodeDataSourceMulti(source, "@content @http-equiv", false);
                            tmp += imbXmlCommonTools.getNodeDataSourceMulti(source, "\" ", true) + Environment.NewLine;
                            output.Append(deploy(tmp, settings.metaSpaceExtractMode, settings));
                            break;

                        default:
                        case textRetrive_meta.onlyDescriptionAndKeywords:
                            tmpVal = imbXmlCommonTools.getNodeDataSourceMulti(source, "@property", false).ToLower();
                            switch (tmpVal)
                            {
                                case "keywords":
                                case "desc":
                                case "description":
                                    output.Append(
                                        deploy(
                                            imbXmlCommonTools.getNodeDataSourceMulti(source, "@property=@content", false),
                                            settings.metaSpaceExtractMode, settings));
                                    break;
                            }
                            break;

                        case textRetrive_meta.skip:
                            break;
                    }
                    break;

                case "title":
                    if (settings.doExportTitle)
                    {
                        output.Append(imbXmlCommonTools.getNodeDataSourceMulti(source, "innertext", false));
                    }
                    break;

                default:
                case "p":
                    output.Append(deploy(imbXmlCommonTools.getNodeDataSourceMulti(source, "innertext", false),
                                         settings.pExtractMode, settings));
                    break;

                case "h":
                case "h1":
                case "h2":
                case "h3":
                case "h4":
                case "h5":
                case "h6":
                    output.Append(deploy(imbXmlCommonTools.getNodeDataSourceMulti(source, "innertext", false),
                                         settings.headingExtractMode, settings));
                    break;

                case "td":
                    output.Append(deploy(imbXmlCommonTools.getNodeDataSourceMulti(source, "innertext", false),
                                         settings.tdExtractMode, settings));
                    break;

                case "tr":
                    output.Append(deploy(imbXmlCommonTools.getNodeDataSourceMulti(source, "innertext", false),
                                         settings.trExtractMode, settings));
                    break;

                case "div":
                    output.Append(deploy(imbXmlCommonTools.getNodeDataSourceMulti(source, "innertext", false),
                                         settings.divExtractMode, settings));
                    break;

                case "span":
                    output.Append(deploy(imbXmlCommonTools.getNodeDataSourceMulti(source, "innertext", false),
                                         settings.spanExtractMode, settings));
                    break;

                case "li":
                    output.Append(deploy(imbXmlCommonTools.getNodeDataSourceMulti(source, "innertext", false),
                                        settings.pExtractMode, settings));
                    break;

                case "a":
                    switch (settings.linksExtractMode)
                    {
                        case textRetrive_links.skip:
                            break;

                        default:
                        case textRetrive_links.title:
                            output.Append(deploy(imbXmlCommonTools.getNodeDataSourceMulti(source, "innertext", false),
                                                 settings.aExtractMode, settings));
                            break;

                        case textRetrive_links.titleAndUrl:
                            tmp = imbXmlCommonTools.getNodeDataSourceMulti(source, "innertext", false) +
                                  Environment.NewLine;
                            tmp += imbXmlCommonTools.getNodeDataSourceMulti(source, "@href", false) +
                                   Environment.NewLine;
                            output.Append(deploy(tmp, settings.aExtractMode, settings));
                            break;
                    }
                    break;

                case "script":
                    if (settings.doExportScripts)
                    {
                        output.Append(Environment.NewLine);
                        output.Append(imbXmlCommonTools.getNodeDataSourceMulti(source, "SCRIPT:@language @type @src",
                                                                               true));
                        output.Append(imbXmlCommonTools.getNodeDataSourceMulti(source, "innertext", false) +
                                      Environment.NewLine);
                        output.Append(Environment.NewLine);
                    }

                    break;

                case "#comment":
                    output.Append(deploy(imbXmlCommonTools.getNodeDataSourceMulti(source, "innertext", false),
                                         settings.commentExtractMode, settings));
                    break;
            }

            if (settings.doRetrieveChildren)
            {
                if (source.HasChildNodes)
                {
                    foreach (XmlNode ch in source.ChildNodes)
                    {
                        output.Append(retriveText(ch, settings));
                    }
                }
            }

            string str = output.ToString().Trim();
            if (settings.doHtmlCleanUp) str = str.htmlContentProcess();
            //if (settings.doCyrToLatTransliteration) str = str.transliterate();

            // if (settings.insertNewLine) output.Append(Environment.NewLine + Environment.NewLine);

            return output.ToString();
        }
    }
}