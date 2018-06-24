namespace imbSCI.Data.collection.graph
{
    /// <summary>
    /// Kind of graph structural change, that was observed during analysis
    /// </summary>
    public enum graphChangeType
    {
        unknown,

        /// <summary>
        /// Graph node was removed
        /// </summary>
        removed,

        /// <summary>
        /// Graph node was moved
        /// </summary>
        moved,

        /// <summary>
        /// Graph node is added
        /// </summary>
        added,

        /// <summary>
        /// Nothing changed
        /// </summary>
        noChange,

        /// <summary>
        /// The node has more children then before
        /// </summary>
        expanded,

        /// <summary>
        /// The node lost some children
        /// </summary>
        contracted
    }
}