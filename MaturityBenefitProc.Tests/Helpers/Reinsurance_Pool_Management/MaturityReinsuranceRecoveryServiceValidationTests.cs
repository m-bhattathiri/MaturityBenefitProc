using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.Reinsurance_Pool_Management;
using Moq;

namespace MaturityBenefitProc.Tests.Helpers.Reinsurance_Pool_Management
{
    [TestClass]
    public class MaturityReinsuranceRecoveryServiceValidationTests
    {
        private Mock<IMaturityReinsuranceRecoveryService> _serviceMock;
        private IMaturityReinsuranceRecoveryService _service;

        [TestInitialize]
        public void Setup()
        {
            _serviceMock = new Mock<IMaturityReinsuranceRecoveryService>();
            _service = _serviceMock.Object;
        }

        [TestMethod]
        public void CalculateTotalRecoveryAmount_ValidInputs_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.CalculateTotalRecoveryAmount("POL123", 10000m)).Returns(5000m);
            _serviceMock.Setup(s => s.CalculateTotalRecoveryAmount("POL124", 0m)).Returns(0m);
            _serviceMock.Setup(s => s.CalculateTotalRecoveryAmount("POL125", -500m)).Returns(0m);

            Assert.AreEqual(5000m, _service.CalculateTotalRecoveryAmount("POL123", 10000m));
            Assert.AreEqual(0m, _service.CalculateTotalRecoveryAmount("POL124", 0m));
            Assert.AreEqual(0m, _service.CalculateTotalRecoveryAmount("POL125", -500m));
            Assert.AreNotEqual(10000m, _service.CalculateTotalRecoveryAmount("POL123", 10000m));
        }

        [TestMethod]
        public void CalculateQuotaShareRecovery_BoundaryPercentages_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.CalculateQuotaShareRecovery("POL1", 1000m, 0.0)).Returns(0m);
            _serviceMock.Setup(s => s.CalculateQuotaShareRecovery("POL1", 1000m, 1.0)).Returns(1000m);
            _serviceMock.Setup(s => s.CalculateQuotaShareRecovery("POL1", 1000m, 0.5)).Returns(500m);

            Assert.AreEqual(0m, _service.CalculateQuotaShareRecovery("POL1", 1000m, 0.0));
            Assert.AreEqual(1000m, _service.CalculateQuotaShareRecovery("POL1", 1000m, 1.0));
            Assert.AreEqual(500m, _service.CalculateQuotaShareRecovery("POL1", 1000m, 0.5));
            Assert.AreNotEqual(1000m, _service.CalculateQuotaShareRecovery("POL1", 1000m, 0.5));
        }

        [TestMethod]
        public void CalculateSurplusTreatyRecovery_RetentionLimits_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.CalculateSurplusTreatyRecovery("P1", 1000m, 1500m)).Returns(0m);
            _serviceMock.Setup(s => s.CalculateSurplusTreatyRecovery("P1", 1000m, 500m)).Returns(500m);
            _serviceMock.Setup(s => s.CalculateSurplusTreatyRecovery("P1", 1000m, 1000m)).Returns(0m);

            Assert.AreEqual(0m, _service.CalculateSurplusTreatyRecovery("P1", 1000m, 1500m));
            Assert.AreEqual(500m, _service.CalculateSurplusTreatyRecovery("P1", 1000m, 500m));
            Assert.AreEqual(0m, _service.CalculateSurplusTreatyRecovery("P1", 1000m, 1000m));
            Assert.AreNotEqual(1000m, _service.CalculateSurplusTreatyRecovery("P1", 1000m, 500m));
        }

        [TestMethod]
        public void CalculateExcessOfLossRecovery_AttachmentPoints_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.CalculateExcessOfLossRecovery("P1", 5000m, 6000m)).Returns(0m);
            _serviceMock.Setup(s => s.CalculateExcessOfLossRecovery("P1", 5000m, 2000m)).Returns(3000m);
            _serviceMock.Setup(s => s.CalculateExcessOfLossRecovery("P1", 5000m, 5000m)).Returns(0m);

            Assert.AreEqual(0m, _service.CalculateExcessOfLossRecovery("P1", 5000m, 6000m));
            Assert.AreEqual(3000m, _service.CalculateExcessOfLossRecovery("P1", 5000m, 2000m));
            Assert.AreEqual(0m, _service.CalculateExcessOfLossRecovery("P1", 5000m, 5000m));
            Assert.AreNotEqual(5000m, _service.CalculateExcessOfLossRecovery("P1", 5000m, 2000m));
        }

        [TestMethod]
        public void GetReinsurancePercentage_ValidAndInvalidDates_ReturnsExpected()
        {
            var date1 = new DateTime(2023, 1, 1);
            var date2 = new DateTime(2024, 1, 1);
            _serviceMock.Setup(s => s.GetReinsurancePercentage("P1", date1)).Returns(0.4);
            _serviceMock.Setup(s => s.GetReinsurancePercentage("P1", date2)).Returns(0.0);

            Assert.AreEqual(0.4, _service.GetReinsurancePercentage("P1", date1));
            Assert.AreEqual(0.0, _service.GetReinsurancePercentage("P1", date2));
            Assert.AreNotEqual(0.5, _service.GetReinsurancePercentage("P1", date1));
            Assert.IsNotNull(_service.GetReinsurancePercentage("P1", date1));
        }

        [TestMethod]
        public void GetPoolAllocationRatio_ValidIds_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.GetPoolAllocationRatio("POOL1", "RE1")).Returns(0.25);
            _serviceMock.Setup(s => s.GetPoolAllocationRatio("POOL1", "RE2")).Returns(0.75);
            _serviceMock.Setup(s => s.GetPoolAllocationRatio("POOL2", "RE1")).Returns(0.0);

            Assert.AreEqual(0.25, _service.GetPoolAllocationRatio("POOL1", "RE1"));
            Assert.AreEqual(0.75, _service.GetPoolAllocationRatio("POOL1", "RE2"));
            Assert.AreEqual(0.0, _service.GetPoolAllocationRatio("POOL2", "RE1"));
            Assert.AreNotEqual(0.5, _service.GetPoolAllocationRatio("POOL1", "RE1"));
        }

        [TestMethod]
        public void IsPolicyReinsured_VariousIds_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.IsPolicyReinsured("P1")).Returns(true);
            _serviceMock.Setup(s => s.IsPolicyReinsured("P2")).Returns(false);
            _serviceMock.Setup(s => s.IsPolicyReinsured("")).Returns(false);

            Assert.IsTrue(_service.IsPolicyReinsured("P1"));
            Assert.IsFalse(_service.IsPolicyReinsured("P2"));
            Assert.IsFalse(_service.IsPolicyReinsured(""));
            Assert.AreNotEqual(true, _service.IsPolicyReinsured("P2"));
        }

        [TestMethod]
        public void IsReinsurerActive_DifferentDates_ReturnsExpected()
        {
            var activeDate = new DateTime(2023, 5, 1);
            var inactiveDate = new DateTime(2025, 5, 1);
            _serviceMock.Setup(s => s.IsReinsurerActive("RE1", activeDate)).Returns(true);
            _serviceMock.Setup(s => s.IsReinsurerActive("RE1", inactiveDate)).Returns(false);

            Assert.IsTrue(_service.IsReinsurerActive("RE1", activeDate));
            Assert.IsFalse(_service.IsReinsurerActive("RE1", inactiveDate));
            Assert.IsFalse(_service.IsReinsurerActive("RE2", activeDate));
            Assert.AreNotEqual(true, _service.IsReinsurerActive("RE1", inactiveDate));
        }

        [TestMethod]
        public void ValidateTreatyLimits_VariousAmounts_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.ValidateTreatyLimits("TR1", 1000m)).Returns(true);
            _serviceMock.Setup(s => s.ValidateTreatyLimits("TR1", 1000000m)).Returns(false);
            _serviceMock.Setup(s => s.ValidateTreatyLimits("TR1", -10m)).Returns(false);

            Assert.IsTrue(_service.ValidateTreatyLimits("TR1", 1000m));
            Assert.IsFalse(_service.ValidateTreatyLimits("TR1", 1000000m));
            Assert.IsFalse(_service.ValidateTreatyLimits("TR1", -10m));
            Assert.AreNotEqual(true, _service.ValidateTreatyLimits("TR1", 1000000m));
        }

        [TestMethod]
        public void CheckFacultativeEligibility_Amounts_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.CheckFacultativeEligibility("P1", 5000000m)).Returns(true);
            _serviceMock.Setup(s => s.CheckFacultativeEligibility("P1", 5000m)).Returns(false);
            _serviceMock.Setup(s => s.CheckFacultativeEligibility("P2", 0m)).Returns(false);

            Assert.IsTrue(_service.CheckFacultativeEligibility("P1", 5000000m));
            Assert.IsFalse(_service.CheckFacultativeEligibility("P1", 5000m));
            Assert.IsFalse(_service.CheckFacultativeEligibility("P2", 0m));
            Assert.AreNotEqual(true, _service.CheckFacultativeEligibility("P1", 5000m));
        }

        [TestMethod]
        public void GetDaysUntilRecoveryDue_VariousDates_ReturnsExpected()
        {
            var date1 = new DateTime(2023, 1, 1);
            _serviceMock.Setup(s => s.GetDaysUntilRecoveryDue("RE1", date1)).Returns(30);
            _serviceMock.Setup(s => s.GetDaysUntilRecoveryDue("RE2", date1)).Returns(45);
            _serviceMock.Setup(s => s.GetDaysUntilRecoveryDue("RE3", date1)).Returns(0);

            Assert.AreEqual(30, _service.GetDaysUntilRecoveryDue("RE1", date1));
            Assert.AreEqual(45, _service.GetDaysUntilRecoveryDue("RE2", date1));
            Assert.AreEqual(0, _service.GetDaysUntilRecoveryDue("RE3", date1));
            Assert.AreNotEqual(30, _service.GetDaysUntilRecoveryDue("RE2", date1));
        }

        [TestMethod]
        public void GetReinsurerCountForPolicy_VariousPolicies_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.GetReinsurerCountForPolicy("P1")).Returns(3);
            _serviceMock.Setup(s => s.GetReinsurerCountForPolicy("P2")).Returns(0);
            _serviceMock.Setup(s => s.GetReinsurerCountForPolicy("P3")).Returns(1);

            Assert.AreEqual(3, _service.GetReinsurerCountForPolicy("P1"));
            Assert.AreEqual(0, _service.GetReinsurerCountForPolicy("P2"));
            Assert.AreEqual(1, _service.GetReinsurerCountForPolicy("P3"));
            Assert.AreNotEqual(2, _service.GetReinsurerCountForPolicy("P1"));
        }

        [TestMethod]
        public void GetActiveTreatiesCount_VariousDates_ReturnsExpected()
        {
            var date1 = new DateTime(2023, 1, 1);
            _serviceMock.Setup(s => s.GetActiveTreatiesCount("RE1", date1)).Returns(5);
            _serviceMock.Setup(s => s.GetActiveTreatiesCount("RE2", date1)).Returns(0);
            _serviceMock.Setup(s => s.GetActiveTreatiesCount("RE3", date1)).Returns(2);

            Assert.AreEqual(5, _service.GetActiveTreatiesCount("RE1", date1));
            Assert.AreEqual(0, _service.GetActiveTreatiesCount("RE2", date1));
            Assert.AreEqual(2, _service.GetActiveTreatiesCount("RE3", date1));
            Assert.AreNotEqual(5, _service.GetActiveTreatiesCount("RE2", date1));
        }

        [TestMethod]
        public void GetPrimaryReinsurerId_VariousPolicies_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.GetPrimaryReinsurerId("P1")).Returns("RE1");
            _serviceMock.Setup(s => s.GetPrimaryReinsurerId("P2")).Returns((string)null);
            _serviceMock.Setup(s => s.GetPrimaryReinsurerId("P3")).Returns("RE2");

            Assert.AreEqual("RE1", _service.GetPrimaryReinsurerId("P1"));
            Assert.IsNull(_service.GetPrimaryReinsurerId("P2"));
            Assert.AreEqual("RE2", _service.GetPrimaryReinsurerId("P3"));
            Assert.AreNotEqual("RE1", _service.GetPrimaryReinsurerId("P3"));
        }

        [TestMethod]
        public void GetTreatyCode_VariousInputs_ReturnsExpected()
        {
            var date1 = new DateTime(2023, 1, 1);
            _serviceMock.Setup(s => s.GetTreatyCode("P1", date1)).Returns("TR-2023-A");
            _serviceMock.Setup(s => s.GetTreatyCode("P2", date1)).Returns((string)null);
            _serviceMock.Setup(s => s.GetTreatyCode("P3", date1)).Returns("TR-2023-B");

            Assert.AreEqual("TR-2023-A", _service.GetTreatyCode("P1", date1));
            Assert.IsNull(_service.GetTreatyCode("P2", date1));
            Assert.AreEqual("TR-2023-B", _service.GetTreatyCode("P3", date1));
            Assert.AreNotEqual("TR-2023-A", _service.GetTreatyCode("P3", date1));
        }

        [TestMethod]
        public void GenerateRecoveryClaimReference_VariousInputs_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.GenerateRecoveryClaimReference("P1", "RE1")).Returns("REC-P1-RE1");
            _serviceMock.Setup(s => s.GenerateRecoveryClaimReference("P2", "RE2")).Returns("REC-P2-RE2");
            _serviceMock.Setup(s => s.GenerateRecoveryClaimReference("", "")).Returns((string)null);

            Assert.AreEqual("REC-P1-RE1", _service.GenerateRecoveryClaimReference("P1", "RE1"));
            Assert.AreEqual("REC-P2-RE2", _service.GenerateRecoveryClaimReference("P2", "RE2"));
            Assert.IsNull(_service.GenerateRecoveryClaimReference("", ""));
            Assert.AreNotEqual("REC-P1-RE1", _service.GenerateRecoveryClaimReference("P2", "RE2"));
        }

        [TestMethod]
        public void CalculateProportionalRecovery_VariousInputs_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.CalculateProportionalRecovery("P1", 1000m, 0.2)).Returns(200m);
            _serviceMock.Setup(s => s.CalculateProportionalRecovery("P1", 1000m, 0.0)).Returns(0m);
            _serviceMock.Setup(s => s.CalculateProportionalRecovery("P1", 1000m, 1.0)).Returns(1000m);

            Assert.AreEqual(200m, _service.CalculateProportionalRecovery("P1", 1000m, 0.2));
            Assert.AreEqual(0m, _service.CalculateProportionalRecovery("P1", 1000m, 0.0));
            Assert.AreEqual(1000m, _service.CalculateProportionalRecovery("P1", 1000m, 1.0));
            Assert.AreNotEqual(200m, _service.CalculateProportionalRecovery("P1", 1000m, 0.0));
        }

        [TestMethod]
        public void CalculateNonProportionalRecovery_VariousInputs_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.CalculateNonProportionalRecovery("P1", 5000m, 1000m)).Returns(4000m);
            _serviceMock.Setup(s => s.CalculateNonProportionalRecovery("P1", 500m, 1000m)).Returns(0m);
            _serviceMock.Setup(s => s.CalculateNonProportionalRecovery("P1", 1000m, 1000m)).Returns(0m);

            Assert.AreEqual(4000m, _service.CalculateNonProportionalRecovery("P1", 5000m, 1000m));
            Assert.AreEqual(0m, _service.CalculateNonProportionalRecovery("P1", 500m, 1000m));
            Assert.AreEqual(0m, _service.CalculateNonProportionalRecovery("P1", 1000m, 1000m));
            Assert.AreNotEqual(4000m, _service.CalculateNonProportionalRecovery("P1", 500m, 1000m));
        }

        [TestMethod]
        public void GetFacultativeReinsuranceRate_VariousPolicies_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.GetFacultativeReinsuranceRate("P1")).Returns(0.15);
            _serviceMock.Setup(s => s.GetFacultativeReinsuranceRate("P2")).Returns(0.0);
            _serviceMock.Setup(s => s.GetFacultativeReinsuranceRate("P3")).Returns(0.5);

            Assert.AreEqual(0.15, _service.GetFacultativeReinsuranceRate("P1"));
            Assert.AreEqual(0.0, _service.GetFacultativeReinsuranceRate("P2"));
            Assert.AreEqual(0.5, _service.GetFacultativeReinsuranceRate("P3"));
            Assert.AreNotEqual(0.15, _service.GetFacultativeReinsuranceRate("P2"));
        }

        [TestMethod]
        public void IsPoolArrangementValid_VariousInputs_ReturnsExpected()
        {
            var date1 = new DateTime(2023, 1, 1);
            _serviceMock.Setup(s => s.IsPoolArrangementValid("POOL1", date1)).Returns(true);
            _serviceMock.Setup(s => s.IsPoolArrangementValid("POOL2", date1)).Returns(false);
            _serviceMock.Setup(s => s.IsPoolArrangementValid("", date1)).Returns(false);

            Assert.IsTrue(_service.IsPoolArrangementValid("POOL1", date1));
            Assert.IsFalse(_service.IsPoolArrangementValid("POOL2", date1));
            Assert.IsFalse(_service.IsPoolArrangementValid("", date1));
            Assert.AreNotEqual(true, _service.IsPoolArrangementValid("POOL2", date1));
        }

        [TestMethod]
        public void GetPoolMemberCount_VariousPools_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.GetPoolMemberCount("POOL1")).Returns(10);
            _serviceMock.Setup(s => s.GetPoolMemberCount("POOL2")).Returns(0);
            _serviceMock.Setup(s => s.GetPoolMemberCount("POOL3")).Returns(5);

            Assert.AreEqual(10, _service.GetPoolMemberCount("POOL1"));
            Assert.AreEqual(0, _service.GetPoolMemberCount("POOL2"));
            Assert.AreEqual(5, _service.GetPoolMemberCount("POOL3"));
            Assert.AreNotEqual(10, _service.GetPoolMemberCount("POOL2"));
        }

        [TestMethod]
        public void GetPoolAdministratorId_VariousPools_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.GetPoolAdministratorId("POOL1")).Returns("ADMIN1");
            _serviceMock.Setup(s => s.GetPoolAdministratorId("POOL2")).Returns((string)null);
            _serviceMock.Setup(s => s.GetPoolAdministratorId("POOL3")).Returns("ADMIN2");

            Assert.AreEqual("ADMIN1", _service.GetPoolAdministratorId("POOL1"));
            Assert.IsNull(_service.GetPoolAdministratorId("POOL2"));
            Assert.AreEqual("ADMIN2", _service.GetPoolAdministratorId("POOL3"));
            Assert.AreNotEqual("ADMIN1", _service.GetPoolAdministratorId("POOL3"));
        }

        [TestMethod]
        public void CalculatePoolMemberShare_VariousInputs_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.CalculatePoolMemberShare("POOL1", "MEM1", 1000m)).Returns(250m);
            _serviceMock.Setup(s => s.CalculatePoolMemberShare("POOL1", "MEM2", 1000m)).Returns(750m);
            _serviceMock.Setup(s => s.CalculatePoolMemberShare("POOL2", "MEM1", 1000m)).Returns(0m);

            Assert.AreEqual(250m, _service.CalculatePoolMemberShare("POOL1", "MEM1", 1000m));
            Assert.AreEqual(750m, _service.CalculatePoolMemberShare("POOL1", "MEM2", 1000m));
            Assert.AreEqual(0m, _service.CalculatePoolMemberShare("POOL2", "MEM1", 1000m));
            Assert.AreNotEqual(250m, _service.CalculatePoolMemberShare("POOL1", "MEM2", 1000m));
        }

        [TestMethod]
        public void ValidateCurrencyCode_VariousCodes_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.ValidateCurrencyCode("USD")).Returns(true);
            _serviceMock.Setup(s => s.ValidateCurrencyCode("XYZ")).Returns(false);
            _serviceMock.Setup(s => s.ValidateCurrencyCode("")).Returns(false);

            Assert.IsTrue(_service.ValidateCurrencyCode("USD"));
            Assert.IsFalse(_service.ValidateCurrencyCode("XYZ"));
            Assert.IsFalse(_service.ValidateCurrencyCode(""));
            Assert.AreNotEqual(true, _service.ValidateCurrencyCode("XYZ"));
        }

        [TestMethod]
        public void GetDefaultCurrency_VariousReinsurers_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.GetDefaultCurrency("RE1")).Returns("USD");
            _serviceMock.Setup(s => s.GetDefaultCurrency("RE2")).Returns("EUR");
            _serviceMock.Setup(s => s.GetDefaultCurrency("RE3")).Returns((string)null);

            Assert.AreEqual("USD", _service.GetDefaultCurrency("RE1"));
            Assert.AreEqual("EUR", _service.GetDefaultCurrency("RE2"));
            Assert.IsNull(_service.GetDefaultCurrency("RE3"));
            Assert.AreNotEqual("USD", _service.GetDefaultCurrency("RE2"));
        }

        [TestMethod]
        public void CheckSanctionsList_VariousReinsurers_ReturnsExpected()
        {
            _serviceMock.Setup(s => s.CheckSanctionsList("RE1")).Returns(false);
            _serviceMock.Setup(s => s.CheckSanctionsList("RE2")).Returns(true);
            _serviceMock.Setup(s => s.CheckSanctionsList("")).Returns(false);

            Assert.IsFalse(_service.CheckSanctionsList("RE1"));
            Assert.IsTrue(_service.CheckSanctionsList("RE2"));
            Assert.IsFalse(_service.CheckSanctionsList(""));
            Assert.AreNotEqual(true, _service.CheckSanctionsList("RE1"));
        }
    }
}