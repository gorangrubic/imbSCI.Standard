using imbSCI.BusinessData.Models.BankAccount;
using imbSCI.BusinessData.Models.Core;

namespace imbSCI.BusinessData.Models.Company
{
    /// <summary>
    /// Set of bank accounts
    /// </summary>
    /// <seealso cref="imbSCI.BusinessData.Models.Core.ModelRecordsCollectionBase{imbSCI.BusinessData.Models.BankAccount.bankAccount}" />
    public class CompanyBankAccountCollection : ModelRecordsCollectionBase<bankAccount>
    {
        public CompanyBankAccountCollection()
        {
        }
    }
}