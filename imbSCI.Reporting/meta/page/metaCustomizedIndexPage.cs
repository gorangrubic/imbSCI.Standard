// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaCustomizedIndexPage.cs" company="imbVeles" >
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
    using imbSCI.Reporting.interfaces;
    using imbSCI.Reporting.meta.document;

    public class metaCustomizedIndexPage : metaCustomizedSimplePage, IMetaComposeAndConstruct
    {
        public metaCustomizedIndexPage(string __title, string __description) : base(__title, __description)
        {
        }

        public override void construct(object[] resources)
        {
            //  AddNavigation(this);
            if (parent is metaDocument)
            {
                AddIndex(parent as metaDocument);
            }

            //if (parent is metaDocumentSet)
            //{
            //    AddFullIndex(parent as metaDocumentSet);
            //}

            base.construct(resources);
        }
    }
}