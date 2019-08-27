using imbSCI.Core.extensions.text;
using imbSCI.Data;
using System;
using System.Data;
using System.Globalization;

namespace imbSCI.DataComplex.converters
{
    public static class StringValueConverters
    {
        public static String GetStringValue(this DataRow dataRow, String columnName, String defaultValue = "")
        {
            if (!dataRow.Table.Columns.Contains(columnName))
            {
                return defaultValue;
            }

            return dataRow[columnName].toStringSafe(defaultValue);

        }



        public static Int32 GetIntegerValue(this DataRow dataRow, String columnName, Int32 defaultValue = 0)
        {
            if (!dataRow.Table.Columns.Contains(columnName))
            {
                return defaultValue;
            }

            return GetIntegerValue(dataRow[columnName].toStringSafe(defaultValue.ToString()));

        }

        public static Int32 GetIntegerValue(String numeric, string groupSeparator = "")
        {
            Int32 output = 0;

            if (numeric is null) return output;

            numeric = numeric.Trim();

            if (numeric.isNullOrEmpty())
            {
                return output;
            }
            else
            {
                groupSeparator = groupSeparator.or(CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator);

                numeric = numeric.Replace(groupSeparator, "");

                // numeric = numeric.Replace(inputDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);

                Int32.TryParse(numeric, out output);
                //output = Int32.Parse(numeric);
            }

            return output;
        }


        public static DateTime GetDateValue(this DataRow dataRow, String columnName, String format = "dd.MM.yyyy")
        {
            if (!dataRow.Table.Columns.Contains(columnName))
            {
                return DateTime.Now;
            }
            String dateString = dataRow[columnName].toStringSafe("");

            return GetDate(dateString, format);

        }

        public static DateTime GetDate(String numeric, String format = "dd.MM.yyyy")
        {
            if (numeric.isNullOrEmpty())
            {
                return DateTime.MinValue;
            }


            DateTime output = DateTime.ParseExact(numeric, format, CultureInfo.InvariantCulture);

            return output;

        }

        public static DateTime GetDate(this DataRow dataRow, String columnName, String format = "dd.MM.yyyy")
        {
            if (!dataRow.Table.Columns.Contains(columnName))
            {
                return DateTime.Now;
            }
            String dateString = dataRow[columnName].toStringSafe("");

            return GetDate(dateString, format);

        }

        public static Decimal GetDecimalValue(this DataRow dataRow, String columnName, String decSeparator = "", String groupSeparator = "", Decimal defaultValue = 0)
        {
            if (!dataRow.Table.Columns.Contains(columnName))
            {
                return defaultValue;
            }
            String dateString = dataRow[columnName].toStringSafe("");

            return GetDecimalValue(dateString, decSeparator, groupSeparator);
        }

        public static Decimal GetDecimalValue(String numeric, String decSeparator = "", String groupSeparator = "")
        {
            // String numeric = dr[nameof(transactionFieldName.ZNESEK_V_BREME)].toStringSafe("0,0");
            decSeparator = decSeparator.or(CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);

            groupSeparator = groupSeparator.or(CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator);



            Decimal output = 0;

            if (numeric.isNullOrEmpty())
            {
                return output;
            }
            else
            {
                numeric = numeric.Replace(groupSeparator, "");

                numeric = numeric.Replace(decSeparator, CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);

                output = Decimal.Parse(numeric);
            }

            return output;
        }

    }
}