using System;

namespace SalaryV2.BL.Classification
{
    public class SalariedClassification : IPaymentClassification
    {
        private readonly decimal _salary;

        public SalariedClassification(decimal salary)
        {
            _salary = salary;
        }

        public virtual decimal Pay(DateTime aDate, DateTime? lastPayday) => _salary;
    }
}