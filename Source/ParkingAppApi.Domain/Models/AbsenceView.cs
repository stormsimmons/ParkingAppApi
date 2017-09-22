
namespace ParkingAppApi.Domain.Models
{
    public class AbsenceView
    {
        public int EmployeeId { get; set; }
        public ParkingBay ParkingBaynumber { get; set; }
        public Absence Absence { get; set; }
        public string BayOwner { get; set; }
    }
}
