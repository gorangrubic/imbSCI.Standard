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
    public class CompanyIntelligence : CompanyIdentificationCore, IRecordWithGetUID
    {
        public CompanyIntelligence()
        {

        }

        [XmlIgnore]
        public CompanyInformation Company { get; set; } = new CompanyInformation();

        [XmlIgnore]
        public FinanceOverviewRecords FinanceRecords { get; set; } = new FinanceOverviewRecords();

        [XmlIgnore]
        public InternationalTradeRecords Imports { get; set; } = new InternationalTradeRecords();

        [XmlIgnore]
        public InternationalTradeRecords Exports { get; set; } = new InternationalTradeRecords();

        [XmlIgnore]
        public PersonCollection Persons { get; set; } = new PersonCollection();

    
    }
}