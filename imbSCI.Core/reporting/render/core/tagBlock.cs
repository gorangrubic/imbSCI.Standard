// --------------------------------------------------------------------------------------------------------------------
// <copyright file="tagBlock.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.render.core
{
    using imbSCI.Core.extensions.text;
    using imbSCI.Data;
    using imbSCI.Data.interfaces;
    using System;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithNameAndDescription" />
    public class tagBlock : IObjectWithNameAndDescription
    {
        private Object _meta;

        /// <summary>
        /// Custom object related to the block
        /// </summary>
        public Object meta
        {
            get { return _meta; }
            set { _meta = value; }
        }




        private Int32 _count = 0;

        /// <summary>
        ///
        /// </summary>
        public Int32 count
        {
            get { return _count; }
            set { _count = value; }
        }

        public Int32 depth
        {
            get
            {
                if (_parent == this) return 0;
                if (parent == null)
                {
                    return 0; //prop
                }
                else
                {
                    return parent.depth + 1;
                }
            }
        }

        public String getTitle(Boolean includeParent)
        {
            String output = getTitlePrefix(includeParent);

            return output.add(name, " ");
        }

        public String getTitlePrefix(Boolean includeParent = true)
        {
            String output = "";

            output = count.toNumberedListPrefix(depth, 3);
            if (includeParent)
            {
                if (parent != null)
                {
                    output = parent.getTitlePrefix().add(output, ".");
                }
            }

            return output;
        }

        public tagBlock(String __tag, String __name, String __description, tagBlock __parent)
        {
            tag = __tag;
            name = __name;
            description = __description;
            parent = __parent;
        }

        private tagBlock _parent;

        /// <summary>
        ///
        /// </summary>
        public tagBlock parent
        {
            get { return _parent; }
            protected set
            {
                if (value == this)
                {
                    _parent = null;
                }
                else
                {

                    _parent = value;
                }

            }
        }

        private String _tag;

        /// <summary>
        ///
        /// </summary>
        public String tag
        {
            get { return _tag; }
            protected set { _tag = value; }
        }

        private String _name;

        /// <summary>
        ///
        /// </summary>
        public String name
        {
            get { return _name; }
            set { _name = value; }
        }

        private String _description;

        /// <summary>
        ///
        /// </summary>
        public String description
        {
            get { return _description; }
            set { _description = value; }
        }
    }
}