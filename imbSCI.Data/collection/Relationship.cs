
using imbSCI.Data.interfaces;
using System;

namespace imbSCI.Data.collection
{


    /// <summary>
    /// Generic class for single relationship definition
    /// </summary>
    /// <typeparam name="TNodeA">The type of the node a.</typeparam>
    /// <typeparam name="TNodeB">The type of the node b.</typeparam>
    public class Relationship<TNodeA, TNodeB>
    where TNodeA : IObjectWithName
    where TNodeB : IObjectWithName
    {
        /// <summary>
        /// Gets or sets the node a.
        /// </summary>
        /// <value>
        /// The node a.
        /// </value>
        public TNodeA NodeA { get; set; }
        /// <summary>
        /// Gets or sets the node b.
        /// </summary>
        /// <value>
        /// The node b.
        /// </value>
        public TNodeB NodeB { get; set; }
        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public double weight { get; set; } = 1;
    }

}