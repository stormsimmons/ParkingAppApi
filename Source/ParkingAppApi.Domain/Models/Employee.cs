using System.Collections.Generic;

namespace ParkingAppApi.Domain.Models
{
	public class Employee
	{
		public int Id { get; set; }
        public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public IList<Absence> AbsenceList { get; set; }
		public ParkingBay ParkingBay { get; set; }
	}
}
