using ServiceStack;

namespace ParkingAppApi.ServiceModel.Employee
{
    [Route("/employee/cancelreservedbay", Verbs = "Post")]
    public class CancelReservedParkingBayRequest
    {
        public EmployeeViewDto employeeViewDto { get; set; }
        public AbsenceViewDto absenceViewDto { get; set; }
    }
}
