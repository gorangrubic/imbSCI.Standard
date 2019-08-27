using imbSCI.BusinessData.Core;
using System;

namespace imbSCI.BusinessData.Models
{
    /// <summary>
    /// Data structure providing the most fundamental (core) business entity identification data
    /// </summary>
    public class CompanyIdentificationCore : IRecordWithGetUID
    {
        public CompanyIdentificationCore()
        {
        }

        /// <summary>
        /// VAT identification number, considered as UID in this library
        /// </summary>
        /// <value>
        /// The vat.
        /// </value>
        public String VAT { get; set; } = "";

        /// <summary>
        /// Unique ID used by an external business software package (i.e. SugarCRM)
        /// </summary>
        /// <value>
        /// The uid.
        /// </value>
        public String UID { get; set; } = "";

        public string GetUID()
        {
            return VAT;
        }
    }
}