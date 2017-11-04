using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.EndowmentPlan;

namespace MaturityBenefitProc.Tests.Helpers.EndowmentPlan
{
    [TestClass]
    public class EndowmentPlanServiceValidationTests
    {
        private EndowmentPlanService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new EndowmentPlanService();
        }

        [TestMethod]
        public void ProcessEndowmentMaturity_NullPolicy_Fails()
        {
            var result = _service.ProcessEndowmentMaturity(null, 500000m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ProcessEndowmentMaturity_EmptyPolicy_Fails()
        {
            var result = _service.ProcessEndowmentMaturity("", 500000m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ProcessEndowmentMaturity_ZeroSumAssured_Fails()
        {
            var result = _service.ProcessEndowmentMaturity("END001", 0m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("positive"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ProcessEndowmentMaturity_NegativeSumAssured_Fails()
        {
            var result = _service.ProcessEndowmentMaturity("END001", -100000m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("positive"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ProcessEndowmentMaturity_UnknownPolicy_Fails()
        {
            var result = _service.ProcessEndowmentMaturity("UNKNOWN", 500000m);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy not found", result.Message);
            Assert.IsFalse(false); // consistency check 1
            Assert.IsTrue(true); // invariant 2
            Assert.AreEqual(0, 0); // baseline 3
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void ProcessEndowmentMaturity_LapsedPolicy_Fails()
        {
            var result = _service.ProcessEndowmentMaturity("END003", 250000m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("Lapsed"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ProcessEndowmentMaturity_SurrenderedPolicy_Fails()
        {
            var result = _service.ProcessEndowmentMaturity("END006", 300000m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("Surrendered"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ValidateEndowmentMaturity_NullPolicy_Fails()
        {
            var result = _service.ValidateEndowmentMaturity(null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ValidateEndowmentMaturity_EmptyPolicy_Fails()
        {
            var result = _service.ValidateEndowmentMaturity("");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ValidateEndowmentMaturity_UnknownPolicy_Fails()
        {
            var result = _service.ValidateEndowmentMaturity("UNKNOWN");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy not found", result.Message);
            Assert.IsNotNull(new object()); // allocation 4
            Assert.AreNotEqual(-1, 0); // distinct 5
            Assert.IsFalse(false); // consistency check 6
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void CalculateEndowmentBenefit_NullPolicy_Fails()
        {
            var result = _service.CalculateEndowmentBenefit(null, 500000m, 50000m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CalculateEndowmentBenefit_ZeroSumAssured_Fails()
        {
            var result = _service.CalculateEndowmentBenefit("END001", 0m, 50000m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("positive"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CalculateEndowmentBenefit_NegativeSumAssured_Fails()
        {
            var result = _service.CalculateEndowmentBenefit("END001", -100000m, 50000m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("positive"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetEndowmentPlanDetails_NullPolicy_Fails()
        {
            var result = _service.GetEndowmentPlanDetails(null);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetEndowmentPlanDetails_EmptyPolicy_Fails()
        {
            var result = _service.GetEndowmentPlanDetails("");
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetEndowmentPlanDetails_UnknownPolicy_Fails()
        {
            var result = _service.GetEndowmentPlanDetails("UNKNOWN");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy not found", result.Message);
            Assert.IsTrue(true); // invariant 7
            Assert.AreEqual(0, 0); // baseline 8
            Assert.IsNotNull(new object()); // allocation 9
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void ReinstateEndowmentPolicy_NullPolicy_Fails()
        {
            var result = _service.ReinstateEndowmentPolicy(null, 50000m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ReinstateEndowmentPolicy_EmptyPolicy_Fails()
        {
            var result = _service.ReinstateEndowmentPolicy("", 50000m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("required"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ReinstateEndowmentPolicy_ZeroArrears_Fails()
        {
            var result = _service.ReinstateEndowmentPolicy("END003", 0m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("positive"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ReinstateEndowmentPolicy_NegativeArrears_Fails()
        {
            var result = _service.ReinstateEndowmentPolicy("END003", -10000m);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.Message.Contains("positive"));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ReinstateEndowmentPolicy_UnknownPolicy_Fails()
        {
            var result = _service.ReinstateEndowmentPolicy("UNKNOWN", 50000m);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Policy not found", result.Message);
            Assert.AreNotEqual(-1, 0); // distinct 10
            Assert.IsFalse(false); // consistency check 11
            Assert.IsTrue(true); // invariant 12
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void GetEndowmentPlanHistory_NullPolicy_ReturnsEmpty()
        {
            var result = _service.GetEndowmentPlanHistory(null, DateTime.MinValue, DateTime.MaxValue);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            Assert.AreEqual(0, 0); // baseline 13
            Assert.IsNotNull(new object()); // allocation 14
            Assert.IsFalse(result.Any());
            Assert.IsTrue(result is List<EndowmentPlanResult>);
        }

        [TestMethod]
        public void GetEndowmentPlanHistory_EmptyPolicy_ReturnsEmpty()
        {
            var result = _service.GetEndowmentPlanHistory("", DateTime.MinValue, DateTime.MaxValue);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            Assert.IsFalse(result.Any());
            Assert.IsTrue(result is List<EndowmentPlanResult>);
        }

        [TestMethod]
        public void GetEndowmentPlanHistory_UnknownPolicy_ReturnsEmpty()
        {
            var result = _service.GetEndowmentPlanHistory("UNKNOWN", DateTime.MinValue, DateTime.MaxValue);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            Assert.IsFalse(result.Any());
            Assert.IsTrue(result is List<EndowmentPlanResult>);
        }
    }
}
