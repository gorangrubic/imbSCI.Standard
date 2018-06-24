// --------------------------------------------------------------------------------------------------------------------
// <copyright file="linknodeTools.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataComplex.linknode
{
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.data;
    using imbSCI.Data;
    using System.Data;
    using System.Text.RegularExpressions;

    public static class linknodeTools
    {
        // ([\w\d-\.=]*)[/#\&\$\?\:]*
        // regex za razbijanje na nodove

        public static Regex regexForPathElements = new Regex(@"([\w\d-\.=]+)[/#\&\$\?\:]*", RegexOptions.Compiled);

        /// <summary>
        /// Builds the parent score collection.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static PropertyCollectionExtended buildParentScoreCollection(this linknodeElement node)
        {
            PropertyCollectionExtended pce = new PropertyCollectionExtended();
            linknodeElement head = node;

            do
            {
                if (head == null) break;
                pce.Add(head.name, head.score, head.name, head.path + " [" + head.level.ToString() + "]");
                if (head.parent != null)
                {
                    head = head.parent;
                }
            } while (head.parent != null);

            return pce;
        }

        /// <summary>
        /// Builds the parent score table.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static DataTable buildParentScoreTable(this linknodeElement node)
        {
            PropertyCollectionExtended pce = node.buildParentScoreCollection();

            DataTable dataTable = pce.buildDataTable("Score table");

            return dataTable;
        }

        public const char PATH_SPLITTER = '/';

        /// <summary>
        /// Processes the <c>path</c>, builds nodes if missing and adds scores to existing elements. Supplied meta object is attached to the last node
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="path">The path.</param>
        /// <param name="meta">The meta.</param>
        /// <param name="score">The score.</param>
        /// <returns></returns>
        public static linknodeElement buildNode(this linknodeElement root, string path, object meta, int score = 1)
        {
            linknodeElement head = root;
            var ms = regexForPathElements.Matches(path);
            string repath = "";
            int level = 0;
            foreach (Match m in ms)
            {
                if ((m.Value.Length == 0) || (m.Value == PATH_SPLITTER.ToString())) continue;

                level++;
                string pathPart = m.Value.Trim(PATH_SPLITTER);
                repath = repath.add(pathPart, PATH_SPLITTER);

                if (head.items.ContainsKey(pathPart))
                {
                    head = head.items[pathPart];
                    head.score++;
                }
                else
                {
                    linknodeElement tmp = new linknodeElement();
                    tmp.setnode(repath, pathPart, head, root, level);
                    tmp.score = score;
                    head.items.Add(pathPart, tmp);
                    head = tmp;
                }
            }

            head.setmeta(meta);
            return head;
        }
    }
}