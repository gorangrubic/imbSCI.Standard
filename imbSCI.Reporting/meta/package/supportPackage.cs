// --------------------------------------------------------------------------------------------------------------------
// <copyright file="supportPackage.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.package
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Reporting.meta.page;
    using imbSCI.Reporting.script;

    public class supportPackage : metaServicePage
    {
        public override docScript compose(docScript script)
        {
            script = this.checkScript(script);

            script.x_directory(directoryOperation.copy, "reportInclude".add(foldername, "\\"), false);

            script.x_scopeIn(this);

            //script.add(appendType.i_page, docScriptArguments.dsa_name, docScriptArguments.dsa_title,docScriptArguments.dsa_description)
            //    .set(name, pageTitle, pageDescription);

            // script.add(appendType.s_settings).arg(docScriptArguments.dsa_stylerSettings, settings);

            script = this.subCompose(script);

            script.x_scopeOut(this);
            return script;
        }

        public supportPackage(string reportIncludeFolder, string description) : base(description, "Support package : " + reportIncludeFolder, "about_" + reportIncludeFolder, 10)
        {
        }

        private string _foldername;

        /// <summary> </summary>
        protected string foldername
        {
            get
            {
                return _foldername;
            }
            set
            {
                _foldername = value;
                OnPropertyChanged("foldername");
            }
        }
    }
}