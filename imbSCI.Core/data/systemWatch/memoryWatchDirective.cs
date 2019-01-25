using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.Core.data.systemWatch
{
/// <summary>
    /// Directive that <see cref="MemoryWatch"/> produces
    /// </summary>
    public enum MemoryWatchDirective
    {
        /// <summary>
        /// Used memory is below allowed limit, therefore continue normal operation
        /// </summary>
        normal,
        /// <summary>
        /// The program should stop consuming additional memory
        /// </summary>
        prevent,
        /// <summary>
        /// The program should flush cached data in order to release memory
        /// </summary>
        flush
    }
}