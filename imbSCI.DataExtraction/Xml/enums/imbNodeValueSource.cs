namespace imbSCI.DataExtraction.Xml.enums
{
    /// <summary>
    /// Koji podatak vadi iz XML Nodea
    /// </summary>
    public enum imbNodeValueSource
    {
        /// <summary>
        /// Ime HTML taga
        /// </summary>
        tagName,

        /// <summary>
        /// Unutra�nji tekst
        /// </summary>
        innerText,

        innerXML,

        /// <summary>
        /// Spolja�nji tekst
        /// </summary>
        outerText,

        /// <summary>
        /// Sav tekst iz nodea
        /// </summary>
        allInnerText,

        /// <summary>
        /// Linija sa atributima
        /// </summary>
        attLine,

        hasChildren,
        hasRelatives,
        nspace,
        count,
        attToCsv,
        attToDList,
        xPath,
        index,

        /// <summary>
        /// Konvertuje u JSON objekat
        /// </summary>
        JSON,

        /// <summary>
        /// Konvertuje u MPS zapis
        /// </summary>
        MPS,

        valueNotNull,
    }
}