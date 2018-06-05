using System;

namespace SalaryV2.BL.Classification
{
    public interface IPaymentClassification
    {
        decimal Pay(DateTime aDate, DateTime? lastPayday);
    }
}