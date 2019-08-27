// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaExternalBlock.cs" company="imbVeles" >
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
    using imbSCI.Core.reporting.render;
    using imbSCI.Data;
    using imbSCI.Reporting.script;

#pragma warning disable CS1574 // XML comment has cref attribute 'ITextRender' that could not be resolved
    /// <summary>
    /// Imports content from file or <see cref="imbSCI.Reporting.reporting.render.ITextRender"/> instance
    /// </summary>
    /// <seealso cref="MetaContainerNestedBase" />
    public class metaExternalBlock : MetaContainerNestedBase
#pragma warning restore CS1574 // XML comment has cref attribute 'ITextRender' that could not be resolved
    {
        /// <summary>
        /// Name for this instance
        /// </summary>
        public string title { get; set; } = "";

        /// <summary>
        /// Human-readable description of object instance
        /// </summary>
        public string description { get; set; } = "";

        public metaExternalBlock(string __includeFilePath, string __title, string undertitle)
        {
            title = __title;
            description = undertitle;
            includeFilePath = __includeFilePath;
        }

        public metaExternalBlock(ITextRender __includeTextBuilder, string __title, string undertitle)
        {
            title = __title;
            description = undertitle;
            includeTextBuilder = __includeTextBuilder;
        }

        /// <summary>
        ///
        /// </summary>
        public string includeFilePath { get; set; }

        private ITextRender _includeTextBuilder;

        /// <summary> </summary>
        public ITextRender includeTextBuilder
        {
            get
            {
                return _includeTextBuilder;
            }
            protected set
            {
                _includeTextBuilder = value;
                OnPropertyChanged("includeTextBuilder");
            }
        }

        /// <summary>
        /// Composes a set of <c>docScriptInstruction</c> into supplied <c>docScript</c> instance or created blank new instance with <c>name</c> of this metaContainer
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        public override docScript compose(docScript script = null)
        {
            if (!title.isNullOrEmpty())
            {
                script.AppendHorizontalLine();

                script.open("import", name, "");

                script.AppendComment(description);

                script.AppendHorizontalLine();
            }
            if (!includeFilePath.isNullOrEmpty())
            {
                script.AppendFromFile(includeFilePath);
            }

            if (includeTextBuilder != null)
            {
                script.AppendDirect(includeTextBuilder.ContentToString());
            }
            if (!title.isNullOrEmpty())
            {
                script.close();
            }
            return script;
        }

        public override void construct(object[] resources)
        {
        }
    }
}