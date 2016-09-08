using System;
using System.Globalization;

namespace MaturityBenefitProc.Helpers.Common
{
    public static class FormatHelper
    {
        private static readonly CultureInfo IndianCulture = new CultureInfo("en-IN");

        public static string FormatCurrency(decimal amount)
        {
            return string.Format(IndianCulture, "INR {0:N2}", amount);
        }

        public static string FormatDate(DateTime date)
        {
            return date.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
        }

        public static string FormatPolicyNumber(string rawNumber)
        {
            if (string.IsNullOrWhiteSpace(rawNumber)) return string.Empty;
            string cleaned = rawNumber.Trim().Replace("-", "").Replace(" ", "");
            if (cleaned.Length <= 4) return cleaned;
            if (cleaned.Length <= 8)
            {
                return string.Format("{0}-{1}", cleaned.Substring(0, 4), cleaned.Substring(4));
            }
            return string.Format("{0}-{1}-{2}",
                cleaned.Substring(0, 4),
                cleaned.Substring(4, 4),
                cleaned.Substring(8));
        }

        public static string MaskAccountNumber(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber)) return string.Empty;
            string trimmed = accountNumber.Trim();
            if (trimmed.Length <= 4) return trimmed;
            int maskLength = trimmed.Length - 4;
            return new string('X', maskLength) + trimmed.Substring(maskLength);
        }

        public static string MaskPan(string pan)
        {
            if (string.IsNullOrWhiteSpace(pan)) return string.Empty;
            string trimmed = pan.Trim();
            if (trimmed.Length < 4) return trimmed;
            return trimmed.Substring(0, 2) + new string('X', trimmed.Length - 4) + trimmed.Substring(trimmed.Length - 2);
        }

        public static string FormatPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return string.Empty;
            string trimmed = phone.Trim();
            if (trimmed.StartsWith("+91")) return trimmed;
            if (trimmed.StartsWith("91") && trimmed.Length == 12) return "+" + trimmed;
            return "+91" + trimmed;
        }
    }
}
