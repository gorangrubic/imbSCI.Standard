// --------------------------------------------------------------------------------------------------------------------
// <copyright file="deliveryUnitItemSupportFile.cs" company="imbVeles" >
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
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.render;
    using imbSCI.Data;
    using imbSCI.Data.enums.appends;
    using imbSCI.Reporting.script;

    /// <summary>
    /// Static file copy
    /// </summary>
    /// <seealso cref="imbSCI.Reporting.meta.delivery.deliveryUnitItem" />
    /// <seealso cref="imbSCI.Reporting.meta.delivery.IDeliveryUnitItem" />
    /// <seealso cref="imbSCI.Reporting.meta.delivery.items.IDeliveryUnitItemFromFileSource" />
    public class deliveryUnitItemSupportFile : deliveryUnitItem, IDeliveryUnitItem, IDeliveryUnitItemFromFileSource, IDeliverySupportFile
    {
        /// <summary>
        ///
        /// </summary>
        public appendLinkType linkType { get; set; } = appendLinkType.scriptLink;

        public deliveryUnitItemSupportFile(string __sourcepath, string __outputfolder) : base(deliveryUnitItemType.supportFile)
        {
            location = deliveryUnitItemLocationBase.globalDeliveryResource;
            flags = deliveryUnitItemFlags.filenameExtensionIsStatic | deliveryUnitItemFlags.linkToPrimaryContent | deliveryUnitItemFlags.filenameIsDataTemplate | deliveryUnitItemFlags.useCopy;

            sourcepath.setup(__sourcepath);
            string opath = __outputfolder.add(sourcepath.filenameWithExtension, "\\");

            outputpath.setup(opath);

            switch (sourcepath.extension)
            {
                case ".css":
                    linkType = appendLinkType.styleLink;
                    break;

                case ".js":
                    linkType = appendLinkType.scriptLink;
                    break;
            }
        }

        public PropertyCollectionDictionary collectOperationStart(IRenderExecutionContext context, IMetaContentNested composer, PropertyCollectionDictionary dict)
        {
            return dict;
            //  throw new NotImplementedException();
        }

        public docScript composeOperationEnd(IRenderExecutionContext context, IMetaContentNested composer, docScript script)
        {
            return script;
            // throw new NotImplementedException();
        }

        public docScript composeOperationStart(IRenderExecutionContext context, IMetaContentNested composer, docScript script)
        {
            return script;
            //    throw new NotImplementedException();
        }

        public void executeScriptInstruction(IRenderExecutionContext context, docScriptInstructionCompiled instruction)
        {
            //  throw new NotImplementedException();
        }

        public override void prepareOperation(IRenderExecutionContext context)
        {
            base.prepareOperation(context);

            // String msg = fileOpsBase.copyFile(src, trg, name);

            //  this.prepareOperation(context);
        }

        public void scopeInOperation(IRenderExecutionContext context, IMetaContentNested newScope)
        {
        }

        public void scopeOutOperation(IRenderExecutionContext context, IMetaContentNested oldScope)
        {
        }

        public void appendToRender(IRenderExecutionContext context, ITextRender __render)
        {
            string pathToFile = getRelativeUrl(context);
            __render.AppendLink(pathToFile, "", "", linkType);
        }

        public string getRelativeUrl(IRenderExecutionContext context, string __relToRoot = null)
        {
            string relJump = __relToRoot;
            if (imbSciStringExtensions.isNullOrEmpty(relJump))
            {
                relJump = context.directoryScope.getRelativePathToParent(context.directoryRoot);
            }

            string relPath = outputpath.toPath("");  //output_fileinfo.FullName.removeStartsWith(context.directoryRoot.FullName);
            relJump = relJump.add(relPath, "");
            relJump = relJump.getWebPathBackslashFormat().Replace(" ", "");
            relJump = relJump.Replace("//", "/");
            // relJump = relJump.Replace("}}}/", "}}}");
            return relJump;
        }
    }
}