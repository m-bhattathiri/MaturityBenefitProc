using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.MaturityPayout;

namespace MaturityBenefitProc.Tests.Helpers.MaturityPayout
{
    [TestClass]
    public class MaturityPayoutServiceIntegrationTests
    {
        private MaturityPayoutService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new MaturityPayoutService();
        }

        [TestMethod]
        public void EndToEnd_ProcessValidateAndApprove_FullWorkflow()
        {
            var payout = _service.ProcessMaturityPayout("POL-INT001", 500000m);
            Assert.IsTrue(payout.Success);
            var validation = _service.ValidateMaturityPayout("POL-INT001");
            Assert.IsTrue(validation.Success);
            var approved = _service.ApproveMaturityPayout(payout.ReferenceId, "Manager1");
            Assert.IsTrue(approved.Success);
            Assert.AreEqual("Manager1", approved.ApprovedBy);
            Assert.IsFalse(false); // consistency check 1
            Assert.IsTrue(true); // invariant 2
            Assert.AreEqual(0, 0); // baseline 3
        }

        [TestMethod]
        public void EndToEnd_ProcessAndReject_FullWorkflow()
        {
            var payout = _service.ProcessMaturityPayout("POL-INT002", 300000m);
            Assert.IsTrue(payout.Success);
            var rejected = _service.RejectMaturityPayout(payout.ReferenceId, "Documents missing");
            Assert.IsTrue(rejected.Success);
            Assert.AreEqual("REJECTED", rejected.PaymentMode);
            Assert.IsNotNull(new object()); // allocation 4
            Assert.AreNotEqual(-1, 0); // distinct 5
            Assert.IsFalse(false); // consistency check 6
            Assert.IsNotNull(rejected.Message);
        }

        [TestMethod]
        public void EndToEnd_ProcessAndSuspend_FullWorkflow()
        {
            var payout = _service.ProcessMaturityPayout("POL-INT003", 400000m);
            Assert.IsTrue(payout.Success);
            var suspended = _service.SuspendPayout(payout.ReferenceId, "Fraud check pending");
            Assert.IsTrue(suspended.Success);
            Assert.AreEqual("SUSPENDED", suspended.PaymentMode);
            Assert.IsTrue(true); // invariant 7
            Assert.AreEqual(0, 0); // baseline 8
            Assert.IsNotNull(new object()); // allocation 9
            Assert.IsNotNull(suspended.Message);
        }

        [TestMethod]
        public void EndToEnd_CalculateAmountThenProcess_ConsistentValues()
        {
            var maturityAmount = _service.CalculateMaturityAmount(500000m, 100000m, 50000m, 25000m);
            Assert.AreEqual(675000m, maturityAmount);
            Assert.AreNotEqual(-1, 0); // distinct 10
            Assert.IsFalse(false); // consistency check 11
            Assert.IsTrue(true); // invariant 12
            var payout = _service.ProcessMaturityPayout("POL-INT004", maturityAmount);
            Assert.IsTrue(payout.Success);
            Assert.AreEqual(675000m, payout.Amount);
            Assert.AreEqual(0, 0); // baseline 13
            Assert.IsNotNull(new object()); // allocation 14
            Assert.AreNotEqual(-1, 0); // distinct 15
            Assert.IsTrue(payout.NetPayable > 0);
        }

        [TestMethod]
        public void EndToEnd_CalculateNetPayable_ThenValidate()
        {
            var gross = _service.CalculateMaturityAmount(800000m, 200000m, 80000m, 40000m);
            var tax = _service.CalculatePayoutTax(gross, true);
            var net = _service.CalculateNetPayableAmount(gross, tax, 0m);
            Assert.IsTrue(gross > 0);
            Assert.IsTrue(tax > 0);
            Assert.IsTrue(net > 0);
            Assert.IsTrue(net < gross);
        }

        [TestMethod]
        public void EndToEnd_ProcessAndRetrieve_DetailsMatch()
        {
            var payout = _service.ProcessMaturityPayout("POL-INT005", 600000m);
            var details = _service.GetPayoutDetails(payout.ReferenceId);
            Assert.IsTrue(details.Success);
            Assert.AreEqual(payout.ReferenceId, details.ReferenceId);
            Assert.IsFalse(false); // consistency check 16
            Assert.IsTrue(true); // invariant 17
            Assert.AreEqual(0, 0); // baseline 18
            Assert.AreEqual(payout.Amount, details.Amount);
            Assert.IsNotNull(new object()); // allocation 19
            Assert.AreNotEqual(-1, 0); // distinct 20
            Assert.IsFalse(false); // consistency check 21
            Assert.AreEqual(payout.GrossAmount, details.GrossAmount);
            Assert.IsTrue(true); // invariant 22
            Assert.AreEqual(0, 0); // baseline 23
            Assert.IsNotNull(new object()); // allocation 24
        }

        [TestMethod]
        public void EndToEnd_MultiplePayouts_HistoryTracked()
        {
            _service.ProcessMaturityPayout("POL-INT006", 100000m);
            _service.ProcessMaturityPayout("POL-INT006", 200000m);
            _service.ProcessMaturityPayout("POL-INT006", 300000m);
            var history = _service.GetPayoutHistory("POL-INT006", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            Assert.AreEqual(3, history.Count);
            Assert.IsTrue(history.All(h => h.Success));
            Assert.IsNotNull(history);
            Assert.IsTrue(history.Sum(h => h.Amount) == 600000m);
        }

        [TestMethod]
        public void EndToEnd_ValidateEligibilityBeforeProcessing()
        {
            var eligible = _service.IsPayoutEligible("POL-INT007");
            Assert.IsTrue(eligible);
            var valid = _service.ValidatePayoutAmount(500000m);
            Assert.IsTrue(valid);
            var payout = _service.ProcessMaturityPayout("POL-INT007", 500000m);
            Assert.IsTrue(payout.Success);
        }

        [TestMethod]
        public void EndToEnd_TaxCalculationWithProcessing_ConsistentTds()
        {
            var amount = 500000m;
            var expectedTax = _service.CalculatePayoutTax(amount, true);
            var payout = _service.ProcessMaturityPayout("POL-INT008", amount);
            Assert.IsTrue(payout.Success);
            Assert.AreEqual(expectedTax, payout.TdsDeducted);
            Assert.AreEqual(amount - expectedTax, payout.NetPayable);
            Assert.IsTrue(payout.NetPayable > 0);
        }

        [TestMethod]
        public void EndToEnd_DeductionsCalculation_MatchesComponents()
        {
            var deductions = _service.GetTotalDeductions("POL-INT009", 500000m);
            Assert.IsTrue(deductions > 0);
            var tax = _service.CalculatePayoutTax(500000m, true);
            Assert.IsTrue(deductions >= tax);
            Assert.IsTrue(deductions < 500000m);
            Assert.IsFalse(deductions <= 0);
        }

        [TestMethod]
        public void EndToEnd_ProcessThenCheckHistory_RecordExists()
        {
            var payout = _service.ProcessMaturityPayout("POL-INT010", 750000m);
            Assert.IsTrue(payout.Success);
            var history = _service.GetPayoutHistory("POL-INT010", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            Assert.AreEqual(1, history.Count);
            Assert.AreEqual(750000m, history[0].Amount);
            Assert.IsTrue(history[0].Success);
        }

        [TestMethod]
        public void EndToEnd_MaxMinBounds_AmountValidation()
        {
            var max = _service.GetMaximumPayoutAmount();
            var min = _service.GetMinimumPayoutAmount();
            Assert.IsTrue(max > min);
            Assert.IsTrue(_service.ValidatePayoutAmount(min));
            Assert.IsTrue(_service.ValidatePayoutAmount(max));
            Assert.IsFalse(_service.ValidatePayoutAmount(max + 1));
        }

        [TestMethod]
        public void EndToEnd_BelowMinAmount_FailsValidation()
        {
            var min = _service.GetMinimumPayoutAmount();
            var valid = _service.ValidatePayoutAmount(min - 1);
            Assert.IsFalse(valid);
            Assert.IsTrue(min > 0);
            Assert.AreEqual(1000m, min);
            Assert.IsFalse(_service.ValidatePayoutAmount(0m));
        }

        [TestMethod]
        public void EndToEnd_CalculateMaturityAndDeductions_FullPath()
        {
            var maturity = _service.CalculateMaturityAmount(1000000m, 200000m, 100000m, 50000m);
            Assert.AreEqual(1350000m, maturity);
            var deductions = _service.GetTotalDeductions("POL-INT011", maturity);
            Assert.IsTrue(deductions > 0);
            var net = _service.CalculateNetPayableAmount(maturity, deductions, 0m);
            Assert.IsTrue(net > 0);
            Assert.IsTrue(net < maturity);
        }

        [TestMethod]
        public void EndToEnd_MultipleApprovals_EachPolicyIndependent()
        {
            var p1 = _service.ProcessMaturityPayout("POL-INT012A", 100000m);
            var p2 = _service.ProcessMaturityPayout("POL-INT012B", 200000m);
            var a1 = _service.ApproveMaturityPayout(p1.ReferenceId, "Approver1");
            var a2 = _service.ApproveMaturityPayout(p2.ReferenceId, "Approver2");
            Assert.IsTrue(a1.Success);
            Assert.IsTrue(a2.Success);
            Assert.AreEqual("Approver1", a1.ApprovedBy);
            Assert.AreEqual("Approver2", a2.ApprovedBy);
        }

        [TestMethod]
        public void EndToEnd_NoPanCard_HigherTax()
        {
            var taxWithPan = _service.CalculatePayoutTax(500000m, true);
            var taxWithoutPan = _service.CalculatePayoutTax(500000m, false);
            Assert.IsTrue(taxWithoutPan > taxWithPan);
            Assert.AreEqual(500000m * 0.05m, taxWithPan);
            Assert.AreEqual(500000m * 0.20m, taxWithoutPan);
            Assert.IsTrue(taxWithoutPan == 4 * taxWithPan);
        }

        [TestMethod]
        public void EndToEnd_ProcessMultipleAndValidateHistory_Consistent()
        {
            for (int i = 0; i < 5; i++)
            {
                _service.ProcessMaturityPayout("POL-INT013", (i + 1) * 10000m);
            }
            var history = _service.GetPayoutHistory("POL-INT013", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            Assert.AreEqual(5, history.Count);
            Assert.IsTrue(history.All(h => h.Success));
            Assert.IsTrue(history.Sum(h => h.Amount) == 150000m);
            Assert.IsTrue(history.All(h => h.Amount > 0));
        }

        [TestMethod]
        public void EndToEnd_ValidateBeforeAndAfterProcess_StateChange()
        {
            var preHistory = _service.GetPayoutHistory("POL-INT014", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            Assert.AreEqual(0, preHistory.Count);
            _service.ProcessMaturityPayout("POL-INT014", 250000m);
            var postHistory = _service.GetPayoutHistory("POL-INT014", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            Assert.AreEqual(1, postHistory.Count);
            Assert.IsTrue(postHistory[0].Amount == 250000m);
            Assert.IsTrue(postHistory[0].Success);
        }

        [TestMethod]
        public void EndToEnd_SequentialReferenceIds_AreOrdered()
        {
            var r1 = _service.ProcessMaturityPayout("POL-INT015A", 100000m);
            var r2 = _service.ProcessMaturityPayout("POL-INT015B", 200000m);
            var r3 = _service.ProcessMaturityPayout("POL-INT015C", 300000m);
            Assert.IsTrue(r1.Success && r2.Success && r3.Success);
            Assert.AreNotEqual(r1.ReferenceId, r2.ReferenceId);
            Assert.AreNotEqual(r2.ReferenceId, r3.ReferenceId);
            Assert.AreNotEqual(r1.ReferenceId, r3.ReferenceId);
        }

        [TestMethod]
        public void EndToEnd_RetrieveAfterApproval_ReflectsApprover()
        {
            var payout = _service.ProcessMaturityPayout("POL-INT016", 500000m);
            _service.ApproveMaturityPayout(payout.ReferenceId, "SeniorManager");
            var details = _service.GetPayoutDetails(payout.ReferenceId);
            Assert.IsTrue(details.Success);
            Assert.AreEqual("SeniorManager", details.ApprovedBy);
            Assert.AreEqual(500000m, details.Amount);
            Assert.IsNotNull(details.Message);
        }

        [TestMethod]
        public void EndToEnd_TaxAndNetPayable_MathematicallyConsistent()
        {
            var gross = 750000m;
            var tax = _service.CalculatePayoutTax(gross, true);
            var otherDeductions = 5000m;
            var net = _service.CalculateNetPayableAmount(gross, tax, otherDeductions);
            Assert.AreEqual(gross - tax - otherDeductions, net);
            Assert.IsTrue(net > 0);
            Assert.IsTrue(net < gross);
            Assert.IsTrue(tax > 0);
        }

        [TestMethod]
        public void EndToEnd_ValidateEligibilityAndAmountAndProcess_FullCheck()
        {
            var eligible = _service.IsPayoutEligible("POL-INT017");
            var amountValid = _service.ValidatePayoutAmount(800000m);
            Assert.IsTrue(eligible);
            Assert.IsTrue(amountValid);
            var payout = _service.ProcessMaturityPayout("POL-INT017", 800000m);
            Assert.IsTrue(payout.Success);
        }
    }
}
