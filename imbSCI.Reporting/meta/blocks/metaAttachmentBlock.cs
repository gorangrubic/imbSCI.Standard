// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaAttachmentBlock.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.data;
    using imbSCI.Data;
    using imbSCI.Data.enums.appends;
    using imbSCI.Reporting.script;

    /// <summary>
    /// Attached file
    /// </summary>
    /// <seealso cref="MetaContainerNestedBase" />
    public class metaAttachmentBlock : MetaContainerNestedBase
    {
        /// <summary>
        ///
        /// </summary>
        public string includeFilePath { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string caption { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string description { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string templateNeedle { get; set; }

        #region ----------- Boolean [ isDataTemplate ] -------  []

        private bool _isDataTemplate = false;
        /// <summary>
        ///
        /// </summary>

        public bool isDataTemplate
        {
            get { return _isDataTemplate; }
            set { _isDataTemplate = value; OnPropertyChanged("isDataTemplate"); }
        }

        #endregion ----------- Boolean [ isDataTemplate ] -------  []

        /// <summary>
        /// Initializes a new instance of the <see cref="metaAttachmentBlock"/> class.
        /// </summary>
        /// <param name="__includeFilePath">The include file path.</param>
        /// <param name="__filename">The filename.</param>
        /// <param name="__caption">The caption.</param>
        /// <param name="__description">The description.</param>
        /// <param name="__templateNeedle">The template needle.</param>
        /// <param name="__isDataTemplate">if set to <c>true</c> [is data template].</param>
        public metaAttachmentBlock(string __includeFilePath, string __filename, string __caption, string __description, string __templateNeedle = "", bool __isDataTemplate = false)
        {
            includeFilePath = __includeFilePath;
            filename = __filename;
            caption = __caption;
            description = __description;
            templateNeedle = __templateNeedle;
            isDataTemplate = __isDataTemplate;
        }

        public override docScript compose(docScript script = null)
        {
            if (templateNeedle.isNullOrEmpty())
            {
                script.AppendFile(includeFilePath, filename, isDataTemplate);
            }
            else
            {
                script.AppendFileTemplated(includeFilePath, templateNeedle, filename, isDataTemplate, false);
            }
            if (!caption.isNullOrEmpty())
            {
                script.AppendHeading(caption, 4);

                script.AppendLine(description);

                script.AppendLink(filename, filename, description, appendLinkType.link);
            }

            return script;
        }

        public override void construct(object[] resources)
        {
        }

        //public override IMetaContentNested SearchForChild(string needle)
        //{
        //    throw new NotImplementedException();
        //}
    }
}