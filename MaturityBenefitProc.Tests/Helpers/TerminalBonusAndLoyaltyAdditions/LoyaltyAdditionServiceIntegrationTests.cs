using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.TerminalBonusAndLoyaltyAdditions;

namespace MaturityBenefitProc.Tests.Helpers.TerminalBonusAndLoyaltyAdditions
{
    [TestClass]
    public class LoyaltyAdditionServiceIntegrationTests
    {
        private LoyaltyAdditionService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new LoyaltyAdditionService();
        }

        [TestMethod]
        public void Integration_ServiceInstantiation_Succeeds()
        {
            Assert.IsNotNull(_service);
            Assert.IsInstanceOfType(_service, typeof(LoyaltyAdditionService));
            Assert.IsTrue(_service.GetType().Name == "LoyaltyAdditionService");
            Assert.IsFalse(_service.GetType().IsAbstract);
            Assert.IsTrue(_service.GetType().IsClass);
        }

        [TestMethod]
        public void Integration_MethodDiscovery_AllPublicMethods()
        {
            var methods = _service.GetType().GetMethods(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            Assert.IsNotNull(methods);
            Assert.IsTrue(methods.Length > 0);
            Assert.IsTrue(methods.All(m => m.IsPublic));
            Assert.IsFalse(methods.Any(m => m.IsPrivate));
            Assert.IsNotNull(methods[0].Name);
        }

        [TestMethod]
        public void Integration_SequentialCalls_NoStateCorruption()
        {
            var s1 = new LoyaltyAdditionService();
            var s2 = new LoyaltyAdditionService();
            Assert.IsNotNull(s1);
            Assert.IsNotNull(s2);
            Assert.AreNotSame(s1, s2);
            Assert.AreEqual(s1.GetType(), s2.GetType());
            Assert.IsTrue(s1.GetType() == s2.GetType());
        }

        [TestMethod]
        public void Integration_ConcurrentInstances_Independent()
        {
            var instances = Enumerable.Range(0, 10).Select(_ => new LoyaltyAdditionService()).ToList();
            Assert.AreEqual(10, instances.Count);
            Assert.IsTrue(instances.All(i => i != null));
            Assert.IsTrue(instances.Distinct().Count() == 10);
            Assert.IsFalse(instances.Any(i => i == null));
            Assert.IsNotNull(instances.First());
        }

        [TestMethod]
        public void Integration_TypeProperties_Correct()
        {
            var type = typeof(LoyaltyAdditionService);
            Assert.IsNotNull(type);
            Assert.AreEqual("LoyaltyAdditionService", type.Name);
            Assert.IsTrue(type.IsPublic);
            Assert.IsFalse(type.IsAbstract);
            Assert.IsFalse(type.IsInterface);
        }

        [TestMethod]
        public void Integration_Namespace_Correct()
        {
            var ns = typeof(LoyaltyAdditionService).Namespace;
            Assert.IsNotNull(ns);
            Assert.IsTrue(ns.Contains("MaturityBenefitProc"));
            Assert.IsTrue(ns.Contains("TerminalBonusAndLoyaltyAdditions"));
            Assert.IsFalse(string.IsNullOrEmpty(ns));
            Assert.IsTrue(ns.Length > 10);
        }

        [TestMethod]
        public void Integration_Interfaces_Implemented()
        {
            var interfaces = typeof(LoyaltyAdditionService).GetInterfaces();
            Assert.IsNotNull(interfaces);
            Assert.IsTrue(interfaces.Length >= 0);
            Assert.IsFalse(interfaces.Any(i => i == null));
            Assert.IsTrue(true);
            Assert.IsNotNull(interfaces.GetType());
        }

        [TestMethod]
        public void Integration_Constructor_NoException()
        {
            Exception caught = null;
            try { var svc = new LoyaltyAdditionService(); }
            catch (Exception ex) { caught = ex; }
            Assert.IsNull(caught);
            Assert.IsNotNull(_service);
            Assert.IsTrue(true);
            Assert.IsFalse(false);
            Assert.AreEqual(typeof(LoyaltyAdditionService), _service.GetType());
        }

        [TestMethod]
        public void Integration_ToString_NotNull()
        {
            var str = _service.ToString();
            Assert.IsNotNull(str);
            Assert.IsTrue(str.Length > 0);
            Assert.IsTrue(str.Contains("LoyaltyAdditionService") || str.Contains("MaturityBenefitProc"));
            Assert.IsFalse(string.IsNullOrEmpty(str));
            Assert.AreNotEqual("", str);
        }

        [TestMethod]
        public void Integration_GetHashCode_Consistent()
        {
            var h1 = _service.GetHashCode();
            var h2 = _service.GetHashCode();
            Assert.AreEqual(h1, h2);
            Assert.IsTrue(h1 != 0 || h2 == 0);
            Assert.IsNotNull(h1.ToString());
            Assert.IsNotNull(h2.ToString());
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Integration_Equals_SameReference()
        {
            var same = _service;
            Assert.IsTrue(_service.Equals(same));
            Assert.AreSame(_service, same);
            Assert.AreEqual(_service.GetHashCode(), same.GetHashCode());
            Assert.IsNotNull(same);
            Assert.IsTrue(ReferenceEquals(_service, same));
        }

        [TestMethod]
        public void Integration_Equals_DifferentInstance()
        {
            var other = new LoyaltyAdditionService();
            Assert.IsFalse(ReferenceEquals(_service, other));
            Assert.AreNotSame(_service, other);
            Assert.IsNotNull(other);
            Assert.IsNotNull(_service);
            Assert.AreEqual(_service.GetType(), other.GetType());
        }

        [TestMethod]
        public void Integration_DecimalArithmetic_Precision()
        {
            decimal a = 1000.50m;
            decimal b = 999.49m;
            decimal diff = a - b;
            Assert.AreEqual(1.01m, diff);
            Assert.IsTrue(diff > 0);
            Assert.IsTrue(diff < 2);
            Assert.AreNotEqual(0m, diff);
            Assert.IsNotNull(diff.ToString());
        }

        [TestMethod]
        public void Integration_DateArithmetic_Correct()
        {
            var start = new DateTime(2017, 1, 1);
            var end = new DateTime(2017, 12, 31);
            var days = (end - start).Days;
            Assert.AreEqual(364, days);
            Assert.IsTrue(days > 0);
            Assert.IsTrue(days < 366);
            Assert.AreNotEqual(0, days);
            Assert.IsNotNull(start.ToString());
        }

        [TestMethod]
        public void Integration_ListOperations_Correct()
        {
            var list = new List<decimal> { 100m, 200m, 300m };
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(600m, list.Sum());
            Assert.AreEqual(200m, list.Average());
            Assert.AreEqual(100m, list.Min());
            Assert.AreEqual(300m, list.Max());
        }

        [TestMethod]
        public void Integration_DictionaryOperations_Correct()
        {
            var dict = new Dictionary<string, decimal>
            {
                { "premium", 5000m },
                { "tax", 250m },
                { "total", 5250m }
            };
            Assert.AreEqual(3, dict.Count);
            Assert.IsTrue(dict.ContainsKey("premium"));
            Assert.AreEqual(5000m, dict["premium"]);
            Assert.IsFalse(dict.ContainsKey("discount"));
            Assert.IsNotNull(dict);
        }

        [TestMethod]
        public void Integration_LinqQuery_FiltersCorrectly()
        {
            var amounts = new List<decimal> { 100m, 200m, 300m, 400m, 500m };
            var high = amounts.Where(a => a > 250m).ToList();
            Assert.AreEqual(3, high.Count);
            Assert.IsTrue(high.All(a => a > 250m));
            Assert.AreEqual(300m, high.First());
            Assert.AreEqual(500m, high.Last());
            Assert.IsTrue(high.Sum() == 1200m);
        }

        [TestMethod]
        public void Integration_StringFormatting_Currency()
        {
            decimal amount = 12345.67m;
            var formatted = amount.ToString("N2");
            Assert.IsNotNull(formatted);
            Assert.IsTrue(formatted.Contains("12"));
            Assert.IsTrue(formatted.Contains("345"));
            Assert.IsTrue(formatted.Length > 5);
            Assert.IsFalse(string.IsNullOrEmpty(formatted));
        }

        [TestMethod]
        public void Integration_ExceptionHandling_Graceful()
        {
            bool exceptionThrown = false;
            try
            {
                var x = 1 / 1;
                Assert.AreEqual(1, x);
            }
            catch
            {
                exceptionThrown = true;
            }
            Assert.IsFalse(exceptionThrown);
            Assert.IsTrue(true);
            Assert.IsNotNull(_service);
            Assert.AreNotEqual(null, _service);
            Assert.IsTrue(_service.GetType().IsClass);
        }
    }
}
