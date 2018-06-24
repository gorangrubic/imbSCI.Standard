// --------------------------------------------------------------------------------------------------------------------
// <copyright file="acePaletteProvider.cs" company="imbVeles" >
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
using Color = System.Drawing.Color;

//using Color = System.Drawing.Color;
//using gradientBrush = System.Drawing.Drawing2D.LinearGradientBrush;

/// <summary>
/// Classes for colors creation and modification, inter-class conversion, management of predefined palettes and other tools for universal styling application
/// </summary>
namespace imbSCI.Core.reporting.colors
{
    using imbSCI.Core.extensions.enumworks;
    using imbSCI.Core.extensions.math;
    using imbSCI.Core.style.color;
    using imbSCI.Data;
    using imbSCI.Data.collection.special;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Palette provider for <c>styleTheme</c>. Resulting colors and brushes depend on <c>mainState</c> values
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    /// \ingroup_disabled report_ll
    public class acePaletteProvider : Dictionary<acePaletteRole, aceColorPalette>
    {
        /// <summary>
        /// Gets the color wheel.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public colorWhellForContent getColorWheel(acePaletteRole role = acePaletteRole.none)
        {
            if (role == acePaletteRole.none) role = active;
            aceColorPalette pal = this[role];

            colorWhellForContent output = new colorWhellForContent(pal.bgColors[acePaletteVariationRole.even.ToInt32()].ColorToHex(), pal.bgColors[acePaletteVariationRole.odd.ToInt32()].ColorToHex(), pal.bgColors[acePaletteVariationRole.important.ToInt32()].ColorToHex());
            output.header = pal.bgColors[acePaletteVariationRole.header.ToInt32()].ColorToHex();
            output.footer = pal.bgColors[acePaletteVariationRole.important.ToInt32()].ColorToHex();
            output.heading = pal.bgColors[acePaletteVariationRole.heading.ToInt32()].ColorToHex();
            return output;
        }

        /// <summary>
        /// circular access to palette
        /// </summary>
        public aceColorPalette paletteWheel
        {
            get
            {
                acePaletteRole rl = paletteRoleWheel.next(1);

                return this[rl];
            }
        }

        private circularSelector<acePaletteRole> _paletteRoleWheel = new circularSelector<acePaletteRole>();

        /// <summary>
        /// Provides next acePaletteRole
        /// </summary>
        public circularSelector<acePaletteRole> paletteRoleWheel
        {
            get { return _paletteRoleWheel; }
            set { _paletteRoleWheel = value; }
        }

        /// <summary>
        /// Automatically build shot set collection on instancing
        /// </summary>
        internal const Boolean doAutoRebuildShotSets = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="acePaletteProvider"/> class using colors: #none, #colorA, #colorB, #colorC ...
        /// </summary>
        /// <param name="colors">List of hex base colors to derive palletes from> #000000.</param>
        /// <remarks>if there are more roles than supplied base colors - it will use light gray default color for all the rest</remarks>
        public acePaletteProvider(IEnumerable<String> colors)
        {
            var roles = Enum.GetValues(typeof(acePaletteRole));
            List<String> baseColors = colors.ToList();

            Int32 i = 0;
            String baseColor = "";
            foreach (acePaletteRole role in roles)
            {
                if (i < baseColors.Count)
                {
                    baseColor = baseColors[i];
                }
                else
                {
                    baseColor = "#F2F2F2";
                }
                Add(role, aceColorPaletteManager.getPalette(role.ToString(), baseColor, role.ToString()));
                i++;
            }

            active = acePaletteRole.none;
            if (doAutoRebuildShotSets) rebuildShotSets();
        }

        /// <summary>
        /// Gets the codename of the color variation.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="isInverse">if set to <c>true</c> [is inverse].</param>
        /// <param name="act">The act.</param>
        /// <returns></returns>
        public static String getColorVariationCodeName(acePaletteVariationRole role, Boolean isInverse, acePaletteRole act)
        {
            return role.ToString().add(isInverse.ToString(), "_").add(act.ToString(), "_");
        }

        /// <summary>
        /// Rebuilds all values of the shot sets
        /// </summary>
        public void rebuildShotSets()
        {
            var roles = Enum.GetValues(typeof(acePaletteRole));
            var varRoles = Enum.GetValues(typeof(acePaletteVariationRole));

            foreach (acePaletteRole pRole in roles)
            {
                foreach (acePaletteVariationRole vRole in varRoles)
                {
                    shotSets.Add(makePalleteShotSet(vRole, true, pRole));
                    shotSets.Add(makePalleteShotSet(vRole, false, pRole));
                }
            }
        }

        internal acePaletteShotSetCollection shotSets = new acePaletteShotSetCollection();

        public String getHexCodeBackground(acePaletteVariationRole role)
        {
            return current.bgColors[role.ToInt32()].ColorToHex();
        }

        /// <summary>
        /// Gets the palette shot set - by role and current state of provider
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public acePaletteShot getShotSet(acePaletteVariationRole role)
        {
            return getShotSet(role, state.inverseState, state.active);
        }

        /// <summary>
        /// Gets the palette shot set - by codename
        /// </summary>
        /// <param name="codeName">Name of the code.</param>
        /// <returns></returns>
        internal acePaletteShot getShotSet(String codeName)
        {
            return shotSets[codeName];
        }

        /// <summary>
        /// Gets the palette shot set - by custom role, inverse and act
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="isInverse">if set to <c>true</c> [is inverse].</param>
        /// <param name="act">The act.</param>
        /// <returns>ShotSet with all colors and brushes</returns>
        /// \ingroup_disabled report_ll_highlights
        public acePaletteShot getShotSet(acePaletteVariationRole role, Boolean isInverse, acePaletteRole act)
        {
            String codeName = getColorVariationCodeName(role, isInverse, act);
            if (!shotSets.ContainsKey(codeName))
            {
                shotSets[codeName] = makePalleteShotSet(role, isInverse, act);
            }
            return getShotSet(codeName);
        }

        #region --- active ------- Currently active pallete role

        private acePaletteRole _active;

        /// <summary>
        /// Currently active pallete role
        /// </summary>
        public acePaletteRole active
        {
            get
            {
                return _active;
            }
            set
            {
                if (value != acePaletteRole.none)
                {
                    _active = value;
                }
            }
        }

        #endregion --- active ------- Currently active pallete role

        /// <summary>
        /// Reverts the variation - loads save state into main state
        /// </summary>
        public void revertVariation()
        {
            _mainState.Learn(_savedState);
        }

        /// <summary>
        /// Saves the variation state. Use <c>revertVariation()</c> to reload it.
        /// </summary>
        public void saveVariation()
        {
            _savedState = _mainState.Clone() as acePaletteProviderState;
        }

        /// <summary>
        /// Sets the variation.
        /// </summary>
        /// <param name="_bgIndex">Index of the bg.</param>
        /// <param name="_isInverse">if set to <c>true</c> [is inverse].</param>
        /// <param name="_fgIndex">Index of the fg.</param>
        /// <param name="itIsTemporary">if set to <c>true</c> it will save current state. Use <c>revertVariation()</c> to reload saved state</param>
        public void setVariation(Int32 _bgIndex, Boolean _isInverse = false, Int32 _fgIndex = -1, Boolean itIsTemporary = false, acePaletteRole act = acePaletteRole.none)
        {
            if (itIsTemporary)
            {
                saveVariation();
            }
            else
            {
                state.set(_bgIndex, _fgIndex, _isInverse);
            }
        }

        /// <summary>
        /// Gets the color[] variation set for [0] background, [1] tp, [2] foreground
        /// </summary>
        /// <param name="_index">The index.</param>
        /// <param name="_isInverse">if set to <c>true</c> [is inverse].</param>
        /// <param name="act">Palette role</param>
        /// <returns>[0] background, [1] tp, [2] foreground</returns>
        internal acePaletteShot makePalleteShotSet(acePaletteVariationRole role, Boolean isInverse, acePaletteRole act = acePaletteRole.none)
        {
            acePaletteShot output = new acePaletteShot();
            Int32 _intex = role.ToInt32();
            if (isInverse) _intex = ccount - _intex - 1;

            _intex = _intex.checkRange(acePaletteVariationRole.none.ToInt32() - 1, 0);

            aceColorPalette tmppal = this[act];
            var bgc = tmppal.bgColors[_intex];
            var bgb = tmppal.bgBrushes[_intex];
            var bgt = tmppal.tpColors[_intex];

            output.addAcePaletteShot(acePaletteShotResEnum.background, tmppal.bgColors[_intex], bgb, null, bgt, bgc);
            output.addAcePaletteShot(acePaletteShotResEnum.foreground, tmppal.fgColors[_intex], tmppal.fgBrushes[_intex], null, tmppal.fgColors[_intex], tmppal.fgColors[_intex]);
            output.addAcePaletteShot(acePaletteShotResEnum.border, tmppal.tpColors[_intex], tmppal.tpBrushes[_intex], null, tmppal.tpColors[_intex], tmppal.bgColors[_intex]);
            output.name = getColorVariationCodeName(role, isInverse, act);

            return output;
        }

        private aceColorPalette _current;

        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public aceColorPalette current
        {
            get
            {
                return this[active];
            }
        }

        private acePaletteProviderState _savedState = new acePaletteProviderState();

        private acePaletteProviderState _mainState = new acePaletteProviderState();

        /// <summary>
        /// active provider state
        /// </summary>
        internal acePaletteProviderState state
        {
            get { return _mainState; }
            set { _mainState = value; }
        }

        /// <summary>
        /// Number of color variations in the pallete
        /// </summary>
        /// <value>
        /// The ccount.
        /// </value>
        internal Int32 ccount
        {
            get
            {
                return current.colorsBottom.Count;
            }
        }

        #region ----------------------------------------------------------------------------------------

        ///// <summary>
        ///// Gets the bg brush.
        ///// </summary>
        ///// <value>
        ///// The bg brush.
        ///// </value>
        //public System.Windows.Media.LinearGradientBrush bgBrush
        //{
        //    get
        //    {
        //        if (state.inverseState) return current.backgrounds[ccount - state.bgIndex];
        //        return current.backgrounds[state.bgIndex];
        //    }
        //}
        ///// <summary>
        ///// Gets the fg brush.
        ///// </summary>
        ///// <value>
        ///// The fg brush.
        ///// </value>
        //public System.Windows.Media.SolidColorBrush fgBrush
        //{
        //    get
        //    {
        //        if (state.inverseState) return current.foregrounds[ccount - state.fgIndex];
        //        return current.foregrounds[state.fgIndex];
        //    }
        //}
        ///// <summary>
        ///// Gets the tp brush.
        ///// </summary>
        ///// <value>
        ///// The tp brush.
        ///// </value>
        //public System.Windows.Media.SolidColorBrush tpBrush
        //{
        //    get
        //    {
        //        if (state.inverseState) return current.underLines[ccount - state.fgIndex];
        //        return current.underLines[state.fgIndex];
        //    }
        //}
        /// <summary>
        /// Gets the color of the bg.
        /// </summary>
        /// <value>
        /// The color of the bg.
        /// </value>
        public Color bgColor
        {
            get
            {
                if (state.inverseState) return current.bgColors[ccount - state.bgIndex];
                return current.bgColors[state.bgIndex];
            }
        }

        /// <summary>
        /// Gets the color of the tp.
        /// </summary>
        /// <value>
        /// The color of the tp.
        /// </value>
        public Color tpColor
        {
            get
            {
                if (state.inverseState) return current.tpColors[ccount - state.fgIndex];
                return current.tpColors[state.bgIndex];
            }
        }

        /// <summary>
        /// Gets the color of the fg.
        /// </summary>
        /// <value>
        /// The color of the fg.
        /// </value>
        public Color fgColor
        {
            get
            {
                if (state.inverseState) return current.fgColors[ccount - state.fgIndex];
                return current.fgColors[state.bgIndex];
            }
        }

        #endregion ----------------------------------------------------------------------------------------

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public acePaletteProvider()
        {
        }
    }
}