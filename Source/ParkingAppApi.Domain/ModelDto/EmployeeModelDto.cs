using System.Collections.Generic;

namespace ParkingAppApi.Domain.ModelDto
{
	public class EmployeeModelDto
    {
		public int Id { get; set; }
        public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public IList<AbsenceModelDto> AbsenceList { get; set; }
		public ParkingBayModelDto ParkingBay { get; set; }
	}
}
