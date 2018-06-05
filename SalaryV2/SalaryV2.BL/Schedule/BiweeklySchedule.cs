using System;

namespace SalaryV2.BL.Schedule
{
    public class BiweeklySchedule : IPaymentSchedule
    {
        private const int WeekAgo = -7;

        public bool IsPayday(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Friday
                && date.IsTheSameMonth(2 * WeekAgo);
        }
    }
}