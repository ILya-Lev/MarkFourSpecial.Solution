﻿namespace Salary.Models
{
    public class Employee : IEntityWithId
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal MajorRate { get; set; }
        public decimal? MinorRate { get; set; }
        //public DeliveryMethod DeliveryMethod { get; set; }
        public decimal? TradeUnionCharge { get; set; }
    }

    //public class DeliveryMethod
    //{
    //    public TYPE Type { get; set; }
    //}
}
