using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MaturityBenefitProc.Helpers.BeneficiaryAndLegalHeirs;

namespace MaturityBenefitProc.Tests.Helpers.BeneficiaryAndLegalHeirs
{
    [TestClass]
    public class MinorNomineeTrusteeServiceMockTests
    {
        private Mock<IMinorNomineeTrusteeService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IMinorNomineeTrusteeService>();
        }

        [TestMethod]
        public void IsNomineeMinor_ValidInput_ReturnsTrue()
        {
            var expectedValue = true;
            _mockService.Setup(s => s.IsNomineeMinor(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.IsNomineeMinor("NOM123", new DateTime(2010, 1, 1), new DateTime(2023, 1, 1));

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsNomineeMinor(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void IsNomineeMinor_AdultInput_ReturnsFalse()
        {
            var expectedValue = false;
            _mockService.Setup(s => s.IsNomineeMinor(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.IsNomineeMinor("NOM123", new DateTime(1990, 1, 1), new DateTime(2023, 1, 1));

            Assert.AreEqual(expectedValue, result);
            Assert.IsFalse(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(true, result);
            _mockService.Verify(s => s.IsNomineeMinor(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void ValidateAppointeeEligibility_Valid_ReturnsTrue()
        {
            var expectedValue = true;
            _mockService.Setup(s => s.ValidateAppointeeEligibility(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.ValidateAppointeeEligibility("APP123", "FATHER");

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.ValidateAppointeeEligibility(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void HasActiveTrusteeMandate_Active_ReturnsTrue()
        {
            var expectedValue = true;
            _mockService.Setup(s => s.HasActiveTrusteeMandate(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.HasActiveTrusteeMandate("POL123", "NOM123");

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.HasActiveTrusteeMandate(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void IsPayoutAllowedToAppointee_Allowed_ReturnsTrue()
        {
            var expectedValue = true;
            _mockService.Setup(s => s.IsPayoutAllowedToAppointee(It.IsAny<string>(), It.IsAny<decimal>())).Returns(expectedValue);

            var result = _mockService.Object.IsPayoutAllowedToAppointee("APP123", 5000m);

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.IsPayoutAllowedToAppointee(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once());
        }

        [TestMethod]
        public void VerifyAppointeeKycStatus_Verified_ReturnsTrue()
        {
            var expectedValue = true;
            _mockService.Setup(s => s.VerifyAppointeeKycStatus(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.VerifyAppointeeKycStatus("APP123");

            Assert.AreEqual(expectedValue, result);
            Assert.IsTrue(result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(false, result);
            _mockService.Verify(s => s.VerifyAppointeeKycStatus(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateAppointeePayoutShare_ValidInput_ReturnsExpected()
        {
            var expectedValue = 5000m;
            _mockService.Setup(s => s.CalculateAppointeePayoutShare(It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateAppointeePayoutShare(10000m, 0.5);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.CalculateAppointeePayoutShare(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetTotalDisbursedToAppointee_ValidInput_ReturnsExpected()
        {
            var expectedValue = 15000m;
            _mockService.Setup(s => s.GetTotalDisbursedToAppointee(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetTotalDisbursedToAppointee("APP123", "POL123");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.GetTotalDisbursedToAppointee(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTrusteeMaintenanceAllowance_ValidInput_ReturnsExpected()
        {
            var expectedValue = 1200m;
            _mockService.Setup(s => s.CalculateTrusteeMaintenanceAllowance(It.IsAny<decimal>(), It.IsAny<int>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateTrusteeMaintenanceAllowance(100m, 12);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.CalculateTrusteeMaintenanceAllowance(It.IsAny<decimal>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void ComputeMinorEducationFundAllocation_ValidInput_ReturnsExpected()
        {
            var expectedValue = 2000m;
            _mockService.Setup(s => s.ComputeMinorEducationFundAllocation(It.IsAny<decimal>(), It.IsAny<double>())).Returns(expectedValue);

            var result = _mockService.Object.ComputeMinorEducationFundAllocation(10000m, 0.2);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.ComputeMinorEducationFundAllocation(It.IsAny<decimal>(), It.IsAny<double>()), Times.Once());
        }

        [TestMethod]
        public void GetPendingPayoutAmount_ValidInput_ReturnsExpected()
        {
            var expectedValue = 3000m;
            _mockService.Setup(s => s.GetPendingPayoutAmount(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetPendingPayoutAmount("NOM123", "POL123");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0m, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.GetPendingPayoutAmount(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetAppointeeSharePercentage_ValidInput_ReturnsExpected()
        {
            var expectedValue = 100.0;
            _mockService.Setup(s => s.GetAppointeeSharePercentage(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetAppointeeSharePercentage("NOM123", "APP123");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.GetAppointeeSharePercentage(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void CalculateTrusteeFeeRate_ValidInput_ReturnsExpected()
        {
            var expectedValue = 1.5;
            _mockService.Setup(s => s.CalculateTrusteeFeeRate(It.IsAny<int>())).Returns(expectedValue);

            var result = _mockService.Object.CalculateTrusteeFeeRate(365);

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.CalculateTrusteeFeeRate(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void GetMinorAgeRatioAtMaturity_ValidInput_ReturnsExpected()
        {
            var expectedValue = 0.8;
            _mockService.Setup(s => s.GetMinorAgeRatioAtMaturity(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetMinorAgeRatioAtMaturity(new DateTime(2010, 1, 1), new DateTime(2023, 1, 1));

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.GetMinorAgeRatioAtMaturity(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetTaxDeductionRateForAppointee_ValidInput_ReturnsExpected()
        {
            var expectedValue = 10.0;
            _mockService.Setup(s => s.GetTaxDeductionRateForAppointee(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetTaxDeductionRateForAppointee("APP123", "TAX01");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0.0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.GetTaxDeductionRateForAppointee(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetDaysUntilNomineeMajority_ValidInput_ReturnsExpected()
        {
            var expectedValue = 1000;
            _mockService.Setup(s => s.GetDaysUntilNomineeMajority(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetDaysUntilNomineeMajority(new DateTime(2010, 1, 1), new DateTime(2023, 1, 1));

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.GetDaysUntilNomineeMajority(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void CountActiveAppointeesForMinor_ValidInput_ReturnsExpected()
        {
            var expectedValue = 1;
            _mockService.Setup(s => s.CountActiveAppointeesForMinor(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.CountActiveAppointeesForMinor("NOM123");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.CountActiveAppointeesForMinor(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetTrusteeMandateDurationMonths_ValidInput_ReturnsExpected()
        {
            var expectedValue = 24;
            _mockService.Setup(s => s.GetTrusteeMandateDurationMonths(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetTrusteeMandateDurationMonths(new DateTime(2021, 1, 1), new DateTime(2023, 1, 1));

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.GetTrusteeMandateDurationMonths(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetNumberOfInstallmentsProcessed_ValidInput_ReturnsExpected()
        {
            var expectedValue = 5;
            _mockService.Setup(s => s.GetNumberOfInstallmentsProcessed(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetNumberOfInstallmentsProcessed("APP123", "POL123");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.GetNumberOfInstallmentsProcessed(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetAppointeeAge_ValidInput_ReturnsExpected()
        {
            var expectedValue = 40;
            _mockService.Setup(s => s.GetAppointeeAge(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.GetAppointeeAge(new DateTime(1980, 1, 1), new DateTime(2020, 1, 1));

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result > 0);
            _mockService.Verify(s => s.GetAppointeeAge(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void RegisterNewAppointee_ValidInput_ReturnsExpected()
        {
            var expectedValue = "APP999";
            _mockService.Setup(s => s.RegisterNewAppointee(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(expectedValue);

            var result = _mockService.Object.RegisterNewAppointee("NOM123", "John", "Doe", new DateTime(1980, 1, 1));

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(string.Empty, result);
            Assert.IsTrue(result.Length > 0);
            _mockService.Verify(s => s.RegisterNewAppointee(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void GetPrimaryAppointeeId_ValidInput_ReturnsExpected()
        {
            var expectedValue = "APP123";
            _mockService.Setup(s => s.GetPrimaryAppointeeId(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetPrimaryAppointeeId("NOM123");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(string.Empty, result);
            Assert.IsTrue(result.Length > 0);
            _mockService.Verify(s => s.GetPrimaryAppointeeId(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GenerateTrusteeMandateReference_ValidInput_ReturnsExpected()
        {
            var expectedValue = "MANDATE123";
            _mockService.Setup(s => s.GenerateTrusteeMandateReference(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GenerateTrusteeMandateReference("POL123", "APP123");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(string.Empty, result);
            Assert.IsTrue(result.Length > 0);
            _mockService.Verify(s => s.GenerateTrusteeMandateReference(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GetAppointeeRelationshipCode_ValidInput_ReturnsExpected()
        {
            var expectedValue = "FATHER";
            _mockService.Setup(s => s.GetAppointeeRelationshipCode(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.GetAppointeeRelationshipCode("NOM123", "APP123");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(string.Empty, result);
            Assert.IsTrue(result.Length > 0);
            _mockService.Verify(s => s.GetAppointeeRelationshipCode(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void RetrieveAppointeeBankAccountId_ValidInput_ReturnsExpected()
        {
            var expectedValue = "BANK123";
            _mockService.Setup(s => s.RetrieveAppointeeBankAccountId(It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.RetrieveAppointeeBankAccountId("APP123");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(string.Empty, result);
            Assert.IsTrue(result.Length > 0);
            _mockService.Verify(s => s.RetrieveAppointeeBankAccountId(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ResolvePayoutStatusCategory_ValidInput_ReturnsExpected()
        {
            var expectedValue = "PENDING";
            _mockService.Setup(s => s.ResolvePayoutStatusCategory(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedValue);

            var result = _mockService.Object.ResolvePayoutStatusCategory("POL123", "APP123");

            Assert.AreEqual(expectedValue, result);
            Assert.IsNotNull(result);
            Assert.AreNotEqual(string.Empty, result);
            Assert.IsTrue(result.Length > 0);
            _mockService.Verify(s => s.ResolvePayoutStatusCategory(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }
    }
}