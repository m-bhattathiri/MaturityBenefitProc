using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.FundValueAndUnitLinkedPlans;
using Moq;

namespace MaturityBenefitProc.Tests.Helpers.FundValueAndUnitLinkedPlans
{
    [TestClass]
    public class UlipMaturityServiceValidationTests
    {
        private Mock<IUlipMaturityService> _mockService;
        private IUlipMaturityService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IUlipMaturityService>();
            _service = _mockService.Object;
        }

        [TestMethod]
        public void CalculateTotalFundValue_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.CalculateTotalFundValue("POL123", It.IsAny<DateTime>())).Returns(150000m);
            _mockService.Setup(s => s.CalculateTotalFundValue("POL456", It.IsAny<DateTime>())).Returns(200000m);

            var result1 = _service.CalculateTotalFundValue("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.CalculateTotalFundValue("POL456", new DateTime(2023, 1, 1));

            Assert.AreEqual(150000m, result1);
            Assert.AreEqual(200000m, result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsTrue(result1 > 0);
        }

        [TestMethod]
        public void CalculateTotalFundValue_InvalidPolicyId_ReturnsZero()
        {
            _mockService.Setup(s => s.CalculateTotalFundValue("", It.IsAny<DateTime>())).Returns(0m);
            _mockService.Setup(s => s.CalculateTotalFundValue(null, It.IsAny<DateTime>())).Returns(0m);

            var result1 = _service.CalculateTotalFundValue("", DateTime.Now);
            var result2 = _service.CalculateTotalFundValue(null, DateTime.Now);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.IsFalse(result1 > 0);
            Assert.IsFalse(result2 > 0);
        }

        [TestMethod]
        public void GetNavOnDate_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.GetNavOnDate("FUND1", It.IsAny<DateTime>())).Returns(25.5m);
            _mockService.Setup(s => s.GetNavOnDate("FUND2", It.IsAny<DateTime>())).Returns(30.0m);

            var result1 = _service.GetNavOnDate("FUND1", DateTime.Now);
            var result2 = _service.GetNavOnDate("FUND2", DateTime.Now);

            Assert.AreEqual(25.5m, result1);
            Assert.AreEqual(30.0m, result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsTrue(result1 > 0);
        }

        [TestMethod]
        public void CalculateMortalityChargeRefund_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.CalculateMortalityChargeRefund("POL123")).Returns(5000m);
            _mockService.Setup(s => s.CalculateMortalityChargeRefund("POL456")).Returns(7500m);

            var result1 = _service.CalculateMortalityChargeRefund("POL123");
            var result2 = _service.CalculateMortalityChargeRefund("POL456");

            Assert.AreEqual(5000m, result1);
            Assert.AreEqual(7500m, result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsTrue(result1 > 0);
        }

        [TestMethod]
        public void CalculateLoyaltyAdditions_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.CalculateLoyaltyAdditions("POL123", 10)).Returns(10000m);
            _mockService.Setup(s => s.CalculateLoyaltyAdditions("POL123", 15)).Returns(20000m);

            var result1 = _service.CalculateLoyaltyAdditions("POL123", 10);
            var result2 = _service.CalculateLoyaltyAdditions("POL123", 15);

            Assert.AreEqual(10000m, result1);
            Assert.AreEqual(20000m, result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsTrue(result2 > result1);
        }

        [TestMethod]
        public void CalculateWealthBoosters_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.CalculateWealthBoosters("POL123", 100000m)).Returns(2000m);
            _mockService.Setup(s => s.CalculateWealthBoosters("POL123", 200000m)).Returns(4000m);

            var result1 = _service.CalculateWealthBoosters("POL123", 100000m);
            var result2 = _service.CalculateWealthBoosters("POL123", 200000m);

            Assert.AreEqual(2000m, result1);
            Assert.AreEqual(4000m, result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsTrue(result2 > result1);
        }

        [TestMethod]
        public void GetTotalAllocatedUnits_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.GetTotalAllocatedUnits("POL123", "FUND1")).Returns(1500.5m);
            _mockService.Setup(s => s.GetTotalAllocatedUnits("POL123", "FUND2")).Returns(2000.75m);

            var result1 = _service.GetTotalAllocatedUnits("POL123", "FUND1");
            var result2 = _service.GetTotalAllocatedUnits("POL123", "FUND2");

            Assert.AreEqual(1500.5m, result1);
            Assert.AreEqual(2000.75m, result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsTrue(result1 > 0);
        }

        [TestMethod]
        public void CalculateSurrenderValue_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.CalculateSurrenderValue("POL123", It.IsAny<DateTime>())).Returns(95000m);
            _mockService.Setup(s => s.CalculateSurrenderValue("POL456", It.IsAny<DateTime>())).Returns(120000m);

            var result1 = _service.CalculateSurrenderValue("POL123", DateTime.Now);
            var result2 = _service.CalculateSurrenderValue("POL456", DateTime.Now);

            Assert.AreEqual(95000m, result1);
            Assert.AreEqual(120000m, result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsTrue(result1 > 0);
        }

        [TestMethod]
        public void GetFundManagementCharge_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.GetFundManagementCharge("FUND1", 0.0135)).Returns(135m);
            _mockService.Setup(s => s.GetFundManagementCharge("FUND2", 0.0150)).Returns(150m);

            var result1 = _service.GetFundManagementCharge("FUND1", 0.0135);
            var result2 = _service.GetFundManagementCharge("FUND2", 0.0150);

            Assert.AreEqual(135m, result1);
            Assert.AreEqual(150m, result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsTrue(result1 > 0);
        }

        [TestMethod]
        public void CalculateGuaranteeAddition_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.CalculateGuaranteeAddition("POL123", 50000m)).Returns(2500m);
            _mockService.Setup(s => s.CalculateGuaranteeAddition("POL123", 100000m)).Returns(5000m);

            var result1 = _service.CalculateGuaranteeAddition("POL123", 50000m);
            var result2 = _service.CalculateGuaranteeAddition("POL123", 100000m);

            Assert.AreEqual(2500m, result1);
            Assert.AreEqual(5000m, result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsTrue(result2 > result1);
        }

        [TestMethod]
        public void GetTotalPremiumPaid_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.GetTotalPremiumPaid("POL123")).Returns(500000m);
            _mockService.Setup(s => s.GetTotalPremiumPaid("POL456")).Returns(750000m);

            var result1 = _service.GetTotalPremiumPaid("POL123");
            var result2 = _service.GetTotalPremiumPaid("POL456");

            Assert.AreEqual(500000m, result1);
            Assert.AreEqual(750000m, result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsTrue(result1 > 0);
        }

        [TestMethod]
        public void GetFundAllocationRatio_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.GetFundAllocationRatio("POL123", "FUND1")).Returns(0.6);
            _mockService.Setup(s => s.GetFundAllocationRatio("POL123", "FUND2")).Returns(0.4);

            var result1 = _service.GetFundAllocationRatio("POL123", "FUND1");
            var result2 = _service.GetFundAllocationRatio("POL123", "FUND2");

            Assert.AreEqual(0.6, result1);
            Assert.AreEqual(0.4, result2);
            Assert.AreEqual(1.0, result1 + result2);
            Assert.IsTrue(result1 > 0);
        }

        [TestMethod]
        public void GetMortalityRate_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.GetMortalityRate(30, 5)).Returns(0.0015);
            _mockService.Setup(s => s.GetMortalityRate(40, 5)).Returns(0.0025);

            var result1 = _service.GetMortalityRate(30, 5);
            var result2 = _service.GetMortalityRate(40, 5);

            Assert.AreEqual(0.0015, result1);
            Assert.AreEqual(0.0025, result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsTrue(result2 > result1);
        }

        [TestMethod]
        public void GetNavGrowthRate_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.GetNavGrowthRate("FUND1", It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(0.12);
            _mockService.Setup(s => s.GetNavGrowthRate("FUND2", It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(0.08);

            var result1 = _service.GetNavGrowthRate("FUND1", DateTime.Now.AddYears(-1), DateTime.Now);
            var result2 = _service.GetNavGrowthRate("FUND2", DateTime.Now.AddYears(-1), DateTime.Now);

            Assert.AreEqual(0.12, result1);
            Assert.AreEqual(0.08, result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsTrue(result1 > 0);
        }

        [TestMethod]
        public void IsEligibleForMortalityRefund_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.IsEligibleForMortalityRefund("POL123")).Returns(true);
            _mockService.Setup(s => s.IsEligibleForMortalityRefund("POL456")).Returns(false);

            var result1 = _service.IsEligibleForMortalityRefund("POL123");
            var result2 = _service.IsEligibleForMortalityRefund("POL456");

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ValidateFundSwitch_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.ValidateFundSwitch("POL123", "FUND1", "FUND2", 10000m)).Returns(true);
            _mockService.Setup(s => s.ValidateFundSwitch("POL123", "FUND1", "FUND2", 1000000m)).Returns(false);

            var result1 = _service.ValidateFundSwitch("POL123", "FUND1", "FUND2", 10000m);
            var result2 = _service.ValidateFundSwitch("POL123", "FUND1", "FUND2", 1000000m);

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetCompletedPolicyYears_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.GetCompletedPolicyYears("POL123", It.IsAny<DateTime>())).Returns(5);
            _mockService.Setup(s => s.GetCompletedPolicyYears("POL456", It.IsAny<DateTime>())).Returns(10);

            var result1 = _service.GetCompletedPolicyYears("POL123", DateTime.Now);
            var result2 = _service.GetCompletedPolicyYears("POL456", DateTime.Now);

            Assert.AreEqual(5, result1);
            Assert.AreEqual(10, result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsTrue(result1 > 0);
        }

        [TestMethod]
        public void GetRemainingPremiumTerms_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.GetRemainingPremiumTerms("POL123")).Returns(5);
            _mockService.Setup(s => s.GetRemainingPremiumTerms("POL456")).Returns(0);

            var result1 = _service.GetRemainingPremiumTerms("POL123");
            var result2 = _service.GetRemainingPremiumTerms("POL456");

            Assert.AreEqual(5, result1);
            Assert.AreEqual(0, result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsTrue(result1 > result2);
        }

        [TestMethod]
        public void GetPrimaryFundId_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.GetPrimaryFundId("POL123")).Returns("FUND1");
            _mockService.Setup(s => s.GetPrimaryFundId("POL456")).Returns("FUND2");

            var result1 = _service.GetPrimaryFundId("POL123");
            var result2 = _service.GetPrimaryFundId("POL456");

            Assert.AreEqual("FUND1", result1);
            Assert.AreEqual("FUND2", result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetPolicyStatus_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.GetPolicyStatus("POL123")).Returns("ACTIVE");
            _mockService.Setup(s => s.GetPolicyStatus("POL456")).Returns("MATURED");

            var result1 = _service.GetPolicyStatus("POL123");
            var result2 = _service.GetPolicyStatus("POL456");

            Assert.AreEqual("ACTIVE", result1);
            Assert.AreEqual("MATURED", result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GenerateMaturityStatementId_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.GenerateMaturityStatementId("POL123", It.IsAny<DateTime>())).Returns("STMT-POL123-2023");
            _mockService.Setup(s => s.GenerateMaturityStatementId("POL456", It.IsAny<DateTime>())).Returns("STMT-POL456-2023");

            var result1 = _service.GenerateMaturityStatementId("POL123", DateTime.Now);
            var result2 = _service.GenerateMaturityStatementId("POL456", DateTime.Now);

            Assert.AreEqual("STMT-POL123-2023", result1);
            Assert.AreEqual("STMT-POL456-2023", result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateFinalMaturityPayout_ValidInputs_ReturnsExpected()
        {
            string status1 = "SUCCESS";
            string status2 = "PENDING";
            _mockService.Setup(s => s.CalculateFinalMaturityPayout("POL123", It.IsAny<DateTime>(), out status1)).Returns(250000m);
            _mockService.Setup(s => s.CalculateFinalMaturityPayout("POL456", It.IsAny<DateTime>(), out status2)).Returns(300000m);

            var result1 = _service.CalculateFinalMaturityPayout("POL123", DateTime.Now, out string outStatus1);
            var result2 = _service.CalculateFinalMaturityPayout("POL456", DateTime.Now, out string outStatus2);

            Assert.AreEqual(250000m, result1);
            Assert.AreEqual(300000m, result2);
            Assert.AreEqual("SUCCESS", outStatus1);
            Assert.AreEqual("PENDING", outStatus2);
        }

        [TestMethod]
        public void CalculateTopUpFundValue_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.CalculateTopUpFundValue("POL123", It.IsAny<DateTime>())).Returns(50000m);
            _mockService.Setup(s => s.CalculateTopUpFundValue("POL456", It.IsAny<DateTime>())).Returns(0m);

            var result1 = _service.CalculateTopUpFundValue("POL123", DateTime.Now);
            var result2 = _service.CalculateTopUpFundValue("POL456", DateTime.Now);

            Assert.AreEqual(50000m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsTrue(result1 > 0);
        }

        [TestMethod]
        public void ApplyDiscontinuanceCharge_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.ApplyDiscontinuanceCharge("POL123", 100000m)).Returns(95000m);
            _mockService.Setup(s => s.ApplyDiscontinuanceCharge("POL456", 100000m)).Returns(98000m);

            var result1 = _service.ApplyDiscontinuanceCharge("POL123", 100000m);
            var result2 = _service.ApplyDiscontinuanceCharge("POL456", 100000m);

            Assert.AreEqual(95000m, result1);
            Assert.AreEqual(98000m, result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsTrue(result1 > 0);
        }

        [TestMethod]
        public void CalculatePartialWithdrawalImpact_ValidInputs_ReturnsExpected()
        {
            _mockService.Setup(s => s.CalculatePartialWithdrawalImpact("POL123", 10000m)).Returns(10500m);
            _mockService.Setup(s => s.CalculatePartialWithdrawalImpact("POL456", 20000m)).Returns(21000m);

            var result1 = _service.CalculatePartialWithdrawalImpact("POL123", 10000m);
            var result2 = _service.CalculatePartialWithdrawalImpact("POL456", 20000m);

            Assert.AreEqual(10500m, result1);
            Assert.AreEqual(21000m, result2);
            Assert.AreNotEqual(result1, result2);
            Assert.IsTrue(result1 > 0);
        }
    }
}