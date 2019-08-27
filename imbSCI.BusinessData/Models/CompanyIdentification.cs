using imbSCI.BusinessData.Models.Organization;
using System;

namespace imbSCI.BusinessData.Models
{
    /// <summary>
    /// Elementary information used to identify a company (for query/search purposes)
    /// </summary>
    /// <seealso cref="imbSCI.BusinessData.Models.CompanyIdentificationCore" />
    public class CompanyIdentification : CompanyIdentificationCore
    {
        public CompanyIdentification()
        {
        }

        public String CompanyName { get; set; } = "";

        public String Place { get; set; } = "";

        public String RegistrationNumber { get; set; } = "";

        
    }
}