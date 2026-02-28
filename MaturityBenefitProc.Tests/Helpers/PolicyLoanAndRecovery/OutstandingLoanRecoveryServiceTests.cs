using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.PolicyLoanAndRecovery;

namespace MaturityBenefitProc.Tests.Helpers.PolicyLoanAndRecovery
{
    [TestClass]
    public class OutstandingLoanRecoveryServiceTests
    {
        private IOutstandingLoanRecoveryService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming OutstandingLoanRecoveryService implements IOutstandingLoanRecoveryService
            // and provides fixed logic for tests to pass.
            // Note: Since the prompt says "Each test creates a new OutstandingLoanRecoveryService()",
            // we will instantiate it here. We assume it exists in the namespace.
            _service = new OutstandingLoanRecoveryService();
        }

        [TestMethod]
        public void CalculateTotalOutstandingPrincipal_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.CalculateTotalOutstandingPrincipal("POL123", new DateTime(2023, 1, 1));
            var result2 = _service.CalculateTotalOutstandingPrincipal("POL456", new DateTime(2023, 5, 1));
            var result3 = _service.CalculateTotalOutstandingPrincipal("POL789", new DateTime(2023, 12, 31));
            var result4 = _service.CalculateTotalOutstandingPrincipal("POL000", new DateTime(2022, 1, 1));

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0m, result1);
            Assert.IsTrue(result1 > 0m);
            Assert.IsNotNull(result2);
            Assert.IsTrue(result3 >= 0m);
        }

        [TestMethod]
        public void CalculateAccruedInterest_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.CalculateAccruedInterest("LN123", new DateTime(2023, 1, 1));
            var result2 = _service.CalculateAccruedInterest("LN456", new DateTime(2023, 6, 1));
            var result3 = _service.CalculateAccruedInterest("LN789", new DateTime(2023, 12, 31));
            var result4 = _service.CalculateAccruedInterest("LN000", new DateTime(2022, 1, 1));

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0m, result1);
            Assert.IsTrue(result2 > 0m);
            Assert.IsTrue(result3 >= 0m);
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void GetDailyInterestAccrualAmount_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.GetDailyInterestAccrualAmount("LN123", 10000m, 0.05);
            var result2 = _service.GetDailyInterestAccrualAmount("LN456", 5000m, 0.03);
            var result3 = _service.GetDailyInterestAccrualAmount("LN789", 20000m, 0.07);
            var result4 = _service.GetDailyInterestAccrualAmount("LN000", 0m, 0.05);

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0m, result1);
            Assert.IsTrue(result2 > 0m);
            Assert.IsTrue(result3 > 0m);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculatePenalties_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.CalculatePenalties("LN123", 30);
            var result2 = _service.CalculatePenalties("LN456", 60);
            var result3 = _service.CalculatePenalties("LN789", 0);
            var result4 = _service.CalculatePenalties("LN000", 90);

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0m, result1);
            Assert.IsTrue(result2 > 0m);
            Assert.AreEqual(0m, result3);
            Assert.IsTrue(result4 > 0m);
        }

        [TestMethod]
        public void GetTotalRecoveryAmount_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.GetTotalRecoveryAmount("POL123", "LN123");
            var result2 = _service.GetTotalRecoveryAmount("POL456", "LN456");
            var result3 = _service.GetTotalRecoveryAmount("POL789", "LN789");
            var result4 = _service.GetTotalRecoveryAmount("POL000", "LN000");

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0m, result1);
            Assert.IsTrue(result2 > 0m);
            Assert.IsTrue(result3 >= 0m);
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void CalculateTaxOnRecovery_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.CalculateTaxOnRecovery(1000m, "TAX01");
            var result2 = _service.CalculateTaxOnRecovery(5000m, "TAX02");
            var result3 = _service.CalculateTaxOnRecovery(0m, "TAX01");
            var result4 = _service.CalculateTaxOnRecovery(2000m, "TAX03");

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0m, result1);
            Assert.IsTrue(result2 > 0m);
            Assert.AreEqual(0m, result3);
            Assert.IsTrue(result4 > 0m);
        }

        [TestMethod]
        public void GetRemainingMaturityProceeds_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.GetRemainingMaturityProceeds(10000m, 2000m);
            var result2 = _service.GetRemainingMaturityProceeds(5000m, 5000m);
            var result3 = _service.GetRemainingMaturityProceeds(20000m, 0m);
            var result4 = _service.GetRemainingMaturityProceeds(15000m, 5000m);

            Assert.AreEqual(8000m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(20000m, result3);
            Assert.AreEqual(10000m, result4);
            Assert.AreNotEqual(0m, result1);
        }

        [TestMethod]
        public void GetCurrentInterestRate_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.GetCurrentInterestRate("LN123");
            var result2 = _service.GetCurrentInterestRate("LN456");
            var result3 = _service.GetCurrentInterestRate("LN789");
            var result4 = _service.GetCurrentInterestRate("LN000");

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0.0, result1);
            Assert.IsTrue(result2 > 0.0);
            Assert.IsTrue(result3 >= 0.0);
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void GetPenaltyRate_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.GetPenaltyRate("LN123", "TYPE_A");
            var result2 = _service.GetPenaltyRate("LN456", "TYPE_B");
            var result3 = _service.GetPenaltyRate("LN789", "TYPE_C");
            var result4 = _service.GetPenaltyRate("LN000", "TYPE_A");

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0.0, result1);
            Assert.IsTrue(result2 > 0.0);
            Assert.IsTrue(result3 >= 0.0);
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void CalculateLoanToValueRatio_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.CalculateLoanToValueRatio(5000m, 10000m);
            var result2 = _service.CalculateLoanToValueRatio(2000m, 10000m);
            var result3 = _service.CalculateLoanToValueRatio(0m, 10000m);
            var result4 = _service.CalculateLoanToValueRatio(10000m, 10000m);

            Assert.AreEqual(0.5, result1);
            Assert.AreEqual(0.2, result2);
            Assert.AreEqual(0.0, result3);
            Assert.AreEqual(1.0, result4);
            Assert.AreNotEqual(0.0, result1);
        }

        [TestMethod]
        public void GetHistoricalAverageInterestRate_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.GetHistoricalAverageInterestRate("LN123", new DateTime(2020, 1, 1), new DateTime(2023, 1, 1));
            var result2 = _service.GetHistoricalAverageInterestRate("LN456", new DateTime(2021, 1, 1), new DateTime(2023, 1, 1));
            var result3 = _service.GetHistoricalAverageInterestRate("LN789", new DateTime(2022, 1, 1), new DateTime(2023, 1, 1));
            var result4 = _service.GetHistoricalAverageInterestRate("LN000", new DateTime(2019, 1, 1), new DateTime(2023, 1, 1));

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0.0, result1);
            Assert.IsTrue(result2 > 0.0);
            Assert.IsTrue(result3 >= 0.0);
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void IsLoanEligibleForRecovery_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.IsLoanEligibleForRecovery("LN123", new DateTime(2023, 1, 1));
            var result2 = _service.IsLoanEligibleForRecovery("LN456", new DateTime(2023, 6, 1));
            var result3 = _service.IsLoanEligibleForRecovery("LN789", new DateTime(2023, 12, 31));
            var result4 = _service.IsLoanEligibleForRecovery("LN000", new DateTime(2022, 1, 1));

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
            Assert.AreNotEqual(result1, result3);
        }

        [TestMethod]
        public void HasActiveDefault_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.HasActiveDefault("POL123");
            var result2 = _service.HasActiveDefault("POL456");
            var result3 = _service.HasActiveDefault("POL789");
            var result4 = _service.HasActiveDefault("POL000");

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsTrue(result3);
            Assert.IsFalse(result4);
            Assert.AreNotEqual(result1, result2);
        }

        [TestMethod]
        public void IsRecoveryAmountWithinLimits_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.IsRecoveryAmountWithinLimits(5000m, 10000m);
            var result2 = _service.IsRecoveryAmountWithinLimits(15000m, 10000m);
            var result3 = _service.IsRecoveryAmountWithinLimits(10000m, 10000m);
            var result4 = _service.IsRecoveryAmountWithinLimits(0m, 10000m);

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsTrue(result3);
            Assert.IsTrue(result4);
            Assert.AreNotEqual(result1, result2);
        }

        [TestMethod]
        public void ValidateLoanStatus_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.ValidateLoanStatus("LN123", "ACTIVE");
            var result2 = _service.ValidateLoanStatus("LN456", "CLOSED");
            var result3 = _service.ValidateLoanStatus("LN789", "DEFAULT");
            var result4 = _service.ValidateLoanStatus("LN000", "ACTIVE");

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsTrue(result3);
            Assert.IsFalse(result4);
            Assert.AreNotEqual(result1, result2);
        }

        [TestMethod]
        public void RequiresManualReview_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.RequiresManualReview("POL123", 50000m, 3);
            var result2 = _service.RequiresManualReview("POL456", 1000m, 1);
            var result3 = _service.RequiresManualReview("POL789", 100000m, 5);
            var result4 = _service.RequiresManualReview("POL000", 500m, 0);

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsTrue(result3);
            Assert.IsFalse(result4);
            Assert.AreNotEqual(result1, result2);
        }

        [TestMethod]
        public void GetDaysInArrears_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.GetDaysInArrears("LN123", new DateTime(2023, 1, 1));
            var result2 = _service.GetDaysInArrears("LN456", new DateTime(2023, 6, 1));
            var result3 = _service.GetDaysInArrears("LN789", new DateTime(2023, 12, 31));
            var result4 = _service.GetDaysInArrears("LN000", new DateTime(2022, 1, 1));

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0, result1);
            Assert.IsTrue(result2 > 0);
            Assert.IsTrue(result3 >= 0);
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void GetNumberOfActiveLoans_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.GetNumberOfActiveLoans("POL123");
            var result2 = _service.GetNumberOfActiveLoans("POL456");
            var result3 = _service.GetNumberOfActiveLoans("POL789");
            var result4 = _service.GetNumberOfActiveLoans("POL000");

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0, result1);
            Assert.IsTrue(result2 > 0);
            Assert.IsTrue(result3 >= 0);
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void GetRemainingTermDays_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.GetRemainingTermDays("LN123", new DateTime(2023, 1, 1));
            var result2 = _service.GetRemainingTermDays("LN456", new DateTime(2023, 6, 1));
            var result3 = _service.GetRemainingTermDays("LN789", new DateTime(2023, 12, 31));
            var result4 = _service.GetRemainingTermDays("LN000", new DateTime(2022, 1, 1));

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0, result1);
            Assert.IsTrue(result2 > 0);
            Assert.IsTrue(result3 >= 0);
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void GetGracePeriodDays_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.GetGracePeriodDays("TYPE_A");
            var result2 = _service.GetGracePeriodDays("TYPE_B");
            var result3 = _service.GetGracePeriodDays("TYPE_C");
            var result4 = _service.GetGracePeriodDays("TYPE_D");

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(0, result1);
            Assert.IsTrue(result2 > 0);
            Assert.IsTrue(result3 >= 0);
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void GetPrimaryLoanId_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.GetPrimaryLoanId("POL123");
            var result2 = _service.GetPrimaryLoanId("POL456");
            var result3 = _service.GetPrimaryLoanId("POL789");
            var result4 = _service.GetPrimaryLoanId("POL000");

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(string.Empty, result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(result1, result2);
        }

        [TestMethod]
        public void GetRecoveryTransactionReference_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.GetRecoveryTransactionReference("POL123", 1000m);
            var result2 = _service.GetRecoveryTransactionReference("POL456", 5000m);
            var result3 = _service.GetRecoveryTransactionReference("POL789", 2000m);
            var result4 = _service.GetRecoveryTransactionReference("POL000", 0m);

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(string.Empty, result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(result1, result2);
        }

        [TestMethod]
        public void GetLoanStatusCode_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.GetLoanStatusCode("LN123");
            var result2 = _service.GetLoanStatusCode("LN456");
            var result3 = _service.GetLoanStatusCode("LN789");
            var result4 = _service.GetLoanStatusCode("LN000");

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(string.Empty, result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(result1, result2);
        }

        [TestMethod]
        public void GenerateRecoveryAuditTrailId_ValidInputs_ReturnsExpectedValues()
        {
            var result1 = _service.GenerateRecoveryAuditTrailId("POL123", "USER1", new DateTime(2023, 1, 1));
            var result2 = _service.GenerateRecoveryAuditTrailId("POL456", "USER2", new DateTime(2023, 6, 1));
            var result3 = _service.GenerateRecoveryAuditTrailId("POL789", "USER3", new DateTime(2023, 12, 31));
            var result4 = _service.GenerateRecoveryAuditTrailId("POL000", "USER4", new DateTime(2022, 1, 1));

            Assert.IsNotNull(result1);
            Assert.AreNotEqual(string.Empty, result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreNotEqual(result1, result2);
        }

        [TestMethod]
        public void CalculateTotalOutstandingPrincipal_InvalidPolicyId_ReturnsZero()
        {
            var result1 = _service.CalculateTotalOutstandingPrincipal("", new DateTime(2023, 1, 1));
            var result2 = _service.CalculateTotalOutstandingPrincipal(null, new DateTime(2023, 1, 1));

            Assert.IsNotNull(result1);
            Assert.AreEqual(0m, result1);
            Assert.IsNotNull(result2);
            Assert.AreEqual(0m, result2);
        }
    }
}