using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Core.reporting.zone;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.SourceData.Analysis
{
    [Flags]
    public enum CellContentType
    {
        unknown = 0,
        empty = 1,
        numeric = 2,

        textual = 4,

        mixed = 8 | textual,

        withComma = 16,

        withDot = 32,

        numericWithComma = numeric | withComma,
        numericWithDot = numeric | withDot,

        numericWithCommaOrDot = numeric | withComma | withDot,

        withMinus = 64,
        withSpace = 128,
        withPlus = 256,
        withPercentage = 512,
        Any=1028,
        date = 2048,
        bank_account = 4096,
        formatted = 8192,

        date_format = date | formatted,
        bank_account_number = bank_account | formatted 
    }
}