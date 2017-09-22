using ParkingAppApi.ServiceModel.Enum;
using ServiceStack;

namespace ParkingAppApi.ServiceModel.Employee
{
    [Route("/employee/availableparkingbays", Verbs = "Get")]
    public class AbsenceViewRequest
    {
        public SearchDateCriteriaDto searchCriteria { get; set; }
    }
}
