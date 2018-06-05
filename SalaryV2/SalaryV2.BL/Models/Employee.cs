using System.Collections.Generic;
using SalaryV2.BL.Affiliation;
using SalaryV2.BL.Classification;
using SalaryV2.BL.Schedule;

namespace SalaryV2.BL.Models
{
    public class Employee
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public IPaymentSchedule PaymentSchedule { get; set; }
        public IPaymentClassification PaymentClassification { get; set; }
        public IPaymentMethod PaymentMethod { get; set; }
        public List<IAffiliation> Affiliations { get; set; } = new List<IAffiliation>();
    }
}
