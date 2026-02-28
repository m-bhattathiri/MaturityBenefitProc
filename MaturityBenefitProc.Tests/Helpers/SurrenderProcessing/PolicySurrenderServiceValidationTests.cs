using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.SurrenderProcessing;

namespace MaturityBenefitProc.Tests.Helpers.SurrenderProcessing
{
    [TestClass]
    public class PolicySurrenderServiceValidationTests
    {
        private PolicySurrenderService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new PolicySurrenderService();
        }

        [TestMethod]
        public void ValidatePolicyEligibility_InvalidPolicyIds_ThrowsArgumentException()
        {
            DateTime validDate = DateTime.Now;
            Assert.ThrowsException<ArgumentException>(() => _service.ValidatePolicyEligibility(null, validDate));
            Assert.ThrowsException<ArgumentException>(() => _service.ValidatePolicyEligibility(string.Empty, validDate));
            Assert.ThrowsException<ArgumentException>(() => _service.ValidatePolicyEligibility("   ", validDate));
            Assert.ThrowsException<ArgumentException>(() => _service.ValidatePolicyEligibility("\t", validDate));
        }

        [TestMethod]
        public void ValidatePolicyEligibility_SequentialCalls_ReturnsExpectedResults()
        {
            DateTime date1 = new DateTime(2023, 1, 1);
            DateTime date2 = new DateTime(2023, 6, 1);
            
            bool result1 = _service.ValidatePolicyEligibility("POL-1001", date1);
            bool result2 = _service.ValidatePolicyEligibility("POL-1002", date2);
            bool result3 = _service.ValidatePolicyEligibility("POL-1001", date2);

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreEqual(result1, _service.ValidatePolicyEligibility("POL-1001", date1));
        }

        [TestMethod]
        public void CalculateBaseSurrenderValue_InvalidPolicyIds_ThrowsArgumentException()
        {
            DateTime effectiveDate = DateTime.Today;
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateBaseSurrenderValue(null, effectiveDate));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateBaseSurrenderValue(string.Empty, effectiveDate));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateBaseSurrenderValue(" ", effectiveDate));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateBaseSurrenderValue("\n", effectiveDate));
        }

        [TestMethod]
        public void CalculateMarketValueAdjustment_NegativeBaseValue_ThrowsArgumentOutOfRangeException()
        {
            string policyId = "POL-2001";
            double marketRate = 0.05;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.CalculateMarketValueAdjustment(policyId, -1m, marketRate));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.CalculateMarketValueAdjustment(policyId, -1000m, marketRate));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.CalculateMarketValueAdjustment(policyId, -0.01m, marketRate));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateMarketValueAdjustment(string.Empty, 100m, marketRate));
        }

        [TestMethod]
        public void CalculateMarketValueAdjustment_BoundaryMarketRates_ProcessesSuccessfully()
        {
            string policyId = "POL-2002";
            decimal baseValue = 10000m;

            decimal resultZero = _service.CalculateMarketValueAdjustment(policyId, baseValue, 0.0);
            decimal resultNegative = _service.CalculateMarketValueAdjustment(policyId, baseValue, -0.02);
            decimal resultHigh = _service.CalculateMarketValueAdjustment(policyId, baseValue, 0.15);

            Assert.IsNotNull(resultZero);
            Assert.IsNotNull(resultNegative);
            Assert.IsNotNull(resultHigh);
            Assert.AreNotEqual(-1m, resultZero);
        }

        [TestMethod]
        public void CalculateSurrenderCharge_NegativeYearsInForce_ThrowsArgumentOutOfRangeException()
        {
            string policyId = "POL-3001";
            decimal baseValue = 5000m;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.CalculateSurrenderCharge(policyId, baseValue, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.CalculateSurrenderCharge(policyId, baseValue, -10));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.CalculateSurrenderCharge(policyId, -500m, 5));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateSurrenderCharge(null, baseValue, 5));
        }

        [TestMethod]
        public void CalculateTerminalBonus_InvalidInputs_ThrowsExceptions()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateTerminalBonus(null, 1000m));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateTerminalBonus(string.Empty, 1000m));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.CalculateTerminalBonus("POL-4001", -50m));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.CalculateTerminalBonus("POL-4001", -0.01m));
        }

        [TestMethod]
        public void CalculateUnearnedPremiumRefund_InvalidPolicyIds_ThrowsArgumentException()
        {
            DateTime surrenderDate = DateTime.Now;
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateUnearnedPremiumRefund(null, surrenderDate));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateUnearnedPremiumRefund(string.Empty, surrenderDate));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateUnearnedPremiumRefund("   ", surrenderDate));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateUnearnedPremiumRefund("\r\n", surrenderDate));
        }

        [TestMethod]
        public void CalculateOutstandingLoanBalance_SequentialCalls_ReturnsConsistentResults()
        {
            string policyId = "POL-5001";
            DateTime calcDate = new DateTime(2023, 10, 1);

            decimal balance1 = _service.CalculateOutstandingLoanBalance(policyId, calcDate);
            decimal balance2 = _service.CalculateOutstandingLoanBalance(policyId, calcDate);
            decimal balance3 = _service.CalculateOutstandingLoanBalance(policyId, calcDate.AddDays(1));

            Assert.IsTrue(balance1 >= 0);
            Assert.AreEqual(balance1, balance2);
            Assert.IsNotNull(balance3);
            Assert.IsTrue(balance3 >= 0);
        }

        [TestMethod]
        public void CalculateLoanInterestAccrued_InvalidPolicyIds_ThrowsArgumentException()
        {
            DateTime calcDate = DateTime.Today;
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateLoanInterestAccrued(null, calcDate));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateLoanInterestAccrued(string.Empty, calcDate));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateLoanInterestAccrued(" ", calcDate));
            Assert.ThrowsException<ArgumentException>(() => _service.CalculateLoanInterestAccrued("\t", calcDate));
        }

        [TestMethod]
        public void CalculateGrossSurrenderValue_ValidInputs_ReturnsPositiveOrZero()
        {
            DateTime effectiveDate = DateTime.Today;
            decimal val1 = _service.CalculateGrossSurrenderValue("POL-6001", effectiveDate);
            decimal val2 = _service.CalculateGrossSurrenderValue("POL-6002", effectiveDate.AddMonths(1));
            decimal val3 = _service.CalculateGrossSurrenderValue("POL-6003", effectiveDate.AddYears(1));

            Assert.IsTrue(val1 >= 0);
            Assert.IsTrue(val2 >= 0);
            Assert.IsTrue(val3 >= 0);
            Assert.IsNotNull(val1);
        }

        [TestMethod]
        public void CalculateNetSurrenderValue_ValidInputs_ReturnsPositiveOrZero()
        {
            DateTime effectiveDate = DateTime.Today;
            decimal net1 = _service.CalculateNetSurrenderValue("POL-7001", effectiveDate);
            decimal net2 = _service.CalculateNetSurrenderValue("POL-7002", effectiveDate.AddDays(15));
            decimal net3 = _service.CalculateNetSurrenderValue("POL-7003", effectiveDate.AddDays(30));

            Assert.IsTrue(net1 >= 0);
            Assert.IsTrue(net2 >= 0);
            Assert.IsTrue(net3 >= 0);
            Assert.IsNotNull(net2);
        }

        [TestMethod]
        public void GetCurrentSurrenderChargeRate_NegativeYear_ThrowsArgumentOutOfRangeException()
        {
            string policyId = "POL-8001";
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.GetCurrentSurrenderChargeRate(policyId, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.GetCurrentSurrenderChargeRate(policyId, -5));
            Assert.ThrowsException<ArgumentException>(() => _service.GetCurrentSurrenderChargeRate(string.Empty, 5));
            Assert.ThrowsException<ArgumentException>(() => _service.GetCurrentSurrenderChargeRate(null, 5));
        }

        [TestMethod]
        public void GetMarketValueAdjustmentFactor_InvalidPolicyIds_ThrowsArgumentException()
        {
            DateTime calcDate = DateTime.Now;
            Assert.ThrowsException<ArgumentException>(() => _service.GetMarketValueAdjustmentFactor(null, calcDate));
            Assert.ThrowsException<ArgumentException>(() => _service.GetMarketValueAdjustmentFactor(string.Empty, calcDate));
            Assert.ThrowsException<ArgumentException>(() => _service.GetMarketValueAdjustmentFactor("   ", calcDate));
            Assert.ThrowsException<ArgumentException>(() => _service.GetMarketValueAdjustmentFactor("\n", calcDate));
        }

        [TestMethod]
        public void GetTerminalBonusRate_NegativeYears_ThrowsArgumentOutOfRangeException()
        {
            string policyId = "POL-9001";
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.GetTerminalBonusRate(policyId, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.GetTerminalBonusRate(policyId, -20));
            Assert.ThrowsException<ArgumentException>(() => _service.GetTerminalBonusRate(null, 10));
            Assert.ThrowsException<ArgumentException>(() => _service.GetTerminalBonusRate(string.Empty, 10));
        }

        [TestMethod]
        public void GetTaxWithholdingRate_InvalidStateCode_ThrowsArgumentException()
        {
            string policyId = "POL-10001";
            Assert.ThrowsException<ArgumentException>(() => _service.GetTaxWithholdingRate(policyId, null));
            Assert.ThrowsException<ArgumentException>(() => _service.GetTaxWithholdingRate(policyId, string.Empty));
            Assert.ThrowsException<ArgumentException>(() => _service.GetTaxWithholdingRate(policyId, " "));
            Assert.ThrowsException<ArgumentException>(() => _service.GetTaxWithholdingRate(null, "NY"));
        }

        [TestMethod]
        public void IsPolicyInForce_InvalidPolicyIds_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.IsPolicyInForce(null));
            Assert.ThrowsException<ArgumentException>(() => _service.IsPolicyInForce(string.Empty));
            Assert.ThrowsException<ArgumentException>(() => _service.IsPolicyInForce("   "));
            Assert.ThrowsException<ArgumentException>(() => _service.IsPolicyInForce("\t"));
        }

        [TestMethod]
        public void HasOutstandingLoans_InvalidPolicyIds_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.HasOutstandingLoans(null));
            Assert.ThrowsException<ArgumentException>(() => _service.HasOutstandingLoans(string.Empty));
            Assert.ThrowsException<ArgumentException>(() => _service.HasOutstandingLoans(" "));
            Assert.ThrowsException<ArgumentException>(() => _service.HasOutstandingLoans("\r\n"));
        }

        [TestMethod]
        public void IsWithinFreeLookPeriod_SequentialCalls_ReturnsExpected()
        {
            DateTime requestDate = DateTime.Today;
            bool result1 = _service.IsWithinFreeLookPeriod("POL-11001", requestDate);
            bool result2 = _service.IsWithinFreeLookPeriod("POL-11002", requestDate.AddDays(-10));
            bool result3 = _service.IsWithinFreeLookPeriod("POL-11003", requestDate.AddDays(10));

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
            Assert.AreEqual(result1, _service.IsWithinFreeLookPeriod("POL-11001", requestDate));
        }

        [TestMethod]
        public void RequiresSpousalConsent_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.RequiresSpousalConsent(null, "CA"));
            Assert.ThrowsException<ArgumentException>(() => _service.RequiresSpousalConsent(string.Empty, "CA"));
            Assert.ThrowsException<ArgumentException>(() => _service.RequiresSpousalConsent("POL-12001", null));
            Assert.ThrowsException<ArgumentException>(() => _service.RequiresSpousalConsent("POL-12001", string.Empty));
        }

        [TestMethod]
        public void ValidateSignatureRequirements_InvalidInputs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.ValidateSignatureRequirements(null, "DOC-123"));
            Assert.ThrowsException<ArgumentException>(() => _service.ValidateSignatureRequirements(string.Empty, "DOC-123"));
            Assert.ThrowsException<ArgumentException>(() => _service.ValidateSignatureRequirements("POL-13001", null));
            Assert.ThrowsException<ArgumentException>(() => _service.ValidateSignatureRequirements("POL-13001", string.Empty));
        }

        [TestMethod]
        public void CheckAntiMoneyLaunderingStatus_NegativeValue_ThrowsArgumentOutOfRangeException()
        {
            string policyId = "POL-14001";
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.CheckAntiMoneyLaunderingStatus(policyId, -1m));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _service.CheckAntiMoneyLaunderingStatus(policyId, -10000m));
            Assert.ThrowsException<ArgumentException>(() => _service.CheckAntiMoneyLaunderingStatus(null, 5000m));
            Assert.ThrowsException<ArgumentException>(() => _service.CheckAntiMoneyLaunderingStatus(string.Empty, 5000m));
        }

        [TestMethod]
        public void InitiateSurrenderWorkflow_InvalidRequestedBy_ThrowsArgumentException()
        {
            string policyId = "POL-15001";
            Assert.ThrowsException<ArgumentException>(() => _service.InitiateSurrenderWorkflow(policyId, null));
            Assert.ThrowsException<ArgumentException>(() => _service.InitiateSurrenderWorkflow(policyId, string.Empty));
            Assert.ThrowsException<ArgumentException>(() => _service.InitiateSurrenderWorkflow(null, "User123"));
            Assert.ThrowsException<ArgumentException>(() => _service.InitiateSurrenderWorkflow(string.Empty, "User123"));
        }

        [TestMethod]
        public void ApproveSurrenderRequest_InvalidWorkflowId_ThrowsArgumentException()
        {
            string approverId = "APP-999";
            Assert.ThrowsException<ArgumentException>(() => _service.ApproveSurrenderRequest(null, approverId));
            Assert.ThrowsException<ArgumentException>(() => _service.ApproveSurrenderRequest(string.Empty, approverId));
            Assert.ThrowsException<ArgumentException>(() => _service.ApproveSurrenderRequest("WF-1001", null));
            Assert.ThrowsException<ArgumentException>(() => _service.ApproveSurrenderRequest("WF-1001", string.Empty));
        }

        [TestMethod]
        public void RejectSurrenderRequest_InvalidInputs_ThrowsArgumentException()
        {
            string workflowId = "WF-2001";
            string reasonCode = "RC-01";
            string rejectedBy = "USR-555";

            Assert.ThrowsException<ArgumentException>(() => _service.RejectSurrenderRequest(null, reasonCode, rejectedBy));
            Assert.ThrowsException<ArgumentException>(() => _service.RejectSurrenderRequest(workflowId, null, rejectedBy));
            Assert.ThrowsException<ArgumentException>(() => _service.RejectSurrenderRequest(workflowId, reasonCode, null));
            Assert.ThrowsException<ArgumentException>(() => _service.RejectSurrenderRequest(string.Empty, reasonCode, rejectedBy));
        }

        [TestMethod]
        public void SuspendSurrenderWorkflow_InvalidInputs_ThrowsArgumentException()
        {
            string workflowId = "WF-3001";
            string reasonCode = "SUSP-01";

            Assert.ThrowsException<ArgumentException>(() => _service.SuspendSurrenderWorkflow(null, reasonCode));
            Assert.ThrowsException<ArgumentException>(() => _service.SuspendSurrenderWorkflow(string.Empty, reasonCode));
            Assert.ThrowsException<ArgumentException>(() => _service.SuspendSurrenderWorkflow(workflowId, null));
            Assert.ThrowsException<ArgumentException>(() => _service.SuspendSurrenderWorkflow(workflowId, string.Empty));
        }

        [TestMethod]
        public void GetSurrenderStatus_InvalidWorkflowId_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => _service.GetSurrenderStatus(null));
            Assert.ThrowsException<ArgumentException>(() => _service.GetSurrenderStatus(string.Empty));
            Assert.ThrowsException<ArgumentException>(() => _service.GetSurrenderStatus("   "));
            Assert.ThrowsException<ArgumentException>(() => _service.GetSurrenderStatus("\n"));
        }

        [TestMethod]
        public void FinalizeSurrenderTransaction_InvalidWorkflowId_ThrowsArgumentException()
        {
            DateTime processingDate = DateTime.Now;
            Assert.ThrowsException<ArgumentException>(() => _service.FinalizeSurrenderTransaction(null, processingDate));
            Assert.ThrowsException<ArgumentException>(() => _service.FinalizeSurrenderTransaction(string.Empty, processingDate));
            Assert.ThrowsException<ArgumentException>(() => _service.FinalizeSurrenderTransaction("   ", processingDate));
            Assert.ThrowsException<ArgumentException>(() => _service.FinalizeSurrenderTransaction("\t", processingDate));
        }
    }
}