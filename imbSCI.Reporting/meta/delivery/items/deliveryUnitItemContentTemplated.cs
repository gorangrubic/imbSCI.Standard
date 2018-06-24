// --------------------------------------------------------------------------------------------------------------------
// <copyright file="deliveryUnitItemContentTemplated.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.delivery.items
{
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.files;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.extensions;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.fields;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.exceptions;
    using imbSCI.Reporting.links;
    using imbSCI.Reporting.meta.page;
    using imbSCI.Reporting.script;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="imbSCI.Reporting.meta.delivery.deliveryUnitItem" />
    public class deliveryUnitItemContentTemplated : deliveryUnitItem, IDeliveryUnitItemFromFileSource, IDeliveryUnitItemWithTemplate
    {
        /// <summary>
        ///
        /// </summary>
        public deliveryUnitItemSimpleRenderOutput sourceRender { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public string filenameSufix { get; set; } = "_tmp";

        /// <summary>
        ///
        /// </summary>
        public reportOutputFormatName format { get; set; }

        public List<reportElementLevel> levels => sourceRender.levels;

        /// <summary>
        /// Saves the output - for external use
        /// </summary>
        /// <param name="mainContent">Content of the main.</param>
        /// <param name="data">The data.</param>
        /// <param name="filepath">The filepath.</param>
        public FileInfo saveOutput(IRenderExecutionContext context, string mainContent, PropertyCollection data, string filepath, bool isDataTemplate = true)
        {
            string filepath_attachment_field = "attachment_" + Path.GetFileName(filepath);

            PropertyCollection content_blocks = new PropertyCollection();
            content_blocks.add(templateFieldSubcontent.main, mainContent);

            string codeOutput = template;

            codeOutput = codeOutput.applyToContent(false, content_blocks);// openBase.openFileToString(tempfile.FullName, true, false);

            if (isDataTemplate)
            {
                codeOutput = codeOutput.applyToContent(false, data);
            }

            codeOutput = codeOutput.CompileLinksInTemplate(context as deliveryInstance, format, levelsOfNewDirectory);

            FileInfo fi = codeOutput.saveStringToFile(filepath, getWritableFileMode.overwrite, Encoding.UTF8);

            data.Add(filepath_attachment_field, filepath);
            return fi;
        }

        public void createNewFile(IRenderExecutionContext context, IMetaContentNested oldScope)
        {
            ITextRender render = sourceRender.outputRender;
            string ext = format.getDefaultExtension();

            string filename = (oldScope.name + filenameSufix).getFilename(ext);

            string filepath = context.directoryScope.FullName.add(filename, "\\");

            // String folderPath = context.data.getProperString(oldScope.path, templateFieldBasic.path_folder, templateFieldBasic.document_path, templateFieldBasic.documentset_path);

            PropertyCollection content_blocks = render.getContentBlocks(true, format);
            IMetaContentNested sc = (IMetaContentNested)oldScope;

            string codeOutput = template;

            try
            {
                IConverterRender cr = (IConverterRender)render;

                reportLinkCollectionSet menu = new reportLinkCollectionSet();
                reportLinkToolbar toolbar = new reportLinkToolbar();
                // sc.GetLinkCollection((deliveryInstance)context, format, sourceRender.levelsOfNewDirectory, false);

                // <---------- MENU ----------------------------
                if (oldScope is metaPage)
                {
                    metaPage oldScope_metaPage = (metaPage)oldScope;
                    menu.Add("Document set", oldScope_metaPage.menu_documentSetMenu.CompileLinkCollection(context as deliveryInstance, format, levelsOfNewDirectory));
                    menu.Add("Document", oldScope_metaPage.menu_documentmenu.CompileLinkCollection(context as deliveryInstance, format, levelsOfNewDirectory));
                    menu.Add("Page", oldScope_metaPage.menu_pagemenu.CompileLinkCollection(context as deliveryInstance, format, levelsOfNewDirectory));

                    menu.Add("Root", oldScope_metaPage.menu_rootmenu.CompileLinkCollection(context as deliveryInstance, format, levelsOfNewDirectory));

                    toolbar = oldScope_metaPage.toolbar_pagetools;
                }

                string menuRender = menu.RenderSetAsDropDowns(cr.converter);
                content_blocks.add(templateFieldSubcontent.html_mainnav, menuRender, false);

                // <---------------- TOOL BAR
                if (toolbar != null)
                {
                    if (toolbar.Any())
                    {
                        string toolbarRender = toolbar.CompileLinkCollection(context as deliveryInstance, format, levelsOfNewDirectory).RenderAsToolbar(cr.converter);
                        content_blocks.add(templateFieldSubcontent.html_toolbar, toolbarRender, false);
                    }
                }

                // <---------------- LINKS
                // codeOutput = codeOutput.CompileLinksInTemplate(context as deliveryInstance, format, levelsOfNewDirectory);
            }
            catch (Exception ex)
            {
                throw new aceReportException("Auto-menu creation : " + ex.Message + " Automenu creation for " + oldScope.path);
            }

            if (context is deliveryInstance)
            {
                deliveryInstance contextDeliveryInstance = (deliveryInstance)context;
                contextDeliveryInstance.unit.blockBuilder.BuildDynamicNavigationTemplates(contextDeliveryInstance, content_blocks);
            }

            codeOutput = codeOutput.applyToContent(false, content_blocks);// openBase.openFileToString(tempfile.FullName, true, false);

            codeOutput = codeOutput.applyToContent(false, context.data);

            codeOutput = codeOutput.CompileLinksInTemplate(context as deliveryInstance, format, levelsOfNewDirectory);

            codeOutput = codeOutput.Replace("..//", "../");
            codeOutput = codeOutput.Replace(" href=\"/", " href=\"");

            //var savedfile = codeOutput.saveStringToFile(filepath, imbSCI.Cores.enums.getWritableFileMode.overwrite, Encoding.UTF8);

            context.saveFileOutput(codeOutput, filepath, getFolderPathForLinkRegistry(context), description);

            //IDocScriptExecutionContext docContext = context as IDocScriptExecutionContext;

            //docContext.linkRegistry[context.directoryScope.FullName].AddLink(filename, "", filepath);
        }

        /// <summary>
        ///
        /// </summary>
        public List<reportElementLevel> levelsOfNewDirectory { get; protected set; } = new List<reportElementLevel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="deliveryUnitItemContentTemplated" /> class.
        /// </summary>
        /// <param name="__sourcepath">The sourcepath.</param>
        /// <param name="__sourceRender">The source render.</param>
        /// <param name="__format">The format.</param>
        public deliveryUnitItemContentTemplated(string __sourcepath, deliveryUnitItemSimpleRenderOutput __sourceRender, reportOutputFormatName __format, IEnumerable<reportElementLevel> __levels) : base(deliveryUnitItemType.contentTemplate)
        {
            location = deliveryUnitItemLocationBase.localResource;
            flags = deliveryUnitItemFlags.useTemplate;
            //useTemplate = true;

            sourceRender = __sourceRender;
            //format.Add(__formats.getFlatList<reportOutputFormatName>());
            levelsOfNewDirectory.AddRange(__levels);

            format = __format;

            sourcepath.setup("".t(templateFieldDeliveryUnit.del_themepath).add(__sourcepath, "\\"));

            string opath2 = "".t(templateFieldBasic.path_here);

            outputpath.setup(opath2);
        }

        //public void prepareOperation(IRenderExecutionContext context)
        //{
        //    String _pt = sourcepath.toPath("", context.data);
        //    template = openBase.openFileToString(_pt, true, false);
        //}

        public PropertyCollectionDictionary collectOperationStart(IRenderExecutionContext context, IMetaContentNested composer, PropertyCollectionDictionary dict)
        {
            var level = composer.elementLevel;

            if (level == sourceRender.levelOfNewFile)
            {
                string filename = dict[composer.path].getProperString("", templateFieldBasic.path_output);

                string parentPath = composer.path;
                if (composer.parent != null) parentPath = composer.parent.path;

                string folderPath = context.data.getProperString(parentPath, templateFieldBasic.path_folder, templateFieldBasic.document_path, templateFieldBasic.documentset_path);

                // filename = context.directoryScope.FullName.add(filename, "\\");
                filepath fp = new filepath(filename);
                //String templateFilename = fp.filename.add(fp.extension, "."); //.toPathWithExtension("", format.getDefaultExtension());
                string templateFilename = fp.toPathWithExtension("", format.getDefaultExtension());

                // context.regFileOutput(templateFilename, folderPath, description);
                // dict[composer.path].Add(templateFieldBasic.path_output, filename);
            }
            else
            {
                // dict[composer.path].Add(templateFieldBasic.path_file, "");
            }

            //throw new NotImplementedException();
            return dict;
        }

        public void scopeInOperation(IRenderExecutionContext context, IMetaContentNested newScope)
        {
            // sourceRender.scopeInDirectoryCheck(context, newScope.elementLevel, newScope);

            //setRelPath(context);
        }

        public void executeScriptInstruction(IRenderExecutionContext context, docScriptInstructionCompiled instruction)
        {
        }

        /// <summary>
        /// Scopes the out operation.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="oldScope">The old scope.</param>
        public void scopeOutOperation(IRenderExecutionContext context, IMetaContentNested oldScope)
        {
            setRelPath(context);

            var level = oldScope.elementLevel;
            if (sourceRender.levelOfNewFile == level)
            {
                createNewFile(context, oldScope);
            }
            else if (sourceRender.levelOfNewPage == level)
            {
            }
            // sourceRender.scopeOutDirectoryCheck(context, level);
        }

        public docScript composeOperationStart(IRenderExecutionContext context, IMetaContentNested composer, docScript script)
        {
            return script;
        }

        public docScript composeOperationEnd(IRenderExecutionContext context, IMetaContentNested composer, docScript script)
        {
            return script;
        }
    }
}