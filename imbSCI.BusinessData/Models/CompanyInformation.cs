using imbSCI.BusinessData.Core;
using imbSCI.BusinessData.Metrics;
using imbSCI.BusinessData.Models.BankAccount;
using imbSCI.BusinessData.Models.Company;
using imbSCI.BusinessData.Models.Organization;
using imbSCI.Core.math.timeseries;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace imbSCI.BusinessData.Models
{
/// <summary>
    /// Basic company information
    /// </summary>
    /// <seealso cref="imbSCI.BusinessData.Models.CompanyIdentificationCore" />
    /// <seealso cref="imbSCI.BusinessData.Core.IRecordWithGetUID" />
    public class CompanyInformation : CompanyIdentification
    {
        public CompanyInformation()
        {
        }

        //public CompanyIdentification Identification { get; set; } = new CompanyIdentification();

        public PostAddress PrimaryAddress { get; set; } = new PostAddress();

        public ListFromString<WebSite> Web { get; set; } = new ListFromString<WebSite>();

        public ListFromString<EmailAddress> Email { get; set; } = new ListFromString<EmailAddress>();

        public CompanySize Size { get; set; } = CompanySize.Unknown;

        public bankAccountList bankAccounts { get; set; } = new bankAccountList();

        public PhoneLineList PhoneLines { get; set; } = new PhoneLineList();

        public BusinessActivityList Activity { get; set; } = new BusinessActivityList();

        public DateTime FoundationDate { get; set; }

    }
}