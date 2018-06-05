using System;

namespace SalaryV2.BL.Schedule
{
    public class WeeklySchedule : IPaymentSchedule
    {
        public bool IsPayday(DateTime date) => date.DayOfWeek == DayOfWeek.Friday;
    }
}