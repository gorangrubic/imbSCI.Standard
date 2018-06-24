// --------------------------------------------------------------------------------------------------------------------
// <copyright file="linksNavigation.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.presets.links
{
    using imbSCI.Core.interfaces;
    using imbSCI.Reporting.meta.blocks;
    using imbSCI.Reporting.meta.collection;
    using imbSCI.Reporting.meta.page;

    /// <summary>
    /// Dynamic collection of links to document, root, other pages, next/prev
    /// </summary>
    public class linksNavigation : metaLinkCollection, INavigation
    {
        public linksNavigation()
        {
            type = metaLinkCollectionType.relative;
            name = "Navigation";
        }

        /// <summary>
        /// Rebuilds navigation items -- this should be run before rendering the content
        /// </summary>
        /// <param name="__parent"></param>
        public void rebuild(IMetaContentNested __parent)
        {
            parent = __parent;

            if (parent.document != null)
            {
                var link = AddLink(parent.document, "Document page", -25);
            }

            if (parent.root != null)
            {
                var link = AddLink(parent.document, "Report page", -50);
            }

            if (parent.isThisDocument)
            {
                int index = parent.indexOf(__parent);

                if (parent.Count() > index)
                {
                    AddLink(parent[index + 1] as IMetaContentNested, "Next page", -10);
                }

                if (index > 0)
                {
                    AddLink(parent[index - 1] as IMetaContentNested, "Prev page", -5);
                }
            }

            if (parent is metaPage)
            {
                var link = AddLink(parent, "Back to parent", -15);
                //link.name = "Parent page";
            }
        }
    }
}