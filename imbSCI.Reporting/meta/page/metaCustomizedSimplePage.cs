// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaCustomizedSimplePage.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.page
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.render;
    using imbSCI.Data;
    using imbSCI.Data.enums.appends;
    using imbSCI.Data.enums.fields;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.interfaces;
    using imbSCI.Reporting.meta.blocks;
    using imbSCI.Reporting.meta.document;
    using imbSCI.Reporting.meta.documentSet;
    using imbSCI.Reporting.script;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Page with customized content blocks
    /// </summary>
    /// <seealso cref="metaPage" />
    /// <seealso cref="imbSCI.Reporting.interfaces.IMetaComposeAndConstruct" />
    public class metaCustomizedSimplePage : metaPage, IMetaComposeAndConstruct
    {
        public metaCustomizedSimplePage(string __title, string __description)
        {
            pageTitle = __title;
            pageDescription = __description;
            basicBlocksFlags = metaPageCommonBlockFlags.none;
            name = __title.getFilename();

            header.title = "{{{" + templateFieldBasic.page_title + "}}}";
            header.description = "".t(templateFieldBasic.page_desc);

            footer.bottomLine = "".t(templateFieldBasic.meta_copyright) + " - ".t(templateFieldBasic.meta_organization); // {{{meta_copyright}}} - {{{meta_author}}}";
            mainBlock = AddScriptBlock();
        }

        private metaDocScriptBlock _mainBlock;

        /// <summary> </summary>
        public metaDocScriptBlock mainBlock
        {
            get
            {
                return _mainBlock;
            }
            protected set
            {
                _mainBlock = value;
                OnPropertyChanged("mainBlock");
            }
        }

        public metaCodeBlock AddCodeBlock(string blockTitle, string blockDescription, string blockContent)
        {
            var contentLines = blockContent.breakLines();
            metaCodeBlock output = new metaCodeBlock(blockTitle, blockDescription, contentLines);
            blocks.Add(output, this);
            output.parent = this;
            return output;
        }

        /// <summary>
        /// Imports external markdown file into the page
        /// </summary>
        /// <remarks>
        /// Filepath starts with application running directory
        /// </remarks>
        /// <example>
        /// <code>
        /// AddExternalContent("reportInclude\\testdocs\\reportintro.md", "", "").priority = 20;
        /// </code>
        /// </example>
        /// <param name="filename">The filename.</param>
        /// <param name="title">The title.</param>
        /// <param name="undertitle">The undertitle.</param>
        /// <returns></returns>
        public metaExternalBlock AddExternalContent(string filename, string title, string undertitle)
        {
            metaExternalBlock output = new metaExternalBlock(filename, title, undertitle);
            output.parent = this;
            blocks.Add(output);
            return output;
        }

        public metaExternalBlock AddExternalContent(ITextRender textBuilder, string title, string undertitle)
        {
            metaExternalBlock output = new metaExternalBlock(textBuilder, title, undertitle);
            output.parent = this;
            blocks.Add(output);
            return output;
        }

        /// <summary>
        /// Adds the attachment.
        /// </summary>
        /// <param name="__includeFilePath">The include file path.</param>
        /// <param name="__filename">The filename.</param>
        /// <param name="__caption">The caption.</param>
        /// <param name="__description">The description.</param>
        /// <param name="__templateNeedle">The template needle.</param>
        /// <param name="__isDataTemplate">if set to <c>true</c> [is data template].</param>
        /// <returns></returns>
        public metaAttachmentBlock AddAttachment(string __includeFilePath, string __filename, string __caption = "", string __description = "", string __templateNeedle = "", bool __isDataTemplate = false)
        {
            metaAttachmentBlock output = new metaAttachmentBlock(__includeFilePath, __filename, __caption, __description, __templateNeedle, __isDataTemplate);

            output.parent = this;
            blocks.Add(output);
            return output;
        }

        //public metaDiagramBlock AddDiagram(diagramModel model, diagramOutputEngineEnum engine)
        //{
        //    metaDiagramBlock output = new metaDiagramBlock(model, engine);
        //    output.parent = this;
        //    blocks.Add(output);
        //    return output;
        //}

        public metaDataTable AddDataTable(DataTable table, string title, string description)
        {
            metaDataTable output = new metaDataTable(table);
            output.description = description;
            output.title = title;
            blocks.Add(output);
            output.parent = this;
            return output;
        }

        public metaDocScriptBlock AddPropertyCollection(PropertyCollection properties, string title, string description)
        {
            metaDocScriptBlock output = new metaDocScriptBlock();

            output.instructions.open("data", title, description);

            foreach (object key in properties.Keys)
            {
                object val = properties[key];

                string str_key = key.toStringSafe();
                string str_value = properties[key].toStringSafe();

                if (str_value == "(..)")
                {
                    if (val != null)
                    {
                        PropertyCollection pc = val.buildPropertyCollection<PropertyCollection>(true, false, "obj");
                        if (pc.Keys.Count > 0)
                        {
                            output.instructions.open("subdata", str_key + " : " + val.GetType().Name, "Object properties");

                            foreach (object objkey in pc.Keys)
                            {
                                string obj_key = objkey.toStringSafe();
                                string obj_value = pc[key].toStringSafe();
                                output.instructions.AppendPair(obj_key, str_value, " = ", false);
                            }

                            output.instructions.close();
                        }
                    }
                }

                output.instructions.AppendPair(str_key, str_value, " = ", false);
            }

            output.instructions.close();

            blocks.Add(output);
            output.parent = this;
            return output;
        }

        /// <summary>
        /// Creates datatemplate for all Enum type members
        /// </summary>
        /// <param name="enumType">Type of an Enum to use as shemata</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public metaDocScriptBlock AddDataTemplate(Type enumType, string title = "", string description = "")
        {
            metaDocScriptBlock output = new metaDocScriptBlock();

            if (imbSciStringExtensions.isNullOrEmpty(title))
            {
                title = enumType.Name.imbTitleCamelOperation(true);
            }

            if (imbSciStringExtensions.isNullOrEmpty(description))
            {
                description = "Showing all data mapped to enumeration (" + enumType.Name + ") members";//  enumType.Name
            }

            output.instructions.open("data", title, description);

            var keys = enumType.GetEnumValues();

            foreach (Enum key in keys)
            {
                string str_key = key.toStringSafe();
                string str_value = "{{{" + key + "}}}";
                output.instructions.AppendPair(str_key, str_value, " = ", false);
            }

            output.instructions.close();

            blocks.Add(output);
            output.parent = this;
            return output;
        }

        /// <summary>
        /// Adds and returns customizable subscript block
        /// </summary>
        /// <returns></returns>
        public metaDocScriptBlock AddScriptBlock()
        {
            metaDocScriptBlock output = new metaDocScriptBlock();

            blocks.Add(output);
            output.parent = this;

            return output;
        }

        /// <summary>
        /// Adds another block
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="lines">The lines.</param>
        /// <returns></returns>
        public metaDocScriptBlock AddBlock(string title, string description, IEnumerable<string> lines)
        {
            metaDocScriptBlock output = new metaDocScriptBlock();

            output.instructions.open("block", title, description);

            foreach (string ln in lines)
            {
                output.instructions.AppendLine(ln);
            }

            output.instructions.close();

            blocks.Add(output);
            output.parent = this;

            return output;
        }

        /// <summary>
        /// Adds the index skip this.
        /// </summary>
        /// <param name="indexSource">The index source.</param>
        /// <param name="skipThis">The skip this.</param>
        /// <param name="output">The output.</param>
        /// <returns></returns>
        public metaDocScriptBlock AddIndexSkipThis(metaDocument indexSource, metaPage skipThis, string customTitle = "", metaDocScriptBlock output = null)
        {
            if (output == null) output = new metaDocScriptBlock();

            if (imbSciStringExtensions.isNullOrEmpty(customTitle))
            {
                customTitle = "Pages";
            }

            output.instructions.open("menu", customTitle, "");

            foreach (metaPage indexPage in indexSource.pages)
            {
                if (indexPage != null)
                {
                    if (indexPage != skipThis)
                    {
                        string filepath = "" + indexPage.name + ".html"; //.t(templateFieldBasic.path_ext);

                        output.instructions.open("page", indexPage.pageTitle, indexPage.pageDescription);

                        output.instructions.c_link(indexPage.pageTitle, filepath, indexPage.name, indexPage.pageDescription, appendLinkType.link);

                        output.instructions.close();
                    }
                }
            }

            output.instructions.close();

            if (!blocks.Contains(output)) blocks.Add(output);

            output.parent = this;

            return output;
        }

        public metaDocScriptBlock AddIndex(metaDocument indexSource, metaDocScriptBlock output = null)
        {
            if (output == null) output = new metaDocScriptBlock();

            output.instructions.open("menu", "Document", "");

            foreach (metaPage indexPage in indexSource.pages)
            {
                string filepath = "" + indexPage.name + ".html"; //.t(templateFieldBasic.path_ext);
                output.instructions.open("page", indexPage.pageTitle, indexPage.pageDescription);

                output.instructions.c_link(indexPage.pageTitle, filepath, indexPage.name, indexPage.pageDescription, appendLinkType.link);

                output.instructions.close();
            }

            output.instructions.close();

            if (!blocks.Contains(output)) blocks.Add(output);

            output.parent = this;

            return output;
        }

        public metaDocScriptBlock AddFullIndex(metaDocumentSet indexSource, metaDocScriptBlock output = null)
        {
            if (output == null) output = new metaDocScriptBlock();

            output.instructions.open("menu", "Content", "");

            //foreach (metaPage indexPage in indexSource.servicePages)
            //{
            //    String filepath = "{{{document_relpath}}}/" + indexPage.name + ".html"; //.t(templateFieldBasic.path_ext);
            //    output.instructions.open("page", indexPage.pageTitle, indexPage.pageDescription);

            //    output.instructions.c_link(indexPage.pageTitle, filepath, indexPage.name, indexPage.pageDescription, appendLinkType.link);

            //    output.instructions.close();

            //}

            AddIndex(indexSource, output);

            foreach (metaDocumentSet indexDoc in indexSource.documentSets)
            {
                AddIndex(indexDoc, output);
            }

            output.instructions.close();

            if (!blocks.Contains(output)) blocks.Add(output);

            output.parent = this;

            return output;
        }

        public metaDocScriptBlock AddIndex(metaDocumentSet indexSource, metaDocScriptBlock output = null)
        {
            if (output == null) output = new metaDocScriptBlock();

            output.instructions.open("menu", "Documents", "");

            foreach (metaDocument indexDoc in indexSource.documents)
            {
                output.instructions.c_link(indexDoc.documentTitle, indexDoc.name.add("index.html", "/"), indexDoc.name, indexDoc.documentDescription, appendLinkType.link);

                foreach (metaPage indexPage in indexDoc.pages)
                {
                    string filepath = "{{{document_relpath}}}/" + indexPage.name + ".html"; //.t(templateFieldBasic.path_ext);
                    output.instructions.open("page", indexPage.pageTitle, indexPage.pageDescription);

                    output.instructions.c_link(indexPage.pageTitle, filepath, indexPage.name, indexPage.pageDescription, appendLinkType.link);

                    output.instructions.close();
                }
            }

            output.instructions.close();

            if (!blocks.Contains(output)) blocks.Add(output);

            output.parent = this;

            return output;
        }

        public metaDocScriptBlock AddNavigation(IMetaContentNested navParent)
        {
            metaDocScriptBlock output = new metaDocScriptBlock();

            output.instructions.open("nav", "Navigation", "");

            output.instructions.c_link("Back", "../index.html", "home", "Back to main index page", appendLinkType.link);
            output.instructions.c_link("Index", "index.html", "home", "Back to main index page", appendLinkType.link);

            output.instructions.close();

            //foreach (metaPage indexPage in indexSource.pages)
            //{
            //    String filepath = indexPage.name + ".html"; //.t(templateFieldBasic.path_ext);
            //    output.instructions.c_link(indexPage.pageTitle, filepath, indexPage.name, indexPage.pageDescription, appendLinkType.link);
            //}

            blocks.Add(output);
            output.parent = this;

            return output;
        }

        //public MetaContainerNestedBase AddCodeBlock(String blockTitle, String blockDescription, IEnumerable<String> blockContent)
        //{
        //    var contentLines = blockContent;
        //    metaCodeBlock output = new metaCodeBlock(blockTitle, blockDescription, contentLines);
        //    blocks.Add(output);
        //    return output;
        //}

        ///// <summary>
        ///// Construct will take all IMetaContentNested and PropertyCollections
        ///// </summary>
        ///// <param name="resources">The resources.</param>
        //public override void construct(params Object[] resources)
        //{
        //    List<Object> reslist = resources.getFlatList<Object>();

        //    foreach (Object res in reslist)
        //    {
        //        if (res is IMetaContentNested)
        //        {
        //            blocks.Add(res as IMetaContentNested, this);
        //        } else if (res is DataTable)
        //        {
        //            DataTable dt = res as DataTable;
        //            metaDataTable mdt = new metaDataTable();
        //            blocks.Add(mdt, this);
        //            mdt.construct(res as DataTable);

        //        } else if (res is PropertyCollection)
        //        {
        //            metaVariablePairs pairs = new metaVariablePairs();
        //            blocks.Add(pairs, this);
        //            pairs.construct(reslist);
        //        } else if (res is IList)
        //        {
        //        }
        //    }

        //  //  List<MetaContainerNestedBase> customContent = reslist.getAllOfType<IMetaContentNested>(false);

        //}

        //public override void construct(params object[] resources)
        //{
        //    base.construct(resources);

        //}

        //public virtual PropertyCollectionDictionary collect(PropertyCollectionDictionary data = null)
        //{
        //    if (data == null) data = new PropertyCollectionDictionary();

        //    PropertyCollection pageData = AppendDataFields();

        //    data.Add(path, pageData);

        //    deliveryInstance.deliveryInstance del = context as deliveryInstance.deliveryInstance;
        //    del.collectOperationStart(context, this, data);

        //    return data;
        //}

        public override docScript compose(docScript script)
        {
            script = this.checkScript(script);

            script.x_scopeIn(this);

            script = this.subCompose(script);

            script.x_scopeOut(this);

            return script;
        }
    }
}