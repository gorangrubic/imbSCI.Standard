// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDeliverySupportFile.cs" company="imbVeles" >
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
    using imbSCI.Core.reporting.render;
    using imbSCI.Data.enums.appends;
    using System.IO;

    /// <summary>
    /// Support file
    /// </summary>
    /// <seealso cref="imbSCI.Reporting.meta.delivery.items.IDeliveryUnitItemFromFileSource" />
    /// <seealso cref="imbSCI.Reporting.meta.delivery.IDeliveryUnitItem" />
    public interface IDeliverySupportFile : IDeliveryUnitItemFromFileSource, IDeliveryUnitItem
    {
        FileInfo sourceFileInfo
        {
            get;
            set;
        }

        FileInfo output_fileinfo { get; }

        appendLinkType linkType { get; }

        string getRelativeUrl(IRenderExecutionContext context, string __relToRoot = null);

        void appendToRender(IRenderExecutionContext context, ITextRender outputRender);
    }
}