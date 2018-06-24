// --------------------------------------------------------------------------------------------------------------------
// <copyright file="deliveryUnitRenderCollection.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.delivery
{
    using imbSCI.Core.reporting.render;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Collection of renders
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{imbSCI.Reporting.meta.delivery.deliveryUnit}" />
    /// <seealso cref="imbSCI.Reporting.reporting.render.IRender"/>
    public class deliveryUnitRenderCollection : IEnumerable<IRender>
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T get<T>() where T : IRender, new()
        {
            IRender output = items.FirstOrDefault(x => x is T);
            if (output == null) output = new T();

            return (T)output;
        }

        /// <summary>
        /// Gets the text renders.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ITextRender> getTextRenders()
        {
            List<ITextRender> output = new List<ITextRender>();

            foreach (IRender r in items)
            {
                if (r is ITextRender)
                {
                    ITextRender r_ITextRender = (ITextRender)r;
                    output.Add(r_ITextRender);
                }
            }

            return output;
        }

        /// <summary>
        ///
        /// </summary>
        protected List<IRender> items { get; set; } = new List<IRender>();

        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

        public IEnumerator<IRender> GetEnumerator()
        {
            return ((IEnumerable<IRender>)items).GetEnumerator();
        }
    }
}