namespace imbSCI.DataExtraction.Xml
{
    /// <summary>
    /// Mod xpath Debug mehanizma
    /// </summary>
    public enum xpathDebugMode
    {
        /// <summary>
        /// Ne obaveštava o izvršavanju xPath-a
        /// </summary>
        disabled,

        /// <summary>
        /// Obaveštava samo ako je Null Result
        /// </summary>
        onNullResult,

        /// <summary>
        /// Obaveštava u svakom slučaju
        /// </summary>
        fullDebug,
    }
}