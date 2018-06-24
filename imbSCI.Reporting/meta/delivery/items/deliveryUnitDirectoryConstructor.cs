// --------------------------------------------------------------------------------------------------------------------
// <copyright file="deliveryUnitDirectoryConstructor.cs" company="imbVeles" >
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
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.extensions;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.fields;
    using imbSCI.Reporting.script;
    using System;
    using System.Data;
    using System.IO;
    using System.Text;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="imbSCI.Reporting.meta.delivery.deliveryUnitItem" />
    /// <seealso cref="imbSCI.Reporting.meta.delivery.items.IDeliveryUnitItemOutputRender" />
    public sealed class deliveryUnitDirectoryConstructor : deliveryUnitItem, IDeliveryUnitItemOutputRender
    {
        public metaContentCriteriaTriggerCollection criteria
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public deliveryUnitDirectoryConstructor(params reportElementLevel[] __levels) : base(deliveryUnitItemType.none)
        {
            location = deliveryUnitItemLocationBase.localResource;
            flags = deliveryUnitItemFlags.none;

            foreach (reportElementLevel lev in __levels)
            {
                levels.Add(lev);
            }

            //levels.AddRange(__levels.getFlatList<reportElementLevel>());
        }

        public void prepareOperation(IRenderExecutionContext context)
        {
            // throw new NotImplementedException();
        }

        public void scopeInOperation(IRenderExecutionContext context, IMetaContentNested newScope)
        {
            context.directoryScope = Directory.CreateDirectory(context.GetDirectoryPath(newScope, levels));

            setRelPath(context);

            if (context is deliveryInstance)
            {
                deliveryInstance contextDeliveryInstance = (deliveryInstance)context;
                contextDeliveryInstance.unit.blockBuilder.BuildDynamicNavigationTemplates(contextDeliveryInstance, context.data);
            }
        }

        //public String makeFolderSignature()

        public void scopeOutOperation(IRenderExecutionContext context, IMetaContentNested oldScope)
        {
            if (context.directoryScope.FullName == context.directoryRoot.FullName)
            {
            }
            else
            {
                builderForMarkdown dirReadMe = new builderForMarkdown();

                dirReadMe.AppendHorizontalLine();

                dirReadMe.AppendHeading("Directory for [" + oldScope.name + "]", 3);

                dirReadMe.AppendLine("Report: {{{test_caption}}},  {{{sys_time}}}, {{{sys_date}}}");

                dirReadMe.AppendLine("> Open 'index.html' for report content");

                dirReadMe.AppendHorizontalLine();

                dirReadMe.AppendHeading("Report element description", 2);

                dirReadMe.AppendPair("Element class type", oldScope.GetType().Name, true, " \t\t = \t");
                dirReadMe.AppendPair("Element logical level", oldScope.elementLevel.ToString(), true, " \t\t = \t");
                dirReadMe.AppendPair("Element logical path", oldScope.path, true, " \t\t = \t"); //.elementLevel.ToString());
                dirReadMe.AppendPair("Element isRoot", oldScope.isThisRoot, true, " \t\t = \t"); //.elementLevel.ToString());

                dirReadMe.AppendHorizontalLine();

                dirReadMe.AppendHeading("Element data dump", 2);

                var pc = oldScope.AppendDataFields(null);
                dirReadMe.AppendPairs(pc, false, " \t\t = \t");

                dirReadMe.AppendHorizontalLine();

                dirReadMe.AppendHeading("Contextual data contextual dump", 2);

                var p2c = context.data;
                dirReadMe.AppendPairs(p2c, false, " \t\t = \t");

                dirReadMe.AppendHorizontalLine();

                dirReadMe.AppendLine("File created: {{{sys_time}}}, {{{sys_date}}}, {{{meta_year}}}");
                dirReadMe.AppendLine("By: {{{meta_softwareName}}}");
                dirReadMe.AppendLine("{{{meta_copyright}}}, {{{meta_author}}}, {{{meta_organization}}}");

                dirReadMe.AppendHorizontalLine();

                dirReadMe.AppendLine("File system path: " + context.directoryScope.FullName);
                dirReadMe.AppendLine("Report root path: " + context.directoryRoot.FullName);

                string content = dirReadMe.ContentToString(true, reportOutputFormatName.markdown);
                content = content.applyToContent(pc);
                content = content.applyToContent(p2c);

                string path = context.directoryScope.FullName.add("readme.md", "\\");

                content.saveStringToFile(path, getWritableFileMode.overwrite, Encoding.UTF8);

                // leaving the directory
                context.directoryScope = context.directoryScope.Parent;
            }
            setRelPath(context);
        }

        public void composeOperationStart(IRenderExecutionContext context, IMetaContentNested composer)
        {
            //  throw new NotImplementedException();
        }

        public void composeOperationEnd(IRenderExecutionContext context, IMetaContentNested composer)
        {
            //  throw new NotImplementedException();
        }

        public void executeScriptInstruction(IRenderExecutionContext context, docScriptInstructionCompiled instruction)
        {
            //  throw new NotImplementedException();
        }

        public PropertyCollectionDictionary collectOperationStart(IRenderExecutionContext context, IMetaContentNested composer, PropertyCollectionDictionary dict)
        {
            string pt = composer.name;
            dict[composer.path].Add(templateFieldBasic.path_folder, pt);
            if (composer.parent != null)
            {
                PropertyCollection pc = dict[composer.parent.path];
                string parentPath = pc.getProperString(templateFieldBasic.path_folder);
                string fullpath = parentPath.add(pt, "\\");
                dict[composer.path].add(templateFieldBasic.path_folder, fullpath);
            }

            //throw new NotImplementedException();
            return dict;
        }

        public docScript composeOperationStart(IRenderExecutionContext context, IMetaContentNested composer, docScript script)
        {
            return script;
            //throw new NotImplementedException();
        }

        public docScript composeOperationEnd(IRenderExecutionContext context, IMetaContentNested composer, docScript script)
        {
            //    throw new NotImplementedException();
            return script;
        }
    }
}