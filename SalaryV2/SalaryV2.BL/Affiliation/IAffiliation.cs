using System;

namespace SalaryV2.BL.Affiliation
{
    public interface IAffiliation
    {
        decimal GetCharge(DateTime since, DateTime until);
    }
}