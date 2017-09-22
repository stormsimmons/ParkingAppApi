using System.Collections.Generic;

namespace ParkingAppApi.ServiceModel.Employee
{
	public class EmployeeDto
	{
		public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public IList<AbsenceDto> AbsenceList { get; set; }
		public ParkingBayDto ParkingBay { get; set; }
	}
}
