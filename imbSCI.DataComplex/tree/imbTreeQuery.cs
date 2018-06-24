// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbTreeQuery.cs" company="imbVeles" >
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
using imbSCI.Data;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.DataComplex.tree
{
    using imbSCI.Core.extensions.data;

    #region imbVeles using

    using System;

    #endregion imbVeles using

    /// <summary>
    ///
    /// </summary>
    public static class imbTreeQuery
    {
        public static Int32 iterationLimit = 10;

        /// <summary>
        /// Pravi "flat" formu iz hijerarhije
        /// </summary>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        public static imbTreeNodeBlockCollection breakToBlocks(this imbTreeNodeBranch parentNode)
        {
            imbTreeNodeBlockCollection output = new imbTreeNodeBlockCollection();

            Int32 i = 0;

            List<imbTreeNode> toDo = new List<imbTreeNode>();
            List<imbTreeNode> newToDo = new List<imbTreeNode>();
            toDo.Add(parentNode);

            if (parentNode == null)
            {
                throw new ArgumentNullException("parentNode", "parentNode is [null], have no content to break to blocks");

                return output;
            }

            //while (i < iterationLimit)
            //{
            do
            {
                if (i > iterationLimit)
                {
                    throw new ArgumentOutOfRangeException("Depth limit reached on imbTreeQuery ::  logType.ExecutionError");
                    break;
                }
                newToDo = new List<imbTreeNode>();
                output.newBlock();
                //output.newBlock();

                foreach (imbTreeNode nd in toDo)
                {
                    switch (nd.type)
                    {
                        default:
                            imbTreeNodeBranch bnd2 = nd as imbTreeNodeBranch;
                            foreach (var t in bnd2) newToDo.Add(t.Value);
                            break;

                        case imbTreeNodeType.leafed: /// ovo je privremeno iskljuceno
                            imbTreeNodeBranch bnd = nd as imbTreeNodeBranch;
                            foreach (var t in bnd) newToDo.Add(t.Value);
                            break;

                        case imbTreeNodeType.leaf:
                            // output.current.Add(nd);

                            if (nd.Any())
                            {
                                output.current.Add(nd);
                            }
                            else if (!nd.sourceContent.Trim().isNullOrEmpty())
                            {
                                output.current.Add(nd);
                            }
                            else
                            {
                            }
                            break;

                        case imbTreeNodeType.end:
                            imbTreeNodeBlock blc = output.newBlock(nd.path);
                            imbTreeNodeBranch end = nd as imbTreeNodeBranch;
                            foreach (var t in end)
                            {
                                blc.Add(t);
                            }

                            break;
                    }
                }

                toDo = newToDo;
                i++;
            } while (toDo.Count > 0);

            //}

            output.removeEmptyBlocks();

            return output;
        }

        /// <summary>
        /// Compresses the nodes.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="iterationLimit">The iteration limit.</param>
        public static void compressNodes(this imbTreeNodeBranch parentNode, Int32 iterationLimit = 10000)
        {
            Int32 i = 0;

            List<imbTreeNode> toDo = new List<imbTreeNode>();
            List<imbTreeNode> newToDo = new List<imbTreeNode>();
            toDo.Add(parentNode);

            do
            {
                if (i > iterationLimit)
                {
                    throw new ArgumentOutOfRangeException("imbTreeQuery.compressNodes(" + parentNode.keyHash + " ==> " + parentNode.sourcePath + ")");

                    break;
                }
                newToDo = new List<imbTreeNode>();

                foreach (imbTreeNode nd in toDo)
                {
                    nd.compressNode();
                }

                foreach (imbTreeNode nd in toDo)
                {
                    if (nd is imbTreeNodeBranch)
                    {
                        imbTreeNodeBranch bnd = nd as imbTreeNodeBranch;
                        foreach (var t in bnd) newToDo.Add(t.Value);
                    }
                }

                i++;
                toDo = newToDo;
            } while (toDo.Count > 0);
        }

        /// <summary>
        /// Pokrece detektovanje tipova
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="depthLimit"></param>
        /// <param name="_flags"></param>
        public static void detectTypes(this imbTreeNodeBranch parentNode, Int32 depthLimit)
        {
            Int32 i = 0;

            List<imbTreeNode> toDo = new List<imbTreeNode>();
            List<imbTreeNode> newToDo = new List<imbTreeNode>();
            toDo.Add(parentNode);

            do
            {
                if (i > depthLimit)
                {
                    // logSystem.log("Depth limit reached on imbTreeQuery :: ", logType.ExecutionError);
                    break;
                }
                newToDo = new List<imbTreeNode>();

                foreach (imbTreeNode nd in toDo)
                {
                    if (nd.type == imbTreeNodeType.unknown)
                    {
                        nd.type = nd.nodeType();
                    }

                    if (nd is imbTreeNodeBranch)
                    {
                        imbTreeNodeBranch bnd = nd as imbTreeNodeBranch;
                        foreach (var t in bnd) newToDo.Add(t.Value);
                    }
                }

                i++;
                toDo = newToDo;
            } while (toDo.Count > 0);

            //return output;
        }

        /// <summary>
        /// Vraca svo lisce
        /// </summary>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        public static List<imbTreeNodeLeaf> allLeafes(this imbTreeNodeBranch parentNode)
        {
            List<imbTreeNodeLeaf> output = new List<imbTreeNodeLeaf>();
            IEnumerable<imbTreeNode> branchNodes = parentNode.items.Values.Where(x => x is imbTreeNodeLeaf);
            branchNodes.ToList().ForEach(x => output.Add(x as imbTreeNodeLeaf));
            return output;
        }

        /// <summary>
        /// Vraca sve grane
        /// </summary>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        public static List<imbTreeNodeBranch> allBranches(this imbTreeNodeBranch parentNode)
        {
            List<imbTreeNodeBranch> output = new List<imbTreeNodeBranch>();
            IEnumerable<imbTreeNode> branchNodes = parentNode.items.Values.Where(x => x is imbTreeNodeBranch);
            branchNodes.ToList().ForEach(x => output.Add(x as imbTreeNodeBranch));
            return output;
        }

        /// <summary>
        /// Tests the node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        internal static Boolean testNode(this imbTreeNode node, imbTreeQueryFlag flag, imbTreeNodeType types)
        {
            var flags = flag.getEnumListFromFlags<imbTreeQueryFlag>();
            if (flags.Contains(imbTreeQueryFlag.collectAll)) return true;
            if (node is imbTreeNodeLeaf)
            {
                if (flags.ContainsOneOrMore(imbTreeQueryFlag.collectAllLeafs)) return true;
            }
            else
            {
                if (flags.ContainsOneOrMore(imbTreeQueryFlag.collectAllBranches)) return true;
            }

            if (flags.Contains(imbTreeQueryFlag.collectAllOfNodeType))
            {
                return types.HasFlag(node.nodeType());
            }

            return false;
        }

        /// <summary>
        /// Vraca sve node-ove koji ispunjavaju uslove upita date u imbTreeNodeType i imbTreeQueryFlag enumima
        /// </summary>
        /// <param name="parentNode">Node nad kojim se vrsi upit</param>
        /// <param name="depthLimit">Limit koliko duboko moze da ide upit</param>
        /// <param name="_flags">imbTreeNodeType i imbTreeQueryFlag enumi kojima se podesava upit</param>
        /// <returns>Kolekcija</returns>
        public static imbTreeNodeCollection query(this imbTreeNodeBranch parentNode, Int32 depthLimit,
                                                  params Object[] _flags)
        {
            imbTreeQueryFlag flags = _flags.getFirstOfType<imbTreeQueryFlag>(); // = new imbTreeQueryFlags(_flags);
            imbTreeNodeType types = _flags.getFirstOfType<imbTreeNodeType>(); // = new imbTreeNodeTypes(_flags);
            imbTreeNodeCollection output = new imbTreeNodeCollection();
            Int32 i = 0;

            List<imbTreeNode> toDo = new List<imbTreeNode>();
            List<imbTreeNode> newToDo = new List<imbTreeNode>();
            toDo.Add(parentNode);

            do
            {
                if (i > depthLimit)
                {
                    //logSystem.log("Depth limit reached on imbTreeQuery :: ", logType.ExecutionError);
                    break;
                }
                newToDo = new List<imbTreeNode>();

                foreach (imbTreeNode nd in toDo)
                {
                    if (nd.testNode(flags, types))
                    {
                        output.Add(nd.path, nd);
                    }

                    if (nd is imbTreeNodeBranch)
                    {
                        imbTreeNodeBranch bnd = nd as imbTreeNodeBranch;
                        foreach (var t in bnd) newToDo.Add(t.Value);
                    }
                }

                i++;
                toDo = newToDo;
            } while (toDo.Count > 0);

            return output;
        }
    }
}