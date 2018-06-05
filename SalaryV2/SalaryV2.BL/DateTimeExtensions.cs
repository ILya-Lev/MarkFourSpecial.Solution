using System;

namespace SalaryV2.BL
{
    public static class DateTimeExtensions
    {
        public static bool IsTheSameMonth(this DateTime date, int afterDays)
        {
            var anotherDate = date.AddDays(afterDays);
            return anotherDate.Month == date.Month && anotherDate.Year == date.Year;
        }
    }
}