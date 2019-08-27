// --------------------------------------------------------------------------------------------------------------------
// <copyright file="areaStyleInstruction.cs" company="imbVeles" >
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
    using imbSCI.Core.reporting.style.shot;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.data;
    using System;
    using System.Collections.Generic;
    using System.Linq;

#pragma warning disable CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    /// <summary>
    /// Set of <see cref="IStyleInstruction"/>s and <see cref="selectRangeArea"/>s to be applied.
    /// </summary>
    /// <remarks>Each instruction is applied to each area. <c>pathList</c> is checked before execution - areas are updated witht his list </remarks>
    /// <seealso cref="aceCommonTypes.primitives.imbBindable" />
    public class areaStyleInstruction : imbBindable
#pragma warning restore CS1574 // XML comment has cref attribute 'imbBindable' that could not be resolved
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="areaStyleInstruction"/> class.
        /// </summary>
        /// <param name="styleShots">The style shots.</param>
        /// <param name="area">The area.</param>
        /// <param name="__doAllowUnclosed">if set to <c>true</c> [do allow unclosed].</param>
        public areaStyleInstruction(IStyleInstruction styleShots, String area, Boolean __doAllowUnclosed = false)
        {
            Add(styleShots);
            Add(area);
            doAllowUnclosed = __doAllowUnclosed;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="areaStyleInstruction"/> class.
        /// </summary>
        /// <param name="styleShots">The style shots.</param>
        /// <param name="area">The area.</param>
        /// <param name="__doAllowUnclosed">if set to <c>true</c> [do allow unclosed].</param>
        public areaStyleInstruction(IStyleInstruction styleShots, selectRangeArea area, Boolean __doAllowUnclosed = false)
        {
            Add(styleShots);
            Add(area);
            doAllowUnclosed = __doAllowUnclosed;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="areaStyleInstruction"/> class.
        /// </summary>
        /// <param name="__doAllowUnclosed">if set to <c>true</c> [do allow unclosed].</param>
        public areaStyleInstruction(Boolean __doAllowUnclosed = false)
        {
            doAllowUnclosed = __doAllowUnclosed;
        }

        private Boolean _doAllowUnclosed = false;

        /// <summary>
        /// Is instruction allowed over unclosed areas
        /// </summary>
        public Boolean doAllowUnclosed
        {
            get { return _doAllowUnclosed; }
            set { _doAllowUnclosed = value; }
        }

        /// <summary>
        /// Adds IStyleInstruction item
        /// </summary>
        /// <param name="in_shots">The in shots.</param>
        public void Add(params IStyleInstruction[] in_shots)
        {
            List<IStyleInstruction> in_shotList = in_shots.getFlatList<IStyleInstruction>();

            shots.AddRange(in_shotList);
        }

        /// <summary>
        /// Adds area/s that are affected by this
        /// </summary>
        /// <param name="in_areas">The in areas.</param>
        public void Add(params selectRangeArea[] in_areas)
        {
            List<selectRangeArea> in_areasList = in_areas.getFlatList<selectRangeArea>();

            areas.AddRange(in_areasList);
        }

        /// <summary>
        /// Adds paths to areas that will be processed later by calling <see cref="resolveAreaPaths(selectRangeAreaDictionary, bool)"/>
        /// </summary>
        /// <param name="in_paths">The in paths.</param>
        public void Add(params String[] in_paths)
        {
            List<String> in_shotList = in_paths.getFlatList<String>();
            areaPaths.AddRange<String>(in_shotList);
        }

        /// <summary>
        /// Gets from paths - TRUE if loaded all
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        /// <param name="allowUnclosed">For special cases where we want to affect an area before it is <c>closed</c></param>
        /// <returns>
        /// TRUE if all areas are loaded - and no <c>path</c> is waiting to be loaded
        /// </returns>
        public Boolean resolveAreaPaths(selectRangeAreaDictionary resolver, Boolean allowUnclosed = false)
        {
            Boolean output = false;
            selectRangeAreaNamed area = null;
            List<String> waiting = new List<string>();
            do
            {
                String pt = areaPaths.Pop();
                area = resolver[pt];

                if (area == null)
                {
                    waiting.Add(pt);
                }
                else
                {
                    if (area.isClosed || allowUnclosed)
                    {
                        areas.Add(area);
                    }
                    else
                    {
                        waiting.Add(pt);
                    }
                }
            } while (areaPaths.Any());

            if (!waiting.Any()) return true;

            areaPaths.AddRange<String>(waiting);

            return false;
        }

        #region --- pathList ------- list of paths for delayed areas retrieval

        private Stack<String> _areaPaths = new Stack<string>();

        /// <summary>
        /// list of paths for delayed areas retrieval
        /// </summary>
        public Stack<String> areaPaths
        {
            get
            {
                return _areaPaths;
            }
            set
            {
                _areaPaths = value;
                OnPropertyChanged("pathList");
            }
        }

        #endregion --- pathList ------- list of paths for delayed areas retrieval

        #region --- shots ------- multiple IStyleInstruction shots to be applied on designated area

        private List<IStyleInstruction> _shots = new List<IStyleInstruction>();

        /// <summary>
        /// multiple IStyleInstruction shots to be applied on designated area
        /// </summary>
        public List<IStyleInstruction> shots
        {
            get
            {
                return _shots;
            }
            set
            {
                _shots = value;
                OnPropertyChanged("shots");
            }
        }

        #endregion --- shots ------- multiple IStyleInstruction shots to be applied on designated area

        #region --- areas ------- Area that is targeted by shots

        private List<selectRangeArea> _areas;

        /// <summary>
        /// Area that is targeted by shots
        /// </summary>
        public List<selectRangeArea> areas
        {
            get
            {
                return _areas;
            }
            set
            {
                _areas = value;
                OnPropertyChanged("areas");
            }
        }

        #endregion --- areas ------- Area that is targeted by shots
    }
}