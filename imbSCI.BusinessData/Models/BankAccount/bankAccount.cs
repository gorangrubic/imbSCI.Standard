using imbSCI.BusinessData.Core;
using System;

namespace imbSCI.BusinessData.Models.BankAccount
{
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'System.IEquatable{imbSCI.BusinessData.Models.BankAccount.bankAccount}'
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    /// <summary>
    /// Data on a bank account
    /// </summary>
    /// <seealso cref="imbSCI.BusinessData.Core.IRecordWithGetUID" />
    /// <seealso cref="System.IEquatable{imbSCI.BusinessData.Models.BankAccount.bankAccount}" />
    public class bankAccount : IRecordWithGetUID, IEquatable<bankAccount>
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'System.IEquatable{imbSCI.BusinessData.Models.BankAccount.bankAccount}'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'bankAccount.bankAccount()'
        public bankAccount()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'bankAccount.bankAccount()'
        {
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'bankAccount.AccountType'
        public bankAccountType AccountType { get; set; } = bankAccountType.DomesticCurrency;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'bankAccount.AccountType'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'bankAccount.AccountNumber'
        public bankAccountNumber AccountNumber { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'bankAccount.AccountNumber'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'bankAccount.CreationDate'
        public DateTime CreationDate { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'bankAccount.CreationDate'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'bankAccount.TerminationDate'
        public DateTime TerminationDate { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'bankAccount.TerminationDate'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'bankAccount.CanBeBlocked'
        public Boolean CanBeBlocked { get; set; } = true;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'bankAccount.CanBeBlocked'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'bankAccount.Equals(bankAccount)'
        public bool Equals(bankAccount other)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'bankAccount.Equals(bankAccount)'
        {
            return AccountNumber.Equals(other.AccountNumber);  //GetUID().Equals(other.GetUID());
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'bankAccount.GetUID()'
        public string GetUID()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'bankAccount.GetUID()'
        {
            if (AccountNumber != null)
            {
                return AccountNumber.ToString(true, bankAccountNumberFormat.doubleSeparation);
            }
            else
            {
                return "";
            }
        }
    }
}