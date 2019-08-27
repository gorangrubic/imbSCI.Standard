using System;

namespace imbSCI.BusinessData.Metrics
{
    /// <summary>
    /// Direction of international trade
    /// </summary>
    [Flags]
    public enum InternationalTradeDirection
    {
        /// <summary>
        /// Never specified
        /// </summary>
        unknown = 0,

        /// <summary>
        /// Imports
        /// </summary>
        import = 1,

        /// <summary>
        /// The export
        /// </summary>
        export = 2,

        /// <summary>
        /// The combined: <see cref="import"/> and <see cref="export"/>
        /// </summary>
        combined = import | export
    }
}