// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaStyleComposer.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.render
{
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.template;
    using imbSCI.Data.data;
    using System.Data;
    using System.Linq;

#pragma warning disable CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="imbSCI.Cores.primitives.imbBindable" />
    /// \ingroup_disabled report_cm_render
    public sealed class metaStyleComposer<T> : imbBindable where T : ITextRender, new()
#pragma warning restore CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    {
        public void AppendColor(aceColorPalette paleta, stringTemplate template, PropertyCollection data)
        {
        }

        #region --- sb ------- text rendering instance

        /// <summary>
        /// text rendering instance
        /// </summary>
        private T sb { get; set; } = new T();

        #endregion --- sb ------- text rendering instance

        private bool hasContent(IMetaContentNested source)
        {
            return source.content.Any();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public T getBuilder()
        {
            return sb;
        }
    }
}