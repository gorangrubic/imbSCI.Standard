// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ncFileJob.cs" company="imbVeles" >
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
// Project: imbSCI.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Core.syntax.nc
{
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.files.job;
    using imbSCI.Core.syntax.nc.line;
    using imbSCI.Core.syntax.nc.param;
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Xml.Serialization;

    [XmlInclude(typeof(ncLineRelativeCriteria))]
    [XmlInclude(typeof(ncLineCriteria))]
    [XmlInclude(typeof(ncParamModify))]
    [XmlInclude(typeof(ncParamModifyCollection))]

    /// <summary>
    /// Podesavanja sa parametrima za promenu NC fajlova
    /// </summary>
    public class ncFileJob<ncFile> : aceFileJobBase
    {
        /// <summary>
        /// Generise string izvestaj o trenutnom poslu
        /// </summary>
        /// <returns></returns>
        public String explainJob()
        {
            String output = base.explainJob();

            output = output.log("Inside each NC file it will select lines with command [" +
                           lineSelector.mainCriteria.commandCriteria + ":" + lineSelector.mainCriteria.customCommand +
                           "]");

            if (lineSelector.relativeCriterias.Any())
            {
                output = output.log("if additional criteria are met:");
                foreach (ncLineRelativeCriteria relCrit in lineSelector)
                {
                    String relLine = "";
                    relLine = " line with command [" + relCrit.commandCriteria.ToString() + "] is found";
                    switch (relCrit.relativeType)
                    {
                        case ncLineRelativeCriteriaType.anywhereWithin:
                            relLine += "anywhere within the selected line and [" + relCrit.relativePosition.ToString() + "] ";
                            break;

                        case ncLineRelativeCriteriaType.onExactPosition:
                            relLine += "exactly [" + relCrit.relativePosition.ToString() + "] lines from the selected line";
                            break;
                    }

                    output = output.log(relLine);
                }
            }

            output =
                output.log("For each selected lines (" + paramModifiers.Count.ToString() +
                           ") param modifiers will be applied");
            foreach (ncParamModify ncpm in paramModifiers)
            {
                output =
                    output.log("Param key [" + ncpm.parameterTarget + "] " + ncpm.modificationType.ToString() +
                               " value[" + ncpm.modificationValue + "]");
            }

            return output;
        }

        #region -----------  lineSelector  -------  [sele]

        private ncLineSelector _lineSelector = new ncLineSelector();

        /// <summary>
        /// sele
        /// </summary>
        // [XmlIgnore]
        [Category("ncFileModifyJob")]
        [DisplayName("Line selector")]
        [Description("Settings of direct and relative criteria for line selection during NC file processing. With this you target what NC instructions should be modified.")]
        public ncLineSelector lineSelector
        {
            get
            {
                return _lineSelector;
            }
            set
            {
                // Boolean chg = (_lineSelector != value);
                _lineSelector = value;
                OnPropertyChanged("lineSelector");
                // if (chg) {}
            }
        }

        #endregion -----------  lineSelector  -------  [sele]

        #region -----------  paramModifiers  -------  [Collection of param modifiers]

        private ncParamModifyCollection _paramModifiers = new ncParamModifyCollection();

        /// <summary>
        /// Collection of param modifiers
        /// </summary>
        // [XmlIgnore]
        [Category("ncFileModifyJob")]
        [DisplayName("Modifiers")]
        [Description("Collection of parameter modification instructions to be performed over ")]
        public ncParamModifyCollection paramModifiers
        {
            get
            {
                return _paramModifiers;
            }
            set
            {
                // Boolean chg = (_paramModifiers != value);
                _paramModifiers = value;
                OnPropertyChanged("paramModifiers");
                // if (chg) {}
            }
        }

        #endregion -----------  paramModifiers  -------  [Collection of param modifiers]
    }
}