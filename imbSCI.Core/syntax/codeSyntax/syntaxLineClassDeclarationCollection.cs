// --------------------------------------------------------------------------------------------------------------------
// <copyright file="syntaxLineClassDeclarationCollection.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.codeSyntax
{
    using System;
    using System.Linq;

    public class syntaxLineClassDeclarationCollection : syntaxDeclarationElementCollection<syntaxLineClassDeclaration>
    {
        public syntaxLineClassDeclaration this[String __name]
        {
            get
            {
                var cl = this.Find(x => x.name == __name);

                if (cl != null)
                {
                    return cl;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns lineclass declaration using classname. If failed, returns first class in the collection.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public syntaxLineClassDeclaration getClass(String className)
        {
            //this.Find(x=>x.name == className)
            foreach (syntaxLineClassDeclaration tmp in this)
            {
                if (tmp.name == className)
                {
                    return tmp;
                }
            }
            return this.First();
        }

        //public void AddLineClass(syntaxBlockLineType __type, String __template, String __tokenQuery)
        //{
        //    String __className = __type.ToString();
        //    syntaxLineClassDeclaration lc = new syntaxLineClassDeclaration(__className, __template, __tokenQuery);
        //    Add(lc);
        //}
    }
}