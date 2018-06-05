using System;
using System.Collections.Generic;
using System.Linq;
using SalaryV2.BL.Models;

namespace SalaryV2.BL.Classification
{
    public class HourlyClassification : IPaymentClassification
    {
        private readonly int _employeeId;
        private readonly decimal _hourlyRate;
        private readonly decimal _overtimeFactor;
        private readonly decimal _weekendFactor;
        private readonly IEmployeeRelatedProvider<TimeCard> _timeCardProvider;
        private int _hoursInStandardDay = 8;

        public HourlyClassification(int employeeId, decimal hourlyRate, decimal overtimeFactor, decimal weekendFactor, IEmployeeRelatedProvider<TimeCard> timeCardProvider)
        {
            _employeeId = employeeId;
            _hourlyRate = hourlyRate;
            _overtimeFactor = overtimeFactor;
            _weekendFactor = weekendFactor;
            _timeCardProvider = timeCardProvider;
        }

        public decimal Pay(DateTime aDate, DateTime? lastPayday)
        {
            var timeCards = GetTimeCardsForLastWeek(aDate, lastPayday);
            var totalHours = timeCards.Sum(tc => NormalizeHours(tc));
            return totalHours * _hourlyRate;
        }

        private ICollection<TimeCard> GetTimeCardsForLastWeek(DateTime aDate, DateTime? lastPayday)
        {
            return _timeCardProvider.GetForEmployee(_employeeId, lastPayday, aDate);
        }

        private decimal NormalizeHours(TimeCard timeCard)
        {
            if (timeCard.Hours < 0 || timeCard.Hours > 24)
            {
                throw new ArgumentOutOfRangeException(nameof(timeCard.Hours), timeCard.Hours,
                    "Employee cannot work either less than 0 or more than 24 hours.");
            }

            //todo: IsHoliday(timeCard.At)
            if (timeCard.At.DayOfWeek == DayOfWeek.Saturday || timeCard.At.DayOfWeek == DayOfWeek.Sunday)
                return (decimal)timeCard.Hours * _weekendFactor;

            //todo: what about penalty?
            if (timeCard.Hours <= _hoursInStandardDay)
                return (decimal)timeCard.Hours;

            return _hoursInStandardDay + (decimal)(timeCard.Hours - _hoursInStandardDay) * _overtimeFactor;
        }
    }
}