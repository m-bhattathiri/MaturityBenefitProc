using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.Common;

namespace MaturityBenefitProc.Tests.Helpers.Common
{
    [TestClass]
    public class FormatHelperTests
    {
        [TestMethod]
        public void FormatCurrency_StandardAmount_ReturnsFormattedString()
        {
            var result = FormatHelper.FormatCurrency(1234.56m);
            Assert.IsTrue(result.StartsWith("INR"));
            Assert.IsTrue(result.Contains("1"));
            Assert.IsTrue(result.Contains("234"));
            Assert.IsTrue(result.Contains(".56"));
        }

        [TestMethod]
        public void FormatCurrency_ZeroAmount_ReturnsFormattedZero()
        {
            var result = FormatHelper.FormatCurrency(0m);
            Assert.IsTrue(result.StartsWith("INR"));
            Assert.IsTrue(result.Contains("0.00"));
        }

        [TestMethod]
        public void FormatCurrency_LargeAmount_ContainsINRPrefix()
        {
            var result = FormatHelper.FormatCurrency(1000000m);
            Assert.IsTrue(result.StartsWith("INR"));
        }

        [TestMethod]
        public void FormatCurrency_NegativeAmount_FormatsCorrectly()
        {
            var result = FormatHelper.FormatCurrency(-500.75m);
            Assert.IsTrue(result.Contains("500"));
            Assert.IsTrue(result.Contains(".75"));
        }

        [TestMethod]
        public void FormatDate_StandardDate_ReturnsDDMmmYYYY()
        {
            var date = new DateTime(2017, 3, 15);
            var result = FormatHelper.FormatDate(date);
            Assert.AreEqual("15-Mar-2017", result);
        }

        [TestMethod]
        public void FormatDate_JanuaryFirst_FormatsCorrectly()
        {
            var date = new DateTime(2016, 1, 1);
            var result = FormatHelper.FormatDate(date);
            Assert.AreEqual("01-Jan-2016", result);
        }

        [TestMethod]
        public void FormatDate_December31_FormatsCorrectly()
        {
            var date = new DateTime(2015, 12, 31);
            var result = FormatHelper.FormatDate(date);
            Assert.AreEqual("31-Dec-2015", result);
        }

        [TestMethod]
        public void FormatPolicyNumber_ShortNumber_ReturnsAsIs()
        {
            var result = FormatHelper.FormatPolicyNumber("ABCD");
            Assert.AreEqual("ABCD", result);
        }

        [TestMethod]
        public void FormatPolicyNumber_SixCharacters_ReturnsDashFormat()
        {
            var result = FormatHelper.FormatPolicyNumber("ABCD12");
            Assert.AreEqual("ABCD-12", result);
        }

        [TestMethod]
        public void FormatPolicyNumber_TwelveCharacters_ReturnsThreeSegments()
        {
            var result = FormatHelper.FormatPolicyNumber("ABCD1234EFGH");
            Assert.AreEqual("ABCD-1234-EFGH", result);
        }

        [TestMethod]
        public void FormatPolicyNumber_NullInput_ReturnsEmpty()
        {
            var result = FormatHelper.FormatPolicyNumber(null);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void FormatPolicyNumber_EmptyInput_ReturnsEmpty()
        {
            var result = FormatHelper.FormatPolicyNumber("");
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void MaskAccountNumber_LongNumber_ShowsLastFour()
        {
            var result = FormatHelper.MaskAccountNumber("1234567890");
            Assert.AreEqual("XXXXXX7890", result);
        }

        [TestMethod]
        public void MaskAccountNumber_FourDigits_ReturnsAsIs()
        {
            var result = FormatHelper.MaskAccountNumber("1234");
            Assert.AreEqual("1234", result);
        }

        [TestMethod]
        public void MaskAccountNumber_NullInput_ReturnsEmpty()
        {
            var result = FormatHelper.MaskAccountNumber(null);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void MaskAccountNumber_EmptyInput_ReturnsEmpty()
        {
            var result = FormatHelper.MaskAccountNumber("");
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void MaskPan_ValidPan_MasksMiddle()
        {
            var result = FormatHelper.MaskPan("ABCPK1234D");
            Assert.AreEqual("ABXXXXXX4D", result);
        }

        [TestMethod]
        public void MaskPan_NullInput_ReturnsEmpty()
        {
            var result = FormatHelper.MaskPan(null);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void MaskPan_ShortInput_ReturnsAsIs()
        {
            var result = FormatHelper.MaskPan("AB");
            Assert.AreEqual("AB", result);
        }

        [TestMethod]
        public void FormatPhoneNumber_TenDigits_AddsPlusNinetyOne()
        {
            var result = FormatHelper.FormatPhoneNumber("9876543210");
            Assert.AreEqual("+919876543210", result);
        }

        [TestMethod]
        public void FormatPhoneNumber_AlreadyHasPlusNinetyOne_ReturnsSame()
        {
            var result = FormatHelper.FormatPhoneNumber("+919876543210");
            Assert.AreEqual("+919876543210", result);
        }

        [TestMethod]
        public void FormatPhoneNumber_StartsWithNinetyOne_AddsPlus()
        {
            var result = FormatHelper.FormatPhoneNumber("919876543210");
            Assert.AreEqual("+919876543210", result);
        }

        [TestMethod]
        public void FormatPhoneNumber_NullInput_ReturnsEmpty()
        {
            var result = FormatHelper.FormatPhoneNumber(null);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void FormatPhoneNumber_EmptyInput_ReturnsEmpty()
        {
            var result = FormatHelper.FormatPhoneNumber("");
            Assert.AreEqual(string.Empty, result);
        }
    }
}
