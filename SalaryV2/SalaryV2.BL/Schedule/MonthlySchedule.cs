using System;

namespace SalaryV2.BL.Schedule
{
    public class MonthlySchedule : IPaymentSchedule
    {
        public bool IsPayday(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return false;
            if (!date.IsTheSameMonth(1))
                return true;

            //todo: what about holidays?
            if (date.DayOfWeek == DayOfWeek.Friday && (!date.IsTheSameMonth(2) || !date.IsTheSameMonth(3)))
                return true;

            return false;
        }
    }
}