
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbSCI.Data.collection
{

    /// <summary>
    /// Describes relationships between instances in the <see cref="SpaceModel"/>
    /// </summary>
    /// <typeparam name="TNodeA">The type of the node a.</typeparam>
    /// <typeparam name="TNodeB">The type of the node b.</typeparam>
    public class Relationships<TNodeA, TNodeB> where TNodeA : IObjectWithName
    where TNodeB : IObjectWithName
    {



        /// <summary>
        /// The links
        /// </summary>
        public List<Relationship<TNodeA, TNodeB>> links = new List<Relationship<TNodeA, TNodeB>>();

        /// <summary>
        /// Adds the specified a.
        /// </summary>
        /// <param name="A">a.</param>
        /// <param name="B">The b.</param>
        /// <param name="weight">The weight.</param>
        /// <returns></returns>
        public Relationship<TNodeA, TNodeB> Add(TNodeA A, TNodeB B, double weight)
        {
            Relationship<TNodeA, TNodeB> output = new Relationship<TNodeA, TNodeB>();
            output.NodeA = A;
            output.NodeB = B;
            output.weight = weight;
            links.Add(output);
            return output;
        }

        /// <summary>
        /// Gets all linked.
        /// </summary>
        /// <param name="nodeA">The node a.</param>
        /// <returns></returns>
        public List<TNodeB> GetAllLinkedB(String nodeName, Boolean OnlyUnique = true)
        {
            List<TNodeB> output = new List<TNodeB>();

            foreach (Relationship<TNodeA, TNodeB> link in links)
            {
                if (link.NodeA.name == nodeName)
                {
                    if (OnlyUnique)
                    {
                        if (!output.Contains(link.NodeB))
                        {
                            output.Add(link.NodeB);
                        }
                    }
                    else
                    {
                        output.Add(link.NodeB);
                    }

                    // output.Add(link.NodeB);
                }
            }

            return output;
        }

        /// <summary>
        /// Gets all linked.
        /// </summary>
        /// <param name="nodeA">The node a.</param>
        /// <returns></returns>
        public List<TNodeA> GetAllLinkedA(String nodeName, Boolean OnlyUnique = true)
        {
            List<TNodeA> output = new List<TNodeA>();

            foreach (Relationship<TNodeA, TNodeB> link in links)
            {
                if (link.NodeB.name == nodeName)
                {
                    if (OnlyUnique)
                    {
                        if (!output.Contains(link.NodeA))
                        {
                            output.Add(link.NodeA);
                        }
                    }
                    else
                    {
                        output.Add(link.NodeA);
                    }
                }

            }

            return output;
        }


        /// <summary>
        /// Gets all linked.
        /// </summary>
        /// <param name="nodeA">The node a.</param>
        /// <returns></returns>
        public List<TNodeB> GetAllLinked(TNodeA nodeA, Boolean OnlyUnique = true)
        {
            List<TNodeB> output = new List<TNodeB>();

            foreach (Relationship<TNodeA, TNodeB> link in links)
            {
                if (link.NodeA.name == nodeA.name)
                {
                    if (OnlyUnique)
                    {
                        if (!output.Contains(link.NodeB))
                        {
                            output.Add(link.NodeB);
                        }
                    }
                    else
                    {
                        output.Add(link.NodeB);
                    }

                }
            }

            return output;
        }

        /// <summary>
        /// Removes all links having specified node
        /// </summary>
        public void Remove(TNodeB vec)
        {
            var sel_links = links.Where(x => x.NodeB.name == vec.name).ToList();
            sel_links.ForEach(x => links.Remove(x));
        }

        /// <summary>
        /// Removes all links having specified node
        /// </summary>
        /// <param name="vec">The vec.</param>
        public void Remove(TNodeA vec)
        {
            var sel_links = links.Where(x => x.NodeA.name == vec.name).ToList();
            sel_links.ForEach(x => links.Remove(x));
        }

        /// <summary>
        /// Gets all nodes A linked to specified node B
        /// </summary>
        /// <param name="nodeB">The node b.</param>
        /// <returns></returns>
        public List<TNodeA> GetAllLinked(TNodeB nodeB, Boolean OnlyUnique = true)
        {
            List<TNodeA> output = new List<TNodeA>();

            foreach (Relationship<TNodeA, TNodeB> link in links)
            {
                if (link.NodeB.name == nodeB.name)
                {
                    if (OnlyUnique)
                    {
                        if (!output.Contains(link.NodeA))
                        {
                            output.Add(link.NodeA);
                        }
                    }
                    else
                    {
                        output.Add(link.NodeA);
                    }

                }
            }

            return output;
        }

        public List<String> GetAllDistinctNames(Boolean byNodeBName = false)
        {
            List<String> output = new List<string>();
            foreach (var l in links)
            {
                String k = l.NodeA.name;
                if (byNodeBName) k = l.NodeB.name;

                if (!output.Contains(k)) output.Add(k);
            }
            return output;
        }


        public Dictionary<String, List<String>> GetAllRelationShipByName(Boolean byNodeBName = false)
        {
            Dictionary<String, List<String>> output = new Dictionary<string, List<string>>();
            foreach (var l in links)
            {
                String k = l.NodeA.name;
                if (byNodeBName) k = l.NodeB.name;

                if (!output.ContainsKey(k)) output.Add(k, new List<string>());


                if (byNodeBName)
                {
                    output[k].Add(l.NodeB.name);
                }
                else
                {
                    output[k].Add(l.NodeA.name);
                }
            }
            return output;
        }

        public List<Relationship<TNodeA, TNodeB>> GetAllRelationships(TNodeA nodeA)
        {
            var relations = links.Where(x => x.NodeA.name == nodeA.name);
            return relations.ToList();
        }
    }

}