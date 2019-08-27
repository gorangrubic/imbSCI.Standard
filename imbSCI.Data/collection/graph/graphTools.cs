// --------------------------------------------------------------------------------------------------------------------
// <copyright file="graphTools.cs" company="imbVeles" >
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
using imbSCI.Data.collection.nested;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Data.collection.graph
{
    /// <summary>
    /// NIJE DOVRSENO
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.List{imbSCI.Data.collection.graph.graphNavigationIteration{T}}" />
    public class graphNavigationResult<T> : List<graphNavigationIteration<T>>
    {
        protected Dictionary<T, graphNavigationThread<T>> threadsByStartNode { get; set; } = new Dictionary<T, graphNavigationThread<T>>();

        public ListDictionaryBase<T, graphNavigationThread<T>> thradsByNode { get; set; } = new ListDictionaryBase<T, graphNavigationThread<T>>();

        public graphNavigationResult(IEnumerable<T> startNodes, Boolean _includeStart)
        {
            foreach (T node in startNodes)
            {
                var thread = new graphNavigationThread<T>(node, _includeStart);
                threadsByStartNode.Add(node, thread);
            }
        }

        public void RunIteration()
        {
            foreach (var threadPair in threadsByStartNode)
            {

            }
        }

    }

    public class graphNavigationThread<T>:List<graphNavigationIteration<T>>
    {
        public List<T> selected { get; set; } = new List<T>();

        public T startNode { get; protected set; }

        public graphNavigationThread(T _start, Boolean _includeStart)
        {
            startNode = _start;


            graphNavigationIteration<T> iteration = new graphNavigationIteration<T>();

            if (_includeStart)
            {

                iteration.Nodes.Add(_start);

                selected.Add(_start);
            }
        }

        public List<T> StartNewIteration()
        {
            var lastIteration = this.LastOrDefault();
            List<T> toNavigate = new List<T>();

            graphNavigationIteration<T> output = new graphNavigationIteration<T>();
            if (lastIteration != null)
            {
                output.Iteration = lastIteration.Iteration++;
                
                foreach (T n in lastIteration.Nodes)
                {
                    if (!selected.Contains(n))
                    {
                        selected.Add(n);
                        toNavigate.Add(n);
                    } else
                    {
                        
                    }
                }
            }
            Add(output);
            return toNavigate;
        }

        //public void RunIteration()
        //{
        //    foreach (var threadPair in threadsByStartNode)
        //    {

        //    }
        //}
    }

    public class graphNavigationIteration<T>
    {
        public Int32 Iteration { get; set; } = 0;

        public List<T> Nodes { get; set; } = new List<T>();

        public graphNavigationIteration<T> StartNewIteration()
        {
            graphNavigationIteration<T> output = new graphNavigationIteration<T>();
            output.Iteration = Iteration++;
            return output;
        }

        public graphNavigationIteration()
        {

        }

    }

    public class graphNavigator<T>
    {
        public graphNavigator()
        {

        }

        public List<T> Select(T node)
        {
            List<T> output = new List<T>();

            if (ForwardSelector != null) output.AddRange(ForwardSelector(node));
            if (BackwardSelector != null) output.AddRange(BackwardSelector(node));

            return output;
        }

       

        public graphNavigationIteration<T> RunIteration(graphNavigationThread<T> thread)
        {
            var lastIteration = thread.LastOrDefault();
            Int32 i = 0;

            graphNavigationIteration<T> newIteration = null;
            List<T> toNavigate = null;
            if (lastIteration == null)
            {
                lastIteration = new graphNavigationIteration<T>();
                if (IncludeStartInResult)
                {
                    lastIteration.Nodes.Add(thread.startNode);
                }
                else
                {
                    lastIteration.Nodes.AddRange(Select(thread.startNode));
                }
                thread.Add(lastIteration);

                
            }

            toNavigate = thread.StartNewIteration();

            
          /*
                 

                newIteration = lastIteration.StartNewIteration();

                foreach (T node in lastIteration.Nodes)
                {

                }
                
            }

            if (i == 0)
            {
               
            }
            */
            lastIteration = newIteration;
            return newIteration;
        }

        protected virtual void DeploySelectors(Func<T, List<T>> forwardSelector, Func<T, List<T>> backwardSelector)
        {
            ForwardSelector = forwardSelector;
            BackwardSelector = backwardSelector;
        }

        protected virtual void Deploy(Int32 limit, Func<T, Boolean> continueNodeEvaluation = null, Func<T, Boolean> goalEvaluation = null)
        {
            IterationLimit = limit;
            if (continueNodeEvaluation != null) ContinueNodeEvaluation = continueNodeEvaluation;
            if (goalEvaluation != null) GoalEvaluation = goalEvaluation;
            
        }

        //public virtual graphNavigationResult<T> Run(IEnumerable<T> startNode, Int32 limit, Func<T, Boolean> continueNodeEvaluation=null, Func<T, Boolean> goalEvaluation=null)
        //{

        //}

        //public virtual graphNavigationResult<T> Run(T startNode, Int32 limit, Func<T, Boolean> continueNodeEvaluation=null, Func<T, Boolean> goalEvaluation=null)
        //{

        //}

        public Int32 IterationLimit { get; set; } = 200;

        public Func<T, Boolean> ContinueNodeEvaluation { get; set; }

        public Func<T, Boolean> GoalEvaluation { get; set; }

        protected virtual Func<T, List<T>> ForwardSelector { get; set; }

        protected virtual Func<T, List<T>> BackwardSelector { get; set; }

        public Boolean PreventCircularNavigation { get; set; } = true;

        public Boolean IncludeStartInResult { get; set; } = true;

    }


    /// <summary>
    /// Extensions for <see cref="graphNode"/> , <see cref="graphWrapNode{TItem}"/> and other graphNode derivatives
    /// </summary>
    public static class graphTools
    {
        /*
         public static List<T> DetectLoopForward<T>(this T startNode, Int32 limit=50) where T : IGraphNode
        {

            List<T> known = new List<T>();

            List<T> heads = new List<T>();

            List<T> neW_heads = new List<T>();

            while (heads.Any())
            {
                foreach (T h in heads)
                {
                    if (known.Contains(h))
                    {

                    }
                }
            }
            
        }

        public static List<T> DetectLoopForward<T>(this T startNode, Int32 limit=50) where T : IGraphNode
        {


            List<List<T>> tracks = new List<List<T>>();

            List<T> heads = new List<T>();

            foreach (T c in startNode)
            {
                heads.Add(c);
            }

            for (int i = 0; i < heads.Count; i++)
            {
                if (i >= tracks.Count)
                {
                    tracks.Add(new List<T>());
                }
                tracks[i].Add()
                heads[i]

            }

            List<T> neW_heads = new List<T>();

            while (heads.Any())
            {

            }
            
        }
        */



        /// <summary>
        /// Returns descendent nodes at specified <c>depthOffset</c>. For branches that end before the offset depth, the leaf node is part of output
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent">The parent - starting node.</param>
        /// <param name="depthOffset">Descendent distance - distance of frontier to select</param>
        /// <returns></returns>
        public static List<T> GetDescendentFrontierAtOffset<T>(this T parent, Int32 depthOffset = 1) where T : IGraphNode
        {
            List<T> output = new List<T>();

            Int32 l = parent.level;
            List<T> newTasks = new List<T>();
            newTasks.Add(parent);

            while (newTasks.Any())
            {
                var tasks = newTasks.ToList();
                newTasks = new List<T>();
                foreach (T cat in tasks)
                {
                    if (cat.Count() == 0)
                    {
                        output.Add(cat);
                    }
                    else
                    {
                        if ((cat.level - l) < depthOffset)
                        {
                            foreach (T subcat in cat)
                            {
                                newTasks.Add(subcat);

                            }
                        }
                        else
                        {
                            foreach (T subcat in cat)
                            {
                                output.Add(subcat);

                            }
                        }
                    }
                }

            }

            return output;
        }

        /// <summary>
        /// Gets a child at given relative path
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent">The parent, from which the path should be interpretted.</param>
        /// <param name="path">Relative path, given with separator defined by {T} or specified. If a path segment not found it will skip it</param>
        /// <param name="customSeparator">Custom separator - to be used instead of one defined by the {T}.</param>
        /// <param name="returnParentIfNotFound">if set to <c>true</c> [return parent if not found].</param>
        /// <returns>
        /// Child at path or parent (if no path segment could be matched)
        /// </returns>
        public static T GetChildAtPath<T>(this T parent, String path, String customSeparator = "", Boolean returnParentIfNotFound = true) where T : IGraphNode
        {
            if (customSeparator.isNullOrEmpty()) customSeparator = parent.pathSeparator;
            List<String> pathParts = path.SplitSmart(customSeparator);
            T head = parent;
            Boolean noMatch = true;
            foreach (String pPart in pathParts)
            {
                if (head.ContainsKey(pPart))
                {
                    head = (T)head[pPart];
                    noMatch = false;
                }
            }
            if (noMatch && !returnParentIfNotFound)
            {
                return default(T);
            }
            return head;
        }



        public static TGraph GetSubgraph<TGraph>(this TGraph SourceNode, Boolean RemoveFromSource = true) where TGraph: class, IGraphNode, new()
        {
            String rootPath = SourceNode.path;
            var allChildren = SourceNode.getAllChildrenInType<TGraph>(null,false, false, 1, 500, true);
            var allPaths = allChildren.Select(x => x.path.removeStartsWith(rootPath));

            TGraph output = BuildGraphFromPaths<TGraph>(allPaths, SourceNode.pathSeparator, true, false);
            if (RemoveFromSource)
            {
                SourceNode.removeFromParent(); //.RemoveChildren(allPaths);
            }
            return output;
        }

        public static Int32 RemoveChildren<TGraph>(this TGraph node, IEnumerable<String> paths) where TGraph : class, IGraphNode
        {
            Int32 c = 0;
            foreach (String p in paths)
            {
                if (node.RemoveChild<TGraph>(p))
                {
                    c++;
                }
            }
            return c;
        }

        public static Boolean RemoveChild<TGraph>(this TGraph node, String path) where TGraph : class, IGraphNode
        {
            
            var child = node.GetChildAtPath(path, node.pathSeparator, false);

            if (child == null)
            {
                child = (node.root as TGraph).GetChildAtPath<TGraph>(path, node.pathSeparator, false);
            }
            if (child == null) return false;
            child.removeFromParent();
            return true;
        }

        /// <summary>
        /// Builds the graph from items. Resulting graph nodes wrap the source items in <see cref="graphWrapNode{TItem}.item"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TGraph">The type of the graph.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="path">The path.</param>
        /// <param name="splitter">The splitter.</param>
        /// <returns></returns>
        public static TGraph BuildGraphFromItems<T, TGraph>(this IEnumerable<T> items, Func<T, String> path, String splitter = "") where T : class, IObjectWithName
            where TGraph : graphWrapNode<T>, IGraphNode, new()
        {
            //TGraph parent = new TGraph(); //default(T);
            //if (splitter == "") splitter = parent.pathSeparator;

            //  var paths = items.Select(x => path(x)).ToList();

            //   Boolean isApsolutePath = paths.Any(x => x.StartsWith(splitter));

            if (items == null) return null;
            if (items.Count() == 0) return null;

            TGraph nodeCreated = null;

            foreach (T item in items)
            {
                if (item != null)
                {
                    String p = path(item);

                    nodeCreated = ConvertPathToGraph<TGraph>(null, p, false, splitter, true);
                    nodeCreated.SetItem(item);
                }
            }
            if (nodeCreated == null) return null;

            return nodeCreated.root as TGraph;

        }

        /// <summary>
        /// Builds the graph from paths. Created graph has artificial root node (outside first node of the paths)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputList">The input list.</param>
        /// <param name="splitter">The splitter character like: / or . If not set, uses the <see cref="IGraphNode.pathSeparator"/>, set by type constructor</param>
        /// <param name="isApsolutePath">if set to <c>true</c> [is apsolute path], starting with leading spliter, e.g.. /div[0]/span[1].</param>
        /// <param name="returnHead">if set to <c>true</c> it will return the last created leaf, otherwise: the root node of the graph</param>
        /// <returns></returns>
        public static T BuildGraphFromPaths<T>(this IEnumerable<String> inputList, String splitter = "", Boolean isApsolutePath = true, Boolean returnHead = true) where T : class, IGraphNode, new()
        {

         //   T parent = new T(); //default(T);

            if (splitter == "") splitter = System.IO.Path.DirectorySeparatorChar.ToString();
            T n = null;

            foreach (String p in inputList)
            {
                n = ConvertPathToGraph<T>(n, p, isApsolutePath, splitter, true);
                if (n != null)
                {
                    n = n.root as T;
                }
            }

            return n;
        }

        /// <summary>
        /// Builds graph defined by <c>path</c> or selecte existing graphnode, as pointed by path. Builds only one branch. Use <see cref="BuildGraphFromPaths{T}(IEnumerable{string}, string, bool, bool)"/> to construct complete graph from paths
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent">Parent graph.</param>
        /// <param name="path">Path to construct from.</param>
        /// <param name="isAbsolutePath">if set to <c>true</c> [is absolute path] (that starts with leading /)</param>
        /// <param name="splitter">The splitter - by default: directory separator.</param>
        /// <param name="returnHead">if set to <c>true</c> if will return the created node</param>
        /// <returns>
        /// Leaf instance
        /// </returns>
        public static T ConvertPathToGraph<T>(this T parent, String path, Boolean isAbsolutePath = true, String splitter = "", Boolean returnHead = true) where T :  class, IGraphNode, new()
        {
            if (splitter == "") splitter = System.IO.Path.DirectorySeparatorChar.ToString();

            if (isAbsolutePath)
            {
                if (parent != null)
                {
                    if (!path.StartsWith(parent.path))
                    {
                        return parent;
                    } else
                    {
                        path = path.removeStartsWith(parent.path);
                    }
                }
            }

            List<string> pathParts = imbSciStringExtensions.SplitSmart(path, splitter);

            IGraphNode head = parent;

            foreach (string part in pathParts)
            {
                if (head == null)
                {
                    parent = new T();
                    parent.name = part;
                    head = parent;
                }
                else
                {
                    if (head.ContainsKey(part))
                    {
                        head = head[part];
                    }
                    else
                    {
                        T sp = new T();
                        sp.name = part;
                        if (head.Add(sp))
                        {
                            head = sp;
                        }; //.Add(part, CAPTION_FOR_TUNNELFOLDER, CAPTION_FOR_TUNNELFOLDER);
                    }
                }
            }
            if (head == null)
            {
                return null;
            }
            if (returnHead)
            {
                return (T)head;
            }
            else
            {
                return (T)head.root;
            }
        }

        /// <summary>
        /// Gets first level parents of the source collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keepRootsFromSource">if set to <c>true</c>: the output will also contain any root (node without parent) node from the specified source collection.</param>
        /// <returns></returns>
        public static List<T> GetParents<T>(this IEnumerable<T> source, Boolean keepRootsFromSource = false) where T : class, IGraphNode
        {
            List<T> output = new List<T>();
            foreach (T t in source)
            {
                if (t.parent != null)
                {
                    if (!output.Contains(t.parent as T))
                    {
                        output.Add(t.parent as T);
                    }
                }
                else
                {
                    if (keepRootsFromSource) output.Add(t);
                }
            }
            return output;
        }

        /// <summary>
        /// Gets the first parent matching criteria.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="skipSelf">if set to <c>true</c> if will not evaluate <c>source</c> node</param>
        /// <param name="returnLastHead">if set to <c>true</c> it will return the node whose parent met the <c>criteria</c></param>
        /// <returns></returns>
        public static T GetFirstParent<T>(this T source, Func<T, Boolean> criteria, Boolean skipSelf=true, Boolean returnLastHead=false) where T : class, IGraphNode
        {
            T head = source;
            T lastHead = source;
            while (head != null)
            {
                Boolean eval = true;
                

                if (skipSelf)
                {
                    if (head == source) eval = false;
                }
                if (eval)
                {
                    if (head is T headT)
                    {
                        if (criteria(headT))
                        {
                            if (returnLastHead)
                            {
                                return lastHead;
                            } else
                            {
                                return headT;
                            }
                            
                        }
                    }
                }
                lastHead = head;
                head = head.parent as T;

            }
            return null;
        }

        /// <summary>
        /// Collects all leaf nodes from the <c>parent</c> node, and pack it into graphNodeSet.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent">The parent.</param>
        /// <param name="leafNames">The leaf names.</param>
        /// <param name="min">The minimum.</param>
        /// <returns></returns>
        public static graphNodeSet GetNodeSetWithLeafs<T>(this T parent, List<String> leafNames, Int32 min = -1) where T : class, IGraphNode
        {
            var leafList = parent.getAllLeafs().Where(x => leafNames.Any(y => x.name.Contains(y)));

            graphNodeSet nodeSet = new graphNodeSet(parent);
            foreach (IGraphNode g in leafList)
            {
                nodeSet.Add(g);
            }

            if (min > -1)
            {
                if (nodeSet.Count() >= min)
                {
                    return nodeSet;
                }
                else
                {
                    return null;
                }
            }

            return nodeSet;
        }

        /// <summary>
        /// Iterative procedure, returning <see cref="graphNodeSetCollection"/> with <see cref="graphNodeSet"/>s rooted at node that has leafs (all or <c>min)</c> with <c>leafNames</c>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source - starting leaf or other branch nodes.</param>
        /// <param name="leafNames">The leaf names.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="maxIterations">The maximum iterations.</param>
        /// <param name="extraIterations">The extra iterations.</param>
        /// <returns></returns>
        public static graphNodeSetCollection GetFirstNodeWithLeafs<T>(this IEnumerable<T> source, List<String> leafNames, Int32 min = -1, Int32 maxIterations = 50, Int32 extraIterations = 5) where T : class, IGraphNode, new()
        {
            graphNodeSetCollection output = new graphNodeSetCollection();

            //var parents = source.GetParents<T>(false);
            List<T> newSource = new List<T>();

            Int32 extraIndex = 0;
            if (min == -1) min = leafNames.Count();

            Int32 c = 0;

            for (int i = 0; i < (maxIterations + extraIterations); i++)
            {
                c = source.Count();

                newSource = new List<T>();

                foreach (var parent in source)
                {
                    if (!parent.isLeaf)
                    {
                        var nSet = parent.GetNodeSetWithLeafs<T>(leafNames, min);
                        if (nSet != null)
                        {
                            output.Add(nSet);
                        }
                        else
                        {
                            if (parent.parent != null)
                            {
                                newSource.Add(parent.parent as T);
                            }
                        }
                    }
                    else
                    {
                        if (parent.parent != null)
                        {
                            newSource.Add(parent.parent as T);
                        }
                    }
                }

                source = newSource;

                if (source.Count() == c)
                {
                    extraIndex++;
                }
                else
                {
                    extraIndex = 0;
                }

                if (extraIndex > extraIterations)
                {
                    i = maxIterations;
                    extraIndex = 0;
                }

                if (!source.Any()) return output;
            }

            return output;
        }

        /// <summary>
        /// Gets all roots.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="maxIterations">The maximum iterations.</param>
        /// <param name="extraIterations">The extra iterations.</param>
        /// <returns></returns>
        public static List<T> GetAllRoots<T>(this IEnumerable<T> source, Int32 maxIterations = 50, Int32 extraIterations = 2, Int32 targetCount = 1) where T : class, IGraphNode
        {
            List<T> output = new List<T>();

            Int32 extraIndex = 0;

            for (int i = 0; i < (maxIterations + extraIterations); i++)
            {
                Int32 c = source.Count();

                output = source.GetParents(true);

                if (output.Count() <= targetCount)
                {
                    return output;
                }

                source = output;

                if (source.Count() == c)
                {
                    extraIndex++;
                }
                else
                {
                    extraIndex = 0;
                }

                if (extraIndex > extraIterations)
                {
                    i = maxIterations;
                    extraIndex = 0;
                }
            }

            return output;
        }

        /// <summary>
        /// Makes the unique name for a child, based on proposal and counter, formatted by limit digit width: e.g. if <c>limit</c> is 100, format is: D3, producing: <c>proposal</c>+001, +002, +003...
        /// </summary>
        /// <param name="parent">The parent for whom the child name is made</param>
        /// <param name="proposal">The proposal form, neither it already exist or not</param>
        /// <param name="limit">The limit: number of cycles to terminate the search</param>
        /// <param name="toSkip">To skip.</param>
        /// <param name="addNumberSufixForFirst">if set to <c>true</c> it adds number sufix even if it is the first child with proposed name</param>
        /// <returns>
        /// Unique name for new child in format: <c>proposal</c>001 up to <c>limit</c>
        /// </returns>
        public static String MakeUniqueChildName(this IGraphNode parent, String proposal, Int32 limit = 999, Int32 toSkip = 0, Boolean addNumberSufixForFirst = true)
        {
            String originalProposal = proposal;
            if (originalProposal == null) originalProposal = "G";

            limit = limit + toSkip;

            String format = "D" + limit.ToString().Length.ToString();

            Int32 c = toSkip;

            if (addNumberSufixForFirst)
            {
                c++;
                proposal = originalProposal + c.ToString(format);
            }

            while (parent.ContainsKey(proposal))
            {
                c++;
                proposal = originalProposal + c.ToString(format);
                if (c > limit)
                {
                    break;
                }
            }

            return proposal;
        }


        /// <summary>
        /// Traverses forward (from target to leaf) until reaches: leaf, a junction or exceeds <c>stepLimit</c>
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="stepLimit">The step limit.</param>
        /// <returns></returns>
        public static IGraphNode GetJunctionOrLeaf(this IGraphNode target, Int32 stepLimit=50)
        {
            IGraphNode head = target;
            Boolean traverse = true;
            Int32 i = 0;
            while (traverse)
            {
                var childNames = head.getChildNames();
                if (childNames.Count == 0)
                {
                    return head;
                }
                if (childNames.Count > 1)
                {
                    return head;
                }
                if (childNames.Count == 1)
                {
                    head = head[childNames.First()];
                    i++;
                }
                if (i > stepLimit)
                {
                    traverse = false;
                }
            }
            return head;
        }


        /// <summary>
        /// Determines if from <c>target</c> to leaf, there are no junctions - but direct branch, leading to leaf
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="stepLimit">The step limit.</param>
        /// <returns>
        ///   <c>true</c> if [is branch to leaf] [the specified step limit]; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsBranchToLeaf(this IGraphNode target, Int32 stepLimit=50)
        {
            var result = target.GetJunctionOrLeaf(stepLimit);
            var childNames = result.getChildNames();
            if (childNames.Count == 0)
            {
                return true;
            } else
            {
                return false;
            }
        }


        /// <summary>
        /// If the graph <c>target</c> has parent, it will move to its level (one level closer to the root)
        /// </summary>
        /// <param name="target">The graph that is moving</param>
        /// <param name="flags">The operation flags</param>
        /// <returns></returns>
        public static IGraphNode RetreatToParent(this IGraphNode target, graphOperationFlag flags = graphOperationFlag.mergeOnSameName)
        {
            if (target.parent != null)
            {
                return target.MoveTo(target.parent, flags);
            }
            return target;
        }

        /// <summary>
        /// Moves to new parent node <c>moveInto</c>
        /// </summary>
        /// <param name="graphToMove">The graph to move.</param>
        /// <param name="moveInto">The move into (future parent graph)</param>
        /// <param name="flags">The operation flags</param>
        /// <returns>Resulting graph, relevant in case of merging</returns>
        public static IGraphNode MoveTo(this IGraphNode graphToMove, IGraphNode moveInto, graphOperationFlag flags = graphOperationFlag.mergeOnSameName)
        {
            if (moveInto.getChildNames().Contains(graphToMove.name))
            {
                if (flags.HasFlag(graphOperationFlag.mergeOnSameName))
                {
                    graphToMove.MergeWith(moveInto[graphToMove.name], flags);
                }
                else if (flags.HasFlag(graphOperationFlag.overwriteOnSameName))
                {
                    moveInto.RemoveByKey(graphToMove.name);
                    moveInto.Add(graphToMove);
                }
            }
            else
            {
                moveInto.Add(graphToMove);
            }
            return graphToMove;
        }

        /// <summary>
        /// Graph node <c>graphToMerge</c> transfers all child nodes to <c>graphToMergeWith</c> and disconnects from its parent
        /// </summary>
        /// <param name="graphToMerge">The graph to merge.</param>
        /// <param name="graphToMergeWith">The graph to merge with.</param>
        /// <param name="flags">The operation flags</param>
        /// <returns></returns>
        public static IGraphNode MergeWith(this IGraphNode graphToMerge, IGraphNode graphToMergeWith, graphOperationFlag flags = graphOperationFlag.mergeOnSameName)
        {
            foreach (IGraphNode child in graphToMerge)
            {
                child.MoveTo(graphToMergeWith, flags);
            }

            graphToMerge.parent.RemoveByKey(graphToMerge.name);

            return graphToMergeWith;
        }
    }
}