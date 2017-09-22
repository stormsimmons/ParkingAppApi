using ServiceStack;

namespace ParkingAppApi.ServiceModel.Employee
{
	[Route("/employee/reserveparking", Verbs = "Post")]
	public class ReserveParkngBayRequest
	{
		public EmployeeViewDto employeeViewDto { get; set; }
		public AbsenceViewDto absenceViewDto { get; set; }
	}
}
