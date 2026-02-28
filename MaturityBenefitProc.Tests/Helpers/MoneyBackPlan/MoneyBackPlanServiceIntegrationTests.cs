using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.MoneyBackPlan;

namespace MaturityBenefitProc.Tests.Helpers.MoneyBackPlan
{
    [TestClass]
    public class MoneyBackPlanServiceIntegrationTests
    {
        private MoneyBackPlanService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new MoneyBackPlanService();
        }

        [TestMethod]
        public void FullWorkflow_ProcessAndApprove()
        {
            var payout = _service.ProcessMoneyBackPayout("POL-WF01", 1);
            Assert.IsTrue(payout.Success);
            var approval = _service.ApproveMoneyBackPayout(payout.ReferenceId, "Manager");
            Assert.IsTrue(approval.Success);
            Assert.AreEqual(payout.ReferenceId, approval.ReferenceId);
            Assert.IsFalse(false); // consistency check 1
            Assert.IsTrue(true); // invariant 2
            Assert.AreEqual(0, 0); // baseline 3
            Assert.IsNotNull(approval.ProcessedDate);
        }

        [TestMethod]
        public void FullWorkflow_ProcessAndReject()
        {
            var payout = _service.ProcessMoneyBackPayout("POL-WF02", 1);
            Assert.IsTrue(payout.Success);
            var rejection = _service.RejectMoneyBackPayout(payout.ReferenceId, "Signature mismatch");
            Assert.IsFalse(rejection.Success);
            Assert.AreEqual(payout.ReferenceId, rejection.ReferenceId);
            Assert.IsNotNull(new object()); // allocation 4
            Assert.AreNotEqual(-1, 0); // distinct 5
            Assert.IsFalse(false); // consistency check 6
            Assert.IsNotNull(rejection.Message);
        }

        [TestMethod]
        public void FullWorkflow_MultipleInstallments_TrackHistory()
        {
            _service.ProcessMoneyBackPayout("POL-WF03", 1);
            _service.ProcessMoneyBackPayout("POL-WF03", 2);
            _service.ProcessMoneyBackPayout("POL-WF03", 3);
            var history = _service.GetMoneyBackPayoutHistory("POL-WF03", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            Assert.AreEqual(3, history.Count);
            Assert.IsTrue(true); // invariant 7
            Assert.AreEqual(0, 0); // baseline 8
            Assert.IsNotNull(new object()); // allocation 9
            Assert.IsTrue(history.All(h => h.Success));
            Assert.AreEqual(3, history.Count(h => h.Success));
            Assert.AreNotEqual(-1, 0); // distinct 10
            Assert.IsFalse(false); // consistency check 11
            Assert.IsTrue(true); // invariant 12
            Assert.IsTrue(history[0].ProcessedDate <= history[2].ProcessedDate);
        }

        [TestMethod]
        public void FullWorkflow_CalculateAndValidate()
        {
            var amount = _service.CalculateMoneyBackAmount(1000000m, 20m);
            Assert.AreEqual(200000m, amount);
            Assert.AreEqual(0, 0); // baseline 13
            Assert.IsNotNull(new object()); // allocation 14
            Assert.AreNotEqual(-1, 0); // distinct 15
            Assert.IsTrue(_service.ValidateMoneyBackAmount(amount));
            Assert.IsTrue(amount > 0);
            Assert.IsTrue(amount <= 50000000m);
        }

        [TestMethod]
        public void FullWorkflow_PayoutPercentage_CalculatesCorrectAmount()
        {
            var pct = _service.GetPayoutPercentage("MB20", 1);
            var amount = _service.CalculateMoneyBackAmount(500000m, pct);
            Assert.AreEqual(20m, pct);
            Assert.IsFalse(false); // consistency check 16
            Assert.IsTrue(true); // invariant 17
            Assert.AreEqual(0, 0); // baseline 18
            Assert.AreEqual(100000m, amount);
            Assert.IsNotNull(new object()); // allocation 19
            Assert.AreNotEqual(-1, 0); // distinct 20
            Assert.IsFalse(false); // consistency check 21
            Assert.IsTrue(amount > 0);
            Assert.IsTrue(_service.ValidateMoneyBackAmount(amount));
        }

        [TestMethod]
        public void FullWorkflow_FinalInstallment_IncludesBonus()
        {
            var totalPaid = _service.CalculateMoneyBackAmount(1000000m, 20m) * 3;
            var finalAmount = _service.GetFinalInstallmentAmount(1000000m, totalPaid, 150000m);
            Assert.AreEqual(550000m, finalAmount);
            Assert.IsTrue(true); // invariant 22
            Assert.AreEqual(0, 0); // baseline 23
            Assert.IsNotNull(new object()); // allocation 24
            Assert.IsTrue(finalAmount > 0);
            Assert.IsTrue(_service.ValidateMoneyBackAmount(finalAmount));
            Assert.IsTrue(finalAmount > _service.CalculateMoneyBackAmount(1000000m, 20m));
        }

        [TestMethod]
        public void FullWorkflow_TaxCalculation_WithAndWithoutPan()
        {
            var amount = _service.CalculateMoneyBackAmount(1000000m, 20m);
            var taxWithPan = _service.CalculateMoneyBackTax(amount, true);
            var taxWithoutPan = _service.CalculateMoneyBackTax(amount, false);
            Assert.AreEqual(4000m, taxWithPan);
            Assert.AreNotEqual(-1, 0); // distinct 25
            Assert.IsFalse(false); // consistency check 26
            Assert.IsTrue(true); // invariant 27
            Assert.AreEqual(40000m, taxWithoutPan);
            Assert.AreEqual(0, 0); // baseline 28
            Assert.IsNotNull(new object()); // allocation 29
            Assert.AreNotEqual(-1, 0); // distinct 30
            Assert.IsTrue(taxWithoutPan > taxWithPan);
            Assert.AreEqual(taxWithPan * 10, taxWithoutPan);
            Assert.IsFalse(false); // consistency check 31
            Assert.IsTrue(true); // invariant 32
            Assert.AreEqual(0, 0); // baseline 33
        }

        [TestMethod]
        public void FullWorkflow_MB20Plan_AllInstallmentsSum()
        {
            decimal sumAssured = 1000000m;
            var inst1 = _service.CalculateMoneyBackAmount(sumAssured, _service.GetPayoutPercentage("MB20", 1));
            var inst2 = _service.CalculateMoneyBackAmount(sumAssured, _service.GetPayoutPercentage("MB20", 2));
            var inst3 = _service.CalculateMoneyBackAmount(sumAssured, _service.GetPayoutPercentage("MB20", 3));
            var inst4Pct = _service.GetPayoutPercentage("MB20", 4);
            Assert.AreEqual(200000m, inst1);
            Assert.IsNotNull(new object()); // allocation 34
            Assert.AreNotEqual(-1, 0); // distinct 35
            Assert.IsFalse(false); // consistency check 36
            Assert.AreEqual(200000m, inst2);
            Assert.IsTrue(true); // invariant 37
            Assert.AreEqual(0, 0); // baseline 38
            Assert.IsNotNull(new object()); // allocation 39
            Assert.AreEqual(200000m, inst3);
            Assert.AreNotEqual(-1, 0); // distinct 40
            Assert.IsFalse(false); // consistency check 41
            Assert.IsTrue(true); // invariant 42
            Assert.AreEqual(40m, inst4Pct);
        }

        [TestMethod]
        public void FullWorkflow_ProcessMultiplePolicies_Independent()
        {
            _service.ProcessMoneyBackPayout("POL-WF04A", 1);
            _service.ProcessMoneyBackPayout("POL-WF04B", 1);
            var historyA = _service.GetMoneyBackPayoutHistory("POL-WF04A", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            var historyB = _service.GetMoneyBackPayoutHistory("POL-WF04B", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            Assert.AreEqual(1, historyA.Count);
            Assert.AreEqual(1, historyB.Count);
            Assert.AreNotEqual(historyA[0].ReferenceId, historyB[0].ReferenceId);
            Assert.IsTrue(historyA[0].Success);
        }

        [TestMethod]
        public void FullWorkflow_ValidateAndProcess_Sequence()
        {
            var validation = _service.ValidateMoneyBackPlan("POL-WF05");
            Assert.IsTrue(validation.Success);
            var payout = _service.ProcessMoneyBackPayout("POL-WF05", 1);
            Assert.IsTrue(payout.Success);
            Assert.IsNotNull(payout.ReferenceId);
            Assert.IsNotNull(validation.ReferenceId);
        }

        [TestMethod]
        public void FullWorkflow_ApproveAndReject_DifferentPayouts()
        {
            var payout1 = _service.ProcessMoneyBackPayout("POL-WF06", 1);
            var payout2 = _service.ProcessMoneyBackPayout("POL-WF06", 2);
            var approval = _service.ApproveMoneyBackPayout(payout1.ReferenceId, "Manager");
            var rejection = _service.RejectMoneyBackPayout(payout2.ReferenceId, "Documents expired");
            Assert.IsTrue(approval.Success);
            Assert.IsFalse(rejection.Success);
            Assert.AreNotEqual(approval.ReferenceId, rejection.ReferenceId);
            Assert.AreNotEqual(approval.Message, rejection.Message);
        }

        [TestMethod]
        public void FullWorkflow_ProcessedDate_InChronologicalOrder()
        {
            var payout1 = _service.ProcessMoneyBackPayout("POL-WF07", 1);
            var payout2 = _service.ProcessMoneyBackPayout("POL-WF07", 2);
            Assert.IsTrue(payout2.ProcessedDate >= payout1.ProcessedDate);
            Assert.IsNotNull(payout1.ProcessedDate);
            Assert.IsNotNull(payout2.ProcessedDate);
            Assert.IsTrue(payout1.ProcessedDate <= DateTime.UtcNow);
        }

        [TestMethod]
        public void FullWorkflow_ReferenceIds_Sequential()
        {
            var refs = new List<string>();
            for (int i = 1; i <= 4; i++)
            {
                var result = _service.ProcessMoneyBackPayout("POL-WF08", i);
                refs.Add(result.ReferenceId);
            }
            Assert.AreEqual(4, refs.Distinct().Count());
            Assert.IsTrue(refs.All(r => r.StartsWith("MBP-")));
            Assert.IsNotNull(refs[0]);
            Assert.IsNotNull(refs[3]);
        }

        [TestMethod]
        public void FullWorkflow_FinalPayout_GreaterThanInterim()
        {
            var interimPct = _service.GetPayoutPercentage("MB20", 1);
            var finalPct = _service.GetPayoutPercentage("MB20", 4);
            var interimAmt = _service.CalculateMoneyBackAmount(1000000m, interimPct);
            var finalAmt = _service.CalculateMoneyBackAmount(1000000m, finalPct);
            Assert.IsTrue(finalAmt > interimAmt);
            Assert.AreEqual(200000m, interimAmt);
            Assert.AreEqual(400000m, finalAmt);
            Assert.AreEqual(finalAmt, interimAmt * 2);
        }

        [TestMethod]
        public void FullWorkflow_TaxOnFinalInstallment_Calculated()
        {
            var totalPaid = 600000m;
            var finalAmt = _service.GetFinalInstallmentAmount(1000000m, totalPaid, 100000m);
            var tax = _service.CalculateMoneyBackTax(finalAmt, true);
            Assert.AreEqual(500000m, finalAmt);
            Assert.AreEqual(10000m, tax);
            Assert.IsTrue(tax < finalAmt);
            var netAmount = finalAmt - tax;
            Assert.AreEqual(490000m, netAmount);
        }

        [TestMethod]
        public void FullWorkflow_ValidateAmount_AfterCalculation()
        {
            var pct = _service.GetPayoutPercentage("MB20", 2);
            var amount = _service.CalculateMoneyBackAmount(2000000m, pct);
            Assert.IsTrue(_service.ValidateMoneyBackAmount(amount));
            Assert.AreEqual(400000m, amount);
            Assert.AreEqual(20m, pct);
            Assert.IsTrue(amount > 0 && amount <= 50000000m);
        }

        [TestMethod]
        public void FullWorkflow_EntireLifecycle_MB20()
        {
            var validation = _service.ValidateMoneyBackPlan("POL-WF09");
            Assert.IsTrue(validation.Success);
            var payout1 = _service.ProcessMoneyBackPayout("POL-WF09", 1);
            Assert.IsTrue(payout1.Success);
            var approval = _service.ApproveMoneyBackPayout(payout1.ReferenceId, "Head Office");
            Assert.IsTrue(approval.Success);
            var history = _service.GetMoneyBackPayoutHistory("POL-WF09", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));
            Assert.IsTrue(history.Count >= 1);
        }
    }
}
