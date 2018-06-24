// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaFooter.cs" company="imbVeles" >
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
    using imbSCI.Reporting.script;
    using System.ComponentModel;

    /// <summary>
    /// Meta descriptive information container for FOOTER
    /// </summary>
    /// \ingroup_disabled docBlocks_common
    public class metaFooter : MetaContainerNestedBase
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

            // script.add(appendType.s_palette).arg(colors);

            script.section(bottomLine, "", content);

            // script.add(appendType.c_section, docScriptArguments.dsa_name, docScriptArguments.dsa_content, docScriptArguments.dsa_description, docScriptArguments.dsa_class_attribute, docScriptArguments.dsa_id_attribute).set(name, content, bottomLine, "footer", "#" + name);

            // script.add(appendType.s_palette).arg(acePaletteRole.colorDefault);

            script.x_scopeOut(this);
            return script;
        }

        ///// <summary>
        ///// PREVAZIĐENO
        ///// </summary>
        ///// <param name="sb">The sb.</param>
        ///// <returns></returns>
        //public string makeTextTemplate(imbStringBuilder sb = null)
        //{
        //    if (sb == null) sb = new imbStringBuilder();

        //    sb.AppendLine("---");
        //    content.ForEach(x => sb.AppendLine(x));
        //    //sb.AppendCreationTime(true);
        //    sb.AppendLine("---");
        //    sb.nextTabLevel();
        //    sb.AppendLine("Stamp: {{{self_stamp}}}");
        //    sb.AppendLine("File path: {{{path_outputPath}}}");
        //    sb.AppendLine("File format: {{{path_format}}}");
        //    sb.AppendLine("File category: {{{path_folder}}}");
        //    sb.AppendLine("File created: {{{sys_time}}}, {{{sys_date}}}");
        //    sb.AppendLine("---");
        //    sb.AppendLine("Memory: {{{sys_mem}}}");
        //    sb.AppendLine("Threads: {{{sys_threads}}}");
        //    sb.AppendLine("Running: {{{sys_runtime}}}");
        //    sb.AppendLine("AppDomain: {{{sys_app}}}");
        //    sb.AppendLine("Path: {{{sys_path}}}");
        //    sb.prevTabLevel();
        //    sb.AppendLine("Uid: {{{sys_uid}}}");
        //    sb.AppendLine("Place holders count: {{{self_plc}}}");
        //    sb.AppendLine("Place holders: {{{self_plcl}}}");
        //    sb.AppendLine("Title: {{{self_title}}}");
        //    sb.AppendLine("Desc: {{{self_desc}}}");
        //    sb.AppendLine("Type: {{{self_type}}}");
        //    sb.AppendLine("Template flags: {{{self_tflags}}}");
        //    sb.AppendLine("Flags: {{{self_flags}}}");

        //    sb.AppendLine("---");

        //    sb.AppendLine(bottomLine);
        //    sb.AppendLine("---");
        //    sb.AppendLine();
        //    //sb.prevTabLevel();

        //    content.ForEach(x => sb.AppendLine(x));

        //    sb.rootTabLevel();

        //    return sb.ToString();
        //}

        //public string makeText(imbStringBuilder sb = null)
        //{
        //    if (sb == null) sb = new imbStringBuilder();

        //    sb.AppendLine("---");
        //    content.ForEach(x => sb.AppendLine(x));
        //    sb.AppendCreationTime(true);
        //    sb.AppendLine("---");

        //    sb.AppendLine(bottomLine);
        //    sb.AppendLine("---");
        //    sb.AppendLine();
        //    //sb.prevTabLevel();

        //    content.ForEach(x => sb.AppendLine(x));

        //    sb.rootTabLevel();

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

        public metaFooter()
        {
            bottomLine = "";
        }

        public metaFooter(string __bottomLine, params string[] entries)
        {
            bottomLine = __bottomLine;
            content.AddRange(entries);
        }

        #region -----------  bottomLine  -------  [The last line on bottom of the document]

        private string _bottomLine; // = new String();

        /// <summary>
        /// The last line on bottom of the document
        /// </summary>
        // [XmlIgnore]
        [Category("metaFooter")]
        [DisplayName("bottomLine")]
        [Description("The last line on bottom of the document")]
        public string bottomLine
        {
            get
            {
                return _bottomLine;
            }
            set
            {
                // Boolean chg = (_bottomLine != value);
                _bottomLine = value;
                OnPropertyChanged("bottomLine");
                // if (chg) {}
            }
        }

        #endregion -----------  bottomLine  -------  [The last line on bottom of the document]
    }
}