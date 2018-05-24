using System;

namespace Salary.Models
{
    public class TimeCard : EntityForEmployee
    {
        private float _hours;

        public float Hours
        {
            get => _hours;
            set
            {
                if (value < 1 || value > 24)
                {
                    throw new ArgumentOutOfRangeException(nameof(Hours),
                          $"Time card hours cannot exceed 24 hours and be less than 1 hour. Actual value is {value}");
                }
                _hours = value;
            }
        }

        public TimeCard(int employeeId) : base(employeeId) { }
    }
}