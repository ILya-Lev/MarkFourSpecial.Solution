using SalaryV2.BL.Affiliation;
using SalaryV2.BL.Classification;
using SalaryV2.BL.Schedule;
using System.Collections.Generic;

namespace SalaryV2.BL.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public IPaymentSchedule PaymentSchedule { get; set; }
        public IPaymentClassification PaymentClassification { get; set; }
        public IPaymentMethod PaymentMethod { get; set; }
        public List<IAffiliation> Affiliations { get; set; } = new List<IAffiliation>();
    }
}
