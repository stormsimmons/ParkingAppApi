using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingAppApi.ServiceModel.Employee
{
    public class AbsenceViewDto
    {
        public int EmployeeId { get; set; }
        public ParkingBayDto ParkingBaynumber { get; set; }
        public AbsenceDto Absence { get; set; }
        public string BayOwner { get; set; }
    }
}
