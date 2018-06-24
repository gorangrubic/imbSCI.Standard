// --------------------------------------------------------------------------------------------------------------------
// <copyright file="styleAutoShotSet.cs" company="imbVeles" >
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
    using System.Collections.Generic;

    /// <summary>
    /// Virtual styleShotSet that is build automatically according to <c>flags</c>
    /// </summary>
    /// <seealso cref="IStyleInstruction" />
    public class styleAutoShotSet : styleShotSet, IStyleInstruction
    {
        public styleAutoShotSet(styleApplicationFlag __flags, params Object[] __resources)
        {
            flags = __flags;
            resources = __resources.getFlatList<Object>();
        }

        //protected IStyleInstruction resolveInstruction(appendRole role, styleApplicationFlag aFlag)
        //{
        //    IStyleInstruction output = null;
        //    appendType type = role.
        //    styleShotSet shot = new styleShotSet(role)

        //    if (flags.HasFlag(styleApplicationFlag.autoByRole))
        //    {
        //    }
        //}

        /// <summary>
        /// Resolves autoshot into a series of instructions
        /// </summary>
        /// <param name="theme">The active teme</param>
        /// <param name="__resources">Resources: set of style related enums and appendType</param>
        /// <seealso cref="aceCommonTypes.enums.appendType"/>
        /// <seealso cref="acePaletteRole"/>
        /// <seealso cref="appendRole"/>
        /// <seealso cref="acePaletteVariationRole"/>
        /// <returns></returns>
        public List<IStyleInstruction> resolve(styleTheme theme, params Object[] __resources)
        {
            List<IStyleInstruction> output = new List<IStyleInstruction>();
            acePaletteVariationRole colorRole = acePaletteVariationRole.none;

            List<Int32> xy = __resources.getAllOfType<Int32>(false);

            //--------- applying customizations
            appendType type = __resources.getFirstOfType<appendType>(false, appendType.none);
            appendRole role = __resources.getFirstOfType<appendRole>(false, appendRole.none);
            Boolean isInverse = __resources.getFirstOfType<Boolean>(false, false);
            acePaletteRole paletteRole = __resources.getFirstOfType<acePaletteRole>(false, acePaletteRole.none);

            if (role == appendRole.none)
            {
                role = appendRole.paragraph;
            }

            //if ((role != appendRole.none) && (type == appendType.none))
            //{
            //    type = role.convertRoleToType();
            //}

            //if ((role != appendRole.none) && (colorRole == acePaletteVariationRole.none))
            //{
            //    colorRole = role.convertRoleToVariationRole();
            //}

            if (paletteRole == acePaletteRole.none) flags &= ~styleApplicationFlag.enableColorShot;

            if (flags.HasFlag(styleApplicationFlag.autoByVariator))
            {
                var tmp = theme.styler.getStyleShot(xy[0], xy[1], type);
                if (flags.HasFlag(styleApplicationFlag.enableColorShot)) palette = tmp.palette; //theme.palletes.getShotSet(colorRole, isInverse, paletteRole);
                if (flags.HasFlag(styleApplicationFlag.enableTextShot)) text = tmp.text; // theme.textShotProvider.getShotSet(role);
                if (flags.HasFlag(styleApplicationFlag.enableContentShot)) container = container; // = theme.styleContainerProvider.getShotSet(role, type);
            }
            else
            {
                if (flags.HasFlag(styleApplicationFlag.enableColorShot)) palette = theme.palletes.getShotSet(colorRole, isInverse, paletteRole);
                if (flags.HasFlag(styleApplicationFlag.enableTextShot)) text = theme.textShotProvider.getShotSet(role);
                if (flags.HasFlag(styleApplicationFlag.enableContentShot)) container = theme.styleContainerProvider.getShotSet(role, type);
            }

            if (flags.HasFlag(styleApplicationFlag.setAsActive)) theme.activeStyleInstruction = this;

            foreach (IStyleInstruction inst in this)
            {
                if (inst != null)
                {
                    output.Add(inst);
                }
            }

            return output;
        }

        private List<Object> _resources;

        /// <summary>
        ///
        /// </summary>
        public List<Object> resources
        {
            get { return _resources; }
            protected set { _resources = value; }
        }

        private styleApplicationFlag _flags;

        /// <summary>
        ///
        /// </summary>
        protected styleApplicationFlag flags
        {
            get { return _flags; }
            set { _flags = value; }
        }
    }
}