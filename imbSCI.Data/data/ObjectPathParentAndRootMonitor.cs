// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectPathParentAndRootMonitor.cs" company="imbVeles" >
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
// Project: imbSCI.Data
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Data.data
{
    using imbSCI.Data.interfaces;
    using System;

    /// <summary>
    /// Monitors changes in scope - results are in State object
    /// </summary>
    /// <seealso cref="imbBindable" />
    public class ObjectPathParentAndRootMonitor : imbBindable
    {
        private ObjectScopeChangeReport state = new ObjectScopeChangeReport();

        private Object lastRoot;
        private Object lastParent;
        private String lastPath;
        private Int32 lastChildrenCount;
        private IObjectWithPathAndChildSelector lastTarget;

        protected ObjectScopeChangeReport State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
            }
        }

        /// <summary>
        /// Sets target and automatically Update - sets all switches if options used
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="setSwitches">if set to <c>true</c> it will set state switches to <c>switchValueToSet</c> value</param>
        /// <param name="switchValueToSet">Value to set to switches, if <c>setSwitches</c> is <c>true</c></param>
        public ObjectPathParentAndRootMonitor(IObjectWithPathAndChildSelector target, Boolean setSwitches = false, Boolean switchValueToSet = true)
        {
            update(target);

            if (setSwitches)
            {
                state.IsChildrenCountChanged = switchValueToSet;
                state.IsParentChanged = switchValueToSet;
                state.IsPathChanged = switchValueToSet;
                state.IsRootChanged = switchValueToSet;
                state.IsTargetChanged = switchValueToSet;
                state.IsChanged = switchValueToSet;
            }
        }

        /// <summary>
        /// Returns state of changes and reset internal change switches if <c>acceptChange</c> is TRUE
        /// </summary>
        /// <param name="acceptChanges">if set to <c>true</c> [accept changes].</param>
        /// <returns>Changes report</returns>
        public ObjectScopeChangeReport getState(Boolean acceptChanges = true)
        {
            ObjectScopeChangeReport output = State;
            if (acceptChanges)
            {
                State = new ObjectScopeChangeReport();
                return output;
            }
            else
            {
                return State;
            }
        }

        /// <summary>
        /// Checks for changes and records it into internal change switches
        /// </summary>
        /// <param name="target">Monitored object</param>
        public void update(IObjectWithPathAndChildSelector target)
        {
            if (State == null) State = new ObjectScopeChangeReport();

            if (lastTarget != target)
            {
                State.IsTargetChanged = true;
                lastTarget = target;
            }

            if (target == null) return;

            if (lastPath != target.path)
            {
                State.IsPathChanged = true;
                lastPath = target.path;
            }

            if (lastParent != target.parent)
            {
                State.IsParentChanged = true;
                lastParent = target.parent;
            }

            if (lastRoot != target.root)
            {
                State.IsRootChanged = true;
                lastRoot = target.root;
            }

            if (lastChildrenCount != target.Count())
            {
                State.IsChildrenCountChanged = true;
                lastChildrenCount = target.Count();
            }
        }
    }
}