// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDeliveryUnitItem.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.delivery
{
    using imbSCI.Core.collection;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.render;
    using imbSCI.Data.interfaces;
    using imbSCI.Reporting.script;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="IObjectWithNameAndDescription" />
    public interface IDeliveryUnitItem : IObjectWithNameAndDescription, IDeliveryComposer
    {
        void prepareOperation(IRenderExecutionContext context);

        PropertyCollectionDictionary collectOperationStart(IRenderExecutionContext context, IMetaContentNested composer, PropertyCollectionDictionary dict);

        //docScript composeOperationStart(IRenderExecutionContext context, IMetaContent composer, docScript script);

        //docScript composeOperationEnd(IRenderExecutionContext context, IMetaContent composer, docScript script);

        void scopeInOperation(IRenderExecutionContext context, IMetaContentNested newScope);

        void executeScriptInstruction(IRenderExecutionContext context, docScriptInstructionCompiled instruction);

        void scopeOutOperation(IRenderExecutionContext context, IMetaContentNested oldScope);

        void reportFinishedOperation(IRenderExecutionContext context);

        string name { get; }
        string description { get; }
        deliveryUnitItemType itemType { get; }
        deliveryUnitItemLocationBase location { get; }
        deliveryUnitItemFlags flags { get; }

        //    string filepath { get; }
        //    aceFolderInfo sourceFolder { get; set; }
        //    string sourcepath { get; }
    }
}