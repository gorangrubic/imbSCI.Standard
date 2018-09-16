// --------------------------------------------------------------------------------------------------------------------
// <copyright file="tableRangeStyle.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.style.table
{
    using imbSCI.Core.reporting.style.core;

    /// <summary>
    /// Describe table style
    /// </summary>
    public class tableRangeStyle
    {
        private styleFourSide _outterLayout = new styleFourSide();

        /// <summary>
        /// Top=mergedHeader, Left=rowCounter, down=footer, right=NOT APPLUED
        /// </summary>
        public styleFourSide outterLayout
        {
            get { return _outterLayout; }
            set { _outterLayout = value; }
        }

        private styleFourSide _innerLayout = new styleFourSide();

        /// <summary>
        /// top=column heading, left=firstColumn, right=lastColumn, down=column foot
        /// </summary>
        public styleFourSide innerLayout
        {
            get { return _innerLayout; }
            set { _innerLayout = value; }
        }
    }
}