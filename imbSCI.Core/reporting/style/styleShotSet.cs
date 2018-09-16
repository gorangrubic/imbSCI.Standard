// --------------------------------------------------------------------------------------------------------------------
// <copyright file="styleShotSet.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.style
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.style.core;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Core.reporting.style.shot;
    using imbSCI.Data.enums.appends;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// 2017:Complete styling definition for one cell, div or whatever is the unit of styling
    /// </summary>
    public class styleShotSet : IStyleInstruction, IEnumerable
    {
        public styleShotSet()
        {
        }

        public styleShotSet(appendRole role, appendType type, acePaletteVariationRole colorRole, acePaletteRole paletteRole, Boolean isInverse, styleTheme theme)
        {
            palette = theme.palletes.getShotSet(colorRole, isInverse, paletteRole);
            text = theme.textShotProvider.getShotSet(role, type);
            container = theme.styleContainerProvider.getShotSet(role, type);
        }

        public styleShotSet(styleContainerShot __container, styleTextShot __text, acePaletteShot __palette)
        {
            palette = __palette;
            text = __text;
            container = __container;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="styleShotSet"/> to <see cref="styleContainerShot"/>.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator styleContainerShot(styleShotSet m)
        {
            return m.container;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="styleShotSet"/> to <see cref="styleTextShot"/>.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator styleTextShot(styleShotSet m)
        {
            return m.text;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="styleShotSet"/> to <see cref="acePaletteShot"/>.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator acePaletteShot(styleShotSet m)
        {
            return m.palette;
        }

        /// <summary>
        /// Gets a value indicating whether this <c>styleShotSet</c> is ready, having all tree sub IStyleInstruction non null.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ready; otherwise, <c>false</c>.
        /// </value>
        public Boolean isReady
        {
            get
            {
                return (container != null) && (text != null) && (palette != null);
            }
        }

        private styleContainerShot _container;

        /// <summary>
        ///
        /// </summary>
        public styleContainerShot container
        {
            get { return _container; }
            set { _container = value; }
        }

        private styleTextShot _text;

        /// <summary>
        ///
        /// </summary>
        public styleTextShot text
        {
            get { return _text; }
            set { _text = value; }
        }

        private acePaletteShot _palette;

        /// <summary>
        ///
        /// </summary>
        public acePaletteShot palette
        {
            get { return _palette; }
            set { _palette = value; }
        }

        /// <summary>
        /// Gets the codename.
        /// </summary>
        /// <returns></returns>
        public string getCodeName()
        {
            return container.getCodeName() + text.getCodeName() + palette.getCodeName();
        }

        /// <summary>
        /// Returns an enumerator that iterates through sub shots.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object to iterate trough all sub style shots.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            List<IStyleInstruction> tempList = new List<IStyleInstruction>();
            tempList.AddMultiple(container, palette, text);

            return tempList.GetEnumerator();
        }
    }
}