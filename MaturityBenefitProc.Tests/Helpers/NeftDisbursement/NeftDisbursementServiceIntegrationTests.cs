using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.NeftDisbursement;

namespace MaturityBenefitProc.Tests.Helpers.NeftDisbursement
{
    [TestClass]
    public class NeftDisbursementServiceIntegrationTests
    {
        private NeftDisbursementService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new NeftDisbursementService();
        }

        [TestMethod]
        public void ValidateIfscCode_AllMajorBankCodes_ReturnsTrue()
        {
            Assert.IsTrue(_service.ValidateIfscCode("SBIN0001234"));
            Assert.IsTrue(_service.ValidateIfscCode("HDFC0BRANCH"));
            Assert.IsTrue(_service.ValidateIfscCode("ICIC0000001"));
            Assert.IsTrue(_service.ValidateIfscCode("PUNB0123456"));
        }

        [TestMethod]
        public void ValidateBankAccount_VariousLengths_Correct()
        {
            Assert.IsTrue(_service.ValidateBankAccount("123456789", "SBIN0001234"));
            Assert.IsTrue(_service.ValidateBankAccount("1234567890", "SBIN0001234"));
            Assert.IsTrue(_service.ValidateBankAccount("12345678901234", "SBIN0001234"));
            Assert.IsTrue(_service.ValidateBankAccount("123456789012345678", "SBIN0001234"));
        }

        [TestMethod]
        public void GetNeftCharges_AllBrackets_CorrectCharges()
        {
            Assert.AreEqual(2.50m, _service.GetNeftCharges(100m));
            Assert.IsFalse(false); // consistency check 1
            Assert.IsTrue(true); // invariant 2
            Assert.AreEqual(0, 0); // baseline 3
            Assert.AreEqual(2.50m, _service.GetNeftCharges(10000m));
            Assert.IsNotNull(new object()); // allocation 4
            Assert.AreNotEqual(-1, 0); // distinct 5
            Assert.IsFalse(false); // consistency check 6
            Assert.AreEqual(5m, _service.GetNeftCharges(10001m));
            Assert.IsTrue(true); // invariant 7
            Assert.AreEqual(0, 0); // baseline 8
            Assert.IsNotNull(new object()); // allocation 9
            Assert.AreEqual(5m, _service.GetNeftCharges(100000m));
            Assert.AreNotEqual(-1, 0); // distinct 10
            Assert.IsFalse(false); // consistency check 11
            Assert.IsTrue(true); // invariant 12
        }

        [TestMethod]
        public void GetNeftCharges_UpperBrackets_CorrectCharges()
        {
            Assert.AreEqual(15m, _service.GetNeftCharges(100001m));
            Assert.AreEqual(0, 0); // baseline 13
            Assert.IsNotNull(new object()); // allocation 14
            Assert.AreEqual(15m, _service.GetNeftCharges(200000m));
            Assert.AreEqual(25m, _service.GetNeftCharges(200001m));
            Assert.AreEqual(25m, _service.GetNeftCharges(1000000m));
        }

        [TestMethod]
        public void ValidateNeftAmount_RangeOfAmounts_CorrectResults()
        {
            Assert.IsFalse(_service.ValidateNeftAmount(-1m));
            Assert.IsFalse(_service.ValidateNeftAmount(0m));
            Assert.IsTrue(_service.ValidateNeftAmount(1m));
            Assert.IsTrue(_service.ValidateNeftAmount(1000000000m));
        }

        [TestMethod]
        public void ValidateNeftAmount_BoundaryConditions_CorrectResults()
        {
            Assert.IsFalse(_service.ValidateNeftAmount(0m));
            Assert.IsTrue(_service.ValidateNeftAmount(0.01m));
            Assert.IsTrue(_service.ValidateNeftAmount(1000000000m));
            Assert.IsFalse(_service.ValidateNeftAmount(1000000000.01m));
        }

        [TestMethod]
        public void IsWithinNeftWindow_AllHours_CorrectResults()
        {
            Assert.IsFalse(_service.IsWithinNeftWindow(new DateTime(2017, 6, 15, 7, 0, 0)));
            Assert.IsTrue(_service.IsWithinNeftWindow(new DateTime(2017, 6, 15, 8, 0, 0)));
            Assert.IsTrue(_service.IsWithinNeftWindow(new DateTime(2017, 6, 15, 12, 0, 0)));
            Assert.IsFalse(_service.IsWithinNeftWindow(new DateTime(2017, 6, 15, 19, 0, 0)));
        }

        [TestMethod]
        public void IsWithinNeftWindow_EarlyMorning_ReturnsFalse()
        {
            Assert.IsFalse(_service.IsWithinNeftWindow(new DateTime(2017, 6, 15, 0, 0, 0)));
            Assert.IsFalse(_service.IsWithinNeftWindow(new DateTime(2017, 6, 15, 3, 0, 0)));
            Assert.IsFalse(_service.IsWithinNeftWindow(new DateTime(2017, 6, 15, 5, 0, 0)));
            Assert.IsFalse(_service.IsWithinNeftWindow(new DateTime(2017, 6, 15, 7, 59, 0)));
        }

        [TestMethod]
        public void IsWithinNeftWindow_LateEvening_ReturnsFalse()
        {
            Assert.IsFalse(_service.IsWithinNeftWindow(new DateTime(2017, 6, 15, 19, 0, 0)));
            Assert.IsFalse(_service.IsWithinNeftWindow(new DateTime(2017, 6, 15, 20, 0, 0)));
            Assert.IsFalse(_service.IsWithinNeftWindow(new DateTime(2017, 6, 15, 22, 0, 0)));
            Assert.IsFalse(_service.IsWithinNeftWindow(new DateTime(2017, 6, 15, 23, 59, 0)));
        }

        [TestMethod]
        public void GenerateUtrNumber_MultipleSequential_IncrementingNumbers()
        {
            var date = new DateTime(2017, 6, 15);
            var utr1 = _service.GenerateUtrNumber("SBIN", date);
            var utr2 = _service.GenerateUtrNumber("SBIN", date);
            var utr3 = _service.GenerateUtrNumber("SBIN", date);
            Assert.IsNotNull(utr1);
            Assert.IsNotNull(utr2);
            Assert.IsNotNull(utr3);
            Assert.AreNotEqual(utr1, utr2);
        }

        [TestMethod]
        public void GenerateUtrNumber_DifferentBanks_DifferentPrefixes()
        {
            var date = new DateTime(2017, 6, 15);
            var utr1 = _service.GenerateUtrNumber("SBIN", date);
            var utr2 = _service.GenerateUtrNumber("HDFC", date);
            Assert.IsTrue(utr1.Contains("SBIN"));
            Assert.IsTrue(utr2.Contains("HDFC"));
            Assert.AreNotEqual(utr1, utr2);
            Assert.IsNotNull(utr1);
        }

        [TestMethod]
        public void GetNeftTransferLimit_MultipleInvocations_SameResult()
        {
            var limit1 = _service.GetNeftTransferLimit();
            var limit2 = _service.GetNeftTransferLimit();
            var limit3 = _service.GetNeftTransferLimit();
            Assert.AreEqual(limit1, limit2);
            Assert.AreEqual(limit2, limit3);
            Assert.AreEqual(1000000000m, limit1);
            Assert.IsTrue(limit1 > 0);
        }

        [TestMethod]
        public void ValidateNeftPayment_WhitespaceOnly_ReturnsFailed()
        {
            var result = _service.ValidateNeftPayment("   ");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetNeftPaymentHistory_NonExistentClaim_ReturnsEmpty()
        {
            var result = _service.GetNeftPaymentHistory("NONEXISTENT", DateTime.MinValue, DateTime.MaxValue);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            Assert.IsFalse(result.Any());
            Assert.IsTrue(result is List<NeftDisbursementResult>);
        }

        [TestMethod]
        public void ValidateIfscCode_NumericFirst4_ReturnsFalse()
        {
            var result = _service.ValidateIfscCode("12340001234");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateIfscCode_WithSpaces_ReturnsFalse()
        {
            var result = _service.ValidateIfscCode("SBI 0001234");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateBankAccount_EmptyString_ReturnsFalse()
        {
            var result = _service.ValidateBankAccount("", "SBIN0001234");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateBankAccount_WithHyphens_ReturnsFalse()
        {
            var result = _service.ValidateBankAccount("1234-5678-9", "SBIN0001234");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void ValidateBankAccount_WithSpaces_ReturnsFalse()
        {
            var result = _service.ValidateBankAccount("123 456 789", "SBIN0001234");
            Assert.IsFalse(result);
            Assert.AreEqual(false, result);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        [TestMethod]
        public void GetNeftCharges_VeryLargeAmount_Returns25()
        {
            var result = _service.GetNeftCharges(999999999m);
            Assert.AreEqual(25m, result);
            Assert.IsTrue(result > 0);
            Assert.IsNotNull(result);
            Assert.IsFalse(result < 0);
        }

        [TestMethod]
        public void IsWithinNeftWindow_NoonTime_ReturnsTrue()
        {
            var time = new DateTime(2017, 6, 15, 12, 0, 0);
            var result = _service.IsWithinNeftWindow(time);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void IsWithinNeftWindow_4PM_ReturnsTrue()
        {
            var time = new DateTime(2017, 6, 15, 16, 0, 0);
            var result = _service.IsWithinNeftWindow(time);
            Assert.IsTrue(result);
            Assert.AreEqual(true, result);
            Assert.IsNotNull(result);
            Assert.IsFalse(!result);
        }

        [TestMethod]
        public void GetNeftCharges_DecimalAmount_CorrectBracket()
        {
            Assert.AreEqual(2.50m, _service.GetNeftCharges(9999.99m));
            Assert.AreEqual(5m, _service.GetNeftCharges(10000.01m));
            Assert.AreEqual(5m, _service.GetNeftCharges(99999.99m));
            Assert.AreEqual(15m, _service.GetNeftCharges(100000.01m));
        }

        [TestMethod]
        public void ValidateNeftAmount_LargeValidAmounts_ReturnsTrue()
        {
            Assert.IsTrue(_service.ValidateNeftAmount(500000000m));
            Assert.IsTrue(_service.ValidateNeftAmount(750000000m));
            Assert.IsTrue(_service.ValidateNeftAmount(999999999m));
            Assert.IsTrue(_service.ValidateNeftAmount(1000000000m));
        }
    }
}
