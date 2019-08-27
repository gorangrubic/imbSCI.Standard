using imbSCI.Core.reporting.render;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.BusinessData.Models.BankAccount
{
    public class bankAccountNumberList : List<bankAccountNumber>
    {
        public bankAccountNumberList()
        {
        }

        /// <summary>
        /// To the string.
        /// </summary>
        /// <param name="useLeadingZeros">if set to <c>true</c> [use leading zeros].</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        public String ToString(Boolean useLeadingZeros = true, bankAccountNumberFormat mode = bankAccountNumberFormat.doubleSeparation)
        {
            var sb = new StringBuilder();

            foreach (var ban in this)
            {
                if (ban.accountNumber != 0)
                {
                    ban.ToString(sb, useLeadingZeros, mode);
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Adds the entry
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="onlyUnique">if set to <c>true</c> if will add only if it does not already exist</param>
        public void AddEntry(bankAccountNumber entry, Boolean onlyUnique)
        {
            if (onlyUnique)
            {
                if (Contains(entry))
                {
                    return;
                }
            }

            Add(entry);
        }

        public void AddEntry(String ban, bankAccountNumberFormat mode = bankAccountNumberFormat.doubleSeparation, Boolean onlyUnique = true)
        {
            bankAccountNumber ban_instance = new bankAccountNumber();
            ban_instance.SetFromString(ban, mode);

            AddEntry(ban_instance, onlyUnique);
        }

        /// <summary>
        /// Used to check if string input line contains non numeric characters.
        /// </summary>
        /// <value>
        /// The non numeric characters.
        /// </value>
        public static Regex nonNumericCharacters { get; set; } = new Regex(@"([^\d]+)");

        public const Int32 digits = 18;

        /// <summary>
        /// REturns count of phrased bank accounts
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <returns></returns>
        public Int32 FromString(String input, Boolean onlyUnique = true, ITextRender output = null)
        {
            if (input.isNullOrEmpty()) return 0;

            Int32 c_old = this.Count;

            input = input.Replace("-", "");
            input = input.Replace(" ", "");

            var lines = input.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries);

            if (input.Length > digits)
            {
                Int32 mod = input.Length % digits;
                Int32 cnt = input.Length / digits;
                if (mod == 0)
                {
                    if (lines.Length == 1)
                    {
                        List<String> splicedLines = new List<string>();

                        for (int i = 0; i < cnt; i++)
                        {
                            Int32 index = i * digits;
                            splicedLines.Add(input.Substring(index, digits));
                        }

                        lines = splicedLines.ToArray();
                    }
                }
            }

            foreach (string ln in lines)
            {
                String lnt = ln.Trim();
                if (lnt.isNullOrEmpty()) continue;
                if (lnt.Length < 6) continue;

                if (nonNumericCharacters.IsMatch(lnt))
                {
                    if (output != null) output.AppendLine("Line: [" + lnt + "] contains letters. Ignored.");
                    continue;
                }

                var entry = new bankAccountNumber(lnt);
                AddEntry(entry, onlyUnique);
            }

            return Count - c_old;
        }
    }
}