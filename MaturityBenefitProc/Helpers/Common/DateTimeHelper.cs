using System;

namespace MaturityBenefitProc.Helpers.Common
{
    public static class DateTimeHelper
    {
        public static DateTime GetFinancialYearStart(DateTime date)
        {
            if (date.Month >= 4)
            {
                return new DateTime(date.Year, 4, 1);
            }
            return new DateTime(date.Year - 1, 4, 1);
        }

        public static DateTime GetFinancialYearEnd(DateTime date)
        {
            if (date.Month >= 4)
            {
                return new DateTime(date.Year + 1, 3, 31);
            }
            return new DateTime(date.Year, 3, 31);
        }

        public static string GetFinancialYear(DateTime date)
        {
            int startYear;
            if (date.Month >= 4)
            {
                startYear = date.Year;
            }
            else
            {
                startYear = date.Year - 1;
            }
            int endYear = startYear + 1;
            return string.Format("{0}-{1}", startYear, endYear.ToString().Substring(2));
        }

        public static int GetCompletedYears(DateTime from, DateTime to)
        {
            if (to < from) return 0;
            int years = to.Year - from.Year;
            if (to.Month < from.Month || (to.Month == from.Month && to.Day < from.Day))
            {
                years--;
            }
            return Math.Max(0, years);
        }

        public static int GetCompletedMonths(DateTime from, DateTime to)
        {
            if (to < from) return 0;
            int months = (to.Year - from.Year) * 12 + (to.Month - from.Month);
            if (to.Day < from.Day)
            {
                months--;
            }
            return Math.Max(0, months);
        }

        public static int GetDaysBetween(DateTime from, DateTime to)
        {
            if (to < from) return 0;
            return (int)(to.Date - from.Date).TotalDays;
        }

        public static bool IsLeapYear(int year)
        {
            return DateTime.IsLeapYear(year);
        }

        public static DateTime GetNextWorkingDay(DateTime date)
        {
            DateTime next = date.AddDays(1);
            while (IsWeekend(next))
            {
                next = next.AddDays(1);
            }
            return next;
        }

        public static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        public static bool IsWithinRange(DateTime date, DateTime rangeStart, DateTime rangeEnd)
        {
            return date.Date >= rangeStart.Date && date.Date <= rangeEnd.Date;
        }
    }
}
