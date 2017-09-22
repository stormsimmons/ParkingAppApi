using System;

namespace ParkingAppApi.Domain.ModelDto
{
	public class AbsenceModelDto
    {
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public EmployeeViewModelDto ReservedBy { get; set; }
    }
}


