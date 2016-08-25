using System;
using System.Text.RegularExpressions;

namespace MaturityBenefitProc.Helpers.Common
{
    public static class ValidationHelper
    {
        private static readonly Regex PanPattern = new Regex(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", RegexOptions.Compiled);
        private static readonly Regex AadhaarPattern = new Regex(@"^[0-9]{12}$", RegexOptions.Compiled);
        private static readonly Regex IfscPattern = new Regex(@"^[A-Z]{4}0[A-Z0-9]{6}$", RegexOptions.Compiled);
        private static readonly Regex PhonePattern = new Regex(@"^[0-9]{10}$", RegexOptions.Compiled);
        private static readonly Regex PincodePattern = new Regex(@"^[0-9]{6}$", RegexOptions.Compiled);
        private static readonly Regex AccountPattern = new Regex(@"^[0-9]{9,18}$", RegexOptions.Compiled);

        public static bool IsValidPan(string pan)
        {
            if (string.IsNullOrWhiteSpace(pan)) return false;
            return PanPattern.IsMatch(pan.Trim());
        }

        public static bool IsValidAadhaar(string aadhaar)
        {
            if (string.IsNullOrWhiteSpace(aadhaar)) return false;
            return AadhaarPattern.IsMatch(aadhaar.Trim());
        }

        public static bool IsValidIfsc(string ifsc)
        {
            if (string.IsNullOrWhiteSpace(ifsc)) return false;
            return IfscPattern.IsMatch(ifsc.Trim());
        }

        public static bool IsValidPolicyNumber(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber)) return false;
            string trimmed = policyNumber.Trim();
            return trimmed.Length >= 8 && trimmed.Length <= 20;
        }

        public static bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            return PhonePattern.IsMatch(phone.Trim());
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            string trimmed = email.Trim();
            int atIndex = trimmed.IndexOf('@');
            if (atIndex <= 0) return false;
            int dotIndex = trimmed.LastIndexOf('.');
            return dotIndex > atIndex + 1 && dotIndex < trimmed.Length - 1;
        }

        public static bool IsValidPincode(string pincode)
        {
            if (string.IsNullOrWhiteSpace(pincode)) return false;
            return PincodePattern.IsMatch(pincode.Trim());
        }

        public static bool IsValidAccountNumber(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber)) return false;
            return AccountPattern.IsMatch(accountNumber.Trim());
        }

        public static bool IsPositiveAmount(decimal amount)
        {
            return amount > 0;
        }

        public static bool IsWithinRange(decimal value, decimal min, decimal max)
        {
            return value >= min && value <= max;
        }
    }
}
