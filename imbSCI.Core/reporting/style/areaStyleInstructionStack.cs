// --------------------------------------------------------------------------------------------------------------------
// <copyright file="areaStyleInstructionStack.cs" company="imbVeles" >
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
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.style.shot;
    using imbSCI.Core.reporting.zone;
    using System;
    using System.Collections.Generic;
    using System.Linq;

#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.Stack{aceCommonTypes.reporting.style.areaStyleInstruction}'
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    /// <summary>
    /// Area Style Instruction stack - waiting for executin
    /// </summary>
    /// <seealso cref="System.Collections.Generic.Stack{aceCommonTypes.reporting.style.areaStyleInstruction}" />
    public class areaStyleInstructionStack : Stack<areaStyleInstruction>
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.Stack{aceCommonTypes.reporting.style.areaStyleInstruction}'
    {
        public areaStyleInstruction Add(IStyleInstruction shot, selectRangeArea area)
        {
            areaStyleInstruction tmp = new areaStyleInstruction(shot, area, false);
            tmp.shots.AddMultiple(tmp as IStyleInstruction);
            return tmp;
        }

        public areaStyleInstruction Add(IStyleInstruction shot, String path)
        {
            areaStyleInstruction tmp = new areaStyleInstruction(shot, path, false);
            tmp.shots.Add(tmp as IStyleInstruction);
            return tmp;
        }

        #region --- pathResolver ------- reference to dictionary to read area memory

        private selectRangeAreaDictionary _pathResolver;

        /// <summary>
        /// reference to dictionary to read area memory
        /// </summary>
        public selectRangeAreaDictionary pathResolver
        {
            get
            {
                return _pathResolver;
            }
            protected set
            {
                _pathResolver = value;
                //OnPropertyChanged("pathResolver");
            }
        }

        #endregion --- pathResolver ------- reference to dictionary to read area memory

        public void execute(ITextRender render, Int32 maxSteps = 1, ILogBuilder loger = null)
        {
            Int32 step = 0;

            if (!this.Any())
            {
                //  loger.log("-- areaStyleInstructionStack is empty -- execute halted");
                return;
            }

            List<areaStyleInstruction> waiting = new List<areaStyleInstruction>();
            while (Count > 0 && step < maxSteps)
            {
                if (!this.Any())
                {
                    //  loger.log("-- areaStyleInstructionStack became empty -- execute broke at "+step.ToString() + " step");
                    return;
                }
                areaStyleInstruction ins = this.Pop();
                step++;
                if (ins.resolveAreaPaths(pathResolver, ins.doAllowUnclosed))
                {
                    if (render is IDocumentRender)
                    {
                        IDocumentRender render_IDocumentRender = (IDocumentRender)render;
                        render_IDocumentRender.ApplyStyle(ins);
                    }

                    //render.ApplyStyle(ins);
                    break;
                }
                else
                {
                    waiting.Add(ins);
                }
            }

            //    loger.log(waiting.Count() + " Instructions is waiting for associated _area_ or _areas_ to be closed");
        }

        public areaStyleInstructionStack(selectRangeAreaDictionary __pathResolver = null)
        {
        }
    }
}