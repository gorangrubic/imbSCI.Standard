using imbSCI.BusinessData.Metrics;
using imbSCI.BusinessData.Models;
using imbSCI.BusinessData.Models.BankAccount;
using imbSCI.BusinessData.Models.Company;
using imbSCI.BusinessData.Models.Organization;
using imbSCI.BusinessData.Storage;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting.render;
using System;

namespace imbSCI.BusinessData.Intelligence
{
    public class IntelligenceRecordProvider
    {
        public void Init(folderNode _rootFolder, ITextRender logger)
        {
            storageFolder = _rootFolder;

            Finance = new FileSystemRecordProvider<FinanceOverviewRecords, FinanceOverview>(storageFolder.Add(nameof(Finance), nameof(Finance), "Records storage"), "fin_", "fin_");

            InternationalTrade = new FileSystemRecordProvider<InternationalTradeRecords, InternationalTradeByCountry>(storageFolder.Add(nameof(InternationalTrade), nameof(InternationalTrade), "Records storage"), "int_", "int_");

            BankAccounts = new FileSystemRecordProvider<CompanyBankAccountCollection, bankAccount>(
                storageFolder.Add(nameof(BankAccounts), nameof(BankAccounts), "Records storage"), "ban_", "ban_");

            Persons = new FileSystemRecordProvider<CompanyPersonCollection, Person>(
                storageFolder.Add(nameof(Persons), nameof(Persons), "Records storage"), "per_", "per_");

            Companies = new FileSystemRecordProvider<CompanyInformationCollection, CompanyInformation>(storageFolder, "acc_", "acc_");
            Companies.OperationMode = RecordProviderOperationMode.singleCollectionMode;
        }

        public FileSystemRecordProvider<FinanceOverviewRecords, FinanceOverview> Finance { get; set; }

        public FileSystemRecordProvider<InternationalTradeRecords, InternationalTradeByCountry> InternationalTrade { get; set; }

        public FileSystemRecordProvider<CompanyBankAccountCollection, bankAccount> BankAccounts { get; set; }

        public FileSystemRecordProvider<CompanyPersonCollection, Person> Persons { get; set; }

        public FileSystemRecordProvider<CompanyInformationCollection, CompanyInformation> Companies { get; set; }

        protected folderNode storageFolder { get; set; }

        public FinanceOverview GetFinanceOverview(String VAT, Int32 year)
        {
            var response = Finance.GetRecord(VAT, year.ToString());

            return response.record as FinanceOverview;
        }

        public InternationalTradeRecords GetTradeRecords(String VAT, Int32 year)
        {
            var response = InternationalTrade.GetRecord(VAT, year.ToString());

            return response.record as InternationalTradeRecords;
        }
    }
}