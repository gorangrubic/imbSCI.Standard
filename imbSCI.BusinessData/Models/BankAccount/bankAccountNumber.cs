using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.BusinessData.Models.BankAccount
{

    public static class bankAccountNumberTools
    {




        /// <summary>
        /// Regex select FORMAT_BANKACCOUNTNUMBER : \A([\d]{3})\-?([\d]{0,8})\-?([\d]{2})\Z
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_FORMAT_BANKACCOUNTNUMBER = new Regex(@"\A([\d]{3})\-?([\d]{0,8})\-?([\d]{2})\Z", RegexOptions.Compiled);

        /// <summary>
        /// Match Evaluation for FORMAT_BANKACCOUNTNUMBER : _select_FORMAT_BANKACCOUNTNUMBER
        /// </summary>
        /// <param name="m">Match with value to process</param>
        /// <returns>For m.value "something" returns "SOMETHING"</returns>
        private static String _replace_FORMAT_BANKACCOUNTNUMBER(Match m)
        {
            String output = m.Value.Replace(".", "");
            output = output.Replace(" ", "");

            return output.ToUpper();
        }


    }



    /// <summary>
    /// Object model for bank account number
    /// </summary>
    public class bankAccountNumber : IEquatable<bankAccountNumber>
    {
        public bankAccountNumber()
        {
        }

        public bankAccountNumber(String input, bankAccountNumberFormat format = bankAccountNumberFormat.unknown)
        {
            SetFromString(input, format);
        }

        public Int32 bankPrefix { get; set; } = 0;

        public UInt64 accountNumber { get; set; } = 0;

        public Int32 controlNumber { get; set; } = 0;

        protected String fullFormat { get; set; } = "";

        public void ToString(StringBuilder sb, Boolean useLeadingZeros = true, bankAccountNumberFormat mode = bankAccountNumberFormat.doubleSeparation)
        {
            if (accountNumber == 0)
            {
                return;
            }

            StringBuilder sent_sb = sb;

            if (useLeadingZeros && mode == bankAccountNumberFormat.doubleSeparation)
            {
                if (!fullFormat.isNullOrEmpty())
                {
                    sb.Append(fullFormat);
                    return;
                }
                sb = new StringBuilder();
            }

            sb.Append(bankPrefix.ToString("D3"));

            if (mode == bankAccountNumberFormat.singleSeparation || mode == bankAccountNumberFormat.doubleSeparation)
            {
                sb.Append("-");
            }

            if (useLeadingZeros)
            {
                sb.Append(accountNumber.ToString("D13"));
            }
            else
            {
                sb.Append(accountNumber.ToString());
            }

            if (mode == bankAccountNumberFormat.doubleSeparation)
            {
                sb.Append("-");
            }

            sb.Append(controlNumber.ToString("D2"));

            if (useLeadingZeros && mode == bankAccountNumberFormat.doubleSeparation)
            {
                fullFormat = sb.ToString();
                sent_sb.Append(fullFormat);
            }
        }

        /// <summary>
        /// To the string.
        /// </summary>
        /// <param name="useLeadingZeros">if set to <c>true</c> [use leading zeros].</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        public String ToString(Boolean useLeadingZeros, bankAccountNumberFormat mode, StringBuilder sb = null)
        {
            if (sb == null) sb = new StringBuilder();

            ToString(sb, useLeadingZeros, mode);

            return sb.ToString();
        }

        public void SetFromString(String input, bankAccountNumberFormat mode = bankAccountNumberFormat.doubleSeparation)
        {
            List<String> parts = new List<string>();
            String rightPart = "";
            String cntPart = "";

            if (input.Length < 6)
            {
                throw new ArgumentException("Input string [" + input + "] cant be interpreted as bank account number!");
            }

            switch (mode)
            {
                case bankAccountNumberFormat.doubleSeparation:
                    parts.AddRange(input.Split('-'));
                    break;

                case bankAccountNumberFormat.noSeparation:
                    parts.Add(input.Substring(0, 3));
                    rightPart = input.Substring(3);

                    cntPart = rightPart.Substring(rightPart.Length - 2);
                    parts.Add(rightPart.Substring(0, rightPart.Length - 2));
                    parts.Add(cntPart);
                    break;

                case bankAccountNumberFormat.singleSeparation:
                    var tmp = input.Split("- ".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                    parts.Add(tmp[0]);

                    rightPart = tmp[1];
                    cntPart = rightPart.Substring(rightPart.Length - 2);
                    parts.Add(rightPart.Substring(0, rightPart.Length - 2));

                    parts.Add(cntPart);
                    break;

                case bankAccountNumberFormat.unknown:
                    input = input.Replace("-", "");
                    input = input.Replace(" ", "");
                    SetFromString(input, bankAccountNumberFormat.noSeparation);
                    return;
                    break;
            }
            Int32 _tmp = 0;
            if (!Int32.TryParse(parts[0], out _tmp))
            {
                throw new ArgumentException("Input string [" + input + "] cant be interpreted as bank account number -- conversion to integer failed!");
            }
            else
            {
                bankPrefix = _tmp;
            }

            UInt64 _utmp = 0;

            if (!UInt64.TryParse(parts[1], out _utmp))
            {
                throw new ArgumentException("Input string [" + input + "] cant be interpreted as bank account number -- conversion to integer failed!");
            }
            else
            {
                accountNumber = _utmp;
            }

            if (!Int32.TryParse(parts[2], out _tmp))
            {
                throw new ArgumentException("Input string [" + input + "] cant be interpreted as bank account number -- conversion to integer failed!");
            }
            else
            {
                controlNumber = _tmp;
            }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(bankAccountNumber other)
        {
            if (bankPrefix == other.bankPrefix)
            {
                if (accountNumber == other.accountNumber)
                {
                    if (controlNumber == other.controlNumber)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}