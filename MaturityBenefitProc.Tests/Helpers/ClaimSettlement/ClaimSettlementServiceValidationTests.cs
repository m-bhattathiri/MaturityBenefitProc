using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.ClaimSettlement;

namespace MaturityBenefitProc.Tests.Helpers.ClaimSettlement
{
    [TestClass]
    public class ClaimSettlementServiceValidationTests
    {
        private ClaimSettlementService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new ClaimSettlementService();
        }

        [TestMethod]
        public void InitiateClaimSettlement_EmptyPolicy_Fails()
        {
            var result = _service.InitiateClaimSettlement("", "CIF001");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void InitiateClaimSettlement_WhitespacePolicy_Fails()
        {
            var result = _service.InitiateClaimSettlement("   ", "CIF001");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void InitiateClaimSettlement_EmptyCif_Fails()
        {
            var result = _service.InitiateClaimSettlement("POL001", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void InitiateClaimSettlement_WhitespaceCif_Fails()
        {
            var result = _service.InitiateClaimSettlement("POL001", "   ");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ValidateClaimSettlement_NullClaim_Fails()
        {
            var result = _service.ValidateClaimSettlement(null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ValidateClaimSettlement_EmptyClaim_Fails()
        {
            var result = _service.ValidateClaimSettlement("");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ApproveSettlement_EmptyClaim_Fails()
        {
            var result = _service.ApproveSettlement("", "Manager");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ApproveSettlement_EmptyApprover_Fails()
        {
            var result = _service.ApproveSettlement("CLM001", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RejectSettlement_EmptyClaim_Fails()
        {
            var result = _service.RejectSettlement("", "reason");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RejectSettlement_EmptyReason_Fails()
        {
            var result = _service.RejectSettlement("CLM001", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ProcessDischargeVoucher_EmptyClaim_Fails()
        {
            var result = _service.ProcessDischargeVoucher("", "Signatory");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ProcessDischargeVoucher_EmptySignatory_Fails()
        {
            var result = _service.ProcessDischargeVoucher("CLM001", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void VerifyNomineeIdentity_NullClaim_Fails()
        {
            var result = _service.VerifyNomineeIdentity(null, "NOM001", "AADHAR123");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void VerifyNomineeIdentity_NullNominee_Fails()
        {
            var result = _service.VerifyNomineeIdentity("CLM001", null, "AADHAR123");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void VerifyNomineeIdentity_NullDocument_Fails()
        {
            var result = _service.VerifyNomineeIdentity("CLM001", "NOM001", null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetSettlementDetails_NullClaim_Fails()
        {
            var result = _service.GetSettlementDetails(null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetSettlementDetails_EmptyClaim_Fails()
        {
            var result = _service.GetSettlementDetails("");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetSettlementDetails_NonExistent_Fails()
        {
            var result = _service.GetSettlementDetails("NONEXISTENT");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Settlement details not found", result.Message);
            Assert.IsNull(result.ReferenceId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EscalateSettlement_EmptyClaim_Fails()
        {
            var result = _service.EscalateSettlement("", "reason");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EscalateSettlement_EmptyReason_Fails()
        {
            var result = _service.EscalateSettlement("CLM001", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SuspendSettlement_EmptyClaim_Fails()
        {
            var result = _service.SuspendSettlement("", "reason");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SuspendSettlement_EmptyReason_Fails()
        {
            var result = _service.SuspendSettlement("CLM001", "");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetSettlementHistory_NullPolicy_ReturnsEmpty()
        {
            var result = _service.GetSettlementHistory(null, DateTime.MinValue, DateTime.MaxValue);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            Assert.IsFalse(result.Any());
            Assert.IsTrue(result is List<ClaimSettlementResult>);
        }

        [TestMethod]
        public void GetSettlementHistory_EmptyPolicy_ReturnsEmpty()
        {
            var result = _service.GetSettlementHistory("", DateTime.MinValue, DateTime.MaxValue);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            Assert.IsFalse(result.Any());
            Assert.IsTrue(result is List<ClaimSettlementResult>);
        }

        [TestMethod]
        public void GetSettlementHistory_UnknownPolicy_ReturnsEmpty()
        {
            var result = _service.GetSettlementHistory("UNKNOWN", DateTime.MinValue, DateTime.MaxValue);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            Assert.IsFalse(result.Any());
            Assert.IsTrue(result is List<ClaimSettlementResult>);
        }
    }
}
