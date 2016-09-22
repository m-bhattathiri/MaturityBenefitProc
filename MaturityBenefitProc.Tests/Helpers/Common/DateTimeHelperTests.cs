using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityBenefitProc.Helpers.Common;

namespace MaturityBenefitProc.Tests.Helpers.Common
{
    [TestClass]
    public class DateTimeHelperTests
    {
        [TestMethod]
        public void GetFinancialYearStart_DateInJuly_ReturnsAprilSameYear()
        {
            var input = new DateTime(2017, 7, 15);
            var result = DateTimeHelper.GetFinancialYearStart(input);
            Assert.AreEqual(new DateTime(2017, 4, 1), result);
        }

        [TestMethod]
        public void GetFinancialYearStart_DateInJanuary_ReturnsAprilPreviousYear()
        {
            var input = new DateTime(2017, 1, 20);
            var result = DateTimeHelper.GetFinancialYearStart(input);
            Assert.AreEqual(new DateTime(2016, 4, 1), result);
        }

        [TestMethod]
        public void GetFinancialYearStart_DateInApril_ReturnsAprilSameYear()
        {
            var input = new DateTime(2016, 4, 1);
            var result = DateTimeHelper.GetFinancialYearStart(input);
            Assert.AreEqual(new DateTime(2016, 4, 1), result);
        }

        [TestMethod]
        public void GetFinancialYearStart_DateInMarch_ReturnsAprilPreviousYear()
        {
            var input = new DateTime(2016, 3, 31);
            var result = DateTimeHelper.GetFinancialYearStart(input);
            Assert.AreEqual(new DateTime(2015, 4, 1), result);
        }

        [TestMethod]
        public void GetFinancialYearEnd_DateInJuly_ReturnsMarchNextYear()
        {
            var input = new DateTime(2017, 7, 15);
            var result = DateTimeHelper.GetFinancialYearEnd(input);
            Assert.AreEqual(new DateTime(2018, 3, 31), result);
        }

        [TestMethod]
        public void GetFinancialYearEnd_DateInFebruary_ReturnsMarchSameYear()
        {
            var input = new DateTime(2017, 2, 10);
            var result = DateTimeHelper.GetFinancialYearEnd(input);
            Assert.AreEqual(new DateTime(2017, 3, 31), result);
        }

        [TestMethod]
        public void GetFinancialYearEnd_DateInDecember_ReturnsMarchNextYear()
        {
            var input = new DateTime(2016, 12, 25);
            var result = DateTimeHelper.GetFinancialYearEnd(input);
            Assert.AreEqual(new DateTime(2017, 3, 31), result);
        }

        [TestMethod]
        public void GetFinancialYear_DateInOctober_ReturnsCorrectFormat()
        {
            var input = new DateTime(2017, 10, 5);
            var result = DateTimeHelper.GetFinancialYear(input);
            Assert.AreEqual("2017-18", result);
        }

        [TestMethod]
        public void GetFinancialYear_DateInJanuary_ReturnsPreviousYearFormat()
        {
            var input = new DateTime(2017, 1, 5);
            var result = DateTimeHelper.GetFinancialYear(input);
            Assert.AreEqual("2016-17", result);
        }

        [TestMethod]
        public void GetFinancialYear_DateInApril_ReturnsCurrentYearFormat()
        {
            var input = new DateTime(2015, 4, 1);
            var result = DateTimeHelper.GetFinancialYear(input);
            Assert.AreEqual("2015-16", result);
        }

        [TestMethod]
        public void GetCompletedYears_ExactTwoYears_ReturnsTwo()
        {
            var from = new DateTime(2015, 6, 1);
            var to = new DateTime(2017, 6, 1);
            Assert.AreEqual(2, DateTimeHelper.GetCompletedYears(from, to));
        }

        [TestMethod]
        public void GetCompletedYears_OneDayShort_ReturnsOneLess()
        {
            var from = new DateTime(2015, 6, 15);
            var to = new DateTime(2017, 6, 14);
            Assert.AreEqual(1, DateTimeHelper.GetCompletedYears(from, to));
        }

        [TestMethod]
        public void GetCompletedYears_ToBeforeFrom_ReturnsZero()
        {
            var from = new DateTime(2017, 6, 1);
            var to = new DateTime(2015, 6, 1);
            Assert.AreEqual(0, DateTimeHelper.GetCompletedYears(from, to));
        }

        [TestMethod]
        public void GetCompletedYears_SameDate_ReturnsZero()
        {
            var date = new DateTime(2016, 8, 20);
            Assert.AreEqual(0, DateTimeHelper.GetCompletedYears(date, date));
        }

        [TestMethod]
        public void GetCompletedMonths_ThreeFullMonths_ReturnsThree()
        {
            var from = new DateTime(2016, 1, 10);
            var to = new DateTime(2016, 4, 10);
            Assert.AreEqual(3, DateTimeHelper.GetCompletedMonths(from, to));
        }

        [TestMethod]
        public void GetCompletedMonths_OneDayShortOfMonth_ReturnsOneLess()
        {
            var from = new DateTime(2016, 1, 15);
            var to = new DateTime(2016, 4, 14);
            Assert.AreEqual(2, DateTimeHelper.GetCompletedMonths(from, to));
        }

        [TestMethod]
        public void GetCompletedMonths_ToBeforeFrom_ReturnsZero()
        {
            var from = new DateTime(2017, 5, 1);
            var to = new DateTime(2016, 5, 1);
            Assert.AreEqual(0, DateTimeHelper.GetCompletedMonths(from, to));
        }

        [TestMethod]
        public void GetCompletedMonths_AcrossYears_ReturnsCorrectCount()
        {
            var from = new DateTime(2015, 11, 1);
            var to = new DateTime(2016, 3, 1);
            Assert.AreEqual(4, DateTimeHelper.GetCompletedMonths(from, to));
        }

        [TestMethod]
        public void GetDaysBetween_TenDays_ReturnsTen()
        {
            var from = new DateTime(2016, 5, 1);
            var to = new DateTime(2016, 5, 11);
            Assert.AreEqual(10, DateTimeHelper.GetDaysBetween(from, to));
        }

        [TestMethod]
        public void GetDaysBetween_SameDate_ReturnsZero()
        {
            var date = new DateTime(2016, 8, 15);
            Assert.AreEqual(0, DateTimeHelper.GetDaysBetween(date, date));
        }

        [TestMethod]
        public void GetDaysBetween_ToBeforeFrom_ReturnsZero()
        {
            var from = new DateTime(2017, 1, 10);
            var to = new DateTime(2016, 12, 25);
            Assert.AreEqual(0, DateTimeHelper.GetDaysBetween(from, to));
        }

        [TestMethod]
        public void IsLeapYear_2016_ReturnsTrue()
        {
            Assert.IsTrue(DateTimeHelper.IsLeapYear(2016));
        }

        [TestMethod]
        public void IsLeapYear_2000_ReturnsTrue()
        {
            Assert.IsTrue(DateTimeHelper.IsLeapYear(2000));
        }

        [TestMethod]
        public void IsLeapYear_2017_ReturnsFalse()
        {
            Assert.IsFalse(DateTimeHelper.IsLeapYear(2017));
        }

        [TestMethod]
        public void IsLeapYear_1900_ReturnsFalse()
        {
            Assert.IsFalse(DateTimeHelper.IsLeapYear(1900));
        }

        [TestMethod]
        public void GetNextWorkingDay_Friday_ReturnsMonday()
        {
            var friday = new DateTime(2017, 3, 10);
            var result = DateTimeHelper.GetNextWorkingDay(friday);
            Assert.AreEqual(new DateTime(2017, 3, 13), result);
            Assert.AreEqual(DayOfWeek.Monday, result.DayOfWeek);
        }

        [TestMethod]
        public void GetNextWorkingDay_Wednesday_ReturnsThursday()
        {
            var wednesday = new DateTime(2017, 3, 8);
            var result = DateTimeHelper.GetNextWorkingDay(wednesday);
            Assert.AreEqual(new DateTime(2017, 3, 9), result);
        }

        [TestMethod]
        public void GetNextWorkingDay_Saturday_ReturnsMonday()
        {
            var saturday = new DateTime(2017, 3, 11);
            var result = DateTimeHelper.GetNextWorkingDay(saturday);
            Assert.AreEqual(new DateTime(2017, 3, 13), result);
        }

        [TestMethod]
        public void IsWeekend_Saturday_ReturnsTrue()
        {
            var saturday = new DateTime(2017, 3, 11);
            Assert.IsTrue(DateTimeHelper.IsWeekend(saturday));
        }

        [TestMethod]
        public void IsWeekend_Sunday_ReturnsTrue()
        {
            var sunday = new DateTime(2017, 3, 12);
            Assert.IsTrue(DateTimeHelper.IsWeekend(sunday));
        }

        [TestMethod]
        public void IsWeekend_Monday_ReturnsFalse()
        {
            var monday = new DateTime(2017, 3, 13);
            Assert.IsFalse(DateTimeHelper.IsWeekend(monday));
        }

        [TestMethod]
        public void IsWithinRange_DateInRange_ReturnsTrue()
        {
            var date = new DateTime(2016, 6, 15);
            var start = new DateTime(2016, 1, 1);
            var end = new DateTime(2016, 12, 31);
            Assert.IsTrue(DateTimeHelper.IsWithinRange(date, start, end));
        }

        [TestMethod]
        public void IsWithinRange_DateBeforeRange_ReturnsFalse()
        {
            var date = new DateTime(2015, 12, 31);
            var start = new DateTime(2016, 1, 1);
            var end = new DateTime(2016, 12, 31);
            Assert.IsFalse(DateTimeHelper.IsWithinRange(date, start, end));
        }

        [TestMethod]
        public void IsWithinRange_DateAfterRange_ReturnsFalse()
        {
            var date = new DateTime(2017, 1, 1);
            var start = new DateTime(2016, 1, 1);
            var end = new DateTime(2016, 12, 31);
            Assert.IsFalse(DateTimeHelper.IsWithinRange(date, start, end));
        }

        [TestMethod]
        public void IsWithinRange_DateOnStartBoundary_ReturnsTrue()
        {
            var date = new DateTime(2016, 1, 1);
            var start = new DateTime(2016, 1, 1);
            var end = new DateTime(2016, 12, 31);
            Assert.IsTrue(DateTimeHelper.IsWithinRange(date, start, end));
        }

        [TestMethod]
        public void IsWithinRange_DateOnEndBoundary_ReturnsTrue()
        {
            var date = new DateTime(2016, 12, 31);
            var start = new DateTime(2016, 1, 1);
            var end = new DateTime(2016, 12, 31);
            Assert.IsTrue(DateTimeHelper.IsWithinRange(date, start, end));
        }
    }
}
