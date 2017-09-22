using ParkingAppApi.ServiceInterface;
using System.Collections.Generic;
using ParkingAppApi.Domain.Models;
using ParkingAppApi.Domain.Interfaces;

namespace ParkingAppApi.Domain.Gateways
{
    public class AbsenceServiceGateway : IAbsenceServiceGateway
    {
        private readonly IEmployeeRepository _employeeRepository;

        public AbsenceServiceGateway(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public IList<Absence> GetAbsenceList(int employeeId)
        {
            Employee employee = _employeeRepository.GetOne(employeeId);
            IList<Absence> absenceList = employee.AbsenceList;
            return absenceList;
        }
    }
}
