using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.BeneficiaryAndLegalHeirs;

namespace MaturityBenefitProc.Tests.Helpers.BeneficiaryAndLegalHeirs
{
    [TestClass]
    public class MinorNomineeTrusteeServiceTests
    {
        private IMinorNomineeTrusteeService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming MinorNomineeTrusteeService implements IMinorNomineeTrusteeService
            _service = new MinorNomineeTrusteeService();
        }

        [TestMethod]
        public void IsNomineeMinor_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.IsNomineeMinor("NOM123", new DateTime(2010, 1, 1), new DateTime(2025, 1, 1));
            var result2 = _service.IsNomineeMinor("NOM124", new DateTime(2000, 1, 1), new DateTime(2025, 1, 1));
            var result3 = _service.IsNomineeMinor("NOM125", new DateTime(2020, 5, 5), new DateTime(2025, 1, 1));
            var result4 = _service.IsNomineeMinor("NOM126", new DateTime(2007, 1, 1), new DateTime(2025, 1, 1));

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsTrue(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void IsNomineeMinor_EdgeCases_ReturnsExpectedResults()
        {
            var result1 = _service.IsNomineeMinor("", new DateTime(2010, 1, 1), new DateTime(2025, 1, 1));
            var result2 = _service.IsNomineeMinor(null, new DateTime(2010, 1, 1), new DateTime(2025, 1, 1));
            var result3 = _service.IsNomineeMinor("NOM123", DateTime.MinValue, DateTime.MaxValue);
            var result4 = _service.IsNomineeMinor("NOM123", DateTime.MaxValue, DateTime.MinValue);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void ValidateAppointeeEligibility_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.ValidateAppointeeEligibility("APP001", "PARENT");
            var result2 = _service.ValidateAppointeeEligibility("APP002", "GUARDIAN");
            var result3 = _service.ValidateAppointeeEligibility("APP003", "FRIEND");
            var result4 = _service.ValidateAppointeeEligibility("APP004", "SIBLING");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsTrue(result4);
        }

        [TestMethod]
        public void ValidateAppointeeEligibility_InvalidInputs_ReturnsFalse()
        {
            var result1 = _service.ValidateAppointeeEligibility("", "PARENT");
            var result2 = _service.ValidateAppointeeEligibility(null, "PARENT");
            var result3 = _service.ValidateAppointeeEligibility("APP001", "");
            var result4 = _service.ValidateAppointeeEligibility("APP001", null);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void HasActiveTrusteeMandate_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.HasActiveTrusteeMandate("POL123", "NOM123");
            var result2 = _service.HasActiveTrusteeMandate("POL124", "NOM124");
            var result3 = _service.HasActiveTrusteeMandate("POL125", "NOM125");
            var result4 = _service.HasActiveTrusteeMandate("POL126", "NOM126");

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsTrue(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void HasActiveTrusteeMandate_InvalidInputs_ReturnsFalse()
        {
            var result1 = _service.HasActiveTrusteeMandate("", "NOM123");
            var result2 = _service.HasActiveTrusteeMandate(null, "NOM123");
            var result3 = _service.HasActiveTrusteeMandate("POL123", "");
            var result4 = _service.HasActiveTrusteeMandate("POL123", null);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void IsPayoutAllowedToAppointee_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.IsPayoutAllowedToAppointee("APP001", 5000m);
            var result2 = _service.IsPayoutAllowedToAppointee("APP002", 150000m);
            var result3 = _service.IsPayoutAllowedToAppointee("APP003", 0m);
            var result4 = _service.IsPayoutAllowedToAppointee("APP004", -100m);

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void VerifyAppointeeKycStatus_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.VerifyAppointeeKycStatus("APP001");
            var result2 = _service.VerifyAppointeeKycStatus("APP002");
            var result3 = _service.VerifyAppointeeKycStatus("APP003");
            var result4 = _service.VerifyAppointeeKycStatus("APP004");

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsTrue(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void VerifyAppointeeKycStatus_InvalidInputs_ReturnsFalse()
        {
            var result1 = _service.VerifyAppointeeKycStatus("");
            var result2 = _service.VerifyAppointeeKycStatus(null);
            var result3 = _service.VerifyAppointeeKycStatus("   ");
            var result4 = _service.VerifyAppointeeKycStatus("INVALID_APP");

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void CalculateAppointeePayoutShare_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.CalculateAppointeePayoutShare(10000m, 50.0);
            var result2 = _service.CalculateAppointeePayoutShare(50000m, 25.5);
            var result3 = _service.CalculateAppointeePayoutShare(0m, 50.0);
            var result4 = _service.CalculateAppointeePayoutShare(10000m, 0.0);

            Assert.AreEqual(5000m, result1);
            Assert.AreEqual(12750m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void GetTotalDisbursedToAppointee_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.GetTotalDisbursedToAppointee("APP001", "POL123");
            var result2 = _service.GetTotalDisbursedToAppointee("APP002", "POL124");
            var result3 = _service.GetTotalDisbursedToAppointee("APP003", "POL125");
            var result4 = _service.GetTotalDisbursedToAppointee("APP004", "POL126");

            Assert.AreEqual(15000m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(5000m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void CalculateTrusteeMaintenanceAllowance_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.CalculateTrusteeMaintenanceAllowance(10000m, 12);
            var result2 = _service.CalculateTrusteeMaintenanceAllowance(50000m, 6);
            var result3 = _service.CalculateTrusteeMaintenanceAllowance(0m, 12);
            var result4 = _service.CalculateTrusteeMaintenanceAllowance(10000m, 0);

            Assert.AreEqual(1200m, result1);
            Assert.AreEqual(3000m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void ComputeMinorEducationFundAllocation_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.ComputeMinorEducationFundAllocation(100000m, 20.0);
            var result2 = _service.ComputeMinorEducationFundAllocation(50000m, 15.5);
            var result3 = _service.ComputeMinorEducationFundAllocation(0m, 20.0);
            var result4 = _service.ComputeMinorEducationFundAllocation(100000m, 0.0);

            Assert.AreEqual(20000m, result1);
            Assert.AreEqual(7750m, result2);
            Assert.AreEqual(0m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void GetPendingPayoutAmount_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.GetPendingPayoutAmount("NOM123", "POL123");
            var result2 = _service.GetPendingPayoutAmount("NOM124", "POL124");
            var result3 = _service.GetPendingPayoutAmount("NOM125", "POL125");
            var result4 = _service.GetPendingPayoutAmount("NOM126", "POL126");

            Assert.AreEqual(25000m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(10000m, result3);
            Assert.AreEqual(0m, result4);
        }

        [TestMethod]
        public void GetAppointeeSharePercentage_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.GetAppointeeSharePercentage("NOM123", "APP001");
            var result2 = _service.GetAppointeeSharePercentage("NOM124", "APP002");
            var result3 = _service.GetAppointeeSharePercentage("NOM125", "APP003");
            var result4 = _service.GetAppointeeSharePercentage("NOM126", "APP004");

            Assert.AreEqual(100.0, result1);
            Assert.AreEqual(50.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void CalculateTrusteeFeeRate_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.CalculateTrusteeFeeRate(365);
            var result2 = _service.CalculateTrusteeFeeRate(180);
            var result3 = _service.CalculateTrusteeFeeRate(0);
            var result4 = _service.CalculateTrusteeFeeRate(-10);

            Assert.AreEqual(2.5, result1);
            Assert.AreEqual(1.25, result2);
            Assert.AreEqual(0.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void GetMinorAgeRatioAtMaturity_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.GetMinorAgeRatioAtMaturity(new DateTime(2010, 1, 1), new DateTime(2020, 1, 1));
            var result2 = _service.GetMinorAgeRatioAtMaturity(new DateTime(2015, 1, 1), new DateTime(2020, 1, 1));
            var result3 = _service.GetMinorAgeRatioAtMaturity(new DateTime(2000, 1, 1), new DateTime(2020, 1, 1));
            var result4 = _service.GetMinorAgeRatioAtMaturity(new DateTime(2020, 1, 1), new DateTime(2010, 1, 1));

            Assert.AreEqual(0.55, result1, 0.01);
            Assert.AreEqual(0.27, result2, 0.01);
            Assert.AreEqual(1.11, result3, 0.01);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void GetTaxDeductionRateForAppointee_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.GetTaxDeductionRateForAppointee("APP001", "TAX01");
            var result2 = _service.GetTaxDeductionRateForAppointee("APP002", "TAX02");
            var result3 = _service.GetTaxDeductionRateForAppointee("APP003", "TAX03");
            var result4 = _service.GetTaxDeductionRateForAppointee("APP004", "TAX04");

            Assert.AreEqual(10.0, result1);
            Assert.AreEqual(15.0, result2);
            Assert.AreEqual(5.0, result3);
            Assert.AreEqual(0.0, result4);
        }

        [TestMethod]
        public void GetDaysUntilNomineeMajority_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.GetDaysUntilNomineeMajority(new DateTime(2010, 1, 1), new DateTime(2020, 1, 1));
            var result2 = _service.GetDaysUntilNomineeMajority(new DateTime(2000, 1, 1), new DateTime(2020, 1, 1));
            var result3 = _service.GetDaysUntilNomineeMajority(new DateTime(2015, 1, 1), new DateTime(2020, 1, 1));
            var result4 = _service.GetDaysUntilNomineeMajority(new DateTime(2020, 1, 1), new DateTime(2010, 1, 1));

            Assert.AreEqual(2922, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(4748, result3);
            Assert.AreEqual(6574, result4);
        }

        [TestMethod]
        public void CountActiveAppointeesForMinor_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.CountActiveAppointeesForMinor("NOM123");
            var result2 = _service.CountActiveAppointeesForMinor("NOM124");
            var result3 = _service.CountActiveAppointeesForMinor("NOM125");
            var result4 = _service.CountActiveAppointeesForMinor("NOM126");

            Assert.AreEqual(1, result1);
            Assert.AreEqual(2, result2);
            Assert.AreEqual(0, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void GetTrusteeMandateDurationMonths_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.GetTrusteeMandateDurationMonths(new DateTime(2020, 1, 1), new DateTime(2021, 1, 1));
            var result2 = _service.GetTrusteeMandateDurationMonths(new DateTime(2020, 1, 1), new DateTime(2020, 7, 1));
            var result3 = _service.GetTrusteeMandateDurationMonths(new DateTime(2020, 1, 1), new DateTime(2020, 1, 1));
            var result4 = _service.GetTrusteeMandateDurationMonths(new DateTime(2021, 1, 1), new DateTime(2020, 1, 1));

            Assert.AreEqual(12, result1);
            Assert.AreEqual(6, result2);
            Assert.AreEqual(0, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void GetNumberOfInstallmentsProcessed_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.GetNumberOfInstallmentsProcessed("APP001", "POL123");
            var result2 = _service.GetNumberOfInstallmentsProcessed("APP002", "POL124");
            var result3 = _service.GetNumberOfInstallmentsProcessed("APP003", "POL125");
            var result4 = _service.GetNumberOfInstallmentsProcessed("APP004", "POL126");

            Assert.AreEqual(5, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(2, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void GetAppointeeAge_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.GetAppointeeAge(new DateTime(1980, 1, 1), new DateTime(2020, 1, 1));
            var result2 = _service.GetAppointeeAge(new DateTime(1990, 6, 15), new DateTime(2020, 1, 1));
            var result3 = _service.GetAppointeeAge(new DateTime(2000, 12, 31), new DateTime(2020, 1, 1));
            var result4 = _service.GetAppointeeAge(new DateTime(2020, 1, 1), new DateTime(2010, 1, 1));

            Assert.AreEqual(40, result1);
            Assert.AreEqual(29, result2);
            Assert.AreEqual(19, result3);
            Assert.AreEqual(0, result4);
        }

        [TestMethod]
        public void RegisterNewAppointee_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.RegisterNewAppointee("NOM123", "John", "Doe", new DateTime(1980, 1, 1));
            var result2 = _service.RegisterNewAppointee("NOM124", "Jane", "Smith", new DateTime(1990, 1, 1));
            var result3 = _service.RegisterNewAppointee("NOM125", "Bob", "Johnson", new DateTime(1975, 1, 1));
            var result4 = _service.RegisterNewAppointee("NOM126", "Alice", "Williams", new DateTime(1985, 1, 1));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void GetPrimaryAppointeeId_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.GetPrimaryAppointeeId("NOM123");
            var result2 = _service.GetPrimaryAppointeeId("NOM124");
            var result3 = _service.GetPrimaryAppointeeId("NOM125");
            var result4 = _service.GetPrimaryAppointeeId("NOM126");

            Assert.AreEqual("APP001", result1);
            Assert.AreEqual("APP002", result2);
            Assert.IsNull(result3);
            Assert.IsNull(result4);
        }

        [TestMethod]
        public void GenerateTrusteeMandateReference_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.GenerateTrusteeMandateReference("POL123", "APP001");
            var result2 = _service.GenerateTrusteeMandateReference("POL124", "APP002");
            var result3 = _service.GenerateTrusteeMandateReference("POL125", "APP003");
            var result4 = _service.GenerateTrusteeMandateReference("POL126", "APP004");

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsNotNull(result4);
        }

        [TestMethod]
        public void GetAppointeeRelationshipCode_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.GetAppointeeRelationshipCode("NOM123", "APP001");
            var result2 = _service.GetAppointeeRelationshipCode("NOM124", "APP002");
            var result3 = _service.GetAppointeeRelationshipCode("NOM125", "APP003");
            var result4 = _service.GetAppointeeRelationshipCode("NOM126", "APP004");

            Assert.AreEqual("PARENT", result1);
            Assert.AreEqual("GUARDIAN", result2);
            Assert.IsNull(result3);
            Assert.IsNull(result4);
        }

        [TestMethod]
        public void RetrieveAppointeeBankAccountId_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.RetrieveAppointeeBankAccountId("APP001");
            var result2 = _service.RetrieveAppointeeBankAccountId("APP002");
            var result3 = _service.RetrieveAppointeeBankAccountId("APP003");
            var result4 = _service.RetrieveAppointeeBankAccountId("APP004");

            Assert.AreEqual("ACC123456", result1);
            Assert.AreEqual("ACC654321", result2);
            Assert.IsNull(result3);
            Assert.IsNull(result4);
        }

        [TestMethod]
        public void ResolvePayoutStatusCategory_ValidInputs_ReturnsExpectedResults()
        {
            var result1 = _service.ResolvePayoutStatusCategory("POL123", "APP001");
            var result2 = _service.ResolvePayoutStatusCategory("POL124", "APP002");
            var result3 = _service.ResolvePayoutStatusCategory("POL125", "APP003");
            var result4 = _service.ResolvePayoutStatusCategory("POL126", "APP004");

            Assert.AreEqual("ACTIVE", result1);
            Assert.AreEqual("PENDING", result2);
            Assert.AreEqual("HOLD", result3);
            Assert.AreEqual("UNKNOWN", result4);
        }
    }
}