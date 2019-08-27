namespace imbSCI.DataExtraction.Xml
{
    /// <summary>
    /// Načini prikupljanja relativnih nodova
    /// </summary>
    public enum collectRelatives
    {
        /// <summary>
        /// Samo krajnje nodove vraća
        /// </summary>
        endNodes,

        /// <summary>
        /// Nodove do određenog nivoa
        /// </summary>
        upToLevel,

        /// <summary>
        /// Nodove od kraja u nazad
        /// </summary>
        downToLevel,

        /// <summary>
        /// Nodove na određenom nivou
        /// </summary>
        onLevel,

        /// <summary>
        /// Direktnu decu
        /// </summary>
        childs,

        /// <summary>
        /// Bratske nodove (isti parent)
        /// </summary>
        brothers,

        /// <summary>
        /// Roditelje do određene dubine
        /// </summary>
        parentNodes,

        /// <summary>
        /// Sve rođake u nazad do određene dubine
        /// </summary>
        relatives,
    }
}