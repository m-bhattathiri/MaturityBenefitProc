using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.Common;

namespace MaturityBenefitProc.Tests.Helpers.Common
{
    [TestClass]
    public class CurrencyHelperTests
    {
        [TestMethod]
        public void RoundToNearestPaisa_ThreeDecimalPlaces_RoundsToTwo()
        {
            Assert.AreEqual(10.57m, CurrencyHelper.RoundToNearestPaisa(10.565m));
        }

        [TestMethod]
        public void RoundToNearestPaisa_ExactTwoDecimals_ReturnsSame()
        {
            Assert.AreEqual(25.50m, CurrencyHelper.RoundToNearestPaisa(25.50m));
        }

        [TestMethod]
        public void RoundToNearestPaisa_MidpointRoundsAway_RoundsUp()
        {
            Assert.AreEqual(10.55m, CurrencyHelper.RoundToNearestPaisa(10.545m));
        }

        [TestMethod]
        public void RoundToNearestPaisa_ZeroAmount_ReturnsZero()
        {
            Assert.AreEqual(0m, CurrencyHelper.RoundToNearestPaisa(0m));
        }

        [TestMethod]
        public void RoundToNearestPaisa_NegativeAmount_RoundsCorrectly()
        {
            Assert.AreEqual(-15.68m, CurrencyHelper.RoundToNearestPaisa(-15.675m));
        }

        [TestMethod]
        public void NumberToWords_ZeroAmount_ReturnsZero()
        {
            var result = CurrencyHelper.NumberToWords(0m);
            Assert.AreEqual("Zero", result);
        }

        [TestMethod]
        public void NumberToWords_OneRupee_ReturnsOneRupees()
        {
            var result = CurrencyHelper.NumberToWords(1m);
            Assert.AreEqual("One Rupees", result);
        }

        [TestMethod]
        public void NumberToWords_HundredRupees_ReturnsCorrectWords()
        {
            var result = CurrencyHelper.NumberToWords(100m);
            Assert.AreEqual("One Hundred Rupees", result);
        }

        [TestMethod]
        public void NumberToWords_ThousandRupees_ReturnsCorrectWords()
        {
            var result = CurrencyHelper.NumberToWords(1000m);
            Assert.AreEqual("One Thousand Rupees", result);
        }

        [TestMethod]
        public void NumberToWords_LakhAmount_ContainsLakh()
        {
            var result = CurrencyHelper.NumberToWords(100000m);
            Assert.IsTrue(result.Contains("Lakh"));
            Assert.IsTrue(result.Contains("Rupees"));
        }

        [TestMethod]
        public void NumberToWords_CroreAmount_ContainsCrore()
        {
            var result = CurrencyHelper.NumberToWords(10000000m);
            Assert.IsTrue(result.Contains("Crore"));
        }

        [TestMethod]
        public void NumberToWords_WithPaise_ContainsPaise()
        {
            var result = CurrencyHelper.NumberToWords(50.75m);
            Assert.IsTrue(result.Contains("Fifty Rupees"));
            Assert.IsTrue(result.Contains("Paise"));
            Assert.IsTrue(result.Contains("Seventy Five"));
        }

        [TestMethod]
        public void NumberToWords_NegativeAmount_ContainsMinus()
        {
            var result = CurrencyHelper.NumberToWords(-100m);
            Assert.IsTrue(result.StartsWith("Minus"));
        }

        [TestMethod]
        public void NumberToWords_TwentyOne_ReturnsCorrectWords()
        {
            var result = CurrencyHelper.NumberToWords(21m);
            Assert.AreEqual("Twenty One Rupees", result);
        }

        [TestMethod]
        public void ApplyPercentage_TenPercentOfThousand_ReturnsHundred()
        {
            Assert.AreEqual(100m, CurrencyHelper.ApplyPercentage(1000m, 10m));
        }

        [TestMethod]
        public void ApplyPercentage_FiftyPercent_ReturnsHalf()
        {
            Assert.AreEqual(500m, CurrencyHelper.ApplyPercentage(1000m, 50m));
        }

        [TestMethod]
        public void ApplyPercentage_ZeroPercent_ReturnsZero()
        {
            Assert.AreEqual(0m, CurrencyHelper.ApplyPercentage(5000m, 0m));
        }

        [TestMethod]
        public void ApplyPercentage_FractionalResult_RoundsToTwoDecimals()
        {
            var result = CurrencyHelper.ApplyPercentage(1000m, 33.33m);
            Assert.AreEqual(333.30m, result);
        }

        [TestMethod]
        public void CalculateCompoundInterest_StandardCase_ReturnsCorrectAmount()
        {
            var result = CurrencyHelper.CalculateCompoundInterest(10000m, 10m, 2);
            Assert.AreEqual(2100m, result);
        }

        [TestMethod]
        public void CalculateCompoundInterest_OneYear_MatchesSimpleInterest()
        {
            var result = CurrencyHelper.CalculateCompoundInterest(10000m, 8m, 1);
            Assert.AreEqual(800m, result);
        }

        [TestMethod]
        public void CalculateCompoundInterest_ZeroPrincipal_ReturnsZero()
        {
            Assert.AreEqual(0m, CurrencyHelper.CalculateCompoundInterest(0m, 10m, 5));
        }

        [TestMethod]
        public void CalculateCompoundInterest_ZeroRate_ReturnsZero()
        {
            Assert.AreEqual(0m, CurrencyHelper.CalculateCompoundInterest(10000m, 0m, 5));
        }

        [TestMethod]
        public void CalculateCompoundInterest_ZeroYears_ReturnsZero()
        {
            Assert.AreEqual(0m, CurrencyHelper.CalculateCompoundInterest(10000m, 10m, 0));
        }

        [TestMethod]
        public void CalculateCompoundInterest_NegativePrincipal_ReturnsZero()
        {
            Assert.AreEqual(0m, CurrencyHelper.CalculateCompoundInterest(-5000m, 10m, 3));
        }

        [TestMethod]
        public void CalculateSimpleInterest_StandardCase_ReturnsCorrectAmount()
        {
            var result = CurrencyHelper.CalculateSimpleInterest(100000m, 10m, 365);
            Assert.AreEqual(10000m, result);
        }

        [TestMethod]
        public void CalculateSimpleInterest_HalfYear_ReturnsHalfYearInterest()
        {
            var result = CurrencyHelper.CalculateSimpleInterest(100000m, 10m, 182);
            var expected = CurrencyHelper.RoundToNearestPaisa(100000m * 10m * 182m / (100m * 365m));
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CalculateSimpleInterest_ZeroPrincipal_ReturnsZero()
        {
            Assert.AreEqual(0m, CurrencyHelper.CalculateSimpleInterest(0m, 10m, 365));
        }

        [TestMethod]
        public void CalculateSimpleInterest_ZeroRate_ReturnsZero()
        {
            Assert.AreEqual(0m, CurrencyHelper.CalculateSimpleInterest(10000m, 0m, 365));
        }

        [TestMethod]
        public void CalculateSimpleInterest_ZeroDays_ReturnsZero()
        {
            Assert.AreEqual(0m, CurrencyHelper.CalculateSimpleInterest(10000m, 10m, 0));
        }

        [TestMethod]
        public void ConvertAnnualToMonthly_TwelvePercent_ReturnsOnePercent()
        {
            Assert.AreEqual(1m, CurrencyHelper.ConvertAnnualToMonthly(12m));
        }

        [TestMethod]
        public void ConvertAnnualToMonthly_ZeroRate_ReturnsZero()
        {
            Assert.AreEqual(0m, CurrencyHelper.ConvertAnnualToMonthly(0m));
        }

        [TestMethod]
        public void ConvertAnnualToMonthly_EightPercent_ReturnsRoundedValue()
        {
            var result = CurrencyHelper.ConvertAnnualToMonthly(8m);
            Assert.AreEqual(0.67m, result);
        }

        [TestMethod]
        public void ConvertAnnualToMonthly_TenPercent_ReturnsRoundedValue()
        {
            var result = CurrencyHelper.ConvertAnnualToMonthly(10m);
            Assert.AreEqual(0.83m, result);
        }
    }
}
