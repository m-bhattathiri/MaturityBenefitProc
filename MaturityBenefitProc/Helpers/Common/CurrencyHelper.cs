using System;

namespace MaturityBenefitProc.Helpers.Common
{
    public static class CurrencyHelper
    {
        public static decimal RoundToNearestPaisa(decimal amount)
        {
            return Math.Round(amount, 2, MidpointRounding.AwayFromZero);
        }

        public static string NumberToWords(decimal amount)
        {
            if (amount == 0) return "Zero";

            long wholePart = (long)Math.Floor(Math.Abs(amount));
            int paisaPart = (int)((Math.Abs(amount) - wholePart) * 100);

            string result = string.Empty;
            if (amount < 0) result = "Minus ";

            if (wholePart == 0)
            {
                result += "Zero";
            }
            else
            {
                result += ConvertWholeNumberToWords(wholePart);
            }

            result += " Rupees";

            if (paisaPart > 0)
            {
                result += " and " + ConvertWholeNumberToWords(paisaPart) + " Paise";
            }

            return result;
        }

        private static string ConvertWholeNumberToWords(long number)
        {
            if (number == 0) return "Zero";

            string[] ones = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine",
                              "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
                              "Seventeen", "Eighteen", "Nineteen" };
            string[] tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            string words = string.Empty;

            if (number >= 10000000)
            {
                words += ConvertWholeNumberToWords(number / 10000000) + " Crore ";
                number %= 10000000;
            }
            if (number >= 100000)
            {
                words += ConvertWholeNumberToWords(number / 100000) + " Lakh ";
                number %= 100000;
            }
            if (number >= 1000)
            {
                words += ConvertWholeNumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }
            if (number >= 100)
            {
                words += ConvertWholeNumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }
            if (number > 0)
            {
                if (number < 20)
                {
                    words += ones[number];
                }
                else
                {
                    words += tens[number / 10];
                    if (number % 10 > 0)
                    {
                        words += " " + ones[number % 10];
                    }
                }
            }

            return words.Trim();
        }

        public static decimal ApplyPercentage(decimal amount, decimal percentage)
        {
            return RoundToNearestPaisa(amount * percentage / 100m);
        }

        public static decimal CalculateCompoundInterest(decimal principal, decimal rate, int years)
        {
            if (principal <= 0 || rate <= 0 || years <= 0) return 0m;
            double factor = Math.Pow((double)(1m + rate / 100m), years);
            decimal maturityAmount = principal * (decimal)factor;
            return RoundToNearestPaisa(maturityAmount - principal);
        }

        public static decimal CalculateSimpleInterest(decimal principal, decimal rate, int days)
        {
            if (principal <= 0 || rate <= 0 || days <= 0) return 0m;
            return RoundToNearestPaisa(principal * rate * days / (100m * 365m));
        }

        public static decimal ConvertAnnualToMonthly(decimal annualRate)
        {
            return RoundToNearestPaisa(annualRate / 12m);
        }
    }
}
