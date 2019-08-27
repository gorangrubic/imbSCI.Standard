// --------------------------------------------------------------------------------------------------------------------
// <copyright file="tagStack.cs" company="imbVeles" >
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
    using System;
    using System.Collections;
    using System.Collections.Generic;

#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.Stack{aceCommonTypes.reporting.render.core.tagBlock}'
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.Collections.Generic.Stack{aceCommonTypes.reporting.render.core.tagBlock}" />
    public class tagStack : IEnumerable, IEnumerable<tagBlock>
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.Stack{aceCommonTypes.reporting.render.core.tagBlock}'
    {
        public Int32 Count => stack.Count;

        public void Clear()
        {
            stack.Clear();
            levelCount.Clear();
        }

        private Stack<tagBlock> _stack = new Stack<tagBlock>();

        /// <summary>
        ///
        /// </summary>
        public Stack<tagBlock> stack
        {
            get { return _stack; }
            set { _stack = value; }
        }

        /// <summary>
        /// Adds the specified tag.
        /// </summary>
        /// <param name="__tag">The tag.</param>
        /// <param name="__name">The name.</param>
        /// <param name="__description">The description.</param>
        /// <returns></returns>
        public tagBlock Add(String __tag, String __name, String __description)
        {
            tagBlock tb = new tagBlock(__tag, __name, __description, getPeek());
            stack.Push(tb);

            levelCount[headingLevel] = levelCount[headingLevel] + 1;
            tb.count = levelCount[headingLevel];

            return tb;
        }

        public tagBlock Peek() => getPeek();

        public tagBlock Pop()
        {
            tagBlock pTb = null;
            if (Count > 0)
            {
                levelCount[headingLevel + 1] = 0;
                pTb = stack.Pop();
            }
            return pTb;
        }

        public String GetPeekName()
        {
            if (Count == 0) return "";
            return getPeek().tag;
        }

        public tagBlock getPeek()
        {
            tagBlock pTb = null;
            if (Count > 0)
            {
                pTb = stack.Peek();
            }
            return pTb;
        }

        ///// <summary>
        ///// Gets the title number-list prefix. Up to third level
        ///// </summary>
        ///// <returns></returns>
        //public String getTitle()
        //{
        //    String output = "";
        //    var tb = getPeek();
        //    if (tb == null) return "";

        //    for (int i = 0; i < headingLevel; i++)
        //    {
        //        Int32 lc = levelCount[i];
        //        switch(i)
        //        {
        //            case 0:
        //                output = output.add(String.Format("{0:D3}", tb.count), " ");
        //                break;
        //            case 1:
        //                output = output.add(String.Format("{0}", tb.count), ".");
        //                break;
        //            case 2:
        //                output = output.add(String.Format("{0}", tb.count.toOrdinalLetter(true)), ".");
        //                break;
        //            default:
        //                //output = output.add(String.Format("{0}", lc), ".");
        //                break;
        //        }
        //    }

        //    return output.add(tb.name, " ");
        //}

        /// <summary>
        /// Pushes the specified tag.
        /// </summary>
        /// <param name="__tag">The tag.</param>
        /// <param name="__name">The name.</param>
        /// <param name="__description">The description.</param>
        /// <returns></returns>
        public tagBlock Push(String __tag, String __name = "", String __description = "")
        {
            tagBlock tb = Add(__tag, __name, __description); //new tagBlock(__tag, __name, __description, Peek());

            return tb;
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)stack).GetEnumerator();
        }

        IEnumerator<tagBlock> IEnumerable<tagBlock>.GetEnumerator()
        {
            return ((IEnumerable<tagBlock>)stack).GetEnumerator();
        }

        private tagBlockTitleCounter _levelCount = new tagBlockTitleCounter();

        /// <summary>
        ///
        /// </summary>
        public tagBlockTitleCounter levelCount
        {
            get { return _levelCount; }
            protected set { _levelCount = value; }
        }

        /// <summary>
        /// Gets the heading level.
        /// </summary>
        /// <value>
        /// The heading level.
        /// </value>
        public Int32 headingLevel
        {
            get
            {
                Int32 outlevel = Count;
                if (outlevel > 6) outlevel = 6;
                return outlevel;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 deepth
        {
            get
            {
                return Count;
            }
        }
    }
}