namespace imbSCI.DataExtraction.Analyzers.Similarity.similarity
{
    /// <summary>
    /// Mode of word decomposition
    /// </summary>
    public enum nGramsModeEnum
    {
        /// <summary>
        /// The overlap mode: e.g. for "category", N=2 it will produce: ca, at, te, eg, go, or, ry
        /// </summary>
        overlap,

        /// <summary>
        /// The ordinal mode: e.g. for "category", N=2 it will produce: ca, te, go, ry
        /// </summary>
        ordinal,
    }
}