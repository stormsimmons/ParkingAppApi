using ServiceStack;

namespace ParkingAppApi.ServiceModel.Employee
{
	[Route("/employee/{id}", Verbs = "Get")]
	public class EmployeeRequestOne
	{
		public int Id { get; set; }
	}
}
