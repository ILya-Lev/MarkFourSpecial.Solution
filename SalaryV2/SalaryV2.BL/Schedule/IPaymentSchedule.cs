using System;

namespace SalaryV2.BL.Schedule
{
    public interface IPaymentSchedule
    {
        bool IsPayday(DateTime date);
    }
}