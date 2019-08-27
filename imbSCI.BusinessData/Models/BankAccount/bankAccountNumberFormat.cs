namespace imbSCI.BusinessData.Models.BankAccount
{
    public enum bankAccountNumberFormat
    {
        // format is undefined, try
        unknown,

        // 340-1100291591
        singleSeparation,

        // 340-11002915-91
        doubleSeparation,

        // 3401100291591
        noSeparation,
    }
}