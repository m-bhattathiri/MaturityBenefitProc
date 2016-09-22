using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.Common;

namespace MaturityBenefitProc.Tests.Helpers.Common
{
    [TestClass]
    public class ValidationHelperTests
    {
        [TestMethod]
        public void IsValidPan_ValidFormat_ReturnsTrue()
        {
            Assert.IsTrue(ValidationHelper.IsValidPan("ABCPK1234D"));
        }

        [TestMethod]
        public void IsValidPan_AnotherValidPan_ReturnsTrue()
        {
            Assert.IsTrue(ValidationHelper.IsValidPan("ZZZZZ9999Z"));
        }

        [TestMethod]
        public void IsValidPan_LowerCase_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidPan("abcpk1234d"));
        }

        [TestMethod]
        public void IsValidPan_TooShort_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidPan("ABCPK123"));
        }

        [TestMethod]
        public void IsValidPan_NullInput_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidPan(null));
        }

        [TestMethod]
        public void IsValidPan_EmptyString_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidPan(""));
        }

        [TestMethod]
        public void IsValidPan_WhitespaceOnly_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidPan("   "));
        }

        [TestMethod]
        public void IsValidAadhaar_Valid12Digits_ReturnsTrue()
        {
            Assert.IsTrue(ValidationHelper.IsValidAadhaar("123456789012"));
        }

        [TestMethod]
        public void IsValidAadhaar_TooFewDigits_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidAadhaar("12345678901"));
        }

        [TestMethod]
        public void IsValidAadhaar_ContainsLetters_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidAadhaar("12345678901A"));
        }

        [TestMethod]
        public void IsValidAadhaar_NullInput_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidAadhaar(null));
        }

        [TestMethod]
        public void IsValidIfsc_ValidFormat_ReturnsTrue()
        {
            Assert.IsTrue(ValidationHelper.IsValidIfsc("SBIN0001234"));
        }

        [TestMethod]
        public void IsValidIfsc_AnotherValidIfsc_ReturnsTrue()
        {
            Assert.IsTrue(ValidationHelper.IsValidIfsc("HDFC0BRANCH"));
        }

        [TestMethod]
        public void IsValidIfsc_MissingZeroAtFifth_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidIfsc("SBIN1001234"));
        }

        [TestMethod]
        public void IsValidIfsc_TooShort_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidIfsc("SBIN0"));
        }

        [TestMethod]
        public void IsValidIfsc_NullInput_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidIfsc(null));
        }

        [TestMethod]
        public void IsValidPolicyNumber_ValidLength8_ReturnsTrue()
        {
            Assert.IsTrue(ValidationHelper.IsValidPolicyNumber("POL12345"));
        }

        [TestMethod]
        public void IsValidPolicyNumber_ValidLength20_ReturnsTrue()
        {
            Assert.IsTrue(ValidationHelper.IsValidPolicyNumber("12345678901234567890"));
        }

        [TestMethod]
        public void IsValidPolicyNumber_TooShort_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidPolicyNumber("POL123"));
        }

        [TestMethod]
        public void IsValidPolicyNumber_NullInput_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidPolicyNumber(null));
        }

        [TestMethod]
        public void IsValidPolicyNumber_EmptyString_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidPolicyNumber(""));
        }

        [TestMethod]
        public void IsValidPhoneNumber_Valid10Digits_ReturnsTrue()
        {
            Assert.IsTrue(ValidationHelper.IsValidPhoneNumber("9876543210"));
        }

        [TestMethod]
        public void IsValidPhoneNumber_TooFewDigits_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidPhoneNumber("987654321"));
        }

        [TestMethod]
        public void IsValidPhoneNumber_TooManyDigits_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidPhoneNumber("98765432101"));
        }

        [TestMethod]
        public void IsValidPhoneNumber_NullInput_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidPhoneNumber(null));
        }

        [TestMethod]
        public void IsValidEmail_ValidAddress_ReturnsTrue()
        {
            Assert.IsTrue(ValidationHelper.IsValidEmail("user@example.com"));
        }

        [TestMethod]
        public void IsValidEmail_MissingAtSign_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidEmail("userexample.com"));
        }

        [TestMethod]
        public void IsValidEmail_NullInput_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidEmail(null));
        }

        [TestMethod]
        public void IsValidEmail_EmptyString_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidEmail(""));
        }

        [TestMethod]
        public void IsValidEmail_AtSignAtStart_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidEmail("@example.com"));
        }

        [TestMethod]
        public void IsValidEmail_NoDotAfterAt_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidEmail("user@examplecom"));
        }

        [TestMethod]
        public void IsValidPincode_Valid6Digits_ReturnsTrue()
        {
            Assert.IsTrue(ValidationHelper.IsValidPincode("110001"));
        }

        [TestMethod]
        public void IsValidPincode_TooFew_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidPincode("11000"));
        }

        [TestMethod]
        public void IsValidPincode_TooMany_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidPincode("1100011"));
        }

        [TestMethod]
        public void IsValidPincode_ContainsLetters_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidPincode("11000A"));
        }

        [TestMethod]
        public void IsValidAccountNumber_Valid9Digits_ReturnsTrue()
        {
            Assert.IsTrue(ValidationHelper.IsValidAccountNumber("123456789"));
        }

        [TestMethod]
        public void IsValidAccountNumber_Valid18Digits_ReturnsTrue()
        {
            Assert.IsTrue(ValidationHelper.IsValidAccountNumber("123456789012345678"));
        }

        [TestMethod]
        public void IsValidAccountNumber_TooShort8Digits_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidAccountNumber("12345678"));
        }

        [TestMethod]
        public void IsValidAccountNumber_TooLong19Digits_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidAccountNumber("1234567890123456789"));
        }

        [TestMethod]
        public void IsValidAccountNumber_NullInput_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsValidAccountNumber(null));
        }

        [TestMethod]
        public void IsPositiveAmount_PositiveValue_ReturnsTrue()
        {
            Assert.IsTrue(ValidationHelper.IsPositiveAmount(100.50m));
        }

        [TestMethod]
        public void IsPositiveAmount_Zero_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsPositiveAmount(0m));
        }

        [TestMethod]
        public void IsPositiveAmount_NegativeValue_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsPositiveAmount(-50m));
        }

        [TestMethod]
        public void IsWithinRange_ValueInRange_ReturnsTrue()
        {
            Assert.IsTrue(ValidationHelper.IsWithinRange(50m, 10m, 100m));
        }

        [TestMethod]
        public void IsWithinRange_ValueBelowMin_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsWithinRange(5m, 10m, 100m));
        }

        [TestMethod]
        public void IsWithinRange_ValueAboveMax_ReturnsFalse()
        {
            Assert.IsFalse(ValidationHelper.IsWithinRange(150m, 10m, 100m));
        }

        [TestMethod]
        public void IsWithinRange_ValueAtMin_ReturnsTrue()
        {
            Assert.IsTrue(ValidationHelper.IsWithinRange(10m, 10m, 100m));
        }

        [TestMethod]
        public void IsWithinRange_ValueAtMax_ReturnsTrue()
        {
            Assert.IsTrue(ValidationHelper.IsWithinRange(100m, 10m, 100m));
        }
    }
}
