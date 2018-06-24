// --------------------------------------------------------------------------------------------------------------------
// <copyright file="setPresetPaletteView.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.documentSet
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Reporting.interfaces;
    using imbSCI.Reporting.meta.document;
    using imbSCI.Reporting.meta.page;
    using imbSCI.Reporting.script;
    using System;
    using System.Collections.Generic;

    public class setPresetPaletteView : metaDocumentSet, IMetaComposeAndConstruct
    {
        public override metaPage indexPage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public setPresetPaletteView()
        {
            name = "paletteView";
            documentSetTitle = "Palette viewer";
        }

        public override void construct(object[] resources) //compose(IMetaComposer composer, metaDocumentTheme theme, PropertyCollection data, params object[] resources)
        {
            List<string> baseColors = resources.getAllOfType<string>();

            documentPaletteView dpv = new documentPaletteView();
            documents.Add(dpv, this);

            //baseColors.Add();

            baseConstruct(resources);
        }

        public override docScript compose(docScript script)
        {
            if (script == null) script = new docScript(name);

            script.x_scopeIn(this);

            documents.Sort();

            foreach (metaDocument pg in documents)
            {
                pg.compose(script);
            }

            script.x_scopeOut(this);

            return script;
        }
    }
}