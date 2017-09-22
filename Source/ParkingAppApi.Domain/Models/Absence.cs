using System;

namespace ParkingAppApi.Domain.Models
{
	public class Absence
	{
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EmployeeView ReservedBy { get; set; }

        public virtual bool IsValidFor(DateTime searchPeriodStart, DateTime searchPeriodEnd)
        {
            if ((searchPeriodStart <= EndDate && StartDate <= searchPeriodEnd) || (searchPeriodStart <= EndDate && StartDate == searchPeriodStart))
            {
                return true;
            }
            return false;
        }
    }
}


