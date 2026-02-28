using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.InternationalAndNriProcessing;

namespace MaturityBenefitProc.Tests.Helpers.InternationalAndNriProcessing
{
    [TestClass]
    public class NreAccountValidationServiceTests
    {
        private INreAccountValidationService _service;

        [TestInitialize]
        public void Setup()
        {
            // Assuming a concrete class named NreAccountValidationService implements the interface
            _service = new NreAccountValidationService();
        }

        [TestMethod]
        public void ValidateNreAccountFormat_ValidInputs_ReturnsTrue()
        {
            var result1 = _service.ValidateNreAccountFormat("1234567890", "SBIN0001234");
            var result2 = _service.ValidateNreAccountFormat("0987654321", "HDFC0001234");
            var result3 = _service.ValidateNreAccountFormat("1111222233", "ICIC0001234");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ValidateNreAccountFormat_InvalidInputs_ReturnsFalse()
        {
            var result1 = _service.ValidateNreAccountFormat("", "SBIN0001234");
            var result2 = _service.ValidateNreAccountFormat("1234567890", "");
            var result3 = _service.ValidateNreAccountFormat(null, null);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsAccountRepatriable_ValidAccountId_ReturnsTrue()
        {
            var result1 = _service.IsAccountRepatriable("NRE123");
            var result2 = _service.IsAccountRepatriable("FCNR456");
            var result3 = _service.IsAccountRepatriable("NRE999");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void IsAccountRepatriable_InvalidAccountId_ReturnsFalse()
        {
            var result1 = _service.IsAccountRepatriable("NRO123");
            var result2 = _service.IsAccountRepatriable("");
            var result3 = _service.IsAccountRepatriable(null);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateRepatriationLimit_ValidInputs_ReturnsExpectedLimit()
        {
            var result1 = _service.CalculateRepatriationLimit("CUST001", new DateTime(2023, 4, 1));
            var result2 = _service.CalculateRepatriationLimit("CUST002", new DateTime(2022, 4, 1));
            var result3 = _service.CalculateRepatriationLimit("CUST003", new DateTime(2021, 4, 1));

            Assert.AreEqual(1000000m, result1);
            Assert.AreEqual(1000000m, result2);
            Assert.AreEqual(1000000m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateRepatriationLimit_InvalidInputs_ReturnsZero()
        {
            var result1 = _service.CalculateRepatriationLimit("", new DateTime(2023, 4, 1));
            var result2 = _service.CalculateRepatriationLimit(null, new DateTime(2022, 4, 1));
            var result3 = _service.CalculateRepatriationLimit("CUST001", DateTime.MinValue);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetCurrentExchangeRate_ValidCurrency_ReturnsRate()
        {
            var result1 = _service.GetCurrentExchangeRate("USD");
            var result2 = _service.GetCurrentExchangeRate("EUR");
            var result3 = _service.GetCurrentExchangeRate("GBP");

            Assert.AreEqual(82.5, result1);
            Assert.AreEqual(89.2, result2);
            Assert.AreEqual(103.4, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetCurrentExchangeRate_InvalidCurrency_ReturnsZero()
        {
            var result1 = _service.GetCurrentExchangeRate("XYZ");
            var result2 = _service.GetCurrentExchangeRate("");
            var result3 = _service.GetCurrentExchangeRate(null);

            Assert.AreEqual(0.0, result1);
            Assert.AreEqual(0.0, result2);
            Assert.AreEqual(0.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetDaysSinceLastKycUpdate_ValidCustomer_ReturnsDays()
        {
            var result1 = _service.GetDaysSinceLastKycUpdate("CUST001");
            var result2 = _service.GetDaysSinceLastKycUpdate("CUST002");
            var result3 = _service.GetDaysSinceLastKycUpdate("CUST003");

            Assert.AreEqual(150, result1);
            Assert.AreEqual(150, result2);
            Assert.AreEqual(150, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetDaysSinceLastKycUpdate_InvalidCustomer_ReturnsMinusOne()
        {
            var result1 = _service.GetDaysSinceLastKycUpdate("");
            var result2 = _service.GetDaysSinceLastKycUpdate(null);
            var result3 = _service.GetDaysSinceLastKycUpdate("INVALID");

            Assert.AreEqual(-1, result1);
            Assert.AreEqual(-1, result2);
            Assert.AreEqual(-1, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetAuthorizedDealerCode_ValidIfsc_ReturnsCode()
        {
            var result1 = _service.GetAuthorizedDealerCode("SBIN0001234");
            var result2 = _service.GetAuthorizedDealerCode("HDFC0001234");
            var result3 = _service.GetAuthorizedDealerCode("ICIC0001234");

            Assert.AreEqual("AD12345", result1);
            Assert.AreEqual("AD12345", result2);
            Assert.AreEqual("AD12345", result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetAuthorizedDealerCode_InvalidIfsc_ReturnsNull()
        {
            var result1 = _service.GetAuthorizedDealerCode("");
            var result2 = _service.GetAuthorizedDealerCode(null);
            var result3 = _service.GetAuthorizedDealerCode("INVALID");

            Assert.IsNull(result1);
            Assert.IsNull(result2);
            Assert.IsNull(result3);
            Assert.AreNotEqual("AD12345", result1);
        }

        [TestMethod]
        public void VerifyFemaCompliance_ValidInputs_ReturnsTrue()
        {
            var result1 = _service.VerifyFemaCompliance("CUST001", 500000m);
            var result2 = _service.VerifyFemaCompliance("CUST002", 100000m);
            var result3 = _service.VerifyFemaCompliance("CUST003", 999999m);

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void VerifyFemaCompliance_InvalidInputs_ReturnsFalse()
        {
            var result1 = _service.VerifyFemaCompliance("CUST001", 1500000m);
            var result2 = _service.VerifyFemaCompliance("", 500000m);
            var result3 = _service.VerifyFemaCompliance(null, 500000m);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ComputeTdsOnNroPayout_ValidInputs_ReturnsCorrectTds()
        {
            var result1 = _service.ComputeTdsOnNroPayout(100000m, 10.0);
            var result2 = _service.ComputeTdsOnNroPayout(50000m, 15.0);
            var result3 = _service.ComputeTdsOnNroPayout(200000m, 5.0);

            Assert.AreEqual(10000m, result1);
            Assert.AreEqual(7500m, result2);
            Assert.AreEqual(10000m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ComputeTdsOnNroPayout_InvalidInputs_ReturnsZero()
        {
            var result1 = _service.ComputeTdsOnNroPayout(0m, 10.0);
            var result2 = _service.ComputeTdsOnNroPayout(-10000m, 15.0);
            var result3 = _service.ComputeTdsOnNroPayout(100000m, -5.0);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetApplicableDtaaRate_ValidCountry_ReturnsRate()
        {
            var result1 = _service.GetApplicableDtaaRate("USA");
            var result2 = _service.GetApplicableDtaaRate("UK");
            var result3 = _service.GetApplicableDtaaRate("UAE");

            Assert.AreEqual(15.0, result1);
            Assert.AreEqual(15.0, result2);
            Assert.AreEqual(15.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetApplicableDtaaRate_InvalidCountry_ReturnsDefaultRate()
        {
            var result1 = _service.GetApplicableDtaaRate("");
            var result2 = _service.GetApplicableDtaaRate(null);
            var result3 = _service.GetApplicableDtaaRate("UNKNOWN");

            Assert.AreEqual(30.0, result1);
            Assert.AreEqual(30.0, result2);
            Assert.AreEqual(30.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CountActiveNreAccounts_ValidCustomer_ReturnsCount()
        {
            var result1 = _service.CountActiveNreAccounts("CUST001");
            var result2 = _service.CountActiveNreAccounts("CUST002");
            var result3 = _service.CountActiveNreAccounts("CUST003");

            Assert.AreEqual(2, result1);
            Assert.AreEqual(2, result2);
            Assert.AreEqual(2, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CountActiveNreAccounts_InvalidCustomer_ReturnsZero()
        {
            var result1 = _service.CountActiveNreAccounts("");
            var result2 = _service.CountActiveNreAccounts(null);
            var result3 = _service.CountActiveNreAccounts("INVALID");

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GenerateForm15CbReference_ValidInputs_ReturnsReference()
        {
            var result1 = _service.GenerateForm15CbReference("CUST001", 500000m);
            var result2 = _service.GenerateForm15CbReference("CUST002", 100000m);
            var result3 = _service.GenerateForm15CbReference("CUST003", 999999m);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.IsTrue(result1.StartsWith("15CB-"));
        }

        [TestMethod]
        public void GenerateForm15CbReference_InvalidInputs_ReturnsNull()
        {
            var result1 = _service.GenerateForm15CbReference("", 500000m);
            var result2 = _service.GenerateForm15CbReference(null, 100000m);
            var result3 = _service.GenerateForm15CbReference("CUST003", -100m);

            Assert.IsNull(result1);
            Assert.IsNull(result2);
            Assert.IsNull(result3);
            Assert.AreNotEqual("15CB-", result1);
        }

        [TestMethod]
        public void CheckOciPioStatusValid_ValidInputs_ReturnsTrue()
        {
            var result1 = _service.CheckOciPioStatusValid("OCI123", DateTime.Now.AddDays(10));
            var result2 = _service.CheckOciPioStatusValid("PIO456", DateTime.Now.AddMonths(6));
            var result3 = _service.CheckOciPioStatusValid("OCI789", DateTime.Now.AddYears(1));

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CheckOciPioStatusValid_InvalidInputs_ReturnsFalse()
        {
            var result1 = _service.CheckOciPioStatusValid("OCI123", DateTime.Now.AddDays(-10));
            var result2 = _service.CheckOciPioStatusValid("", DateTime.Now.AddDays(10));
            var result3 = _service.CheckOciPioStatusValid(null, DateTime.Now.AddDays(10));

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTotalRemittedAmountYtd_ValidInputs_ReturnsAmount()
        {
            var result1 = _service.GetTotalRemittedAmountYtd("CUST001", 2023);
            var result2 = _service.GetTotalRemittedAmountYtd("CUST002", 2022);
            var result3 = _service.GetTotalRemittedAmountYtd("CUST003", 2021);

            Assert.AreEqual(250000m, result1);
            Assert.AreEqual(250000m, result2);
            Assert.AreEqual(250000m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetTotalRemittedAmountYtd_InvalidInputs_ReturnsZero()
        {
            var result1 = _service.GetTotalRemittedAmountYtd("", 2023);
            var result2 = _service.GetTotalRemittedAmountYtd(null, 2022);
            var result3 = _service.GetTotalRemittedAmountYtd("CUST001", -1);

            Assert.AreEqual(0m, result1);
            Assert.AreEqual(0m, result2);
            Assert.AreEqual(0m, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateNroWithholdingTaxPercentage_ValidInputs_ReturnsPercentage()
        {
            var result1 = _service.CalculateNroWithholdingTaxPercentage(true, "US");
            var result2 = _service.CalculateNroWithholdingTaxPercentage(true, "UK");
            var result3 = _service.CalculateNroWithholdingTaxPercentage(false, "US");

            Assert.AreEqual(15.0, result1);
            Assert.AreEqual(15.0, result2);
            Assert.AreEqual(30.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void CalculateNroWithholdingTaxPercentage_InvalidInputs_ReturnsDefault()
        {
            var result1 = _service.CalculateNroWithholdingTaxPercentage(false, "");
            var result2 = _service.CalculateNroWithholdingTaxPercentage(false, null);
            var result3 = _service.CalculateNroWithholdingTaxPercentage(true, "UNKNOWN");

            Assert.AreEqual(30.0, result1);
            Assert.AreEqual(30.0, result2);
            Assert.AreEqual(30.0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetPendingFemaDeclarationsCount_ValidCustomer_ReturnsCount()
        {
            var result1 = _service.GetPendingFemaDeclarationsCount("CUST001");
            var result2 = _service.GetPendingFemaDeclarationsCount("CUST002");
            var result3 = _service.GetPendingFemaDeclarationsCount("CUST003");

            Assert.AreEqual(1, result1);
            Assert.AreEqual(1, result2);
            Assert.AreEqual(1, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void GetPendingFemaDeclarationsCount_InvalidCustomer_ReturnsZero()
        {
            var result1 = _service.GetPendingFemaDeclarationsCount("");
            var result2 = _service.GetPendingFemaDeclarationsCount(null);
            var result3 = _service.GetPendingFemaDeclarationsCount("INVALID");

            Assert.AreEqual(0, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(0, result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ResolveSwiftCode_ValidInputs_ReturnsSwiftCode()
        {
            var result1 = _service.ResolveSwiftCode("SBI", "MUMBAI");
            var result2 = _service.ResolveSwiftCode("HDFC", "DELHI");
            var result3 = _service.ResolveSwiftCode("ICICI", "CHENNAI");

            Assert.AreEqual("SBININBB", result1);
            Assert.AreEqual("SBININBB", result2);
            Assert.AreEqual("SBININBB", result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ResolveSwiftCode_InvalidInputs_ReturnsNull()
        {
            var result1 = _service.ResolveSwiftCode("", "MUMBAI");
            var result2 = _service.ResolveSwiftCode("SBI", "");
            var result3 = _service.ResolveSwiftCode(null, null);

            Assert.IsNull(result1);
            Assert.IsNull(result2);
            Assert.IsNull(result3);
            Assert.AreNotEqual("SBININBB", result1);
        }

        [TestMethod]
        public void ValidateNroToNreTransferEligibility_ValidInputs_ReturnsTrue()
        {
            var result1 = _service.ValidateNroToNreTransferEligibility("NRO123", "NRE456", 50000m);
            var result2 = _service.ValidateNroToNreTransferEligibility("NRO789", "NRE012", 100000m);
            var result3 = _service.ValidateNroToNreTransferEligibility("NRO345", "NRE678", 999999m);

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsNotNull(result1);
        }

        [TestMethod]
        public void ValidateNroToNreTransferEligibility_InvalidInputs_ReturnsFalse()
        {
            var result1 = _service.ValidateNroToNreTransferEligibility("NRO123", "NRE456", 1500000m);
            var result2 = _service.ValidateNroToNreTransferEligibility("", "NRE456", 50000m);
            var result3 = _service.ValidateNroToNreTransferEligibility("NRO123", null, 50000m);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsNotNull(result1);
        }
    }
}