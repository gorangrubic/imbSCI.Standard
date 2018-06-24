// --------------------------------------------------------------------------------------------------------------------
// <copyright file="deliveryUnitItemPaletteCSS.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.reporting.extensions;
    using imbSCI.Core.reporting.render;
    using imbSCI.Data.enums;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="imbSCI.Reporting.meta.delivery.deliveryUnitItem" />
    public class deliveryUnitItemPaletteCSS : deliveryUnitItemSupportFile, IDeliveryUnitItem, IDeliveryUnitItemFromFileSource, IDeliverySupportFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="deliveryUnitItemPaletteCSS"/> class.
        /// </summary>
        /// <param name="opath">The opath.</param>
        public deliveryUnitItemPaletteCSS(string __sourcepath, string __outputfolder) : base(__sourcepath, __outputfolder)
        {
            location = deliveryUnitItemLocationBase.globalDeliveryResource;
            flags = deliveryUnitItemFlags.filenameExtensionIsStatic | deliveryUnitItemFlags.linkToPrimaryContent | deliveryUnitItemFlags.filenameIsDataTemplate | deliveryUnitItemFlags.useTemplate;
        }

        public override void prepareOperation(IRenderExecutionContext context)
        {
            string output_path = outputpath.toPath(context.directoryRoot.FullName, context.data);

            var pc = context.theme.AppendDataFields(context.data);

            base.prepareOperation(context);

            string output = template.applyToContent(false, context.data);

            output_fileinfo = output_path.getWritableFile(getWritableFileMode.overwrite);

            //output.saveStringToFile(output_fileinfo.FullName, getWritableFileMode.overwrite); // = output.saveStringToFile(output_path, imbSCI.Cores.enums.getWritableFileMode.overwrite);

            context.saveFileOutput(output, output_fileinfo.FullName, "css", description);
        }

        /*

        public void composeOperationEnd(IRenderExecutionContext context, IMetaContent composer)
        {
          //  throw new NotImplementedException();
        }

        public void composeOperationStart(IRenderExecutionContext context, IMetaContent composer)
        {
            //throw new NotImplementedException();
        }

        public void scopeInOperation(IRenderExecutionContext context, IMetaContentNested newScope)
        {
            //throw new NotImplementedException();
        }

        public void scopeOutOperation(IRenderExecutionContext context, IMetaContentNested oldScope)
        {
           // throw new NotImplementedException();
        }

        public PropertyCollectionDictionary collectOperationStart(IRenderExecutionContext context, IMetaContent composer, PropertyCollectionDictionary dict)
        {
            throw new NotImplementedException();
        }

        public void executeScriptInstruction(IRenderExecutionContext context, docScriptInstructionCompiled instruction)
        {
            throw new NotImplementedException();
        }

        public docScript composeOperationStart(IRenderExecutionContext context, IMetaContent composer, docScript script)
        {
            throw new NotImplementedException();
        }

        public docScript composeOperationEnd(IRenderExecutionContext context, IMetaContent composer, docScript script)
        {
            throw new NotImplementedException();
        }
        */
    }
}