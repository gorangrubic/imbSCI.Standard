// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaKeywords.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.blocks
{
    using imbSCI.Core.reporting.colors;
    using imbSCI.Data.enums.appends;
    using imbSCI.Reporting.script;

    /// <summary>
    /// container for keywords describing this document
    /// </summary>
    /// \ingroup_disabled docBlocks_common
    public class metaKeywords : MetaContainerNestedBase

    {
        public override void construct(object[] resources)
        {
            colors = acePaletteRole.colorA;
            width = blockWidth.full;
        }

        public override docScript compose(docScript script)
        {
            if (script == null) script = new docScript(name);
            script.x_scopeIn(this);

            script.add(appendType.s_palette).arg(colors);

            script.list("Keywords", "", content, 2, true);

            script.add(appendType.s_palette).arg(acePaletteRole.colorDefault);

            script.x_scopeOut(this);
            return script;
        }

        //public string makeText(imbStringBuilder sb = null)
        //{
        //    if (sb == null) sb = new imbStringBuilder();

        //    sb.AppendLine("---");
        //    sb.AppendLine("Keywords");
        //    sb.AppendLine("---");
        //    string kw = "";
        //    content.ForEach(x => kw.Append(x, ";"));
        //    sb.AppendLine(kw);
        //    sb.AppendLine("---");
        //    //sb.prevTabLevel();

        //   // sb.rootTabLevel();

        //    return sb.ToString();
        //}

        //public reportXmlDocument makeXml(reportXmlDocument xmlReport = null)
        //{
        //    if (xmlReport == null) xmlReport = new reportXmlDocument();
        //    return xmlReport;
        //}

        //public reportHtmlDocument makeHtml(reportHtmlDocument htmlReport = null)
        //{
        //    throw new NotImplementedException();
        //}

        //public metaKeywords(params string[] words)
        //{
        //    content.AddRange(words);
        //}

        //public string makeTextTemplate(imbStringBuilder sb = null)
        //{
        //    sb.AppendLine("---");
        //    sb.AppendLine("Keywords");
        //    sb.AppendLine("---");
        //    sb.AppendLine("{{{keywords_content}}}");
        //    //String kw = "";
        //    //content.ForEach(x => kw.Append(x, ";"));
        //    //sb.AppendLine(kw);
        //    sb.AppendLine("---");
        //    //sb.prevTabLevel();
        //    return sb.ToString();
        //}
    }
}