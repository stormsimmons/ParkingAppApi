using System;

namespace ParkingAppApi.ServiceModel.Employee
{
	public class AbsenceDto
	{
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public EmployeeViewDto ReservedBy { get; set; }
	}
}